// Custom Local Domain using HTTPS, Kestrel & ASP.NET Core
// https://dotnetplaybook.com/custom-local-domain-using-https-kestrel-asp-net-core/
// https://www.youtube.com/watch?v=96KHOaIe19w
// Some problem whit my Have from .NET 3.X and 5 to .NET 6.0
// https://stackoverflow.com/questions/69904260/configuring-kestrel-server-options-in-net-6-startup
// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/certauth?view=aspnetcore-6.0#configure-your-server-to-require-certificates

// NOTE:
// It is working. Just certificate my be added manualy to browser
// The path is: https://weather.io:5001/weatherforecast

using HttpsTest;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.ConfigureHttpsDefaults(options =>
        options.ClientCertificateMode = ClientCertificateMode.RequireCertificate);
});

builder.Host.ConfigureServices((context, services) =>
{
    HostConfig.CertPath = builder.Configuration["CertPath"];
    HostConfig.CertPassword = builder.Configuration["CertPassword"];
});

/// <summary>
/// Wont work
/// </summary>
//builder.Host.ConfigureWebHostDefaults(webBuilder =>
//{
//    webBuilder.ConfigureKestrel(opt =>
//    {
//        opt.ListenAnyIP(5000);
//        opt.ListenAnyIP(5001, listOpt =>
//        {
//            listOpt.UseHttps(HostConfig.CertPath, HostConfig.CertPassword);
//        });
//    });
//});

builder.WebHost.UseKestrel(opt =>
{
    // Just demo, not use in production
    var host = Dns.GetHostEntry("weather.io");
    //opt.ListenAnyIP(500);
    opt.Listen(host.AddressList[0], 5000);
    opt.Listen(host.AddressList[0], 5001, listOpt =>
    {
        listOpt.UseHttps(HostConfig.CertPath, HostConfig.CertPassword);
    });
});

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
