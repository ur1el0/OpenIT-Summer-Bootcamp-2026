import { useState } from 'react';
import { useStudentContext } from '../context/StudentContext';
import { useProgramContext } from '../context/ProgramContext';
import { useSectionContext } from '../context/SectionContext';

const emptyStudent = {
    FirstName: "",
    LastName: "",
    Section: "",
    Program: "",
    Grade: "",
    Year: 1,
    Gender: "",
    Enrolled: true,
};

function Forms() {
    const { students, loading, error, addStudent, editStudent, removeStudent } = useStudentContext();
    const { programs } = useProgramContext();
    const { sections } = useSectionContext();

    const [studentForm, setStudentForm] = useState(emptyStudent);
    const [editingStudentId, setEditingStudentId] = useState(null);
    const [editingLinks, setEditingLinks] = useState({ studentSectionId: null, gradeId: null, enrolledAt: null });

    const handleChange = (event) => {
        const { name, value, type, checked } = event.target;
        const val = type === 'checkbox' ? checked : value;

        if (name === "Section") {
            const selectedSection = sections.find((s) => String(s.id ?? s.Id) === String(val));
            const selectedProgramId = selectedSection?.programId ?? selectedSection?.ProgramId ?? "";
            setStudentForm((prev) => ({
                ...prev,
                Section: val,
                Program: selectedProgramId ? String(selectedProgramId) : "",
            }));
            return;
        }

        setStudentForm((prev) => ({
            ...prev,
            [name]: val,
        }));
    };

    const handleSubmit = async (event) => {
        event.preventDefault();

        const payload = {
            firstName: studentForm.FirstName,
            lastName: studentForm.LastName,
            year: Number(studentForm.Year) || 1,
            gender: studentForm.Gender || "",
            enrolled: studentForm.Enrolled !== false,
        };

        if (editingStudentId !== null) {
            await editStudent(
                editingStudentId,
                payload,
                studentForm.Section,
                studentForm.Grade,
                editingLinks.studentSectionId,
                editingLinks.enrolledAt,
                editingLinks.gradeId
            );
            handleCancelEdit();
        } else {
            await addStudent(payload, studentForm.Section, studentForm.Grade);
            setStudentForm(emptyStudent);
        }
    };

    const handleEdit = (student) => {
        const studentId = student.id ?? student.Id;
        setStudentForm({
            FirstName: student.firstName ?? student.FirstName ?? "",
            LastName: student.lastName ?? student.LastName ?? "",
            Section: student._sectionId ? String(student._sectionId) : "",
            Program: student._programId ? String(student._programId) : "",
            Grade: student.Grade !== "" && student.Grade !== undefined ? String(student.Grade) : "",
            Year: student.year ?? student.Year ?? 1,
            Gender: student.gender ?? student.Gender ?? "",
            Enrolled: student.enrolled ?? student.Enrolled ?? false,
        });
        setEditingStudentId(studentId);
        setEditingLinks({
            studentSectionId: student._studentSectionId,
            gradeId: student._gradeId,
            enrolledAt: student._studentSectionEnrolledAt,
        });
    };

    const handleDelete = async (studentId) => {
        if (window.confirm("Are you sure you want to delete this student?")) {
            await removeStudent(studentId);
        }
    };

    const handleCancelEdit = () => {
        setStudentForm(emptyStudent);
        setEditingStudentId(null);
        setEditingLinks({ studentSectionId: null, gradeId: null, enrolledAt: null });
    };

    const getStudentId = (student) => student.id ?? student.Id;
    const getStudentValue = (student, key) => student[key] ?? student[key.charAt(0).toLowerCase() + key.slice(1)] ?? "";

    if (loading && students.length === 0) {
        return <div id="center"><p>Loading...</p></div>;
    }

    return (
        <section id="center">
            <div>
                <h2>Student List</h2>
                {error && <p role="alert" style={{color: 'red'}}>{error}</p>}
                {students.length === 0 ? (
                    <p>No students found.</p>
                ) : (
                    <table>
                        <thead>
                            <tr>
                                <th>First Name</th>
                                <th>Last Name</th>
                                <th>Year</th>
                                <th>Section</th>
                                <th>Program</th>
                                <th>Enrolled</th>
                                <th>Grade</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {students.map((student) => {
                                const studentId = getStudentId(student);

                                return (
                                    <tr key={studentId ?? `${getStudentValue(student, 'FirstName')}-${getStudentValue(student, 'LastName')}`}>
                                        <td>{getStudentValue(student, 'FirstName')}</td>
                                        <td>{getStudentValue(student, 'LastName')}</td>
                                        <td>{getStudentValue(student, 'Year')}</td>
                                        <td>{getStudentValue(student, 'Section')}</td>
                                        <td>{getStudentValue(student, 'Program')}</td>
                                        <td>
                                            {(student.Enrolled ?? student.enrolled) ? (
                                                <span className="badge enrolled">Enrolled</span>
                                            ) : (
                                                <span className="badge not-enrolled">Not Enrolled</span>
                                            )}
                                        </td>
                                        <td>{getStudentValue(student, 'Grade')}</td>
                                        <td>
                                            <button type='button' className="action-edit" onClick={() => handleEdit(student)}>
                                                Edit
                                            </button>
                                            <button type='button' className="action-edit" onClick={() => handleDelete(studentId)} style={{backgroundColor: '#e74c3c', marginLeft: 8}}>
                                                Delete
                                            </button>
                                        </td>
                                    </tr>
                                );
                            })}
                        </tbody>
                    </table>
                )}
            </div>

            <form onSubmit={handleSubmit}>
                <h2>{editingStudentId !== null ? "Edit Student" : "Add Student"}</h2>

                <div className="form-row">
                    <input
                        type='text'
                        value={studentForm.FirstName}
                        name="FirstName"
                        onChange={handleChange}
                        placeholder='First Name'
                    />

                    <input
                        type='text'
                        value={studentForm.LastName}
                        name="LastName"
                        onChange={handleChange}
                        placeholder='Last Name'
                    />
                </div>

                <div className="form-row">
                    <select name="Section" value={studentForm.Section} onChange={handleChange}>
                        <option value="">-- Select Section --</option>
                        {sections.map((section) => (
                            <option key={section.id ?? section.Id} value={section.id ?? section.Id}>
                                {section.code ?? section.Code}
                            </option>
                        ))}
                    </select>

                    <select name="Program" value={studentForm.Program} onChange={handleChange} disabled>
                        <option value="">-- Select Program --</option>
                        {programs.map((program) => (
                            <option key={program.id ?? program.Id} value={program.id ?? program.Id}>
                                {program.programName ?? program.ProgramName}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="form-row">
                    <input
                        type='text'
                        value={studentForm.Grade}
                        name="Grade"
                        onChange={handleChange}
                        placeholder='Grade'
                    />

                    <input
                        type='number'
                        value={studentForm.Year}
                        name="Year"
                        onChange={handleChange}
                        placeholder='Year'
                    />
                </div>

                <div className="form-row" style={{ alignItems: 'center' }}>
                    <input
                        type='text'
                        value={studentForm.Gender}
                        name="Gender"
                        onChange={handleChange}
                        placeholder='Gender'
                    />

                    <label style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
                        <input type="checkbox" name="Enrolled" checked={!!studentForm.Enrolled} onChange={handleChange} />
                        <span className="muted">Enrolled</span>
                    </label>
                </div>

                <button type='submit' disabled={loading}>
                    {loading ? "Saving..." : editingStudentId !== null ? "Update Student" : "Input Student"}
                </button>

                {editingStudentId !== null ? (
                    <button type='button' onClick={handleCancelEdit} style={{marginLeft: 12, backgroundColor: '#95a5a6'}}>
                        Cancel
                    </button>
                ) : null}
            </form>
        </section>
    );
}

export default Forms;