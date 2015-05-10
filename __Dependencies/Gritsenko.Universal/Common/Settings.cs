using System.Threading.Tasks;
using Windows.Storage;
using Gritsenko.Universal.Abstract;
using Newtonsoft.Json;

namespace Gritsenko.Universal.Common
{
    public class LocalSettings : ISettingsService
    {
        public async Task Save(string key, object value)
        {
            var applicationData = ApplicationData.Current;

            var localSettings = applicationData.LocalSettings;
            localSettings.Values[key] = value;
        }

        /**********************************************************************************/

        public T Load<T>(string key)
        {
            var applicationData = ApplicationData.Current;

            var localSettings = applicationData.LocalSettings;

            var value = localSettings.Values[key];

            if (value != null)
            {
                return (T) value;
            }
            else
            {
                // Access data in value
            }

            // Delete a simple setting
            return default(T);
        }

        /**********************************************************************************/

        public void SaveAsJson(string key, object value)
        {
            Save(key, JsonConvert.SerializeObject(value));
        }

        /**********************************************************************************/

        public T LoadFromJson<T>(string key)
        {
            var json = Load<string>(key);
            if (string.IsNullOrEmpty(json))
            {
                return default (T);
            }
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}