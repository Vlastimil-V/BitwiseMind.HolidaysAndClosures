#!/bin/bash

# Set current directory to script's directory
cd "$(dirname "$0")"

# Delete all BIN and OBJ folders
echo "Deleting all BIN and OBJ folders..."
find . -type d \( -name bin -o -name obj \) -exec rm -rf {} +
echo "BIN and OBJ folders successfully deleted"

# Docker builder prune
docker builder prune -af

# Stop and remove containers, networks
docker-compose -f docker-compose.yaml down

# List & remove one or more containers
for container_id in $(docker ps -a -qf "network=bitwisemind-network"); do
  docker rm "$container_id"
done

# Create and start container
docker-compose -f docker-compose.yaml build --no-cache
docker-compose -f docker-compose.yaml up -d --force-recreate

# Pause for user input to exit
read -n 1 -s -r -p $'\e[1;32mPress any key to continue...\e[0m'