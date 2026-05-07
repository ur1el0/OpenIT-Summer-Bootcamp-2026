using EnrollmentSystemApi.Data;
using EnrollmentSystemApi.DTOs.Sections;
using EnrollmentSystemApi.models;

namespace EnrollmentSystemApi.Services.Sections;

public class SectionService(InMemoryEnrollmentStore store) : ISectionService
{
    public List<SectionResponseDTO> GetAllSections()
    {
        return [.. store.Sections.Select(MapToResponse)];
    }

    public SectionResponseDTO? GetSectionById(int id)
    {
        var section = store.Sections.FirstOrDefault(s => s.Id == id);
        return section is null ? null : MapToResponse(section);
    }

    public SectionResponseDTO? GetSectionByCode(string code)
    {
        var section = store.Sections.FirstOrDefault(s => string.Equals(s.Code, code, StringComparison.OrdinalIgnoreCase));
        return section is null ? null : MapToResponse(section);
    }

    public SectionResponseDTO Create(SectionCreateDTO sectionCreateDTO)
    {
        var nextId = store.Sections.Count == 0 ? 1 : store.Sections.Max(s => s.Id) + 1;
        var section = new Section
        {
            Id = nextId,
            Code = sectionCreateDTO.Code
        };

        store.Sections.Add(section);
        return MapToResponse(section);
    }

    public bool Update(int id, SectionUpdateDTO sectionUpdateDTO)
    {
        var section = store.Sections.FirstOrDefault(s => s.Id == id);
        if (section is null)
        {
            return false;
        }

        section.Code = sectionUpdateDTO.Code;
        return true;
    }

    public bool Patch(int id, SectionPatchDTO sectionPatchDTO)
    {
        var section = store.Sections.FirstOrDefault(s => s.Id == id);
        if (section is null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(sectionPatchDTO.Code))
        {
            section.Code = sectionPatchDTO.Code;
        }

        return true;
    }

    public bool Delete(int id)
    {
        var section = store.Sections.FirstOrDefault(s => s.Id == id);
        if (section is null)
        {
            return false;
        }

        store.Students.RemoveAll(student => student.SectionId == id);
        store.Sections.Remove(section);
        return true;
    }

    private SectionResponseDTO MapToResponse(Section section)
    {
        return new SectionResponseDTO
        {
            Id = section.Id,
            Code = section.Code,
            StudentCount = store.Students.Count(student => student.SectionId == section.Id)
        };
    }
}
