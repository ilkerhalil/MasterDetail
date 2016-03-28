using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using TreeUtility;

namespace MasterDetail.Models
{
    public class Category:ITreeNode<Category>
    {
        public int Id { get; set; }

        public int? ParentCategoryId { get; set; }

        public string CategoryName { get; set; }

        public virtual  List<InventoryItem> InventoryItems { get; set; }

        public Category Parent { get; set; }

        public IList<Category> Children { get; set; }
    }
}