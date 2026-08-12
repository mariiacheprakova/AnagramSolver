using AnagramSolver.Dapper.Models;
using Dapper;
using Microsoft.Data.SqlClient;

string connectionString =
    "Server=localhost;Database=AnagramSolver_CF;Trusted_Connection=True;TrustServerCertificate=True;";

using var connection = new SqlConnection(connectionString);

await connection.OpenAsync();

//
// SELECT
//
const string selectLongWordsSql =
    """
    SELECT
        Id,
        Value,
        CategoryId,
        CreatedAt
    FROM Words
    WHERE LEN(Value) > @MinimumLength
    ORDER BY LEN(Value) DESC;
    """;

IEnumerable<Word> longWords =
    await connection.QueryAsync<Word>(
        selectLongWordsSql,
        new
        {
            MinimumLength = 4
        });

foreach (var word in longWords)
{
    Console.WriteLine(
        $"{word.Id}: {word.Value}, CategoryId: {word.CategoryId}");
}

//
// INSERT
//
const string insertSql =
    """
    INSERT INTO Words
        (Value, CategoryId, CreatedAt)
    VALUES
        (@Value, @CategoryId, @CreatedAt);
    """;

int insertedRows =
    await connection.ExecuteAsync(
        insertSql,
        new
        {
            Value = "vysnia",
            CategoryId = 1,
            CreatedAt = DateTime.Now
        });

Console.WriteLine($"Inserted rows: {insertedRows}");

//
// UPDATE
//
const string updateSql =
    """
    UPDATE Words
    SET
        Value = @Value,
        CategoryId = @CategoryId
    WHERE Id = @Id;
    """;

int updatedRows =
    await connection.ExecuteAsync(
        updateSql,
        new
        {
            Id = 1,
            Value = "orange",
            CategoryId = 2
        });

Console.WriteLine($"Updated rows: {updatedRows}");

//
// DELETE
//
const string deleteSql =
    """
    DELETE FROM Words
    WHERE Id = @Id;
    """;

int deletedRows =
    await connection.ExecuteAsync(
        deleteSql,
        new
        {
            Id = 3
        });

Console.WriteLine($"Deleted rows: {deletedRows}");