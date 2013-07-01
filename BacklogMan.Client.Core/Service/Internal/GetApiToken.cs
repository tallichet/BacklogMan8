using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.Service.Internal
{
    [DataContract]
    public class GetApiTokenRequestContent
    {
        #region Send
        [DataMember(Name = "username", EmitDefaultValue = false)]
        public string Username { get; set; }
        [DataMember(Name = "password", EmitDefaultValue = false)]
        public string Password { get; set; }
        #endregion

    }

    [DataContract]
    public class GetApiTokenResponseContent
    {
        #region result
        [DataMember(Name = "token", EmitDefaultValue = false)]
        public string ApiToken { get; set; }
        #endregion
    }
}
