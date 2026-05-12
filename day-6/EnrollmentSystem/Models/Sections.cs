using System.ComponentModel.DataAnnotations;

namespace EnrollmentSystem;

public class Sections
{
    public int Id {get;set;}
    
    [Required]
    [MaxLength(100)]
    public string Code {get;set;} = "";
    public int Year {get;set;}
    public int? ProgramId {get;set;}
}