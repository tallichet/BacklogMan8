using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core.Model
{
    [DataContract]
    public class Statistics
    {
        /// <summary>
        /// number of estimated remaining points
        /// </summary>
        [DataMember(Name = "estimated_points")]
        public int PointsEstimated { get; set; }

        /// <summary>
        /// number of completed points
        /// </summary>
        [DataMember(Name = "completed_points")]
        public int PointsCompleted { get; set; }

        /// <summary>
        /// percent of story estimated
        /// </summary>
        [DataMember(Name = "percent_estimated")]
        public double PercentEstimated { get; set; }

        /// <summary>
        /// percent of story completed
        /// </summary>
        [DataMember(Name = "percent_completed")]
        public double PercentCompleted { get; set; }

        /// <summary>
        /// total number of stories
        /// </summary>
        [DataMember(Name = "total_stories")]
        public int StoriesTotal { get; set; }

        /// <summary>
        /// total number of points
        /// </summary>
        [DataMember(Name = "total_points")]
        public int PointsTotal { get; set; }

        /// <summary>
        /// number of estimated stories
        /// </summary>
        [DataMember(Name = "estimated_stories")]
        public int StoriesEstimated { get; set; }

        /// <summary>
        /// number of completed stories
        /// </summary>
        [DataMember(Name = "completed_stories")]
        public int StoriesCompleted { get; set; }
    }
}
