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
    public partial class OrganizationPage : PhoneApplicationPage
    {
        public OrganizationPage()
        {
            InitializeComponent();
            (ApplicationBar.Buttons[0] as ApplicationBarIconButton).Text = BacklogMan.Client.Phone8.Resources.AppResources.MenuBarRefreshButtonLabel;            
        }

        private void Project_Tapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var p = (sender as Panel).Tag as Core.Model.Project;
            ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().CurrentProject = p;
            NavigationService.Navigate(new Uri("/ProjectPage.xaml", UriKind.Relative));
        }

        private void Backlog_Tapped(object sender, System.Windows.Input.GestureEventArgs e)
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
            ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().RefreshOrganzationCommand.Execute(null);
        }
    }
}