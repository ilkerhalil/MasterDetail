using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using MasterDetail.Models;
using TreeUtility;

namespace MasterDetail.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();

        // GET: Categories
        public async Task<ActionResult> Index()
        {
            var listOfNodes = GetListOfNodes();
            var topLevelCategories = listOfNodes.ConvertToForest();
            var fullString = topLevelCategories.Aggregate("<ul>", (current, topLevelCategory) => current + EnumerateNodes(topLevelCategory));

            return View((object)fullString);
        }

        private string EnumerateNodes(Category parent)
        {
            var content = "";
            content += "<li>";
            content += parent.CategoryName;
            if (parent.Children.Count == 0)
                content += "</li>";
            else
                content += "<ul>";
            var numberofChildren = parent.Children.Count;
            for (var i = 0; i <= numberofChildren; i++)
            {
                if (numberofChildren > 0 && i < numberofChildren)
                {
                    var child = parent.Children[i];
                    content += EnumerateNodes(child);
                }
                if (numberofChildren > 0 && i == numberofChildren)
                    content += "</ul>";

            }
            return content;


        }

        private IList<Category> GetListOfNodes()
        {
            var sourceCategories = _applicationDbContext.Categories.ToList();
            var categories = new List<Category>();
            foreach (var sourceCategory in sourceCategories)
            {
                var c = new Category
                {
                    Id = sourceCategory.Id,
                    CategoryName = sourceCategory.CategoryName
                };
                if (sourceCategory.ParentCategoryId != null)
                {
                    c.Parent = new Category { Id = sourceCategory.ParentCategoryId.Value };
                }
                categories.Add(c);
            }
            return categories;
        }


        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ParentCategoryId,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Categories.Add(category);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await _applicationDbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ParentCategoryId,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Entry(category).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await _applicationDbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await _applicationDbContext.Categories.FindAsync(id);
            _applicationDbContext.Categories.Remove(category);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _applicationDbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
