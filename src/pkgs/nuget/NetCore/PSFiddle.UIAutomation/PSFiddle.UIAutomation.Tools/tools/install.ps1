Param(
	[String] $installPath, 
	[String] $toolsPath, 
	[object] $package, 
	[object] $project
)

$contentFilesFolder = [System.IO.Path]::Combine($installPath,"tools\contentFiles")
$dumpFolder         = [System.IO.Path]::Combine($project.FullName,"App_MetisTools")
Remove-Item $dumpFolder -Filter "*.*" -Recurse -Force
Remove-Item $dumpFolder 


