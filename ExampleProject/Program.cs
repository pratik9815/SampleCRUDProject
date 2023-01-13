using DAL;
using DAL.DataContext;
using DAL.Interface;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//This is to configure usermanager and userrole
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
//Here we should add that we added dependency while using the repository pattern
builder.Services.AddScoped<IProductRepository, ProductRepositry>();

builder.Services.AddCors();
// Add services to the container.
builder.Services.AddControllersWithViews();
//Adding DbContext this uses
//we add the data context using a separate class called daldependencyinjection
//We call the static class from here and pass the configuration parameter from here
builder.Services.AddDALDependencyInjection(builder.Configuration);

//swagger connection
//configures the open api document and add jwt bearer authentication
builder.Services.AddOpenApiDocument(options =>
{
    options.Title = "ExampleApi";
    options.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = OpenApiSecurityApiKeyLocation.Header,
        Description = "Type: Bearer {Your Jwt token}"
    });
    options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});
//configuring the jwtbearer authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    options.RequireAuthenticatedSignIn = false;
})
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenDefination:JwtKey"])),

                   ValidateIssuer = true,
                   ValidIssuer = builder.Configuration["TokenDefination:JwtIssuer"],

                   ValidateAudience = true,
                   ValidAudience = builder.Configuration["TokenDefination:JwtAudience"],

                   ValidateLifetime = true,
                   ClockSkew = TimeSpan.Zero,
               };
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
//This is to allow any method for spa files to post or get using the url
app.UseCors(options =>
{
    options.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin();
});

app.UseStaticFiles();

app.UseRouting();

//swagger connection
app.UseOpenApi();

app.UseSwaggerUi3(options =>
{
    options.Path = "/api";  

});
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
