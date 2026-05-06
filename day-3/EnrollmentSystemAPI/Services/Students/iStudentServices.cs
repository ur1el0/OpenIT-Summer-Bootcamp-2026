namespace EnrollmentSystem.Services.Student
{
    public class StudentServices : IStudentServices
    {
        public List<Student> GetAll();
        public Student? GetById(int id);
        public List<Student> Search(string? FirstName, string? LastName, int? Age, int? SectionId, char? Gender);
        public void Create(Student student);
        public void Update(int id, Student updated);
        public void Patch(int id, Student patched);
        public void Delete(int id);
    }
}