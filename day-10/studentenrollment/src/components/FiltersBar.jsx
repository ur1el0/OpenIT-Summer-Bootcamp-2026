import React from 'react';

const FiltersBar = () => {
  return (
    <div className="filters-bar">
      <input type="text" placeholder="Search students..." />
      <select>
        <option value="">Filter by Program</option>
      </select>
      <select>
        <option value="">Filter by Section</option>
      </select>
    </div>
  );
};

export default FiltersBar;
