using System;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentSystem;

public class EnrollmentContext : DbContext
{
    public DbSet<Students>Students {get;set;}
    public DbSet<Programs> Programs {get;set;}
    public DbSet<Sections> Sections {get;set;}
    public DbSet<Student_Sections> Student_Sections {get;set;}
    public DbSet<StudentGrades> StudentGrades {get;set;}
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=EnrollmentSystemEF;Username=postgres;Password=ccms"
        );
    }
}
