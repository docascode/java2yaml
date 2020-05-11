:DownloadDoclet
SET DocletLocation=%~dp0src\java2yaml\Tools
IF NOT EXIST "%DocletLocation%\docfx-doclet.jar" (
IF NOT EXIST "%DocletLocation%" MD "%DocletLocation%"
ECHO Download docfx-doclet to "%DocletLocation%" 
powershell -NoProfile -ExecutionPolicy UnRestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://github.com/docascode/docfx-doclet/releases/latest/download/docfx-doclet-1.0-SNAPSHOT-jar-with-dependencies.jar' -OutFile '%DocletLocation%\docfx-doclet.jar'"
)
