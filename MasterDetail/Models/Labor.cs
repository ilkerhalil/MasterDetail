﻿namespace MasterDetail.Models
{
    public partial class Labor
    {
        public int LaborId { get; set; }

        public int WorkOrderId { get; set; }

        public WorkOrder WorkOrder { get; set; }

        public string ServiceItemCode { get; set; }

        public string ServiceItemName { get; set; }

        public decimal LaborHours { get; set; }

        public decimal Rate { get; set; }

        public decimal ExtendedPrice { get; set; }

        public string Notes { get; set; }


    }
}