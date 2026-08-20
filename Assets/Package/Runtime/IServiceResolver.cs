using System;

namespace FinalClick.Services
{
    public interface IServiceResolver
    {
        public bool TryGet(Type type, out object service);
    }
}