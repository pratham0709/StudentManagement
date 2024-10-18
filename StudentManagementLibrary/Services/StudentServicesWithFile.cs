using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace StudentManagementLibrary
{
    public class StudentServicesWithFile : IStudentService
    {
        private readonly List<Student> mystudents = new List<Student>();
        private const string CsvFilePath = "C:\\Users\\Prathamesh\\source\\repos\\Final Task\\students.csv";

        public StudentServicesWithFile()
        {
            LoadStudentsFromCsv();
        }

        public void AddStudent(Student student)
        {
            if (!IsValidStudent(student)) return;
            student.FirstName = student.FirstName.Trim();
            student.LastName = student.LastName.Trim();
            mystudents.Add(student);
            SaveStudentsToCsv();
        }

        public List<Student> GetAllStudents() => mystudents;

        public void DeleteStudent(string firstName, string lastName)
        {
            var studentToRemove = mystudents.FirstOrDefault(s => s.FirstName.Trim() == firstName.Trim() &&
                                                                s.LastName.Trim() == lastName.Trim());
            if (studentToRemove != null)
            {
                mystudents.Remove(studentToRemove);
                SaveStudentsToCsv(); // Update CSV after deletion
            }
        }

        public List<Student> SearchStudent(string firstName = null, string lastName = null, int? age = null)
        {
            var students = mystudents.AsQueryable();
            if (!string.IsNullOrEmpty(firstName))
                students = students.Where(s => s.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(lastName))
                students = students.Where(s => s.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase));
            if (age.HasValue)
                students = students.Where(s => s.Age == age.Value);
            return students.ToList();
        }

        public bool IsValidStudent(Student student)
        {
            return Regex.IsMatch(student.FirstName, @"^[A-Za-z]+$") &&
                   Regex.IsMatch(student.LastName, @"^[A-Za-z]+$") &&
                   student.Age > 0 && student.Age < 120;
        }

        private void LoadStudentsFromCsv()
        {
            if (File.Exists(CsvFilePath))
            {
                var lines = File.ReadAllLines(CsvFilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 3 && int.TryParse(parts[2], out int age))
                    {
                        mystudents.Add(new Student
                        {
                            FirstName = parts[0],
                            LastName = parts[1],
                            Age = age
                        });
                    }
                }
            }
        }

        private void SaveStudentsToCsv()
        {
            var csvContent = new List<string>();
            foreach (var student in mystudents)
            {
                csvContent.Add(student.ToString());
            }
            File.WriteAllLines(CsvFilePath, csvContent);
        }
    }
}
