#!/usr/bin/env sh

ARCHITECTURE=$1
PUBLISH_LOCATION=$2

# Validate architecture

if [[ ! "$ARCHITECTURE" =~ ^(osx-arm64|win-x64|linux-x64)$ ]]; then
    echo "Value '$ARCHITECTURE' is not a valid architecture or not an architecture in the set tested."
    exit 1
fi

# Assume a single solution file in the src sub-folder

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[1]}" )" && pwd )"
SOLUTION=$(find "$SCRIPT_DIR/src" -name "*.sln" -type f | head -n 1)

if [ -z "$SOLUTION" ]; then
  echo " ❌ No .sln file found in src/ directory."
  exit 2
fi

echo "Found solution: $SOLUTION"

# Step 1: Run tests in Release mode (this builds what it needs for testing)

echo "🧪 Running tests in Release mode..."
if ! dotnet test "$SOLUTION" \
  -c Release \
  --no-restore; then
  echo " ❌ TESTS FAILED"
  echo " Check the error messages above."
  exit 3
fi

echo " ✅ All tests passed. Proceeding to publish..."

# Step 2: Publish once with maximum runtime speed optimizations

if ! dotnet publish $SCRIPT_DIR/src/todo/ \
  -c Release \
  -r $ARCHITECTURE \
  -p:PublishReadyToRun=true \
  -p:OptimizationPreference=Speed \
  -p:TieredCompilation=false \
  --self-contained; then

    echo " ❌ BUILD/PUBLISH FAILED."
    echo " Check the error messages above."
    exit 4
fi

echo ""
echo " ✅ Build succeeded. Proceeding to copy..."

# Clean previous deployment
if ! sudo rm -R -f $PUBLISH_LOCATION; then
  echo " ❌ FAILED TO DELETE /usr/local/bin/todo."
  exit 5
fi

echo " ✅ Any old version in $PUBLISH_LOCATION has been deleted."

# Copy the published output
if ! sudo cp -R $SCRIPT_DIR/src/todo/bin/Release/net10.0/$ARCHITECTURE/publish $PUBLISH_LOCATION; then
  echo " ❌ FAILED TO COPY NEW FILES TO /usr/local/bin/todo."
  exit 6
fi

echo " ✅ Copied new version to $PUBLISH_LOCATION."
echo " ✅ Deployment completed successfully!"
