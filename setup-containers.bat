@echo off
setlocal enabledelayedexpansion

set SQL_PORT=51533
set UI_PORT=51420

set DB_NAME=RankTrackerDb
set DB_PASSWORD=YourStrong@Passw0rd
set DB_IMAGE=mcr.microsoft.com/mssql/server:2022-latest
set PROJECT_ROOT=%~dp0
set DB_DEPLOY_PATH=%PROJECT_ROOT%RankTracker.DbDeploy

:: 1. Start SQL Server
echo [1/4] Starting SQL Server on port %SQL_PORT%...
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=%DB_PASSWORD%" -e "MSSQL_PID=Express" ^
    -p %SQL_PORT%:1433 --name ranktracker-sql -d %DB_IMAGE% || goto :error

:: 2. Initialize Database
echo [2/4] Initializing database...
timeout /t 20 /nobreak >nul
sqlcmd -S localhost,%SQL_PORT% -U sa -P %DB_PASSWORD% -d master -Q "CREATE DATABASE [%DB_NAME%]" || goto :error
sqlcmd -S localhost,%SQL_PORT% -U sa -P %DB_PASSWORD% -d %DB_NAME% -i "%DB_DEPLOY_PATH%\01_InitDB.sql" || goto :error
sqlcmd -S localhost,%SQL_PORT% -U sa -P %DB_PASSWORD% -d %DB_NAME% -i "%DB_DEPLOY_PATH%\02_SeedData.sql" || goto :error

:: 3. Build Angular Docker Image
echo [3/4] Building Angular Docker image...
docker build -t ranktracker-ui -f "%PROJECT_ROOT%RankTracker.UI\Dockerfile" "%PROJECT_ROOT%RankTracker.UI" || goto :error

:: Run Angular container
echo [4/4] Starting Angular on port %UI_PORT%...
docker run -d -p %UI_PORT%:4200 --name ranktracker-ui ranktracker-ui
timeout /t 20 /nobreak >nul

echo.
echo SETUP COMPLETE!
echo - SQL Server running on port %SQL_PORT%
echo - Angular image built as 'ranktracker-ui'
pause
exit /b 0

:error
echo ERROR: Operation failed at step above
pause
exit /b 1