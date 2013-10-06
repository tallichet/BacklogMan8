using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Practices.ServiceLocation;
using System.Windows.Threading;

namespace BacklogMan.Client.Phone8.Controls
{
    public partial class UserStoryStatus : UserControl
    {
        public UserStoryStatus()
        {
            InitializeComponent();
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
            if (d != null)
            {
                var story = e.NewValue as Core.Model.Story;

                VisualStateManager.GoToState(ctrl, story.Status.ToString(), false);
            }
        }

        private void LayoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            
        }

        private void SetStatus_Click(object sender, RoutedEventArgs e)
        {
            var status = (Core.Model.StoryStatus)Enum.Parse(typeof(Core.Model.StoryStatus), (sender as MenuItem).Tag as string);

            ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().SetStoriesStatus(this.Story, status).ContinueWith( (t) => 
            {
                if (t.Status == System.Threading.Tasks.TaskStatus.RanToCompletion && t.Result)
                {
                    this.Dispatcher.InvokeAsync(() => VisualStateManager.GoToState(this, status.ToString(), false));
                }
            });
        }
    }
}
