using System;

namespace ExpenseTrackerApp
{
    public class InvalidAmountException : Exception
    {
        public InvalidAmountException(string message) : base(message) { }
    }

    public class ExpenseNotFoundException : Exception
    {
        public ExpenseNotFoundException(string message) : base(message) { }
    }
}