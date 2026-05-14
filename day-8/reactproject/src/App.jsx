import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from './assets/vite.svg'
import heroImg from './assets/hero.png'
import './App.css'
import Header from './common/Header'
import Counter from './common/Counter'
import Sidebar from './common/Sidebar'

const App = () => {

  return (
    <>
      <Header />
      <Sidebar />
      <Counter />
 
    </>
  )
}
export default App;