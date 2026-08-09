using System;

namespace ExpenseTrackerApp
{
    public class Expense
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return $"[ID: {Id}] {Date:yyyy-MM-dd} | Category: {Category,-12} | Title: {Title,-15} | Amount: ₹{Amount:F2}";
        }
    }
}
