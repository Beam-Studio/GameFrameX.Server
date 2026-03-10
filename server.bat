@echo off
setlocal

set SERVER_DIR=%~dp0bin\app_debug
set SERVER_DLL=%SERVER_DIR%\GameFrameX.Launcher.dll
set SERVER_TYPE=Game
set SERVER_ID=9000
set INNER_PORT=29100
set OUTER_PORT=29010
set HTTP_PORT=28080
set WS_PORT=29110
set APM_PORT=29090
set MIN_MODULE=1
set MAX_MODULE=9999
set DB_URL=mongodb://admin:admin@localhost:27017/?authSource=admin
set DB_NAME=gameframex

if "%1"=="" goto usage
if "%1"=="start" goto start
if "%1"=="stop" goto stop
if "%1"=="restart" goto restart
if "%1"=="build" goto build
if "%1"=="status" goto status
goto usage

:start
echo Starting GameFrameX Server...
start "GameFrameX" /D "%SERVER_DIR%" dotnet %SERVER_DLL% ^
  --ServerType=%SERVER_TYPE% --ServerId=%SERVER_ID% ^
  --InnerPort=%INNER_PORT% --OuterPort=%OUTER_PORT% ^
  --HttpPort=%HTTP_PORT% --WsPort=%WS_PORT% ^
  --APMPort=%APM_PORT% ^
  --MinModuleId=%MIN_MODULE% --MaxModuleId=%MAX_MODULE% ^
  --DataBaseUrl="%DB_URL%" --DataBaseName=%DB_NAME%
echo Server started.
goto end

:stop
echo Stopping GameFrameX Server...
for /f "tokens=2" %%a in ('tasklist /fi "WINDOWTITLE eq GameFrameX" /fo list ^| findstr "PID:"') do (
    taskkill /pid %%a /f >nul 2>&1
)
REM Also kill by matching command line
for /f "tokens=2 delims=," %%a in ('tasklist /fo csv /nh ^| findstr /i "dotnet"') do (
    taskkill /pid %%a /f >nul 2>&1
)
echo Server stopped.
goto end

:restart
call :stop
timeout /t 2 /nobreak >nul
call :start
goto end

:build
echo Building GameFrameX Server...
dotnet build
if %errorlevel% neq 0 (
    echo Build failed!
    goto end
)
echo Build succeeded.
goto end

:status
echo Checking server status...
tasklist /fo csv /nh 2>nul | findstr /i "dotnet" >nul
if %errorlevel%==0 (
    echo Server is RUNNING.
    tasklist /fi "IMAGENAME eq dotnet.exe" 2>nul
) else (
    echo Server is STOPPED.
)
goto end

:usage
echo.
echo Usage: server.bat [command]
echo.
echo Commands:
echo   start     Start the game server
echo   stop      Stop the game server
echo   restart   Restart the game server
echo   build     Build the project
echo   status    Check if server is running
echo.

:end
endlocal
