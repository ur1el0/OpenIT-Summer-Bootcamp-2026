import { createContext, useContext, useState, useEffect } from "react";
import { getPrograms, createProgram, deleteProgram } from "../services/Service";

const ProgramContext = createContext();

export const ProgramProvider = ({ children }) => {
    const [programs, setPrograms] = useState([]);
    const [loading, setLoading] = useState(false);

    const loadPrograms = async () => {
        try {
            const data = await getPrograms();
            setPrograms(Array.isArray(data) ? data : []);
        } catch (e) {
            console.error("Failed to load programs", e);
        }
    };

    const addProgram = async (programData) => {
        setLoading(true);
        try {
            await createProgram(programData);
            await loadPrograms();
        } catch (e) {
            console.error("Failed to add program", e);
        } finally {
            setLoading(false);
        }
    };

    const removeProgram = async (id) => {
        try {
            await deleteProgram(id);
            await loadPrograms();
        } catch (e) {
            console.error("Failed to delete program", e);
        }
    };

    useEffect(() => {
        loadPrograms();
    }, []);

    return (
        <ProgramContext.Provider
            value={{
                programs,
                loading,
                loadPrograms,
                addProgram,
                removeProgram
            }}
        >
            {children}
        </ProgramContext.Provider>
    );
};

export const useProgramContext = () => useContext(ProgramContext);
