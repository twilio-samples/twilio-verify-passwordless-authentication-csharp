# TOTP-backed Passwordless Auth in C# Using Twilio Verify

C# port of the PHP sample in the parent folder. Implements the same three-step
TOTP setup flow against the Twilio Verify API:

1. `/` — enter a username, which creates a new TOTP factor on a random entity identity
2. `/challenge` — scan the QR code in an authenticator app and submit the generated code to verify the factor
3. `/token` — submit subsequent codes to create a challenge and confirm the factor still works

## Prerequisites

- .NET 9 SDK
- A Twilio account with a Verify Service. Get the Service SID from
  https://console.twilio.com/us1/develop/verify/services

## Setup

```
cp .env.example .env
# fill in TWILIO_ACCOUNT_SID, TWILIO_AUTH_TOKEN, TWILIO_VERIFY_SERVICE_SID
dotnet run
```

Then open http://localhost:5000 (the actual URL is printed at startup).

## Structure

- `Program.cs` — loads `.env`, initializes the Twilio client, wires up sessions and Razor Pages
- `Pages/Index.*` — username form and TOTP factor creation
- `Pages/Challenge.*` — QR code display and factor verification
- `Pages/Token.*` — ongoing challenge creation and verification
- `wwwroot/css/styles.css` — styling

The QR code is generated locally with QRCoder and inlined as a base64 PNG data URI.
