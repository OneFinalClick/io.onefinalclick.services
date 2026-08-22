using System;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

namespace FinalClick.Services
{
    public static class SceneExtensions
    {
        [UsedImplicitly]
        public static bool TryGetService<T>(this Scene scene, out T service)
        {
            return SceneServices.TryGet<T>(scene, out service);
        }

        [UsedImplicitly]
        public static T GetService<T>(this Scene scene)
        {
            if (TryGetService<T>(scene, out var service) == false)
            {
                throw new ArgumentException("Service not found.");
            }
            
            return service;
        }
    }
}