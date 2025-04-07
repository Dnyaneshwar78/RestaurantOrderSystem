using Microsoft.OpenApi.Models;
using Orleans.Configuration;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Load Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// 2️⃣ Logging
builder.Logging.AddConsole();

// 3️⃣ Orleans Configuration with Dashboard
builder.Host.UseOrleans((context, siloBuilder) =>
{
    var config = context.Configuration;
    var connectionString = config.GetConnectionString("PostgresConnection");

    siloBuilder.UseAdoNetClustering(options =>
    {
        options.Invariant = "Npgsql";
        options.ConnectionString = connectionString;
    });

    siloBuilder.AddAdoNetGrainStorage("OrderStorage", options =>
    {
        options.Invariant = "Npgsql";
        options.ConnectionString = connectionString;
    });

    siloBuilder.Configure<ClusterOptions>(options =>
    {
        options.ClusterId = config["Orleans:ClusterId"];
        options.ServiceId = config["Orleans:ServiceId"];
    });

    // 🧩 Enable Orleans Dashboard
    siloBuilder.UseDashboard(options =>
    {
        options.HostSelf = true;
        options.Port = 8081; // Access the dashboard at http://localhost:8081
    });
});

// 4️⃣ Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Restaurant Order API", Version = "v1" });
});

var app = builder.Build();

// 5️⃣ Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant Order API V1");
    });
}

app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
