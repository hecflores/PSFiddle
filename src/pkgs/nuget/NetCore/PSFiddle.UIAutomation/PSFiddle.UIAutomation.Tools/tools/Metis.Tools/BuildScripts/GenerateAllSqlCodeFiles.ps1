Param(
	[string] $ProjectFolder
)

Write-Host "Fetching sql files..." -NoNewline
$sqlFilesFound = Get-ChildItem -Path $ProjectFolder -Filter "*.sql" -Recurse  | Where {-not ($_.FullName -match 'bin[^\w]' -or $_.FullName -match 'obj[^\w]')}
Write-Host "$($sqlFilesFound.Length)"

$GeneratedDBScript = (Join-path $PSScriptRoot "../GenerationScripts/GenerateDBClasses.ps1" )
$sqlFilesFound | Foreach {
	&$GeneratedDBScript $ProjectFolder ($_.FullName)
}

[Environment]::Exit(0)