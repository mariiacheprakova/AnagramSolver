using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.// knows how to create controllerds, views, ..
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IWordRepository, FileWordRepository>();
builder.Services.AddScoped<IAnagramSolver, AnagramSolverService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) // not developing locally
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection(); // security
app.UseRouting(); // construct url like /home/index

app.UseAuthorization(); // logins and permissions

app.MapStaticAssets(); //wwwroot - css, js, make them available

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}") // routing rule + setting default id - optional
    .WithStaticAssets();


app.Run();
