using AnagramSolver.EF.DatabaseFirst.Data;
using Microsoft.EntityFrameworkCore;

await using var context = new AnagramDbContext();

var words = await context.Words
    .Where(word => word.Value.Length > 4)
    .OrderBy(word => word.Value.Length)
    .ToListAsync();

foreach (var word in words)
{
    Console.WriteLine(word.Value);
}