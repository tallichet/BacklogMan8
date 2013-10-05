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
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().ApiKey))
            {
                NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
            }
        }

        private void Organization_Tapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var org = (sender as Panel).Tag as Core.Model.Organization;
            ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().CurrentOrganization = org;
            NavigationService.Navigate(new Uri("/OrganizationPage.xaml", UriKind.Relative));
        }

        private void Project_Tapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var p = (sender as Panel).Tag as Core.Model.Project;
            ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().CurrentProject = p;
            NavigationService.Navigate(new Uri("/ProjectPage.xaml", UriKind.Relative));
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            backgroundImage.Visibility = pivotMain.SelectedIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Backlog_Tapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var b = (sender as Panel).Tag as Core.Model.Backlog;
            ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().CurrentBacklog = b;
            NavigationService.Navigate(new Uri("/BacklogPage.xaml", UriKind.Relative));
        }
    }
}