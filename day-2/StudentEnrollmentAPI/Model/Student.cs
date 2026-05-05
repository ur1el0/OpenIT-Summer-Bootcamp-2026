using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentAPI.Model;

public class Student
{
    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public int Year { get; }
    public string Course { get; }
    public string Gender { get; }
    public string OldSchool { get; }
    public string Enrolled { get; set; }
    public string Department { get; }
    public string Address { get; }

    public Student(int id, string firstName, string lastName, int year, string course, string gender, string oldSchool, string enrolled, string department, string address)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Year = year;
        Course = course;
        Gender = gender;
        OldSchool = oldSchool;
        Enrolled = enrolled;
        Department = department;
        Address = address;
    }
}