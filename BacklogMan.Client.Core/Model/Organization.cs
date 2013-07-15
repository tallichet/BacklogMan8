using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.Model
{
    [DataContract]
    public class Organization
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "url")]
        public string UrlString { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "web_site")]
        public string WebSite { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "users")]
        public List<BacklogmanUser> Users { get; set; }

        [DataMember(Name = "projects")]
        public List<OrganizationProject> Projects { get; set; }
    }

    [DataContract]
    public class OrganizationProject
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Id { get; set; }
    }
}
