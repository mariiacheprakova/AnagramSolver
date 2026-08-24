using AnagramSolver.EF.CodeFirst.Data;
using AnagramSolver.EF.CodeFirst.Import;
using AnagramSolver.EF.CodeFirst.Models;
using Microsoft.EntityFrameworkCore;
var options =
    new DbContextOptionsBuilder<AnagramDbContext>()
        .UseSqlServer(
            @"Server=(localdb)\mssqllocaldb;" +
            "Database=AnagramSolver_CF;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True;")
        .Options;
await using var context = new AnagramDbContext(options);

if (!await context.Categories.AnyAsync())
{
    context.Categories.AddRange(
        new Category { Name = "Noun" },
        new Category { Name = "Verb" },
        new Category { Name = "Adjective" });
    await context.SaveChangesAsync();
}

var seeder = new WordDatabaseSeeder(context);
string dictionaryPath = Path.Combine(
    Directory.GetCurrentDirectory(), "AnagramSolver.WebApp", "zodynas.txt");
await seeder.ImportAsync(dictionaryPath);