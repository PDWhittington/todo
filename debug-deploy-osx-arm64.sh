#!/usr/bin/env sh

set -euo pipefail

SCRIPT_DIR="$(dirname "$(readlink -f "$0")")"

$SCRIPT_DIR/deploy.sh 'osx-arm64' 'Debug' '/usr/local/bin/todo/'
