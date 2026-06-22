#!/usr/bin/env sh

ARCHITECTURE=$1
CONFIGURATION=$2
DEPLOY_LOCATION=$3

# Validate architecture

if [[ ! "$ARCHITECTURE" =~ ^(osx-arm64|win-x64|linux-x64)$ ]]; then
    echo "Value '$ARCHITECTURE' is not a valid architecture or not an architecture in the set tested."
    exit 1
fi

# Validate configuration

if [[ ! "$CONFIGURATION" =~ ^(Debug|Release)$ ]]; then
    echo "Value '$CONFIGURATION' is not a valid configuration. Must be Debug or Release."
    exit 1
fi

# Assume a single solution file in the src sub-folder


SCRIPT_DIR="$(dirname "$(readlink -f "$0")")"
SOLUTION=$(find "$SCRIPT_DIR/src" -name "*.sln" -type f | head -n 1)

if [ -z "$SOLUTION" ]; then
  echo " ❌ No .sln file found in src/ directory."
  exit 2
fi

echo "Found solution: $SOLUTION"

# Run tests (using the specified configuration)

echo "🧪 Running tests in $CONFIGURATION mode..."
if ! dotnet test "$SOLUTION" \
  -c $CONFIGURATION \
  --no-restore; then
  echo " ❌ TESTS FAILED"
  echo " Check the error messages above."
  exit 3
fi

echo " ✅ All tests passed. Proceeding to publish..."

# Publish (Release gets extra runtime speed optimizations; Debug does not)


EXTRA_PUBLISH_FLAGS=""

if [ "$CONFIGURATION" = "Release" ]; then
  EXTRA_PUBLISH_FLAGS="-p:PublishReadyToRun=true -p:OptimizationPreference=Speed -p:TieredCompilation=false --self-contained"
fi

if ! dotnet publish $SCRIPT_DIR/src/todo/ \
  -c $CONFIGURATION \
  -r $ARCHITECTURE \
  $EXTRA_PUBLISH_FLAGS; then

    echo " ❌ BUILD/PUBLISH FAILED."
    echo " Check the error messages above."
    exit 4
fi

echo ""
echo " ✅ Build succeeded. Proceeding to copy..."

# Clean previous deployment
if ! sudo rm -R -f $DEPLOY_LOCATION; then
  echo " ❌ FAILED TO DELETE /usr/local/bin/todo."
  exit 5
fi

echo " ✅ Any old version in $DEPLOY_LOCATION has been deleted."

# Copy the published output
if ! sudo cp -R $SCRIPT_DIR/src/todo/bin/$CONFIGURATION/net10.0/$ARCHITECTURE/publish $DEPLOY_LOCATION; then
  echo " ❌ FAILED TO COPY NEW FILES TO /usr/local/bin/todo."
  exit 6
fi

echo " ✅ Copied new version to $DEPLOY_LOCATION."
echo " ✅ Deployment completed successfully!"
