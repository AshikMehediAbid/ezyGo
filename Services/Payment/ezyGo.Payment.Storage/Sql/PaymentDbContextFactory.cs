using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ezyGo.Payment.Storage.Sql;

public class PaymentDbContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
{
    public PaymentDbContext CreateDbContext(string[] args)
    {
        // Get the assembly location to find the project directory
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
        
        // Navigate from bin/Debug/net10.0 to the Storage project root
        var storageProjectPath = assemblyDirectory;
        if (!string.IsNullOrEmpty(storageProjectPath) && storageProjectPath.Contains("bin"))
        {
            var parent = Directory.GetParent(assemblyDirectory!);
            storageProjectPath = parent?.Parent?.Parent?.FullName;
        }
        
        // Try multiple paths to find the Service project
        var possiblePaths = new List<string>();
        
        if (storageProjectPath != null)
        {
            possiblePaths.Add(Path.Combine(storageProjectPath, "../ezyGo.Payment.Service"));
            possiblePaths.Add(Path.Combine(storageProjectPath, "..", "ezyGo.Payment.Service"));
        }
        
        // Also try from current directory
        possiblePaths.Add(Path.Combine(Directory.GetCurrentDirectory(), "../ezyGo.Payment.Service"));
        possiblePaths.Add(Path.Combine(Directory.GetCurrentDirectory(), "../../../Services/Payment/ezyGo.Payment.Service"));
        possiblePaths.Add(Path.Combine(Directory.GetCurrentDirectory(), "../../Services/Payment/ezyGo.Payment.Service"));
        
        string? basePath = null;
        foreach (var path in possiblePaths)
        {
            if (path != null && Directory.Exists(path))
            {
                basePath = path;
                break;
            }
        }

        // If we still can't find it, try to locate it relative to the solution
        if (basePath == null || !Directory.Exists(basePath))
        {
            var currentDir = Directory.GetCurrentDirectory();
            // Look for the solution root
            var dir = new DirectoryInfo(currentDir);
            while (dir != null && !dir.GetFiles("*.sln").Any())
            {
                dir = dir.Parent;
            }
            
            if (dir != null)
            {
                basePath = Path.Combine(dir.FullName, "Services", "Payment", "ezyGo.Payment.Service");
            }
        }

        // Build configuration - order matters: later sources override earlier ones
        var builder = new ConfigurationBuilder();
        
        // Load appsettings first (lowest precedence)
        if (basePath != null && Directory.Exists(basePath))
        {
            builder.SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true);
        }
        
        // User secrets override appsettings (loaded via UserSecretsId)
        builder.AddUserSecrets("45c18765-cfa9-4eb0-acfd-4ba5c40f47aa"); // Payment.Service UserSecretsId
        
        // Environment variables take highest precedence (loaded last)
        builder.AddEnvironmentVariables();

        var configuration = builder.Build();

        // Get connection string from configuration - try multiple methods
        var connectionString = configuration.GetConnectionString("PaymentConnection")
            ?? configuration["ConnectionStrings:PaymentConnection"]
            ?? Environment.GetEnvironmentVariable("ConnectionStrings__PaymentConnection");

        // Check if connection string is still the placeholder value
        if (string.IsNullOrWhiteSpace(connectionString) || connectionString == "[See User Secrets]")
        {
            throw new InvalidOperationException(
                "Could not find a valid connection string named 'PaymentConnection'. " +
                "The connection string is either missing or still set to the placeholder value '[See User Secrets]'. " +
                "Please ensure it is configured in user secrets or environment variables. " +
                $"Current value: '{(string.IsNullOrWhiteSpace(connectionString) ? "null or empty" : connectionString)}'");
        }

        var optionsBuilder = new DbContextOptionsBuilder<PaymentDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new PaymentDbContext(optionsBuilder.Options);
    }
}

