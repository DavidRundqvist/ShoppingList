$ErrorActionPreference = "Stop"

pushd $PSScriptRoot

# Stop previous version
Write-Host -foreground Cyan "Stopping old version..."
ssh -i .\davidAtBraavos david@braavos "sudo killall dotnet"


# Publish new
Write-Host -foreground Cyan "Publishing new version..."
rename-item "\\BRAAVOS\asp\ShoppingList" "\\BRAAVOS\asp\ShoppingList_old"
dotnet publish .\src\ShoppingList -o "\\BRAAVOS\asp\ShoppingList" --configuration Release -r ubuntu.14.04-x64

#Start new
Write-Host -foreground Cyan "Starting new version..."
$startCommand = "cd /home/david/asp/ShoppingList/
nohup dotnet ShoppingList.dll > /dev/null 2>&1 &"
ssh -i .\davidAtBraavos david@braavos $startCommand

# Cleanup
Write-Host -foreground Cyan "Removing old version..."
del "\\BRAAVOS\asp\ShoppingList_old" -Recurse -Force
popd

Write-Host -ForegroundColor Green "Shopping list published" 