# TucBookingSystem Tests

Detta projekt innehåller omfattande tester för TucBookingSystem API.

## 📋 Test Categories

### Unit Tests
- **BookingServiceTests.cs** - Service layer logic för bokningar (14 tester)
- **RoomServiceTests.cs** - Service layer logic för rum (4 tester)
- **AuthServiceTests.cs** - Autentisering och användarhantering (4 tester)
- **AuthStateProviderTests.cs** - Authentication state provider (5 tester)

### Repository Integration Tests (InMemory Database)
- **BookingRepositoryIntegrationTests.cs** - Databasoperationer för bokningar (7 tester)
- **UserRepositoryIntegrationTests.cs** - Databasoperationer för användare (5 tester)

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
- ✅ Services (BookingService, RoomService, AuthService)
- ✅ Authentication (AuthStateProvider)
- ✅ Repositories (BookingRepository, UserRepository)

**Totalt:** 47 tester med 100% pass rate ✅

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

## 📈 Test Statistics

- **Totalt:** 47 tester (100% passing ✅)

**Unit Tests:** ~28 tester
- BookingServiceTests: 14 tester
- RoomServiceTests: 4 tester
- AuthServiceTests: 4 tester
- AuthStateProviderTests: 5 tester

**Integration Tests:** ~19 tester
- BookingRepositoryIntegrationTests: 7 tester
- UserRepositoryIntegrationTests: 5 tester

**Pass Rate:** 100% (47/47 tester passing) 🎉

## 👨‍💻 Författare

TUC Booking System Team
