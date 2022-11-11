using System;
using System.Collections.Generic;

#nullable disable

namespace Instagram.Models
{
    public partial class Gender
    {
        public Gender()
        {
            Users = new HashSet<User>();
        }

        public int GenderId { get; set; }
        public string GenderName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
