namespace AutoDealer.API.Controllers;

[Authorize]
[ApiController]
[Route("tests")]
public class TestController : DbContextController<Test>
{
    public TestController(AutoDealerContext context) : base(context)
    {
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var tests = Context.Tests
            .Include(test => test.TestAutos)
            .ThenInclude(testAuto => testAuto.Auto)
            .ThenInclude(auto => auto.CarModel)
            .Include(test => test.TestAutos)
            .ThenInclude(testAuto => testAuto.Auto)
            .ThenInclude(auto => auto.Details)
            .ThenInclude(detail => detail.DetailSeries)
            .ToArray();
        return Ok("All tests listed", tests);
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var found = Find(id);
        return found is { }
            ? Ok("Test found", found)
            : Problem(detail: "Test with such ID doesn't exist", statusCode: StatusCodes.Status404NotFound);
    }

    [Authorize(Roles = nameof(Post.Tester))]
    [HttpPost("start")]
    public IActionResult StartTest()
    {
        var test = new Test { StartDate = DateOnly.FromDateTime(DateTime.Today) };

        Context.Tests.Add(test);
        Context.SaveChanges();

        return Ok("Test started", test);
    }

    [Authorize(Roles = nameof(Post.Tester))]
    [HttpPut("{id:int}/autos/set")]
    public async Task<IActionResult> SetAutoToTest(int id, ICollection<int> autoIds)
    {
        var found = Find(id);
        if (found == null)
            return Problem(detail: "Test with such ID doesn't exist", statusCode: StatusCodes.Status404NotFound);

        if (found.EndDate is { })
            return Problem(detail: "Test is already finished", statusCode: StatusCodes.Status400BadRequest);

        var autos = Context.Autos
            .Where(auto => autoIds.Contains(auto.Id))
            .ToArray();

        if (autos.Length < autoIds.Count)
        {
            var missingIds = autoIds.Where(autoId => autos.All(auto => auto.Id != autoId)).ToArray();
            var error = new
            {
                message = "Can't found Autos by specified IDs. Remove them",
                missingIdList = missingIds
            }.ToString();
            return Problem(detail: error, statusCode: StatusCodes.Status400BadRequest);
        }

        if (autos.Any(auto => auto.Status is not AutoStatus.Assembled))
        {
            var unsuitableAutos = autos.Where(auto => auto.Status is not AutoStatus.Assembled);
            var error = new
            {
                message = "List of IDs references to Autos that are already testing, were testing or were sold",
                autos = unsuitableAutos
            }.ToString();
            return Problem(detail: error, statusCode: StatusCodes.Status400BadRequest);
        }

        foreach (var auto in autos)
        {
            var testingAuto = new TestAuto { IdAuto = auto.Id };
            found.TestAutos.Add(testingAuto);
        }

        Context.Tests.Update(found);
        await Context.SaveChangesAsync();

        foreach (var auto in autos)
        {
            auto.Status = AutoStatus.Testing;
            Context.Autos.Update(auto);
        }

        await Context.SaveChangesAsync();
        await LoadReferencesAsync(found);

        return Ok("Autos were set for test", found);
    }

    [Authorize(Roles = nameof(Post.Tester))]
    [HttpPatch("{testId:int}/autos/{autoId:int}/certify")]
    public async Task<IActionResult> SetCertificationStatuses(int testId, int autoId, [FromBody] TestStatus testStatus)
    {
        var found = Context.Tests
            .Include(test => test.TestAutos)
            .ThenInclude(testAuto => testAuto.Auto)
            .ThenInclude(auto => auto.CarModel)
            .FirstOrDefault(test => test.Id == testId);
        if (found is null)
            return Problem(detail: $"Test with such ID ({testId}) doesn't exist",
                statusCode: StatusCodes.Status404NotFound);

        var testAuto = found.TestAutos.FirstOrDefault(auto => auto.IdAuto == autoId);
        if (testAuto is null)
            return Problem(detail: $"Test doesn't contain auto with such ID ({autoId})",
                statusCode: StatusCodes.Status404NotFound);

        if (found.EndDate is { })
            return Problem(detail: "Test is already finished", statusCode: StatusCodes.Status400BadRequest);


        testAuto.CertificationDate = testStatus is not TestStatus.NotChecked
            ? DateOnly.FromDateTime(DateTime.Today)
            : null;
        testAuto.Status = testStatus;

        Context.Tests.Update(found);
        await Context.SaveChangesAsync();

        await LoadReferencesAsync(found);

        return Ok("Test status for auto was set", found);
    }

