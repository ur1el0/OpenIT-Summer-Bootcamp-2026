import { createContext, useContext, includeContext, useState } from "react";

const SectionContext = createContext();

export const SectionProvider = ({ children }) => {

    const [sections, setSections] = useState([]);

    return (
        <SectionContext.Provider
            value=
            {{
                sections, setSections
            }}>
            {children}
            </SectionContext.Provider>
    );
}

export const useSectionContext = () => {
    const context = useContext(SectionContext);
    return context;
}