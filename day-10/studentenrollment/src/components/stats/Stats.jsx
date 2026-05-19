const Stats = ({ students, programs }) => 
{
    const totalStudents = students.length;
    const totalPrograms = programs.length;
    const enrolledStudents = students.filter ((student) => student.enrolled).length;
    const avgGrade = students.map((student) => Number(student.avgGrade)).filter((grade) => Number.isFinite(grade)).reduce((sum, grade) => sum + grade, 0) / (students.length || 1);
    
    return (
        <div className="stats">
            <div className="stat">
                <p>{totalStudents}</p>
                <h3>Total Students</h3 >
            </div>
            <div className="stat">
                <p>{enrolledStudents}</p>
                <h3>Enrolled Students</h3>
            </div>
            <div className="stat">
                <p>{totalPrograms}</p>
                <h3>Total Programs</h3>
            </div>
            <div className="stat">
                <p>{avgGrade.toFixed(2)}</p>
                <h3>Average Grade</h3>
            </div>
        </div>
    )
    
}

export default Stats;