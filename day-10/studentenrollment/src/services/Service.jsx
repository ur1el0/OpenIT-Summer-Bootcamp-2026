const API_URL = "http://localhost:5056/api";

export const handleResponse = async (response) => {
    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || `Request failed with status ${response.status}`);
    }

    if (response.status === 204) {
        return null;
    }

    const contentType = response.headers.get("content-type") || "";
    if (contentType.includes("application/json")) {
        return response.json();
    }

    return null;
};

export const getStudents = async () => {
    const response = await fetch(`${API_URL}/students`);
    return handleResponse(response);
};

export const createStudents = async (studentData) => {
    const response = await fetch(`${API_URL}/students`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(studentData)
        }
    );
    return handleResponse(response);
};

export const updateStudents = async (studentId, studentData) => {
    const response = await fetch(`${API_URL}/students/${studentId}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(studentData)
    });
    return handleResponse(response);
};

export const deleteStudents = async (studentId) => {
    const response = await fetch(`${API_URL}/students/${studentId}`,
        {
            method: "DELETE",
        }
    );
    return handleResponse(response);
};

// Programs
export const getPrograms = async () => {
    const response = await fetch(`${API_URL}/programs`);
    return handleResponse(response);
};

export const createProgram = async (programData) => {
    const response = await fetch(`${API_URL}/programs`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(programData)
    });
    return handleResponse(response);
};

export const updateProgram = async (programId, programData) => {
    const response = await fetch(`${API_URL}/programs/${programId}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(programData)
    });
    return handleResponse(response);
};

export const deleteProgram = async (programId) => {
    const response = await fetch(`${API_URL}/programs/${programId}`, { method: "DELETE" });
    return handleResponse(response);
};

// Sections
export const getSections = async () => {
    const response = await fetch(`${API_URL}/sections`);
    return handleResponse(response);
};

export const createSection = async (sectionData) => {
    const response = await fetch(`${API_URL}/sections`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(sectionData)
    });
    return handleResponse(response);
};

export const updateSection = async (sectionId, sectionData) => {
    const response = await fetch(`${API_URL}/sections/${sectionId}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(sectionData)
    });
    return handleResponse(response);
};

export const deleteSection = async (sectionId) => {
    const response = await fetch(`${API_URL}/sections/${sectionId}`, { method: "DELETE" });
    return handleResponse(response);
};

// Student Sections
export const getStudentSections = async () => {
    const response = await fetch(`${API_URL}/student-sections`);
    return handleResponse(response);
};

export const createStudentSection = async (data) => {
    const response = await fetch(`${API_URL}/student-sections`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });
    return handleResponse(response);
};

export const updateStudentSection = async (id, data) => {
    const response = await fetch(`${API_URL}/student-sections/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });
    return handleResponse(response);
};

export const deleteStudentSection = async (id) => {
    const response = await fetch(`${API_URL}/student-sections/${id}`, { method: "DELETE" });
    return handleResponse(response);
};

// Student Grades
export const getStudentGrades = async () => {
    const response = await fetch(`${API_URL}/student-grades`);
    return handleResponse(response);
};

export const createStudentGrade = async (data) => {
    const response = await fetch(`${API_URL}/student-grades`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });
    return handleResponse(response);
};

export const updateStudentGrade = async (id, data) => {
    const response = await fetch(`${API_URL}/student-grades/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });
    return handleResponse(response);
};

export const deleteStudentGrade = async (id) => {
    const response = await fetch(`${API_URL}/student-grades/${id}`, { method: "DELETE" });
    return handleResponse(response);
};
