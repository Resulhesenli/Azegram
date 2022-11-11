using System;
using System.Collections.Generic;

#nullable disable

namespace Instagram.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
        }

        public int PostId { get; set; }
        public int? PostUserId { get; set; }
        public string PostPhotoUrl { get; set; }
        public string PostDescription { get; set; }
        public DateTime PostWriteDate { get; set; }

        public virtual User PostUser { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
    }
}
