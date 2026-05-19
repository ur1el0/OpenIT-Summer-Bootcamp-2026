import React, { useState } from 'react';
import './styles/App.css';
import Header from './components/bar/Header';
import Footer from './components/bar/Footer';
import Stats from './components/stats/Stats';
import Data from './components/Data';
import { useStudentContext } from './context/StudentContext';
import { useProgramContext } from './context/ProgramContext';
import { useSectionContext } from './context/SectionContext';

const StudentsTab = () => {
  const { students, removeStudent, addStudent, editStudent } = useStudentContext()
    const { programs } = useProgramContext()
    const { sections } = useSectionContext()
  
    const [searchTerm, setSearchTerm] = useState('')
    const [programFilter, setProgramFilter] = useState('')
    const [yearFilter, setYearFilter] = useState('')
  
    const mappedStudents = (students || []).map(student => ({
      id: student.id || student.Id,
      firstName: student.firstName || student.FirstName || '',
      lastName: student.lastName || student.LastName || '',
      name: `${student.firstName || student.FirstName} ${student.lastName || student.LastName}`,
      year: student.yearLevel || student.YearLevel || 1,
      gender: student.gender || student.Gender || 'Other',
      program: student.Program || '',
      section: student.Section || '',
      avgGrade: student.Grade || '',
      enrolled: student._studentSectionId != null,
      _sectionId: student._sectionId,
      _studentSectionId: student._studentSectionId,
      _studentSectionEnrolledAt: student._studentSectionEnrolledAt,
      _gradeId: student._gradeId,
    }))
  
    const filteredStudents = mappedStudents.filter((student) => {
      const nameMatches = student.name.toLowerCase().includes(searchTerm.toLowerCase())
      const programMatches = !programFilter || student.program === programFilter
      const yearMatches = !yearFilter || String(student.year) === yearFilter
      return nameMatches && programMatches && yearMatches
    })
  
    const uniquePrograms = Array.from(new Set(mappedStudents.map((s) => s.program).filter(Boolean))).sort()
  
    const handleDelete = (id) => {
      if(window.confirm('Delete this student?')) {
        removeStudent(id);
      }
    };
  
    const [showAddForm, setShowAddForm] = useState(false);
    const [formData, setFormData] = useState({ firstName: '', lastName: '', yearLevel: 1, gender: 'Male', sectionId: '', grade: '' });
    const [editingStudent, setEditingStudent] = useState(null);

    const handleEditStart = (student) => {
      setEditingStudent(student);
      setShowAddForm(true);
      setFormData({
        firstName: student.firstName,
        lastName: student.lastName,
        yearLevel: student.year,
        gender: student.gender,
        sectionId: student._sectionId ? String(student._sectionId) : '',
        grade: student.avgGrade,
      });
    };
  
    const handleAddSubmit = (e) => {
      e.preventDefault();
      const payload = {
        firstName: formData.firstName,
        lastName: formData.lastName,
        yearLevel: parseInt(formData.yearLevel, 10),
        gender: formData.gender,
      };

      if (editingStudent) {
        editStudent(
          editingStudent.id,
          payload,
          formData.sectionId,
          formData.grade,
          editingStudent._studentSectionId,
          editingStudent._studentSectionEnrolledAt,
          editingStudent._gradeId,
        );
      } else {
        addStudent(payload, formData.sectionId, formData.grade);
      }

      setShowAddForm(false);
      setEditingStudent(null);
      setFormData({ firstName: '', lastName: '', yearLevel: 1, gender: 'Male', sectionId: '', grade: '' });
    };

    return (
        <div style={{ marginTop: '20px' }}>
            <Stats students={mappedStudents} programs={programs} />

            <div className="filters-section">
                <div className="search-box">
                <input
                    type="text"
                    placeholder="Search by name..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                />
                </div>
                <div className="filter-dropdowns">
                <select value={programFilter} onChange={(e) => setProgramFilter(e.target.value)}>
                    <option value="">All Programs</option>
                    {uniquePrograms.map((prog) => (
                    <option key={prog} value={prog}>{prog}</option>
                    ))}
                </select>
                <select value={yearFilter} onChange={(e) => setYearFilter(e.target.value)}>
                    <option value="">All Years</option>
                    <option value="1">Year 1</option>
                    <option value="2">Year 2</option>
                    <option value="3">Year 3</option>
                    <option value="4">Year 4</option>
                </select>
                </div>
            </div>

            <div className="add-student-section" style={{ padding: '0 20px 20px' }}>
                <button onClick={() => setShowAddForm(!showAddForm)} style={{ padding: '8px 16px', background: '#007bff', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>
              {showAddForm ? 'Cancel' : '+ Add New Student'}
                </button>
                {showAddForm && (
                <form onSubmit={handleAddSubmit} style={{ marginTop: '16px', display: 'flex', gap: '8px', flexWrap: 'wrap' }}>
                    <input required placeholder="First Name" value={formData.firstName} onChange={e => setFormData({...formData, firstName: e.target.value})} style={{ padding: '8px' }} />
                    <input required placeholder="Last Name" value={formData.lastName} onChange={e => setFormData({...formData, lastName: e.target.value})} style={{ padding: '8px' }} />
                    <select value={formData.gender} onChange={e => setFormData({...formData, gender: e.target.value})} style={{ padding: '8px' }}>
                        <option value="Male">Male</option>
                        <option value="Female">Female</option>
                    </select>
                    <input type="number" min="1" max="4" required placeholder="Year" value={formData.yearLevel} onChange={e => setFormData({...formData, yearLevel: e.target.value})} style={{ padding: '8px' }} />
                    <select value={formData.sectionId} onChange={e => setFormData({...formData, sectionId: e.target.value})} style={{ padding: '8px' }}>
                        <option value="">No Section</option>
                        {sections.map(s => <option key={s.id || s.Id} value={s.id || s.Id}>{s.code || s.Code}</option>)}
                    </select>
                    <input type="number" step="0.01" placeholder="Initial Grade" value={formData.grade} onChange={e => setFormData({...formData, grade: e.target.value})} style={{ padding: '8px' }} />
                    <button type="submit" style={{ padding: '8px 16px', background: '#28a745', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>{editingStudent ? 'Update' : 'Save'}</button>
                </form>
                )}
            </div>

            <div className="students-list">
                <div className="data-row data-header">
                <div className="data-cell">Name</div>
                <div className="data-cell">Year</div>
                <div className="data-cell">Gender</div>
                <div className="data-cell">Program</div>
                <div className="data-cell">Section</div>
                <div className="data-cell">Avg Grade</div>
                <div className="data-cell">Status</div>
                <div className="data-cell">Actions</div>
                </div>
                {filteredStudents.map((student) => (
                <div className="data-row" key={student.id}>
                    <div className="data-cell">{student.name}</div>
                    <div className="data-cell">{student.year}</div>
                    <div className="data-cell">{student.gender}</div>
                    <div className="data-cell">{student.program || '-'}</div>
                    <div className="data-cell">{student.section || '-'}</div>
                    <div className="data-cell">{student.avgGrade}</div>
                    <div className="data-cell">
                        <span className={`status ${student.enrolled ? 'enrolled' : 'not-enrolled'}`}>
                            {student.enrolled ? 'Enrolled' : 'Not Enrolled'}
                        </span>
                    </div>
                    <div className="data-cell" style={{ display: 'flex', gap: '8px' }}>
                      <button onClick={() => handleEditStart(student)} style={{ padding: '4px 8px', background: '#1f2448', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>Update</button>
                        <button onClick={() => handleDelete(student.id)} style={{ padding: '4px 8px', background: '#dc3545', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>Delete</button>
                    </div>
                </div>
                ))}
            </div>
        </div>
    );
};

const ProgramsTab = () => {
    const { programs, loading, addProgram, removeProgram } = useProgramContext();
    const [name, setName] = React.useState('');
  
    const handleCreate = async (e) => {
      e.preventDefault();
      if (!name.trim()) return;
      await addProgram({ programName: name });
      setName('');
    };
  
    const handleDelete = async (id) => {
      if (!window.confirm('Delete program?')) return;
      await removeProgram(id);
    };
  
    return (
      <div style={{ padding: '20px' }}>
        <h2>Programs Management</h2>
        <form onSubmit={handleCreate} style={{display:'flex',gap:'12px',alignItems:'center', marginBottom: '20px'}}>
          <input placeholder="Program name" value={name} onChange={e=>setName(e.target.value)} style={{ padding: '8px', minWidth: '200px' }} />
          <button type="submit" disabled={loading} style={{ padding: '8px 16px', background: '#28a745', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>
            {loading ? 'Saving...' : 'Add Program'}
          </button>
        </form>
  
        <table style={{ width: '100%', textAlign: 'left', borderCollapse: 'collapse' }}>
          <thead>
            <tr style={{ borderBottom: '1px solid #ccc' }}>
              <th style={{ padding: '10px' }}>Program Name</th>
              <th style={{ padding: '10px' }}>Actions</th>
            </tr>
          </thead>
          <tbody>
            {programs.map(p=> (
              <tr key={p.id ?? p.Id} style={{ borderBottom: '1px solid #eee' }}>
                <td style={{ padding: '10px' }}>{p.programName ?? p.ProgramName}</td>
                <td style={{ padding: '10px' }}>
                  <button onClick={()=>handleDelete(p.id ?? p.Id)} style={{ padding: '4px 8px', background: '#dc3545', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>Delete</button>
                </td>
              </tr>
            ))}
            {programs.length === 0 && <tr><td colSpan="2" style={{ padding: '10px' }}>No programs found.</td></tr>}
          </tbody>
        </table>
      </div>
    );
};

const SectionsTab = () => {
    const { sections, loading, addSection, removeSection } = useSectionContext();
    const { programs } = useProgramContext();
    const [code, setCode] = React.useState('');
    const [programId, setProgramId] = React.useState('');
  
    const handleCreate = async (e) => {
      e.preventDefault();
      if (!code.trim() || !programId) return;
      await addSection({ code, programId: parseInt(programId, 10) });
      setCode('');
      setProgramId('');
    };
  
    const handleDelete = async (id) => {
      if (!window.confirm('Delete section?')) return;
      await removeSection(id);
    };
  
    return (
      <div style={{ padding: '20px' }}>
        <h2>Sections Management</h2>
        <form onSubmit={handleCreate} style={{display:'flex',gap:'12px',alignItems:'center', marginBottom: '20px'}}>
          <input placeholder="Section Code (e.g. IT-101)" value={code} onChange={e=>setCode(e.target.value)} style={{ padding: '8px', minWidth: '200px' }} />
          <select value={programId} onChange={e=>setProgramId(e.target.value)} style={{ padding: '8px' }} required>
            <option value="">Select Program</option>
            {programs.map(p => <option key={p.id ?? p.Id} value={p.id ?? p.Id}>{p.programName ?? p.ProgramName}</option>)}
          </select>
          <button type="submit" disabled={loading} style={{ padding: '8px 16px', background: '#28a745', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>
            {loading ? 'Saving...' : 'Add Section'}
          </button>
        </form>
  
        <table style={{ width: '100%', textAlign: 'left', borderCollapse: 'collapse' }}>
          <thead>
            <tr style={{ borderBottom: '1px solid #ccc' }}>
              <th style={{ padding: '10px' }}>Section Code</th>
              <th style={{ padding: '10px' }}>Program ID</th>
              <th style={{ padding: '10px' }}>Actions</th>
            </tr>
          </thead>
          <tbody>
            {sections.map(s => (
              <tr key={s.id ?? s.Id} style={{ borderBottom: '1px solid #eee' }}>
                <td style={{ padding: '10px' }}>{s.code ?? s.Code}</td>
                <td style={{ padding: '10px' }}>{programs.find(p => (p.id ?? p.Id) === (s.programId ?? s.ProgramId))?.programName ?? (s.programId ?? s.ProgramId)}</td>
                <td style={{ padding: '10px' }}>
                  <button onClick={()=>handleDelete(s.id ?? s.Id)} style={{ padding: '4px 8px', background: '#dc3545', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>Delete</button>
                </td>
              </tr>
            ))}
            {sections.length === 0 && <tr><td colSpan="3" style={{ padding: '10px' }}>No sections found.</td></tr>}
          </tbody>
        </table>
      </div>
    );
};

const App = () => {
  const [activeTab, setActiveTab] = useState('students');

  return (
    <>
      <Header />
      <div style={{ display: 'flex', gap: '10px', padding: '20px 20px 0 20px', borderBottom: '1px solid #ddd' }}>
        <button onClick={() => setActiveTab('students')} style={{ padding: '10px 20px', border: 'none', background: activeTab === 'students' ? '#007bff' : 'transparent', color: activeTab === 'students' ? '#fff' : '#333', cursor: 'pointer', borderRadius: '4px 4px 0 0' }}>Students</button>
        <button onClick={() => setActiveTab('programs')} style={{ padding: '10px 20px', border: 'none', background: activeTab === 'programs' ? '#007bff' : 'transparent', color: activeTab === 'programs' ? '#fff' : '#333', cursor: 'pointer', borderRadius: '4px 4px 0 0' }}>Programs</button>
        <button onClick={() => setActiveTab('sections')} style={{ padding: '10px 20px', border: 'none', background: activeTab === 'sections' ? '#007bff' : 'transparent', color: activeTab === 'sections' ? '#fff' : '#333', cursor: 'pointer', borderRadius: '4px 4px 0 0' }}>Sections</button>
      </div>

      {activeTab === 'students' && <StudentsTab />}
      {activeTab === 'programs' && <ProgramsTab />}
      {activeTab === 'sections' && <SectionsTab />}

      <Footer />
    </>
  )
}

export default App;
