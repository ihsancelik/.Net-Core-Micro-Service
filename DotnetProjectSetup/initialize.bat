@echo off

set git_base=https://github.com/advanced-information-technologies

set git_library=%git_base%/Libraries.git

set git_apicore=%git_base%/Miracle.Core.Api.git
set git_apicore_database=%git_base%/Miracle.Core.Api.Database.git
set git_apicore_models=%git_base%/Miracle.Core.Api.Models.git
set git_apicore_services=%git_base%/Miracle.Core.Api.Services.git
set git_apicore_adapters=%git_base%/Miracle.Core.Api.Adapters.git

set git_api=%git_base%/Miracle.Api.git

set basePath=%MiracleSoftware%
set appPath=%basePath%\App
set databasePath=%basePath%\Database
set adapterPath=%basePath%\Adapter
set libraryPath=%basePath%\Library
set servicePath=%basePath%\Service
set modelPath=%basePath%\Model

mkdir %basePath%
mkdir %appPath%
mkdir %databasePath%
mkdir %adapterPath%
mkdir %libraryPath%
mkdir %modelPath%
mkdir %servicePath%

git clone %git_library% %libraryPath%

git clone %git_apicore% %appPath%\Miracle.Core.Api
git clone %git_apicore_database% %databasePath%\Miracle.Core.Api.Database
git clone %git_apicore_models% %modelPath%\Miracle.Core.Api.Models
git clone %git_apicore_services% %servicePath%\Miracle.Core.Api.Services
git clone %git_apicore_adapters% %adapterPath%\Miracle.Core.Api.Adapters

git clone %git_api% %appPath%\Miracle.Api

REM build.bat File
set buildFileName=%basePath%\build.bat
echo @echo off > %buildFileName%
echo set Configuration=%%1>> %buildFileName%
echo set Platform=%%2>> %buildFileName%

echo set libraryRoutesPath=%libraryPath%\Library.Routes >> %buildFileName%
echo set libraryHelpersPath=%libraryPath%\Library.Helpers >> %buildFileName%
echo set libraryDependencyPath=%libraryPath%\Library.Dependency >> %buildFileName%
echo set libraryResponsesPath=%libraryPath%\Library.Responses >> %buildFileName%

echo set modelsApiCorePath=%modelPath%\Miracle.Core.Api.Models >> %buildFileName%
echo set databaseApiCorePath=%databasePath%\Miracle.Core.Api.Database >> %buildFileName%
echo set servicesApiCorePath=%servicePath%\Miracle.Core.Api.Services >> %buildFileName%

echo set target_libraries=%basePath%\Bin\Libraries\%%Platform%%\%%Configuration%%\netcoreapp3.1 >> %buildFileName%
echo set target_adapters=%basePath%\Bin\Adapters\%%Platform%%\%%Configuration%%\netcoreapp3.1 >> %buildFileName%
echo set target_databases=%basePath%\Bin\Databases\%%Platform%%\%%Configuration%%\netcoreapp3.1 >> %buildFileName%
echo set target_models=%basePath%\Bin\Models\%%Platform%%\%%Configuration%%\netcoreapp3.1 >> %buildFileName%
echo set target_services=%basePath%\Bin\Services\%%Platform%%\%%Configuration%%\netcoreapp3.1 >> %buildFileName%

echo dotnet build %%libraryRoutesPath%% -c %%Configuration%% -r %%Platform%% -o %%target_libraries%% >> %buildFileName%
echo dotnet build %%libraryHelpersPath%% -c %%Configuration%% -r %%Platform%% -o %%target_libraries%% >> %buildFileName%
echo dotnet build %%libraryDependencyPath%% -c %%Configuration%% -r %%Platform%% -o %%target_libraries%% >> %buildFileName%
echo dotnet build %%libraryResponsesPath%% -c %%Configuration%% -r %%Platform%% -o %%target_libraries%% >> %buildFileName%

echo dotnet build %%modelsApiCorePath%% -c %%Configuration%% -r %%Platform%% -o %%target_models%% >> %buildFileName%
echo dotnet build %%databaseApiCorePath%% -c %%Configuration%% -r %%Platform%% -o %%target_databases%% >> %buildFileName%
echo dotnet build %%servicesApiCorePath%% -c %%Configuration%% -r %%Platform%% -o %%target_services%% >> %buildFileName%
REM END

call %basePath%\build.bat Debug x64

echo Process completed!
pause
exit