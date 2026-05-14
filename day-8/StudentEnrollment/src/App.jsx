import { useState } from 'react'
import './App.css'
import Header from './components/bar/Header'
import Footer from './components/bar/Footer'
import Stats from './components/stats/Stats'
import Data from './components/Data'

const App = () => {
  const [students] = useState([
    {
      id: 1,
      name: 'Roosc Zaño',
      year: 3,
      gender: 'Male',
      program: 'BSIT',
      section: 'IT-101',
      avgGrade: 91.0,
      enrolled: true,
    },
    {
      id: 2,
      name: 'Ron Vincent Cada',
      year: 3,
      gender: 'Female',
      program: 'BSIT',
      section: 'IT-101',
      avgGrade: 75.0,
      enrolled: true,
    },
    {
      id: 3,
      name: 'Mike Andrei Gomez',
      year: 4,
      gender: 'Female',
      program: 'BSIT',
      section: 'IT-101',
      avgGrade: 60.0,
      enrolled: false,
    },
    {
      id: 4,
      name: 'Kurt Patrick Laja',
      year: 2,
      gender: 'Male',
      program: 'BSIT',
      section: 'IT-102',
      avgGrade: 90.0,
      enrolled: true,
    },
    {
      id: 5,
      name: 'Mica Cada',
      year: 1,
      gender: 'Female',
      program: 'BSCS',
      section: 'IT-102',
      avgGrade: 90.0,
      enrolled: true,
    },
  ])

  const [programs] = useState([
    { id: 1, name: 'BSIT' },
    { id: 2, name: 'BSCS' },
    { id: 3, name: 'BSENG' },
  ])

  const [searchTerm, setSearchTerm] = useState('')
  const [programFilter, setProgramFilter] = useState('')
  const [yearFilter, setYearFilter] = useState('')

  const filteredStudents = students.filter((student) => {
    const nameMatches = student.name.toLowerCase().includes(searchTerm.toLowerCase())
    const programMatches = !programFilter || student.program === programFilter
    const yearMatches = !yearFilter || String(student.year) === yearFilter

    return nameMatches && programMatches && yearMatches
  })

  const uniquePrograms = Array.from(new Set(students.map((s) => s.program))).sort()

  return (
    <>
      <Header />

      <Stats 
        students={students}
        programs={programs}
      />

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
              <option key={prog} value={prog}>
                {prog}
              </option>
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

      <div className="students-list">
        <div className="data-row data-header">
          <div className="data-cell">Name</div>
          <div className="data-cell">Year</div>
          <div className="data-cell">Gender</div>
          <div className="data-cell">Program</div>
          <div className="data-cell">Section</div>
          <div className="data-cell">Avg Grade</div>
          <div className="data-cell">Status</div>
        </div>
        {filteredStudents.map((student) => (
          <Data
            key={student.id}
            name={student.name}
            year={student.year}
            gender={student.gender}
            program={student.program}
            section={student.section}
            avgGrade={student.avgGrade}
            status={student.enrolled ? "Enrolled" : "Not Enrolled"}
          />
        ))}
      </div>
      <Footer />
    </>
  )
}

export default App
