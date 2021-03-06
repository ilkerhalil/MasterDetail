﻿using System;

namespace MasterDetail.Models
{
    public class WorkOrder
    {
        public int WorkOrderId { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public DateTime OrderDateTime { get; set; }

        public DateTime? TargetDateTime { get; set; }

        public DateTime? DropDeadDateTime { get; set; }

        public string Description { get; set; }

        public WorkOrderStatus WorkOrderStatus { get; set; }

        public decimal Total { get; set; }

        public string CertificationRequirements { get; set; }

        public ApplicationUser CurrentWorker { get; set; }

        public string CurrentWorkerId { get; set; }


    }
}