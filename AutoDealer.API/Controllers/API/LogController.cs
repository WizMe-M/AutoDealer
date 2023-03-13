namespace AutoDealer.API.Controllers.API;

[Authorize(Roles = nameof(Post.DatabaseAdmin))]
[ApiController]
[Route("logs")]
public class LogController : DbContextController<Log>
{
    public LogController(AutoDealerContext context) : base(context)
    {
    }

    [HttpGet]
    public IActionResult Get()
    {
        var logs = Context.Logs.ToArray();
        return Ok("Logs listed", logs);
    }

    protected override Task LoadReferencesAsync(Log entity) => throw new NotSupportedException();
}