using System;
using System.Collections.Generic;
using System.Linq;
using OneFinalClick.Services.Injection;
using UnityEngine;

namespace OneFinalClick.Services
{
    public class ServiceCollection : IServiceResolver
    {
        public bool IsStarted => _isStarted;

        private readonly IReadOnlyList<IService> _managedServices;
        private readonly IReadOnlyDictionary<Type, object> _registeredServices;
        private bool _isStarted = false;
        private IServiceResolver _outerScopeResolver;

        public ServiceCollection(IReadOnlyList<IService> managedServices, IReadOnlyDictionary<Type, object> registeredServices)
        {
            _managedServices = managedServices.ToList();
            _registeredServices = new Dictionary<Type, object>(registeredServices);
        }

        public bool TryGet(Type type, out object service)
        {
            if (_registeredServices.TryGetValue(type, out service) == true)
            {
                return true;
            }

            if (_outerScopeResolver != null)
            {
                return _outerScopeResolver.TryGet(type, out service);
            }

            return false;
        }
        
        public void StartServices(ServiceCollection outerScopeServiceResolver = null)
        {
            _outerScopeResolver = outerScopeServiceResolver;
            IServiceResolver serviceResolver = outerScopeServiceResolver != null ? this.CreateResolverWithFallback(outerScopeServiceResolver) : this;
            InjectServices(serviceResolver);
            
            Debug.Assert(_isStarted == false, "Services already started");

            _isStarted = true;
            
            foreach (IService service in _managedServices)
            {
                try
                {
                    service.OnServiceStart();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
        
        private void InjectServices(IServiceResolver serviceResolver)
        {
            Debug.Log("Injecting services...");
            Debug.Assert(serviceResolver != null, "Service resolver not started");

            var services =  _registeredServices.Values.Distinct();

            foreach (var service in services)
            {
                try
                {
                    ServiceInjection.TryInject(serviceResolver, service);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            
            Debug.Log("Injecting services. Completed..");
        }
        
        public void UpdateServices()
        {
            Debug.Assert(_isStarted == true, "Services not started");
            
            foreach (IService service in _managedServices)
            {
                try
                {
                    service.OnServiceUpdate();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
        
        public void StopServices()
        {
            if (_isStarted == false)
            {
                return;
            }

            _isStarted = false;
            
            foreach (IService service in _managedServices)
            {
                try
                {
                    service.OnServiceStop();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}
