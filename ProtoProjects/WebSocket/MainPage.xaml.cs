using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Sockets;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WebSocket
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        MessageWebSocket ws = new MessageWebSocket();
            
        public MainPage()
        {
            Notifications = new ObservableCollection<string>();
            this.InitializeComponent();
            Client();
        }

        public ObservableCollection<string> Notifications { get; private set; }

        private async void Client()
        {
            ws.Control.MessageType = SocketMessageType.Utf8;
            ws.MessageReceived += (sender, args) =>
            {
                var reader = args.GetDataReader();
                var message = reader.ReadString(reader.UnconsumedBufferLength);
                Debug.WriteLine(message);
            };
            ws.Closed += (sender, args) =>
            {
                ws.Dispose();
            };
            var uri = new Uri("wss://app.backlogman.com/ws/Project/2/");
            await ws.ConnectAsync(uri);
        }

    }
}
