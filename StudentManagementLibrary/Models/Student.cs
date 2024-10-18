namespace StudentManagementLibrary
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        //public override string ToString() => $"{FirstName}, {LastName}, {Age}";
        public override string ToString() => $"{FirstName.Trim()}, {LastName.Trim()}, {Age}"; 
        
    }
}
