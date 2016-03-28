using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace MasterDetail.Models.ModelConfigurations
{
    public class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {
            Property(category => category.Id).HasColumnName("CategoryId");

            Property(category => category.CategoryName)
                .HasMaxLength(20)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                new IndexAnnotation
                (new IndexAttribute("AK_Category_CategoryName") { IsUnique = true }));

            HasOptional(category => category.Parent)
                .WithMany(category => category.Children)
                .HasForeignKey(category => category.ParentCategoryId)
                .WillCascadeOnDelete(false);
        }
    }
}