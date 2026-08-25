using FinalClick.ProjectSettings;
using UnityEngine;

namespace FinalClick.Services
{
    [ProjectSettings(fileName:"ServiceProjectSettings", fileDirectory:"FinalClick", settingsProviderDirectory:"FinalClick", settingsProviderName:"Services", editorOnly: true)]
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