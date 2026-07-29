#!/usr/bin/env sh

set -euo pipefail

# Returns: linux | darwin | windows | wsl | unknown
detect_os() {
  case "$(uname -s 2>/dev/null)" in
    Linux*)
      if grep -qiE 'microsoft|wsl' /proc/version 2>/dev/null; then
        echo "wsl"
      else
        echo "linux"
      fi
      ;;
    Darwin*) echo "darwin" ;;
    CYGWIN*|MINGW*|MSYS*) echo "windows" ;;   # Git Bash / MSYS2 / Cygwin
    *) echo "unknown" ;;
  esac
}

ARCHITECTURE='osx-arm64'
CONFIGURATION='Release'
DEPLOY_LOCATION='/usr/local/bin/todo/'

# Validate architecture

if [[ ! "$ARCHITECTURE" =~ ^(osx-arm64|win-x64|linux-x64)$ ]]; then
    echo "Value '$ARCHITECTURE' is not a valid architecture or not an architecture in the set tested."
    exit 1
fi

# Check we are on the right architecture

OS=$(detect_os)

echo "ARCHITECTURE=$ARCHITECTURE"
echo "OS=$OS"

if [[ "$ARCHITECTURE" == "win-x64" && "$OS" != "windows" ]]; then

  echo "You are trying to deploy for Windows, but this operating system is not Windows."
  exit 1
fi

if [[ "$ARCHITECTURE" == "osx-x64" && "$OS" != "darwin" ]]; then

  echo "You are trying to publish for MacOS, but this operating system is not MacOS"
  exit 1
fi

if [[ "$ARCHITECTURE" == "linux-x64" && "$OS" != "linux" ]]; then

  echo "You are trying to publish for Linux, but this operating system is not Linux (WSL is considered a different category)."
  exit 1
fi

# Validate configuration

if [[ ! "$CONFIGURATION" =~ ^(Debug|Release)$ ]]; then
    echo "Value '$CONFIGURATION' is not a valid configuration. Must be Debug or Release."
    exit 1
fi

# Assume a single solution file in the src sub-folder

SCRIPT_DIR="$(dirname "$(readlink -f "$0")")"
SOLUTION=$(find "$SCRIPT_DIR/src" -name "TodoGitTesting.slnx" -type f | head -n 1)

if [ -z "$SOLUTION" ]; then
  echo " ❌ No .slnx file found in src/ directory."
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

if ! dotnet publish $SCRIPT_DIR/src/todo/TodoReferencing.csproj \
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
