import { createContext, useContext, useState, useEffect } from "react";
import { getSections, createSection, deleteSection } from "../services/Service";

const SectionContext = createContext();

export const SectionProvider = ({ children }) => {
    const [sections, setSections] = useState([]);
    const [loading, setLoading] = useState(false);

    const loadSections = async () => {
        try {
            const data = await getSections();
            setSections(Array.isArray(data) ? data : []);
        } catch (e) {
            console.error("Failed to load sections", e);
        }
    };

    const addSection = async (sectionData) => {
        setLoading(true);
        try {
            await createSection(sectionData);
            await loadSections();
        } catch (e) {
            console.error("Failed to add section", e);
        } finally {
            setLoading(false);
        }
    };

    const removeSection = async (id) => {
        try {
            await deleteSection(id);
            await loadSections();
        } catch (e) {
            console.error("Failed to delete section", e);
        }
    };

    useEffect(() => {
        loadSections();
    }, []);

    return (
        <SectionContext.Provider
            value={{
                sections,
                loading,
                loadSections,
                addSection,
                removeSection
            }}
        >
            {children}
        </SectionContext.Provider>
    );
};

export const useSectionContext = () => useContext(SectionContext);