using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace MasterDetail.Models.ModelConfigurations
{
    public class CustomerConfiguration : EntityTypeConfiguration<Customer>
    {
        public CustomerConfiguration()
        {
            Property(customer => customer.AccountNumber)
                .HasMaxLength(8)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("AK_Customer_AccountNumber") { IsUnique = true }));

            Property(customer => customer.CompanyName)
                .HasMaxLength(30)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("AK_Customer_CompanyName") { IsUnique = true }));
            Property(customer => customer.Address).HasMaxLength(30).IsRequired();
            Property(customer => customer.City).HasMaxLength(15).IsRequired();
            Property(customer => customer.State).HasMaxLength(2).IsRequired();
            Property(customer => customer.ZipCode).HasMaxLength(10).IsRequired();
            Property(customer => customer.Phone).HasMaxLength(15).IsOptional();
        }
    }
}