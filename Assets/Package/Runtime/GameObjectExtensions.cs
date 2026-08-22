using System;
using JetBrains.Annotations;
using UnityEngine;

namespace FinalClick.Services
{
    public static class GameObjectExtensions
    {
        [UsedImplicitly]
        public static bool TryGetService<T>(this GameObject gameObject, out T service)
        {
            if (gameObject == null)
            {
                // Can be null when scene is destroyed.
                service = default;
                return false;
            }

            return SceneServices.TryGet<T>(gameObject.scene, out service);
        }

        [UsedImplicitly]
        public static T GetService<T>(this GameObject gameObject)
        {
            if (TryGetService<T>(gameObject, out var service) == false)
            {
                throw new ArgumentException("Service not found.");
            }
            
            return service;
        }
    }
}