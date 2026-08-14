using DotNetEnv;
using Twilio;

Env.TraversePath().Load();

foreach (var name in new[] { "TWILIO_ACCOUNT_SID", "TWILIO_AUTH_TOKEN", "TWILIO_VERIFY_SERVICE_SID" })
{
    if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(name)))
    {
        throw new InvalidOperationException($"Required environment variable '{name}' is not set. Copy .env.example to .env and fill it in.");
    }
}

TwilioClient.Init(
    Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID"),
    Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN"));

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
    options.Conventions.ConfigureFilter(new Microsoft.AspNetCore.Mvc.IgnoreAntiforgeryTokenAttribute());
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "app_session";
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.MapRazorPages();

app.Run();
