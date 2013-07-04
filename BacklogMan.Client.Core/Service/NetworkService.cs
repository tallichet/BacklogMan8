using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Dictionary<Type, System.Runtime.Serialization.Json.DataContractJsonSerializer> Seriliazers = new Dictionary<Type, System.Runtime.Serialization.Json.DataContractJsonSerializer>();

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

            var responseData = await PostData<Internal.GetApiTokenRequestContent, Internal.GetApiTokenResponseContent>(uri, new Internal.GetApiTokenRequestContent() { Username = username, Password = password });

            return responseData.ApiToken;
        }

        public async Task<int> AddStory(int projectId, int backlogId, Model.Story newStory)
        {
            try
            {
                var uri = new Uri(BacklogManApiBaseUri, "./projects/" + projectId + "/backlogs/" + backlogId + "/stories/");
                var result = await PostData<Model.Story, Model.Story>(uri, newStory);
                return result.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<bool> MoveStory(int projectId, int targetBacklogId, int movedStoryId, int[] storyIdOrder)
        {
            var uri = new Uri(BacklogManApiBaseUri, "./project/" + projectId + "/_move_story/");

            var request = new Internal.MoveStoryRequest()
            {
                MovedStory = movedStoryId,
                TargetBacklog = targetBacklogId,
                StoryOrder = storyIdOrder
            };

            var result = await PostData<Internal.MoveStoryRequest, Internal.Result>(uri, request);

            return result.Ok;
        }

        public async Task<bool> OrderBacklog(int projectId, int movedBacklog, int[] backlogIdOrder)
        {
            var uri = new Uri(BacklogManApiBaseUri, "./project/" + projectId + "/_order_backlog/");

            var request = new Internal.OrderBacklogRequest()
            {
                MovedBacklog = movedBacklog,
                BacklogOrder = backlogIdOrder
            };

            var result = await PostData<Internal.OrderBacklogRequest, Internal.Result>(uri, request);

            return result.Ok;
        }

        #region Network methods
        private async Task<T> DownloadDocument<T>(Uri url)
        {
            using (var s = await Client.GetStreamAsync(url))
            {
                if (Seriliazers.ContainsKey(typeof(T)) == false)
                {
                    Seriliazers.Add(typeof(T), new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T)));
                }
                var serializer = Seriliazers[typeof(T)];
                return (T)serializer.ReadObject(s);
            }
        }

        private async Task<R> PostData<T, R>(Uri uri, T objectToSend)
        {
            var content = new StringContent(
                Helper.Serialize<T>(objectToSend),
                Encoding.UTF8,
                "application/json");

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
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("Response (" + response.StatusCode.ToString() + ") Content: " + responseContent);

                throw new Exception("Unknwon error");
            }

            using (var answerStream = await response.Content.ReadAsStreamAsync())
            {
                var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(R));

                return (R)serializer.ReadObject(answerStream);
            }
        }

        #endregion
    }
}
