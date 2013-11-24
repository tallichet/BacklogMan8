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
                
                IsInProgress = true;
                try
                {
                    await downloadOrganizations();
                    await downloadProjects();
                }
                catch (Exception ex)
                {
                    ServiceLocator.Current.GetInstance<IInternalNotificationViewModel>().ShowNotificationForKey("ErrorNotificationTitleDownloadProject");
                    if (Debugger.IsAttached)
                    {
                        Debug.WriteLine("Error on Main.ViewModeldownloadOrganizations(): " + ex.Message);
                        Debugger.Break();
                    }
                }                
                IsInProgress = false;
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
                var o2 = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadOrganization(o.Id);
                this.Organizations.Add(o2);
            }

            await refreshMainBacklogs();
        }

        private async Task downloadProjects()
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

        private bool isInProgress;
        public bool IsInProgress
        {
            get
            {
                return isInProgress;
            }
            private set
            {
                if (value != isInProgress)
                {
                    isInProgress = value;
                    this.RaisePropertyChanged(() => this.IsInProgress);
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
                if (value == null)
                {
                    currentOrganization = null;
                    OrganizationBacklogs.Clear();
                    OrganizationProjects.Clear();
                }
                else
                {
                    currentOrganization = value;
                    refreshOrganization();
                }
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
                if (value == null)
                {
                    currentProject = null;
                    ProjectBacklogs.Clear();
                }
                else
                {
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
                if (value == null)
                {
                    currentBacklog = null;
                    BacklogStories.Clear();
                }
                else
                {
                    currentBacklog = value;
                    RefreshBacklogStories();
                }
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

        public void ClearApiKey()
        {
            // Todo : Cancel any current network operation
            ServiceLocator.Current.GetInstance<Service.INetworkService>().APIKey = null;
            ServiceLocator.Current.GetInstance<Service.INetworkService>().ClearCache();
            CurrentBacklog = null;
            CurrentOrganization = null;
            CurrentProject = null;

            MainBacklogs.Clear();
            ProjectsStandalone.Clear();
            Projects.Clear();
            BacklogStories.Clear();
            OrganizationBacklogs.Clear();
            OrganizationProjects.Clear();
            Organizations.Clear();
        }

        private async void refreshBacklogs()
        {
            try
            {
                IsInProgress = true;
                ProjectBacklogs.Clear();
                var project = CurrentProject; // this allow us to be sure the project don't change during the download

                if (project == null) return;

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
            catch (Exception ex)
            {
                ServiceLocator.Current.GetInstance<IInternalNotificationViewModel>().ShowNotificationForKey("ErrorNotificationTitleDownloadProject");
                if (Debugger.IsAttached)
                {
                    Debug.WriteLine("Error on Main.ViewModeldownloadOrganizations(): " + ex.Message);
                    Debugger.Break();
                }
            }
            finally
            {
                IsInProgress = false;
            }
        }

        /// <summary>
        /// Refresh the main backlogs. Get the organzations data to get all the main backlogs
        /// </summary>
        private async Task refreshMainBacklogs()
        {
            var backlogTuples = (from o in Organizations
                           from b in o.Backlogs
                           where b.IsMain
                           select new Tuple<Model.Organization, Model.Backlog> (o, b)).ToList();

            MainBacklogs.Clear();
            foreach (var tuple in backlogTuples)
            {
                try
                {
                    var backlog = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadBacklog(tuple.Item2.Id);
                    backlog.Organization = tuple.Item1;
                    MainBacklogs.Add(backlog);
                }
                catch (Exception)
                {
                    MainBacklogs.Add(tuple.Item2);
                }
            }


        }

        private async void refreshOrganization()
        {
            var wasInProgress = IsInProgress;
            IsInProgress = true;
            await refreshOrganizationBacklogs();
            await refreshOrganizationProjects();
            
            if (!wasInProgress)
                IsInProgress = false;
        }

        private async Task refreshOrganizationBacklogs()
        {
            OrganizationBacklogs.Clear();
            var org = CurrentOrganization;

            if (org == null) return;

            foreach (var b in org.Backlogs)
            {
                try
                {
                    if (b.IsArchive) continue;

                    var backlog = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadBacklog(b.Id);
                    backlog.Organization = org;

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

        private async Task refreshOrganizationProjects()
        {
            OrganizationProjects.Clear();
            var org = CurrentOrganization;

            if (org == null) return;

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
            try
            {
                BacklogStories.Clear();

                var backlog = CurrentBacklog; // this allow us to be sure the backlog don't change during the download

                if (backlog == null) return;
                var stories = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadStories(CurrentBacklog.Id);
                foreach (var s in stories)
                {
                    BacklogStories.Add(s);
                    s.Backlog = backlog;
                }
            }
            catch (Exception ex)
            {
                ServiceLocator.Current.GetInstance<IInternalNotificationViewModel>().ShowNotificationForKey("ErrorNotificationTitleDownloadProject");
                if (Debugger.IsAttached)
                {
                    Debug.WriteLine("Error on Main.ViewModeldownloadOrganizations(): " + ex.Message);
                    Debugger.Break();
                }
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

        public async Task<bool> UpdateStory(Model.Story updateStory)
        {
            try
            {
                IsInProgress = true;
                int result;
                checkNotNullFields(updateStory);

                if (updateStory.Id <= 0)
                {
                    result = await ServiceLocator.Current.GetInstance<Service.INetworkService>().AddStory(CurrentBacklog.Id, updateStory);
                    if (result >= 0)
                    {
                        var story = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadStory(CurrentBacklog.Id, result);
                        BacklogStories.Add (story);
                        story.Backlog = CurrentBacklog;
                    }
                }
                else
                {
                    result = await ServiceLocator.Current.GetInstance<Service.INetworkService>().UpdateStory(updateStory);
                    if (result >= 0)
                    {
                        var story = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadStory(updateStory.Backlog.Id, updateStory.Id);
                        BacklogStories.ReplaceItemById(story);
                        story.Backlog = updateStory.Backlog;
                    }
                }
            
            
                IsInProgress = false;
                return result > -1;
            }
            catch (Exception ex)
            {
                ServiceLocator.Current.GetInstance<IInternalNotificationViewModel>().ShowNotificationForKey("ErrorNotificationTitleDownloadProject");
                if (Debugger.IsAttached)
                {
                    Debug.WriteLine("Error on Main.ViewModeldownloadOrganizations(): " + ex.Message);
                    Debugger.Break();
                }
                return false;
            }
        }
        
        public async Task<bool> SetStoriesStatus(Model.Story story, Model.StoryStatus newStatus)
        {
            try
            {
                IsInProgress = true;
                var result = await ServiceLocator.Current.GetInstance<Service.INetworkService>().UpdateStoryStatus(story.Id, newStatus);
                if (result)
                {
                    story.Status = newStatus;
                }
                IsInProgress = false;
                return result;
            }
            catch (Exception ex)
            {
                ServiceLocator.Current.GetInstance<IInternalNotificationViewModel>().ShowNotificationForKey("ErrorNotificationTitleDownloadProject");
                if (Debugger.IsAttached)
                {
                    Debug.WriteLine("Error on Main.ViewModeldownloadOrganizations(): " + ex.Message);
                    Debugger.Break();
                }
                return false;
            }
            
        }
        
        public async Task<bool> SetStoryPoints(Model.Story story, int points)
        {
            try
            {
                IsInProgress = true;
                var prevPoints = story.Points;
                story.Points = points;
                var storyId = await ServiceLocator.Current.GetInstance<Service.INetworkService>().UpdateStory(story);
                IsInProgress = false;
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
            catch (Exception ex)
            {
                ServiceLocator.Current.GetInstance<IInternalNotificationViewModel>().ShowNotificationForKey("ErrorNotificationTitleDownloadProject");
                if (Debugger.IsAttached)
                {
                    Debug.WriteLine("Error on Main.ViewModeldownloadOrganizations(): " + ex.Message);
                    Debugger.Break();
                }
                return false;
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

        #region Commands
        private RelayCommand refreshBacklogCommand;
        private RelayCommand refreshHomeCommand;
        private RelayCommand refreshProjectCommand;
        private RelayCommand refreshOrganizationCommand;

        public System.Windows.Input.ICommand RefreshBacklogCommand
        {
            get
            {
                if (refreshBacklogCommand == null)
                {
                    refreshBacklogCommand = new RelayCommand(async () =>
                    {
                        try
                        {
                            IsInProgress = true;
                            if (CurrentBacklog != null)
                            {
                                var id = CurrentBacklog.Id;
                                CurrentBacklog = null;
                                CurrentBacklog = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadBacklog(id);
                            }
                            IsInProgress = false;
                        }
                        catch (Exception ex)
                        {
                            ServiceLocator.Current.GetInstance<IInternalNotificationViewModel>().ShowNotificationForKey("ErrorNotificationTitleDownloadProject");
                            if (Debugger.IsAttached)
                            {
                                Debug.WriteLine("Error on Main.ViewModeldownloadOrganizations(): " + ex.Message);
                                Debugger.Break();
                            }
                        }
                    });
                }
                return refreshBacklogCommand;
            }
        }

        public System.Windows.Input.ICommand RefreshHomeCommand
        {
            get
            {
                if (refreshHomeCommand == null)
                {
                    refreshHomeCommand = new RelayCommand(() =>
                    {
                        this.Organizations.Clear();
                        this.Projects.Clear();
                        this.ProjectsStandalone.Clear();
                        this.MainBacklogs.Clear();

                        Init();
                    });
                }
                return refreshHomeCommand;
            }
        }

        public System.Windows.Input.ICommand RefreshProjectCommand
        {
            get
            {
                if (refreshProjectCommand == null)
                {
                    refreshProjectCommand = new RelayCommand(async () =>
                    {
                        IsInProgress = true;
                        CurrentProject = null;
                        CurrentProject = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadProject(CurrentProject.Id);
                        IsInProgress = false;
                    });
                }
                return refreshProjectCommand;
            }
        }

        public System.Windows.Input.ICommand RefreshOrganzationCommand
        {
            get 
            {
                if (refreshOrganizationCommand == null)
                {
                    refreshOrganizationCommand = new RelayCommand(async () =>
                    {
                        IsInProgress = true;
                        CurrentOrganization = null;
                        CurrentOrganization = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadOrganization(CurrentOrganization.Id);
                        IsInProgress = false;
                    });
                }
                return refreshOrganizationCommand;
            }
        }

        #endregion

        #region helpers
        private void checkNotNullFields(Model.Story story)
        {
            if (story.AcceptanceCriteria == null) story.AcceptanceCriteria = "";
            if (story.Comments == null) story.Comments = "";
            if (story.Theme == null) story.Theme = "";
            if (story.ColorString == null) story.ColorString = "";
            if (story.AsUser == null) story.AsUser = "";
            if (story.Goal == null) story.Goal = "";
            if (story.Result == null) story.Result = "";
        }
        #endregion


    }
}
