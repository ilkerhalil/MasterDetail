using System.Data.Entity.ModelConfiguration;

namespace MasterDetail.Models.ModelConfigurations
{
    public class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
        {
            Property(user => user.FirstName).HasMaxLength(15).IsRequired();
            Property(user => user.LastName).HasMaxLength(15).IsRequired();
            Property(user => user.Address).HasMaxLength(30).IsOptional();
            Property(user => user.City).HasMaxLength(20).IsOptional();
            Property(user => user.State).HasMaxLength(2).IsOptional();
            Property(user => user.ZipCode).HasMaxLength(10).IsOptional();

        }
    }
}