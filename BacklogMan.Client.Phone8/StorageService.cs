using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Phone8
{
    public class StorageService : Core.Service.IStorageService
    {

        public void SaveSetting<T>(string key, T value)
        {
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings[key] = value;
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public bool TryLoadSetting<T>(string key, out T value)
        {
            T v;
            if (System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.TryGetValue(key, out v))
            {
                value = v;
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
