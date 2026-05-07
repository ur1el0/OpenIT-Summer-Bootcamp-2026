using EnrollmentSystemApi.Data;
using EnrollmentSystemApi.DTOs.Students;
using EnrollmentSystemApi.models;

namespace EnrollmentSystemApi.Services.Students;

public class StudentService(InMemoryEnrollmentStore store) : IStudentService
{
    public List<StudentResponseDTO> GetAllStudents()
    {
        return [.. store.Students.Select(MapToResponse)];
    }

    public StudentResponseDTO? GetStudentById(int id)
    {
        var student = store.Students.FirstOrDefault(s => s.Id == id);
        return student is null ? null : MapToResponse(student);
    }

    public List<StudentResponseDTO>? GetStudentsBySectionCode(string sectionCode)
    {
        var section = FindSectionByCode(sectionCode);
        if (section is null)
        {
            return null;
        }

        return [.. store.Students
            .Where(student => student.SectionId == section.Id)
            .Select(MapToResponse)];
    }

    public StudentResponseDTO? GetStudentBySectionCodeAndId(string sectionCode, int studentId)
    {
        var section = FindSectionByCode(sectionCode);
        if (section is null)
        {
            return null;
        }

        var student = store.Students.FirstOrDefault(s => s.SectionId == section.Id && s.Id == studentId);
        return student is null ? null : MapToResponse(student);
    }

    public List<StudentResponseDTO> GetStudents(string? firstName, string? lastName, string? gender, int? age)
    {
        return [.. ApplyStudentSearch(store.Students, firstName, lastName, gender, age).Select(MapToResponse)];
    }

    public List<StudentResponseDTO>? GetStudentsBySectionCode(
        string sectionCode,
        string? firstName,
        string? lastName,
        string? gender,
        int? age)
    {
        var section = FindSectionByCode(sectionCode);
        if (section is null)
        {
            return null;
        }

        var sectionStudents = store.Students.Where(student => student.SectionId == section.Id);
        return [.. ApplyStudentSearch(sectionStudents, firstName, lastName, gender, age).Select(MapToResponse)];
    }

    public StudentResponseDTO? Create(StudentCreateDTO studentCreateDTO)
    {
        if (!SectionExists(studentCreateDTO.SectionId))
        {
            return null;
        }

        var nextId = store.Students.Count == 0 ? 1 : store.Students.Max(s => s.Id) + 1;
        var newStudent = new Student
        {
            Id = nextId,
            FirstName = studentCreateDTO.FirstName,
            LastName = studentCreateDTO.LastName,
            Age = studentCreateDTO.Age,
            Gender = studentCreateDTO.Gender,
            SectionId = studentCreateDTO.SectionId
        };

        store.Students.Add(newStudent);
        return MapToResponse(newStudent);
    }

    public StudentResponseDTO? CreateInSection(string sectionCode, StudentCreateDTO studentCreateDTO)
    {
        var section = FindSectionByCode(sectionCode);
        if (section is null)
        {
            return null;
        }

        studentCreateDTO.SectionId = section.Id;
        return Create(studentCreateDTO);
    }

    public bool Update(int id, StudentUpdateDTO studentUpdateDTO)
    {
        if (!SectionExists(studentUpdateDTO.SectionId))
        {
            return false;
        }

        var student = store.Students.FirstOrDefault(s => s.Id == id);
        if (student is null)
        {
            return false;
        }

        student.FirstName = studentUpdateDTO.FirstName;
        student.LastName = studentUpdateDTO.LastName;
        student.Age = studentUpdateDTO.Age;
        student.Gender = studentUpdateDTO.Gender;
        student.SectionId = studentUpdateDTO.SectionId;
        return true;
    }

    public bool UpdateInSection(string sectionCode, int studentId, StudentUpdateDTO studentUpdateDTO)
    {
        var section = FindSectionByCode(sectionCode);
        var student = section is null
            ? null
            : store.Students.FirstOrDefault(s => s.SectionId == section.Id && s.Id == studentId);

        if (section is null || student is null)
        {
            return false;
        }

        studentUpdateDTO.SectionId = section.Id;
        return Update(studentId, studentUpdateDTO);
    }

    public bool Patch(int id, StudentPatchDTO studentPatchDTO)
    {
        if (studentPatchDTO.SectionId.HasValue && !SectionExists(studentPatchDTO.SectionId.Value))
        {
            return false;
        }

        var student = store.Students.FirstOrDefault(s => s.Id == id);
        if (student is null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(studentPatchDTO.FirstName))
        {
            student.FirstName = studentPatchDTO.FirstName;
        }

        if (!string.IsNullOrWhiteSpace(studentPatchDTO.LastName))
        {
            student.LastName = studentPatchDTO.LastName;
        }

        if (studentPatchDTO.Age.HasValue)
        {
            student.Age = studentPatchDTO.Age.Value;
        }

        if (!string.IsNullOrWhiteSpace(studentPatchDTO.Gender))
        {
            student.Gender = studentPatchDTO.Gender;
        }

        if (studentPatchDTO.SectionId.HasValue)
        {
            student.SectionId = studentPatchDTO.SectionId.Value;
        }

        return true;
    }

    public bool PatchInSection(string sectionCode, int studentId, StudentPatchDTO studentPatchDTO)
    {
        var section = FindSectionByCode(sectionCode);
        var student = section is null
            ? null
            : store.Students.FirstOrDefault(s => s.SectionId == section.Id && s.Id == studentId);

        if (section is null || student is null)
        {
            return false;
        }

        studentPatchDTO.SectionId = section.Id;
        return Patch(studentId, studentPatchDTO);
    }

    public bool Delete(int id)
    {
        var student = store.Students.FirstOrDefault(s => s.Id == id);
        if (student is null)
        {
            return false;
        }

        store.Students.Remove(student);
        return true;
    }

    public bool DeleteInSection(string sectionCode, int studentId)
    {
        var section = FindSectionByCode(sectionCode);
        var student = section is null
            ? null
            : store.Students.FirstOrDefault(s => s.SectionId == section.Id && s.Id == studentId);

        if (student is null)
        {
            return false;
        }

        store.Students.Remove(student);
        return true;
    }

    private static IEnumerable<Student> ApplyStudentSearch(
        IEnumerable<Student> students,
        string? firstName,
        string? lastName,
        string? gender,
        int? age)
    {
        var result = students;

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            result = result.Where(s => s.FirstName.Contains(firstName, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            result = result.Where(s => s.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(gender))
        {
            result = result.Where(s => string.Equals(s.Gender, gender, StringComparison.OrdinalIgnoreCase));
        }

        if (age.HasValue)
        {
            result = result.Where(s => s.Age == age.Value);
        }

        return result;
    }

    private bool SectionExists(int sectionId)
    {
        return store.Sections.Any(section => section.Id == sectionId);
    }

    private Section? FindSectionByCode(string sectionCode)
    {
        return store.Sections.FirstOrDefault(s => string.Equals(s.Code, sectionCode, StringComparison.OrdinalIgnoreCase));
    }

    private StudentResponseDTO MapToResponse(Student student)
    {
        var sectionCode = store.Sections.FirstOrDefault(section => section.Id == student.SectionId)?.Code ?? string.Empty;

        return new StudentResponseDTO
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Age = student.Age,
            Gender = student.Gender,
            SectionCode = sectionCode
        };
    }
}
