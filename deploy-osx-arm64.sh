#!/usr/bin/env sh

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[1]}" )" && pwd )"

$SCRIPT_DIR/deploy.sh 'osx-arm64' '/usr/local/bin/todo/'
