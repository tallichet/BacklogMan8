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

        [DataMember(Name = "story_count")]
        public int StoryCount { get; set; }

        [DataMember(Name = "users")]
        public List<BacklogmanUser> Users { get; set; }

        [DataMember(Name = "available_themes")]
        public List<string> Themes { get; set; }

        [DataMember(Name= "stats")]
        public Statistics Statistics { get; set; }
    }

    
}
