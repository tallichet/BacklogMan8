using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.Service.Internal
{
    [DataContract]
    public class MoveStoryRequest
    {
        [DataMember(Name = "target_backlog")]
        public int TargetBacklog;

        [DataMember(Name = "moved_story")]
        public int MovedStory;

        [DataMember(Name = "order")]
        public int[] StoryOrder;
    }

    [DataContract]
    public class OrderBacklogRequest
    {
        [DataMember(Name = "moved_backlog")]
        public int MovedBacklog;

        [DataMember(Name = "order")]
        public int[] BacklogOrder;
    }

    [DataContract]
    public class UpdateStoriesStatusRequest
    {
        public UpdateStoriesStatusRequest(Model.StoryStatus status)
        {
            switch (status)
            {
                case Model.StoryStatus.InProgress:
                    StatusString = "in_progress";
                    break;
                default:
                    StatusString = status.ToString().ToLower();
                    break;
            }
        }

        [DataMember(Name = "status")]
        public string StatusString { get; set; }
    }

    [DataContract]
    public class Result
    {
        
        [DataMember(Name = "ok")]
        public bool Ok;
    }
}
