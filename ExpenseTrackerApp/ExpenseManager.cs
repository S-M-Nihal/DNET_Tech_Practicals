using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ExpenseTrackerApp
{
    public class ExpenseManager
    {
        private List<Expense> _expenses = new List<Expense>();
        private readonly string _filePath = "expenses.json";
        private int _nextId = 1;

        public ExpenseManager()
        {
            LoadData();
        }

        public void AddExpense(string title, decimal amount, string category)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Expense title cannot be empty.");

            if (amount <= 0)
                throw new InvalidAmountException("Expense amount must be greater than zero.");

            var expense = new Expense
            {
                Id = _nextId++,
                Title = title.Trim(),
                Amount = amount,
                Category = string.IsNullOrWhiteSpace(category) ? "General" : category.Trim(),
                Date = DateTime.Now
            };

            _expenses.Add(expense);
            SaveData();
        }

        public List<Expense> GetAllExpenses()
        {
            if (!_expenses.Any())
                throw new ExpenseNotFoundException("No expenses recorded yet.");

            return _expenses;
        }

        public void DeleteExpense(int id)
        {
            var expense = _expenses.FirstOrDefault(e => e.Id == id);
            if (expense == null)
                throw new ExpenseNotFoundException($"Expense with ID {id} was not found.");

            _expenses.Remove(expense);
            SaveData();
        }

        public decimal CalculateTotal()
        {
            return _expenses.Sum(e => e.Amount);
        }

        private void SaveData()
        {
            try
            {
                string json = JsonSerializer.Serialize(_expenses, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
            catch (IOException ex)
            {
                throw new Exception($"File I/O Error while saving data: {ex.Message}");
            }
        }

        private void LoadData()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    _expenses = JsonSerializer.Deserialize<List<Expense>>(json) ?? new List<Expense>();
                    if (_expenses.Any())
                    {
                        _nextId = _expenses.Max(e => e.Id) + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Warning] Failed to load previous data: {ex.Message}");
                _expenses = new List<Expense>();
            }
        }
    }
}