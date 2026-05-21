const API_URL = import.meta.env.VITE_API_URL || "/api";
const AUTH_URL = import.meta.env.VITE_AUTH_URL || "";

const apiFetch = (path, options = {}) => fetch(`${API_URL}${path}`, {
    ...options,
    credentials: "include"
});

const authFetch = (path, options = {}) => fetch(`${AUTH_URL}${path}`, {
    ...options,
    credentials: "include"
});

export const handleResponse = async (response) => {
    if (!response.ok) {
        const errorText = await response.text();
        let errorMessage = errorText;

        try {
            const errorBody = JSON.parse(errorText);
            errorMessage = errorBody.message || errorMessage;
        } catch {
            // Keep the plain response text when the server did not return JSON.
        }

        throw new Error(errorMessage || `Request failed with status ${response.status}`);
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
    const response = await apiFetch("/students");
    return handleResponse(response);
};

export const createStudents = async (studentData) => {
    const response = await apiFetch("/students",
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
    const response = await apiFetch(`/students/${studentId}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(studentData)
    });
    return handleResponse(response);
};

export const deleteStudents = async (studentId) => {
    const response = await apiFetch(`/students/${studentId}`,
        {
            method: "DELETE",
        }
    );
    return handleResponse(response);
};

// Programs
export const getPrograms = async () => {
    const response = await apiFetch("/programs");
    return handleResponse(response);
};

export const createProgram = async (programData) => {
    const response = await apiFetch("/programs", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(programData)
    });
    return handleResponse(response);
};

export const updateProgram = async (programId, programData) => {
    const response = await apiFetch(`/programs/${programId}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(programData)
    });
    return handleResponse(response);
};

export const deleteProgram = async (programId) => {
    const response = await apiFetch(`/programs/${programId}`, { method: "DELETE" });
    return handleResponse(response);
};

// Sections
export const getSections = async () => {
    const response = await apiFetch("/sections");
    return handleResponse(response);
};

export const createSection = async (sectionData) => {
    const response = await apiFetch("/sections", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(sectionData)
    });
    return handleResponse(response);
};

export const updateSection = async (sectionId, sectionData) => {
    const response = await apiFetch(`/sections/${sectionId}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(sectionData)
    });
    return handleResponse(response);
};

export const deleteSection = async (sectionId) => {
    const response = await apiFetch(`/sections/${sectionId}`, { method: "DELETE" });
    return handleResponse(response);
};

// Student Sections
export const getStudentSections = async () => {
    const response = await apiFetch("/student-sections");
    return handleResponse(response);
};

export const createStudentSection = async (data) => {
    const response = await apiFetch("/student-sections", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });
    return handleResponse(response);
};

export const updateStudentSection = async (id, data) => {
    const response = await apiFetch(`/student-sections/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });
    return handleResponse(response);
};

export const deleteStudentSection = async (id) => {
    const response = await apiFetch(`/student-sections/${id}`, { method: "DELETE" });
    return handleResponse(response);
};

// Student Grades
export const getStudentGrades = async () => {
    const response = await apiFetch("/student-grades");
    return handleResponse(response);
};

export const createStudentGrade = async (data) => {
    const response = await apiFetch("/student-grades", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });
    return handleResponse(response);
};

export const updateStudentGrade = async (id, data) => {
    const response = await apiFetch(`/student-grades/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });
    return handleResponse(response);
};

export const deleteStudentGrade = async (id) => {
    const response = await apiFetch(`/student-grades/${id}`, { method: "DELETE" });
    return handleResponse(response);
};

export const registerUser = async (credentials) => {
    const response = await authFetch("/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(credentials)
    });
    return handleResponse(response);
};

export const loginUser = async (credentials) => {
    const response = await authFetch("/login?useCookies=true", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(credentials)
    });
    return handleResponse(response);
};

export const getCurrentUser = async () => {
    const response = await authFetch("/auth/me");
    return handleResponse(response);
};

export const logoutUser = async () => {
    const response = await authFetch("/logout", { method: "POST" });
    return handleResponse(response);
};
