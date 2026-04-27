#!/bin/sh
# scripts/install-hooks.sh

echo "Installing git hooks..."

# Copy all hooks from .githooks to .git/hooks
cp .githooks/* .git/hooks/ 2>/dev/null || true

# Make them executable
chmod +x .git/hooks/*

echo "✅ Git hooks installed successfully!"
echo "   (pre-push will now run tests before pushing)"
