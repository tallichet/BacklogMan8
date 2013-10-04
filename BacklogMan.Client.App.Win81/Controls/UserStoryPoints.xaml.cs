using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BacklogMan.Client.App.Win81.Controls
{
    public sealed partial class UserStoryPoints : UserControl
    {
        public UserStoryPoints()
        {
            this.InitializeComponent();
        }


        public Core.Model.Story Story
        {
            get { return (Core.Model.Story)GetValue(StoryProperty); }
            set { SetValue(StoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Story.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StoryProperty =
            DependencyProperty.Register("Story", typeof(Core.Model.Story), typeof(UserStoryPoints), new PropertyMetadata(null, StoryPropertyChanged));

        private static void StoryPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as UserStoryPoints;
            if (ctrl != null)
            {
                var story = e.NewValue as Core.Model.Story;
                if (story != null && story.Points >= 0)
                {
                    ctrl.pointsDisplay.Text = story.Points.ToString();
                }
                else
                {
                    ctrl.pointsDisplay.Text = "";
                }
            }
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs arg)
        {
            var flyout = new Flyout();
            var picker = new PointsPicker();
            picker.PointsChoosed += (_, e) =>
            {
                flyout.Hide();
                setStoryPoints(e.Points);
            };

            arg.Handled = true;

            flyout.Content = picker;
            flyout.ShowAt(this);
        }

        private async void setStoryPoints(int newPoints)
        {
            var result = await ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().SetStoryPoints(this.Story, newPoints);
            if (result)
            {
                this.pointsDisplay.Text = newPoints.ToString();
            }
        }

    }
}
