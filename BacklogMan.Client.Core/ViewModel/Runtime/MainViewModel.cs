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

            BacklogStoriesAccepted = new ReorderableCollection<Model.Story>();
            BacklogStoriesCompleted = new ReorderableCollection<Model.Story>();
            BacklogStoriesInProgress = new ReorderableCollection<Model.Story>();
            BacklogStoriesRejected = new ReorderableCollection<Model.Story>();
            BacklogStoriesToDo = new ReorderableCollection<Model.Story>();

            BacklogStories.ManualReordered += backlogStories_ManualReordered;
            OrganizationProjects = new System.Collections.ObjectModel.ObservableCollection<Model.Project>();

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
                await DownloadOrganizations();
                await DownloadProjects();
                await RefreshNotEstimatedStories();
            }
        }

        public System.Collections.ObjectModel.ObservableCollection<Model.Organization> Organizations
        {
            get;
            private set;
        }

        public System.Collections.ObjectModel.ObservableCollection<Model.Project> OrganizationProjects
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

        private async Task DownloadOrganizations()
        {
            this.Organizations.Clear();

            var organizations = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadOrganizations();
            foreach (var o in organizations)
            {
                this.Organizations.Add(o);
            }
        }

        private async Task DownloadProjects()
        {
            this.Projects.Clear();
            this.ProjectsStandalone.Clear();
            var listProjectsToDownload = new List<Model.Project>();

            var projects = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadProjects();
            foreach (var p in projects)
            {
                this.Projects.Add(p);

                // if the project is part of an org, add the project to the org.
                // other wise add the project to the 
                var org = this.Organizations.FirstOrDefault(o => o.Projects != null && o.Projects.Any(op => op.Id == p.Id));
                if (org == null)
                {
                    listProjectsToDownload.Add(p);
                }
            }

            foreach (var p in listProjectsToDownload)
            {
                try
                {
                    ProjectsStandalone.Add(await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadProject(p.Id));
                }
                catch (Exception)
                {
                    ProjectsStandalone.Add(p);
                }
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
                refreshOrganizationProjects();
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
                // if it is an organization project, use the one from the project list
                if (string.IsNullOrEmpty(value.UrlString))
                {
                    currentProject = Projects.First(p => p.Id == value.Id);
                }
                else
                {
                    currentProject = value;
                }
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
            try
            {
                ProjectBacklogs.Clear();
                var project = CurrentProject; // this allow us to be sure the project don't change during the download
                
                foreach (var b in project.Backlogs)
                {
                    ProjectBacklogs.Add(
                        await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadBacklog(b.Id));
                    b.Project = project;
                }
            }
            catch (Exception)
            {
                ServiceLocator.Current.GetInstance<IInternalNotificationViewModel>().ShowNotificationForKey("ErrorNotificationTitleDownloadProject");
            }
        }

        private async void refreshOrganizationProjects()
        {
            OrganizationProjects.Clear();
            var org = CurrentOrganization;
            foreach (var p in org.Projects)
            {
                try
                {
                    OrganizationProjects.Add(
                        await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadProject(p.Id));
                }
                catch (Exception)
                {
                    // Maybe we do not have access to this projects
                    OrganizationProjects.Add(p);
                }
            }
        }

        public async void RefreshBacklogStories()
        {
            BacklogStories.Clear();

            BacklogStoriesAccepted.Clear();
            BacklogStoriesCompleted.Clear();
            BacklogStoriesInProgress.Clear();
            BacklogStoriesRejected.Clear();
            BacklogStoriesToDo.Clear();

            var backlog = CurrentBacklog; // this allow us to be sure the backlog don't change during the download
            var stories = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadStories(CurrentBacklog.Id);
            foreach (var s in stories)
            {
                BacklogStories.Add(s);
                s.Backlog = backlog;

                switch (s.Status)
                {
                    case Model.StoryStatus.Accepted:
                        BacklogStoriesAccepted.Add(s);
                        break;
                    case Model.StoryStatus.Completed:
                        BacklogStoriesCompleted.Add(s);
                        break;
                    case Model.StoryStatus.InProgress:
                        BacklogStoriesInProgress.Add(s);
                        break;
                    case Model.StoryStatus.Rejected:
                        BacklogStoriesRejected.Add(s);
                        break;
                    case Model.StoryStatus.ToDo:
                        BacklogStoriesToDo.Add(s);
                        break;
                }
            }
        }

        private async Task RefreshNotEstimatedStories()
        {
            NotEstimatedStories.Clear();

            foreach (var p in Projects)
            {
                if (p.Backlogs != null)
                {
                    foreach (var b in p.Backlogs)
                    {
                        var stories = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadStories(b.Id);
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


        public void DeleteStories(Model.Story[] storiesToDelete)
        {
            foreach (var s in storiesToDelete)
            {
                ServiceLocator.Current.GetInstance<Core.Service.INetworkService>().DeleteStory(s);
            }

            this.RefreshBacklogStories();
        }

        public ReorderableCollection<Model.Story> BacklogStoriesToDo
        {
            get;
            private set;
        }
        public ReorderableCollection<Model.Story> BacklogStoriesInProgress
        {
            get;
            private set;
        }

        public ReorderableCollection<Model.Story> BacklogStoriesCompleted
        {
            get;
            private set;
        }

        public ReorderableCollection<Model.Story> BacklogStoriesAccepted
        {
            get;
            private set;
        }

        public ReorderableCollection<Model.Story> BacklogStoriesRejected
        {
            get;
            private set;
        }
    }
}
