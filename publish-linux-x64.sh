#!/usr/bin/env sh
# This is provided more as an example than anything else

if ! dotnet publish src/todo/ \
    -c Release \
    -r linux-x64 \
    -p:PublishReadyToRun=true \
    --self-contained; then

  echo " ❌ BUILD FAILED"
  echo "   Check the error messages above for the exact reason (compilation error,"
  echo "   missing package, wrong target framework, etc.)."
  echo ""
  exit 1 # non-zero exit code = script failed
fi

echo ""
echo " ✅ Build succeeded. Proceeding to copy..."

if ! sudo rm -R -f /usr/local/bin/todo; then

  echo " ❌FAILED TO DELETE /usr/local/bin/todo"
  echo ""
  exit 2 # non-zero exit code = script failed
fi

if ! sudo sudo cp -R src/todo/bin/Release/net10.0/linux-x64/publish /usr/local/bin/todo/; then

  echo " ❌FAILED TO COPY NEW FILES TO /usr/local/bin/todo"
  echo ""
  exit 3 # non-zero exit code = script failed
fi

echo " ✅ Deployment completed successfully!"
