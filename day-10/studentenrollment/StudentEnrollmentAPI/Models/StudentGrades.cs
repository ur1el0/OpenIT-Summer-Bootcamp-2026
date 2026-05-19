using System.Text.Json.Serialization;
using StudentEnrollmentAPI.Models;

namespace EnrollmentSystem;


public class StudentGrades
{
    public int Id {get;set;}
    public int StudentId {get;set;}
    public int Student_SectionsId {get;set;}
    public int grade {get;set;}
    
    [JsonIgnore]
    public Student_Sections? Student_Sections {get;set;}
    
    public Students? Student {get;set;}
    public Sections? Section {get;set;}
}