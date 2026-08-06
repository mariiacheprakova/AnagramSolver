-- Task 1
SELECT Value, ROW_NUMBER() OVER
(
	ORDER BY LEN(Value) 
) AS RowNumber
FROM Words;

--Task 2
SELECT Value,
RANK()
OVER
(
	PARTITION BY CategoryId
	ORDER BY LEN(Value) DESC
) AS RankIncategory
FROM Words;

-- Task 3
WITH CategoryWords AS
(
    SELECT
        Value,
        CategoryId,
        ROW_NUMBER() OVER
        (
            PARTITION BY CategoryId
            ORDER BY LEN(Value) DESC
        ) AS RowNumber
    FROM Words
)
SELECT
    Value,
    CategoryId
FROM CategoryWords
WHERE RowNumber = 1;

-- Task 4
SELECT
    Value,
    SUM(LEN(Value))
    OVER
    (
        ORDER BY Id
    ) AS RunningTotal
FROM Words;

-- Task 5
SELECT
    Value,
    LAG(Value) OVER (ORDER BY Id) AS PreviousWord,
    LEAD(Value) OVER (ORDER BY Id) AS NextWord
FROM Words;