#!/usr/bin/env sh

set -euo pipefail

if ! dotnet restore ./src/; then
	echo " ❌ RESTORE FAILED"
	echo " Check the error messages above."
	exit 1
fi

echo ""
echo " ✅ Build succeeded. Proceeding to copy..."
