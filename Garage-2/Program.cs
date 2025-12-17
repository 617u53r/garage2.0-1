using Garage_2.Data;
using Garage_2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Garage_2Context>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("Garage_2Context") ?? throw new InvalidOperationException("Connection string 'Garage_2Context' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=ParkedVehicles}/{action=Index}/{id?}")
	.WithStaticAssets();

// Create the database and apply migrations at startup
// not used in production scenarios
using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<Garage_2Context>();
	db.Database.Migrate();
}
// Populate the database
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;

	SeedData.Initialize(services);
}

app.Run();