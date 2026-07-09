using dotnet_starter.Middleware;
using dotnet_starter.Presenters;
using dotnet_starter.Utils;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory()
});

builder.Configuration
    .AddYamlFile("config.yml", optional: false, reloadOnChange: true);


builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<AuthorizationUtils>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin()     // 🔓 Allow all origins
            .AllowAnyMethod()     // 🔨 Allow all HTTP methods (GET, POST, etc.)
            .AllowAnyHeader();    // 🧾 Allow any header
    });
});


builder.Services.AddHttpClient<IHttpClientHelper, HttpClientHelper>();

builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>()
    .AddClasses(classes => classes.InNamespaces(
        "dotnet_starter.Services",
        "dotnet_starter.Utils"
    ))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
);


builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = ValidationResponseHandler.HandleValidationError;
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<JwtMiddleware>();

app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
