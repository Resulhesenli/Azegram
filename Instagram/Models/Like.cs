using System;
using System.Collections.Generic;

#nullable disable

namespace Instagram.Models
{
    public partial class Like
    {
        public int LikeId { get; set; }
        public int? LikeUserId { get; set; }
        public int? LikePostId { get; set; }
        public bool? IsLike { get; set; }

        public virtual Post LikePost { get; set; }
        public virtual User LikeUser { get; set; }
    }
}
