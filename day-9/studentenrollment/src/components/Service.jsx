const API_URL = "http://localhost:5056/api";

export const getStudents = async () => {
    const response = await fetch(`${API_URL}/students`);
    return response.json();
};

export const createStudents = async (studentData) => {
    const response = await fetch(API_URL,
        {
            method:"POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(studentData)
        }
    );
    return response.json();
};

export const updateStudents = async (studentId, studentData) => {
    const response = await fetch(`${API_URL}/students/${studentId}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(studentData)
    });
    return response.json();
};

export const deleteStudents = async (studentId) => {
    const response = await fetch (`${API_URL}/${studentId}`,
        {
            method: "DELETE",
        }
    )
    return response.json();
};