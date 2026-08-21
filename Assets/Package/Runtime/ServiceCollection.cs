using System;
using System.Collections.Generic;
using System.Linq;
using FinalClick.Services.Injection;
using UnityEngine;

namespace FinalClick.Services
{
    public class ServiceCollection : IServiceResolver
    {
        public bool IsStarted => _isStarted;

        private readonly IReadOnlyList<IService> _managedServices;
        private readonly IReadOnlyDictionary<Type, object> _registeredServices;
        private bool _isStarted = false;

        public ServiceCollection(IReadOnlyList<IService> managedServices, IReadOnlyDictionary<Type, object> registeredServices)
        {
            _managedServices = managedServices.ToList();
            _registeredServices = new Dictionary<Type, object>(registeredServices);
        }

        public bool TryGet(Type type, out object service)
        {
            return _registeredServices.TryGetValue(type, out service);
        }
        
        public void StartServices(ServiceCollection outerScopeServices = null)
        {
            InjectServices(outerScopeServices);
            
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
        
        private void InjectServices(IServiceResolver serviceResolver = null)
        {
            Debug.Log("Injecting services...");

            var services =  _registeredServices.Values.Distinct();

            serviceResolver = serviceResolver != null ? this.CreateResolverWithFallback(serviceResolver) : this;
            
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
