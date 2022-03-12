#!/usr/bin/env sh
# This is provided more as an example than anything else

dotnet publish src/todo/ -c Release -r linux-x64 -p:PublishReadyToRun=true --self-contained

sudo rm -R /usr/local/bin/todo

sudo cp -R src/todo/bin/Release/net6.0/linux-x64/publish /usr/local/bin/todo/

