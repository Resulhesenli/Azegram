using System;
using System.Collections.Generic;

#nullable disable

namespace Instagram.Models
{
    public partial class Follower
    {
        public int FollowerId { get; set; }
        public int? FollowerFromUserId { get; set; }
        public int? FollowerToUserId { get; set; }
        public bool? FollowerIsAccept { get; set; }

        public virtual User FollowerFromUser { get; set; }
        public virtual User FollowerToUser { get; set; }
    }
}
