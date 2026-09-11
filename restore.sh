#!/usr/bin/env sh

set -eu
# pipefail is not portable to older dash
(set -o pipefail) 2>/dev/null && set -o pipefail

if ! dotnet restore ./src/; then
	echo " ❌ RESTORE FAILED"
	echo " Check the error messages above."
	exit 1
fi

echo ""
echo " ✅ Build succeeded. Proceeding to copy..."
