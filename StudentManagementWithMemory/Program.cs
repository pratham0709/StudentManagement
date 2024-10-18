using StudentManagementLibrary;
using System;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IStudentService, StudentServicesWithMemory>()
            .BuildServiceProvider();

        var service = serviceProvider.GetService<IStudentService>();

        // Example usage
        while (true)
        {
            DisplayMenu();
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddStudent(service);
                    break;
                case "2":
                    ShowAllStudents(service);
                    break;
                case "3":
                    SearchStudent(service);
                    break;
                case "4":
                    DeleteStudent(service);
                    break;
                case "5":
                    Console.WriteLine("Exiting the program. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
            Console.WriteLine();
        }
    }

    private static void DisplayMenu()
    {
        Console.WriteLine("=== Student Management System ===");
        Console.WriteLine("1. Add Student");
        Console.WriteLine("2. Show All Students");
        Console.WriteLine("3. Search Student");
        Console.WriteLine("4. Delete Student");
        Console.WriteLine("5. Exit");
        Console.Write("Choose an option: ");
    }

    private static void AddStudent(IStudentService service)
    {
        Console.Write("Enter First Name: ");
        var firstName = Console.ReadLine();
        Console.Write("Enter Last Name: ");
        var lastName = Console.ReadLine();
        Console.Write("Enter Age: ");

        if (int.TryParse(Console.ReadLine(), out int age))
        {
            var student = new Student { FirstName = firstName, LastName = lastName, Age = age };
            if (service.IsValidStudent(student))
            {
                service.AddStudent(student);
                Console.WriteLine("Student added successfully!");
            }
            else
            {
                Console.WriteLine("Invalid student data. Please ensure all fields are valid.");
            }

        }
        else
        {
            Console.WriteLine("Invalid age. Must be a number.");

        }
    }

    private static void ShowAllStudents(IStudentService service)
    {
        var students = service.GetAllStudents();
        if (students.Count == 0)
        {
            Console.WriteLine("No students found.");
        }
        else
        {
            Console.WriteLine("=== List of Students ===");
            foreach (var student in students)
            {
                Console.WriteLine(student.ToString());
            }
        }

    }

    private static void SearchStudent(IStudentService service)
    {
        Console.Write("Enter First Name (or leave empty): ");
        var firstName = Console.ReadLine();
        Console.Write("Enter Last Name (or leave empty): ");
        var lastName = Console.ReadLine();
        Console.Write("Enter Age (or leave empty): ");
        var ageInput = Console.ReadLine();
        int? age = string.IsNullOrEmpty(ageInput) ? (int?)null : int.Parse(ageInput);

        var results = service.SearchStudent(firstName, lastName, age);
        if (results.Count == 0)
        {
            Console.WriteLine("No matching students found.");
        }
        else
        {
            Console.WriteLine("=== Search Results ===");
            foreach (var student in results)
            {
                Console.WriteLine(student.ToString());
            }
        }

    }

    private static void DeleteStudent(IStudentService service)
    {
        Console.Write("Enter First Name of student to delete: ");
        var firstName = Console.ReadLine();
        Console.Write("Enter Last Name of student to delete: ");
        var lastName = Console.ReadLine();

        var students = service.GetAllStudents();
        var studentExists = students.Any(s => s.FirstName.Equals(firstName) &&
                                              s.LastName.Equals(lastName));
        if (studentExists)
        {
            service.DeleteStudent(firstName, lastName);
            Console.WriteLine($"Student '{firstName} {lastName}' deleted successfully.");
        }
        else
        {
            Console.WriteLine($"Student '{firstName} {lastName}' does not exist.");
        }
    }
}
