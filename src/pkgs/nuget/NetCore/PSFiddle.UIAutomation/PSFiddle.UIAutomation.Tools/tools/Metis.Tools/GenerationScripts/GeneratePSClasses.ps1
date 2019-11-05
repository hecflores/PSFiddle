Param(
	[string] $ProjectFolder,
	[string] $SqlFilePath
)

$__DbScriptsFile = [System.IO.Path]::ChangeExtension($SqlFilePath,"ps1.cs")
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

$matches | ForEach-Object {if(-not ($argumentNames -contains $_.Groups[1].Value)){ $argumentNames += $_.Groups[1].Value}} 

$argumentNames | ForEach-Object {$arguments +=  "String $($_) = `"`""} 
$argumentNames | ForEach-Object {$tokenMethodCalls += ".TokenReplace(`"`$($($_))`", $($_))"} 

$arguments = $arguments -join ","
$tokenMethodCalls = $tokenMethodCalls -join "`r`n					"

$relativeFilePath = $SqlFilePath.Replace($ProjectFolder,"")
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
					.RunAsPowershellContent()
                    .Out(b => XConsole.WriteLine($"Powershell Module file: {b.FilePath}"))
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
    public partial interface IPowershellModules
    {
		$($I_DbScriptsGeneratedContent)
    }
}
"@
$__DbScriptsContent = @"

namespace MC.Track.TestSuite.UI.Generated
{
    public partial class PowershellModules : IPowershellModules
    {
		$($__DbScriptsGeneratedContent)
    }
}
"@

$__DbScriptsFile = [System.IO.Path]::ChangeExtension($SqlFilePath,".ps1.cs")
$MainContent  = $I_DbScriptsContent
$MainContent += "`r`n`r`n"
$MainContent += $__DbScriptsContent

$fileName = [System.IO.Path]::GetFileName($__DbScriptsFile)
Write-Host "Generating $($fileName)..." -NoNewline
[System.IO.File]::WriteAllText($__DbScriptsFile, $MainContent);
Write-Host "Done"


