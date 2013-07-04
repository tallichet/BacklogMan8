using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BacklogMan.Client.App.Win8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }



        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Username.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register("Username", typeof(string), typeof(LoginPage), new PropertyMetadata("", UsernameOrPasswordChanged));



        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(LoginPage), new PropertyMetadata("", UsernameOrPasswordChanged));

        private static void UsernameOrPasswordChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as LoginPage).loginCommand.RaiseCanExecuteChanged();
        }

        private RelayCommand loginCommand = null;

        private bool loginInProgress = false;

        public ICommand LoginCommand
        {
            get
            {
                if (loginCommand == null)
                {
                    loginCommand = new RelayCommand(
                        () => tryLogin(),
                        () => !loginInProgress && !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password)
                    );
                }
                return loginCommand;
            }
        }

        private async void tryLogin()
        {
            loginInProgress = true;
            loginCommand.RaiseCanExecuteChanged();
            
            var result = await ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().GetApiKey(Username, Password);

            if (!result)
            {
                loginInProgress = false;
                loginCommand.RaiseCanExecuteChanged();
            }
            else
            {
                this.Frame.Navigate(typeof(MainPage));
                this.Frame.SetNavigationState("1,0");
            }
        }

    }
}
