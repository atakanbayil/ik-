using System;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("YourConnectionStringName")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure cookie authentication.
builder.Services.AddAuthentication("YourCookieAuthScheme")
    .AddCookie("YourCookieAuthScheme", options =>
    {
        options.LoginPath = "/"; // Your login path
        options.LogoutPath = "/"; // Your logout path
        options.AccessDeniedPath = "/AccessDenied"; // Path when access is denied
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

// Use authentication and authorization.
app.UseAuthentication();
app.UseAuthorization();

// Define routes
app.MapControllerRoute(
    name: "login",
    pattern: "login",
    defaults: new { controller = "Home", action = "Login" });

app.MapControllerRoute(
    name: "register",
    pattern: "register",
    defaults: new { controller = "Home", action = "Register" });

app.MapControllerRoute(
    name: "logout",
    pattern: "logout",
    defaults: new { controller = "Home", action = "Logout" });
app.MapControllerRoute(
    name: "searchList",
    pattern: "searchList",
    defaults: new { controller = "Home", action = "SearchList" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
