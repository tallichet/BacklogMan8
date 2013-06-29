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
            ApiKey = "d6e4dc02f5cd443a3a480ae5debd6ddf7bec0707";

            Init();
        }

        private async void Init()
        {
            await DownloadProjects();
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

        private async void refreshStories()
        {
            BacklogStories.Clear();
            var stories = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadStories(CurrentProject.Id, CurrentBacklog.id);
            foreach (var s in stories)
            {
                BacklogStories.Add(s);
            }
        }
    }
}
