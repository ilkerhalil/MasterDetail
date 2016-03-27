using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MasterDetail.Models;
using MasterDetail.ViewModels;
using Microsoft.AspNet.Identity.Owin;

namespace MasterDetail.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class ApplicationRolesController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public ApplicationRolesController()
        {

        }
        public ApplicationRolesController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>(); }
            set { _roleManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            set
            {
                _userManager = value;
            }
        }


        // GET: ApplicationRoles
        public async Task<ActionResult> Index()
        {
            return View(await RoleManager.Roles.ToListAsync());
        }

        // GET: ApplicationRoles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var applicationRole = await RoleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            return View(applicationRole);
        }

        // GET: ApplicationRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name")] ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var applicationRole = new ApplicationRole { Name = applicationRoleViewModel.Name };
                //db.IdentityRoles.Add(applicationRole);
                var roleResult = await RoleManager.CreateAsync(applicationRole);
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError("", roleResult.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: ApplicationRoles/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var applicationRole = await RoleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            var applicationRoleViewModel = new ApplicationRoleViewModel
            {
                Id = applicationRole.Id,
                Name = applicationRole.Name
            };

            return View(applicationRole);
        }

        // POST: ApplicationRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var applicationRole = await RoleManager.FindByIdAsync(applicationRoleViewModel.Id);
                var originalName = applicationRole.Name;
                if (originalName == "Admin" && applicationRoleViewModel.Name != "Admin")
                {
                    ModelState.AddModelError("", "You cannot change the name of the Admin role.");
                    return View(applicationRoleViewModel);
                }
                if (originalName == "Admin" && applicationRoleViewModel.Name == "Admin")
                {
                    ModelState.AddModelError("", "You cannot change the name of the Admin to role.");
                    return View(applicationRoleViewModel);
                }
                applicationRole.Name = applicationRoleViewModel.Name;
                await RoleManager.UpdateAsync(applicationRole);
                return RedirectToAction("Index");
            }
            return View(applicationRoleViewModel);
        }

        // GET: ApplicationRoles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var applicationRole = await RoleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            return View(applicationRole);
        }

        // POST: ApplicationRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ApplicationRole applicationRole = await RoleManager.FindByIdAsync(id);
            if (applicationRole.Name == "Admin")
            {
                ModelState.AddModelError("", "You cannot delete the Admin role.");
                return View(applicationRole);
            }
            await RoleManager.DeleteAsync(applicationRole);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RoleManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
