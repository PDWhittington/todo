# This is provided more as an example than anything else

dotnet publish src -c Release

sudo rm -R /usr/local/bin/todo

sudo cp -R src/todo/bin/Release/net6.0/publish /usr/local/bin/todo/

