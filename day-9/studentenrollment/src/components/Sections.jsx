import React from 'react';
import { getSections, createSection, deleteSection, getPrograms } from './Service';

export default function Sections(){
  const [sections, setSections] = React.useState([]);
  const [programs, setPrograms] = React.useState([]);
  const [form, setForm] = React.useState({ Code:'', Year:2024, ProgramId: ''});

  const load = async () => {
    try{
      const s = await getSections();
      setSections(Array.isArray(s)?s:[]);
      const p = await getPrograms();
      setPrograms(Array.isArray(p)?p:[]);
    }catch(e){console.error(e)}
  };

  React.useEffect(()=>{ load(); },[]);

  const handleCreate = async (e) =>{
    e.preventDefault();
    const payload = { Code: form.Code, Year: Number(form.Year) || 0, ProgramId: form.ProgramId? Number(form.ProgramId): null };
    await createSection(payload);
    setForm({ Code:'', Year:2024, ProgramId:'' });
    await load();
  };

  const handleDelete = async (id) =>{
    if(!confirm('Delete section?')) return;
    await deleteSection(id);
    await load();
  };

  return (
    <div id="center">
      <div>
        <h2>Create Section</h2>
        <form onSubmit={handleCreate} className="form-row">
          <input placeholder="Code" value={form.Code} onChange={e=>setForm(f=>({...f, Code:e.target.value}))} />
          <input type="number" placeholder="Year" value={form.Year} onChange={e=>setForm(f=>({...f, Year:e.target.value}))} />
          <select value={form.ProgramId} onChange={e=>setForm(f=>({...f, ProgramId:e.target.value}))}>
            <option value="">-- Program (optional) --</option>
            {programs.map(p=> <option key={p.id ?? p.Id} value={p.id ?? p.Id}>{p.programName ?? p.ProgramName}</option>)}
          </select>
          <button type="submit">Create</button>
        </form>
      </div>

      <div>
        <h2>Sections</h2>
        <table>
          <thead><tr><th>Code</th><th>Year</th><th>Program</th><th></th></tr></thead>
          <tbody>
            {sections.map(s=> (
              <tr key={s.id ?? s.Id}>
                <td>{s.code ?? s.Code}</td>
                <td>{s.year ?? s.Year}</td>
                <td>{(s.program?.programName ?? s.programName) || (s.ProgramName) || (s.programId ?? s.ProgramId) }</td>
                <td><button onClick={()=>handleDelete(s.id ?? s.Id)}>Delete</button></td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
