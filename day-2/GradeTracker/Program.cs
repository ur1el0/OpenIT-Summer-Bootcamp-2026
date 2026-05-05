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

        students[0].grades.AddRange(new double[] { 85.0, 90.0, 78.0 });
        students[1].grades.AddRange(new double[] { 92.0, 88.0, 95.0 });
        students[2].grades.AddRange(new double[] { 80.0, 82.0, 79.0 });
        students[3].grades.AddRange(new double[] { 88.0, 91.0, 87.0 });

        Student topStudent = students.OrderByDescending(s => s.GetAverage()).First();
        
        Console.WriteLine($"Top Student: {topStudent.name}({topStudent.course}) - Average Grade: {topStudent.GetAverage():F2}");
        Console.WriteLine($"\nHonor: {string.Join(", ", students.Where(s => s.IsHonor()).Select(s => s.name))}");
        Console.WriteLine($"\nCS: {students.Count(s => s.course == "CS")} students | IT: {students.Count(s => s.course == "IT")} students");
        Console.WriteLine($"\nRank 1: {topStudent.name} - Average Grade: {topStudent.GetAverage():F2} | Rank 2: {students.OrderByDescending(s => s.GetAverage()).Skip(1).First().name} - Average Grade: {students.OrderByDescending(s => s.GetAverage()).Skip(1).First().GetAverage():F2} | Rank 3: {students.OrderByDescending(s => s.GetAverage()).Skip(2).First().name} - Average Grade: {students.OrderByDescending(s => s.GetAverage()).Skip(2).First().GetAverage():F2}");
    }  
}