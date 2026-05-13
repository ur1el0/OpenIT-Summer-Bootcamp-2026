const API_BASE = 'http://localhost:5056/api';

// State
let allStudents = [];
let allPrograms = [];
let allSections = [];
let studentGrades = [];
let studentSections = [];

// Initialize
document.addEventListener('DOMContentLoaded', () => {
    loadData();
    attachEventListeners();
});

function attachEventListeners() {
    document.getElementById('searchInput').addEventListener('input', filterAndRender);
    document.getElementById('programFilter').addEventListener('change', filterAndRender);
    document.getElementById('yearFilter').addEventListener('change', filterAndRender);
}

async function loadData() {
    try {
        // Load all data in parallel
        const [students, programs, sections, grades, studSections] = await Promise.all([
            fetch(`${API_BASE}/students`).then(r => r.json()).catch(() => []),
            fetch(`${API_BASE}/programs`).then(r => r.json()).catch(() => []),
            fetch(`${API_BASE}/sections`).then(r => r.json()).catch(() => []),
            fetch(`${API_BASE}/student-grades`).then(r => r.json()).catch(() => []),
            fetch(`${API_BASE}/student-sections`).then(r => r.json()).catch(() => [])
        ]);

        allStudents = Array.isArray(students) ? students : [];
        allPrograms = Array.isArray(programs) ? programs : [];
        allSections = Array.isArray(sections) ? sections : [];
        studentGrades = Array.isArray(grades) ? grades : [];
        studentSections = Array.isArray(studSections) ? studSections : [];

        // Populate program dropdown
        populateProgramDropdown();

        // Update stats
        updateStats();

        // Render table
        filterAndRender();
    } catch (error) {
        console.error('Error loading data:', error);
    }
}

function populateProgramDropdown() {
    const select = document.getElementById('programFilter');
    allPrograms.forEach(program => {
        const option = document.createElement('option');
        option.value = program.id;
        option.textContent = program.programName;
        select.appendChild(option);
    });
}

function updateStats() {
    const totalStudents = allStudents.length;
    const enrolled = allStudents.filter(s => s.enrolled).length;
    const totalPrograms = allPrograms.length;
    const avgGrade = calculateAvgGrade();

    document.getElementById('totalStudentsCount').textContent = totalStudents;
    document.getElementById('enrolledCount').textContent = enrolled;
    document.getElementById('programsCount').textContent = totalPrograms;
    document.getElementById('avgGradeCount').textContent = avgGrade.toFixed(1);
}

function calculateAvgGrade() {
    if (studentGrades.length === 0) return 0;
    const sum = studentGrades.reduce((acc, g) => acc + g.grade, 0);
    return sum / studentGrades.length;
}

function filterAndRender() {
    const searchTerm = document.getElementById('searchInput').value.toLowerCase();
    const programId = document.getElementById('programFilter').value;
    const year = document.getElementById('yearFilter').value;

    let filtered = allStudents.filter(student => {
        const name = `${student.firstName} ${student.lastName}`.toLowerCase();
        const matchesSearch = name.includes(searchTerm);
        const matchesProgram = !programId || hasStudentInProgram(student.id, parseInt(programId));
        const matchesYear = !year || student.year === parseInt(year);

        return matchesSearch && matchesProgram && matchesYear;
    });

    renderTable(filtered);
}

function hasStudentInProgram(studentId, programId) {
    // Check if student is enrolled in a section of this program
    const studSect = studentSections.find(ss => ss.studentId === studentId);
    if (!studSect) return false;
    
    const section = allSections.find(s => s.id === studSect.sectionId);
    return section && section.programId === programId;
}

function renderTable(students) {
    const tbody = document.getElementById('studentTableBody');
    tbody.innerHTML = '';

    if (students.length === 0) {
        tbody.innerHTML = '<tr><td colspan="7" style="text-align: center; padding: 20px;">No students found</td></tr>';
        return;
    }

    students.forEach(student => {
        const row = document.createElement('tr');
        
        // Get student's sections and average grade
        const studentSection = getStudentSection(student.id);
        const avgGrade = getStudentAvgGrade(student.id);
        const program = getStudentProgram(student.id);
        const section = studentSection ? getSectionById(studentSection.sectionId) : null;

        row.innerHTML = `
            <td>${student.firstName} ${student.lastName}</td>
            <td>${student.year}</td>
            <td>${student.gender}</td>
            <td>${program ? program.programName : '-'}</td>
            <td>${section ? section.code : '-'}</td>
            <td>${avgGrade > 0 ? avgGrade.toFixed(1) : '-'}</td>
            <td>
                <span class="status-badge ${student.enrolled ? 'enrolled' : 'not-enrolled'}">
                    ${student.enrolled ? 'Enrolled' : 'Not Enrolled'}
                </span>
            </td>
        `;
        tbody.appendChild(row);
    });
}

function getStudentSection(studentId) {
    return studentSections.find(ss => ss.studentId === studentId);
}

function getSectionById(sectionId) {
    return allSections.find(s => s.id === sectionId);
}

function getStudentAvgGrade(studentId) {
    const grades = studentGrades.filter(g => g.studentId === studentId);
    if (grades.length === 0) return 0;
    const sum = grades.reduce((acc, g) => acc + g.grade, 0);
    return sum / grades.length;
}

function getStudentProgram(studentId) {
    const studentSection = getStudentSection(studentId);
    if (!studentSection) return null;
    
    const section = getSectionById(studentSection.sectionId);
    if (!section) return null;
    
    return allPrograms.find(p => p.id === section.programId) || null;
}
