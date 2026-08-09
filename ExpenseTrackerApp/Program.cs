using System;

namespace ExpenseTrackerApp
{
    internal class Program
    {
        private static ExpenseManager _manager = new ExpenseManager();

        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("     EXPENSE TRACKER MODULE (.NET)      ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Add Expense");
                Console.WriteLine("2. View All Expenses");
                Console.WriteLine("3. View Total Expenditure");
                Console.WriteLine("4. Delete Expense");
                Console.WriteLine("5. Exit");
                Console.WriteLine("========================================");
                Console.Write("Select an option (1-5): ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddExpenseUI();
                            break;
                        case "2":
                            ViewExpensesUI();
                            break;
                        case "3":
                            ViewTotalUI();
                            break;
                        case "4":
                            DeleteExpenseUI();
                            break;
                        case "5":
                            running = false;
                            Console.WriteLine("Exiting module. Good luck with Practical 3!");
                            break;
                        default:
                            throw new FormatException("Invalid selection. Please enter a number between 1 and 5.");
                    }
                }
                catch (InvalidAmountException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[Custom Validation Error] {ex.Message}");
                }
                catch (ExpenseNotFoundException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"[Data Error] {ex.Message}");
                }
                catch (FormatException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[Input Format Error] {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[System Error] An unexpected error occurred: {ex.Message}");
                }
                finally
                {
                    Console.ResetColor();
                    if (running)
                    {
                        Console.WriteLine("\nPress any key to return to the menu...");
                        Console.ReadKey();
                    }
                }
            }
        }

        private static void AddExpenseUI()
        {
            Console.Write("Enter Title: ");
            string title = Console.ReadLine();

            Console.Write("Enter Category (e.g. Food, Transport): ");
            string category = Console.ReadLine();

            Console.Write("Enter Amount (₹): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                throw new FormatException("Amount must be a valid numeric decimal number.");
            }

            _manager.AddExpense(title ?? "", amount, category ?? "");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✓ Expense added successfully!");
        }

        private static void ViewExpensesUI()
        {
            var list = _manager.GetAllExpenses();
            Console.WriteLine("--- RECORDED EXPENSES ---");
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        private static void ViewTotalUI()
        {
            decimal total = _manager.CalculateTotal();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Total Expense Accumulated: ₹{total:F2}");
        }

        private static void DeleteExpenseUI()
        {
            ViewExpensesUI();
            Console.Write("\nEnter Expense ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                throw new FormatException("ID must be a valid integer.");
            }

            _manager.DeleteExpense(id);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ Expense ID {id} deleted successfully!");
        }
    }
}

