using System;

namespace OneLastClick.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MonoBehaviourServiceAttribute : Attribute
    {
        public readonly Type[] RegisterTypes;

        public MonoBehaviourServiceAttribute(params Type[] registerTypes)
        {
            RegisterTypes = registerTypes ?? Array.Empty<Type>();
            
            foreach (var registerType in RegisterTypes)
            {
                if (registerType == null)
                {
                    throw new ArgumentNullException(nameof(registerTypes));
                }
            }
        }
    }
}