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

:: Download docfx-doclet
CALL :DownloadDoclet

:: Build
dotnet build %BuildProj%
SET BuildErrorLevel=%ERRORLEVEL%
GOTO :Exit

:DownloadDoclet
SET DocletLocation=%~dp0src\Microsoft.Content.Build.Java2Yaml.Steps\tools
IF NOT EXIST "%DocletLocation%\docfx-doclet.jar" (
IF NOT EXIST "%DocletLocation%" MD "%DocletLocation%"
ECHO Download docfx-doclet to "%DocletLocation%" 
powershell -NoProfile -ExecutionPolicy UnRestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://github.com/docascode/docfx-doclet/releases/latest/download/docfx-doclet-1.0-SNAPSHOT-jar-with-dependencies.jar' -OutFile '%DocletLocation%\docfx-doclet.jar'"
)

IF NOT EXIST "%DocletLocation%\docfx-doclet.jar" (
ECHO Cannot find docfx-doclet.jar)

:Exit
POPD
ECHO.
EXIT /B %ERRORLEVEL%