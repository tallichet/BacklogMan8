using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BacklogMan.Client.App.Win8.Controls
{
    public sealed partial class StoryEditor : UserControl
    {
        public StoryEditor()
        {
            this.InitializeComponent();
        }



        public int ProjectId
        {
            get { return (int)GetValue(ProjectIdProperty); }
            set { SetValue(ProjectIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProjectId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectIdProperty =
            DependencyProperty.Register("ProjectId", typeof(int), typeof(StoryEditor), new PropertyMetadata(0));

        public int BacklogId
        {
            get { return (int)GetValue(BacklogIdProperty); }
            set { SetValue(BacklogIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BacklogId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BacklogIdProperty =
            DependencyProperty.Register("BacklogId", typeof(int), typeof(StoryEditor), new PropertyMetadata(0));

        private async void SaveStory(object sender, RoutedEventArgs e)
        {
            string message;
            if (await (this.DataContext as Core.Model.Story).Save(ProjectId, BacklogId))
            {
                message = "Story saved";
            }
            else
            {
                message = "Story NOT saved";
            }

            var dlg = new MessageDialog(message);
            await dlg.ShowAsync();
        }
    }
}
