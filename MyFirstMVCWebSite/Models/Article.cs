using System;
using System.Collections.Generic;

namespace MyFirstMVCWebSite.Models
{
    public partial class Article
    {
        public long ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public long ArticleTypeId { get; set; }
        public virtual ArticleType ArticleType { get; set; }
    }
}
