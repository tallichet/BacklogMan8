using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.Model
{
    [DataContract]
    public class Story : IItemWithId
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "as_a")]        
        public string AsUser { get; set; }

        [DataMember(Name = "i_want_to")]
        public string Goal { get; set; }

        [DataMember(Name = "so_i_can")]
        public string Result { get; set; }

        [DataMember(Name = "acceptances")]
        public string AcceptanceCriteria { get; set; }

        [DataMember(Name = "points")]
        public double Points { get; set; }

        [DataMember(Name = "theme")]
        public string Theme { get; set; }

        [DataMember(Name = "color")]
        public string ColorString { get; set; }

        [DataMember(Name = "comments")]
        public string Comments { get; set; }

        [DataMember(Name="create_date")]
        public string DateTimeString { get; set; }

        [DataMember(Name = "status")]
        public string StatusString
        {
            get
            {
                switch (Status)
                {
                    case StoryStatus.InProgress:
                        return "in_progress";
                    case StoryStatus.ToDo:
                        return "to_do";
                    default:
                        return Status.ToString().ToLower();
                }
            }
            set 
            {
                StoryStatus s;
                if (Enum.TryParse<StoryStatus>(value.Replace("_", ""), true, out s))
                    Status = s;
            }
        }

        [IgnoreDataMember]
        public StoryStatus Status { get; set; }

        [IgnoreDataMember]
        public string Description
        {
            get
            {
                return string.Format("As {0}, I want to {1}, so I can {2}", AsUser, Goal, Result);
            }
        }

        [DataMember(Name = "lang")]
        public string Language { get; set; }

        [IgnoreDataMember]
        public Backlog Backlog { get; set; }

        public async Task<bool> Save(int backlogId)
        {
            var newId = await ServiceLocator.Current.GetInstance<Service.INetworkService>().AddStory(backlogId, this);
            if (newId > 0)
            {
                this.Id = newId;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public enum StoryStatus
    {
        ToDo,
        InProgress,
        Completed,
        Accepted,
        Rejected,
    }
}
