using AuctionService.Data;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddNpgsql<AuctionDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddScoped<IItemCategoryRepository, ItemCategoryRepository>();

builder.Services.AddMassTransit(x =>
{
    x.AddEntityFrameworkOutbox<AuctionDbContext>(opt =>
    {
        opt.QueryDelay = TimeSpan.FromSeconds(10);

        opt.UsePostgres();
        opt.UseBusOutbox();
    });

    x.AddConsumersFromNamespaceContaining<AuctionFinished>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false));

    x.UsingRabbitMq((context, configure) =>
    {
        configure.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
        {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });

        configure.ConfigureEndpoints(context);
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.Authority = builder.Configuration["IdentityServiceUrl"];
        opt.RequireHttpsMetadata = false;
        opt.TokenValidationParameters.ValidateAudience = false;
        opt.TokenValidationParameters.NameClaimType = "username";
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    DbInitializer.InitDb(app);
}
catch (Exception)
{
    Console.WriteLine("Seed exception occurred");
}

app.Run();

public partial class Program { }