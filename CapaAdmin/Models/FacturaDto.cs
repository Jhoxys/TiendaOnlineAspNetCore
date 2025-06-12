namespace CapaAdmin.Models
{
    public class FacturaDto
    {

        public List<BillingDto> BillingDto { get; set; } = new List<BillingDto>();


        public Clients Clients { get; set; }

    }

}
