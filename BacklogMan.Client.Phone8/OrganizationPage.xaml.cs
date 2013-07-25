﻿using System;
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
        }

        private void Project_Tapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var p = (sender as Panel).Tag as Core.Model.Project;
            ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().CurrentProject = p;
            NavigationService.Navigate(new Uri("/ProjectPage.xaml", UriKind.Relative));
        }
    }
}