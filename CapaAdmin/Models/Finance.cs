using Microsoft.EntityFrameworkCore;

namespace CapaAdmin.Models
{
    public class Finance
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        [Precision(16, 2)]
        public decimal DailyEarnings { get; set; }

        [Precision(16, 2)]
        public decimal MonthlyEarnings { get; set; }

        [Precision(16, 2)]
        public decimal WeeklyEarnings { get; set; }


        [Precision(16, 2)]
        public decimal DailyExpenses { get; set; }

        [Precision(16, 2)]
        public decimal MonthlyExpenses { get; set; }

        [Precision(16, 2)]
        public decimal WeeklyExpenses { get; set; }


        public Product Product { get; set; } = new Product();

        public Inventory Inventory { get; set; } = new Inventory();
    }
}
