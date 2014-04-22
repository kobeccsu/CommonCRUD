using System;
using System.Collections.Generic;

namespace MyFirstMVCWebSite.Models
{
    public partial class BlogUser
    {
        public BlogUser()
        {
            this.Posts = new List<Post>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public virtual BlogUser BlogUser1 { get; set; }
        public virtual BlogUser BlogUser2 { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
