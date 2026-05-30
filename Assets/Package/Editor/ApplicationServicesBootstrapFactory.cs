using FinalClick.ProjectSettings;
using UnityEngine;

namespace FinalClick.Services.Editor
{
    public class ApplicationServicesBootstrapFactory
    {
        internal static GameObject Create()
        {
            // Create from prefab in settings, or make blank gameobject.
            bool usePrefab = ProjectSettingsDatabase.Get<ServicesProjectSettings>().TryGetServicesPrefab(out GameObject servicesPrefab);

            GameObject servicesInstance = usePrefab ? Object.Instantiate(servicesPrefab) : new GameObject("Application Services");

            AddSavedServicesToApplicationServices(servicesInstance);
            SetGameObjectAsApplicationServices(servicesInstance);
            
            return servicesInstance;
        }

        private static void AddSavedServicesToApplicationServices(GameObject gameObject)
        {
            gameObject.AddComponent<RegisterNoneMonoBehaviourApplicationServices>();
        }
        
        private static void SetGameObjectAsApplicationServices(GameObject gameObject)
        {
            gameObject.AddComponent<ApplicationServicesMarker>();
        }
    }
}