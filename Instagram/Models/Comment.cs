using System;
using System.Collections.Generic;

#nullable disable

namespace Instagram.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int? CommentUserId { get; set; }
        public int? CommentPostId { get; set; }
        public string CommentDescription { get; set; }
        public DateTime CommentWriteDate { get; set; }

        public virtual Post CommentPost { get; set; }
        public virtual User CommentUser { get; set; }
    }
}
