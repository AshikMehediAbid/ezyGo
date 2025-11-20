# ezyGo Authentication Service

A production-ready authentication microservice built with .NET 8, Entity Framework Core, and JWT authentication.

## Features

- ✅ JWT Authentication with proper issuer/audience validation
- ✅ Secure password hashing using ASP.NET Identity
- ✅ Input validation with data annotations
- ✅ Comprehensive error handling with custom exceptions
- ✅ Structured logging
- ✅ Health checks
- ✅ CORS configuration
- ✅ Security headers (HSTS)
- ✅ Database constraints and indexes
- ✅ Comprehensive unit tests
- ✅ Docker support
- ✅ Production-ready configuration

## Architecture

```
ezyGo.Auth.Service/          # API Layer
├── Controllers/             # API Controllers
├── Program.cs              # Application startup

ezyGo.Auth.Domain/           # Domain Layer
├── Managers/               # Business logic
├── Models/                 # DTOs
├── Exceptions/             # Custom exceptions
└── Mapping/                # AutoMapper profiles

ezyGo.Auth.Storage/         # Data Layer
├── Entities/               # Database entities
├── Repositories/           # Data access
├── Sql/                    # DbContext
└── Migrations/             # Database migrations

ezyGo.Auth.Test/            # Test Layer
└── UnitTest1.cs            # Unit tests
```

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (or use Docker)
- Docker (optional)

### Development Setup

1. **Clone and navigate to the project:**
   ```bash
   cd Services/Auth
   ```

2. **Update connection string in `appsettings.json`:**
   ```json
   {
     "ConnectionStrings": {
       "AuthConnection": "Server=localhost;Database=ezyGoAuth;Trusted_Connection=true;TrustServerCertificate=true"
     }
   }
   ```

3. **Update JWT configuration:**
   ```json
   {
     "Jwt": {
       "Key": "YourSecretKeyHereMinimum256Bits",
       "Issuer": "ezyGoAuth",
       "Audience": "ezyGoClient"
     }
   }
   ```

4. **Run database migrations:**
   ```bash
   cd ezyGo.Auth.Service
   dotnet ef database update
   ```

5. **Run the application:**
   ```bash
   dotnet run
   ```

### Using Docker

1. **Run with Docker Compose:**
   ```bash
   docker-compose up -d
   ```

2. **Access the API:**
   - API: http://localhost:5005
   - Swagger: http://localhost:5005/swagger
   - Health Check: http://localhost:5005/health

## API Endpoints

### Authentication

#### Register User
```http
POST /api/auth/sign-up
Content-Type: application/json

{
  "userName": "johndoe",
  "email": "john@example.com",
  "phoneNumber": "+1234567890",
  "password": "SecurePass123!",
  "role": "Customer"
}
```

#### Login User
```http
POST /api/auth/sign-in
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "SecurePass123!"
}
```

#### Response
```json
{
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "tokenType": "Bearer"
}
```

## Security Features

- **Password Requirements**: Minimum 8 characters with uppercase, lowercase, number, and special character
- **JWT Security**: Proper issuer/audience validation, secure key management
- **Input Validation**: Comprehensive validation on all inputs
- **Error Handling**: Secure error messages without information leakage
- **CORS**: Configured for specific origins
- **HSTS**: HTTP Strict Transport Security enabled
- **Database Security**: Unique constraints, proper indexing

## Testing

Run the test suite:

```bash
cd ezyGo.Auth.Test
dotnet test
```

## Production Deployment

### Environment Variables

Set these environment variables in production:

```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__AuthConnection=your-production-connection-string
Jwt__Key=your-production-jwt-secret-key
Jwt__Issuer=ezyGoAuth
Jwt__Audience=ezyGoClient
```

### Docker Production

```bash
docker build -t ezygo-auth .
docker run -p 80:80 -e ConnectionStrings__AuthConnection="your-connection-string" ezygo-auth
```

## Monitoring

- **Health Check**: `/health` endpoint for monitoring
- **Logging**: Structured logging with different levels
- **Metrics**: Built-in ASP.NET Core metrics

## Security Considerations

1. **Change default JWT secret** in production
2. **Use HTTPS** in production
3. **Configure proper CORS** origins
4. **Set up database backups**
5. **Monitor logs** for security events
6. **Use environment variables** for secrets

## Contributing

1. Follow the existing code structure
2. Add tests for new features
3. Update documentation
4. Ensure all tests pass

## License

This project is part of the ezyGo system.
