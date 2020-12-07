@echo off
if not defined in_subprocess (cmd /k set in_subprocess=y ^& %0 %*) & exit )

taskkill /FI "windowtitle eq BAT*"
taskkill /IM chrome.exe /F
taskkill /FI "windowtitle eq ng*"
taskkill /FI "windowtitle eq npm*"
taskkill /FI "windowtitle eq API*"
taskkill /FI "windowtitle eq FRONT*"
taskkill /FI "windowtitle eq Windows*"

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
	start cmd /c "title=FRONT && cd src/Web && npm i && ng serve -o --port=4200 --hmr"
) ELSE (
	taskkill /IM code.exe /F
	start cmd /c "title=FRONT && cd src/Web && npm i && code . && ng serve -o --port=4200 --hmr"
)

start cmd /c "title=API && cd src/Api && dotnet build && dotnet watch run --urls=https://localhost:5001;http://localhost:5000"

where /q devenv
IF ERRORLEVEL 1 (
 	ECHO Visual Studio não instalado
	start https://visualstudio.microsoft.com/pt-br/thank-you-downloading-visual-studio/?sku=Community&rel=15#
) ELSE (
	taskkill /IM devenv.exe /F
	start cmd /c "cd src && start cliente.sln exit"
)


exit

