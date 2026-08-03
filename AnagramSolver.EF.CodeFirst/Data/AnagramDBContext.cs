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
}