using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OneFinalClick.Services
{
    public class ServicesCollectionBuilder
    {
        private readonly Dictionary<Type, object> _registeredServices = new();
        private readonly List<IService> _managedServices = new();
        
        public ServiceCollection Build()
        {
            return new ServiceCollection(_managedServices, _registeredServices);
        }

        public void RegisterGameObject(GameObject gameObject)
        {
            IService[] services = gameObject.GetComponentsInChildren<IService>(true);

            foreach (IService service in services)
            {
                RegisterManaged(service);
            }
            
            IServiceRegisterer[] allComponents = gameObject.GetComponents<IServiceRegisterer>();
            foreach (IServiceRegisterer component in allComponents)
            {
                component.RegisterServices(this);
            }
        }

        [UsedImplicitly]
        public void Register(object service, params Type[] types)
        {
            foreach (var t in types)
            {
                if (t.IsInstanceOfType(service) == false)
                    throw new ArgumentException($"Service must be assignable to {t}", nameof(types));

                _registeredServices.Add(t, service);
            }

            if (service is IService managedService)
            {
                RegisterManaged(managedService);
            }
        }

        private void RegisterManaged(IService service)
        {
            if (_managedServices.Contains(service) == false)
            {
                _managedServices.Add(service);
            }
        }
        
        [UsedImplicitly]
        public void Register<TI, T>() where T : TI, new()
        {
            Register<TI, T>(new T());
        }

        [UsedImplicitly]
        public void Register<TI, T>(T service) where T : TI
        {
            // ReSharper disable once HeapView.PossibleBoxingAllocation
            Register(service, typeof(TI), typeof(T));
        }
        
        [UsedImplicitly]
        public void Register<T>(T service)
        {
            // ReSharper disable once HeapView.PossibleBoxingAllocation
            Register(service, typeof(T));
        }
        
                
        [UsedImplicitly]
        public void Register<T>() where T : new()
        {
            // ReSharper disable once HeapView.PossibleBoxingAllocation
            Register(new T(), typeof(T));
        }

        public void RegisterSceneServices(Scene scene)
        {
            foreach (GameObject go in scene.GetRootGameObjects())
            {
                RegisterGameObject(go);
            }
        }

        public bool TryGet(Type type, out object instance)
        {
            return _registeredServices.TryGetValue(type, out instance);
        }
    }
}
