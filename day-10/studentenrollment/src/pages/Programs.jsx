import React from 'react';
import { getPrograms, createProgram, deleteProgram } from '../services/Service';

export default function Programs() {
  const [programs, setPrograms] = React.useState([]);
  const [name, setName] = React.useState('');
  const [loading, setLoading] = React.useState(false);

  const load = async () => {
    try {
      const data = await getPrograms();
      setPrograms(Array.isArray(data) ? data : []);
    } catch (e) {
      console.error(e);
    }
  };

  React.useEffect(() => { load(); }, []);

  const handleCreate = async (e) => {
    e.preventDefault();
    if (!name.trim()) return;
    setLoading(true);
    try {
      await createProgram({ ProgramName: name });
      setName('');
      await load();
    } catch (e) {
      console.error(e);
    } finally { setLoading(false); }
  };

  const handleDelete = async (id) => {
    if (!confirm('Delete program?')) return;
    await deleteProgram(id);
    await load();
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