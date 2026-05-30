using System;
using System.Collections.Generic;
using FinalClick.ProjectSettings;
using UnityEngine;

namespace FinalClick.Services
{
    [ProjectSettings(fileName:"FinalClickServiceSettings", settingsProviderDirectory:"FinalClick", settingsProviderName:"Services")]
    public class ServicesProjectSettings : ScriptableObject, IProjectSettingsPreSaveProcessor
    {
        [SerializeField]
        private GameObject _servicesPrefab;

        [SerializeField]
        private List<ApplicationServiceRegistrationData> _applicationServiceData = new List<ApplicationServiceRegistrationData>();

        private GameObject ServicesPrefab => _servicesPrefab;

        public IReadOnlyCollection<ApplicationServiceRegistrationData> GetApplicationServiceData() => _applicationServiceData;

        internal bool TryGetServicesPrefab(out GameObject servicesPrefab)
        {
            servicesPrefab = ServicesPrefab;
            return servicesPrefab != null;
        }
        
        public void OnPreSave()
        {
            _applicationServiceData.RemoveAll(data => data.IsDataStillValid() == false);
        }

        public void RemoveAllNoneValidData()
        {
            _applicationServiceData.RemoveAll(data => data.IsDataStillValid() == false);
        }

        public bool Exists(Type type)
        {
            return _applicationServiceData.Exists(data => data.GetServiceType() == type);
        }

        public void Add(ApplicationServiceRegistrationData data)
        {
            _applicationServiceData.Add(data);
        }
    }
}