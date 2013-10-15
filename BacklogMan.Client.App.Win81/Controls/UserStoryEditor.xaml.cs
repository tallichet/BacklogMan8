using BacklogMan.Client.App.Win81.Common;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class UserStoryEditor : UserControl
    {
        public UserStoryEditor()
        {
            this.InitializeComponent();

            #region init story points list
            storyEditPoints.Items.Add(-1d);
            storyEditPoints.Items.Add(0d);
            storyEditPoints.Items.Add(1d);
            storyEditPoints.Items.Add(2d);
            storyEditPoints.Items.Add(3d);
            storyEditPoints.Items.Add(5d);
            storyEditPoints.Items.Add(8d);
            storyEditPoints.Items.Add(13d);
            storyEditPoints.Items.Add(20d);
            storyEditPoints.Items.Add(40d);
            storyEditPoints.Items.Add(100d);
            #endregion

            #region init story status list
            storyEditStatus.Items.Add(Core.Model.StoryStatus.ToDo);
            storyEditStatus.Items.Add(Core.Model.StoryStatus.InProgress);
            storyEditStatus.Items.Add(Core.Model.StoryStatus.Completed);
            storyEditStatus.Items.Add(Core.Model.StoryStatus.Accepted);
            storyEditStatus.Items.Add(Core.Model.StoryStatus.Rejected);
            #endregion
        }

        public Core.Model.Story Story
        {
            get
            {
                return this.DataContext as Core.Model.Story;
            }
        }

        #region Commands
        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(() => Save());
                }
                return saveCommand;
            }
        }
        #endregion

        private async void Save()
        {
            this.IsStoryEditable = false;

            await ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().UpdateStory(this.Story);
                
            await HideAsync();

            this.IsStoryEditable = true;
        }

        public void Show()
        {
            VisualStateManager.GoToState(this, "Visible", true);
        }

        public void Hide()
        {
            VisualStateManager.GoToState(this, "Collapsed", true);
            
        }

        public async Task HideAsync()
        {
            Hide();
            await Task.Delay(600);  // wait end of animation
        }

        private async void backgroundGridTapped(object sender, TappedRoutedEventArgs e)
        {
            if (!e.Handled)
            {
                await this.HideAsync();
                this.DataContext = null;
            }
        }

        private void scrollviewerTapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        public bool IsStoryEditable
        {
            get { return (bool)GetValue(IsStoryEditableProperty); }
            set { SetValue(IsStoryEditableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsStoryEditable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsStoryEditableProperty =
            DependencyProperty.Register("IsStoryEditable", typeof(bool), typeof(UserStoryEditor), new PropertyMetadata(true));
    }

    public class StoryUpdateEventArgs : EventArgs
    {
        public StoryUpdateEventArgs(Core.Model.Story story)
        {
            this.Story = story;
        }

        public Core.Model.Story Story { get; private set; }
    }
}
