#!/usr/bin/env sh

set -eu
# pipefail is not portable to older dash
(set -o pipefail) 2>/dev/null && set -o pipefail

# This is provided more as an example than anything else

SCRIPT_DIR="$(dirname "$(readlink -f "$0")")"

$SCRIPT_DIR/deploy.sh 'win-x64' 'Debug' '/c/portable/todo'
