using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.ViewModel.Runtime
{
    public class InternalNotificationViewModel : ViewModelBase, IInternalNotificationViewModel
    {

        public string Title
        {
            get;
            private set;
        }

        public void ShowNotificationText(string text)
        {
            ShowNotification(text, TimeSpan.FromSeconds(3));
        }

        private async void ShowNotification(string text, TimeSpan duration)
        {
            Title = text;
            base.RaisePropertyChanged<string>(() => this.Title);

            await Task.Delay(duration);

            Title = "";
            base.RaisePropertyChanged<string>(() => this.Title);
        }
        
        public void ShowNotificationForKey(string key, params object[] args)
        {
            var text = ServiceLocator.Current.GetInstance<Service.ILocalizationService>().GetStringForKey(key);
            ShowNotification(string.Format(text, args), TimeSpan.FromSeconds(5));
        }
    }
}
