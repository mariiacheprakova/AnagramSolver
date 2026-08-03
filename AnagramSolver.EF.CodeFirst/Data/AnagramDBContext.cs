using AnagramSolver.EF.CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.EF.CodeFirst.Data;

public class AnagramDbContext : DbContext
{
    public DbSet<Word> Words => Set<Word>();

    public DbSet<Category> Categories => Set<Category>();

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost;" +
            "Database=AnagramSolver_CF;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True;");
    }
}