#!/usr/bin/env sh

#!/usr/bin/env bash
# Replace these with your repo
OWNER="PDWhittington"
REPO="todo"

URL="https://github.com/${OWNER}/${REPO}/archive/refs/heads/main.zip"

echo "Downloading latest main branch as ZIP..."
curl -L -o "${REPO}-main.zip" "$URL" # -L follows the redirect GitHub issues

echo "Unzipping..."
unzip -q "${REPO}-main.zip"

echo "Done! Files are in ./${REPO}-main/"
ls -l "${REPO}-main/"
