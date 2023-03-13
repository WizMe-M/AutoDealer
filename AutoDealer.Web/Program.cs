var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
AutoDealerContext.ConfigureBuilder(dataSourceBuilder);
var dataSource = dataSourceBuilder.Build();
builder.Services.AddDbContext<AutoDealerContext>(options => options.UseNpgsql(dataSource));

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
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "AutoDealer API", Version = "v1" });
    options.AddSecurityDefinition(
        JwtBearerDefaults.AuthenticationScheme,
        new OpenApiSecurityScheme
        {
            Description = """
JWT Authorization header using the Bearer scheme. 
Enter 'Bearer' [space] and then your token in the text input below.
Example: 'Bearer 12345'
""",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = JwtBearerDefaults.AuthenticationScheme
        });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddFluentValidationAutoValidation(configuration =>
{
    configuration.DisableDataAnnotationsValidation = true;
});

builder.Services.AddValidatorsFromAssemblyContaining<Program>(lifetime: ServiceLifetime.Singleton);

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
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = jwtConfig.SecretKey,
            ValidateAudience = false,
            LifetimeValidator = (notBefore, expires, _, _) =>
                notBefore <= DateTime.UtcNow && expires > DateTime.UtcNow
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler(applicationBuilder => applicationBuilder.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerPathFeature>()!.Error;
    var problemResponse = new ProblemDetails
    {
        Title = $"Caught an exception while proceeding request. {exception.Message}",
        Detail = exception.InnerException?.Message,
        Status = StatusCodes.Status400BadRequest,
        Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.1",
    };
    await context.Response.WriteAsJsonAsync(problemResponse);
}));

app.MapControllers().RequireAuthorization();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<AutoDealerContext>()!;
    await context.Database.MigrateAsync();
}

app.Run();