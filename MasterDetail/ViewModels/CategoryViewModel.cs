using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MasterDetail.Models;

namespace MasterDetail.ViewModels
{
    public class CategoryViewModel
    {
        private int? _parentCategoryId;
        public int Id { get; set; }

        [Display(Name = "Parent Category")]
        public int? ParentCategoryId
        {
            get { return _parentCategoryId; }
            set
            {
                if (Id == value) throw new InvalidOperationException("A category cannot have itself as its parent");
                _parentCategoryId = value;
            }
        }

        [Required(ErrorMessage = "You must enter a category name")]
        [StringLength(20, ErrorMessage = "Category must be 20 characters or shorter")]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        public virtual List<InventoryItem> InventoryItems { get; set; }

        public virtual Category Parent { get; set; }

        public IList<Category> Children { get; set; }
    }
}