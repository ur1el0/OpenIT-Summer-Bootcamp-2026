using Microsoft.EntityFrameworkCore;

namespace BootcampEF;
public class StudentContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses{ get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>().ToTable("Course");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=bootcamp;Username=postgres;Password=ccms"
        );
    }
}