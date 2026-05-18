import React from 'react';

import '../App.css';


function Programs()
{
  const [programs, setPrograms] = React.useState([]);

  const fetchPrograms = async () => {
    try {
      const response = await fetch('http://localhost:5056/api/programs');
      const data = await response.json();

      // console.log(data);
      setPrograms(data);

    } catch(error) {
      console.error('Error fetching programs:', error);
    }

  };
  React.useEffect(() => {
    fetchPrograms();
  }, []);

  return (
    <>
      {programs.map((program) => (
        <div key={program.id}>
          <h2>{program.programName}</h2>
          <p>{program.description}</p>
        </div>
      ))}
    </>
  );
}

export default Programs;