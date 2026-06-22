#!/usr/bin/env sh

SCRIPT_DIR="$(dirname "$(readlink -f "$0")")"

$SCRIPT_DIR/deploy.sh 'linux-x64' 'Debug' '/usr/local/bin/todo/'
