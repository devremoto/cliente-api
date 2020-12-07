@echo off
if not defined in_subprocess (cmd /k set in_subprocess=y ^& %0 %*) & exit )

taskkill /FI "windowtitle eq BAT*"
taskkill /IM devenv.exe /F
taskkill /IM chrome.exe /F
taskkill /IM code.exe /F

taskkill /FI "windowtitle eq ng*"

taskkill /FI "windowtitle eq npm*"
taskkill /FI "windowtitle eq API*"
taskkill /FI "windowtitle eq FRONT*"

where /q node
IF ERRORLEVEL 1 (
	ECHO Nodejs não instalado, redirecionando para página de download
	start https://nodejs.org
	EXIT /B
)

where /q ng
IF ERRORLEVEL 1 (
	npm i @angular/cli -g
	EXIT /B
)

where /q code
IF ERRORLEVEL 1 (
	ECHO Visual Studio code não instalado, redirecionando para página de download
	start https://code.visualstudio.com/
	start cmd /c "title=FRONT && cd ../backend/Web && ng serve -o --port=4200 --hmr"
) ELSE (
	start cmd /c "title=FRONT && code . && ng serve -o --port=4200 --hmr"
)

start cmd /c "title=API && cd ../backend/Api && dotnet build && dotnet watch run --urls=https://localhost:5001;http://localhost:5000"

start cmd /c "cd ../ && start file-explorer.sln exit

exit

