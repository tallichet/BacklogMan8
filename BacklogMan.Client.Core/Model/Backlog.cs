using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BacklogMan.Client.Core.Model
{
    [DataContract]
    public class Backlog
    {
        [DataMember(Name="id")]
        public int Id { get; set; }

        [DataMember(Name = "url")]
        public string UrlString { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "story_count")]
        public int StoryCount { get; set; }

        [IgnoreDataMember]
        public Project Project
        {
            get
            {
                return findProjectForBacklog();
            }
        }

        private Core.Model.Project findProjectForBacklog()
        {
            foreach (var p in ServiceLocator.Current.GetInstance<Core.ViewModel.IMainViewModel>().Projects)
            {
                if (p.Backlogs.Any(b => b.Id == this.Id))
                    return p;
            }
            return null;
        }
    }
}
