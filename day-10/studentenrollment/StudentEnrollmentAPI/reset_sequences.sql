SELECT setval(pg_get_serial_sequence('"Students"', 'Id'), COALESCE(MAX("Id"), 0) + 1, false) FROM "Students";
SELECT setval(pg_get_serial_sequence('"Programs"', 'Id'), COALESCE(MAX("Id"), 0) + 1, false) FROM "Programs";
SELECT setval(pg_get_serial_sequence('"Sections"', 'Id'), COALESCE(MAX("Id"), 0) + 1, false) FROM "Sections";
SELECT setval(pg_get_serial_sequence('"Student_Sections"', 'Id'), COALESCE(MAX("Id"), 0) + 1, false) FROM "Student_Sections";
SELECT setval(pg_get_serial_sequence('"StudentGrades"', 'Id'), COALESCE(MAX("Id"), 0) + 1, false) FROM "StudentGrades";
