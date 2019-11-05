Param(
	[string] $installPath,
	[string] $projectPath
)

$metisPath            = $installPath
$metisVersion         = [System.IO.Path]::GetFileName($installPath)
$toolsPSModulesFolder = [System.IO.Path]::Combine($installPath,"tools\Metis.Tools\PSModules")
$metisModule          = [System.IO.Path]::Combine($toolsPSModulesFolder,"Metis\Metis.psd1")
$currentModulePath = $env:PSModulePath
$newModulePath     = $currentModulePath

$modules  = $currentModulePath -split ";"
if($modules | Where {$_ -eq $toolsPSModulesFolder}){
	Write-Host "Module already has been installed with version '$($metisVersion)'"
	return
}

Write-Host "Installing Metis Module version '$($metisVersion)'`r`n  $($metisPath)"
$modules  = $modules | Where { -not ($newModulePath -match '(?:[^\;]*mc\.metis\.tools[^;]*)') }
$modules += $toolsPSModulesFolder
$newModulePath = $modules -join ";"

# Set the new PSModulePath
[Environment]::SetEnvironmentVariable("PSModulePath", $newModulePath, "User")
[Environment]::SetEnvironmentVariable("PSModulePath", $newModulePath, "Process")

Write-Host "Old PSModulePath: '$($currentModulePath -split ";")'"
Write-Host "New PSModulePath: '$($newModulePath -split ";")'"

$currentMetisTestProjectPath = $env:MetisTestProjectPath

Import-Module $metisModule

Write-Host "Adding this project to Metis Registrations`r`n  $($projectPath)"
Add-MetisProject $projectPath


