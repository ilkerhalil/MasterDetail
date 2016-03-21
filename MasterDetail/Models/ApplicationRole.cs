using Microsoft.AspNet.Identity.EntityFramework;

namespace MasterDetail.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {
            
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}