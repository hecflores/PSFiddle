Param(
    [string] $organization,
    [string] $project,
    [string] $repositoryId,
    [string] $repositoryUri,
    [string] $repositoryType,
    [string] $rootDirectory,
    [string] $branch,
	[bool]   $deleteDepricatedReleaseDefinitions = $false,
	[string] $prefix = "hecflores.PSFiddle",
    [bool]   $mocked = $false
)


function Write-Color() {
    Param (
        [string] $text = $(Write-Error "You must specify some text"),
        [switch] $NoNewLine = $false
    )
	
    $startColor = $host.UI.RawUI.ForegroundColor;
	# $regex = ([regex]'(.+?)(?:\{(red|cyan|green|blue|magenta)\}|$)(.*)')
	# while(-not ([String]::IsNullOrEmpty($text))){
		# $before = $regex.Replace($text,'$1')
		# $color  = $regex.Replace($text,'$2')
		# $after  = $regex.Replace($text,'$3')
		 # if ($_ -in [enum]::GetNames("ConsoleColor")) {
			# $host.UI.RawUI.ForegroundColor = ($_ -as [System.ConsoleColor]);
		# }
	# }
	
    $text.Split( [char]"{", [char]"}" ) | ForEach-Object { $i = 0; } {
        if ($i % 2 -eq 0) {
            Write-Host $_ -NoNewline;
        } else {
            if ($_ -in [enum]::GetNames("ConsoleColor")) {
                $host.UI.RawUI.ForegroundColor = ($_ -as [System.ConsoleColor]);
            }
			else{
				 Write-Host "{$($_)}" -NoNewline;
			}
        }

        $i++;
    }

    if (!$NoNewLine) {
        Write-Host;
    }
    $host.UI.RawUI.ForegroundColor = $startColor;
}

$branch = $branch.Replace("refs/heads/","")
$rootDirectory = [System.IO.Path]::GetFullPath($rootDirectory)
$rootDirectory = $rootDirectory -replace "[\/\\]$",""

Write-Output "Getting files..."
$allYamlFiles      = Get-ChildItem -Path $rootDirectory -Filter "*.vsts-ci.yml" -Recurse  
$existingYamlFiles = az devops invoke --area build --resource definitions --org $organization --route-parameters @("project=$project") --query-parameters @("includeAllProperties=true","repositoryId=$repositoryId","repositoryType=$repositoryType") --api-version 5.1  --query "value[*].{yamlFileName: process.yamlFilename, Id: id}" | ConvertFrom-Json

$existingYamlFiles | Foreach-Object {
	$_.yamlFileName = $_.yamlFileName -replace "$prefix\/",""
}

$existingYamlFiles = $existingYamlFiles | ForEach-Object{ @{
    FullFilePath = ([System.IO.Path]::GetFullPath([System.IO.Path]::Combine($rootDirectory, "./$($_.yamlFileName)"))  -replace "[\/\\]","/");
    RelativeFilePath = (($($_.yamlFileName) -replace "$prefix\/","") -replace "[\/\\]","/");
    DefinitionId = $_.Id;
    FolderPath = ([System.IO.Path]::GetFullPath([System.IO.Path]::Combine($rootDirectory, "./$($_.yamlFileName)/../"))  -replace "[\/\\]","/");
    RelativeFolderPath = (($($_.yamlFileName) -replace "^([^\/\\])",'/$1') -replace "[\/\\]","/") -replace "\/[^\/]+$","";
} }
$uniqueExistingYamlFiles = $existingYamlFiles.RelativeFolderPath | Where-Object {$_.Length -gt 0} | Foreach-Object {([regex]"^([^\/\\])").Replace($_,'/$1')} | Get-Unique

$allYamlFiles = $allYamlFiles | ForEach-Object{ @{
    FullFilePath = ([System.IO.Path]::GetFullPath( $_.FullName) -replace "[\/\\]","/");
    RelativeFilePath = ($_.FullName.Substring($rootDirectory.Length, $_.FullName.Length - $rootDirectory.Length) -replace "[\/\\]","/");
    FolderPath = ([System.IO.Path]::GetFullPath([System.IO.Path]::Combine($_.FullName, "/../"))  -replace "[\/\\]","/");
    RelativeFolderPath = ($_.FullName.Substring($rootDirectory.Length, $_.FullName.Length - $rootDirectory.Length - $_.Name.Length - 1) -replace "[\/\\]","/");
} } 
$uniqueAllYamlFiles = $allYamlFiles.RelativeFolderPath | Where-Object {$_.Length -gt 0} | Foreach-Object {([regex]"^([^\/\\])").Replace($_,'/$1')} | Get-Unique

