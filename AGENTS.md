# TOTP-backed Passwordless Auth in C# Using Twilio Verify

An ASP.NET Core Razor Pages sample that walks a user through registering a TOTP factor with the Twilio Verify API, scanning a QR code in an authenticator app, and validating time-based one-time codes.

## Commands

```bash
# Install (restores NuGet packages)
dotnet restore

# Run
dotnet run
# Then open http://localhost:5000
```

## Environment Variables

Copy `.env.example` to `.env`. Never commit `.env`.

```bash
cp .env.example .env
```

| Variable | Where to find | Format |
| -------- | ------------- | ------ |
| `TWILIO_ACCOUNT_SID` | [Console](https://console.twilio.com) homepage | Starts with `AC` |
| `TWILIO_AUTH_TOKEN` | Console homepage → click to reveal | 32-char string. Treat as a password. |
| `TWILIO_VERIFY_SERVICE_SID` | Console → Verify → Services | Starts with `VA` |

## Project Structure

- `Program.cs` — loads `.env`, initializes the Twilio client, wires up sessions and Razor Pages
- `Pages/Index.*` — username form; creates a new TOTP factor on POST
- `Pages/Challenge.*` — renders the QR code and verifies the initial factor code
- `Pages/Token.*` — validates subsequent TOTP codes via a Verify challenge
- `wwwroot/css/styles.css` — styling

## Agent Boundaries

**Always:**
- Confirm `.env` is configured before running any command
- Use the Environment Variables section to guide the user to each credential — don't ask them to find values without direction
- Confirm the app is running before asking the user to test it

**Never:**
- Run the app with missing or placeholder credentials
- Hardcode credentials or phone numbers in source files
- Skip the `cp .env.example .env` step

## Verify It's Working

1. Run `dotnet run` and open http://localhost:5000.
2. Enter a username and submit — you should be redirected to `/challenge` with a QR code.
3. Scan the QR code with Authy, Google Authenticator, or any TOTP app, then enter the 6-digit code and submit — you should land on `/token` with a "Factor setup complete!" flash message.
4. On `/token`, enter a fresh 6-digit code from the same authenticator app — you should see "Verification success."

## Twilio Resources

- [Twilio Console](https://console.twilio.com) — credentials, Verify Services, phone numbers
- [Twilio Verify API docs](https://www.twilio.com/docs/verify/api)
- [Verify TOTP quickstart](https://www.twilio.com/docs/verify/quickstarts/totp)
- [Twilio C# / .NET SDK reference](https://www.twilio.com/docs/libraries/reference/twilio-csharp/)
