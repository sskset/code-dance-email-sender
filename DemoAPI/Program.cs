
using CodeDance.EmailSender;
using CodeDance.EmailSender.SendGrid;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SendGrid;

namespace DemoAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        // Email Settings
        builder.Services.Configure<EmailSenderConfig>(builder.Configuration.GetSection("EmailSender"));
        builder.Services.AddTransient<ISendGridClient>(sp =>
        {
            var emailSettings = sp.GetService<IOptions<EmailSenderConfig>>();
            return new SendGridClient(emailSettings.Value.ApiKey);
        });
        builder.Services.AddTransient<IEmailSender>(sp=>{
            var emailSettings = sp.GetService<IOptions<EmailSenderConfig>>();
            return new SendGridEmailSender(emailSettings.Value, sp.GetRequiredService<ISendGridClient>());
        });
        // ends


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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
    }
}
