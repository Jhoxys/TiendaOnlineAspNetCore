namespace CapaAdmin.Models
{
    public class FacturaDto
    {

        public decimal DebtType { get; set; }  // 0,1,2
        public List<BillingDto> BillingDto { get; set; } = new List<BillingDto>();


        public Clients Clients { get; set; }

    }

}
