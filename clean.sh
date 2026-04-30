#!/usr/bin/env sh

echo "Deleting all bin folders . . ."

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[1]}" )" && pwd )"

if ! find $SCRIPT_DIR -type d -name bin -exec rm -rf {} +; then
  echo " ❌ Deleting bin folders failed."
  exit 1
fi

echo "Deleting all obj folders . . ."

if ! find $SCRIPT_DIR -type d -name obj -exec rm -rf {} +; then
  echo " ❌ Deleting obj folders failed."
  exit 2
fi

echo "✅ All folders deleted successfully."

find $SCRIPT_DIR -type d -name "bin"
find $SCRIPT_DIR -type d -name "obj"
