function Generate-RandomPassword {
    param (
        [Parameter(Mandatory=$true)]
        [ValidateRange(8,16)]
        [int]$Length
    )

    # Character sets
    $upperCase = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'
    $lowerCase = 'abcdefghijklmnopqrstuvwxyz'
    $numbers = '0123456789'
    $specialChars = '!@#$%^&*()_-+=<>?:{}[]|~`'

    # Ensure at least one of each type is included
    $passwordChars = @(
        ($upperCase.ToCharArray() | Get-Random),
        ($lowerCase.ToCharArray() | Get-Random),
        ($numbers.ToCharArray() | Get-Random),
        ($specialChars.ToCharArray() | Get-Random)
    )

    # Combine all characters into a single string for further character selection
    $allChars = $upperCase + $lowerCase + $numbers + $specialChars

    # Fill the rest of the password
    for ($i = $passwordChars.Count; $i -lt $Length; $i++) {
        $passwordChars += $allChars.ToCharArray() | Get-Random
    }

    # Shuffle the password characters to avoid any patterns
    $shuffledPasswordChars = $passwordChars | Get-Random -Count $Length
    $randomPassword = -join $shuffledPasswordChars
    
    return $randomPassword
}

# Example usage
$password = Generate-RandomPassword -Length 12
Set-Clipboard -Value $password
echo "Generated password is in clipboard"