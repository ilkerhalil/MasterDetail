using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using MasterDetail.Models;
using MasterDetail.ViewModels;
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
            content += "<li  class=\"treenode\">";
            content += parent.CategoryName;
            content += $"<a href=\"/Categories/Edit/{parent.Id}\" class=\"btn btn-primary btn-xs treenodeeditbutton\">Edit</a>";
            content += $"<a href=\"/Categories/Delete/{parent.Id}\" class=\"btn btn-danger btn-xs treenodeeditbutton\">Delete</a>";
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

        void ValidateParentsAreParentless(Category category)
        {
            if (category.ParentCategoryId == null)
                return;
            var parentCategory = _applicationDbContext.Categories.Find(category.ParentCategoryId);
            if (parentCategory.ParentCategoryId != null)
                throw new InvalidOperationException("You cannot nest this category more");
            var numberOfChildren = _applicationDbContext.Categories.Count(c => c.ParentCategoryId == category.Id);
            if (numberOfChildren > 0)
                throw new InvalidOperationException("You cannot nest this category's children more than two levels deep.");
        }

        private SelectList PopulateParentCategorySelectList(int? categoryId)
        {
            SelectList selectList;
            if (categoryId == null)
            {
                selectList = new SelectList(_applicationDbContext.Categories.Where(w => w.ParentCategoryId == null), "Id", "CategoryName");
            }
            else if (_applicationDbContext.Categories.Count(c => c.ParentCategoryId == categoryId) == 0)
            {
                selectList =
                    new SelectList(
                        _applicationDbContext.Categories.Where(w => w.ParentCategoryId == null && w.Id != categoryId),
                        "Id", "CategoryName");
            }
            else
            {
                selectList = new SelectList(_applicationDbContext.Categories.Where(w => false), "Id", "CategoryName");
            }
            return selectList;
        }




        // GET: Categories/Create
        public ActionResult Create()
        {
            ViewBag.ParentCategoryIdSelectList = PopulateParentCategorySelectList(null);
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
                try
                {
                    ValidateParentsAreParentless(category);

                }
                catch (Exception exception)
                {
                    ModelState.AddModelError("", exception.Message);
                    ViewBag.ParentCategoryIdSelectList = PopulateParentCategorySelectList(null);
                    return View(category);
                }

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
            var category = await _applicationDbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            var categoryViewModel = new CategoryViewModel
            {
                Id = category.Id,
                ParentCategoryId = category.ParentCategoryId,
                CategoryName = category.CategoryName
            };

            ViewBag.ParentCategoryIdSelectList = PopulateParentCategorySelectList(id);
            return View(categoryViewModel);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ParentCategoryId,CategoryName")] CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var editedCategory = new Category();
                try
                {
                    editedCategory.Id = categoryViewModel.Id;
                    editedCategory.ParentCategoryId = categoryViewModel.ParentCategoryId;
                    editedCategory.CategoryName = categoryViewModel.CategoryName;
                    ValidateParentsAreParentless(editedCategory);
                }
                catch (Exception exception)
                {

                    ModelState.AddModelError("", exception.Message);
                    ViewBag.ParentCategoryIdSelectList = PopulateParentCategorySelectList(categoryViewModel.Id);
                    return View("Edit", categoryViewModel);
                }

                _applicationDbContext.Entry(editedCategory).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //var categoryViewModel = new CategoryViewModel
            //{
            //    Id = category.Id,
            //    ParentCategoryId = category.ParentCategoryId,
            //    CategoryName = category.CategoryName
            //};
            ViewBag.ParentCategoryIdSelectList = new SelectList(_applicationDbContext.Categories, "Id", "CategoryName");
            return View(categoryViewModel);
        }

        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = await _applicationDbContext.Categories.FindAsync(id);
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
            var category = await _applicationDbContext.Categories.FindAsync(id);
            try
            {
                _applicationDbContext.Categories.Remove(category);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException exception)
            {
                ModelState.AddModelError("", "You attempted to delete a category that had child categories associated with it.");
            }
            catch (Exception exception)
            {

                ModelState.AddModelError("", exception.Message);
            }
            return View("Delete", category);


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
