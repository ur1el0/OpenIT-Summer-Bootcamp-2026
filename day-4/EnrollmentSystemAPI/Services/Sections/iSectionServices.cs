using EnrollmentSystemApi.DTOs.Sections;

namespace EnrollmentSystemApi.Services.Sections;

public interface ISectionService
{
    List<SectionResponseDTO> GetAllSections();
    SectionResponseDTO? GetSectionById(int id);
    SectionResponseDTO? GetSectionByCode(string code);
    SectionResponseDTO Create(SectionCreateDTO sectionCreateDTO);
    bool Update(int id, SectionCreateDTO sectionUpdateDTO);
    bool Patch(int id, SectionPatchDTO sectionPatchDTO);
    bool Delete(int id);
}