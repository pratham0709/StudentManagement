using StudentManagementLibrary.ServiceModels;
using System.Collections.Generic;

namespace StudentManagementLibrary
{
    public interface IStudentService
    {
        ResultBase AddStudent(Student student);
        List<Student> GetAllStudents();
        ResultBase DeleteStudent(string firstName, string lastName);
        List<Student> SearchStudent(string firstName = null, string lastName = null, int? age = null);
        bool IsValidStudent(Student student);
    }
}
