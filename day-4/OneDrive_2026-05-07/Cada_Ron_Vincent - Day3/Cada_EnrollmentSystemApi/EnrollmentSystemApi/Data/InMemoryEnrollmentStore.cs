namespace EnrollmentSystemApi.Data;

public class InMemoryEnrollmentStore
{
    public List<Section> Sections { get; } =
    [
        new Section
        {
            Id = 1,
            Code = "M2022"
        }
    ];

    public List<Student> Students { get; } =
    [
        new Student
        {
            Id = 1,
            FirstName = "Ron",
            LastName = "Cada",
            Age = 20,
            Gender = "Male",
            SectionId = 1
        }
    ];
}
