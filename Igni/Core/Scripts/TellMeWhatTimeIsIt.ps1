$currentDate = Get-Date

$formattedDate = $currentDate.ToString("'Teraz jest' dddd MM/dd/yyyy 'i mamy godzine' HH:mm")

Write-Host $formattedDate
Get-Date