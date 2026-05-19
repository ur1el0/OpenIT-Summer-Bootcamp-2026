import { createContext, useContext } from "react";
import { useState } from "react";

const ProgramContext = createContext();

export const ProgramProvider = ({ children }) => {
    const [programs, setPrograms] = useState([]);

    return (
        <ProgramContext.Provider
            value=
            {{
                programs, setPrograms
            }}
            >
                {children}
            </ProgramContext.Provider>
    );
}

export const useProgramContext = () => {
    const context = useContext(ProgramContext);
    return context;
}