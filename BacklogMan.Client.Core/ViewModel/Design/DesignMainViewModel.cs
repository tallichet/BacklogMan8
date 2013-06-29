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
    }
}
