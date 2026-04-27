#!/bin/sh

echo "Deleting all bin folders . . ."

if ! rm -rf **/bin/ 2>/dev/null; then
  echo " ❌ Deleting bin folders failed."
  exit 1
fi

echo "Deleting all obj folders . . ."

if ! rm -rf **/obj/ 2>/dev/null; then
  echo " ❌ Deleting obj folders failed."
  exit 2
fi

echo "✅ All folders deleted successfully."
