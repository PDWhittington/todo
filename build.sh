#!/usr/bin/env sh

SOLUTION=$(find src -name "*.sln" -type f | head -n 1)

if [ -z "$SOLUTION" ]; then
  echo " ❌ No .sln file found in src/ directory"
  exit 1
fi

echo "Found solution: $SOLUTION"

# Step 2: Publish once with maximum runtime speed optimizations
# -c Release
# -r osx-arm64 (your exact target)
# --self-contained
# -p:PublishReadyToRun=true → AOT compilation for better startup + reduced JIT
# -p:OptimizationPreference=Speed → tells the compiler to favor execution speed over size
# -p:TieredCompilation=false → disables tiered JIT (forces more aggressive optimizations; good for "run fast" scenarios)
# -p:PublishTrimmed=false → we don't care about size
# -p:PublishReadyToRun=true \

if ! dotnet publish src/todo/ \
  -c Release \
  -r osx-arm64 \
  --self-contained true; then
  echo " ❌ BUILD/PUBLISH FAILED"
  echo " Check the error messages above."
  exit 1
fi

# if ! dotnet publish src/todo/ \
#   -c Release \
#   -r osx-arm64 \
#   --self-contained true \
#   -p:PublishAot=true \
#   -p:PublishReadyToRun=true \
#   -p:OptimizationPreference=Speed \
#   -p:TieredCompilation=false; then
#   echo " ❌ BUILD/PUBLISH FAILED"
#   echo " Check the error messages above."
#   exit 1
# fi

echo ""
echo " ✅ Build succeeded. Proceeding to copy..."
