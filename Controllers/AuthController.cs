using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration configuration;

    private readonly IHttpClientFactory httpClientFactory;

    public AuthController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        this.configuration = configuration;
        this.httpClientFactory = httpClientFactory;
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login()
    {
        var clientId = configuration["GitHub:ClientId"];
        var redirectUri = "https://localhost:5048/api/auth/callback";

        var url = "https://github.com/login/oauth/authorize" +
        $"?client_id={clientId}" +
        $"&redirect_uri={redirectUri}" +
        $"&scope=user";
        return Redirect(url);
    }

    [HttpGet("callback")]

    public async Task<IActionResult> Callback2([FromQuery] string code)
    {
        var clientId = configuration["GitHub:ClientId"];
        var clientSecret = configuration["GitHub:ClientSecret"];

        var client = httpClientFactory.CreateClient();

        var response = await client.PostAsync(
        "https://github.com/login/oauth/access_token",
        new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["code"] = code
        })
        );

        var body = await response.Content.ReadAsStringAsync();

        var token = body
        .Split('&')
        .First(x => x.StartsWith("access_token="))
        .Replace("access_token=", "");

        HttpContext.Session.SetString("GitHubToken", token);

        return Ok("Usuario autenticado correctamente");
    }
}