using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace BacklogMan.Client.App.Win8
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class MainPage : BacklogMan.Client.App.Win8.Common.LayoutAwarePage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Assign a collection of bindable groups to this.DefaultViewModel["Groups"]
        }

        private void ProjectItemClicked(object sender, ItemClickEventArgs e)
        {
            var project = e.ClickedItem as Core.Model.Project;

            var vm = ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>();
            vm.CurrentProject = project;

            this.Frame.Navigate(typeof(ProjectPage));
        }

        private void OrgItemClicked(object sender, ItemClickEventArgs e)
        {
            var org = e.ClickedItem as Core.Model.Organization;

            var vm = ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>();
            vm.CurrentOrganization = org;

            this.Frame.Navigate(typeof(OrganizationPage));
        }

        private void OpenNotEstimatedStoriesDetail(object sender, ItemClickEventArgs e)
        {

        }
    }
}
