using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using EnrollmentSystem;

namespace StudentEnrollmentAPI.Models;

public class Student_Sections
{
    public int Id {get;set;}
     
    [Required]
    public int StudentId {get;set;}
    [Required]
    public int SectionId {get;set;}
    public DateTime EnrolledAt {get;set;} = DateTime.UtcNow;
    public Students? Student {get;set;}
    public Sections? Section {get;set;}
    
    [JsonIgnore]
    public List<StudentGrades> StudentGrades {get;set;} = [];

}