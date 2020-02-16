@echo off

pushd %~dp0

md artifacts
call dotnet --info
call dotnet restore src
if %errorlevel% neq 0 exit /b %errorlevel%
call dotnet test src/Standart.Hash.xxHash.Test
if %errorlevel% neq 0 exit /b %errorlevel%

echo Packing Standart.Hash.xxHash
call dotnet pack src/Standart.Hash.xxHash -c Release -o .\artifacts

popd