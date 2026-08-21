using System;
using System.Collections.Generic;
using System.Reflection;
using FinalClick.Services.Attributes;
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
                generatedRegistrationBuilderFunction?.Invoke(builder);
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
            if (IsStarted == false)
            {
                return;
            }
            
            UnbindDelegates();

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

        // Ensure stop is called when exiting playmode or closing the application.
        // -----------------------------------------------------------------------
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void EnsureApplicationServicesUnregistersOnExit()
        {
            BindDelegates();
        }

#if UNITY_EDITOR
        private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
        {
            if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
            {
                Stop();
            }
        }
#endif
        
        private static void BindDelegates()
        {
            Application.quitting += Stop;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
        }
        
        private static void UnbindDelegates()
        {
            Application.quitting -= Stop;

#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
#endif
        }
    }
}
