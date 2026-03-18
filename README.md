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

Läs [SECRETS_SETUP.md](SECRETS_SETUP.md) för mer information.

---

## 🚀 Funktioner

- ✅ **Användarhantering** - Registrering, login, lösenordsåterställning
- ✅ **Rumsbokning** - Boka grupprum med tidsvalidering
- ✅ **Konflikthantering** - Automatisk detektering av dubbelbokningar
- ✅ **Admin-panel** - Hantera rum och användare
- ✅ **Email-notifikationer** - För lösenordsåterställning
- ✅ **JWT Authentication** - Säker autentisering

## 📋 Tech Stack

### Backend (API)
- **.NET 10** - Modern C# 14
- **ASP.NET Core Web API** - RESTful API
- **Entity Framework Core** - ORM
- **SQLite** - Databas (development)
- **JWT** - Authentication
- **BCrypt** - Password hashing

### Frontend (Client)
- **Blazor Server** - Interactive web UI
- **Bootstrap** - Responsive design
- **Razor Components** - Component-based architecture

### Testing
- **xUnit** - Test framework
- **FluentAssertions** - Readable test assertions
- **Moq** - Mocking framework
- **InMemory Database** - Repository integration tests

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
# Följ instruktioner i SECRETS_SETUP.md
dotnet user-secrets init --project TucBookingSystem.Api
dotnet user-secrets set "Jwt:Key" "DIN_HEMLIGA_NYCKEL_32_TECKEN" --project TucBookingSystem.Api
# ... osv
```

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

**Test Coverage:** 33 tester (100% pass rate)
- Unit Tests: 22
- Repository Integration Tests: 11

## 📁 Projektstruktur

```
TucBookingSystem/
├── TucBookingSystem.Api/          # Backend API
│   ├── Controllers/                # API endpoints
│   ├── Services/                   # Business logic
│   ├── Repositories/               # Data access
│   ├── Models/                     # Database entities
│   └── Data/                       # DbContext
├── TucBookingSystem.Client/        # Blazor frontend
│   ├── Pages/                      # Razor pages
│   ├── Components/                 # Reusable components
│   ├── Services/                   # Client services
│   └── Layout/                     # Layout components
├── TucBookingSystem.Shared/        # Shared DTOs
│   └── DTOs/                       # Data transfer objects
└── TucBookingSystem.Tests/         # Test project
    ├── Unit Tests/                 # Service tests
    └── Integration Tests/          # Repository tests
```

## 🔐 Säkerhet

- JWT tokens för autentisering
- BCrypt password hashing
- CORS-konfiguration
- User Secrets för känslig data
- Role-based authorization

## 📚 API Endpoints

### Authentication
- `POST /api/auth/register` - Registrera ny användare
- `POST /api/auth/login` - Logga in
- `POST /api/auth/request-password-reset` - Begär lösenordsåterställning
- `POST /api/auth/reset-password` - Återställ lösenord

### Bookings (Kräver auth)
- `GET /api/bookings` - Hämta alla bokningar
- `GET /api/bookings/{id}` - Hämta specifik bokning
- `GET /api/bookings/user/{userId}` - Hämta användares bokningar
- `POST /api/bookings` - Skapa ny bokning
- `DELETE /api/bookings/{id}` - Radera bokning

### Rooms
- `GET /api/rooms` - Hämta alla rum
- `GET /api/rooms/{id}` - Hämta specifikt rum
- `POST /api/rooms` - Skapa nytt rum (Admin)
- `DELETE /api/rooms/{id}` - Radera rum (Admin)

## 🤝 Bidra

1. Forka projektet
2. Skapa en feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit dina ändringar (`git commit -m 'Add some AmazingFeature'`)
4. Push till branchen (`git push origin feature/AmazingFeature`)
5. Öppna en Pull Request

## 📝 License

Detta projekt är utvecklat för TUC (Teknikcollege).

## 👥 Team

TUC Booking System Development Team

## 📮 Support

För frågor eller problem, öppna ett issue på GitHub.

---

**Version:** 1.0.0  
**Last Updated:** 2026-01-19
