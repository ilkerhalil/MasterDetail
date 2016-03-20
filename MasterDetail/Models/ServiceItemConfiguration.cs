using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace MasterDetail.Models
{
    public class ServiceItemConfiguration : EntityTypeConfiguration<ServiceItem>

    {
        public ServiceItemConfiguration()
        {
            Property(si => si.ServiceItemCode)
                .HasMaxLength(15)
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("AK_ServiceItem_ServiceItemCode")
                {
                    IsUnique = true
                }));
            Property(si => si.ServiceItemName)
                .HasMaxLength(80)
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAttribute("AK_ServiceItem_ServiceItemName")
                {
                    IsUnique = true
                });
            Property(si => si.Rate).HasPrecision(18, 2);

        }
    }
}