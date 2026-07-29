#!/usr/bin/env sh

set -euo pipefail

SOLUTION=$(find src -name "*.slnx" -type f | head -n 1)

if [ -z "$SOLUTION" ]; then
	echo " ❌ No .slnx file found in src/ directory"
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

echo " ✅ All tests passed."
