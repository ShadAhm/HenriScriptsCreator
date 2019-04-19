using ShadAhm.HenriScriptsCreator.Service.Models;
using Newtonsoft.Json;
using System.IO;

namespace ShadAhm.HenriScriptsCreator.Service.Functions
{
    public class AppSettingsService
    {
        public AppSettingsModel ReadAppSettings(string settingsFilePath)
        {
            using (StreamReader r = new StreamReader(settingsFilePath))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<AppSettingsModel>(json);
            }
        }
    }
}
