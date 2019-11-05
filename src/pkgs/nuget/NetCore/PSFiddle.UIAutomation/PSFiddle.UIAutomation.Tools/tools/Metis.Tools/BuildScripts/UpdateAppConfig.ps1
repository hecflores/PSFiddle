Param(
	[string] $PluginPath,
	[string] $PluginName,
	[string] $AppSettingFullPath
)

if(-not [System.IO.File]::Exists($AppSettingFullPath)){
	throw "App Settings File '$($AppSettingFullPath)' does not exists"
}

[XML]$appSettings = Get-Content $AppSettingFullPath
$singleNode = $appSettings.SelectSingleNode("/configuration/specFlow/plugins/add[@name='$($PluginName)']/@path")
if($singleNode)
{
	$singleNode
	Write-Host "Node Found: $($singleNode.'#text')"
}
else {
	Write-Host "Node was not found"
}

$singleNode.'#text' = $PluginPath

$appSettings.Save($AppSettingFullPath)