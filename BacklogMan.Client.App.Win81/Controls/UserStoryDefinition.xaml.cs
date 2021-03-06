﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
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
    public sealed partial class UserStoryDefinition : UserControl
    {
        public UserStoryDefinition()
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
            DependencyProperty.Register("Story", typeof(Core.Model.Story), typeof(UserStoryDefinition), new PropertyMetadata(null, StoryPropertyChanged));

        private static void StoryPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as UserStoryDefinition;
            var story = e.NewValue as Core.Model.Story;

            if (ctrl != null)
            {
                if (story == null)
                {
                    ctrl.textbox.Inlines.Clear();
                }
                else
                {
                    var res = ResourceLoader.GetForCurrentView();

                    ctrl.textbox.Inlines.Add(new Windows.UI.Xaml.Documents.Run() { Text = res.GetString("StoryAsA") });
                    ctrl.textbox.Inlines.Add(new Windows.UI.Xaml.Documents.Run() { Text = story.AsUser });
                    ctrl.textbox.Inlines.Add(new Windows.UI.Xaml.Documents.Run() { Text = res.GetString("StoryIWantTo")});
                    ctrl.textbox.Inlines.Add(new Windows.UI.Xaml.Documents.Run() { Text = story.Goal, FontWeight = FontWeights.Bold });
                    if (string.IsNullOrWhiteSpace(story.Result) == false)
                    {
                        ctrl.textbox.Inlines.Add(new Windows.UI.Xaml.Documents.Run() { Text = res.GetString("StorySoICan") });
                        ctrl.textbox.Inlines.Add(new Windows.UI.Xaml.Documents.Run() { Text = story.Result });
                    }
                }
            }
        }

    }
}
