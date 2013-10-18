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
    public sealed partial class DeletionConfirmation : UserControl
    {
        private AsyncOperation asyncOperation;
        private Flyout flyout;

        public DeletionConfirmation(string message)
        {
            this.InitializeComponent();

            this.deletionConfirmationText.Text = message;
        }

        private void delete_click(object sender, RoutedEventArgs e)
        {
            flyout.Closed -= flyout_Closed;
            flyout.Hide();
            asyncOperation.SetResults(true);
            asyncOperation.Completed(asyncOperation, AsyncStatus.Completed);
        }

        public IAsyncOperation<bool> ShowDeleteConfirmationAsync(FrameworkElement placementTarget)
        {
            asyncOperation = new AsyncOperation();
            flyout = new Flyout();
            flyout.Content = this;
            flyout.Closed += flyout_Closed;
            flyout.ShowAt(placementTarget);
            return asyncOperation;
        }

        void flyout_Closed(object sender, object e)
        {
            asyncOperation.SetResults(false);
            asyncOperation.Completed(asyncOperation, AsyncStatus.Completed);
        }

        private class AsyncOperation : IAsyncOperation<bool>
        {

            public AsyncOperationCompletedHandler<bool> Completed
            {
                get; set;
            }

            private bool? result = null;


            public void SetResults(bool value)
            {
                result = value;
            }


            public bool GetResults()
            {
                return result.Value;
            }

            public void Cancel()
            {
                throw new NotImplementedException();
            }

            public void Close()
            {
                throw new NotImplementedException();
            }

            public Exception ErrorCode
            {
                get { throw new NotImplementedException(); }
            }

            public uint Id
            {
                get { throw new NotImplementedException(); }
            }

            public AsyncStatus Status
            {
                get 
                {
                    if (result.HasValue)
                    {
                        return AsyncStatus.Completed;
                    }
                    else
                    {
                        return AsyncStatus.Started;
                    }
                }
            }
        }
    }
}
