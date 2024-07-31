

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarter();
builder.Services.AddMediatR(config => { 
config.RegisterServicesFromAssembly(typeof(Program).Assembly);
config.AddOpenBehavior(typeof(ValidationBehaviour<,>)); 

});
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
builder.Services.AddMarten(Option => 
{ Option.Connection(builder.Configuration.GetConnectionString("Database")); }).UseLightweightSessions();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
var app = builder.Build();

/// <summary>
///  Below is code for Handleling Global Exception Generic Way also implemented Custom Exception Handeling ...
/// </summary>
///
/*
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (exception == null)
        {
            return;
        }

        var problemDetails = new ProblemDetails
        {
            Title = exception.Message,
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.StackTrace

        };

        var logger = context.RequestServices.GetService<ILogger<Program>>();
        logger.LogError(exception,exception.Message);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problemDetails);


    });
});*/
//app.MapGet("/", () => "Working!");
app.UseExceptionHandler(options => { });
app.MapCarter();
app.Run();
