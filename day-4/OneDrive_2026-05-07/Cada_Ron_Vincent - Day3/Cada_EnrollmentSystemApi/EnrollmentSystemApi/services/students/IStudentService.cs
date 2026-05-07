using EnrollmentSystemApi.DTOs.Students;

namespace EnrollmentSystemApi.Services.Students;

public interface IStudentService
{
    List<StudentResponseDTO> GetAllStudents();
    List<StudentResponseDTO> GetStudents(string? firstName, string? lastName, string? gender, int? age);
    StudentResponseDTO? GetStudentById(int id);
    List<StudentResponseDTO>? GetStudentsBySectionCode(string sectionCode);
    List<StudentResponseDTO>? GetStudentsBySectionCode(string sectionCode, string? firstName, string? lastName, string? gender, int? age);
    StudentResponseDTO? GetStudentBySectionCodeAndId(string sectionCode, int studentId);
    StudentResponseDTO? Create(StudentCreateDTO studentCreateDTO);
    StudentResponseDTO? CreateInSection(string sectionCode, StudentCreateDTO studentCreateDTO);
    bool Update(int id, StudentUpdateDTO studentUpdateDTO);
    bool UpdateInSection(string sectionCode, int studentId, StudentUpdateDTO studentUpdateDTO);
    bool Patch(int id, StudentPatchDTO studentPatchDTO);
    bool PatchInSection(string sectionCode, int studentId, StudentPatchDTO studentPatchDTO);
    bool Delete(int id);
    bool DeleteInSection(string sectionCode, int studentId);
}
