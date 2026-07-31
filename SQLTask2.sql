
SELECT
    Value,
    LEN(Value) AS WordLength
FROM Words
WHERE LEN(Value) > (
    SELECT AVG(LEN(Value))
    FROM Words
)
ORDER BY WordLength DESC;

SELECT c.Name, COUNT(w.Id) 
FROM Categories as c
INNER JOIN Words as w
	ON c.ID = w.CategoryId
GROUP BY c.Name
HAVING COUNT(w.Id) > (
		SELECT AVG(CAST(CategoryWordCount AS DECIMAL(10,2)))
		FROM ( 
		SELECT w.CategoryId, COUNT(w.CategoryId) AS CategoryWordCount
		FROM Words as w
		GROUP BY w.CategoryId) as WordCount
);

SELECT 
w1.Value,
(
	SELECT COUNT(*)
	FROM Words as w2
	WHERE w2.CategoryId = w1.CategoryId
) AS wordsInCategory
FROM Words AS w1;


SELECT
w1.Value,c.Name
FROM Words AS w1
INNER JOIN Categories AS c
	ON w1.CategoryId = c.Id
WHERE LEN(w1.Value) =
(
	SELECT MAX(LEN(w2.Value))
	FROM Words as w2
	WHERE w2.CategoryId = w1.CategoryId
);