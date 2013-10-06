using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BacklogMan.Client.Phone8.Resources;

namespace BacklogMan.Client.Phone8.Controls
{
    public partial class UserStoryDefinition : UserControl
    {
        public UserStoryDefinition()
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
            DependencyProperty.Register("Story", typeof(Core.Model.Story), typeof(UserStoryDefinition), new PropertyMetadata(null, StoryPropertyChanged));

        private static void StoryPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as UserStoryDefinition;
            var story = e.NewValue as Core.Model.Story;

            if (ctrl != null)
            {
                ctrl.textbox.Inlines.Clear();
                if (story != null)
                {
                    ctrl.textbox.Inlines.Add(new System.Windows.Documents.Run() { Text = AppResources.StoryAsA + " " });
                    ctrl.textbox.Inlines.Add(new System.Windows.Documents.Run() { Text = story.AsUser });
                    ctrl.textbox.Inlines.Add(new System.Windows.Documents.Run() { Text = " " + AppResources.StoryIWantTo + " " });
                    ctrl.textbox.Inlines.Add(new System.Windows.Documents.Run() { Text = story.Goal, FontWeight = FontWeights.Bold });
                    if (string.IsNullOrWhiteSpace(story.Result) == false)
                    {
                        ctrl.textbox.Inlines.Add(new System.Windows.Documents.Run() { Text = " " + AppResources.StorySoICan });
                        ctrl.textbox.Inlines.Add(new System.Windows.Documents.Run() { Text = story.Result });
                    }
                }
            }
        }
    }
}
