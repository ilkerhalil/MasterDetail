using System.IO;
using System.Web.Security;
using MasterDetail.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MasterDetail.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            //AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MasterDetail.Models.ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager) { AllowOnlyAlphanumericUserNames = false };

            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ApplicationDbContext()));

            var name = "pluralsightnimda@gmail.com";
            var password = "Pluralsight#1";
            var firstName = "Admin";
            var roleName = "Admin";
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleResult = roleManager.Create(role);
            }
            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser() { UserName = name, Email = name, FirstName = firstName };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }

            var accountNumber = "ABC123";
            context.Customers.AddOrUpdate(cust => cust.AccountNumber,
                new Customer
                {
                    AccountNumber = accountNumber,
                    CompanyName = "ABC Company of America",
                    Address = "123 Main St.",
                    City = "Anytown",
                    State = "GA",
                    ZipCode = "30071"
                });
            context.SaveChanges();

            var customer = context.Customers.First(f => f.AccountNumber == accountNumber);
            var description = "Just another work order";
            context.WorkOrders.AddOrUpdate(wo => wo.Description, new WorkOrder
            {
                Description = description,
                CustomerId = customer.CustomerId
            });
            context.SaveChanges();
            var workOrder = context.WorkOrders.First(f => f.Description == description);
            context.Parts.AddOrUpdate(part => part.InventoryItemCode, new Part
            {
                InventoryItemCode = "THING1",
                InventoryItemName = "Thing Number One",
                Quantity = 1,
                UnitPrice = 1.23m,
                WorkOrderId = workOrder.WorkOrderId,

            });
            context.Labors.AddOrUpdate(labor => labor.ServiceItemCode, new Labor
            {
                ServiceItemCode = "INSTALL",
                ServiceItemName = "Installation",
                LaborHours = 9.87m,
                Rate = 35.95m,
                WorkOrderId = workOrder.WorkOrderId

            });
            var categoryName = "Devices";

            context.Categories.AddOrUpdate(cat => cat.CategoryName, new Category { CategoryName = categoryName });
            context.SaveChanges();

            var category = context.Categories.First(f => f.CategoryName == categoryName);

            context.InventoryItems.AddOrUpdate(item => item.InventoryItemCode, new InventoryItem
            {
                InventoryItemCode = "THING2",
                InventoryItemName = "A Second Kind of Thing",
                UnitPrice = 3.3m,
                CategoryId = category.CategoryId
            });
            context.ServiceItems.AddOrUpdate(item => item.ServiceItemCode, new ServiceItem
            {
                ServiceItemCode = "CLEAN",
                ServiceItemName = "General Cleaning",
                Rate = 23.50m

            });
            context.SaveChanges();
        }
    }
}
