using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Twilio.Rest.Verify.V2.Service.Entity;

namespace TwilioVerifyTotp.Pages;

public class TokenModel : PageModel
{
    private readonly string _verifyServiceSid =
        Environment.GetEnvironmentVariable("TWILIO_VERIFY_SERVICE_SID")!;

    public string FriendlyName { get; private set; } = string.Empty;
    public string Seed { get; private set; } = string.Empty;
    public string? Message { get; private set; }

    public void OnGet()
    {
        FriendlyName = HttpContext.Session.GetString("friendly_name") ?? string.Empty;
        Seed = HttpContext.Session.GetString("seed") ?? string.Empty;
        Message = TempData["message"] as string;
    }

    public IActionResult OnPost(string? code)
    {
        var seed = HttpContext.Session.GetString("seed") ?? string.Empty;
        var sid = HttpContext.Session.GetString("sid") ?? string.Empty;

        var challenge = ChallengeResource.Create(
            pathServiceSid: _verifyServiceSid,
            pathIdentity: seed,
            factorSid: sid,
            authPayload: code ?? string.Empty);

        TempData["message"] = challenge.Status == ChallengeResource.ChallengeStatusesEnum.Approved
            ? "Verification success."
            : "Verification failed.";

        return Redirect("/token");
    }
}
