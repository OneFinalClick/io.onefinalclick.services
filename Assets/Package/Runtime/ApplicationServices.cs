using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace FinalClick.Services
{
    public static class ApplicationServices
    {
        private static ServiceCollection _serviceCollection;
        private static bool IsStarted => _serviceCollection != null && _serviceCollection.IsStarted;
        internal static ServiceCollection ServiceCollection => _serviceCollection;

        private static readonly List<Action<ServicesCollectionBuilder>> RegisteredBuilderFunctions = new();

        public static void AddApplicationServicesBuilderFunction(Action<ServicesCollectionBuilder> registration)
        {
            Debug.Assert(registration != null, "provided function is null.");
            RegisteredBuilderFunctions.Add(registration);
        }

        public static bool HasStarted()
        {
            return IsStarted;
        }
        
        internal static void StartFromGameObject(GameObject gameObject)
        {
            Debug.Assert(IsStarted == false, "Services already started");
            
            ServicesCollectionBuilder builder = new();

            foreach (var generatedRegistrationBuilderFunction in RegisteredBuilderFunctions)
            {
                try
                {
                    generatedRegistrationBuilderFunction?.Invoke(builder);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            builder.RegisterGameObject(gameObject);
            
            _serviceCollection = builder.Build();
            StartServices();
        }


        internal static bool TryUpdate()
        {
            if (IsStarted == false)
            {
                return false;
            }

            UpdateServices();
            return true;
        }

        private static void Stop()
        {
            Application.quitting -= Stop;
            
            if (IsStarted == false)
            {
                return;
            }
            
            StopServices();
            _serviceCollection = null;
        }

        [UsedImplicitly]
        public static bool TryGet<TI>(out TI service)
        {
            if (HasStarted() == false)
            {
                service = default;
                return false;
            }
            
            return _serviceCollection.TryGet<TI>(out service);
        }

        [UsedImplicitly]
        public static TI Get<TI>()
        {
            return _serviceCollection.Get<TI>();
        }
        
        private static void StartServices()
        {
            Debug.Assert(IsStarted == false, "Services already started");
            
            Application.quitting += Stop;
            
            Debug.Log("Starting application services...");
            
            ApplicationServicesUpdater.EnsureHasUpdater();
            
            _serviceCollection.StartServices();
            
            Debug.Log("Started application services.");
        }
        
        private static void UpdateServices()
        {
            Debug.Assert(IsStarted == true, "Services not started");
            _serviceCollection.UpdateServices();
            SceneServices.UpdateServices();
        }
        
        private static void StopServices()
        {
            Debug.Assert(IsStarted == true, "Services not started");
            Debug.Log("Stopping application services...");

            SceneServices.StopSceneServices();
            _serviceCollection.StopServices();
            
            Debug.Log("Stopped application services.");
        }
    }
}
