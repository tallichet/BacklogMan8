using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.ViewModel.Design
{
    public class DesignSearchViewModel : ISearchViewModel
    {
        public DesignSearchViewModel()
        {
            #region define organizations
            MatchingOrganizations = new System.Collections.ObjectModel.ObservableCollection<Model.Organization>();
            MatchingOrganizations.Add(new Model.Organization()
            {
                Id = 1,
                Name = "epyx product team",
                Description = "responsable du dev exodoc et exopoint et plus généralement des mobiles",
                Email = "dev@epyx.ch",
                UrlString = "http://apps.backlogman.com/organizations/1",
                WebSite = "http://www.epyx.ch",
                Users = new List<Model.BacklogmanUser>() 
                {
                    new Model.BacklogmanUser() {Email = "ctt@epyx.ch", FullName="Cedric Tallichet"},
                    new Model.BacklogmanUser() {Email = "dsi@epyx.ch", FullName="David Saradini"}
                },
                Projects = new List<Model.Project>()
                {
                    new Model.Project() {Id = 1, Name = "Project n° 1"},
                    new Model.Project() {Id = 2, Name = "Project n° 2"}
                }
            });
            MatchingOrganizations.Add(new Model.Organization()
            {
                Id = 2,
                Name = "epyx green team",
                Description = "l'équipe à SWF",
                Email = "green@epyx.ch",
                UrlString = "http://apps.backlogman.com/organizations/1",
                WebSite = "http://www.epyx.ch",
                Users = new List<Model.BacklogmanUser>() 
                {
                    new Model.BacklogmanUser() {Email = "ctt@epyx.ch", FullName="Cedric Tallichet"},
                    new Model.BacklogmanUser() {Email = "dsi@epyx.ch", FullName="David Saradini"}
                },
                Projects = new List<Model.Project>()
                {
                    new Model.Project() {Id = 1, Name = "Project n° 1"},
                    new Model.Project() {Id = 2, Name = "Project n° 2"}
                }
            });
            #endregion
            #region define projects
            MatchingProjects = new System.Collections.ObjectModel.ObservableCollection<Model.Project>();
            MatchingProjects.Add(new Model.Project()
            {
                Name = "Project n° 1",
                Description = "This is my first project!",
                Users = new List<Model.BacklogmanUser>()
                {
                    new Model.BacklogmanUser() {FullName = "cedric", Email = "cedric@suisse.ch"},
                    new Model.BacklogmanUser() {FullName = "david", Email = "david@suisse.ch"},
                }
            });
            MatchingProjects.Add(new Model.Project()
            {
                Name = "Project n° 2",
                Description = "This is my second project!",
                Users = new List<Model.BacklogmanUser>()
                {                    
                    new Model.BacklogmanUser() {FullName = "david", Email = "david@suisse.ch"},
                    new Model.BacklogmanUser() {FullName = "cedric", Email = "cedric@suisse.ch"},
                }
            });
            MatchingProjects.Add(new Model.Project()
            {
                Name = "Project n° 3",
                Description = "This is my first standalone project!",
                Users = new List<Model.BacklogmanUser>()
                {
                    new Model.BacklogmanUser() {FullName = "cedric", Email = "cedric@suisse.ch"},
                    new Model.BacklogmanUser() {FullName = "david", Email = "david@suisse.ch"},
                }
            });            
            #endregion
            #region define backlogs
            MatchingBacklogs = new ObservableCollection<Model.Backlog>();

            MatchingBacklogs.Add(new Model.Backlog()
            {
                Name = "Backlog n° 1",
                Description = "This is my first project!",
                Project = MatchingProjects.First(),
                Themes = new string[] { "Windows", "Windows Phone" }.ToList(),
            });
            MatchingBacklogs.Add(new Model.Backlog()
            {
                Name = "Backlog n° 2",
                Description = "This is my second project!",
                Project = MatchingProjects.First(),
                Themes = new string[] { "Windows", "Windows Phone" }.ToList(),
            });
            #endregion
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

        public Task SearchAll(string criteria)
        {
            throw new NotImplementedException();
        }

        public string Criteria
        {
            get { return "my search"; }
        }

        public int ResultCount
        {
            get { return 12; }
        }
    }
}
