using UnityEngine;

namespace OneLastClick.Services
{
    /// <summary>
    /// This will be ran really early in the initial to register all application services.
    /// </summary>
    [DefaultExecutionOrder(-1000)]
    internal class ApplicationServicesMarker : MonoBehaviour
    {
        private void Awake()
        {
            if (ApplicationServices.HasStarted() == true)
            {
                DestroyImmediate(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject);
            ApplicationServices.StartFromGameObject(gameObject);
        }
    }
}