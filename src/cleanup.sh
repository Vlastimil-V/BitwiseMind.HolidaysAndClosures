#!/bin/bash

# Set current directory to script's directory
cd "$(dirname "$0")"

# Delete nuget cache
echo "Deleting nuget cache..."
dotnet nuget locals all --clear

dotnet restore

# Delete all BIN and OBJ folders
echo "Deleting all BIN and OBJ folders..."
find . -type d \( -name bin -o -name obj \) -exec rm -rf {} +
echo "BIN and OBJ folders successfully deleted. Close the window..."

# Pause for user input to exit
read -n 1 -s -r -p $'\e[1;32mPress any key to continue...\e[0m'