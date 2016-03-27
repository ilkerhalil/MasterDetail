using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MasterDetail.Models;
using Microsoft.AspNet.Identity.Owin;

namespace MasterDetail.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            set { _userManager = value; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>(); }
            set { _roleManager = value; }
        }
        // private ApplicationDbContext db = new ApplicationDbContext();


        public ApplicationUsersController()
        {

        }

        public ApplicationUsersController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        // GET: ApplicationUsers
        public async Task<ActionResult> Index()
        {
            return View(await UserManager.Users.ToListAsync());
        }


        // GET: ApplicationUsers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var applicationUser = await UserManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            var userRoles = await UserManager.GetRolesAsync(applicationUser.Id);
            applicationUser.RoleList = RoleManager.Roles.ToList().Select(s => new SelectListItem
            {
                Selected = userRoles.Contains(s.Name),
                Text = s.Name,
                Value = s.Name
            });
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id")] ApplicationUser applicationUser, params string[] rolesSelectedOnView)
        {
            if (ModelState.IsValid)
            {
                var rolesCurrentlyPersistedForUser = await UserManager.GetRolesAsync(applicationUser.Id);
                var isThisUserAnAdmin = rolesCurrentlyPersistedForUser.Contains("Admin");

                rolesSelectedOnView = rolesSelectedOnView ?? new string[] { };
                var isThisUserAnAdminDeselected = !rolesSelectedOnView.Contains("Admin");

                var role = await RoleManager.FindByNameAsync("Admin");
                var isOnlyOneUserAdmin = role.Users.Count == 1;

                applicationUser.RoleList = RoleManager.Roles.ToList().Select(s => new SelectListItem
                {
                    Selected = rolesCurrentlyPersistedForUser.Contains(s.Name),
                    Text = s.Name,
                    Value = s.Name
                });

                if (isThisUserAnAdmin && isThisUserAnAdminDeselected && isOnlyOneUserAdmin)
                {
                    ModelState.AddModelError("", "At least one user must retain the Admin role;you are attempting to delete the Admin role");
                    return View(applicationUser);
                }

                applicationUser = await UserManager.FindByIdAsync(applicationUser.Id);

                var result = await UserManager.AddToRolesAsync(applicationUser.Id, rolesSelectedOnView.Except(rolesCurrentlyPersistedForUser).ToArray());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View(applicationUser);
                }
                result = await UserManager.RemoveFromRolesAsync(applicationUser.Id, rolesCurrentlyPersistedForUser.Except(rolesSelectedOnView).ToArray());
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View(applicationUser);
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed");
            return View(applicationUser);
        }


        public async Task<ActionResult> LockAccount([Bind(Include = "Id")] string id)
        {
            await UserManager.ResetAccessFailedCountAsync(id);
            await UserManager.SetLockoutEndDateAsync(id, DateTimeOffset.UtcNow.AddYears(100));
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> UnlockAccount([Bind(Include = "Id")] string id)
        {
            await UserManager.ResetAccessFailedCountAsync(id);
            await UserManager.SetLockoutEndDateAsync(id, DateTimeOffset.UtcNow.AddYears(-1));
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
