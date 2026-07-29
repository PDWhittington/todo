#!/usr/bin/env sh

set -euo pipefail

# This is provided more as an example than anything else

SCRIPT_DIR="$(dirname "$(readlink -f "$0")")"

$SCRIPT_DIR/deploy.sh 'win-x64' 'Release' '/mnt/c/portable/todo'
