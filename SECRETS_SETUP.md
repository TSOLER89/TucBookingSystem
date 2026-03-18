# 🔐 Secrets Setup Guide

## ⚠️ VIKTIGT: Secrets har tagits bort från appsettings.json

Känsliga nycklar och lösenord finns inte längre i Git av säkerhetsskäl.

## 🛠️ Setup för Development

### Metod 1: User Secrets (Rekommenderad)

1. Öppna terminalen i `TucBookingSystem.Api` mappen
2. Kör:
```bash
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "DIN_HEMLIGA_NYCKEL_MINST_32_TECKEN"
dotnet user-secrets set "Email:FromAddress" "din-email@gmail.com"
dotnet user-secrets set "Email:Username" "din-email@gmail.com"
dotnet user-secrets set "Email:Password" "ditt-gmail-app-password"
```

### Metod 2: Local appsettings (Enklare för test)

Skapa `TucBookingSystem.Api/appsettings.Development.json` (gitignored):
```json
{
  "Jwt": {
    "Key": "DIN_HEMLIGA_NYCKEL_MINST_32_TECKEN"
  },
  "Email": {
    "FromAddress": "din-email@gmail.com",
    "Username": "din-email@gmail.com",
    "Password": "ditt-gmail-app-password"
  }
}
```

## 📧 Gmail Setup

1. Gå till Google Account → Security
2. Aktivera 2-Factor Authentication
3. Gå till "App passwords"
4. Generera ett nytt app password för "Mail"
5. Använd det genererade lösenordet (16 tecken utan spaces)

## 🔑 JWT Key

Generera en säker nyckel (minst 32 tecken):
```bash
# PowerShell
-join ((65..90) + (97..122) + (48..57) | Get-Random -Count 32 | % {[char]$_})
```

## ✅ Verifiera

Kör projektet:
```bash
dotnet run --project TucBookingSystem.Api
```

Om du får fel om missing configuration, dubbelkolla att secrets är korrekt uppsatta.

## 📝 För Production

För Azure/Production, använd:
- Azure Key Vault
- Environment Variables
- Azure App Configuration

**PUBLICERA ALDRIG SECRETS I GIT!** 🔒
