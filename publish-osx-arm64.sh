#!/usr/bin/env sh

SOLUTION=$(find src -name "*.sln" -type f | head -n 1)

if [ -z "$SOLUTION" ]; then
  echo " ❌ No .sln file found in src/ directory"
  exit 1
fi

echo "Found solution: $SOLUTION"

# Step 1: Run tests in Release mode (this builds what it needs for testing)
echo "🧪 Running tests in Release mode..."
if ! dotnet test "$SOLUTION" \
  -c Release \
  --no-restore; then
  echo " ❌ TESTS FAILED"
  echo " Check the error messages above."
  exit 4
fi

echo " ✅ All tests passed. Proceeding to publish..."

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
  -p:PublishReadyToRun=true \
  -p:OptimizationPreference=Speed \
  -p:TieredCompilation=false \
  --self-contained; then
  echo " ❌ BUILD/PUBLISH FAILED"
  echo " Check the error messages above."
  exit 1
fi

echo ""
echo " ✅ Build succeeded. Proceeding to copy..."

# Clean previous deployment
if ! sudo rm -R -f /usr/local/bin/todo; then
  echo " ❌ FAILED TO DELETE /usr/local/bin/todo"
  exit 2
fi

# Copy the published output
if ! sudo cp -R src/todo/bin/Release/net10.0/osx-arm64/publish /usr/local/bin/todo/; then
  echo " ❌ FAILED TO COPY NEW FILES TO /usr/local/bin/todo"
  exit 3
fi

echo " ✅ Deployment completed successfully!"
