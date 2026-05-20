import { createContext, useContext, useState, useEffect } from "react";
import {
    getStudents,
    createStudents,
    updateStudents,
    deleteStudents,
    getStudentSections,
    createStudentSection,
    updateStudentSection,
    getStudentGrades,
    createStudentGrade,
    updateStudentGrade,
    deleteStudentGrade
} from "../services/Service";
import { useProgramContext } from "./ProgramContext";
import { useSectionContext } from "./SectionContext";

const StudentContext = createContext();

export const StudentProvider = ({ children }) => {
    const [students, setStudents] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const { programs } = useProgramContext();
    const { sections } = useSectionContext();

    const loadStudents = async () => {
        try {
            setError("");
            setLoading(true);

            const [studentData, studentSectionData, gradeData] = await Promise.all([
                getStudents(),
                getStudentSections(),
                getStudentGrades()
            ]);

            const studentList = Array.isArray(studentData) ? studentData : [];
            const studentSectionList = Array.isArray(studentSectionData) ? studentSectionData : [];
            const gradeList = Array.isArray(gradeData) ? gradeData : [];

            const programsById = new Map(programs.map((p) => [p.id ?? p.Id, p]));
            const sectionsById = new Map(sections.map((s) => [s.id ?? s.Id, s]));

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

            setStudents(enrichedStudents);
        } catch (err) {
            setError(err.message || "Failed to load students.");
        } finally {
            setLoading(false);
        }
    };

    const addStudent = async (studentPayload, sectionIdStr, gradeStr) => {
        setLoading(true);
        setError("");
        try {
            const created = await createStudents(studentPayload);
            const studentId = created?.id ?? created?.Id;

            if (sectionIdStr && studentId) {
                const createdLink = await createStudentSection({
                    studentId: studentId,
                    sectionId: Number(sectionIdStr),
                });
                const activeStudentSectionId = createdLink?.id ?? createdLink?.Id ?? null;

                if (gradeStr !== "" && activeStudentSectionId) {
                    await createStudentGrade({
                        studentId: studentId,
                        student_SectionsId: activeStudentSectionId,
                        grade: Number(gradeStr),
                    });
                }
            }
            await loadStudents();
        } catch (err) {
            setError(err.message || "Failed to add student.");
        } finally {
            setLoading(false);
        }
    };

    const editStudent = async (
        studentId,
        studentPayload,
        sectionIdStr,
        gradeStr,
        _studentSectionId,
        _studentSectionEnrolledAt,
        _gradeId
    ) => {
        setLoading(true);
        setError("");
        try {
            await updateStudents(studentId, studentPayload);

            const selectedSectionId = sectionIdStr ? Number(sectionIdStr) : null;
            let activeStudentSectionId = _studentSectionId;

            if (selectedSectionId) {
                if (_studentSectionId) {
                    await updateStudentSection(_studentSectionId, {
                        studentId: studentId,
                        sectionId: selectedSectionId,
                        enrolledAt: _studentSectionEnrolledAt ?? new Date().toISOString(),
                    });
                } else {
                    const createdLink = await createStudentSection({
                        studentId: studentId,
                        sectionId: selectedSectionId,
                    });
                    activeStudentSectionId = createdLink?.id ?? createdLink?.Id ?? null;
                }
            }

            if (gradeStr !== "" && activeStudentSectionId) {
                const gradePayload = {
                    studentId: studentId,
                    student_SectionsId: activeStudentSectionId,
                    grade: Number(gradeStr),
                };

                if (_gradeId) {
                    await updateStudentGrade(_gradeId, gradePayload);
                } else {
                    await createStudentGrade(gradePayload);
                }
            } else if (gradeStr === "" && _gradeId) {
                await deleteStudentGrade(_gradeId);
            }

            await loadStudents();
        } catch (err) {
            setError(err.message || "Failed to update student.");
        } finally {
            setLoading(false);
        }
    };

    const removeStudent = async (studentId) => {
        setError("");
        try {
            await deleteStudents(studentId);
            await loadStudents();
        } catch (err) {
            setError(err.message || "Failed to delete student.");
        }
    };

    useEffect(() => {
        if (programs.length > 0 || sections.length > 0) {
            loadStudents();
        }
    }, [programs, sections]);

    return (
        <StudentContext.Provider value={{ students, loading, error, loadStudents, addStudent, editStudent, removeStudent }}>
            {children}
        </StudentContext.Provider>
    );
};

export const useStudentContext = () => useContext(StudentContext);