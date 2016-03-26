dnvm use -r coreclr 1.0.0-rc1-update1
dnu restore
$env:Hosting:Environment="Development"
dnx web
