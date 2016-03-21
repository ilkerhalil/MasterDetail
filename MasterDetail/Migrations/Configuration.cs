using MasterDetail.Models;

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
