using DAL;
using DAL.DataContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Adding DbContext  this  uses
builder.Services.AddDALDependencyInjection(builder.Configuration);

//swagger connection
builder.Services.AddOpenApiDocument(options =>
{
    options.Title = "ExampleApi";
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

//swagger connection
app.UseOpenApi();

app.UseSwaggerUi3(options =>
{
    options.Path = "/api";  
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
