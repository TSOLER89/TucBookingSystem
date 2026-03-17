# E-postkonfiguration för Glömt Lösenord

Systemet använder **MailKit** för att skicka e-postmeddelanden när användare glömmer sina lösenord.

## Konfiguration

### Alternativ 1: Gmail (Rekommenderat för testning)

1. **Aktivera 2-stegverifiering** på ditt Gmail-konto
2. **Skapa ett App-lösenord:**
   - Gå till: https://myaccount.google.com/apppasswords
   - Välj "Mail" och "Windows Computer"
   - Kopiera det genererade lösenordet (16 tecken)

3. **Uppdatera `appsettings.json`:**
```json
"Email": {
  "SmtpServer": "smtp.gmail.com",
  "SmtpPort": "587",
  "FromName": "TUC Booking System",
  "FromAddress": "din-email@gmail.com",
  "Username": "din-email@gmail.com",
  "Password": "ditt-app-lösenord-här"
}
```

### Alternativ 2: SendGrid

1. Skapa ett konto på https://sendgrid.com
2. Skapa en API-nyckel
3. Uppdatera EmailService.cs för att använda SendGrid API

### Alternativ 3: Outlook/Hotmail

```json
"Email": {
  "SmtpServer": "smtp-mail.outlook.com",
  "SmtpPort": "587",
  "FromName": "TUC Booking System",
  "FromAddress": "din-email@outlook.com",
  "Username": "din-email@outlook.com",
  "Password": "ditt-lösenord"
}
```

### Alternativ 4: Annan SMTP-server

Kontakta din e-postleverantör för SMTP-inställningar.

## Säkerhet

**VIKTIGT:** Spara ALDRIG riktiga lösenord i `appsettings.json` i produktion!

### För produktion:

1. **Använd User Secrets för lokal utveckling:**
```bash
cd TucBookingSystem.Api
dotnet user-secrets init
dotnet user-secrets set "Email:Username" "din-email@gmail.com"
dotnet user-secrets set "Email:Password" "ditt-app-lösenord"
```

2. **Använd Environment Variables eller Azure Key Vault i produktion**

## Testning

1. Konfigurera e-postinställningarna enligt ovan
2. Starta applikationen
3. Gå till /forgot-password
4. Ange en e-postadress som finns i systemet
5. Kontrollera din inbox för återställningslänken

## Felsökning

### E-post skickas inte

- Kontrollera att SMTP-inställningarna är korrekta
- Kontrollera att lösenordet är rätt (för Gmail, använd App-lösenord)
- Kolla loggen i Visual Studio Output-fönstret
- Försäkra dig om att port 587 inte är blockerad av brandvägg

### Gmail: "Less secure app access"

Gmail kräver numera 2-stegverifiering och App-lösenord. Vanliga lösenord fungerar inte längre.

## Funktioner

- ✅ HTML och textformat för e-post
- ✅ 30 minuters utgångstid för återställningslänkar
- ✅ Säker hantering (samma meddelande oavsett om e-post finns eller inte)
- ✅ Error logging
- ✅ Token som endast kan användas en gång

