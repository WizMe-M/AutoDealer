using System.Text.Json.Serialization;
using AutoDealer.API.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
AutoDealerContext.ConfigureBuilder(dataSourceBuilder);
var dataSource = dataSourceBuilder.Build();
builder.Services.AddDbContext<AutoDealerContext>(options => options.UseNpgsql(dataSource));

// add repos
builder.Services.AddTransient<CrudRepositoryBase<User>, UserRepository>();
builder.Services.AddTransient<CrudRepositoryBase<Employee>, EmployeeRepository>();

// add services
builder.Services.AddScoped<AuthService>();

builder.Services.AddControllers(options => options.UseGeneralRoutePrefix("api"));
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler(applicationBuilder => applicationBuilder.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerPathFeature>()!.Error;
    var response = new { error = exception.Message };
    await context.Response.WriteAsJsonAsync(response);
}));

app.MapControllers();

app.Run();