// Custom Local Domain using HTTPS, Kestrel & ASP.NET Core
// https://dotnetplaybook.com/custom-local-domain-using-https-kestrel-asp-net-core/
// https://www.youtube.com/watch?v=96KHOaIe19w

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
