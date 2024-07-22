using Mapster;
using Marten;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarter();
builder.Services.AddMediatR(config => { 
config.RegisterServicesFromAssembly(typeof(Program).Assembly);

});
builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
builder.Services.AddMarten(Option => 
{ Option.Connection(builder.Configuration.GetConnectionString("Database")); }).UseLightweightSessions();
var app = builder.Build();

//app.MapGet("/", () => "Working!");
app.MapCarter();
app.Run();
