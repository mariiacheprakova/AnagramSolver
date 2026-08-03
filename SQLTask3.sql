-- Task 1
WITH Stats AS
(
    SELECT
        CategoryId,
        COUNT(*) AS CategoryCount,
        AVG(CAST(LEN(Value) AS DECIMAL(10,2))) AS AverageLength,
        MAX(LEN(Value)) AS MaximumLength
    FROM Words
    GROUP BY CategoryId
)
SELECT
    CategoryId,
    CategoryCount,
    AverageLength,
    MaximumLength
FROM Stats;

-- Task 2
WITH LongWords AS
(
    SELECT
        Value,
        LEN(Value) AS WordLength,
        'Long' AS WordType
    FROM Words
    WHERE LEN(Value) > 5
),
ShortWords AS
(
    SELECT
        Value,
        LEN(Value) AS WordLength,
        'Short' AS WordType
    FROM Words
    WHERE LEN(Value) < 4
),
Combined AS
(
    SELECT
        Value,
        WordLength,
        WordType
    FROM LongWords

    UNION ALL

    SELECT
        Value,
        WordLength,
        WordType
    FROM ShortWords
)
SELECT
    Value,
    WordLength,
    WordType
FROM Combined;

-- Task 3

WITH SortingCategories AS
(
    SELECT
        CategoryId,
        COUNT(CategoryId) AS WordsCount
    FROM Words
    GROUP BY CategoryId
)
SELECT
    CategoryId,
    WordsCount
FROM SortingCategories
WHERE WordsCount >
(
    SELECT AVG(WordsCount)
    FROM SortingCategories
)
ORDER BY WordsCount DESC;
