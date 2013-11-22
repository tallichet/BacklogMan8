using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.ViewModel.Runtime
{
    public class SearchViewModel : ViewModelBase, ISearchViewModel
    {
        private List<Model.Organization> allOrganizations;
        private List<Model.Project> allProjects;
        private List<Model.Backlog> allBacklogs;

        public SearchViewModel()
        {
            MatchingOrganizations = new System.Collections.ObjectModel.ObservableCollection<Model.Organization>();
            MatchingProjects = new System.Collections.ObjectModel.ObservableCollection<Model.Project>();
            MatchingBacklogs = new System.Collections.ObjectModel.ObservableCollection<Model.Backlog>();
        }

        public void ClearResults()
        {
            MatchingOrganizations.Clear();
            MatchingProjects.Clear();
            MatchingBacklogs.Clear();
        }

        public async Task SearchAll(string criteria)
        {
            try
            {
                Criteria = criteria;
                this.RaisePropertyChanged(() => Criteria);

                ClearResults();

                allOrganizations = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadOrganizations();
                foreach (var org in allOrganizations)
                {
                    if (org.Name.IndexOf(criteria, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        MatchingOrganizations.Add(org);
                        RaisePropertyChanged("ResultCount");
                    }
                }

                allProjects = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadProjects();
                foreach (var prj in allProjects)
                {
                    if (prj.Name.IndexOf(criteria, StringComparison.OrdinalIgnoreCase) > -1
                     || prj.Code.IndexOf(criteria, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        MatchingProjects.Add(prj);
                        RaisePropertyChanged("ResultCount");
                    }
                }

                allBacklogs = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadBacklogs();
                foreach (var backlog in allBacklogs)
                {
                    if (backlog.Name.IndexOf(criteria, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        MatchingBacklogs.Add(backlog);
                        RaisePropertyChanged("ResultCount");
                    }
                }
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                {
                    Debug.WriteLine("exception on search: " + ex.Message);
                    Debugger.Break();
                }
            }
        }

        public System.Collections.ObjectModel.ObservableCollection<Model.Organization> MatchingOrganizations
        {
            get; 
            private set;
        }

        public System.Collections.ObjectModel.ObservableCollection<Model.Project> MatchingProjects
        {
            get;
            private set;
        }

        public System.Collections.ObjectModel.ObservableCollection<Model.Backlog> MatchingBacklogs
        {
            get;
            private set;
        }

        public string Criteria { get; private set; }

        public int ResultCount
        {
            get
            {
                return MatchingBacklogs.Count + MatchingOrganizations.Count + MatchingProjects.Count;
            }
        }
    }
}
