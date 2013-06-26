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
        ObservableCollection<Model.Project> Projects { get; }
    }
}
