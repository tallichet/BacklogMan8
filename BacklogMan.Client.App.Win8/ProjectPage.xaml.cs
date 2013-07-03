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

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace BacklogMan.Client.App.Win8
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class ProjectPage : BacklogMan.Client.App.Win8.Common.LayoutAwarePage
    {
        public ProjectPage()
        {
            this.InitializeComponent();

            var vm = this.DataContext as Core.ViewModel.IMainViewModel;
            CollectionViewSource cvs = new CollectionViewSource();
            cvs.Source = vm.CurrentProject.Backlogs;
            itemGridView.SetBinding(GridView.ItemsSourceProperty, new Binding() { Source = cvs.View });// 
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
            // TODO: Assign a bindable group to this.DefaultViewModel["Group"]
            // TODO: Assign a collection of bindable items to this.DefaultViewModel["Items"]
        }

        private void BacklogItemClicked(object sender, ItemClickEventArgs e)
        {
            var backlog = e.ClickedItem as Core.Model.Backlog;

            var vm = ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>();
            vm.CurrentBacklog = backlog;

            this.Frame.Navigate(typeof(BacklogPage));
        }
    }
}
