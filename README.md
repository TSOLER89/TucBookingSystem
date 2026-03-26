# 🏢 TUC Booking System

Ett modernt bokningssystem för grupprum och resurser byggt med .NET 10 och Blazor.

## ⚡ QUICK START (EFTER CLONE/PULL)

**HAR DU PRECIS KLONAT/PULLAT PROJEKTET?**

Kör detta i PowerShell från projektets root-mapp:
```powershell
.\setup.ps1
```

**ELLER manuellt:**
```bash
cd TucBookingSystem.Api
copy appsettings.Development.json.template appsettings.Development.json
dotnet ef database update
```

**Sedan tryck F5 i Visual Studio och allt fungerar!** ✅

---

## 🚀 Funktioner

### Core Features
- ✅ **Användarhantering** - Registrering, login, lösenordsåterställning
- ✅ **Rumsbokning** - Boka grupprum med tidsvalidering (08:00-20:00)
- ✅ **Konflikthantering** - Automatisk detektering av dubbelbokningar
- ✅ **Helgvalidering** - Blockerar bokningar på lördagar och söndagar
- ✅ **Visa upptagna tider** - Se bokningar innan du försöker boka
- ✅ **Admin-panel** - Hantera rum och användare
- ✅ **Email-notifikationer** - För lösenordsåterställning
- ✅ **JWT Authentication** - Säker autentisering med Bearer tokens
- ✅ **Global Error Handling** - Strukturerad felhantering med logging

## 📋 Tech Stack

### Backend (API)
- **.NET 10** - Modern C# 14
- **ASP.NET Core Web API** - RESTful API med Swagger dokumentation
- **Entity Framework Core** - ORM med SQLite provider
- **SQLite** - Databas (development)
- **JWT Bearer** - Token-based authentication
- **BCrypt (PasswordHasher)** - Säker lösenordshashning
- **MailKit** - SMTP email service
- **ILogger** - Strukturerad logging i alla services
- **Global Exception Handler** - Centraliserad felhantering

### Frontend (Client)
- **Blazor Server** - Interactive server-side rendering
- **Bootstrap 5** - Responsive design
- **Razor Components** - Component-based architecture
- **SignalR** - Real-time kommunikation (inbyggt i Blazor Server)
- **ProtectedSessionStorage** - Säker state management

### Testing
- **xUnit** - Test framework
- **FluentAssertions** - Readable test assertions
- **Moq** - Mocking framework för unit tests
- **InMemory Database** - Repository integration tests
- **40 tester totalt** (38 passing, 2 under development)

## 🛠️ Setup

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Visual Studio 2026 eller VS Code
- Git

### Installation

1. **Klona repot**
```bash
git clone https://github.com/TSOLER89/TucBookingSystem.git
cd TucBookingSystem
```

2. **Setup secrets** (VIKTIGT!)
```bash
cd TucBookingSystem.Api
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "DettaArEnSuperHemligNyckelSomArMinst32Tecken12345!"
dotnet user-secrets set "Jwt:Issuer" "TucBookingSystem"
dotnet user-secrets set "Jwt:Audience" "TucBookingSystemUsers"
```

