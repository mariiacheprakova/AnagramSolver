using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts.Models;

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


builder.Services.AddSingleton<IWordRepository, FileWordRepository>();
builder.Services.AddScoped<IAnagramSolver, AnagramSolverService>();
builder.Services.AddScoped<LetterCounter>();
builder.Services.AddSession();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (!app.Environment.IsDevelopment()) 
{
    app.UseExceptionHandler("/Home/Error");
    
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
    pattern: "{controller=Home}/{action=Index}/{id?}") 

    .WithStaticAssets();

app.Run();
