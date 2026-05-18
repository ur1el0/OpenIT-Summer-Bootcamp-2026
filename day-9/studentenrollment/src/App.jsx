import './App.css';
import React from 'react';
import Forms from './components/Forms';
import Programs from './components/Programs';
import Sections from './components/Sections';
import Nav from './components/Nav';


function App() {
  const [view, setView] = React.useState('students');

  return (
    <div>
      <Nav selected={view} onSelect={setView} />
      {view === 'students' && <Forms />}
      {view === 'programs' && <Programs />}
      {view === 'sections' && <Sections />}
    </div>
  );
}
export default App;