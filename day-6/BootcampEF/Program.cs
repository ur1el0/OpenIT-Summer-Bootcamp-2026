using System;
using Microsoft.EntityFrameworkCore;
using BootcampEF;


// var students = context.Students
// .Include(s=>s.Course)
// .ToList();

// var student = new Student
// {
//     Name="John",
//     Course = course
// };


// context.Students.Add(student);
// context.SaveChanges();



// Console.WriteLine("\nStudent List:");

// foreach (var s in students)
// {
//     Console.WriteLine($"{s.Id} - {s.Name} - {s.Course}");
// }

// var firstStudent = context.Students.FirstOrDefault();

// if (firstStudent != null)
// {
//     firstStudent.Name="Updated John";

//     context.SaveChanges();

//     Console.WriteLine("\nStudent Updated!");
// }


// var deleteStudent = context.Students.FirstOrDefault();
// if(deleteStudent != null)
// {
//     context.Students.Remove(deleteStudent);

//     context.SaveChanges();
//     Console.WriteLine("\nStudent Deleted!");
// }


using var context = new StudentContext();

// context.Database.Migrate();

// var course = new Course
// {
//     Name = "IS",
//     Students =
//     [
//         new() { Name = "Johnson" },
//         new() { Name = "Anna" },
//         new() { Name = "Mark" },
//     ]
// };

// context.Courses.Add(course);
// context.SaveChanges();

// Console.WriteLine("Course and students added!");


// var students = context.Students
//     .Include(s=>s.Course)
//     .OrderBy(s => s.Course.Name)
//     .ThenBy(s=>s.Name)
//     .ToList();

// foreach (var s in students)
// {
//     Console.WriteLine($"{s.Course.Name} - {s.Name}");
// }

var students = context.Students
.Include(s=>s.Course)
.Where(s=>s.Course.Name == "IS")
.ToList();

foreach (var s in students)
{
    Console.WriteLine($"{s.Course.Name} - {s.Name}");
}