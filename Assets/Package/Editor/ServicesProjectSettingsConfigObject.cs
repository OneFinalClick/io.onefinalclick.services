using System;
using System.Collections.Generic;
using UnityEngine;

namespace FinalClick.Services.Editor
{
    public class ServicesProjectSettingsConfigObject : ScriptableObject
    {
        [SerializeField]
        private GameObject _servicesPrefab;

        [SerializeField]
        private List<ApplicationServiceRegistrationData> _applicationServiceData = new List<ApplicationServiceRegistrationData>();
        
        public GameObject ServicesPrefab
        {
            get => _servicesPrefab;
            set => _servicesPrefab = value;
        }
        
        public IReadOnlyCollection<ApplicationServiceRegistrationData> ApplicationServiceData => _applicationServiceData;

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