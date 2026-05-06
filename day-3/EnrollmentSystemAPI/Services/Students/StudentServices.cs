using EnrollmentSystem.Models;

namespace EnrollmentSystem.Services.Student
{
    public class StudentServices : IStudentServices
    {
        private readonly List<Student> _students = new()
        {
            new Student
            {
                Id = 1,
                FirstName = "Roosc",
                LastName = "Zaño",
                Age = 21,
                SectionId = 1,
                Gender = 'M'
            },
            new Student
            {
                Id = 2,
                FirstName = "Kurt",
                LastName = "Laja",
                Age = 35,
                SectionId = 2,
                Gender = 'F'
            }
        };

        public List<Student> GetAll()
        {
            return _students;
        }

        public Student? GetById(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id);
        }

        public List<Student> Search(string? firstName, string? lastName, int? age, int? sectionId, char? gender)
        {
            var result = _students.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(firstName))
                result = result.Where(s => s.FirstName.Contains(firstName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(lastName))
                result = result.Where(s => s.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase));

            if (age.HasValue)
                result = result.Where(s => s.Age == age.Value);

            if (sectionId.HasValue)
                result = result.Where(s => s.SectionId == sectionId.Value);

            if (gender.HasValue)
                result = result.Where(s => s.Gender == gender.Value);

            return result.ToList();
        }

        public void Create(Student student)
        {
            student.Id = _students.Count == 0 ? 1 : _students.Max(s => s.Id) + 1;
            _students.Add(student);
        }

        public void Update(int id, Student updated)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null) return;

            student.FirstName = updated.FirstName;
            student.LastName = updated.LastName;
            student.Age = updated.Age;
            student.SectionId = updated.SectionId;
            student.Gender = updated.Gender;
        }

        public void Patch(int id, Student patched)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null) return;

            student.FirstName = patched.FirstName;
            student.LastName = patched.LastName;
            student.Age = patched.Age;
            student.SectionId = patched.SectionId;
            student.Gender = patched.Gender;
        }

        public void Delete(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null) return;

            _students.Remove(student);
        }
    }
}