using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace MyFirstMVCWebSite.Models.Mapping
{
    public class PostMap : EntityTypeConfiguration<Post>
    {
        public PostMap()
        {
            // Primary Key
            this.HasKey(t => t.PostId);

            // Properties
            this.Property(t => t.PostId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.PostTitle)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Posts");
            this.Property(t => t.PostId).HasColumnName("PostId");
            this.Property(t => t.PostTitle).HasColumnName("PostTitle");
            this.Property(t => t.BlogUserId).HasColumnName("BlogUserId");

            // Relationships
            this.HasRequired(t => t.BlogUser)
                .WithMany(t => t.Posts)
                .HasForeignKey(d => d.BlogUserId);

        }
    }
}
