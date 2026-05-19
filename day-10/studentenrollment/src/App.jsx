import './styles/App.css';
import React from 'react';
import Forms from './pages/Forms';
import Programs from './pages/Programs';
import Sections from './pages/Sections';
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