    [Authorize(Roles = nameof(Post.Tester))]
    [HttpPut("{id:int}/autos/certify")]
    public async Task<IActionResult> SetCertificationStatusesForAll(int id, ICollection<TestedAuto> testedAutos)
    {
        var found = Context.Tests
            .Include(test => test.TestAutos)
            .ThenInclude(auto => auto.Auto)
            .ThenInclude(auto => auto.CarModel)
            .FirstOrDefault(test => test.Id == id);
        if (found is null)
            return Problem(detail: "Test with such ID doesn't exist", statusCode: StatusCodes.Status404NotFound);

        if (found.EndDate is { }) return Problem("Test is already finished");

        var autos = Context.Autos
            .Where(auto => testedAutos.Select(testedAuto => testedAuto.AutoId).Contains(auto.Id))
            .ToArray();

        if (autos.Length < testedAutos.Count)
        {
            var missingIds = testedAutos
                .Where(autoId => autos.All(auto => auto.Id != autoId.AutoId))
                .ToArray();
            var error = new
            {
                message = "Can't found Autos by specified IDs. Remove them",
                missingIdList = missingIds
            }.ToString();
            return Problem(detail: error, statusCode: StatusCodes.Status400BadRequest);
        }

        foreach (var testedAuto in testedAutos)
        {
            var testAuto = found.TestAutos.First(auto => auto.IdAuto == testedAuto.AutoId);
            testAuto.Status = testedAuto.Status;
            testAuto.CertificationDate = testedAuto.Status is not TestStatus.NotChecked
                ? DateOnly.FromDateTime(DateTime.Today)
                : null;
            Context.TestAutos.Update(testAuto);
        }

        await Context.SaveChangesAsync();
        await LoadReferencesAsync(found);

        return Ok("Certification statuses were set for testing autos", found);
    }

    [Authorize(Roles = nameof(Post.Tester))]
    [HttpPost("{id:int}/finish")]
    public async Task<IActionResult> FinishTest(int id)
    {
        var found = Context.Tests
            .Include(test => test.TestAutos)
            .ThenInclude(testAuto => testAuto.Auto)
            .ThenInclude(auto => auto.CarModel)
            .FirstOrDefault(test => test.Id == id);
        if (found is null)
            return Problem(detail: "Test with such ID doesn't exist", statusCode: StatusCodes.Status404NotFound);

        if (found.EndDate is { }) return Problem("Test is already finished");

        found.EndDate = DateOnly.FromDateTime(DateTime.Today);

        foreach (var testAuto in found.TestAutos)
        {
            var auto = Context.Autos.First(a => a.Id == testAuto.IdAuto);
            auto.Status = testAuto.Status is TestStatus.Certified ? AutoStatus.Selling : AutoStatus.Assembled;
            Context.Autos.Update(auto);

            if (testAuto.Status is TestStatus.NotChecked)
                testAuto.CertificationDate = DateOnly.FromDateTime(DateTime.Today);
        }

        Context.Tests.Update(found);
        await Context.SaveChangesAsync();
        await LoadReferencesAsync(found);

        return Ok("Test finished", found);
    }

    protected override async Task LoadReferencesAsync(Test entity)
    {
        await Context.Tests.Entry(entity)
            .Collection(test => test.TestAutos).Query()
            .Include(auto => auto.Auto)
            .ThenInclude(auto => auto.Details)
            .ThenInclude(detail => detail.DetailSeries)
            .Include(auto => auto.Auto)
            .ThenInclude(auto => auto.CarModel).LoadAsync();
    }

    private Test? Find(int id)
    {
        return Context.Tests
            .Include(test => test.TestAutos)
            .ThenInclude(testAuto => testAuto.Auto)
            .ThenInclude(auto => auto.CarModel)
            .Include(test => test.TestAutos)
            .ThenInclude(testAuto => testAuto.Auto)
            .ThenInclude(auto => auto.Details)
            .ThenInclude(detail => detail.DetailSeries)
            .FirstOrDefault(test => test.Id == id);
    }
}