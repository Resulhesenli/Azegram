using System;
using System.Collections.Generic;

#nullable disable

namespace Instagram.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            FollowerFollowerFromUsers = new HashSet<Follower>();
            FollowerFollowerToUsers = new HashSet<Follower>();
            Likes = new HashSet<Like>();
            Posts = new HashSet<Post>();
        }

        public int UserId { get; set; }
        public int? UserGenderId { get; set; }
        public string UserNickName { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserProfilePhotoUrl { get; set; }
        public string UserBio { get; set; }
        public string UserPassword { get; set; }

        public virtual Gender UserGender { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Follower> FollowerFollowerFromUsers { get; set; }
        public virtual ICollection<Follower> FollowerFollowerToUsers { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
