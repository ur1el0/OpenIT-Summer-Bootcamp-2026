using EnrollmentSystem.Models;

namespace EnrollmentSystem.Services.Students
{
    public interface IStudentServices
    {
        List<Student> GetAll();
        Student? GetById(int id);
        List<Student> Search(string? firstName, string? lastName, int? age, int? sectionId, char? gender);
        void Create(Student student);
        void Update(int id, Student updated);
        void Patch(int id, Student patched);
        void Delete(int id);
    }
}