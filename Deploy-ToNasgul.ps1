# Copy files
$slFolder = Resolve-Path $PSScriptRoot\src\ShoppingList
$binFolder = Resolve-Path $slFolder\bin
$objFolder = Resolve-Path $slFolder\obj
$targetFolder = "\\nasgul\David\ShoppingList"
robocopy $slFolder $targetFolder /MIR /XD $binFolder $objFolder

# Build and run docker image
$dockerPath = "/share/CACHEDEV1_DATA/.qpkg/container-station/bin/docker"
$dockerRmCmd = "$dockerPath rm -f shoppinglist;sleep 3"
$gotoFolderCmd = "cd /share/David/ShoppingList"
$dockerBuildCmd = "$dockerPath build -t shoppinglist:latest ."
$dockerRunCmd = "$dockerPath run --restart always -d -p 8000:8080 --name shoppinglist -v /share/David/ShoppingListData:/data shoppinglist"
$allCommands = "$dockerRmCmd ; $gotoFolderCmd ; $dockerBuildCmd ; $dockerRunCmd"
ssh -o HostKeyAlgorithms=+ssh-rsa -o PubkeyAcceptedAlgorithms=+ssh-rsa -o MACs=hmac-sha2-256 david@nasgul $allCommands 
 
