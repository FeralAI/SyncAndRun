@ECHO ON
set VERSION=1.0.0
dotnet publish -c Release -r win-x64 -o ./.publish/SyncAndRun-%VERSION%_win-x64 -p:PublishSingleFile=true -p:PublishReadyToRun=true
dotnet publish -c Release -r linux-x64 -o ./.publish/SyncAndRun-%VERSION%_linux-x64 -p:PublishSingleFile=true
dotnet publish -c Release -r osx-x64 -o ./.publish/SyncAndRun-%VERSION%_osx-x64 -p:PublishSingleFile=true
