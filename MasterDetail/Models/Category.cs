﻿using System.Collections.Generic;

namespace MasterDetail.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public virtual  List<InventoryItem> InventoryItems { get; set; }

    }
}