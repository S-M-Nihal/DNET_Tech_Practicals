using System;
using System.Collections.Generic;

namespace StudentAdmissionManagement
{
    public class Student
    {
        // Encapsulation using C# Auto-Properties with Private Setters
        public int StudentId { get; private set; }
        public string Name { get; private set; }
        public string Course { get; private set; }
        public double Percentage { get; private set; }
        public string AdmissionStatus { get; private set; }

        // Primary Constructor
        public Student(int id, string name, string course, double percentage)
        {
            StudentId = id;
            Name = name;
            Course = course;
            Percentage = percentage;

            EvaluateAdmission();
        }

        // Constructor Overloading (Default values if cutoff is standard)
        public Student(int id, string name, string course)
            : this(id, name, course, 0.0) { }

        // Private Encapsulated Method
        private void EvaluateAdmission()
        {
            // Business Logic: 50% cutoff mark
            AdmissionStatus = Percentage >= 50.0 ? "Approved" : "Rejected";
        }

        // Public Presentation Method
        public void DisplayStudentDetails()
        {
            Console.WriteLine($"| {StudentId,-5} | {Name,-15} | {Course,-22} | {Percentage,6:F1}% | {AdmissionStatus,-8} |");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Student Admission Management System";
            Console.WriteLine("==========================================================================");
            Console.WriteLine("                  STUDENT ADMISSION MANAGEMENT MODULE                     ");
            Console.WriteLine("==========================================================================");

            // Using Generic List to store Object Instances dynamically
            List<Student> studentList = new List<Student>
            {
                new Student(101, "Khushal", "Computer Science", 95.6),
                new Student(102, "Nihal", "Information Technology", 44.2), // Sample dynamic rejected case
                new Student(103, "Saaho", "Cyber Security", 77.4),
                new Student(104, "Rajamouli", "Architecture", 88.0) 
            };

            // Display Table Header
            Console.WriteLine("\n| ID    | Name            | Course                 | Marks   | Status   |");
            Console.WriteLine("--------------------------------------------------------------------------");

            // Iterating through object collection
            foreach (var student in studentList)
            {
                student.DisplayStudentDetails();
            }

            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}