using Microsoft.EntityFrameworkCore;

namespace CapaAdmin.Models
{
    public class Inventory
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
        public decimal YearEarnings { get; set; }

        [Precision(16, 2)]
        public decimal DailyExpenses { get; set; }

        [Precision(16, 2)]
        public decimal MonthlyExpenses { get; set; }

        [Precision(16, 2)]
        public decimal WeeklyExpenses { get; set; }

        [Precision(16, 2)]
        public decimal YearExpenses { get; set; }
        public DateTime CreatedAt { get; set; }


        public int? BillingId { get; set; }
 




    }
}
