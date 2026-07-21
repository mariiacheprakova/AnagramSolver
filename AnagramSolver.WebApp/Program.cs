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

// Add services to the container.// knows how to create controllerds, views, ..
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Swagger is a tool that automatically generates interactive documentation for an API and allows developers to test endpoints without writing client code.

builder.Services.AddSingleton<IWordRepository, FileWordRepository>();
builder.Services.AddScoped<IAnagramSolver, AnagramSolverService>();
builder.Services.AddScoped<LetterCounter>();
builder.Services.AddSession();// registers the session service



var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) // not developing locally
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection(); // security
app.UseRouting(); // construct url like /home/index
app.UseSession(); //enables it for incoming http requests


app.UseAuthorization(); // logins and permissions

app.MapControllers();
app.MapStaticAssets(); //wwwroot - css, js, make them available

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}") // routing rule + setting default id - optional

    .WithStaticAssets();
//GET requests retrieve information.They should not modify data.
//POST is used when data changes.


//"The application follows the MVC pattern and the principle of Separation of Concerns. The controller coordinates requests, the service contains the business logic, the repository manages data access, the ViewModel transports data to the View, and the View is responsible solely for presentation. This organisation keeps the code modular, maintainable, and easier to test."

app.Run();
