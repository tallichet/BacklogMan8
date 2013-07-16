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
            Organizations = new System.Collections.ObjectModel.ObservableCollection<Model.Organization>();
            Projects = new System.Collections.ObjectModel.ObservableCollection<Model.Project>();
            ProjectsStandalone = new System.Collections.ObjectModel.ObservableCollection<Model.Project>();
            ProjectBacklogs = new ReorderableCollection<Model.Backlog>();
            ProjectBacklogs.ManualReordered += projectBacklogs_ManualReordered;
            BacklogStories = new ReorderableCollection<Model.Story>();
            BacklogStories.ManualReordered += backlogStories_ManualReordered;

            NotEstimatedStories = new System.Collections.ObjectModel.ObservableCollection<Model.Story>();

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
                await RefreshNotEstimatedStories();
            }
        }

        public System.Collections.ObjectModel.ObservableCollection<Model.Organization> Organizations
        {
            get;
            private set;
        }

        public System.Collections.ObjectModel.ObservableCollection<Model.Project> ProjectsStandalone
        {
            get;
            private set;
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

        private Model.Organization currentOrganization = null;
        public Model.Organization CurrentOrganization
        {
            get
            {
                return currentOrganization;
            }
            set
            {
                currentOrganization = value;
                // Todo: define the refresh to do here
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

        public ReorderableCollection<Model.Backlog> ProjectBacklogs
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

        public ReorderableCollection<Model.Story> BacklogStories
        {
            get;
            private set;
        }

        public System.Collections.ObjectModel.ObservableCollection<Model.Story> NotEstimatedStories
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

            ProjectBacklogs.Clear();
            var project = CurrentProject; // this allow us to be sure the project don't change during the download
            var backlogs = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadBacklogs(CurrentProject.Id);
            foreach (var b in backlogs)
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

        private async Task RefreshNotEstimatedStories()
        {
            NotEstimatedStories.Clear();

            foreach (var p in Projects)
            {
                var backlogs = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadBacklogs(p.Id);
                foreach (var b in backlogs)
                {
                    var stories = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadStories(p.Id, b.Id);
                    foreach (var s in stories)
                    {
                        if (s.Points < 0)
                        {
                            NotEstimatedStories.Add(s);
                        }
                    }
                }
            }
        }

        #region events
        private async void projectBacklogs_ManualReordered(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var movedBacklog = e.NewItems[0] as Model.Backlog;

            if (movedBacklog == null) return;

            var operationResult = await ServiceLocator.Current.GetInstance<Service.INetworkService>().OrderBacklogInProject(this.CurrentProject.Id, movedBacklog.Id,
                                        ProjectBacklogs.Select(b => b.Id).ToArray());

            if (Debugger.IsAttached)
            {
                Debug.WriteLine("moved backlog '{0}' operation resut is {1}", movedBacklog.Name, operationResult);
            }

            if (operationResult)
            {
                ServiceLocator.Current.GetInstance<IInternalNotificationViewModel>().ShowNotificationText("backlog reordered");
            }
            else
            {
                ServiceLocator.Current.GetInstance<IInternalNotificationViewModel>().ShowNotificationText("error when ordering backlogs");
            }
        }

        private async void backlogStories_ManualReordered(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var movedStory = e.NewItems[0] as Model.Story;

            if (movedStory == null) return;

            var operationResult = await ServiceLocator.Current.GetInstance<Service.INetworkService>().MoveStory(
                                    movedStory.Backlog.Project.Id, 
                                    movedStory.Backlog.Id,
                                    movedStory.Id,
                                    BacklogStories.Select(s => s.Id).ToArray());

            if (Debugger.IsAttached)
            {
                Debug.WriteLine("moved stories '{0}' operation resut is {1}", movedStory.Code, operationResult);
            }

            if (operationResult)
            {
                ServiceLocator.Current.GetInstance<IInternalNotificationViewModel>().ShowNotificationText("story moved");
            }
            else
            {
                ServiceLocator.Current.GetInstance<IInternalNotificationViewModel>().ShowNotificationText("error when ordering stories");
            }
        }
        #endregion
    }
}
