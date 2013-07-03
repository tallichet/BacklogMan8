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
            BacklogStories = new System.Collections.ObjectModel.ObservableCollection<Model.Story>();

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


        public Model.Project CurrentProject
        {
            get;set;
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
                refreshStories();
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
                SaveApiKey(this.ApiKey);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async void refreshStories()
        {
            BacklogStories.Clear();
            var stories = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadStories(CurrentProject.Id, CurrentBacklog.Id);
            foreach (var s in stories)
            {
                BacklogStories.Add(s);
            }
        }

        private void SaveApiKey(string apiKey)
        {
            // TODO: save the key for both Win8 and WinPhone8
        }

        private string LoadApiKey()
        {
            // TODO: load the key for both Win8 and WinPhone8
            return null;
        }
    }
}
