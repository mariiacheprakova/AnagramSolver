using AnagramSolver.EF.CodeFirst.Data;
using AnagramSolver.EF.CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

using var context = new AnagramDbContext();

//
// Task 1 - Insert sample data
//
if (!context.Categories.Any())
{
    var noun = new Category
    {
        Name = "Noun"
    };

    var verb = new Category
    {
        Name = "Verb"
    };

    context.Categories.AddRange(noun, verb);

    context.Words.AddRange(
        new Word
        {
            Value = "apple",
            Category = noun
        },
        new Word
        {
            Value = "pear",
            Category = noun
        },
        new Word
        {
            Value = "run",
            Category = verb
        },
        new Word
        {
            Value = "random"
        });

    context.SaveChanges();
}

//
// Task 2 - Include / Eager Loading
//
Console.WriteLine("----- All words with categories -----");

var words = context.Words
    .Include(w => w.Category)
    .ToList();

foreach (var word in words)
{
    Console.WriteLine($"{word.Value} - {word.Category?.Name}");
}

//
// Task 3 - Filter by category
//
Console.WriteLine("\n----- Nouns -----");

var nouns = context.Words
    .Where(w => w.Category!.Name == "Noun")
    .ToList();

foreach (var word in nouns)
{
    Console.WriteLine(word.Value);
}

//
// Task 4 - Group by category
//
Console.WriteLine("\n----- Word count by category -----");

var groups = context.Words
    .Include(w => w.Category)
    .GroupBy(w => w.Category!.Name)
    .Select(group => new
    {
        Category = group.Key,
        Count = group.Count()
    })
    .ToList();

foreach (var group in groups)
{
    Console.WriteLine($"{group.Category}: {group.Count}");
}

//
// Task 5 - Words without category
//
Console.WriteLine("\n----- Words without category -----");

var uncategorized = context.Words
    .Where(w => w.CategoryId == null)
    .ToList();

foreach (var word in uncategorized)
{
    Console.WriteLine(word.Value);
}