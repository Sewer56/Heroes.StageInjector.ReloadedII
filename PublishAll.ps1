
# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

./Publish.ps1 -ProjectPath "sonicheroes.utils.stageinjector/sonicheroes.utils.stageinjector.csproj" `
              -PackageName "sonicheroes.utils.stageinjector" `

Pop-Location