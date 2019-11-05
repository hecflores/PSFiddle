Param(
	[string] $AutomationProjectFolder,
	[string] $UIProjectFolder
)


Function DeleteIfExists([string] $relative){
	Write-Host "   Deleting '$($relative)'"
	
	$file = [System.IO.Path]::Combine("$($ProjectFolder)",$relative)
	Get-ChildItem $file -Recurse | Foreach{
		Write-Host "      $($_.FullName)"
		del $_.FullName
	}
	
}

Write-Host "Deleting Generated Files"
DeleteIfExists "$($AutomationProjectFolder).pom/**/*"
DeleteIfExists "$($AutomationProjectFolder)**/*.automationmaster"
DeleteIfExists "$($UIProjectFolder)**/*.automationmaster.generated"


