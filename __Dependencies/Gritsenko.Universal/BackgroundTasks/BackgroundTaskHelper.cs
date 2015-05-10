using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Background;

namespace Gritsenko.Universal.BackgroundTasks
{
    public class BackgroundTaskHelper
    {
        //
        // Register a background task with the specified taskEntryPoint, name, trigger,
        // and condition (optional).
        //
        // taskEntryPoint: Task entry point for the background task.
        // taskName: A name for the background task.
        // trigger: The trigger for the background task.
        // condition: Optional parameter. A conditional event that must be true for the task to fire.
        //
        public static async Task<BackgroundTaskRegistration> RegisterBackgroundTask(string taskEntryPoint,
                                                                        string taskName,
                                                                        IBackgroundTrigger trigger,
                                                                        IBackgroundCondition condition)
        {
            //
            // Check for existing registrations of this background task.
            //
            var isNewVersion = await CheckAppVersion();

            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {

                if (cur.Value.Name == taskName)
                {
                    // 
                    // The task is already registered.
                    // 

                    if (!isNewVersion)
                    {
                        return (BackgroundTaskRegistration) (cur.Value);
                    }
                    else
                    {
                        cur.Value.Unregister(false);  
                    }
                }
            }


            //
            // Register the background task.
            //

            var builder = new BackgroundTaskBuilder();

            builder.Name = taskName;
            builder.TaskEntryPoint = taskEntryPoint;
            builder.SetTrigger(trigger);

            if (condition != null)
            {

                builder.AddCondition(condition);
            }

            BackgroundTaskRegistration task = builder.Register();

            return task;
        }

        /**********************************************************************************/

        private async static Task<bool> CheckAppVersion()
        {
            var result = false;
            String appVersion = String.Format("{0}.{1}.{2}.{3}",
                    Package.Current.Id.Version.Build,
                    Package.Current.Id.Version.Major,
                    Package.Current.Id.Version.Minor,
                    Package.Current.Id.Version.Revision);

            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values["TaskAppVersion"] != appVersion)
            {
                // Our app has been updated
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["TaskAppVersion"] = appVersion;

                // Call RemoveAccess
                BackgroundExecutionManager.RemoveAccess();
                result = true;
            }

            var status = await BackgroundExecutionManager.RequestAccessAsync();
            return result;
        }

    }
}
