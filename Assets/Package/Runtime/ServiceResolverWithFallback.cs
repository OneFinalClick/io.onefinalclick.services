using System;

namespace OneLastClick.Services
{
    internal class ServiceResolverWithFallback : IServiceResolver
    {
        private readonly IServiceResolver _serviceResolverMain;
        private readonly IServiceResolver _serviceResolverFallback;

        public ServiceResolverWithFallback(IServiceResolver serviceResolverMain, IServiceResolver serviceResolverFallback)
        {
            _serviceResolverMain = serviceResolverMain;
            _serviceResolverFallback = serviceResolverFallback;
        }

        public bool TryGet(Type type, out object service)
        {
            if (_serviceResolverMain.TryGet(type, out service) == true)
            {
                return true;
            }

            if (_serviceResolverFallback.TryGet(type, out service) == true)
            {
                return true;
            }

            return false;
        }
    }
}