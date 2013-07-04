using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
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
            ProjectBacklogs = new System.Collections.ObjectModel.ObservableCollection<Model.Backlog>();
            BacklogStories = new System.Collections.ObjectModel.ObservableCollection<Model.Story>();

            string apiKey;
            if (ServiceLocator.Current.GetInstance<Service.IStorageService>().TryLoadSetting<string>("ApiKey", out apiKey))
            {
                // Do not use the global setter to prevent clear + init method calls
                ServiceLocator.Current.GetInstance<Service.INetworkService>().APIKey = apiKey;
            }

            Init();
        }

        private async void Init()
        {
            if (!string.IsNullOrEmpty(ApiKey))
            {
                await DownloadProjects();
            }
        }

        public System.Collections.ObjectModel.ObservableCollection<Model.Project> Projects
        {
            get;
            private set;
        }

        public string ApiKey
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Service.INetworkService>().APIKey;
            }
            set 
            {
                ServiceLocator.Current.GetInstance<Service.INetworkService>().APIKey = value;
                ServiceLocator.Current.GetInstance<Service.INetworkService>().ClearCache();
                Init();
            }
        }

        private async Task DownloadProjects()
        {
            this.Projects.Clear();

            var projects = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadProjects();
            foreach (var p in projects)
            {
                this.Projects.Add(p);
            }
        }

        private Model.Project currentProject = null;
        public Model.Project CurrentProject
        {
            get
            {
                return currentProject;
            }
            set
            {
                currentProject = value;
                refreshBacklogs();
            }
        }

        public System.Collections.ObjectModel.ObservableCollection<Model.Backlog> ProjectBacklogs
        {
            get;
            private set;
        }

        private Model.Backlog currentBacklog = null;
        public Model.Backlog CurrentBacklog
        {
            get
            {
                return currentBacklog;
            }
            set
            {
                currentBacklog = value;
                RefreshBacklogStories();
            }
        }

        public System.Collections.ObjectModel.ObservableCollection<Model.Story> BacklogStories
        {
            get;
            private set;
        }

        public async Task<bool> GetApiKey(string username, string password)
        {
            try
            {
                this.ApiKey = await ServiceLocator.Current.GetInstance<Service.INetworkService>().GetApiKey(username, password);
                ServiceLocator.Current.GetInstance<Service.IStorageService>().SaveSetting<string>("ApiKey", this.ApiKey);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async void refreshBacklogs()
        {
<<<<<<< HEAD
            BacklogStories.Clear();
            var stories = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadStories(CurrentProject.Id, CurrentBacklog.Id);
            foreach (var s in stories)
=======
            ProjectBacklogs.Clear();
            var project = CurrentProject; // this allow us to be sure the project don't change during the download
            var backlogs = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadBacklogs(CurrentProject.Id);
            foreach (var b in backlogs)
>>>>>>> cb57972725087eb0cc2d9112c4e3b500c45c16f5
            {
                ProjectBacklogs.Add(b);
                b.Project = project;
            }
        }

        public async void RefreshBacklogStories()
        {
            BacklogStories.Clear();
            var backlog = CurrentBacklog; // this allow us to be sure the backlog don't change during the download
            var stories = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadStories(CurrentProject.Id, CurrentBacklog.Id);
            foreach (var s in stories)
            {
                BacklogStories.Add(s);
                s.Backlog = backlog;
            }
        }
    }
}
