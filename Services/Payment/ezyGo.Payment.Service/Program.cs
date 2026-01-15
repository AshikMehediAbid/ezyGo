using ezyGo.Core.Notification.Email.Services;
using ezyGo.Core.Web;
using ezyGo.Payment.Domain.Managers;
using ezyGo.Payment.Domain.Managers.Interfaces;
using ezyGo.Payment.Service.Configuration;
using ezyGo.Payment.Storage.Repositories;
using ezyGo.Payment.Storage.Repositories.Interfaces;
using ezyGo.PdfGenerator.Services;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// pdf
QuestPDF.Settings.License = LicenseType.Community;

// Register HttpClient
builder.Services.AddHttpClient();

// IoC
builder.Services.AddDependencies(builder.Configuration);

// Register services
builder.Services.AddScoped<IPaymentService, AamarPayService>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITicketPdfService, TicketPdfService>();



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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

// Authentication
builder.Services.AddJwtBearerAuthentication(builder.Configuration);

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
