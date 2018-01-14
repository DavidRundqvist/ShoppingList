$ErrorActionPreference = "Stop"

rename-item "\\BRAAVOS\asp\ShoppingList" "\\BRAAVOS\asp\ShoppingList_old"

"Publishing new..."
dotnet publish .\src\ShoppingList -o "\\BRAAVOS\asp\ShoppingList" --configuration Release -r ubuntu.14.04-x64

"Removing old..."
del "\\BRAAVOS\asp\ShoppingList_old" -Recurse -Force

write-host "Shopping list published" -ForegroundColor Green
