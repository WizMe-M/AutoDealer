var culture = new CultureInfo("en");
Thread.CurrentThread.CurrentCulture = culture;
Thread.CurrentThread.CurrentUICulture = culture;
ValidatorOptions.Global.LanguageManager.Culture = culture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<HttpClient>();
builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSingleton<ApiClient>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddMvc(options => options.Filters.Add(new ApiAuthExceptionFilterAttribute()));

builder.Services.AddFluentValidationAutoValidation(configuration => configuration.DisableDataAnnotationsValidation = true);
builder.Services.AddValidatorsFromAssemblyContaining<LoginUser>(ServiceLifetime.Singleton);

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/Auth/Login");
        options.LogoutPath = new PathString("/Auth/Logout");
        options.ExpireTimeSpan = TimeSpan.FromHours(6);
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();