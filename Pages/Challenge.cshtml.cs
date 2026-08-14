using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QRCoder;
using Twilio.Rest.Verify.V2.Service.Entity;

namespace TwilioVerifyTotp.Pages;

public class ChallengeModel : PageModel
{
    private readonly string _verifyServiceSid =
        Environment.GetEnvironmentVariable("TWILIO_VERIFY_SERVICE_SID")!;

    public string QrCode { get; private set; } = string.Empty;
    public string Seed { get; private set; } = string.Empty;

    public void OnGet()
    {
        var otpUri = HttpContext.Session.GetString("otp_uri") ?? string.Empty;
        Seed = HttpContext.Session.GetString("seed") ?? string.Empty;
        QrCode = BuildQrCodeDataUri(otpUri);
    }

    public IActionResult OnPost(string? code)
    {
        var seed = HttpContext.Session.GetString("seed") ?? string.Empty;
        var sid = HttpContext.Session.GetString("sid") ?? string.Empty;

        var factor = FactorResource.Update(
            pathServiceSid: _verifyServiceSid,
            pathIdentity: seed,
            pathSid: sid,
            authPayload: code ?? string.Empty);

        if (factor.Status == FactorResource.FactorStatusesEnum.Verified)
        {
            TempData["message"] = "Factor setup complete!";
            return Redirect("/token");
        }

        return Redirect("/challenge");
    }

    private static string BuildQrCodeDataUri(string payload)
    {
        if (string.IsNullOrEmpty(payload))
        {
            return string.Empty;
        }

        using var generator = new QRCodeGenerator();
        using var data = generator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.M);
        var png = new PngByteQRCode(data).GetGraphic(10);
        return "data:image/png;base64," + Convert.ToBase64String(png);
    }
}
