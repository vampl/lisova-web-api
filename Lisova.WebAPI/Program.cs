using Lisova.Services.Context.Data;
using Lisova.Services.Repositories;
using Lisova.WebAPI.Mappers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Get connection data stings, check if connection data is present 
var connectionString = builder.Configuration.GetConnectionString("LisovaDatabase") ??
                       throw new Exception("The `LisovaDatabase` connection string is not set.");
_ = builder.Configuration.GetValue<string?>("SQLiteDatabaseFile", null) ??
    throw new Exception("The `SQLiteDatabaseFile` configuration setting is not set.");

// Add services to the container.
builder.Services.AddDbContext<LisovaContext>(options => options.UseSqlite(connectionString));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeMapper, EmployeeMapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();