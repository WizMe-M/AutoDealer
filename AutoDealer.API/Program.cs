using AutoDealer.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AutoDealerContext>(options => options.UseNpgsql(connectionString));

// add repos
builder.Services.AddTransient<CrudRepositoryBase<User>, UserRepository>();

// add services
builder.Services.AddScoped<AuthService>();

builder.Services.AddControllers(options => options.UseGeneralRoutePrefix("api"));

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

app.MapControllers();

app.Run();