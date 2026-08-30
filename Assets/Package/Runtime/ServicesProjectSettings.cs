using OneLastClick.ProjectSettings;
using UnityEngine;

namespace OneLastClick.Services
{
    [ProjectSettings(fileName:"ServiceProjectSettings", fileDirectory:"OneLastClick", settingsProviderDirectory:"OneLastClick", settingsProviderName:"Services", editorOnly: true)]
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