﻿using BacklogMan.Client.App.Win81.Common;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BacklogMan.Client.App.Win81.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class BacklogPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public BacklogPage()
        {
            this.SizeChanged += PageSizeChanged;

            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }
        
        private void PageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 260)
            {
                BacklogItemWidth = e.NewSize.Width - 260;
            }

            if (e.NewSize.Width > e.NewSize.Height)
            {
                LandscapeVisibility = Visibility.Visible;
                ProtraitVisibility = Visibility.Collapsed;
            }
            else
            {
                LandscapeVisibility = Visibility.Collapsed;
                ProtraitVisibility = Visibility.Visible;
            }
        }

        public double BacklogItemWidth
        {
            get { return (double)GetValue(BacklogItemWidthProperty); }
            private set { SetValue(BacklogItemWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BacklogItemWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BacklogItemWidthProperty =
            DependencyProperty.Register("BacklogItemWidth", typeof(double), typeof(BacklogPage), new PropertyMetadata(300));


        /// <summary>
        /// return visible if landscape, false if not
        /// </summary>
        public Visibility LandscapeVisibility
        {
            get { return (Visibility)GetValue(LandscapeVisibilityProperty); }
            private set { SetValue(LandscapeVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LandscapeVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LandscapeVisibilityProperty =
            DependencyProperty.Register("LandscapeVisibility", typeof(Visibility), typeof(BacklogPage), new PropertyMetadata(Visibility.Visible));


        public Visibility ProtraitVisibility
        {
            get { return (Visibility)GetValue(ProtraitVisibilityProperty); }
            set { SetValue(ProtraitVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProtraitVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProtraitVisibilityProperty =
            DependencyProperty.Register("ProtraitVisibility", typeof(Visibility), typeof(BacklogPage), new PropertyMetadata(Visibility.Collapsed));

        


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
            storyEditor.Hide();
        }

        #endregion


        private void story_tapped(object sender, TappedRoutedEventArgs e)
        {
            var story = (sender as FrameworkElement).Tag as Core.Model.Story;

            var editStory = new Core.Model.Story(story);

            storyEditor.DataContext = editStory;

            storyEditor.Show();
        }

        private void addStory_Click(object sender, RoutedEventArgs e)
        {
            var story = Core.Model.Story.CreateDefault();

            storyEditor.DataContext = story;

            storyEditor.Show();
        }

        private async void deleteStory_Click(object sender, RoutedEventArgs e)
        {
            if (storiesList.SelectedItems != null && storiesList.SelectedItems.Count > 0)
            {
                var deleteConfirmationMessage = string.Format(
                        new ResourceLoader().GetString("DeleteStoryMessage"),
                        storiesList.SelectedItems.Count
                    );

                var deleteConfirmed = await new Controls.DeletionConfirmation(deleteConfirmationMessage).ShowDeleteConfirmationAsync(sender as FrameworkElement);

                if (deleteConfirmed)
                {
                    var storiesToDelete = storiesList.SelectedItems.Cast<Core.Model.Story>().ToArray();
                    ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().DeleteStories(storiesToDelete);
                }
            }
        }

        private void storiesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItems = (sender as ListViewBase).SelectedItems;

            if (selectedItems != null && selectedItems.Count > 0)
            {
                commandBar.IsSticky = true;
                commandBar.IsOpen = true;

                deleteStoryAppbarButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                commandBar.IsOpen = false;
                commandBar.IsSticky = false;
                
                deleteStoryAppbarButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }
    }
}
