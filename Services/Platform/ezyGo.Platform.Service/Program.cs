using ezyGo.Platform.Service.Configuration;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// PDF License
QuestPDF.Settings.License = LicenseType.Community;

// IoC
builder.Services.AddDependencies(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueFrontend",
        builder =>
        {
            builder
                .WithOrigins("http://localhost:8080") // Vue app origin
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowVueFrontend");
app.UseAuthorization();

app.MapControllers();

app.Run();
