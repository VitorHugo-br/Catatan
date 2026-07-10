using Microsoft.Extensions.Caching.Memory;
using MyBlazorApp.Components;
using MyBlazorApp.Extensions;
using MyBlazorApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<TokenProvider>();

builder.Services.AddHttpClientConfig();

builder.Services.AdicionarMudServices();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AdicionarServicos();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.MapGet("/login-complete", (string code, IMemoryCache cache, HttpContext context) =>
{
    if (!cache.TryGetValue(code, out string? jwt) || string.IsNullOrEmpty(jwt))
        return Results.Redirect("/?erro=sessao-expirada");

    cache.Remove(code); 

    context.Response.Cookies.Append("jwt_token", jwt, new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = DateTimeOffset.UtcNow.AddDays(7),
        Path = "/"
    });

    return Results.Redirect("/home");
});

app.MapGet("/api/logout", (HttpContext context) =>
{
    context.Response.Cookies.Delete("jwt_token", new CookieOptions { Path = "/" });
    return Results.Redirect("/");
});

app.Run();
