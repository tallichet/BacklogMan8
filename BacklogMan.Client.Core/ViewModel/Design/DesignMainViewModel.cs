using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.ViewModel.Design
{
    public class DesignMainViewModel : IMainViewModel
    {
        public DesignMainViewModel()
        {
            #region define organizations
            Organizations = new System.Collections.ObjectModel.ObservableCollection<Model.Organization>();
            Organizations.Add(new Model.Organization()
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
            Organizations.Add(new Model.Organization()
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
            Projects = new System.Collections.ObjectModel.ObservableCollection<Model.Project>();
            OrganizationProjects = new System.Collections.ObjectModel.ObservableCollection<Model.Project>();
            Projects.Add(new Model.Project()
            {
                Name = "Project n° 1",
                Description = "This is my first project!",
                Users = new List<Model.BacklogmanUser>()
                {
                    new Model.BacklogmanUser() {FullName = "cedric", Email = "cedric@suisse.ch"},
                    new Model.BacklogmanUser() {FullName = "david", Email = "david@suisse.ch"},
                }
            });
            OrganizationProjects.Add(Projects.Last());
            Projects.Add(new Model.Project()
            {
                Name = "Project n° 2",
                Description = "This is my second project!",
                Users = new List<Model.BacklogmanUser>()
                {                    
                    new Model.BacklogmanUser() {FullName = "david", Email = "david@suisse.ch"},
                    new Model.BacklogmanUser() {FullName = "cedric", Email = "cedric@suisse.ch"},
                }
            });
            OrganizationProjects.Add(Projects.Last());
            ProjectsStandalone = new System.Collections.ObjectModel.ObservableCollection<Model.Project>();
            ProjectsStandalone.Add(new Model.Project()
            {
                Name = "Project n° 3",
                Description = "This is my first standalone project!",
                Users = new List<Model.BacklogmanUser>()
                {
                    new Model.BacklogmanUser() {FullName = "cedric", Email = "cedric@suisse.ch"},
                    new Model.BacklogmanUser() {FullName = "david", Email = "david@suisse.ch"},
                }
            });
            Projects.Add(ProjectsStandalone.Last());
            ProjectsStandalone.Add(new Model.Project()
            {
                Name = "Project n° 4",
                Description = "This is my second standalone project!",
                Users = new List<Model.BacklogmanUser>()
                {                    
                    new Model.BacklogmanUser() {FullName = "david", Email = "david@suisse.ch"},
                    new Model.BacklogmanUser() {FullName = "cedric", Email = "cedric@suisse.ch"},
                }
            });
            Projects.Add(ProjectsStandalone.Last());            
            #endregion
            #region define backlogs
            ProjectBacklogs = new ReorderableCollection<Model.Backlog>();
            ProjectBacklogs.Add(new Model.Backlog()
            {
                Name = "Backlog n° 1",
                Description = "This is my first project!",
                Project = Projects.First(),
                Themes = new string[] {"Windows", "Windows Phone"}.ToList(),
            });
            ProjectBacklogs.Add(new Model.Backlog()
            {
                Name = "Backlog n° 2",
                Description = "This is my second project!",
                Project = Projects.First(),
                Themes = new string[] { "Windows", "Windows Phone" }.ToList(),
            });
            #endregion
            #region define Stories
            BacklogStories = new ReorderableCollection<Model.Story>();
            BacklogStories.Add(new Model.Story()
            {
                AsUser = "Designer",
                Goal = "have a design view model",
                Result = "degign more easily",
                Points = 5,
                Backlog = ProjectBacklogs.First(),
                Status = Model.StoryStatus.ToDo,
                Theme = "Design",
                Code = "JDG1",
                AcceptanceCriteria = @"- etre beau
- doit avoir assez de contenu
- voilà quoi",
                ColorString = "#FF0011"
            });
            BacklogStories.Add(new Model.Story()
            {
                AsUser = "Developer",
                Goal = "see stories of my backlog",
                Result = "choose what to do next",
                Points = 8,
                Backlog = ProjectBacklogs.First(),
                Status = Model.StoryStatus.InProgress,
                Theme = "Design",
                Code = "JDG1",
                AcceptanceCriteria = @"- première ligne
- 2ème ligne
- 3ème ligne
- 4ème ligne",
                ColorString = "#00FF33"
            });
            #endregion

            #region define not estimared Stories
            NotEstimatedStories = new System.Collections.ObjectModel.ObservableCollection<Model.Story>();
            NotEstimatedStories.Add(new Model.Story()
            {
                AsUser = "Designer",
                Goal = "have a design view model",
                Result = "degign more easily",
                Points = -1,
                Backlog = ProjectBacklogs.First(),
                Status = Model.StoryStatus.ToDo,
                Theme = "Design",
                Code = "JDG1",
                AcceptanceCriteria = @"- etre beau
- doit avoir assez de contenu
- voilà quoi",
                ColorString = "#FF0011"
            });
            NotEstimatedStories.Add(new Model.Story()
            {
                AsUser = "Developer",
                Goal = "see stories of my backlog",
                Result = "choose what to do next",
                Points = -1,
                Backlog = ProjectBacklogs.First(),
                Status = Model.StoryStatus.InProgress,
                Theme = "Design",
                Code = "JDG1",
                AcceptanceCriteria = @"- première ligne
- 2ème ligne
- 3ème ligne
- 4ème ligne",
                ColorString = "#00FF33"
            });
            #endregion
        }




        public System.Collections.ObjectModel.ObservableCollection<Model.Organization> Organizations
        {
            get; 
            set;
        }

        public Model.Organization CurrentOrganization
        {
            get { return Organizations.First(); }
            set { throw new NotImplementedException(); }
        }

        public System.Collections.ObjectModel.ObservableCollection<Model.Project> OrganizationProjects
        {
            get;
            set;
        }

        public System.Collections.ObjectModel.ObservableCollection<Model.Project> Projects
        {
            get;
            set;
        }

        public System.Collections.ObjectModel.ObservableCollection<Model.Project> ProjectsStandalone
        {
            get;
            set;
        }


        public string ApiKey
        {
            get 
            {
                return "asdfjapsoidfasjdf";
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public Model.Project CurrentProject
        {
            get { return Projects.First(); }
            set { throw new NotImplementedException(); }
        }

        public ReorderableCollection<Model.Backlog> ProjectBacklogs
        {
            get;
            set;
        }


        public Model.Backlog CurrentBacklog
        {
            get
            {
                return ProjectBacklogs.First();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ReorderableCollection<Model.Story> BacklogStories
        {
            get;
            set;
        }


        public Task<bool> GetApiKey(string Username, string Password)
        {
            throw new NotImplementedException();
        }


        public void RefreshBacklogStories()
        {
            throw new NotImplementedException();
        }


        public System.Collections.ObjectModel.ObservableCollection<Model.Story> NotEstimatedStories
        {
            get;
            private set;
        }
    }
}
