using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BacklogMan.Client.Core.Model
{
    [DataContract]
    public class BacklogmanUser
    {
        [DataMember(Name = "full_name")]
        public string FullName { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }
    }
}
