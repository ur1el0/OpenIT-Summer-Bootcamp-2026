using EnrollmentSystem;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace StudentEnrollmentAPI;

public class EnrollmentContext : IdentityDbContext<ApplicationUser>
{
    public EnrollmentContext(DbContextOptions<EnrollmentContext> options)
        : base(options)
    {
    }

    public DbSet<Students> Students { get; set; } = null!;
    public DbSet<Programs> Programs {get;set;}
    public DbSet<Sections> Sections {get;set;}
    public DbSet<StudentSections> Student_Sections {get;set;}
    public DbSet<StudentGrades> StudentGrades {get;set;}
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=studentenrollmentsystemef;Username=postgres;Password=ccms"
            );
        }
    }
}
