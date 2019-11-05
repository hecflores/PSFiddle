Param(
	[string] $ProjectFolder
)

Write-Host "Fetching ps1 files..." -NoNewline
$ps1FilesFound = Get-ChildItem -Path $ProjectFolder -Filter "*.ps1" -Recurse  | Where {-not ($_.FullName -match 'bin[^\w]' -or $_.FullName -match 'obj[^\w]')}
Write-Host "$($ps1FilesFound.Length)"

$GeneratedPSScript = (Join-path $PSScriptRoot "../GenerationScripts/GeneratePSClasses.ps1" )
$ps1FilesFound | Foreach {
	&$GeneratedPSScript $ProjectFolder ($_.FullName)
}

[Environment]::Exit(0)