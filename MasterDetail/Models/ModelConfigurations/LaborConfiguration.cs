using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace MasterDetail.Models.ModelConfigurations
{
    public class LaborConfiguration : EntityTypeConfiguration<Labor>
    {
        public LaborConfiguration()
        {
            Property(labor => labor.ServiceItemCode)
                .HasMaxLength(15)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("AK_Labor", 2) { IsUnique = true }));
            Property(labor => labor.WorkOrderId)
                            .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                                new IndexAnnotation(new IndexAttribute("AK_Labor", 1) { IsUnique = true }));

            Property(labor => labor.ServiceItemName)
                .HasMaxLength(80)
                .IsRequired();

            Property(labor => labor.LaborHours).HasPrecision(18, 2);

            Property(labor => labor.Rate).HasPrecision(18, 2);

            Property(labor => labor.ExtendedPrice)
                .HasPrecision(18, 2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            Property(labor => labor.Notes).HasMaxLength(140).IsOptional();
        }
    }
}