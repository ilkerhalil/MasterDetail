using System.Data.Entity.ModelConfiguration;

namespace MasterDetail.Models.ModelConfigurations
{
    public class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
        {
            Property(user => user.FirstName).HasMaxLength(15).IsOptional();
        }
    }
}