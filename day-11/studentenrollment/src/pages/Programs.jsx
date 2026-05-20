import { useState } from 'react';
import { useProgramContext } from '../context/ProgramContext';

export default function Programs() {
  const { programs, loading, addProgram, removeProgram } = useProgramContext();
  const [name, setName] = useState('');

  const handleCreate = async (e) => {
    e.preventDefault();
    if (!name.trim()) return;
    await addProgram({ programName: name });
    setName('');
  };

  const handleDelete = async (id) => {
    if (!confirm('Delete program?')) return;
    await removeProgram(id);
  };

  return (
    <div id="center">
      <div>
        <h2>Programs</h2>
        <form onSubmit={handleCreate} style={{display:'flex',gap:12,alignItems:'center'}}>
          <input placeholder="Program name" value={name} onChange={e=>setName(e.target.value)} />
          <button type="submit" disabled={loading}>{loading? 'Saving...':'Add'}</button>
        </form>
      </div>

      <div>
        <h2>Existing Programs</h2>
        <table>
          <thead>
            <tr><th>Name</th><th></th></tr>
          </thead>
          <tbody>
            {programs.map(p=> (
              <tr key={p.id ?? p.Id}>
                <td>{p.programName ?? p.ProgramName}</td>
                <td>
                  <button onClick={()=>handleDelete(p.id ?? p.Id)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}