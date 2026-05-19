import { createContext, useContext } from "react";
import { useState } from "react";

const StudentContext = createContext();

export const StudentProvider = ({ children }) => {
    
    const [students, setStudents] = useState([]);
    const [programs, setPrograms] = useState([]);
    const [sections, setSections] = useState([]);
    
    return (
        <StudentContext.Provider value={{ students, setStudents, programs, setPrograms, sections, setSections }}>
            {children}
        </StudentContext.Provider>
    );
};

export const useStudentContext = () => {
    const context = useContext(StudentContext);
    return context;
}