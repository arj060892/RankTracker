@echo off
setlocal enabledelayedexpansion

set SQL_PORT=51533
set API_PORT=7148
set UI_PORT=51420

start "" dotnet run --project "RankTracker.Api\RankTracker.Api.csproj" ^
    --launch-profile "https" ^
    --ConnectionStrings:DefaultConnection "Server=localhost,%SQL_PORT%;Database=RankTrackerDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"
timeout /t 15 /nobreak >nul

start "" "http://localhost:%UI_PORT%"