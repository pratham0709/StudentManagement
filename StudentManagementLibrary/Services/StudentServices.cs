using Dapper;
using MySql.Data.MySqlClient;
using StudentManagementLibrary.ServiceModels;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StudentManagementLibrary
{
    public class StudentServices : IStudentService
    {
        private readonly MySqlConnection myconnection;

        public StudentServices(MySqlConnection connection)
        {
            myconnection = connection;
        }

        public ResultBase AddStudent(Student student)
        {
            var result = IsValidStudent(student);
            if (!result.Success) return result;

            var sql = "INSERT INTO Students (FirstName, LastName, Age) VALUES (@FirstName, @LastName, @Age)";
            myconnection.Execute(sql, student);

            result.Success = true;
            result.Message = "Student added successfully";
            return result;
        }

        public List<Student> GetAllStudents()
        {
            var sql = "SELECT * FROM Students";
            return myconnection.Query<Student>(sql).ToList();
        }

        public ResultBase DeleteStudent(string firstName, string lastName)
        {
            var sql = "DELETE FROM Students WHERE FirstName = @FirstName AND LastName = @LastName";
            myconnection.Execute(sql, new { FirstName = firstName, LastName = lastName });
        }

        public List<Student> SearchStudent(string firstName = null, string lastName = null, int? age = null)
        {
            var sql = "SELECT * FROM Students WHERE 1=1"; // Always true condition for building dynamic query
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(firstName))
            {
                sql += " AND FirstName = @FirstName";
                parameters.Add("FirstName", firstName);
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                sql += " AND LastName = @LastName";
                parameters.Add("LastName", lastName);
            }

            if (age.HasValue)
            {
                sql += " AND Age = @Age";
                parameters.Add("Age", age.Value);
            }

            return myconnection.Query<Student>(sql, parameters).ToList();
        }

        public ResultBase IsValidStudent(Student student)
        {
            var result = new ResultBase();

            if (string.IsNullOrEmpty(student.FirstName) && string.IsNullOrEmpty(student.LastName))
            {
                result.Message = "Student name missing.";
            }
            if(!Regex.IsMatch(student.FirstName, @"^[A-Za-z]+$") && !Regex.IsMatch(student.LastName, @"^[A-Za-z]+$"))
            {
                result.Message = "Student name contains special characters.";
            }
            if(student.Age < 0)
            {
                result.Message = "Student age is less than zero";
            }
            if(student.Age > 120)
            {
                result.Message = "Student age is to large to set";
            }
            return result;            
        }
    }
}
