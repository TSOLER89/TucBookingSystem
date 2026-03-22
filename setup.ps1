# 🚀 TUC Booking System - Quick Setup Script
# Kör detta efter att du klonat/pullat projektet

Write-Host "🔧 Setting up TUC Booking System..." -ForegroundColor Cyan
Write-Host ""

# Navigera till Api-projektet
$apiPath = "TucBookingSystem.Api"

if (-not (Test-Path $apiPath)) {
    Write-Host "❌ Kan inte hitta TucBookingSystem.Api mappen!" -ForegroundColor Red
    Write-Host "   Se till att du kör scriptet från projektets root-mapp." -ForegroundColor Yellow
    exit 1
}

Set-Location $apiPath

# Kopiera template-filen
Write-Host "📋 Kopierar appsettings template..." -ForegroundColor Yellow
$templateFile = "appsettings.Development.json.template"
$targetFile = "appsettings.Development.json"

if (-not (Test-Path $templateFile)) {
    Write-Host "❌ Template-filen hittades inte!" -ForegroundColor Red
    exit 1
}

Copy-Item $templateFile $targetFile -Force
Write-Host "✅ appsettings.Development.json skapad!" -ForegroundColor Green
Write-Host ""

# Kör database migrations
Write-Host "🗄️  Kör database migrations..." -ForegroundColor Yellow
dotnet ef database update

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Databasen är klar!" -ForegroundColor Green
} else {
    Write-Host "⚠️  Database migration misslyckades. Kör manuellt:" -ForegroundColor Yellow
    Write-Host "   dotnet ef database update" -ForegroundColor Gray
}

Write-Host ""
Write-Host "🎉 Setup klar!" -ForegroundColor Green
Write-Host ""
Write-Host "📌 Nästa steg:" -ForegroundColor Cyan
Write-Host "   1. Starta projektet med F5 i Visual Studio" -ForegroundColor White
Write-Host "   2. Registrera en användare" -ForegroundColor White
Write-Host "   3. Logga in och börja boka rum!" -ForegroundColor White
Write-Host ""
Write-Host "💡 Tips: Läs SECRETS_SETUP.md för mer information om konfiguration" -ForegroundColor Gray
Write-Host ""

Set-Location ..
