# Pobierz informacje o dysku C:
$diskInfo = Get-PSDrive -Name C

# Przekonwertuj ilo�� wolnej przestrzeni na gigabajty i zaokr�glij do 1 miejsca po przecinku
$freeSpaceGB = [Math]::Round($diskInfo.Free / 1GB, 1)

# Wy�wietl ilo�� wolnej przestrzeni na dysku C: w gigabajtach, zaokr�glone do 1 miejsca po przecinku
echo "You Have $freeSpaceGB GB free space on Disk C"