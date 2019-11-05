Param(
	[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
	[string]$UserStoryID
)
$projectFile      = "$ProjectFileFullPath$"
$toolsFolder      = "$PackageFolder$"
$mainScript       = [System.IO.Path]::Combine($toolsFolder, ".\tools\Metis.Tools\TestCase-Sync-VSTS.ps1")
$releaseDefScript = [System.IO.Path]::Combine($toolsFolder, ".\tools\Metis.Tools\Release-Definitions\VSTS-Tests-Sync.releasedef");

if(-not [System.IO.File]::Exists($mainScript)){
	throw "Main Script '$($mainScript)' does not exists"
}
if(-not [System.IO.File]::Exists($releaseDefScript)){
	throw "Release Def '$($releaseDefScript)' does not exists"
}

&$mainScript -SettingsFilePath $releaseDefScript -UserStory $UserStoryID -ProjectFile $projectFile