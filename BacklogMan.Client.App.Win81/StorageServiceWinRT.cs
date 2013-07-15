using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BacklogMan.Client.App.Win81
{
    public class StorageServiceWinRT : Core.Service.IStorageService
    {
        public void SaveSetting<T>(string key, T value)
        {
            Windows.Storage.ApplicationData.Current.RoamingSettings.Values[key] = value;
        }

        public bool TryLoadSetting<T>(string key, out T value)
        {
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey(key))
            {
                value = (T)roamingSettings.Values[key];
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }
    }
}
