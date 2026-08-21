using FinalClick.ProjectSettings;
using UnityEngine;

namespace FinalClick.Services
{
    [ProjectSettings(fileName:"FinalClickServiceSettings", settingsProviderDirectory:"FinalClick", settingsProviderName:"Services")]
    public class ServicesProjectSettings : ScriptableObject
    {
        [SerializeField]
        private GameObject _servicesPrefab;

        private GameObject ServicesPrefab => _servicesPrefab;

        internal bool TryGetServicesPrefab(out GameObject servicesPrefab)
        {
            servicesPrefab = ServicesPrefab;
            return servicesPrefab != null;
        }
    }
}