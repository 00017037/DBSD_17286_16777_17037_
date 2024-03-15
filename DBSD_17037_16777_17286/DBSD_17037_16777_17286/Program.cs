using DBSD_17037_16777_17286.DAL.Infrastructure;
using DBSD_17037_16777_17286.DAL.Models;
using DBSD_17037_16777_17286.DAL.Repositories;
using DBSD_17037_16777_17286.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(typeof(Program));


IConfiguration config = builder.Configuration;

string connStr = config.GetConnectionString("Macro")
    .Replace("|DataDirectory|", builder.Environment.ContentRootPath);
builder.Services.AddDbContext<MacroDbContext>(options =>
    options.UseSqlServer(connStr)
);


// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddScoped<IRepository<Employee>,EmployeeRepository>();
builder.Services.AddScoped<IRepository<Person>,PersonRepository>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200") // Specify the client origin here
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
