# CodeDance.EmailSender.SendGrid

## Overview

`CodeDance.EmailSender.SendGrid` is a simple and flexible .NET library for sending emails using the SendGrid API. This package allows you to easily integrate email sending capabilities into your .NET applications, whether you're building a web application, API, or any other .NET-based project.

## Installation

You can install the package via the NuGet Package Manager, .NET CLI, or by adding it directly to your project file.

### Package source

[NuGet](https://nuget.pkg.github.com/sskset/index.json)

### .NET CLI

```bash
dotnet add package CodeDance.EmailSender.SendGrid
```

### User Secrets

```bash
dotnet user-secrets init
dotnet user-secrets set "EmailSenderConfig:ApiKey" "xxxxxx"
```

### Package Reference

```bash
<PackageReference Include="CodeDance.EmailSender.SendGrid" Version="0.0.1" />
```

## Usage

Here is a simple example of how to use the `CodeDance.EmailSender.SendGrid` package to send an email.

### Example

#### Dependency Injection

```csharp
// Email Settings
builder.Services.AddSendGridEmailSender(builder.Configuration);
// ends
```

#### appsettings.json

```csharp
  "EmailSenderConfig": {
    "SenderName": "Code Dance",
    "SenderEmail": "info@codedance.com.au",
    "ApiKey": ""
  }
```

#### Sample

```csharp
using System.Threading.Tasks;
using CodeDance.EmailSender;
using EmailSender.SendGrid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DemoAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController : ControllerBase
{
    private readonly ILogger<EmailController> _logger;
    private readonly IEmailSender _emailSender;

    public EmailController(ILogger<EmailController> logger, IEmailSender emailSender)
    {
        _logger = logger;
        _emailSender = emailSender;
    }

    [HttpPost(Name = "SendTestEmail")]
    public async Task<IActionResult> SendEmail()
    {
        await _emailSender.SendAsync(EmailTemplate.EmailConfirmationTemplateId, "ske@gmail.com", new
        {
            email = "ske@gmail.com",
            emailConfirmationLink = "https://chatgpt.com"
        });

        return Ok();
    }
}
```
