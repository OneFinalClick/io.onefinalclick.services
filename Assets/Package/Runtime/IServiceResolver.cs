using System;

namespace OneLastClick.Services
{
    public interface IServiceResolver
    {
        public bool TryGet(Type type, out object service);
    }
}