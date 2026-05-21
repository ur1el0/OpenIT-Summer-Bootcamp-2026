import React, { useEffect, useState } from 'react';
import './styles/App.css';
import Header from './components/bar/Header';
import Footer from './components/bar/Footer';
import Stats from './components/stats/Stats';
import { useStudentContext } from './context/StudentContext';
import { useProgramContext } from './context/ProgramContext';
import { useSectionContext } from './context/SectionContext';
import { ProgramProvider } from './context/ProgramContext';
import { SectionProvider } from './context/SectionContext';
import { StudentProvider } from './context/StudentContext';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import { getCurrentUser, loginUser, logoutUser, registerUser } from './services/Service';

const StudentsTab = ({ authUser, onRequireAuth }) => {
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
      year: student.year || student.Year || 1,
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
      if (!authUser) {
        onRequireAuth();
        return;
      }

      if(window.confirm('Delete this student?')) {
        removeStudent(id);
      }
    };
  
    const [showAddForm, setShowAddForm] = useState(false);
    const [formData, setFormData] = useState({ firstName: '', lastName: '', yearLevel: 1, gender: 'Male', sectionId: '', grade: '' });
    const [editingStudent, setEditingStudent] = useState(null);

    const handleEditStart = (student) => {
      if (!authUser) {
        onRequireAuth();
        return;
      }

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
        year: parseInt(formData.yearLevel, 10),
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
      <div className="page-surface">
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

            <div className="add-student-section">
                <button
                  className="btn btn-primary"
                  onClick={() => {
                  if (!authUser) {
                    onRequireAuth();
                    return;
                  }
                  setShowAddForm(!showAddForm);
                }}
                >
              {showAddForm ? 'Cancel' : '+ Add New Student'}
                </button>
                {showAddForm && (
                <form onSubmit={handleAddSubmit} className="inline-form">
                    <input required placeholder="First Name" value={formData.firstName} onChange={e => setFormData({...formData, firstName: e.target.value})} />
                    <input required placeholder="Last Name" value={formData.lastName} onChange={e => setFormData({...formData, lastName: e.target.value})} />
                    <select value={formData.gender} onChange={e => setFormData({...formData, gender: e.target.value})}>
                        <option value="Male">Male</option>
                        <option value="Female">Female</option>
                    </select>
                    <input type="number" min="1" max="4" required placeholder="Year" value={formData.yearLevel} onChange={e => setFormData({...formData, yearLevel: e.target.value})} />
                    <select value={formData.sectionId} onChange={e => setFormData({...formData, sectionId: e.target.value})}>
                        <option value="">No Section</option>
                        {sections.map(s => <option key={s.id || s.Id} value={s.id || s.Id}>{s.code || s.Code}</option>)}
                    </select>
                    <input type="number" step="0.01" placeholder="Initial Grade" value={formData.grade} onChange={e => setFormData({...formData, grade: e.target.value})} />
                    <button className="btn btn-accent" type="submit">{editingStudent ? 'Update' : 'Save'}</button>
                </form>
                )}
            </div>

            <div className="students-list">
                <div className={`data-row data-header${authUser ? '' : ' data-row--no-actions'}`}>
                <div className="data-cell">Name</div>
                <div className="data-cell">Year</div>
                <div className="data-cell">Gender</div>
                <div className="data-cell">Program</div>
                <div className="data-cell">Section</div>
                <div className="data-cell">Avg Grade</div>
                <div className="data-cell">Status</div>
                {authUser && <div className="data-cell">Actions</div>}
                </div>
                {filteredStudents.map((student) => (
                <div className={`data-row${authUser ? '' : ' data-row--no-actions'}`} key={student.id}>
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
                    {authUser && <div className="data-cell action-cell">
                      <button className="btn btn-ghost btn-sm" onClick={() => handleEditStart(student)}>Update</button>
                        <button className="btn btn-danger btn-sm" onClick={() => handleDelete(student.id)}>Delete</button>
                    </div>}
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
      <div className="page-grid">
        <div className="card">
          <h2>Programs</h2>
          <form onSubmit={handleCreate} className="form-row">
            <input placeholder="Program name" value={name} onChange={e=>setName(e.target.value)} />
            <button className="btn btn-primary" type="submit" disabled={loading}>
              {loading ? 'Saving...' : 'Add Program'}
            </button>
          </form>
        </div>
  
        <div className="card table-card">
          <table className="data-table">
            <thead>
              <tr>
                <th>Program Name</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {programs.map(p=> (
                <tr key={p.id ?? p.Id}>
                  <td>{p.programName ?? p.ProgramName}</td>
                  <td>
                    <button className="btn btn-danger btn-sm" onClick={()=>handleDelete(p.id ?? p.Id)}>Delete</button>
                  </td>
                </tr>
              ))}
              {programs.length === 0 && <tr><td colSpan="2">No programs found.</td></tr>}
            </tbody>
          </table>
        </div>
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
      <div className="page-grid">
        <div className="card">
          <h2>Sections</h2>
          <form onSubmit={handleCreate} className="form-row">
            <input placeholder="Section Code (e.g. IT-101)" value={code} onChange={e=>setCode(e.target.value)} />
            <select value={programId} onChange={e=>setProgramId(e.target.value)} required>
              <option value="">Select Program</option>
              {programs.map(p => <option key={p.id ?? p.Id} value={p.id ?? p.Id}>{p.programName ?? p.ProgramName}</option>)}
            </select>
            <button className="btn btn-primary" type="submit" disabled={loading}>
              {loading ? 'Saving...' : 'Add Section'}
            </button>
          </form>
        </div>
  
        <div className="card table-card">
          <table className="data-table">
            <thead>
              <tr>
                <th>Section Code</th>
                <th>Program ID</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {sections.map(s => (
                <tr key={s.id ?? s.Id}>
                  <td>{s.code ?? s.Code}</td>
                  <td>{programs.find(p => (p.id ?? p.Id) === (s.programId ?? s.ProgramId))?.programName ?? (s.programId ?? s.ProgramId)}</td>
                  <td>
                    <button className="btn btn-danger btn-sm" onClick={()=>handleDelete(s.id ?? s.Id)}>Delete</button>
                  </td>
                </tr>
              ))}
              {sections.length === 0 && <tr><td colSpan="3">No sections found.</td></tr>}
            </tbody>
          </table>
        </div>
      </div>
    );
};

const App = () => {
  const [activeTab, setActiveTab] = useState('students');
  const [authMode, setAuthMode] = useState(null);
  const [authUser, setAuthUser] = useState(null);
  const [authLoading, setAuthLoading] = useState(true);
  const [authSubmitting, setAuthSubmitting] = useState(false);
  const [authError, setAuthError] = useState('');
  const [authInfo, setAuthInfo] = useState('');

  useEffect(() => {
    const restoreSession = async () => {
      try {
        const currentUser = await getCurrentUser();
        setAuthUser(currentUser);
      } catch {
        setAuthUser(null);
      } finally {
        setAuthLoading(false);
      }
    };

    restoreSession();
  }, []);

  const handleAuthSubmit = async ({ username, password }) => {
    setAuthSubmitting(true);
    setAuthError('');
    setAuthInfo('');

    try {
      if (authMode === 'login') {
        const result = await loginUser({ username, password });
        setAuthUser(result);
        setAuthMode(null);
      } else {
        await registerUser({ username, password });
        setAuthMode('login');
        setAuthInfo('Account created. Sign in to continue.');
      }
    } catch (error) {
      setAuthError(error.message || 'Authentication failed.');
    } finally {
      setAuthSubmitting(false);
    }
  };

  const handleLogout = async () => {
    try {
      await logoutUser();
    } finally {
      setAuthUser(null);
      setActiveTab('students');
      setAuthMode(null);
      setAuthInfo('');
      setAuthError('');
    }
  };

  const openAuthPage = (mode, info = '') => {
    setAuthError('');
    setAuthInfo(info);
    setAuthMode(mode);
  };

  if (authLoading) {
    return (
      <main className="auth-shell">
        <div className="auth-loading">Checking your session...</div>
      </main>
    );
  }

  if (!authUser && authMode === 'login') {
    return (
      <LoginPage
        onToggleMode={() => openAuthPage('register')}
        onSubmit={handleAuthSubmit}
        loading={authSubmitting}
        error={authError}
        info={authInfo}
      />
    );
  }

  if (!authUser && authMode === 'register') {
    return (
      <RegisterPage
        onToggleMode={() => openAuthPage('login')}
        onSubmit={handleAuthSubmit}
        loading={authSubmitting}
        error={authError}
        info={authInfo}
      />
    );
  }

  return (
    <ProgramProvider>
      <SectionProvider>
        <StudentProvider>
          <>
            <Header />
            <div className="dashboard-bar">
              <div>
                <span className="dashboard-bar__label">{authUser ? 'Signed in as' : 'Public access'}</span>
                <strong>{authUser ? (authUser.userName || authUser.UserName) : 'Student records are view-only'}</strong>
              </div>
              {authUser ? (
                <button className="dashboard-bar__logout" onClick={handleLogout} type="button">
                  Logout
                </button>
              ) : (
                <div className="dashboard-bar__actions">
                  <button className="dashboard-bar__login" onClick={() => openAuthPage('login')} type="button">
                    Login
                  </button>
                  <button className="dashboard-bar__logout" onClick={() => openAuthPage('register')} type="button">
                    Register
                  </button>
                </div>
              )}
            </div>
            <div className="tab-bar">
              <button onClick={() => setActiveTab('students')} className={activeTab === 'students' ? 'tab-button is-active' : 'tab-button'}>Students</button>
              {authUser && <button onClick={() => setActiveTab('programs')} className={activeTab === 'programs' ? 'tab-button is-active' : 'tab-button'}>Programs</button>}
              {authUser && <button onClick={() => setActiveTab('sections')} className={activeTab === 'sections' ? 'tab-button is-active' : 'tab-button'}>Sections</button>}
            </div>

            {activeTab === 'students' && (
              <StudentsTab
                authUser={authUser}
                onRequireAuth={() => openAuthPage('login', 'Sign in to add, update, or delete students.')}
              />
            )}
            {authUser && activeTab === 'programs' && <ProgramsTab />}
            {authUser && activeTab === 'sections' && <SectionsTab />}

            <Footer />
          </>
        </StudentProvider>
      </SectionProvider>
    </ProgramProvider>
  );
}

export default App;
