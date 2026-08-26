using OneFinalClick.ProjectSettings;
using UnityEngine;

namespace OneFinalClick.Services
{
    [ProjectSettings(fileName:"ServiceProjectSettings", fileDirectory:"OneFinalClick", settingsProviderDirectory:"OneFinalClick", settingsProviderName:"Services", editorOnly: true)]
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