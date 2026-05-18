import React, { use, useEffect } from 'react';
import { getStudents, createStudents, updateStudents, deleteStudents } from './Service';

function Forms() {
    const [students, setStudents] = React.useState({
        FirstName: "",
        LastName: ""
    });
    
    useEffect(() => {
        console.log("forms initialized");
    }, []);

    const handleChange = (event) => {
        const { name, value } = event.target;

        // console.log(value);   
        setStudents((prevStudents) => ({
            ...prevStudents,
            [name]: value,
        }));        
    }

    const handleSubmit = () => {
        const studentCreated = createStudents(students);

    }

    return (
        <>
            <section id="center">
                <form >
                <h1>Forms</h1>

                <input 
                    type='text'
                    value={students.FirstName}
                    name="FirstName"
                    onChange={handleChange} 
                    />
                <h2>{students.FirstName} </h2>
                <input
                    type='text'
                    value={students.LastName}
                    name="LastName"
                    onChange={handleChange}
                    />
                <h2> {students.LastName}</h2>
                </form>
            </section>
        </>
    )
}   

export default Forms;