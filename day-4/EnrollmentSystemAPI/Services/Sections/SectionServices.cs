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