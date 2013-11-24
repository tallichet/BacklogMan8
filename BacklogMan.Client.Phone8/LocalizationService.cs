using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BacklogMan.Client.Phone8
{
    public class LocalizationService : Core.Service.ILocalizationService
    {
        public string GetStringForKey(string key)
        {
            return Resources.AppResources.ResourceManager.GetString(key);
        }
        public void DisplayMessageInMessageBox(string key)
        {
            string title = "";
            string content = key;
            try
            {
                title = GetStringForKey(key + "_Title");
                content = GetStringForKey(key + "_Content");
            }
            finally
            {
                MessageBox.Show(content, title, MessageBoxButton.OK);
            }
        }


    }
}
