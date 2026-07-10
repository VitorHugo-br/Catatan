namespace MyBlazorApp.Services;

public class TokenProvider
{
    public string? Jwt { get; set; }

    public bool IsAuthenticated => !string.IsNullOrEmpty(Jwt);
}
