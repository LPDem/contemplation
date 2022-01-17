@echo off
rmdir /s /q Release
dotnet publish "Contemplation.csproj" -v:minimal -c:Release -o:Release
del Release\appsettings.Development.json