USE AnagramSolver
GO 

CREATE INDEX IX_Words_Value
ON Words(Value);

SELECT * 
FROM Words
WHERE Value = 'alus';


BEGIN TRANSACTION

INSERT INTO SearchLog
(
	SearchText,
	ResultCount
)
VALUES
(
	'alus',
	3
);

UPDATE Words
SET Value = 'ALUS'
WHERE Value = 'alus';

COMMIT;

SELECT *
FROM Words;