# TucBookingSystem Tests

Detta projekt innehåller omfattande tester för TucBookingSystem API.

## 📋 Test Categories

### Unit Tests
- **BookingServiceTests.cs** - Service layer logic för bokningar
- **RoomServiceTests.cs** - Service layer logic för rum
- **AuthServiceTests.cs** - Autentisering och användarhantering
- **EmailServiceTests.cs** - Email-funktionalitet

### Repository Integration Tests (InMemory Database)
- **BookingRepositoryIntegrationTests.cs** - Databasoperationer för bokningar
- **RoomRepositoryIntegrationTests.cs** - Databasoperationer för rum
- **UserRepositoryIntegrationTests.cs** - Databasoperationer för användare

## 🛠️ Tech Stack

- **xUnit** - Test framework
- **FluentAssertions** - Readable assertions
- **Moq** - Mocking framework
- **Microsoft.EntityFrameworkCore.InMemory** - In-memory database för repository tester

## ▶️ Kör tester

### Alla tester
```bash
dotnet test
```

### Specifik testklass
```bash
dotnet test --filter "FullyQualifiedName~AuthServiceTests"
```

### Med code coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## 📊 Test Coverage

Nuvarande test coverage inkluderar:
- ✅ Services (BookingService, RoomService, AuthService, EmailService)
- ✅ Repositories (BookingRepository, RoomRepository, UserRepository)

## 🎯 Test Patterns

### Unit Tests
- Använder Moq för att mocka dependencies
- Testar en komponent i isolation
- Snabba och fokuserade

### Repository Integration Tests
- Använder InMemory database
- Testar databasoperationer
- Verifierar att repository fungerar korrekt mot databas

## 📝 Exempel

### Unit Test
```csharp
[Fact]
public async Task GetAllAsync_ShouldReturnAllRooms()
{
    // Arrange
    _roomRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(roomList);

    // Act
    var result = await _service.GetAllAsync();

    // Assert
    result.Should().HaveCount(2);
}
```

### Repository Integration Test
```csharp
[Fact]
public async Task CreateAsync_ShouldAddBooking()
{
    // Arrange
    var booking = new Booking { /* ... */ };

    // Act
    var result = await _repository.CreateAsync(booking);

    // Assert
    result.Id.Should().BeGreaterThan(0);
}
```

## 🔍 CI/CD

Testerna körs automatiskt vid:
- Push till main
- Pull requests
- Innan merge

## 👨‍💻 Författare

TUC Booking System Team
