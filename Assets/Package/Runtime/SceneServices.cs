using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OneFinalClick.Services
{
    public static class SceneServices
    {
        private static readonly Dictionary<Scene, ServiceCollection> _sceneServices = new Dictionary<Scene, ServiceCollection>();

        public static bool HasStartedForScene(Scene scene)
        {
            Debug.Assert(scene.IsValid() == true, "Scene is not valid");
            
            if (_sceneServices.TryGetValue(scene, out var services) == false)
            {
                return false;
            }

            return services.IsStarted;
        }
        
        [UsedImplicitly]
        public static bool TryGet<TI>(Scene scene, out TI service)
        {
            if (scene.IsValid() == false || _sceneServices.TryGetValue(scene, out ServiceCollection services) == false)
            {
                return ApplicationServices.TryGet<TI>(out service);
            }
            
            return services.TryGet(out service);
        }

        [UsedImplicitly]
        public static TI Get<TI>(Scene scene)
        {
            if (scene.IsValid() == true && _sceneServices.TryGetValue(scene, out ServiceCollection services) == true)
            {
                return services.Get<TI>();
            }

            return ApplicationServices.Get<TI>();
        }

        public static void StartServicesForScene(Scene scene)
        { 
            Debug.Assert(_sceneServices.ContainsKey(scene) == false, "Services already started");
            
            ServicesCollectionBuilder builder = new();

            builder.RegisterSceneServices(scene);
            
            var services = builder.Build();
            _sceneServices.Add(scene, services);
            
            services.StartServices(ApplicationServices.ServiceCollection);
            Debug.Log($"Started services for scene: {scene.name}({scene.handle})");
        }

        internal static void UpdateServices()
        {
            foreach (var sceneServices in _sceneServices)
            {
                sceneServices.Value.UpdateServices();
            }
        }

        internal static void StopSceneServices()
        {
            // Create a copy as StopServicesForScene will modify _sceneServices.
            var scenes = _sceneServices.Keys.ToList();

            foreach (var scene in scenes)
            {
                StopServicesForScene(scene);
            }
        }
        
        internal static void StopServicesForScene(Scene scene)
        {
            if (_sceneServices.TryGetValue(scene, out var services) == false)
            {
                return;
            }
            
            services.StopServices();
            _sceneServices.Remove(scene);
           Debug.Log($"Stopped services for scene: {scene.name}({scene.handle})");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void EnsureApplicationServicesUnregistersOnExit()
        {
            Application.quitting += OnApplicationQuitting;
            BindDelegates();
        }

        private static void OnApplicationQuitting()
        {
            Application.quitting -= OnApplicationQuitting;
            UnbindDelegates();
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode _)
        {
            StartServicesForScene(scene);
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            StopServicesForScene(scene);
        }

        private static void BindDelegates()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private static void UnbindDelegates()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}
