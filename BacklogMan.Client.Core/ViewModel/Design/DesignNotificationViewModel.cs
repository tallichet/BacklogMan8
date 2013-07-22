using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BacklogMan.Client.Core.ViewModel.Design
{
    public class DesignNotificationViewModel : IInternalNotificationViewModel
    {

        public string Title
        {
            get { return "this is a notification"; }
        }

        public void ShowNotificationText(string text)
        {
            throw new NotImplementedException();
        }


        public void ShowNotificationForKey(string key, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
