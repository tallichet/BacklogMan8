using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Practices.ServiceLocation;

namespace BacklogMan.Client.Phone8
{
    public partial class ProjectPage : PhoneApplicationPage
    {
        public ProjectPage()
        {
            InitializeComponent();
        }

        private void BacklogTapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var b = (sender as Panel).Tag as Core.Model.Backlog;
            ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().CurrentBacklog = b;
            NavigationService.Navigate(new Uri("/BacklogPage.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator()
            {
                Text = "Backlogman",
                IsVisible = true,
            };
        }

        private void refreshClick(object sender, EventArgs e)
        {
            ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().RefreshProjectCommand.Execute(null);
        }
    }
}