using ThreadPilot_DataModels;
using WebApplication_TEKsystem_Test_B;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


// Registrera HttpClientFactory
builder.Services.AddHttpClient();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// # add Feature toggles
builder.Services.Configure<FeatureToggles>(
    builder.Configuration.GetSection("FeatureToggles"));

// # add Configurable endpoint for Vehicle lookups
builder.Services.Configure<VehicleServiceOptions>(builder.Configuration.GetSection("VehicleService"));

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

// ## health check
app.MapGet("/health", () => Results.Ok("Healthy"));


app.Run();
