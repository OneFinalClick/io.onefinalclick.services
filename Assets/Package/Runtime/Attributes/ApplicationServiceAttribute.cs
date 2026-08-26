using System;

namespace OneFinalClick.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ApplicationServiceAttribute : Attribute
    {
        public readonly Type[] RegisterTypes;
        
        public ApplicationServiceAttribute(params Type[] registerTypes)
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