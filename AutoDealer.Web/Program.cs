using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<HttpClient>(provider =>
{
    var client = new HttpClient();
#if DEBUG
    client.BaseAddress = new Uri("https://localhost:7138/");
#else
        ApiClient.BaseAddress = new Uri("https://api:44357/");
#endif
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "*/*");
    return client;
});


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddMvc();

builder.Services
    .AddControllersWithViews()
    .AddRazorRuntimeCompilation();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();