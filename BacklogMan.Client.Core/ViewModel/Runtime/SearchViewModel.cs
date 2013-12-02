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

        private int inProgressCount;
        private bool cancelSearch;

        public SearchViewModel()
        {
            MatchingOrganizations = new System.Collections.ObjectModel.ObservableCollection<Model.Organization>();
            MatchingProjects = new System.Collections.ObjectModel.ObservableCollection<Model.Project>();
            MatchingBacklogs = new System.Collections.ObjectModel.ObservableCollection<Model.Backlog>();

            allBacklogs = new List<Model.Backlog>();
            inProgressCount = 0;
        }

        public bool InProgress
        {
            get { return inProgressCount != 0; }
            set
            {
                if (value)
                {
                    inProgressCount += 1;
                }
                else
                {
                    inProgressCount -= 1;
                    if (inProgressCount < 0) inProgressCount = 0;
                }
                if (inProgressCount < 2) // logiquement, on est passé de 1 à 0 ou de 0 à 1...
                    RaisePropertyChanged(() => InProgress);
            }
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
                cancelSearch = false;
                InProgress = true;
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
                if (cancelSearch) return;

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

                if (cancelSearch) return;

                #region backlogs
                allBacklogs.Clear();

                foreach (var org in allOrganizations)
                {
                    if (org.Backlogs == null)
                    {
                        var orgfull = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadOrganization(org.Id);
                        searchBacklogs(orgfull.Backlogs.Select(b => b.Id).AsEnumerable(), criteria);
                    }
                    else
                    {
                        searchBacklogs(org.Backlogs.Select(b => b.Id).AsEnumerable(), criteria);
                    }
                    if (cancelSearch) return;
                }

                foreach (var prj in allProjects)
                {
                    if (prj.Backlogs == null)
                    {
                        var prjfull = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadProject(prj.Id);
                        searchBacklogs(prjfull.Backlogs.Select(b => b.Id).AsEnumerable(), criteria);
                    }
                    else
                    {
                        searchBacklogs(prj.Backlogs.Select(b => b.Id).AsEnumerable(), criteria);
                    }
                    if (cancelSearch) return;
                }
                #endregion
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                {
                    Debug.WriteLine("exception on search: " + ex.Message);
                    Debugger.Break();
                }
            }
            finally
            {
                InProgress = false;
            }
        }

        private async void searchBacklogs(IEnumerable<int> backlogsId, string criteria)
        {
            foreach (var id in backlogsId)
            {
                var backlog = await ServiceLocator.Current.GetInstance<Service.INetworkService>().DownloadBacklog(id);
                allBacklogs.Add(backlog);

                if (backlog.Name.IndexOf(criteria, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    MatchingBacklogs.Add(backlog);
                    RaisePropertyChanged("ResultCount");
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


        public void CancelSearch()
        {
            cancelSearch = true;
        }
    }
}
