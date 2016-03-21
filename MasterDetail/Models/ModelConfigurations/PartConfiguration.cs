using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace MasterDetail.Models.ModelConfigurations
{
    public class PartConfiguration : EntityTypeConfiguration<Part>
    {
        public PartConfiguration()
        {
            Property(part => part.InventoryItemCode)
                .HasMaxLength(15)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("AK_Part", 2) { IsUnique = true }));


            Property(part => part.WorkOrderId)
                  .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("AK_Part", 1) { IsUnique = true }));

            Property(part => part.InventoryItemName)
                .HasMaxLength(80)
                .IsRequired();

            Property(part => part.UnitPrice).HasPrecision(18, 2);

            Property(part => part.ExtentedPrice).HasPrecision(18, 2).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            Property(part => part.Notes)
                .HasMaxLength(140)
                .IsOptional();
        }
    }
}