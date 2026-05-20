import { useState } from 'react';
import { useSectionContext } from '../context/SectionContext';
import { useProgramContext } from '../context/ProgramContext';

export default function Sections(){
  const { sections, loading, addSection, removeSection } = useSectionContext();
  const { programs } = useProgramContext();

  const [form, setForm] = useState({ Code:'', Year:2024, ProgramId: ''});

  const handleCreate = async (e) =>{
    e.preventDefault();
    const payload = { code: form.Code, year: Number(form.Year) || 0, programId: form.ProgramId? Number(form.ProgramId): null };
    await addSection(payload);
    setForm({ Code:'', Year:2024, ProgramId:'' });
  };

  const handleDelete = async (id) =>{
    if(!confirm('Delete section?')) return;
    await removeSection(id);
  };

  return (
    <div id="center" className="page-grid">
      <div className="card">
        <h2>Create Section</h2>
        <form onSubmit={handleCreate} className="form-row">
          <input placeholder="Code" value={form.Code} onChange={e=>setForm(f=>({...f, Code:e.target.value}))} />
          <input type="number" placeholder="Year" value={form.Year} onChange={e=>setForm(f=>({...f, Year:e.target.value}))} />
          <select value={form.ProgramId} onChange={e=>setForm(f=>({...f, ProgramId:e.target.value}))}>
            <option value="">-- Program (optional) --</option>
            {programs.map(p=> <option key={p.id ?? p.Id} value={p.id ?? p.Id}>{p.programName ?? p.ProgramName}</option>)}
          </select>
          <button className="btn btn-primary" type="submit" disabled={loading}>Create</button>
        </form>
      </div>

      <div className="card table-card">
        <h2>Sections</h2>
        <table className="data-table">
          <thead><tr><th>Code</th><th>Year</th><th>Program</th><th></th></tr></thead>
          <tbody>
            {sections.map(s=> (
              <tr key={s.id ?? s.Id}>
                <td>{s.code ?? s.Code}</td>
                <td>{s.year ?? s.Year}</td>
                <td>{(s.program?.programName ?? s.programName) || (s.ProgramName) || (s.programId ?? s.ProgramId) }</td>
                <td><button className="btn btn-danger btn-sm" onClick={()=>handleDelete(s.id ?? s.Id)}>Delete</button></td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
