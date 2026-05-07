<<<<<<< HEAD
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
=======
using EnrollmentSystem.Models;

namespace EnrollmentSystem.Services.Sections
{
    public interface ISectionsService
    {
        List<Section> GetAll();
    }
>>>>>>> fbffcb53571c48c8df295b2262b2029a1ff37dba
}