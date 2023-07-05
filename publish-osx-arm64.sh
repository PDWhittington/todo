#!/usr/bin/env sh
# This is provided more as an example than anything else

dotnet publish src/todo/ -c Release -r osx-arm64 -p:PublishReadyToRun=true --self-contained

sudo rm -R -f /usr/local/bin/todo

sudo cp -R src/todo/bin/Release/net7.0/osx-arm64/publish /usr/local/bin/todo/

