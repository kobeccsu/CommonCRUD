using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace MyFirstMVCWebSite.Models.Mapping
{
    public class ArticleMap : EntityTypeConfiguration<Article>
    {
        public ArticleMap()
        {
            // Primary Key
            this.HasKey(t => t.ArticleId);

            // Properties
            // Table & Column Mappings
            this.ToTable("Articles");
            this.Property(t => t.ArticleId).HasColumnName("ArticleId");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.ArticleTypeId).HasColumnName("ArticleTypeId");

            // Relationships
            this.HasRequired(t => t.ArticleType)
                .WithMany(t => t.Articles)
                .HasForeignKey(d => d.ArticleTypeId);

        }
    }
}
