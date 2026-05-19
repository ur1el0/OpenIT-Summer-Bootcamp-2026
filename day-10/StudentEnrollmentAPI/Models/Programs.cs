using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentAPI.Models;

public class Programs
{
    public int Id {get; set;}
    [Required]
    [MaxLength(150)]
    public string ProgramName {get;set;} = "";
}