using System;

namespace OneFinalClick.Services
{
    public static class ServiceResolverExtensions
    {
        public static IServiceResolver CreateResolverWithFallback(this IServiceResolver serviceResolver, IServiceResolver fallbackResolver)
        {
            return new ServiceResolverWithFallback(serviceResolver, fallbackResolver);
        }
        
        public static bool TryGet<T>(this IServiceResolver serviceResolver, out T service)
        {
            if (serviceResolver.TryGet(typeof(T), out var serviceAsObject) == false)
            {
                service = default;
                return false;
            }

            if (serviceAsObject is T typedService)
            {
                service = typedService;
                return true;
            }

            service = default;
            return false;
        }
        
        public static object Get(this IServiceResolver serviceResolver, Type type)
        {
            if (serviceResolver.TryGet(type, out object service) == false)
            {
                throw new InvalidOperationException($"No service found for type {type}");
            }

            return service;
        }

        public static TI Get<TI>(this IServiceResolver serviceResolver)
        {
            return (TI) serviceResolver.Get(typeof(TI));
        }
    }
}