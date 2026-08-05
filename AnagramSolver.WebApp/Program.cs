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
builder.Services.AddSession();
builder.Services.AddSingleton<
    MemoryCache<IReadOnlyCollection<string>>>();
builder.Services.AddSingleton<
    ILogger,
    Logging>();
builder.Services.AddScoped<IWordRepository, FileWordRepository>();
builder.Services.AddScoped<IAnagramSolver>(
    serviceProvider =>
    {
        IWordRepository repository =
            serviceProvider.GetRequiredService<
                IWordRepository>();
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
            lengthFilter);
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

var app =
    builder.Build();
await using (AsyncServiceScope scope =
    app.Services.CreateAsyncScope())
{
    var dictionaryPath =
        Path.Combine(
            app.Environment.ContentRootPath,
            "zodynas.txt");
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