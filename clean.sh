#!/bin/sh

echo "Deleting all bin folders . . ."

if ! find . -type d -name bin -exec rm -rf {} +; then
  echo " ❌ Deleting bin folders failed."
  exit 1
fi

echo "Deleting all obj folders . . ."

if ! find . -type d -name obj -exec rm -rf {} +; then
  echo " ❌ Deleting obj folders failed."
  exit 2
fi

echo "✅ All folders deleted successfully."

find ./ -type d -name "bin"
find ./ -type d -name "obj"
