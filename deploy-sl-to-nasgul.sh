#!/bin/bash
set -e  # Exit on error

# ===== Copy files =====
# Define paths
SL_FOLDER="$(realpath "$(dirname "$0")/src/ShoppingList")"

echo "SL Folder $SL_FOLDER"

TARGET_FOLDER="/mnt/shoppinglist"

# Mirror directory, excluding bin and obj folders
rsync -av --delete \
      --exclude 'bin/' \
      --exclude 'obj/' \
      "$SL_FOLDER/" "$TARGET_FOLDER/"


# ===== Build and run Docker image on remote host =====
DOCKER_PATH="/share/CACHEDEV1_DATA/.qpkg/container-station/bin/docker"

# Commands to execute remotely on nasgul
REMOTE_CMDS=$(cat <<'EOF'
/share/CACHEDEV1_DATA/.qpkg/container-station/bin/docker rm -f shoppinglist || true
sleep 3
cd /share/David/ShoppingList
/share/CACHEDEV1_DATA/.qpkg/container-station/bin/docker build --no-cache -t shoppinglist:latest .
/share/CACHEDEV1_DATA/.qpkg/container-station/bin/docker run --restart always -d -p 8000:8080 --name shoppinglist -v /share/David/ShoppingListData:/data shoppinglist
EOF
)

# Run remote commands via SSH
ssh -o HostKeyAlgorithms=+ssh-rsa \
    -o PubkeyAcceptedAlgorithms=+ssh-rsa \
    -o MACs=hmac-sha2-256 \
    david@192.168.0.205 "$REMOTE_CMDS"
