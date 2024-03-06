$currentDate = Get-Date

$formattedDate = $currentDate.ToString("'Today is ' MM/dd/yyyy HH:mm")

Write-Host $formattedDate
Get-Date