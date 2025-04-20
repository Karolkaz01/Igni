# Pobierz informacje o dysku C:
$diskInfo = Get-PSDrive -Name C

# Przekonwertuj iloœæ wolnej przestrzeni na gigabajty i zaokr¹glij do 1 miejsca po przecinku
$freeSpaceGB = [Math]::Round($diskInfo.Free / 1GB, 1)

# Wyœwietl iloœæ wolnej przestrzeni na dysku C: w gigabajtach, zaokr¹glone do 1 miejsca po przecinku
echo "You Have $freeSpaceGB GB free space on Disk C"