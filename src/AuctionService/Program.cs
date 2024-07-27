using AuctionService.Data;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddNpgsql<AuctionDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x =>
{
    x.AddEntityFrameworkOutbox<AuctionDbContext>(opt =>
    {
        opt.QueryDelay = TimeSpan.FromSeconds(10);

        opt.UsePostgres();
        opt.UseBusOutbox();
    });

    x.UsingRabbitMq((context, configure) =>
    {
        configure.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

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