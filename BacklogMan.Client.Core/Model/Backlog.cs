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

        [IgnoreDataMember]
        public int StoryCount
        {
            get
            {
                return Statistics != null ? Statistics.StoriesTotal : 0;
            }
        }

        [DataMember(Name = "available_themes")]
        public List<string> Themes { get; set; }

        [DataMember(Name = "stats")]
        public Statistics Statistics { get; set; }


        [IgnoreDataMember]
        public string ThemesAsListString
        {
            get
            {
                return string.Join(", ", Themes);
            }
        }

        [IgnoreDataMember]
        public Project Project { get; set; }
    }
}
