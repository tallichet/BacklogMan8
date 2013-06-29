using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.ViewModel
{
    public interface IMainViewModel
    {
        /// <summary>
        /// List of all projects
        /// </summary>
        ObservableCollection<Model.Project> Projects { get; }

        /// <summary>
        /// Api key used to login on backlog man
        /// </summary>
        string ApiKey { get; set; }

        Model.Project CurrentProject { get; set; }
    }
}
