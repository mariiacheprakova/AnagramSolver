using AnagramSolver.EF.DatabaseFirst.Data;
using Microsoft.EntityFrameworkCore;

var options =
    new DbContextOptionsBuilder<AnagramDbContext>()
        .UseSqlServer(
            "Server=localhost;" +
            "Database=AnagramSolver;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True;")
        .Options;

await using var context = new AnagramDbContext(options);
var longWords =
    await context.Words
        .AsNoTracking()
        .Where(word => word.Value.Length > 4)
        .OrderBy(word => word.Value.Length)
        .ToListAsync();

Console.WriteLine("--- Words longer than 4 characters ---");

foreach (var word in longWords)
{
    Console.WriteLine(
        $"{word.Value} — length: {word.Value.Length}");
}