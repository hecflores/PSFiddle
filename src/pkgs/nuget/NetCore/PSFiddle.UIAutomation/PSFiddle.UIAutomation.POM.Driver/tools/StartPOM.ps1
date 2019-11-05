Param(
	[string] $POMExeFile,
	[string] $AutomationProjectFolder,
	[string] $UIProjectFolder
)
$rootPath = $PSScriptRoot


$POMExecutePS1 = (Join-path $rootPath ".\ExecutePOM.ps1")
$command = " &('$($POMExecutePS1)')  -POMExeFile '$($POMExeFile)'  -AutomationProjectFolder '$($AutomationProjectFolder)'  -UIProjectFolder '$($UIProjectFolder)'"
Write-Host $command
Start-Process Powershell.exe -ArgumentList "-command",$command -Wait
