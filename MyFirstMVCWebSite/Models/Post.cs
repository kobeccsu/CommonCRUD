using System;
using System.Collections.Generic;

namespace MyFirstMVCWebSite.Models
{
    public partial class Post
    {
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public int BlogUserId { get; set; }
        public virtual BlogUser BlogUser { get; set; }
    }
}
