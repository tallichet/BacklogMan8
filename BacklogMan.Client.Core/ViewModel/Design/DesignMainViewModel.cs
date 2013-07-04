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
            #region define projects
            Projects = new System.Collections.ObjectModel.ObservableCollection<Model.Project>();
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
            #endregion
            #region define backlogs
            ProjectBacklogs = new System.Collections.ObjectModel.ObservableCollection<Model.Backlog>();
            ProjectBacklogs.Add(new Model.Backlog()
            {
                Name = "Backlog n° 1",
                Description = "This is my first project!",
                Project = Projects.First(),
                StoryCount = 2,
                Themes = new string[] {"Windows", "Windows Phone"}.ToList(),
            });
            ProjectBacklogs.Add(new Model.Backlog()
            {
                Name = "Backlog n° 2",
                Description = "This is my second project!",
                Project = Projects.First(),
                StoryCount = 2,
                Themes = new string[] { "Windows", "Windows Phone" }.ToList(),
            });
            #endregion
            #region define Stories
            BacklogStories = new System.Collections.ObjectModel.ObservableCollection<Model.Story>();
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
        }

        public System.Collections.ObjectModel.ObservableCollection<Model.Project> Projects
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

        public System.Collections.ObjectModel.ObservableCollection<Model.Backlog> ProjectBacklogs
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

        public System.Collections.ObjectModel.ObservableCollection<Model.Story> BacklogStories
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
    }
}
