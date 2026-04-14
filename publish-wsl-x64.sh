#!/usr/bin/env sh
# This is provided more as an example than anything else

if ! dotnet publish src/todo/ \
  -c Release \
  -r win-x64 \
  -p:PublishReadyToRun=true \
  --self-contained; then

  echo " ❌ BUILD FAILED"
  echo "   Check the error messages above for the exact reason (compilation error,"
  echo "   missing package, wrong target framework, etc.)."
  echo ""
  exit 1 # non-zero exit code = script failed
fi

PUBLISH_SOURCE="src/todo/bin/Release/net10.0/win-x64/publish"
TARGET_DIR="/mnt/c/portable/todo"

echo ""
echo " ✅ Build succeeded. Proceeding to copy..."

if ! rm -rf "$TARGET_DIR"; then

  echo " ❌FAILED TO DELETE $TARGET_DIR"
  echo ""
  exit 2 # non-zero exit code = script failed
fi

if ! cp -R "$PUBLISH_SOURCE/" "$TARGET_DIR/"; then

  echo " ❌FAILED TO COPY NEW FILES TO $TARGET_DIR"
  echo ""
  exit 3 # non-zero exit code = script failed
fi

echo " ✅ Deployment completed successfully!"
