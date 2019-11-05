Param(
	[string] $installPath,
	[string] $projectPath
)


$metisPath            = Resolve-Path $installPath
$metisVersion         = ([System.IO.Path]::GetFileName($installPath))
$toolsPSModulesFolder = Resolve-Path ([System.IO.Path]::Combine($installPath,"tools\Metis.Tools\PSModules"))
$metisModule          = Resolve-Path ([System.IO.Path]::Combine($toolsPSModulesFolder,"Metis\Metis.psd1"))

$currentModulePath = $env:PSModulePath
$newModulePath     = $currentModulePath

$modules  = $currentModulePath -split ";" | Foreach-Object { Resolve-Path $_ }
if($modules | Where {$_ -eq $toolsPSModulesFolder}){
	Write-Host "Module already has been installed with version '$($metisVersion)'"
	return
}

$InstallMetisModulesPS1 = (Join-path $PSScriptRoot ".\InstallMetisModules.ps1")
$command = " &('$($InstallMetisModulesPS1)')  '$($installPath)'  '$($projectPath)'"
Write-Host $command
Start-Process Powershell.exe -ArgumentList "-command",$command -Wait


