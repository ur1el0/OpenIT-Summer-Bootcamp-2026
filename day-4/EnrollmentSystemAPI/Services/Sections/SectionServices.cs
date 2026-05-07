<<<<<<< HEAD
using System.Linq;
using System.Collections.Generic;
using EnrollmentSystemApi.Data;
using EnrollmentSystemApi.DTOs.Sections;
using EnrollmentSystemApi.Models;

namespace EnrollmentSystemApi.Services.Sections;

public class SectionService : ISectionService
{
    private readonly InMemoryEnrollmentStore store;

    public SectionService(InMemoryEnrollmentStore store)
    {
        this.store = store;
    }

    public List<SectionResponseDTO> GetAllSections()
    {
        return store.Sections.Select(MapToResponse).ToList();
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

    public bool Update(int id, SectionCreateDTO sectionUpdateDTO)
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
=======
using EnrollmentSystem.Models;

namespace EnrollmentSystem.Services.Sections
{
    public class SectionServices : ISectionsService
    {
        private readonly List<Section> _sections = new()
        {
            new Section
            {
                Id = 1,
                Code = "BSIT-1A",
                Students = []
            },
            new Section
            {
                Id = 2,
                Code = "BSIT-1B",
                Students = []
            }
        };

        public List<Section> GetAll()
        {
            return _sections;
        }
    }
}
>>>>>>> fbffcb53571c48c8df295b2262b2029a1ff37dba
