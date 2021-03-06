﻿Param(
	[string] $ProjectFolder,
	[string] $SqlFilePath
)

$__DbScriptsFile = [System.IO.Path]::ChangeExtension($SqlFilePath,"sql.cs")
$newHash = (Get-FileHash $SqlFilePath).Hash
if(Test-Path $__DbScriptsFile){
	$currentContent = [System.IO.File]::ReadAllText($__DbScriptsFile)
	if($currentContent -match "// ### hash (.+) ###"){
		$oldHash = $Matches[1]
		

		if($oldHash -eq $newHash){
			return
		}
	}
}
# Write-Host "'$($SqlFilePath) has chanaged..."
# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


$sqlContent = (Get-Content $SqlFilePath -Raw) 
$matches = ([regex]'\$\(([A-Za-z_][A-Za-z_0-9]*)\)').Matches($sqlContent)

$argumentNames = @()
$arguments = @()
$tokenMethodCalls = @()

$matches | Foreach {if(-not ($argumentNames -contains $_.Groups[1].Value)){ $argumentNames += $_.Groups[1].Value}} 

$argumentNames | Foreach {$arguments +=  "String $($_) = `"`""} 
$argumentNames | Foreach {$tokenMethodCalls += ".TokenReplace(`"`$($($_))`", $($_))"} 
$arguments += "Action<DataSet> dataOut = null"

$arguments = $arguments -join ","
$tokenMethodCalls = $tokenMethodCalls -join "`r`n					"

$relativeFilePath =  $SqlFilePath.Replace($ProjectFolder,"")
$relativeFilePath = $relativeFilePath -replace "^[\/\\]",""
$fileName = [System.IO.Path]::GetFileNameWithoutExtension($SqlFilePath) -replace '[^\w]',""

$function = "void $($fileName)()"

$I_DbScriptsGeneratedContent=@"

		void $($fileName)($($arguments));

"@


$__DbScriptsGeneratedContent=@"

		public void $($fileName)($($arguments)){
			Dependencies.Files().Factory().UseFile(@"$($relativeFilePath)", file => {
				file.MakeCopy()
					$($tokenMethodCalls)
					.Out((outFile) => XConsole.WriteLine($"SQL FILE: {outFile.FilePath}"))
					.ExecuteAsSqlStatment(dataOut)
					.DeleteNow();
				}).Build();		
		}

"@

$format = '(\#region GENERATED_CODE.*?[\n\r])([\S\s]*?[\n\r])([^\n\r]+#endregion)'

$I_DbScriptsContent = @"
// ### hash $($newHash) ###

using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Dependencies;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Helpers;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MC.Track.TestSuite.UI.Generated
{
    public partial interface IDbScripts
    {
        #region GENERATED_CODE
		$($I_DbScriptsGeneratedContent)
		#endregion
    }
}
"@
$__DbScriptsContent = @"


namespace MC.Track.TestSuite.UI.Generated
{
    public partial class DbScripts : IDbScripts
    {
		$($__DbScriptsGeneratedContent)
    }
}
"@





$MainContent  = $I_DbScriptsContent
$MainContent += "`r`n`r`n"
$MainContent += $__DbScriptsContent


$fileName = [System.IO.Path]::GetFileName($__DbScriptsFile)
Write-Host "Generating $($fileName)..." -NoNewline
[System.IO.File]::WriteAllText($__DbScriptsFile, $MainContent)
Write-Host "Done"


