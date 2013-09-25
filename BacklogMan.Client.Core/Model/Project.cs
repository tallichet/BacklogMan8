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
    public class Project : IItemWithId
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

        [DataMember(Name = "lang")]
        public string Language { get; set; }

        [DataMember(Name = "organization")]
        public int OrganizationId { get; set; }

        [IgnoreDataMember]
        public int StoryCount
        {
            get
            {
                return Statistics != null ? Statistics.StoriesTotal : 0;
            }
        }

        [DataMember(Name = "users")]
        public List<BacklogmanUser> Users { get; set; }

        [DataMember(Name = "available_themes")]
        public List<string> Themes { get; set; }

        [DataMember(Name= "stats")]
        public Statistics Statistics { get; set; }

        [DataMember(Name = "backlogs")]
        public List<Backlog> Backlogs { get; set; }

        /// <summary>
        /// Return true if was extracted from the full version
        /// </summary>
        [IgnoreDataMember]
        public bool IsFull
        {
            get
            {
                return Users != null;
            }
        }
    }

    
}
