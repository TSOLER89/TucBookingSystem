# TucBookingSystem Tests

Detta projekt innehåller omfattande tester för TucBookingSystem API.

## 📋 Test Categories

### Unit Tests
- **BookingServiceTests.cs** - Service layer logic för bokningar
- **RoomServiceTests.cs** - Service layer logic för rum
- **AuthServiceTests.cs** - Autentisering och användarhantering
- **EmailServiceTests.cs** - Email-funktionalitet

### Integration Tests

#### Controller Tests (WebApplicationFactory)
- **AuthControllerIntegrationTests.cs** - Registrering, login, lösenordsåterställning
- **BookingsControllerIntegrationTests.cs** - CRUD operations för bokningar
- **RoomsControllerIntegrationTests.cs** - CRUD operations för rum

#### Repository Tests (InMemory Database)
- **BookingRepositoryIntegrationTests.cs** - Databasoperationer för bokningar
- **RoomRepositoryIntegrationTests.cs** - Databasoperationer för rum
- **UserRepositoryIntegrationTests.cs** - Databasoperationer för användare

## 🛠️ Tech Stack

- **xUnit** - Test framework
- **FluentAssertions** - Readable assertions
- **Moq** - Mocking framework
- **Microsoft.AspNetCore.Mvc.Testing** - Integration testing
- **Microsoft.EntityFrameworkCore.InMemory** - In-memory database för tester

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
- ✅ Controllers (AuthController, BookingsController, RoomsController)
- ✅ Repositories (BookingRepository, RoomRepository, UserRepository)

## 🎯 Test Patterns

### Unit Tests
- Använder Moq för att mocka dependencies
- Testar en komponent i isolation
- Snabba och fokuserade

### Integration Tests
- Använder WebApplicationFactory för controller tests
- Använder InMemory database för repository tests
- Testar komponenter tillsammans
- Verifierar att system fungerar end-to-end

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

### Integration Test
```csharp
[Fact]
public async Task GetAll_ShouldReturnOk_AndListOfRooms()
{
    // Act
    var response = await _client.GetAsync("/api/rooms");
    
    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
}
```

## 🔍 CI/CD

Testerna körs automatiskt vid:
- Push till main
- Pull requests
- Innan merge

## 👨‍💻 Författare

TUC Booking System Team
