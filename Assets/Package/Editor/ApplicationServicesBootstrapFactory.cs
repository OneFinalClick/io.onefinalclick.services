using OneFinalClick.ProjectSettings;
using OneFinalClick.ProjectSettings.Editor;
using UnityEngine;

namespace OneFinalClick.Services.Editor
{
    public class ApplicationServicesBootstrapFactory
    {
        internal static GameObject Create()
        {
            // Create instance of from prefab set in settings,
            // if none set, just make an empty GameObject instead
            bool usePrefab = ProjectSettingsEditorDatabase.GetOrCreateDefault<ServicesProjectSettings>().TryGetServicesPrefab(out GameObject servicesPrefab);
            GameObject servicesInstance = usePrefab ? Object.Instantiate(servicesPrefab) : new GameObject("Application Services");

            SetGameObjectAsApplicationServices(servicesInstance);
            
            return servicesInstance;
        }

        
        private static void SetGameObjectAsApplicationServices(GameObject gameObject)
        {
            gameObject.AddComponent<ApplicationServicesMarker>();
        }
    }
}