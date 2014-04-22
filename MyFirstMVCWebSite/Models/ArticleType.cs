using System;
using System.Collections.Generic;

namespace MyFirstMVCWebSite.Models
{
    public partial class ArticleType
    {
        public ArticleType()
        {
            this.Articles = new List<Article>();
        }

        public long ArticleTypeId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}
