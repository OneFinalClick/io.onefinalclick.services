using System;

namespace OneFinalClick.Services
{
    public interface IServiceResolver
    {
        public bool TryGet(Type type, out object service);
    }
}