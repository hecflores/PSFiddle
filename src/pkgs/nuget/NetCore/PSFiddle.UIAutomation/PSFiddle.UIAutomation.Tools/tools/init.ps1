Param(
	[String] $installPath, 
	[String] $toolsPath, 
	[object] $package, 
	[object] $project
)


# $vsixInstallerTool = "$($env:MSBuildStartupDirectory)VSIXInstaller.exe"
# if(-not [System.IO.File]::Exists($vsixInstallerTool)){
# 	throw "VSIXInstaller does not exists"
# }

# $versionFolderName          = [System.IO.Path]::GetFileName($installPath)
# $visualStudiosExtensionPath = [System.IO.Path]::Combine($installPath,"tools\MCVSExtension\MC.VS.Extension.vsix")
# if(-not [System.IO.File]::Exists($visualStudiosExtensionPath)){
# 	throw "MC.VS.Extension.vsix was not found"
# }

# start $visualStudiosExtensionPath 

# $contentFilesFolder     = [System.IO.Path]::Combine($installPath,"tools\contentFiles\**")
# $dumpFolder             = [System.IO.Path]::Combine("C:\ModulePackages")
# $appSettingsFile        = [System.IO.Path]::Combine($project.FullName,"..\app.config")
# $changeAppSettingsFile  = [System.IO.Path]::Combine($installPath,"tools\Metis.Tools\BuildScripts\UpdateAppConfig.ps1")


# if(-not [System.IO.Directory]::Exists($dumpFolder))
# {
# 	md $dumpFolder
# }


# Copy-Item -Path $contentFilesFolder -Destination $dumpFolder -Recurse -Force

# $ppFiles = Get-ChildItem -Path $dumpFolder  -Filter "*.*" -Recurse | Where { [System.IO.File]::Exists($_.FullName) } | Where { [System.IO.Path]::GetExtension($_.FullName) -ne ".dll"}
# $projectFolder = [System.IO.Path]::Combine($installPath,"..\")
# $ppFiles | Foreach {
# 	$content = Get-Content $_.FullName
# 	$content = $content -replace '\$PackageFolder\$',$installPath
# 	$content = $content -replace '\$ProjectFileFullPath\$',$project.FullName
# 	$content | Set-Content $_.FullName
# }

# &$changeAppSettingsFile -AppSettingFullPath $appSettingsFile -PluginName "SpecFlow.SharedStepFeature" -PluginPath ".\App_MetisTools\SpecFlowPlugin"
# &$changeAppSettingsFile -AppSettingFullPath $appSettingsFile -PluginName "SpecFlow.SyncVSTS" -PluginPath ".\App_MetisTools\SpecFlowPlugin"