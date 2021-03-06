
Function Show-PSFiddle {
	
	
	$projects = Get-PSFiddleAllProjects
	Write-Host "`r`nProjects Found ($(($projects | Measure).Count)):"
	$projects | Foreach {
		Write-Host "$($_.Name)`r`n  $($_.Path)`r`n"
	}
	
	
	$helpText = Get-Content ([System.IO.Path]::Combine($PSScriptRoot, "..\Files\Help.txt")) -Raw
	Write-Host ""
	Write-Host ""
	Write-Host $helpText
}

Export-ModuleMember -Function Show-PSFiddle


################################################
Function Get-PSFiddleAllProjects {
	
	$projects = $env:PSFiddleTestProjectPath -split ";"| Where { -not ([String]::IsNullOrEmpty($_))} | Foreach {
		$name = [System.IO.Path]::GetFileNameWithoutExtension($_)
		$project = New-Object PSObject
		$project | add-member Noteproperty Name $name
		$project | add-member Noteproperty Path $_
		return $project
	} 

	return $projects
	
}

Export-ModuleMember -Function Get-PSFiddleAllProjects

################################################
Function Get-PSFiddleProject {
	Param(
		[String] $ProjectName = $null
	)
	if(-not $ProjectName){
		Write-Error "No ProjectName was provided"
		return $null
	}
	$project = Get-PSFiddleAllProjects | Where {$_.Name -eq $ProjectName}
	if(-not $project)
	{
		Write-Error "$($projectName) was not found..."
	}
	return $project
}

Export-ModuleMember -Function Get-PSFiddleProject

################################################
Function Add-PSFiddleProject {
	Param(
		[String] $projectPath
	)
	
	if(-not ([System.IO.Path]::IsPathRooted($projectPath))){
		$currentLocation = Get-Location
		$projectPath = [System.IO.Path]::Combine($currentLocation, $projectPath)
	}

	if(-not ([System.IO.File]::Exists($projectPath))){
		throw "Project file '$($projectPath)' does not exists"
	}

	if(([System.IO.Path]::GetExtension($projectPath)) -ne ".csproj"){
		throw "Project file '$($projectPath)' was expected to have extension 'csproj' but didnt"
	}

	$oldPSFiddleProject = $env:PSFiddleTestProjectPath
	if(-not $oldPSFiddleProject){
		$oldPSFiddleProject = ""
	}
	$projects = $oldPSFiddleProject -split ";"
	$projects+=$projectPath
	
	$newPSFiddleProject = $projects -join ";"
	
	[Environment]::SetEnvironmentVariable("PSFiddleTestProjectPath", $newPSFiddleProject, "User")
	[Environment]::SetEnvironmentVariable("PSFiddleTestProjectPath", $newPSFiddleProject, "Process")

}

Export-ModuleMember -Function Add-PSFiddleProject

################################################
Function Show-PSFiddleLocations {
	
	Write-Host "PSScriptRoot: $PSPSScriptRoot"
	Write-Host "PSCommandPath: $PSCommandPath"
	Write-Host "Current-Location: $(Get-Location)"

}
Export-ModuleMember -Function Show-PSFiddleLocations



################################################
Function Sync-TestsFromUserStory{
	Param(
		[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
		[string]$ProjectName,
		[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
		[string]$UserStoryID
	)

	$project = Get-PSFiddleProject $ProjectName
	if($project)
	{
		$projectFile      = $project.Path
		$mainScript       = [System.IO.Path]::Combine($PSScriptRoot, "..\..\..\..\..\tools\PSFiddle.Tools\TestCase-Sync-VSTS.ps1")
		$releaseDefScript = [System.IO.Path]::Combine($PSScriptRoot, "..\..\..\..\..\tools\PSFiddle.Tools\Release-Definitions\VSTS-Tests-Sync.releasedef");

		if(-not [System.IO.File]::Exists($mainScript)){
			throw "Main Script '$($mainScript)' does not exists"
		}
		if(-not [System.IO.File]::Exists($releaseDefScript)){
			throw "Release Def '$($releaseDefScript)' does not exists"
		}

		&$mainScript -SettingsFilePath $releaseDefScript -UserStory $UserStoryID -ProjectFile $projectFile	
	}
}

Export-ModuleMember -Function Sync-TestsFromUserStory

################################################
Function Get-PSFiddlePackagePath{
	
	$metisExtensionPath = [System.IO.Path]::Combine($PSScriptRoot, "..\..\..\..\..\")
	return $metisExtensionPath | Resolve-Path

}

Export-ModuleMember -Function Get-PSFiddlePackagePath

################################################
Function Get-PSFiddlePackageRelativePath{
	Param(
		[String] $RelativePath,
		[switch] $ExpectFileToExists = $true
	)
	$file = [System.IO.Path]::Combine((Get-PSFiddlePackagePath), $RelativePath)
	if($ExpectFileToExists){
		if(-not (Test-Path $file)){
			throw "File '$($RelativePath)' was not found and was expected"
		}
	}
	return $file | Resolve-Path

}

Export-ModuleMember -Function Get-PSFiddlePackageRelativePath

################################################
Function Install-PSFiddleVSExtension{
	
	$extensionPath = Get-PSFiddlePackageRelativePath "tools\MCVSEXtension\MC.VS.Extension.vsix" -ExpectFileToExists
	start $extensionPath
}

Export-ModuleMember -Function Install-PSFiddleVSExtension