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
using BacklogMan.Client.Core.ViewModel;

namespace BacklogMan.Client.Phone8
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
            if (string.IsNullOrWhiteSpace(ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().ApiKey))
            {
                sectionLogout.Visibility = Visibility.Collapsed;
            }
            else
            {
                sectionLogin.Visibility = Visibility.Collapsed;
            }
        }



        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Username.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register("Username", typeof(string), typeof(Settings), new PropertyMetadata(""));



        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(Settings), new PropertyMetadata(""));

        private async void Connect(object sender, RoutedEventArgs e)
        {
            try
            {
                buttonConnect.IsEnabled = false;
                connectProgressBar.IsIndeterminate = true;
                if (await ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().GetApiKey(Username, Password))
                {
                    this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                    this.NavigationService.RemoveBackEntry();
                }
                else
                {
                    MessageBox.Show("unable to connect. please retry");
                    Password = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connectProgressBar.IsIndeterminate = false;
                buttonConnect.IsEnabled = true;
            }
        }

        private void self_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator()
            {
                Text = "Backlogman",
                IsVisible = true,
            };
        }

        private void Disconnect(object sender, RoutedEventArgs e)
        {
            sectionLogout.Visibility = System.Windows.Visibility.Collapsed;
            ServiceLocator.Current.GetInstance<IMainViewModel>().ClearApiKey();
            sectionLogin.Visibility = System.Windows.Visibility.Visible;
        }


    }
}