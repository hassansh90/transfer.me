using Microsoft.Azure.Cosmos;
using Server;

var builder = WebApplication.CreateBuilder(args); //this line enumerates the secrets.json file stored in appdata

// Read connection string from User Secrets (dev) or Environment Variables (prod)
var cosmosConnectionString = builder.Configuration["CosmosDb:ConnectionString"] 
    ?? throw new InvalidOperationException("CosmosDb:ConnectionString not configured. Run: dotnet user-secrets set \"CosmosDb:ConnectionString\" \"<your-connection-string>\"");

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Adding this service fixed the error of Dependency Injection error
builder.Services.AddTransient<ICosmosDbService, CosmosDbService>();
builder.Services.AddTransient<IDownloadService, DownloadService>();
builder.Services.AddTransient<Server.Logger.ILogger, Server.Logger.Logger>();
builder.Services.AddSingleton<CosmosClient>(ServiceProvider =>
{
    return new CosmosClient(cosmosConnectionString);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCorsPolicy", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors("DevCorsPolicy");

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

// Serve static files from Blazor WebAssembly (automatically included via ProjectReference)
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

// Fallback to index.html for Blazor client-side routing (must be after MapControllers)
// This handles all non-API routes and serves index.html for client-side routing
// The fallback automatically excludes routes that are already mapped (like /api/*)
app.MapFallbackToFile("index.html");

app.Run();
