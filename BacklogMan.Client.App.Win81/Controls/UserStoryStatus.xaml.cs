using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BacklogMan.Client.App.Win81.Controls
{
    public sealed partial class UserStoryStatus : UserControl
    {
        public UserStoryStatus()
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
            DependencyProperty.Register("Story", typeof(Core.Model.Story), typeof(UserStoryStatus), new PropertyMetadata(null, StoryPropertyChanged));

        private static void StoryPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as UserStoryStatus;
            if (ctrl != null)
            {
                var story = e.NewValue as Core.Model.Story;
                if (story != null && story.Points >= 0)
                {
                    ctrl.status.Text = getLabelForStatus(story.Status);
                }
                else
                {
                    ctrl.status.Text = "-";
                }
            }
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!this.IsEnabled) return;

            e.Handled = true;

            var menu = new MenuFlyout();

            menu.Items.Add(new MenuFlyoutItem() 
            {
                Text = getLabelForStatus(Core.Model.StoryStatus.ToDo),
                Command = new Common.RelayCommand(() => setStatus(Core.Model.StoryStatus.ToDo)),
            });
            menu.Items.Add(new MenuFlyoutItem()
            {
                Text = getLabelForStatus(Core.Model.StoryStatus.InProgress),
                Command = new Common.RelayCommand(() => setStatus(Core.Model.StoryStatus.InProgress)),
            });
            menu.Items.Add(new MenuFlyoutItem()
            {
                Text = getLabelForStatus(Core.Model.StoryStatus.Completed),
                Command = new Common.RelayCommand(() => setStatus(Core.Model.StoryStatus.Completed)),
            });
            menu.Items.Add(new MenuFlyoutItem()
            {
                Text = getLabelForStatus(Core.Model.StoryStatus.Accepted),
                Command = new Common.RelayCommand(() => setStatus(Core.Model.StoryStatus.Accepted)),
            });
            menu.Items.Add(new MenuFlyoutItem()
            {
                Text = getLabelForStatus(Core.Model.StoryStatus.Rejected),
                Command = new Common.RelayCommand(() => setStatus(Core.Model.StoryStatus.Rejected)),
            });

            menu.Placement = FlyoutPlacementMode.Left;
            menu.ShowAt(this);
        }

        private static string getLabelForStatus(Core.Model.StoryStatus newStatus)
        {
            switch(newStatus)
            {
                case Core.Model.StoryStatus.ToDo:
                    return "to do";
                case Core.Model.StoryStatus.InProgress:
                    return "in progress";
                case Core.Model.StoryStatus.Completed:
                    return "completed";
                case Core.Model.StoryStatus.Accepted:
                    return "accepted";
                case Core.Model.StoryStatus.Rejected:
                    return "rejected";
                default:
                    return "[" + newStatus.ToString() + "]";
            }
        }

        private async void setStatus(Core.Model.StoryStatus newStatus)
        {
            var result = await ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().SetStoriesStatus(this.Story, newStatus);
            if (result)
            {
                this.status.Text = getLabelForStatus(newStatus);
            }
        }

    }
}
