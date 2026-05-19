import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './styles/index.css'
import App from './App.jsx'
import { ProgramProvider } from './context/ProgramContext'
import { SectionProvider } from './context/SectionContext'
import { StudentProvider } from './context/StudentContext'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <ProgramProvider>
      <SectionProvider>
        <StudentProvider>
          <App />
        </StudentProvider>
      </SectionProvider>
    </ProgramProvider>
  </StrictMode>,
)
