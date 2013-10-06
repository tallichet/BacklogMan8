using BacklogMan.Client.App.Win81.Common;
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
    public sealed partial class UserStoryEditor : UserControl
    {
        public UserStoryEditor()
        {
            this.InitializeComponent();
        }

        public Core.Model.Story Story
        {
            get
            {
                return this.DataContext as Core.Model.Story;
            }
        }

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

        private void Save()
        {
            if (StoryUpdate != null)
            {
                StoryUpdate(this, new StoryUpdateEventArgs(this.Story));
            }
        }

        public event EventHandler<StoryUpdateEventArgs> StoryUpdate;
        
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
