Param(
	[string] $ProjectFolder
)

Write-Host "Cleaning Feature Files..." -NoNewline

$ps1FilesFound = Get-ChildItem -Path $ProjectFolder -Filter "*.feature.cs" -Recurse  | Foreach {
	Write-Host "Deleting '$($_.FullName)'"
	del $_.FullName
}

[Environment]::Exit(0)