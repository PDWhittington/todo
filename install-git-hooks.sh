#!/bin/sh
# scripts/install-hooks.sh

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[1]}" )" && pwd )"

echo "Installing git hooks..."

# Copy all hooks from .githooks to .git/hooks
cp $SCRIPT_DIR/.githooks/* $SCRIPT_DIR/.git/hooks/ 2>/dev/null || true

# Make them executable
chmod +x $SCRIPT_DIR/.git/hooks/*

echo "✅ Git hooks installed successfully!"
echo "   (pre-push will now run tests before pushing)"
