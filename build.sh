#!/usr/bin/env sh

set -euo pipefail

# Get the absolute directory of the script itself (works even when called via ../script.sh, symlinks, etc.)
ARCHITECTURE=$1
PUBLISH_LOCATION=$2

# Validate architecture

if [[ ! "$ARCHITECTURE" =~ ^(osx-arm64|win-x64|linux-x64)$ ]]; then
    echo "Value '$ARCHITECTURE' is not a valid architecture or not an architecture in the set tested."
    exit 1
fi

# Assume a single solution file in the src sub-folder

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[1]}" )" && pwd )"
SOLUTION=$(find "$SCRIPT_DIR/src" -name "*.slnx" -type f | head -n 1)

if [ -z "$SOLUTION" ]; then
  echo " ❌ No .slnx file found in src/ directory"
  exit 2
fi

echo "Found solution: $SOLUTION"

if ! dotnet publish $SOLUTION \
  -c Release \
  -r $ARCHITECTURE \
  -p:PublishReadyToRun=true \
  -p:OptimizationPreference=Speed \
  -p:TieredCompilation=false \
  --self-contained; then

    echo " ❌ BUILD FAILED."
    echo " Check the error messages above."
    exit 3
fi

echo ""
echo " ✅ Build succeeded."
