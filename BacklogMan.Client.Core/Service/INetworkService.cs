using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.Service
{
    public interface INetworkService
    {
        Task<T> DownloadDocument<T>(Uri url);
        string APIKey { get; set; }

        Task<List<Model.Project>> DownloadProjects();
        Task<Model.Project> DownloadProject(string projectId);

        Task<List<Model.Backlog>> DownloadBacklogs(string projectId);
        Task<Model.Backlog> DownloadBacklog(string projectId, string backlogId);

        Task<List<Model.Story>> DownloadStories(string projectId, string backlogId);
        Task<Model.Story> DownloadStory(string projectId, string backlogId, string stroyId);
    }
}
