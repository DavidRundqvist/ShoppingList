# Copy files
$ngFolder = Resolve-Path $PSScriptRoot\src\ShoppingListNginx
$targetFolder = "\\nasgul\David\ShoppingListNginx"
robocopy $ngFolder $targetFolder 

# Build and run docker image
$dockerPath = "/share/CACHEDEV1_DATA/.qpkg/container-station/bin/docker"
$dockerRmCmd = "$dockerPath rm -f shoppinglistnginx;sleep 3"
$gotoFolderCmd = "cd /share/David/ShoppingListNginx"
$dockerBuildCmd = "$dockerPath build -t shoppinglistnginx ."
$dockerRunCmd = "$dockerPath run --restart always -d -p 8130:8130 --name shoppinglistnginx shoppinglistnginx"
$allCommands = "$dockerRmCmd ; $gotoFolderCmd ; $dockerBuildCmd ; $dockerRunCmd"
ssh -o HostKeyAlgorithms=+ssh-rsa -o PubkeyAcceptedAlgorithms=+ssh-rsa -o MACs=hmac-sha2-256 david@nasgul $allCommands 
 
