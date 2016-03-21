using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace MasterDetail.Models.ModelConfigurations
{
    public class ServiceItemConfiguration : EntityTypeConfiguration<ServiceItem>

    {
        public ServiceItemConfiguration()
        {
            Property(si => si.ServiceItemCode)
                .HasMaxLength(15)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("AK_ServiceItem_ServiceItemCode")
                {
                    IsUnique = true
                }));
            Property(si => si.ServiceItemName)
                .HasMaxLength(80)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("AK_ServiceItem_ServiceItemName")
                {
                    IsUnique = true
                }));
            Property(si => si.Rate).HasPrecision(18, 2);

        }
    }
}