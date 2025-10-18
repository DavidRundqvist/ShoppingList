#!/bin/bash
# Mounts //nasgul/David/ShoppingList permanently on Kubuntu
# Run this script with sudo:  sudo ./mount-nasgul.sh

set -e

SHARE="//192.168.0.205/David/ShoppingList"
MOUNT_POINT="/mnt/shoppinglist"
CRED_FILE="/etc/samba/credentials-nasgul"
SMB_VERSION="3.0"

echo "=== Step 1: Installing CIFS utilities ==="
apt update -y
apt install -y cifs-utils

echo "=== Step 2: Creating mount point at $MOUNT_POINT ==="
mkdir -p "$MOUNT_POINT"

echo "=== Step 3: Creating credentials file at $CRED_FILE ==="
read -p "Enter NAS username: " NAS_USER
read -s -p "Enter NAS password: " NAS_PASS
echo
cat <<EOF > "$CRED_FILE"
username=$NAS_USER
password=$NAS_PASS
EOF
chmod 600 "$CRED_FILE"
echo "Credentials file created and secured."

echo "=== Step 4: Testing manual mount ==="
if mount -t cifs "$SHARE" "$MOUNT_POINT" -o "credentials=$CRED_FILE,iocharset=utf8,vers=$SMB_VERSION"; then
    echo "Mount successful! Files visible at $MOUNT_POINT"
else
    echo "❌ Mount failed. Check SMB version or network connection."
    exit 1
fi

echo "=== Step 5: Adding entry to /etc/fstab ==="
FSTAB_LINE="$SHARE $MOUNT_POINT cifs credentials=$CRED_FILE,iocharset=utf8,vers=$SMB_VERSION,nofail,_netdev 0 0"

# Only add if not already present
if ! grep -qs "$SHARE" /etc/fstab; then
    echo "$FSTAB_LINE" >> /etc/fstab
    echo "Added to /etc/fstab:"
    echo "$FSTAB_LINE"
else
    echo "Entry already exists in /etc/fstab — skipping."
fi

echo "=== Testing auto-mount ==="
umount "$MOUNT_POINT" || true
mount -a

if mountpoint -q "$MOUNT_POINT"; then
    echo "✅ Auto-mount verified successfully at $MOUNT_POINT"
else
    echo "⚠️  Auto-mount test failed — please check /etc/fstab syntax."
fi
