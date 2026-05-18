import React, { useEffect, useState } from 'react';
import {
    createStudents,
    deleteStudents,
    getStudents,
    updateStudents,
    getPrograms,
    getSections,
    getStudentSections,
    getStudentGrades,
    createStudentSection,
    updateStudentSection,
    createStudentGrade,
    updateStudentGrade,
    deleteStudentGrade
} from './Service';

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
    const [students, setStudents] = useState([]);
    const [studentForm, setStudentForm] = useState(emptyStudent);
    const [editingStudentId, setEditingStudentId] = useState(null);
    const [error, setError] = useState("");
    const [isSaving, setIsSaving] = useState(false);
    const [programs, setPrograms] = useState([]);
    const [sections, setSections] = useState([]);
    const [editingLinks, setEditingLinks] = useState({ studentSectionId: null, gradeId: null, enrolledAt: null });

    useEffect(() => {
        loadStudents();
        loadPrograms();
        loadSections();
    }, []);

    const loadStudents = async () => {
        try {
            setError("");
            const [studentData, programData, sectionData, studentSectionData, gradeData] = await Promise.all([
                getStudents(),
                getPrograms(),
                getSections(),
                getStudentSections(),
                getStudentGrades()
            ]);

            const studentList = Array.isArray(studentData) ? studentData : [];
            const programList = Array.isArray(programData) ? programData : [];
            const sectionList = Array.isArray(sectionData) ? sectionData : [];
            const studentSectionList = Array.isArray(studentSectionData) ? studentSectionData : [];
            const gradeList = Array.isArray(gradeData) ? gradeData : [];

            const programsById = new Map(programList.map((p) => [p.id ?? p.Id, p]));
            const sectionsById = new Map(sectionList.map((s) => [s.id ?? s.Id, s]));

            const studentSectionByStudentId = new Map();
            for (const item of studentSectionList) {
                const studentId = item.studentId ?? item.StudentId;
                const prev = studentSectionByStudentId.get(studentId);
                const currentId = item.id ?? item.Id ?? 0;
                const prevId = prev?.id ?? prev?.Id ?? 0;
                if (!prev || currentId > prevId) {
                    studentSectionByStudentId.set(studentId, item);
                }
            }

            const gradeByStudentSectionId = new Map();
            for (const item of gradeList) {
                const studentSectionId = item.student_SectionsId ?? item.Student_SectionsId;
                const prev = gradeByStudentSectionId.get(studentSectionId);
                const currentId = item.id ?? item.Id ?? 0;
                const prevId = prev?.id ?? prev?.Id ?? 0;
                if (!prev || currentId > prevId) {
                    gradeByStudentSectionId.set(studentSectionId, item);
                }
            }

            const enrichedStudents = studentList.map((student) => {
                const studentId = student.id ?? student.Id;
                const studentSection = studentSectionByStudentId.get(studentId);
                const studentSectionId = studentSection?.id ?? studentSection?.Id ?? null;
                const sectionId = studentSection?.sectionId ?? studentSection?.SectionId ?? null;
                const section = studentSection?.section ?? studentSection?.Section ?? sectionsById.get(sectionId) ?? null;
                const sectionCode = section?.code ?? section?.Code ?? "";
                const programId = section?.programId ?? section?.ProgramId ?? null;
                const program = section?.program ?? section?.Program ?? programsById.get(programId) ?? null;
                const programName = program?.programName ?? program?.ProgramName ?? "";
                const gradeRecord = studentSectionId ? gradeByStudentSectionId.get(studentSectionId) : null;
                const gradeValue = gradeRecord ? (gradeRecord.grade ?? gradeRecord.Grade ?? "") : "";

                return {
                    ...student,
                    Section: sectionCode,
                    Program: programName,
                    Grade: gradeValue,
                    _sectionId: sectionId,
                    _programId: programId,
                    _studentSectionId: studentSectionId,
                    _studentSectionEnrolledAt: studentSection?.enrolledAt ?? studentSection?.EnrolledAt ?? null,
                    _gradeId: gradeRecord?.id ?? gradeRecord?.Id ?? null,
                };
            });

            setPrograms(programList);
            setSections(sectionList);
            setStudents(enrichedStudents);
        } catch (fetchError) {
            setError(fetchError.message || "Failed to load students.");
        }
    };

    const loadPrograms = async () => {};
    const loadSections = async () => {};

    const handleChange = (event) => {
        const { name, value, type, checked } = event.target;
        const val = type === 'checkbox' ? checked : value;

        if (name === "Section") {
            const selectedSection = sections.find((s) => String(s.id ?? s.Id) === String(val));
            const selectedProgramId = selectedSection?.programId ?? selectedSection?.ProgramId ?? "";
            setStudentForm((prevStudents) => ({
                ...prevStudents,
                Section: val,
                Program: selectedProgramId ? String(selectedProgramId) : "",
            }));
            return;
        }

        setStudentForm((prevStudents) => ({
            ...prevStudents,
            [name]: val,
        }));
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        try {
            setIsSaving(true);
            setError("");

            const payload = {
                FirstName: studentForm.FirstName,
                LastName: studentForm.LastName,
                Year: Number(studentForm.Year) || 1,
                Gender: studentForm.Gender || "",
                Enrolled: studentForm.Enrolled !== false,
            };

            if (editingStudentId !== null) {
                await updateStudents(editingStudentId, payload);

                const selectedSectionId = studentForm.Section ? Number(studentForm.Section) : null;
                let activeStudentSectionId = editingLinks.studentSectionId;

                if (selectedSectionId) {
                    if (editingLinks.studentSectionId) {
                        await updateStudentSection(editingLinks.studentSectionId, {
                            StudentId: editingStudentId,
                            SectionId: selectedSectionId,
                            EnrolledAt: editingLinks.enrolledAt ?? new Date().toISOString(),
                        });
                    } else {
                        const createdLink = await createStudentSection({
                            StudentId: editingStudentId,
                            SectionId: selectedSectionId,
                        });
                        activeStudentSectionId = createdLink?.id ?? createdLink?.Id ?? null;
                    }
                }

                if (studentForm.Grade !== "" && activeStudentSectionId) {
                    const gradePayload = {
                        StudentId: editingStudentId,
                        Student_SectionsId: activeStudentSectionId,
                        grade: Number(studentForm.Grade),
                    };

                    if (editingLinks.gradeId) {
                        await updateStudentGrade(editingLinks.gradeId, gradePayload);
                    } else {
                        await createStudentGrade(gradePayload);
                    }
                } else if (studentForm.Grade === "" && editingLinks.gradeId) {
                    await deleteStudentGrade(editingLinks.gradeId);
                }
            } else {
                const created = await createStudents(payload);
                const studentId = created?.id ?? created?.Id;

                if (studentForm.Section && studentId) {
                    const sectionId = Number(studentForm.Section);
                    const studentSection = await createStudentSection({ StudentId: studentId, SectionId: sectionId });

                    if (studentForm.Grade) {
                        await createStudentGrade({
                            StudentId: studentId,
                            Student_SectionsId: studentSection?.id ?? studentSection?.Id,
                            grade: Number(studentForm.Grade),
                        });
                    }
                }
            }

            await loadStudents();
            setStudentForm(emptyStudent);
            setEditingStudentId(null);
            setEditingLinks({ studentSectionId: null, gradeId: null, enrolledAt: null });
        } catch (submitError) {
            setError(submitError.message || "Failed to save student.");
        } finally {
            setIsSaving(false);
        }
    };

    const handleEdit = (student) => {
        const studentId = student.id ?? student.Id ?? null;
        setEditingStudentId(studentId);
        setStudentForm({
            ...emptyStudent,
            FirstName: student.FirstName ?? student.firstName ?? "",
            LastName: student.LastName ?? student.lastName ?? "",
            Section: student._sectionId ? String(student._sectionId) : "",
            Program: student._programId ? String(student._programId) : "",
            Grade: student.Grade ?? student.grade ?? "",
            Year: student.Year ?? student.year ?? 1,
            Gender: student.Gender ?? student.gender ?? "",
            Enrolled: student.Enrolled ?? student.enrolled ?? true,
        });
        setEditingLinks({
            studentSectionId: student._studentSectionId ?? null,
            gradeId: student._gradeId ?? null,
            enrolledAt: student._studentSectionEnrolledAt ?? null,
        });
        setError("");
    };

    const handleDelete = async (studentId) => {
        try {
            setError("");
            await deleteStudents(studentId);
            await loadStudents();
            if (editingStudentId === studentId) {
                setEditingStudentId(null);
                setStudentForm(emptyStudent);
            }
        } catch (deleteError) {
            setError(deleteError.message || "Failed to delete student.");
        }
    };

    const handleCancelEdit = () => {
        setEditingStudentId(null);
        setStudentForm(emptyStudent);
        setEditingLinks({ studentSectionId: null, gradeId: null, enrolledAt: null });
        setError("");
    };

    const getStudentId = (student) => student.id ?? student.Id;
    const getStudentValue = (student, key) => student[key] ?? student[key.charAt(0).toLowerCase() + key.slice(1)] ?? "";

    return (
        <section id="center">
            <div>
                <h2>Student List</h2>
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
                                            <button type='button' className="action-delete" onClick={() => handleDelete(studentId)}>
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

                {error ? <p role="alert">{error}</p> : null}

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

                    <select name="Program" value={studentForm.Program} onChange={handleChange}>
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

                <button type='submit' disabled={isSaving}>
                    {isSaving ? "Saving..." : editingStudentId !== null ? "Update Student" : "Input Student"}
                </button>

                {editingStudentId !== null ? (
                    <button type='button' onClick={handleCancelEdit}>
                        Cancel
                    </button>
                ) : null}
            </form>
        </section>
    );
}

export default Forms;