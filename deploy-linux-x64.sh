#!/usr/bin/env sh

set -eu
# pipefail is not portable to older dash
(set -o pipefail) 2>/dev/null && set -o pipefail

SCRIPT_DIR="$(dirname "$(readlink -f "$0")")"

$SCRIPT_DIR/deploy.sh 'linux-x64' 'Release' '/usr/local/bin/todo/'
