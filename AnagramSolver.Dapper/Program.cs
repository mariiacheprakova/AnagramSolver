using Dapper;
using Microsoft.Data.SqlClient;
using AnagramSolver.Dapper.Models;

string connectionString =
    "Server=localhost;Database=AnagramSolver_CF;Trusted_Connection=True;TrustServerCertificate=True;";

using var connection =
    new SqlConnection(connectionString);

//
// SELECT
//
var words = connection.Query<Word>(
    "SELECT * FROM Words");

foreach (var word in words)
{
    Console.WriteLine(word.Value);
}