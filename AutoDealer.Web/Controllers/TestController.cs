namespace AutoDealer.Web.Controllers;

public class TestController : MvcController
{
    public TestController(ApiClient client) : base(client)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Table()
    {
        var tests = await Client.GetAsync<Test[]>("tests");
        return View(tests.Value ?? Array.Empty<Test>());
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var test = await Client.PostAsync<string, Test>("tests/start", string.Empty);
        
        var autos = await Client.GetAsync<Auto[]>($"autos?filter={AutoStatus.Assembled}");
        ViewBag.AssembledAutos = autos.Value ?? Array.Empty<Auto>();
        return View("Info", test.Value!);
    }

    [HttpGet]
    public async Task<IActionResult> Info(int id)
    {
        var test = await Client.GetAsync<Test>($"tests/{id}");
        if (test.Value is null) return RedirectToAction("Table", "Test");
        
        var autos = await Client.GetAsync<Auto[]>($"autos?filter={AutoStatus.Assembled}");
        ViewBag.AssembledAutos = autos.Value ?? Array.Empty<Auto>();
        return View(test.Value);
    }

    [HttpPost]
    public async Task<IActionResult> SetAutos(int id, int[] onTest)
    {
        var test = await Client.PutAsync<int[], Test>($"tests/{id}/autos/set", onTest);
        if (test.Details is null) return RedirectToAction("Info", "Test");
        
        ModelState.AddModelError("", test.Details);
        var autos = await Client.GetAsync<Auto[]>($"autos?filter={AutoStatus.Assembled}");
        ViewBag.AssembledAutos = autos.Value ?? Array.Empty<Auto>();
        return View("Info", test.Value);
    }

    [HttpGet("Certify/{testId:int}-{autoId:int}-{status}")]
    public async Task<IActionResult> Certify(int testId, int autoId, TestStatus status)
    {
        var test = await Client.PatchAsync<TestStatus, Test>($"tests/{testId}/autos/{autoId}/certify", status);
        return RedirectToAction("Info", "Test", new { id = testId });
    }

    [HttpGet]
    public async Task<IActionResult> Finish(int id)
    {
        var finished = await Client.PostAsync<string, Test>($"tests/{id}/finish", string.Empty);
        return RedirectToAction("Table", "Test");
    }
}