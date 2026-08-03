using AnagramSolver.BusinessLogic;
using AnagramSolver.BusinessLogic.Decorators;
using AnagramSolver.BusinessLogic.Filters;
using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst.Data;
using AnagramSolver.EF.CodeFirst.Import;
using Microsoft.EntityFrameworkCore;
using ILogger = AnagramSolver.Contracts.ILogger;

var builder = WebApplication.CreateBuilder(args);

string connectionString =
    builder.Configuration.GetConnectionString(
        "AnagramDatabase")
    ?? throw new InvalidOperationException(
        "AnagramDatabase connection string is missing.");

builder.Services.AddDbContext<AnagramDbContext>(
    options =>
        options.UseSqlServer(connectionString));

AnagramSettings settings =
    builder.Configuration
        .GetSection("AnagramSettings")
        .Get<AnagramSettings>()
    ?? throw new InvalidOperationException(
        "AnagramSettings configuration is missing.");

builder.Services.AddSingleton(settings);

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<
    IWordRepository,
    EfWordRepository>();

builder.Services.AddScoped<
    ISearchLogRepository,
    EfSearchLogRepository>();

builder.Services.AddScoped<
    WordDatabaseSeeder>();

builder.Services.AddSingleton<
    MemoryCache<IReadOnlyCollection<string>>>();

builder.Services.AddSingleton<
    ILogger,
    Logging>();

builder.Services.AddScoped<IAnagramSolver>(
    serviceProvider =>
    {
        IWordRepository repository =
            serviceProvider.GetRequiredService<
                IWordRepository>();

        ISearchLogRepository searchLogRepository =
            serviceProvider.GetRequiredService<
                ISearchLogRepository>();

        ILogger logger =
            serviceProvider.GetRequiredService<
                ILogger>();

        MemoryCache<IReadOnlyCollection<string>> cache =
            serviceProvider.GetRequiredService<
                MemoryCache<IReadOnlyCollection<string>>>();

        var lengthFilter =
            new LengthFilter();

        var letterFilter =
            new LetterFilter();

        var supportedWordTypeFilter =
            new SupportedWordTypeFilter();

        lengthFilter.SetNext(
            letterFilter);

        letterFilter.SetNext(
            supportedWordTypeFilter);

        IAnagramSolver solver =
            new AnagramSolverService(
                repository,
                lengthFilter,
                searchLogRepository);

        solver =
            new CacheDecorator(
                solver,
                cache);

        solver =
            new LoggingDecorator(
                logger,
                solver);

        return solver;
    });

builder.Services.AddSession();

var app =
    builder.Build();

await using (AsyncServiceScope scope =
    app.Services.CreateAsyncScope())
{
    WordDatabaseSeeder seeder =
        scope.ServiceProvider
            .GetRequiredService<
                WordDatabaseSeeder>();

    string dictionaryPath =
        Path.Combine(
            app.Environment.ContentRootPath,
            "zodynas.txt");

    int importedCount =
        await seeder.ImportAsync(
            dictionaryPath);

    Console.WriteLine(
        $"Dictionary import completed. " +
        $"Imported {importedCount} new words.");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(
        "/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern:
            "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();