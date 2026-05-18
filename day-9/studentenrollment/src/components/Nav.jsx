import React from 'react';

export default function Nav({ selected, onSelect }) {
  return (
    <nav style={{display:'flex',gap:12,justifyContent:'center',marginTop:20}}>
      <button onClick={() => onSelect('students')} style={selected==='students'?active:button}>Students</button>
      <button onClick={() => onSelect('programs')} style={selected==='programs'?active:button}>Programs</button>
      <button onClick={() => onSelect('sections')} style={selected==='sections'?active:button}>Sections</button>
    </nav>
  );
}

const button = {
  padding: '8px 14px',
  borderRadius: 10,
  border: '1px solid rgba(148,163,184,0.12)',
  background: 'transparent',
  color: '#e2e8f0',
  cursor: 'pointer'
};

const active = {
  ...button,
  background: 'linear-gradient(135deg,#f43f5e,#fb7185)',
  color: 'white',
  boxShadow: '0 8px 20px rgba(244,63,94,0.18)'
};
