#!/usr/bin/env sh

SCRIPT_DIR="$(dirname "$(readlink -f "$0")")"

$SCRIPT_DIR/deploy-debug.sh 'osx-arm64' '/usr/local/bin/todo/'
