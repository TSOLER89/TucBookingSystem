# 🔐 Secrets Setup Guide

## ⚠️ VIKTIGT: Secrets har tagits bort från appsettings.json

Känsliga nycklar och lösenord finns inte längre i Git av säkerhetsskäl.

## ⚡ SNABB START (För att köra igång direkt)

**Efter att du klonat/pullat projektet:**

1. **Gå till `TucBookingSystem.Api` mappen**
2. **Kopiera template-filen:**
   ```bash
   copy appsettings.Development.json.template appsettings.Development.json
   ```
   (På Mac/Linux: `cp appsettings.Development.json.template appsettings.Development.json`)

3. **Kör database migrations:**
   ```bash
   dotnet ef database update
   ```

4. **Starta projektet!** (F5 i Visual Studio)

**Nu fungerar login/registrering!** ✅

---

## 🛠️ Setup för Development

### Metod 1: Kopiera Template (Snabbast)

Använd den medföljande template-filen:
```bash
cd TucBookingSystem.Api
copy appsettings.Development.json.template appsettings.Development.json
```

Denna fil innehåller redan fungerande JWT-nyckel för development!

### Metod 2: User Secrets (Mer säkert)

1. Öppna terminalen i `TucBookingSystem.Api` mappen
2. Kör:
```bash
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "DettaArEnSuperHemligNyckelSomArMinst32Tecken"
dotnet user-secrets set "Email:FromAddress" "din-email@gmail.com"
dotnet user-secrets set "Email:Username" "din-email@gmail.com"
dotnet user-secrets set "Email:Password" "ditt-gmail-app-password"
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
