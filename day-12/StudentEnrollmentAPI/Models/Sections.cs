using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentAPI.Models;

public class Sections
{
    public int Id {get;set;}
    
    [Required]
    [MaxLength(100)]
    public string Code {get;set;} = "";
    public int Year {get;set;}
    public int? ProgramId {get;set;}
}