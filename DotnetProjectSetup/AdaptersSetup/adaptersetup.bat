@echo off

set /A counter=0

for /f "tokens=*" %%a in (Settings.txt) do call :processline %%a

goto :start
:processline
set settings[%counter%]=%*
set /A counter+=1
break
goto :eof
:eof

:start
set adapterType=%settings[0]%
set adapterNameWithoutExtension=%settings[1]%
set adapterName=%adapterNameWithoutExtension%.dll
set platform=%settings[2]%
set configuration=%settings[3]%

set adapterBinPath=%MiracleSoftware%\Bin\Adapters\%adapterType%.Adapters\%adapterNameWithoutExtension%\%platform%\%configuration%\netcoreapp3.1

mkdir %adapterBinPath%\Setup
mkdir %adapterBinPath%\Setup\Dependencies
mkdir %adapterBinPath%\Setup\Library
mkdir %adapterBinPath%\Setup\Settings

copy %adapterBinPath%\%adapterName% %adapterBinPath%\Setup\Library
cls

set settingsJsonName=%adapterName%.json
set settingsJsonPath=%adapterBinPath%\Setup\Settings\%settingsJsonName%

echo { >> %settingsJsonPath%
echo   "name":"%adapterName%",>> %settingsJsonPath%
echo   "dependencies":[ >> %settingsJsonPath%
echo   ] >> %settingsJsonPath%
echo } >> %settingsJsonPath%
cls
echo Process completed!
echo If the adapter haves dependencies, you can add the %settingsJsonName% file!
pause
start %adapterBinPath%\Setup
exit