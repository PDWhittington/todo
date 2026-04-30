#!/usr/bin/env sh
# This is provided more as an example than anything else

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[1]}" )" && pwd )"

$SCRIPT_DIR/deploy.sh 'win-x64' '/mnt/c/portable/todo'
