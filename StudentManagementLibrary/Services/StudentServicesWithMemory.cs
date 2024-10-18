using StudentManagementLibrary.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StudentManagementLibrary
{
    public class StudentServicesWithMemory : IStudentService
    {

        private readonly List<Student> mystudents = new List<Student>();


        public ResultBase AddStudent(Student student)
        {
            if (!IsValidStudent(student)) return;
            mystudents.Add(student);
        }

        public List<Student> GetAllStudents() => mystudents;

        public void DeleteStudent(string firstName, string lastName)
        {
            var studentToRemove = mystudents.FirstOrDefault(s => s.FirstName == firstName && s.LastName == lastName);
            if (studentToRemove != null) mystudents.Remove(studentToRemove);
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

        public ResultBase IsValidStudent(Student student)
        {
            var result = new ResultBase();

            if (string.IsNullOrEmpty(student.FirstName) && string.IsNullOrEmpty(student.LastName))
            {
                result.Message = "Student name missing.";
            }
            if (!Regex.IsMatch(student.FirstName, @"^[A-Za-z]+$") && !Regex.IsMatch(student.LastName, @"^[A-Za-z]+$"))
            {
                result.Message = "Student name contains special characters.";
            }
            if (student.Age < 0)
            {
                result.Message = "Student age is less than zero";
            }
            if (student.Age > 120)
            {
                result.Message = "Student age is to large to set";
            }
            return result; ;
        }
    }
}
