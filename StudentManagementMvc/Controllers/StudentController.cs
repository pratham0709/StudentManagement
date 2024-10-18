using Microsoft.AspNetCore.Mvc;
using StudentManagementLibrary;

public class StudentController : Controller
{
    private readonly IStudentService mystudentService;

    public StudentController(IStudentService studentService)
    {
        mystudentService = studentService;
    }


    public IActionResult Index(string firstName = null, string lastName = null, int? age = null)
    {
        var students = mystudentService.SearchStudent(firstName, lastName, age);
        return View(students);
    }

    [HttpPost]
    public IActionResult Add(Student student)
    {
        mystudentService.AddStudent(student);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(string firstName, string lastName)
    {
        mystudentService.DeleteStudent(firstName, lastName);
        return RedirectToAction("Index");
    }

}
