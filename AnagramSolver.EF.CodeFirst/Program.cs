using AnagramSolver.EF.CodeFirst.Data;
using AnagramSolver.EF.CodeFirst.Import;
using Microsoft.EntityFrameworkCore;
var options =
    new DbContextOptionsBuilder<AnagramDbContext>()
        .UseSqlServer(
            "Server=localhost;" +
            "Database=AnagramSolver_CF;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True;")
        .Options;
await using var context =
    new AnagramDbContext(options);
var seeder =
    new WordDatabaseSeeder(context);
string dictionaryPath =
    @"C:\Users\marii\Desktop\projectSolver\AnagramSolver.WebApp\zodynas.txt";
await seeder.ImportAsync(dictionaryPath);