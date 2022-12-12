
using LinkShortener.Interfaces;
using LinkShortener.Repository;
using LinkShortener.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddControllers().Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddTransient<ILinksService, LinksService>()
    .AddSingleton<IQrService, QrService>()
    .AddDbContext<LinkContext>(x => x
        .UseNpgsql(builder.Configuration.GetConnectionString("Links")));


var app = builder.Build();

app
    .UseSwagger()
    .UseSwaggerUI();

app.MapControllers();
app.Run();