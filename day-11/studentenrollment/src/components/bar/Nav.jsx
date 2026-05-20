export default function Nav({ selected, onSelect }) {
  return (
    <nav className="nav-bar">
      <button onClick={() => onSelect('students')} className={selected==='students' ? 'nav-button is-active' : 'nav-button'}>Students</button>
      <button onClick={() => onSelect('programs')} className={selected==='programs' ? 'nav-button is-active' : 'nav-button'}>Programs</button>
      <button onClick={() => onSelect('sections')} className={selected==='sections' ? 'nav-button is-active' : 'nav-button'}>Sections</button>
    </nav>
  );
}
