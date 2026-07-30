IF DB_ID('AnagramSolver') IS NULL
BEGIN
	CREATE DATABASE AnagramSolver;
END;
GO 
USE AnagramSolver;
GO

DROP TABLE IF EXISTS Words;
DROP TABLE IF EXISTS SearchLog;
DROP TABLE IF EXISTS Categories;
GO

CREATE TABLE Categories (
Id INT IDENTITY(1,1) PRIMARY KEY,
Name NVARCHAR(50) NOT NULL
);


CREATE TABLE Words (
Id INT IDENTITY(1,1) PRIMARY KEY,
Value NVARCHAR(100) NOT NULL,
CategoryId INT NULL REFERENCES Categories(Id),
CreatedAt DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE SearchLog (
Id INT IDENTITY(1,1) PRIMARY KEY,
SearchText NVARCHAR(100) NOT NULL,
ResultCount INT NOT NULL,
SearchedAt DATETIME2 DEFAULT GETDATE()
);

INSERT INTO Categories (Name) VALUES
	('Noun'),('Verb'),('Adjective');

INSERT INTO Words (Value,CategoryId) VALUES
('alus', 1), ('sula', 1), ('vanduo', 1), ('katinas', 1),
('medis', 1), ('programavimas', 1), ('saulė', 1),
('bėgti', 2), ('mąstyti', 2), ('koduoti', 2),
('gražus', 3), ('greitas', 3), ('stiprus', 3);

INSERT INTO Words (Value,CategoryId) VALUES
('nuostabus',3) , ('džiaugtis',2), ('lapė',1);

SELECT *
FROM Words;

UPDATE Words
SET Value = 'Grazuu', CategoryId = 3
WHERE Value = 'gražus';

DELETE FROM Words WHERE CategoryId = 2;

SELECT c.Name AS CategoryName, 
       w.Value,
       w.CreatedAt
FROM Words AS w
INNER JOIN Categories as c
    ON  w.CategoryId = c.Id;


INSERT INTO SearchLog (SearchText, ResultCount)
VALUES
    ('alus', 1),
    ('katinas', 0),
    ('saulė', 2);

SELECT SearchText, COUNT(*) AS SearchCount
FROM SearchLog
GROUP BY SearchText

