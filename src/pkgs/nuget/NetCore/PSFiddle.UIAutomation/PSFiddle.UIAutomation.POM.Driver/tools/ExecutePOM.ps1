Param(
	[string] $POMExeFile,
	[string] $AutomationProjectFolder,
	[string] $UIProjectFolder
)

$UIProjectFolder         = Resolve-Path $UIProjectFolder
$AutomationProjectFolder = Resolve-Path $AutomationProjectFolder
$POMExeFile              = Resolve-Path $POMExeFile

$generatedFolder = (Join-path $AutomationProjectFolder ".\Generated")
$pageObjectFolder = (Join-path $AutomationProjectFolder ".\Generated\PageObjects")
	
if(-not ([System.IO.Directory]::Exists($generatedFolder))){
	md $generatedFolder
}
if(-not ([System.IO.Directory]::Exists($pageObjectFolder))){
	md $pageObjectFolder
}

Write-Host "+--------------------------------------------------------------------------+" 
Write-Host "+--------------------------------------------------------------------------+"
Write-Host "+---------------------- Page Object Generation v2 -------------------------+" 
Write-Host "+--------------------------------------------------------------------------+" 
Write-Host "+--------------------------------------------------------------------------+" 
Write-Host ""
&$POMExeFile $UIProjectFolder $AutomationProjectFolder