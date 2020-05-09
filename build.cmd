@ECHO OFF
PUSHD %~dp0

SETLOCAL
SETLOCAL ENABLEDELAYEDEXPANSION

ECHO Checking Dotnet CLI version...
dotnet --version
IF NOT '%ErrorLevel%' == '0' (
    ECHO Error: build.cmd requires Dotnet CLI. Please follow https://www.microsoft.com/net/core to install .NET Core.
    SET ERRORLEVEL=1
    GOTO :Exit
)

:EnvSet
SET BuildProj=%~dp0Java2Yaml.sln
SET Configuration=%1
IF '%Configuration%'=='' (
    SET Configuration=Release
)
SET CachedNuget=%LocalAppData%\NuGet\NuGet.exe

:: nuget wrapper requires nuget.exe path in %PATH%
SET PATH=%PATH%;%LocalAppData%\NuGet

:: Build
dotnet build %BuildProj%
SET BuildErrorLevel=%ERRORLEVEL%
GOTO :Exit

:Exit
POPD
ECHO.
EXIT /B %ERRORLEVEL%