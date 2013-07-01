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
                if (client == null)
                {
                    client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    if (APIKey != null)
                        client.DefaultRequestHeaders.Add("Authorization", "token " + APIKey);
                }
                return client;
            }
        }

        public void ClearCache()
        {
            client = null;
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

        public Task<Model.Project> DownloadProject(int projectId)
        {
            var uri = new Uri(BacklogManApiBaseUri, "./projects/" + projectId + "/");
            return DownloadDocument<Model.Project>(uri);
        }

        public Task<List<Model.Backlog>> DownloadBacklogs(int projectId)
        {
            var uri = new Uri(BacklogManApiBaseUri, "./projects/" + projectId + "/backlogs/");
            return DownloadDocument<List<Model.Backlog>>(uri);
        }

        public Task<Model.Backlog> DownloadBacklog(int projectId, int backlogId)
        {
            var uri = new Uri(BacklogManApiBaseUri, "./projects/" + projectId + "/backlogs/" + backlogId + "/");
            return DownloadDocument<Model.Backlog>(uri);
        }

        public Task<List<Model.Story>> DownloadStories(int projectId, int backlogId)
        {
            var uri = new Uri(BacklogManApiBaseUri, "./projects/" + projectId + "/backlogs/" + backlogId + "/stories/");
            return DownloadDocument<List<Model.Story>>(uri);
        }

        public Task<Model.Story> DownloadStory(int projectId, int backlogId, int storyId)
        {
            var uri = new Uri(BacklogManApiBaseUri, "./projects/" + projectId + "/backlogs/" + backlogId + "/stories/" + storyId + "/");
            return DownloadDocument<Model.Story>(uri);        
        }


        public async Task<string> GetApiKey(string username, string password)
        {
            var uri = new Uri(BacklogManApiBaseUri, "./api-token-auth/");


            using (var stream = new System.IO.MemoryStream()) 
            {
                var content = new StringContent(
                    Helper.Serialize<Internal.GetApiTokenRequestContent>(new Internal.GetApiTokenRequestContent() { Username = username, Password = password }),
                    Encoding.UTF8,
                    "application/json");
                
                stream.Position = 0;

                var response = await Client.PostAsync(uri, content);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) 
                {
                    throw new Exception("Unauthorized");
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden) 
                {
                    throw new Exception("Forbidden");
                }
                if (response.IsSuccessStatusCode == false) 
                {
                    var reason = response.ReasonPhrase;
                    var answer = await response.Content.ReadAsStringAsync();
                    throw new Exception("Unknwon error");
                }

                using (var answerStream = await response.Content.ReadAsStreamAsync())
                {
                    var serializer2 = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(Internal.GetApiTokenResponseContent));

                    var responseData = serializer2.ReadObject(answerStream) as Internal.GetApiTokenResponseContent;
                    return responseData.ApiToken;
                }
            }
        }
    }
}
