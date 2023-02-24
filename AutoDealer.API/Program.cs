using System.Text.Json.Serialization;
using AutoDealer.API;
using AutoDealer.API.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
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
builder.Services.AddScoped<HashService>();

builder.Services.AddControllers(options => options.UseGeneralRoutePrefix("api"));

var jwtConfigurationSection = builder.Configuration.GetSection(JwtConfig.SectionName);
builder.Services.Configure<JwtConfig>(jwtConfigurationSection);
var jwtConfig = jwtConfigurationSection.Get<JwtConfig>()!;

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

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidateAudience = true,
            ValidAudience = "apiWithAuthBackend",
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = jwtConfig.SecretKey
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler(applicationBuilder => applicationBuilder.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerPathFeature>()!.Error;
    var response = new { error = exception.Message };
    await context.Response.WriteAsJsonAsync(response);
}));

app.MapControllers().RequireAuthorization();

app.Run();