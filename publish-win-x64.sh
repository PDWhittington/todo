#!/usr/bin/env sh
# This is provided more as an example than anything else

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[1]}" )" && pwd )"

$SCRIPT_DIR/publish.sh 'win-x64' '/c/portable/todo'
