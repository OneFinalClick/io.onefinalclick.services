using System.Collections.Generic;
using FinalClick.ProjectSettings;
using FinalClick.Services.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace FinalClick.Services
{
    internal class RegisterNoneMonoBehaviourApplicationServices : MonoBehaviour
    {
        [RegisterServices]
        [UsedImplicitly]
        private void RegisterNoneMonoBehaviourServices(ServicesCollectionBuilder builder)
        {
            IReadOnlyCollection<ApplicationServiceRegistrationData> servicesData = ProjectSettingsDatabase.Get<ServicesProjectSettings>().GetApplicationServiceData();
            foreach (var serviceData in servicesData)
            {
                builder.Register(serviceData.CreateNewInstance(), serviceData.GetRegisterAsTypes());
            }
        }
    }
}