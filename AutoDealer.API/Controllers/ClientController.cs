namespace AutoDealer.API.Controllers;

[Authorize]
[ApiController]
[Route("clients")]
public class ClientController : DbContextController<Client>
{
    public ClientController(AutoDealerContext context) : base(context)
    {
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var clients = Context.Clients.ToArray();
        return Ok("All clients listed", clients);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var client = Find(id);
        return client is { }
            ? Ok("Client found", client)
            : NotFound("Client with such ID doesn't exist");
    }

    [Authorize(Roles = nameof(Post.Seller))]
    [HttpPost("create")]
    public IActionResult CreateEmployee([FromBody] ClientData data)
    {
        var client = new Client
        {
            FirstName = data.FullName.FirstName,
            LastName = data.FullName.LastName,
            MiddleName = data.FullName.MiddleName,
            Birthdate = data.BirthData.Birthdate,
            Birthplace = data.BirthData.Birthplace,
            PassportNumber = data.Passport.Number,
            PassportSeries = data.Passport.Series,
            PassportIssuer = data.Passport.Issuer,
            PassportDepartmentCode = data.Passport.DepartmentCode
        };

        var foundWithPassport = Context.Clients.FirstOrDefault(emp =>
            emp.PassportNumber == client.PassportNumber &&
            emp.PassportSeries == client.PassportSeries) is { };

        if (foundWithPassport) return BadRequest("There is already client with set passport data");

        Context.Clients.Add(client);
        Context.SaveChanges();

        return Ok("Client was successfully created", client);
    }

    [Authorize(Roles = nameof(Post.Seller))]
    [HttpPatch("{id:int}/update/full-name")]
    public IActionResult UpdateFullName(int id, [FromBody] FullName fullName)
    {
        var found = Find(id);
        if (found is null) return NotFound("Client with such ID doesn't exist");

        found.FirstName = fullName.FirstName;
        found.LastName = fullName.LastName;
        found.MiddleName = fullName.MiddleName;
        Context.Clients.Update(found);
        Context.SaveChanges();

        return Ok("Client's full name was updated", found);
    }

    [Authorize(Roles = nameof(Post.Seller))]
    [HttpPatch("{id:int}/update/birth")]
    public IActionResult UpdateBirthData(int id, [FromBody] BirthData birthData)
    {
        var found = Find(id);
        if (found is null) return NotFound("Client with such ID doesn't exist");

        found.Birthplace = birthData.Birthplace;
        found.Birthdate = birthData.Birthdate;
        Context.Clients.Update(found);
        Context.SaveChanges();

        return Ok("Client's birth data was updated", found);
    }

    [Authorize(Roles = nameof(Post.Seller))]
    [HttpPatch("{id:int}/update/passport")]
    public IActionResult UpdatePassport(int id, [FromBody] FullPassport passport)
    {
        var found = Find(id);
        if (found is null) return NotFound("Client with such ID doesn't exist");

        var foundWithPassport = Context.Clients.FirstOrDefault(emp =>
            emp.Id != id
            && emp.PassportSeries == passport.Series
            && emp.PassportNumber == passport.Number) is { };
        if (foundWithPassport)
            return BadRequest("There is already another client with set passport data");

        found.PassportSeries = passport.Series;
        found.PassportNumber = passport.Number;
        found.PassportIssuer = passport.Issuer;
        found.PassportDepartmentCode = passport.DepartmentCode;
        Context.Clients.Update(found);
        Context.SaveChanges();

        return Ok("Client's passport was updated", found);
    }

    [Authorize(Roles = nameof(Post.Seller))]
    [HttpDelete("{id:int}/delete")]
    public IActionResult Delete(int id)
    {
        var client = Find(id);
        if (client is null) return NotFound("Client with such ID doesn't exist");

        Context.Clients.Remove(client);
        Context.SaveChanges();

        return Ok("Client was deleted", client);
    }

    private Client? Find(int id) => Context.Clients.FirstOrDefault(e => e.Id == id);

    protected override Task LoadReferencesAsync(Client entity) => throw new NotSupportedException();
}