using System.Diagnostics;
using System.Linq;
using FinalClick.ProjectSettings;
using UnityEditor;

namespace FinalClick.Services.Editor
{
    public static class ServicesProjectSettingsEditorSync
    {
        [Conditional("UNITY_EDITOR")]
        private static void SyncApplicationServiceDataWithCurrentTypes()
        {
            ServicesProjectSettings settings = ProjectSettingsDatabase.Get<ServicesProjectSettings>();
            
            Debug.Assert(settings != null, "Missing settings.");
            
            var validTypes = FinalClick.Services.Attributes.RegisterAsApplicationServiceAttribute.GetTypesWithApplicationServiceAttribute().ToHashSet();

            // Remove any types that should no longer be registered.
            settings.RemoveAllNoneValidData();

            foreach (var type in validTypes)
            {
                bool exists = settings.Exists(type);
                if (!exists)
                {
                    ApplicationServiceRegistrationData newData = new ApplicationServiceRegistrationData(type);
                    settings.Add(newData);
                }
            }
            
            ProjectSettingsDatabase.Save(settings);
        }

        [InitializeOnLoadMethod]
        private static void OnProjectRecompile()
        {
            SyncApplicationServiceDataWithCurrentTypes();
        }
    }
}