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
    public sealed partial class BacklogPage : BacklogMan.Client.App.Win8.Common.LayoutAwarePage
    {
        public BacklogPage()
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
            // TODO: Assign a bindable group to this.DefaultViewModel["Group"]
            // TODO: Assign a collection of bindable items to this.DefaultViewModel["Items"]
        }

        private void StoryTapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void StoryLayerTapped(object sender, TappedRoutedEventArgs e)
        {
            if (!e.Handled)
            {
                HideStoryEditor();
            }
        }

        public void ShowStoryEditor()
        {
            VisualStateManager.GoToState(this, "StoryLayerVisible", true);

            this.BottomAppBar.IsOpen = false;
            this.BottomAppBar.IsSticky = false;
        }

        public void HideStoryEditor()
        {
            VisualStateManager.GoToState(this, "StoryLayerCollapsed", true);
        }

        private void showStory(object sender, ItemClickEventArgs e)
        {
            var story = e.ClickedItem as Core.Model.Story;

            ShowStoryEditor();
            storyEditor.DataContext = story;
        }

        private void deleteSelectedStories(object sender, RoutedEventArgs e)
        {
            if (itemGridView == null || itemGridView.SelectedItems == null) return;

            ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().DeleteStories(itemGridView.SelectedItems.Cast<Core.Model.Story>().ToArray());

            this.BottomAppBar.IsOpen = false;
            this.BottomAppBar.IsSticky = false;
        }

        private void editSelectedStory(object sender, RoutedEventArgs e)
        {
            if (itemGridView == null || itemGridView.SelectedItems == null) return;

            if (itemGridView.SelectedItems.Count == 1)
            {
                ShowStoryEditor();
                storyEditor.DataContext = itemGridView.SelectedItems.First();
            }
        }

        private void createNewStory(object sender, RoutedEventArgs e)
        {
            ShowStoryEditor();
            storyEditor.DataContext = new Core.Model.Story() 
            { 
                Backlog = ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().CurrentBacklog,
                Status = Core.Model.StoryStatus.ToDo
            };
        }

        private void selectedStories_Changed(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ListViewBase;

            if (list == null) return;

            switch (list.SelectedItems.Count)
            {
                case 0:
                    storyCommands.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    createStoryCommand.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    break;
                case 1:
                    storyCommands.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    createStoryCommand.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    break;
                default:
                    storyCommands.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    createStoryCommand.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    break;
            }

            if (list.SelectedItems.Count == 0)
            {
                this.BottomAppBar.IsOpen = false;
                this.BottomAppBar.IsSticky = false;
            }
            else
            {
                this.BottomAppBar.IsSticky = true;
                this.BottomAppBar.IsOpen = true;                
            }
            
        }

        private void refreshStories(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().RefreshBacklogStories();
        }
    }
}
