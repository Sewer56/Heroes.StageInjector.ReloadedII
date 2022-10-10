# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/sonicheroes.utils.stageinjector/*" -Force -Recurse
dotnet publish "./sonicheroes.utils.stageinjector.csproj" -c Release -o "$env:RELOADEDIIMODS/sonicheroes.utils.stageinjector" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location