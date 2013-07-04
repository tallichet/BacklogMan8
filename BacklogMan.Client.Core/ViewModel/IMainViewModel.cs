using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.ViewModel
{
    public interface IMainViewModel
    {
        /// <summary>
        /// List of all projects
        /// </summary>
        ObservableCollection<Model.Project> Projects { get; }

        /// <summary>
        /// Api key used to login on backlog man
        /// </summary>
        string ApiKey { get; set; }

        /// <summary>
        /// The currently selected project
        /// </summary>
        Model.Project CurrentProject { get; set; }

        /// <summary>
        /// Backlog of the current project
        /// </summary>
        ObservableCollection<Model.Backlog> ProjectBacklogs { get; }

        /// <summary>
        /// The currently selected backlog
        /// </summary>
        Model.Backlog CurrentBacklog { get; set; }

        /// <summary>
        /// Stories of the current backlog
        /// </summary>
        ObservableCollection<Model.Story> BacklogStories { get; }

        /// <summary>
        /// Try to get a new API Key
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        Task<bool> GetApiKey(string Username, string Password);

        void RefreshBacklogStories();
    }
}
