using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

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
            return resource.GetString(key);
        }
    }
}
