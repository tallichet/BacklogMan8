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
        Task<Model.Project> DownloadProject(int projectId);

        Task<List<Model.Backlog>> DownloadBacklogs(int projectId);
        Task<Model.Backlog> DownloadBacklog(int projectId, int backlogId);

        Task<List<Model.Story>> DownloadStories(int projectId, int backlogId);
        Task<Model.Story> DownloadStory(int projectId, int backlogId, int stroyId);
    }
}
