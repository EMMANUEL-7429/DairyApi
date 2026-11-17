using DairyApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------
// Add services to the container
// ---------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Register DbContext with SQL Server ---
builder.Services.AddDbContext<DairyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Configure CORS for Angular dev server ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular dev origin
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --- Configure HTTPS redirection ---
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7187; // match your launchSettings.json
});

var app = builder.Build();

// ---------------------------
// Configure the HTTP request pipeline
// ---------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// --- Use CORS BEFORE Authorization & MapControllers ---
app.UseCors("AllowAngularDev");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
