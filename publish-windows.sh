#!/usr/bin/env sh
# This is provided more as an example than anything else

dotnet publish src/todo/ -c Release -r win-x64 -p:PublishReadyToRun=true --self-contained

rm -R /c/portable/todo

cp -R src/todo/bin/Release/net7.0/win-x64/publish /c/portable/todo/

