using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.ViewModel
{
    public interface ISearchViewModel
    {
        ObservableCollection<Core.Model.Organization> MatchingOrganizations { get; }
        ObservableCollection<Core.Model.Project> MatchingProjects { get; }
        ObservableCollection<Core.Model.Backlog> MatchingBacklogs { get; }

        Task SearchAll(string criteria);

        string Criteria { get; }

        int ResultCount { get; }

        bool InProgress { get; }

        void CancelSearch();

    }
}
