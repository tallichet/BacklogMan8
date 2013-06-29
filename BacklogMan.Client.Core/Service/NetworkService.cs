using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.Service
{
    public class NetworkService : INetworkService
    {
        private Uri BacklogManApiBaseUri = new Uri("https://app.backlogman.com/api/");

        private HttpClient client;
        private Dictionary<Type, System.Runtime.Serialization.Json.DataContractJsonSerializer> Seriliazers = new Dictionary<Type,System.Runtime.Serialization.Json.DataContractJsonSerializer>();

        protected HttpClient Client
        {
            get
            {
                if (APIKey == null) 
                    throw new ArgumentNullException("API key not set");
                if (client == null)
                {
                    client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", "token " + APIKey);
                }
                return client;
            }
        }

        public async Task<T> DownloadDocument<T>(Uri url)
        {
            using (var s = await Client.GetStreamAsync(url))
            {
                if (Seriliazers.ContainsKey(typeof(T)) == false) 
                {
                    Seriliazers.Add(typeof(T), new  System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T)));
                }
                var serializer = Seriliazers[typeof(T)];
                return (T)serializer.ReadObject(s);
            }
        }

        public string APIKey { get; set; }


        public Task<List<Model.Project>> DownloadProjects()
        {
            var uri = new Uri(BacklogManApiBaseUri, "./projects/");
            return DownloadDocument<List<Model.Project>>(uri);
        }

        public Task<Model.Project> DownloadProject(string projectId)
        {
            var uri = new Uri(BacklogManApiBaseUri, "./projects/" + projectId + "/");
            return DownloadDocument<Model.Project>(uri);
        }

        public Task<List<Model.Backlog>> DownloadBacklogs(string projectId)
        {
            var uri = new Uri(BacklogManApiBaseUri, "./projects/" + projectId + "/backlogs/");
            return DownloadDocument<List<Model.Backlog>>(uri);
        }

        public Task<Model.Backlog> DownloadBacklog(string projectId, string backlogId)
        {
            var uri = new Uri(BacklogManApiBaseUri, "./projects/" + projectId + "/backlogs/" + backlogId + "/");
            return DownloadDocument<Model.Backlog>(uri);
        }

        public Task<List<Model.Story>> DownloadStories(string projectId, string backlogId)
        {
            var uri = new Uri(BacklogManApiBaseUri, "./projects/" + projectId + "/backlogs/" + backlogId + "/stories/");
            return DownloadDocument<List<Model.Story>>(uri);
        }

        public Task<Model.Story> DownloadStory(string projectId, string backlogId, string storyId)
        {
            var uri = new Uri(BacklogManApiBaseUri, "./projects/" + projectId + "/backlogs/" + backlogId + "/stories/" + storyId + "/");
            return DownloadDocument<Model.Story>(uri);        
        }
    }
}
