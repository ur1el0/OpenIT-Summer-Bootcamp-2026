const Data = (
    { name, year, gender, program, section, avgGrade, status }
) => {
    return (
        <div className="data-row">
            <div className="data-cell">{name}</div>
            <div className="data-cell">{year}</div>
            <div className="data-cell">{gender}</div>
            <div className="data-cell">{program || '-'}</div>
            <div className="data-cell">{section || '-'}</div>
            <div className="data-cell">{avgGrade}</div>
            <div className="data-cell">
                <span className={`status ${status === 'Enrolled' ? 'enrolled' : 'not-enrolled'}`}>
                    {status}
                </span>
            </div>
        </div>
    );
}

export default Data;