**OBS:** För e-postfunktionalitet (glömt lösenord), se [Email-konfiguration](#-email-konfiguration-valfritt) nedan.

3. **Kör migrations** (om nödvändigt)
```bash
cd TucBookingSystem.Api
dotnet ef database update
```

4. **Starta API**
```bash
dotnet run --project TucBookingSystem.Api
# API körs på: https://localhost:7002
```

5. **Starta Client** (nytt terminal-fönster)
```bash
dotnet run --project TucBookingSystem.Client
# Client körs på: https://localhost:7001
```

## 🧪 Tester

Kör alla tester:
```bash
dotnet test
```

Kör med coverage:
```bash
dotnet test --collect:"XPlat Code Coverage"
```

Kör specifika tester:
```bash
dotnet test --filter "FullyQualifiedName~BookingServiceTests"
```

**Test Coverage:** 47 tester (100% pass rate ✅)
- **Unit Tests:** 27 tester
  - BookingServiceTests (14 tester) - inklusive helgvalidering
  - RoomServiceTests (4 tester)
  - AuthServiceTests (4 tester)
  - AuthStateProviderTests (5 tester)
- **Repository Integration Tests:** 20 tester
  - BookingRepositoryIntegrationTests (7 tester)
  - UserRepositoryIntegrationTests (5 tester)

**Testade funktioner:**
- ✅ Bokningsvalidering (tid, datum, konflikt, helger)
- ✅ Användarautentisering och registrering
- ✅ Repository CRUD-operationer
- ✅ Relation-laddning från databas
- ✅ Authorization och ägarskap

## 📁 Projektstruktur

```
TucBookingSystem/
├── TucBookingSystem.Api/              # Backend API (.NET 10)
│   ├── Controllers/                    # API endpoints (Auth, Bookings, Rooms)
│   ├── Services/                       # Business logic med ILogger
│   │   ├── AuthService.cs             # JWT token generation
│   │   ├── BookingService.cs          # Bokningslogik + validering
│   │   ├── RoomService.cs             # Rumshantering
│   │   └── EmailService.cs            # SMTP email via MailKit
│   ├── Repositories/                   # Data access layer
│   │   ├── BookingRepository.cs       # Inklusive konflikt-check
│   │   ├── RoomRepository.cs
│   │   └── UserRepository.cs
│   ├── Models/                         # Database entities
│   │   ├── User.cs
│   │   ├── Room.cs
│   │   ├── Booking.cs
│   │   └── PasswordResetToken.cs
│   ├── Data/
│   │   └── ApplicationDbContext.cs    # EF Core DbContext med seed data
│   ├── Middleware/
│   │   └── GlobalExceptionHandler.cs  # Global error handling
│   ├── Migrations/                     # EF Core migrations
│   └── appsettings.Development.json.template  # Template för secrets
├── TucBookingSystem.Client/            # Blazor Server Frontend
│   ├── Pages/                          # Razor pages (11 sidor)
│   │   ├── Home.razor
│   │   ├── Login.razor
│   │   ├── Register.razor
│   │   ├── Bookings.razor
│   │   ├── Rooms.razor
│   │   ├── Admin.razor
│   │   ├── MinProfil.razor
│   │   ├── ForgotPassword.razor
│   │   └── ResetPassword.razor
│   ├── Components/Shared/              # Återanvändbara komponenter
│   │   ├── BookingForm.razor          # Visar upptagna tider
│   │   └── BookingList.razor
│   ├── Services/                       # Client services för API-anrop
│   │   ├── AuthService.cs
│   │   ├── BookingService.cs
│   │   ├── RoomService.cs
│   │   └── UserStateService.cs
│   └── Layout/                         # Layout components
│       ├── MainLayout.razor
│       └── NavMenu.razor
├── TucBookingSystem.Shared/            # Shared DTOs mellan API och Client
│   └── DTOs/                           # Data transfer objects
│       ├── BookingDto.cs
│       ├── CreateBookingDto.cs
│       ├── RoomDto.cs
│       ├── UserDto.cs
│       └── LoginRequestDto.cs
├── TucBookingSystem.Tests/             # Test project (xUnit)
│   ├── BookingServiceTests.cs          # 14 unit tests
│   ├── RoomServiceTests.cs             # 4 unit tests
│   ├── AuthServiceTests.cs             # 4 unit tests  
│   ├── BookingRepositoryIntegrationTests.cs
│   ├── RoomRepositoryIntegrationTests.cs
│   └── UserRepositoryIntegrationTests.cs
├── setup.ps1                           # Automated setup script
└── README.md                           # Detta dokument
```

## 🔐 Säkerhet

### Säkerhetsåtgärder
- **JWT Bearer Tokens** - Token-based authentication med 24h livslängd
- **BCrypt Password Hashing** - Använder ASP.NET Identity PasswordHasher
- **Role-based Authorization** - Admin och User roller
- **CORS-konfiguration** - Endast tillåtna origins
- **User Secrets** - Känslig data aldrig i Git
- **Global Exception Handler** - Centraliserad felhantering utan att läcka känslig information
- **.gitignore** - Databaser och secrets-filer exkluderas
- **Email Validation** - Unique email constraint i databas

### 📧 Email-konfiguration (Valfritt)

För att aktivera "Glömt lösenord"-funktionen behöver du konfigurera email.

#### Gmail (Rekommenderat för utveckling):

1. **Aktivera 2-stegverifiering** på ditt Gmail-konto
2. **Skapa ett App-lösenord:**
   - Gå till: https://myaccount.google.com/apppasswords
   - Välj "Mail" och "Windows Computer"
   - Kopiera det genererade lösenordet (16 tecken)

3. **Lägg till i User Secrets:**
   ```bash
   cd TucBookingSystem.Api
   dotnet user-secrets set "Email:SmtpServer" "smtp.gmail.com"
   dotnet user-secrets set "Email:SmtpPort" "587"
   dotnet user-secrets set "Email:FromName" "TUC Booking System"
   dotnet user-secrets set "Email:FromAddress" "din-email@gmail.com"
   dotnet user-secrets set "Email:Username" "din-email@gmail.com"
   dotnet user-secrets set "Email:Password" "xxxx xxxx xxxx xxxx"
   ```

#### Outlook/Hotmail:
```bash
dotnet user-secrets set "Email:SmtpServer" "smtp-mail.outlook.com"
dotnet user-secrets set "Email:SmtpPort" "587"
dotnet user-secrets set "Email:FromAddress" "din-email@outlook.com"
dotnet user-secrets set "Email:Username" "din-email@outlook.com"
dotnet user-secrets set "Email:Password" "ditt-lösenord"
```

#### Testning:
- E-postfunktionaliteten är valfri för grundläggande användning
- Systemet fungerar utan email-konfiguration, men "Glömt lösenord" kommer inte fungera
- Felsökning: Kontrollera loggen i Visual Studio Output-fönstret om email inte skickas

## 📚 API Endpoints

### 🔓 Public Endpoints (ingen auth krävs)

#### Authentication
- `POST /api/auth/register` - Registrera ny användare
- `POST /api/auth/login` - Logga in och få JWT token
- `POST /api/auth/forgot-password` - Begär lösenordsåterställning
- `POST /api/auth/reset-password-api` - Återställ lösenord med token

#### Rooms
- `GET /api/rooms` - Hämta alla aktiva rum
- `GET /api/rooms/{id}` - Hämta specifikt rum

#### Bookings (delvis public)
- `GET /api/bookings/room/{roomId}/date/{date}` - Se upptagna tider för ett rum (ingen auth krävs)

### 🔒 Protected Endpoints (kräver JWT Bearer token)

#### Bookings
- `GET /api/bookings/my` - Hämta mina bokningar
- `POST /api/bookings` - Skapa ny bokning
  - Validerar: tid (08:00-20:00), datum (ej förflutet), helger (blockeras), konflikter
- `DELETE /api/bookings/{id}` - Ta bort egen bokning

#### Admin Endpoints (kräver Admin-roll)
- `GET /api/bookings` - Hämta alla bokningar
- `POST /api/rooms` - Skapa nytt rum
- `DELETE /api/rooms/{id}` - Ta bort rum

### 📖 Swagger UI

Testa API:et interaktivt via Swagger:

1. Starta API-projektet: `dotnet run --project TucBookingSystem.Api`
2. Öppna: `https://localhost:7002/swagger`
3. För autentiserade endpoints:
   - Logga in via `/api/auth/login` och kopiera JWT token
   - Klicka "Authorize" och skriv: `Bearer {din-token}`
   - Nu kan du testa alla skyddade endpoints!

## 🤝 Bidra

1. Forka projektet
2. Skapa en feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit dina ändringar (`git commit -m 'Add some AmazingFeature'`)
4. Push till branchen (`git push origin feature/AmazingFeature`)
5. Öppna en Pull Request

## 👥 Team

**TUC Booking System Development Team:**

- **[Tsoler Hayitian](https://github.com/TSOLER89)** - Project Lead & Backend Developer
- **[Jenny Khranovska](https://github.com/jennykhranovska)** - Frontend & Auth Developer
- **[Aygen](https://github.com/AigennA)** - Backend Developer
- **[Marachinos](https://github.com/Marachinos)** - Frontend Developer
- **[Marika Romeo](https://github.com/MarikaRomeo)** - Full Stack Developer


### 🎓 Om Projektet
Detta projekt utvecklades som en del av utbildningen på **TUC (Teknikcollege)** under 2026.

## 📮 Support

För frågor eller problem, öppna ett issue på GitHub.

## 📝 License

Detta projekt är utvecklat för **TUC (Teknikcollege)** som ett utbildningsprojekt.

---

**Version:** 2.0.0  
**Last Updated:** 2026-03-26  
**Utvecklat av TUC Booking System Team** ❤️

