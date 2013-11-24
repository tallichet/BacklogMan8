using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;

namespace BacklogMan.Client.App.Win8
{
    public class LocalizationServiceWinRT : Core.Service.ILocalizationService
    {
        ResourceLoader resource;

        public LocalizationServiceWinRT()
        {
            resource = new ResourceLoader();
        }

        public string GetStringForKey(string key)
        {
            try
            {
                return resource.GetString(key);
            }
            catch
            {
                return "[" + key + "]";
            }
        }


        public async void DisplayMessageInMessageBox(string key)
        {
            var dlg = new MessageDialog(GetStringForKey(key + "_content"));
            dlg.Title = GetStringForKey(key + "_title");
            await dlg.ShowAsync();

        }


    }
}
