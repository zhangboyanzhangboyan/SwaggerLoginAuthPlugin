# SwaggerAuthPlugin

A Swagger authentication plugin for ASP.NET Core applications.

## Features

- Simple login page for Swagger UI protection
- Configurable username/password
- Session-based authentication
- Easy integration with existing ASP.NET Core projects

## Installation

```bash
dotnet add package SwaggerAuthPlugin
```

## Configuration

In your `Program.cs`:

```csharp
builder.Services.AddSwaggerAuth(options =>
{
    options.Enabled = true;
    options.DefaultUsername = "admin";
    options.DefaultPassword = "123456";
});

app.UseSwaggerAuth();
app.UseSwagger();
```

## Options

| Option | Description | Default |
|--------|-------------|---------|
| `Enabled` | Enable/disable authentication | `true` |
| `CheckUrl` | URL to check login status | `/swagger/check-login` |
| `SubmitUrl` | URL to submit login | `/swagger/submit-login` |
| `EnableOnProduction` | Enable in production | `true` |
| `DefaultUsername` | Default username | `admin` |
| `DefaultPassword` | Default password | `1234567` |

## License

MIT