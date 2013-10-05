using BacklogMan.Client.App.Win81.Common;
using BacklogMan.Client.App.Win81.Data;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Hub Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=??????

namespace BacklogMan.Client.App.Win81
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public HubPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.SizeChanged += HubPage_SizeChanged;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var sampleDataGroups = await SampleDataSource.GetGroupsAsync();
            this.DefaultViewModel["Groups"] = sampleDataGroups;
        }

        /// <summary>
        /// Invoked when a HubSection header is clicked.
        /// </summary>
        /// <param name="sender">The Hub that contains the HubSection whose header was clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        void Hub_SectionHeaderClick(object sender, HubSectionHeaderClickEventArgs e)
        {
            HubSection section = e.Section;
            var group = section.DataContext;
            if (section == sectionProjects)
            {
                this.Frame.Navigate(typeof(Pages.ProjectPage), ((SampleDataGroup)group).UniqueId);
            }
            //
        }

        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        /// <param name="sender">The GridView or ListView
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            if (e.ClickedItem is Core.Model.Project)
            {
                ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().CurrentProject = e.ClickedItem as Core.Model.Project;
                this.Frame.Navigate(typeof(Pages.ProjectPage));
            }
            else if (e.ClickedItem is Core.Model.Organization)
            {
                ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().CurrentOrganization = e.ClickedItem as Core.Model.Organization;
                this.Frame.Navigate(typeof(Pages.OrganisationPage));
            }
            else if (e.ClickedItem is Core.Model.Backlog)
            {
                ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().CurrentBacklog = e.ClickedItem as Core.Model.Backlog;
                this.Frame.Navigate(typeof(Pages.BacklogPage));
            }
        }
        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void hubpage_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().ApiKey))
            {
                var opt = new CredentialPickerOptions()
                {
                    AuthenticationProtocol = AuthenticationProtocol.Basic,
                    AlwaysDisplayDialog = true,
                    Caption = "log to backlog man",
                    Message = "please enter credentials for backlog man",
                    TargetName = "https://apps.backlogman.com"
                };

                var cred = await CredentialPicker.PickAsync(opt);

                var key = await ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().GetApiKey(cred.CredentialUserName, cred.CredentialPassword);
            }
        }



        public double HubSectionHeight
        {
            get { return (double)GetValue(HubSectionHeightProperty); }
            set { SetValue(HubSectionHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HubSectionHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HubSectionHeightProperty =
            DependencyProperty.Register("HubSectionHeight", typeof(double), typeof(HubPage), new PropertyMetadata(0.0));

        




        void HubPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < e.NewSize.Height)
            {
                // portrait
                this.ImageSectionWidth = 0;
            }
            else
            {
                ImageSectionWidth = e.NewSize.Height * 1.622;
            }

            HubSectionHeight = e.NewSize.Height - 120;
        }

        public double ImageSectionWidth
        {
            get { return (double)GetValue(ImageSectionWidthProperty); }
            set { SetValue(ImageSectionWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSectionWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSectionWidthProperty =
            DependencyProperty.Register("ImageSectionWidth", typeof(double), typeof(HubPage), new PropertyMetadata(0));




    }
}
