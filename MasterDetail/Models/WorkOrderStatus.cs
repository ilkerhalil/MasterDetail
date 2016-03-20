namespace MasterDetail.Models
{
    public enum WorkOrderStatus
    {
        Created =0,
        InProcess= 10,
        Rework=15,
        Submitted=20,
        Approved = 30,
        Canceled= -10,
        Rejected=-20
    }
}