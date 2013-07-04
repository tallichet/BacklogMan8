using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.Model
{
    [DataContract]
    public class Project
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "url")]
        public string UrlString { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "users")]
        public List<BacklogmanUser> Users { get; set; }

<<<<<<< HEAD
        private ReorderableCollection<Backlog> backlogs = null;

        [DataMember(Name = "backlogs")]
        public ReorderableCollection<Backlog> Backlogs 
        {
            get { return backlogs; }
            set
            {
                if (backlogs != value)
                {
                    if (backlogs != null) backlogs.ManualReordered -= backlogs_ManualReordered;
                    backlogs = value;
                    if (backlogs != null) backlogs.ManualReordered += backlogs_ManualReordered;
                }
            }
        }

        void backlogs_ManualReordered(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var movedBacklog = e.NewItems[0] as Backlog;

            if (movedBacklog == null) return;

            var operationResult = ServiceLocator.Current.GetInstance<Service.INetworkService>().OrderBacklog(this.Id, movedBacklog.Id,
                                        backlogs.Select(b => b.Id).ToArray());

            if (Debugger.IsAttached)
            {
                Debug.WriteLine("moved backlog '{0}' operation resut is {1}", movedBacklog.Name, operationResult);
            }
        }
=======
        [DataMember(Name = "available_themes")]
        public List<string> Themes { get; set; }
>>>>>>> cb57972725087eb0cc2d9112c4e3b500c45c16f5
    }
}
