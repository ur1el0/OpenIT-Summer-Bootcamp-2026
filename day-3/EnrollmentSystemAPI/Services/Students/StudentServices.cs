namespace EnrollmentSystem.Services.Student
{
    public class StudentServices : IStudentServices
    {
        public readonly List<Student> _students = new List<Student>
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

        public List <Student> GetAll()
        {
            return _students;
        }

        public Student? GetById(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            return student;
        }
        public List<Student> Search(string? FirstName, string? LastName, int? Age, int? SectionId, char? Gender)
        {
            if (!string.IsNullOrEmpty(FirstName))
                result = result.Where(s => s.FirstName.Contains(FirstName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(LastName))
                result = result.Where(s => s.LastName.Contains(LastName, StringComparison.OrdinalIgnoreCase));

            if (Age.HasValue)
                result = result.Where(s => s.Age == Age.Value);

            if (SectionId.HasValue)
                result = result.Where(s => s.SectionId == SectionId.Value);

            if (Gender.HasValue)
                result = result.Where(s => s.Gender == Gender.Value);

            return result.ToList();
        }
        public void Create(Student student)
        {
            student.id = _students.Count == 0 ? 1 : _students.Max(s => s.Id) + 1;
            _students.Add(book);
        }

        public void Update(int id, Student updated)
        {
            var update = _students.FirstOrDefault(s => s.Id == id);
            if (update == null) return;
            update.FirstName = updated.FirstName;
            update.LastName = updated.LastName;
            update.Age = updated.Age;
            update.Gender = updated.Gender;
        }

        public void Patch(int id, Student patch)
        {
            var update = _students.FirstOrDefault(s => s.Id == id);
            if (patch == null) return;
            if (patch.FirstName != null) update.FirstName = patch.FirstName;
            if (patch.LastName != null) update.LastName = patch.FirstName;
            if (patch.Age != null) update.Age = patch.Age;
            if (patch.Gender != null) update.Gender = patch.Gender;
        }

        public void Delete(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null) return;
            _students.Remove(student);
        }

    }
}