$missingYamlFiles = $allYamlFiles | Where-Object {
    $file = $_.FullFilePath.ToLower()
    $fullFilePaths = $existingYamlFiles.FullFilePath.ToLower()
    if($file -in $fullFilePaths ){
        return $false
    }
    return $true
}
$deletedYamlFiles = $existingYamlFiles | Where-Object {
    $file = $_.FullFilePath.ToLower()
    $fullFilePaths = $allYamlFiles.FullFilePath.ToLower()
    if($file -in $fullFilePaths ){
        return $false
    }
    return $true
}
$deletedFolders = $uniqueExistingYamlFiles | Where-Object {
    $file = $_.ToLower()
    $fullFilePaths = $uniqueAllYamlFiles.ToLower()
    if( $fullFilePaths | Where-Object {$_.StartsWith($file)} ){
        return $false
    }
    return $true
}
Write-Color "`r`n {magenta} A l l {gray}"
$allYamlFiles | ForEach-Object {Write-Color " {white}$($_.RelativeFilePath){gray}"}

Write-Color "`r`n{magenta} E x i s t i n g {gray}"
$existingYamlFiles | ForEach-Object{Write-Color " {white}$($_.RelativeFilePath){gray}"}

Write-Color "`r`n{magenta} M i s s i n g {gray} "
$missingYamlFiles | ForEach-Object{Write-Color " {white}$($_.RelativeFilePath){gray}"}

Write-Color "`r`n{magenta} Y a m l s   N e e d   t o   b e   d e l e t e d{gray}"
$deletedYamlFiles | ForEach-Object{Write-Color " {white}$($_.RelativeFilePath){gray}"}

Write-Color "`r`n{magenta} F o l d e r s   N e e d   t o   b e   d e l e t e d{gray}"
$deletedFolders | ForEach-Object{Write-Color " {white}$($_){gray}"}

Write-Color "`r`n{magenta} C r e a t i n g   M i s s i n g   P i p e l i n e s {gray}"
$missingYamlFiles | ForEach-Object {
    $fileName    = [System.IO.Path]::GetFileName($_.RelativeFilePath)
    $releaseName = $fileName.Replace(".vsts-ci.yml","")

    $relativeYamlFilePath   = $_.RelativeFilePath
    $relativeYamlFolderPath = $relativeYamlFilePath.Substring(0, $relativeYamlFilePath.Length - $fileName.Length)
    $relativeYamlFilePath = $relativeYamlFilePath.Replace("\","/")
	

    Write-Color " {white}$($_.RelativeFilePath){gray}"
    $command = "az pipelines create --org $organization --yaml-path $relativeYamlFilePath --folder-path $relativeYamlFolderPath --name $releaseName --description `"Auto Created from $_`" --repository $repositoryUri --branch $branch --repository-type $repositoryType"
    Write-Color "   {gray}$command`r`n"
    if(-not $mocked){
        Invoke-Expression $command
    }
}

if($deleteDepricatedReleaseDefinitions){
    Write-Color "`r`n{magenta} D e l e t i n g   D e p r i c a t e d   P i p e l i n e s {gray}"
	$deletedYamlFiles | ForEach-Object {

        $definitionId = $_.DefinitionId

		Write-Color " {white}$($_.RelativeFilePath){gray}"
		$command = "az pipelines delete --org $organization --id $definitionId --yes"
		Write-Color "   {gray}$command`r`n"
		if(-not $mocked){
		    Invoke-Expression $command
        }
	}
    Write-Color "`r`n{magenta} D e l e t i n g   D e p r i c a t e d   F o l d e r s{gray}"
    $deletedFolders | ForEach-Object {

        $folder = $_

		Write-Color " {white}$($_){gray}"
		$command = "az pipelines folder --org $organization -path $folder --yes"
		Write-Color "   {gray}$command`r`n"
        if(-not $mocked){
		    Invoke-Expression $command
        }
	}

}

