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
    public sealed partial class ProjectsPage : BacklogMan.Client.App.Win8.Common.LayoutAwarePage
    {
        public ProjectsPage()
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

        private void OpenProject(object sender, RoutedEventArgs e)
        {
            var project = (sender as Control).Tag as Core.Model.Project;

            ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().CurrentProject = project;

            this.Frame.Navigate(typeof(ProjectPage));
        }

        private void BacklogItemClicked(object sender, ItemClickEventArgs e)
        {
            var backlog = e.ClickedItem as Core.Model.Backlog;
            var project = findProjectForBacklog(backlog);

            var vm = ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>();
            vm.CurrentProject = project;
            vm.CurrentBacklog = backlog;

            this.Frame.Navigate(typeof(BacklogPage));
        }

        private Core.Model.Project findProjectForBacklog(Core.Model.Backlog backlog)
        {
            foreach (var p in ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().Projects)
            {
                if (p.Backlogs.Any(b => b.Id == backlog.Id))
                    return p;
            }
            return null;
        }
    }
}
