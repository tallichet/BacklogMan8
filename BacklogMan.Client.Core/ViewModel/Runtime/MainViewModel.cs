﻿using GalaSoft.MvvmLight;
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
            OrganizationBacklogs = new ReorderableCollection<Model.Backlog>();
            OrganizationBacklogs.ManualReordered += organizationBacklogs_ManualReordered;
            BacklogStories = new ReorderableCollection<Model.Story>();
            BacklogStories.ManualReordered += backlogStories_ManualReordered;
            
            MainBacklogs = new System.Collections.ObjectModel.ObservableCollection<Model.Backlog>();

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
                await downloadOrganizations();
                await DownloadProjects();
                //await RefreshNotEstimatedStories();
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

        private async Task downloadOrganizations()
        {
            this.Organizations.Clear();

            var organizations = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadOrganizations();
            foreach (var o in organizations)
            {
                var o2 = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadOrganization(o.Id.ToString());
                this.Organizations.Add(o2);
            }

            await refreshMainBacklogs();
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
                refreshOrganizationBacklogs();
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

        public ReorderableCollection<Model.Backlog> OrganizationBacklogs
        {
            get;
            private set;
        }

        public System.Collections.ObjectModel.ObservableCollection<Model.Backlog> MainBacklogs
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
                    if (b.IsArchive) continue;

                    var backlog = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadBacklog(b.Id);
                                        
                    if (b.IsMain)
                    {
                        ProjectBacklogs.Insert(0, backlog);
                    }
                    else
                    {
                        ProjectBacklogs.Add(backlog);
                    }
                    backlog.Project = project;
                }
            }
            catch (Exception)
            {
                ServiceLocator.Current.GetInstance<IInternalNotificationViewModel>().ShowNotificationForKey("ErrorNotificationTitleDownloadProject");
            }
        }

        /// <summary>
        /// Refresh the main backlogs. Get the organzations data to get all the main backlogs
        /// </summary>
        private async Task refreshMainBacklogs()
        {
            var backlogs = (from o in Organizations
                           from b in o.Backlogs
                           where b.IsMain
                           select b).ToList();

            MainBacklogs.Clear();
            foreach (var b in backlogs)
            {
                try
                {
                    var backlog = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadBacklog(b.Id);
                    MainBacklogs.Add(backlog);
                }
                catch (Exception)
                {
                    MainBacklogs.Add(b);
                }
            }


        }

        private async void refreshOrganizationBacklogs()
        {
            OrganizationBacklogs.Clear();
            var org = CurrentOrganization;
            foreach (var b in org.Backlogs)
            {
                try
                {
                    if (b.IsArchive) continue;

                    var backlog = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadBacklog(b.Id);

                    if (b.IsMain)
                    {
                        OrganizationBacklogs.Insert(0, backlog);
                    }
                    else
                    {
                        OrganizationBacklogs.Add(backlog);
                    }
                }
                catch (Exception)
                {
                    // Maybe we do not have access to this projects
                    OrganizationBacklogs.Add(b);
                }
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

            var backlog = CurrentBacklog; // this allow us to be sure the backlog don't change during the download
            var stories = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadStories(CurrentBacklog.Id);
            foreach (var s in stories)
            {
                BacklogStories.Add(s);
                s.Backlog = backlog;
            }
        }
        public void DeleteStories(Model.Story[] storiesToDelete)
        {
            foreach (var s in storiesToDelete)
            {
                ServiceLocator.Current.GetInstance<Core.Service.INetworkService>().DeleteStory(s);
            }

            this.RefreshBacklogStories();
        }
        
        public async Task<bool> SetStoriesStatus(Model.Story story, Model.StoryStatus newStatus)
        {
            var result = await ServiceLocator.Current.GetInstance<Service.INetworkService>().UpdateStoryStatus(story.Id, newStatus);
            if (result)
            {
                story.Status = newStatus;
            }
            return result;
        }
        
        public async Task<bool> SetStoryPoints(Model.Story story, int points)
        {
            var prevPoints = story.Points;
            story.Points = points;
            var storyId = await ServiceLocator.Current.GetInstance<Service.INetworkService>().UpdateStory(story);
            if (storyId < 0)
            {
                story.Points = prevPoints;
                return false;
            }
            else
            {
                return true;
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

        private async void organizationBacklogs_ManualReordered(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var movedBacklog = e.NewItems[0] as Model.Backlog;

            if (movedBacklog == null) return;

            var operationResult = await ServiceLocator.Current.GetInstance<Service.INetworkService>().OrderBacklogInOrganization(this.CurrentOrganization.Id, movedBacklog.Id,
                                        OrganizationBacklogs.Select(b => b.Id).ToArray());

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

            var operationResult = await ServiceLocator.Current.GetInstance<Service.INetworkService>().MoveStory(movedStory.Backlog.Id, movedStory.Id, BacklogStories.Select(s => s.Id).ToArray());

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
