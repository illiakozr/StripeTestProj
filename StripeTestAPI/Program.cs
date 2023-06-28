using Serilog.Events;
using Serilog;
using StripeTestAPI.Extensions;
using StripeTestAPI.Interfaces.Services;
using StripeTestAPI.Middlewares;
using StripeTestAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Serilog to log to the console
var logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("StripeTestAPI", LogEventLevel.Information)
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddHttpClient();
// Configure Stripe
builder.Services.ConfigureStripe(builder.Configuration);
builder.Services.AddScoped<IStripeAccessFacade, StripeAccessFacade>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Register the custom exception middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
