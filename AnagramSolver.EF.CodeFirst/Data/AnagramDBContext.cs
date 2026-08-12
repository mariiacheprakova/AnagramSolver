using AnagramSolver.EF.CodeFirst.Models;
using Microsoft.EntityFrameworkCore;
namespace AnagramSolver.EF.CodeFirst.Data;
public class AnagramDbContext : DbContext
{
    public AnagramDbContext(
        DbContextOptions<AnagramDbContext> options)
        : base(options)
    {
    }
    public DbSet<Word> Words =>
        Set<Word>();
    public DbSet<Category> Categories =>
        Set<Category>();
    public DbSet<SearchLog> SearchLogs =>
        Set<SearchLog>();

    protected override void OnModelCreating(
       ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Noun"
            },
            new Category
            {
                Id = 2,
                Name = "Verb"
            },
            new Category
            {
                Id = 3,
                Name = "Adjective"
            });
    }
}