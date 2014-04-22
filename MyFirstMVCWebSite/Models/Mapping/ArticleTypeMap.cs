using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace MyFirstMVCWebSite.Models.Mapping
{
    public class ArticleTypeMap : EntityTypeConfiguration<ArticleType>
    {
        public ArticleTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.ArticleTypeId);

            // Properties
            // Table & Column Mappings
            this.ToTable("ArticleTypes");
            this.Property(t => t.ArticleTypeId).HasColumnName("ArticleTypeId");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
