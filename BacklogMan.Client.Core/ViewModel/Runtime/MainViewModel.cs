using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.ViewModel.Runtime
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {

        public MainViewModel()
        {
            Projects = new System.Collections.ObjectModel.ObservableCollection<Model.Project>();
            ApiKey = "d6e4dc02f5cd443a3a480ae5debd6ddf7bec0707".ToUpper();

            Init();
        }

        private async void Init()
        {
            await DownloadProjects();
        }

        public System.Collections.ObjectModel.ObservableCollection<Model.Project> Projects
        {
            get;
            private set;
        }

        public string ApiKey
        {
            get; 
            set;
        }

        private async Task DownloadProjects()
        {
            try
            {
                var client = new HttpClient();

                //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("token", ApiKey);

                client.DefaultRequestHeaders.Add("Authorization", "token d6e4dc02f5cd443a3a480ae5debd6ddf7bec0707");

                using (var stream = await client.GetStreamAsync("https://app.backlogman.com/api/projects/"))
                {
                    Projects.Clear();

                    var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<Model.Project>));
                    foreach (var p in serializer.ReadObject(stream) as List<Model.Project>)
                    {
                        Projects.Add(p);
                    }
                }
            }
            catch (WebException webEx)
            {
                Debug.WriteLine(new StreamReader(webEx.Response.GetResponseStream()).ReadToEnd());
            }
            catch (HttpRequestException httpEx)
            {
                Debug.WriteLine(httpEx.StackTrace);
            }
        }
    }
}
