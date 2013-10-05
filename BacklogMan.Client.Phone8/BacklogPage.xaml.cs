using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace BacklogMan.Client.Phone8
{
    public partial class BacklogPage : PhoneApplicationPage
    {
        public BacklogPage()
        {
            InitializeComponent();
        }

        private void Story_Tapped(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator()
            {
                Text = "Backlogman",
                IsVisible = true,
            };
        }
    }
}