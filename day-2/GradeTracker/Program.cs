class Student
{
    public int id{ get; set; }
    public string name{ get; set; }
    public string course{ get; set; }
    public List<double> grades{ get; set; } = new();
    

    public Student(int id, string name, string course)
    {
        this.id = id;
        this.name = name;
        this.course = course;
        grades = new List<double>();
    }

    public double GetAverage()
    {
        if (grades.Count == 0) return 0;
        double sum = 0;
        foreach (double grade in grades)
        {
            sum += grade;
        }
        return sum / grades.Count;
    }

    public bool IsHonor()
    {
        return GetAverage() >= 87.0;
    }

    public override string ToString()
    {
        return $"{id} {name} {course} - Average Grade: {GetAverage():F2}";
    }

    public int GetId()
    {
        return id;
    }

    public string GetName()
    {
        return name;
    }

    public string GetCourse()
    {
        return course;
    }
}

class MainClass
{
    static void Main(String[] args)
    {
        List<Student> students = new List<Student>();

        students.Add(new Student(1, "Mike Gomez", "IT"));
        students.Add(new Student(2, "Kurt Laja", "CS"));
        students.Add(new Student(3, "Roosc Zaño", "IT"));
        students.Add(new Student(4, "Ron Cada", "IT"));
        students.Add(new Student(5, "Neo Medrano", "CS"));

        students[0].grades.AddRange(new double[] { 85.0, 90.0, 78.0 });
        students[1].grades.AddRange(new double[] { 92.0, 88.0, 95.0 });
        students[2].grades.AddRange(new double[] { 80.0, 82.0, 79.0 });
        students[3].grades.AddRange(new double[] { 88.0, 91.0, 87.0 });
        students[4].grades.AddRange(new double[] { 95.0, 94.0, 96.0 });

        List<Student> rankedStudents = students
            .OrderByDescending(s => s.GetAverage())
            .ToList();

        Student topStudent = rankedStudents.First();

        Console.WriteLine($"Top Student: {topStudent.GetName()}({topStudent.GetCourse()}) - Average Grade: {topStudent.GetAverage():F2}");
        Console.WriteLine($"\nHonor: {string.Join(", ", students.Where(s => s.IsHonor()).Select(s => s.GetName()))}");
        Console.WriteLine($"\nCS: {students.Count(s => s.GetCourse() == "CS")} students | IT: {students.Count(s => s.GetCourse() == "IT")} students\n");

        for (int i = 0; i < rankedStudents.Count; i++)
        {
            Console.WriteLine($"Rank {i + 1}: {rankedStudents[i].GetName()} - Average Grade: {rankedStudents[i].GetAverage():F2}");
        }

        Console.WriteLine("\nStudents Grouped by Course:");
        var groupedByCourse = students.GroupBy(s => s.course);
        foreach (var group in groupedByCourse)
        {
            Console.WriteLine($"\nCourse: {group.Key}");
            foreach(var student in group)
            {
                Console.WriteLine($" - {student.GetName()}");
            }
        }
        Console.WriteLine("\nTop 3 Per Course:");
        foreach (var group in groupedByCourse)
        {
            Console.WriteLine($"\nCourse: {group.Key}");
            var top3 = group.OrderByDescending(s => s.GetAverage()).Take(3);
            foreach (var student in top3)
            {
                Console.WriteLine($" - {student.GetName()}: {student.GetAverage():F2}");
            }
        }
    }
}