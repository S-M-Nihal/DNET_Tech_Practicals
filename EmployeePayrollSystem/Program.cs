using System;
using System.Collections.Generic;

namespace EmployeePayrollSystem
{
    
    public interface IPayable
    {
        double CalculateSalary();
    }

    public abstract class Employee : IPayable
    {
        protected int id;
        protected string name;

        public Employee(int empId, string empName)
        {
            id = empId;
            name = empName;
        }

        public string Name => name;

        
        public abstract double CalculateSalary();

        public virtual void DisplayPayrollDetails()
        {
            Console.WriteLine($"ID: {id} | Name: {name} | Monthly Salary: ${CalculateSalary():N2}");
        }
    }

    
    public class FullTimeEmployee : Employee
    {
        private double monthlySalary;
        private double bonus;

        public FullTimeEmployee(int empId, string empName, double baseSalary, double empBonus)
            : base(empId, empName)
        {
            monthlySalary = baseSalary;
            bonus = empBonus;
        }

        public override double CalculateSalary()
        {
            return monthlySalary + bonus;
        }

        public override void DisplayPayrollDetails()
        {
            Console.Write("[Full-Time] ");
            base.DisplayPayrollDetails();
        }
    }

    
    public class PartTimeEmployee : Employee
    {
        private double hourlyRate;
        private int hoursWorked;

        public PartTimeEmployee(int empId, string empName, double rate, int hours)
            : base(empId, empName)
        {
            hourlyRate = rate;
            hoursWorked = hours;
        }

        public override double CalculateSalary()
        {
            return hourlyRate * hoursWorked;
        }

        public override void DisplayPayrollDetails()
        {
            Console.Write("[Part-Time] ");
            base.DisplayPayrollDetails();
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Enterprise Employee Payroll System ===\n");

            
         
            List<Employee> payrollList = new List<Employee>();

            payrollList.Add(new FullTimeEmployee(101, "Nihal", 13000.00, 750.00));
            payrollList.Add(new PartTimeEmployee(102, "Charles Leclerc", 90.50, 80));
            payrollList.Add(new FullTimeEmployee(103, "Lewis Hamilton", 12500.00, 300.00));
            payrollList.Add(new PartTimeEmployee(104, "Max Verstappen", 100.00, 65));

            
            Console.WriteLine("Processing Payroll Records...\n");
            foreach (Employee emp in payrollList)
            {
                
                emp.DisplayPayrollDetails();
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}