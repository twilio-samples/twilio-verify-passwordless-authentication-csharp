using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Twilio.Rest.Verify.V2.Service.Entity;

namespace TwilioVerifyTotp.Pages;

public class IndexModel : PageModel
{
    private readonly string _verifyServiceSid =
        Environment.GetEnvironmentVariable("TWILIO_VERIFY_SERVICE_SID")!;

    public void OnGet()
    {
    }

    public IActionResult OnPost(string? username)
    {
        var seed = Convert.ToHexString(RandomNumberGenerator.GetBytes(20))
            .ToLowerInvariant()
            .Substring(0, 32);

        HttpContext.Session.SetString("seed", seed);

        var factor = NewFactorResource.Create(
            pathServiceSid: _verifyServiceSid,
            pathIdentity: seed,
            friendlyName: username ?? string.Empty,
            factorType: NewFactorResource.FactorTypesEnum.Totp);

        HttpContext.Session.SetString("friendly_name", factor.FriendlyName ?? string.Empty);
        HttpContext.Session.SetString("sid", factor.Sid ?? string.Empty);
        HttpContext.Session.SetString("url", factor.Url?.ToString() ?? string.Empty);
        HttpContext.Session.SetString("otp_uri", ExtractOtpUri(factor.Binding));

        return Redirect(factor.Status == NewFactorResource.FactorStatusesEnum.Unverified
            ? "/challenge"
            : "/");
    }

    private static string ExtractOtpUri(object? binding)
    {
        if (binding is null) return string.Empty;
        var token = binding as JToken ?? JToken.FromObject(binding);
        return token["uri"]?.ToString() ?? string.Empty;
    }
}
