using Api.Data;
using Api.Data.Models;
using Api.Rules;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.EnableAnnotations();
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "Employee Benefit Cost Calculation Api",
		Description = "Api to support employee benefit cost calculations"
	});
});

builder.Services.AddMemoryCache(); // Typically would not use a MemoryCache and instead would use something like Redis/ElastiCache
builder.Services.AddRules();
builder.Services.AddRepositories();
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var allowLocalhost = "allow localhost";
builder.Services.AddCors(options =>
{
	options.AddPolicy(allowLocalhost,
		policy => { policy.WithOrigins("http://localhost:3000", "http://localhost"); });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

	// Make sure the database is created and seeded
	using var scope = app.Services.CreateScope();
	var scopedServices = scope.ServiceProvider;
	var context = scopedServices.GetRequiredService<DataContext>();
	context.Database.EnsureCreated();
}

app.UseCors(allowLocalhost);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }