[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [string]$SettingsFilePath
)
DynamicParam
{
	# Load the Modules I need...
	 $modules = @()
     $rootpath = $PSScriptRoot
	 
     $modules+=@{Path = Join-path $rootpath ".\Modules\ConfigManager.psm1" -Resolve;
				 Name = "ConfigManager"}
	 $modules+=@{Path = Join-path $rootpath ".\Modules\VSTSModule.psm1" -Resolve;
				 Name = "VSTSModule"}
	 $modules | Foreach { 
		$loader = [Scriptblock]::Create("using module $($_.Path)`r`n`r`nRemove-Module $($_.Name) -ErrorAction SilentlyContinue`r`nImport-Module $($_.Path)")
		. $loader
	 }
	 
	# Display all Available Variables
	 Write-Host "`r`nAvailable Variables (Before Setting): " -ForegroundColor DarkBlue
	 Get-ChildItem env: | %{ Write-Host ("{0,-50}: {1}" -f $_.Name,$_.Value) -ForegroundColor DarkCyan }
	 
     
	 
	 # Read in Settings
     Write-Host "Reading in Settings..." -NoNewline
     [XML]$settings= Get-Content $SettingsFilePath
     Write-Host "Ok."
	 
	 # Generate Parameter List
	 $parameters=@()
     return GenerateDynamicVariableDictionary $settings ([ref]$parameters)
}
Process{ 

	Function CleanStepForStepClass([string]$step){
		$step = $step -replace '"','""'
		$step = $step -replace '\(','\('
		$step = $step -replace '\)','\)'
		$step = $step -replace '\[','\['
		$step = $step -replace '\]','\]'
		$step = $step -replace '\*','\*'
		$step = $step -replace '\.','\.'
		$step = $step -replace '\/','\/'
		$step = $step -replace '\$','\$'
		$step = $step -replace '\?','\?'
		$step = $step -replace '\!','\!'
		$step = $step -replace '^ *(.*?) +$','$1'
		
		return $step
	}
	Function ConvertStepToToUseForTestMethod([hashtable]$Step){
		$id = [String]::Format("{0,0:000}", $Step["Index"])
		$stepType = $Step["Type"]
		$stepText = CleanStepForStepClass $Step["Text"]
				
		$camleText= "Step_$($id)"
		
		return $camleText
	}
	Function FullyPopulateWorkItem([string]$workId, [hashtable]$VSTSWorkItems, [hashtable]$alreadyUsedTestCases){
	
		try
		{
			$WorkItems = @{}
			$WorkItems["Steps"]          = @()
		
			# Check if work item exists
			if($alreadyUsedTestCases[$workId]){
				Write-Host "Work Item $($workId): Already populated" -ForegroundColor Cyan
				return;
			}
			
			# Populate
			PopulateWorkItem $workId $WorkItems
			
			Write-Host "Work Item $($workId): Successfull" -ForegroundColor Green
			$VSTSWorkItems["TestCases"] += $WorkItems
			$alreadyUsedTestCases[$workId] = $true
		}
		catch{
			Write-Host "Work Item $($workId): Failed - $($_.Exception.Message)" -ForegroundColor Red
		}
		
		
	}
	Function GenerateNormalFileXMLNodeForProj([object]$nodes, [XML]$project, $filePath){
		$EmbeddedResource = $project.CreateElement("EmbeddedResource")
		$Include = $project.CreateAttribute("Include")
		$Include.Value = $filePath
		
		$CopyToOutputDirectory = $project.CreateElement("CopyToOutputDirectory")
		$CopyToOutputDirectory.InnerText = "Always"
		
		$EmbeddedResource.Attributes.Append($Include)
		# $nodes["Nodes"]+=$EmbeddedResource
	
	}
	Function GenerateCompileItemWithDependentXMLNodeForProj([object]$nodes, [XML]$project, [string] $filePath, [string] $dependentName){
		# Add Compile XML Node
		$AutoGen = $project.CreateElement("AutoGen")
		$DesignTime = $project.CreateElement("DesignTime")
		$DependentUpon = $project.CreateElement("DependentUpon")

		$AutoGen.InnerText="True"
		$DesignTime.InnerText="True"
		$DependentUpon.InnerText=$dependentName 

		$Compile = $project.CreateElement("Compile")
		$include = $project.CreateAttribute("Include")
		$include.Value = $filePath
		$Compile.Attributes.Append($include)
		$Compile.AppendChild($AutoGen)
		$Compile.AppendChild($DesignTime)
		$Compile.AppendChild($DependentUpon)
		# $nodes["Nodes"]+=$Compile

	
	}
	Function GenerateUnImportantFileXMLNodeForProj([object]$nodes, [XML]$project, $filePath){
		$None = $project.CreateElement("None")
		$Include = $project.CreateAttribute("Include")
		$Include.Value = $filePath
		
		$None.Attributes.Append($Include)
		# $nodes["Nodes"]+=$None
	
	}
	Function GenerateCsFileXMLNodeForProj([object]$nodes, [XML]$project, $filePath){
		$compile = $project.CreateElement("Compile")
		$include = $project.CreateAttribute("Include")
		$include.Value = $filePath
		$compile.Attributes.Append($include)
		# $nodes["Nodes"]+=$compile
	
	}
	Function GenerateFeatureFileXMLNodeForProj([object]$nodes, [XML]$project, [string]$featureCsFilePath, [string]$featureFileCsName, [string]$featureFilePath){
	
		if(-not $nodes){
			throw "Nodes Parameter missing"
		}
		if(-not $project){
			throw "project Parameter missing"
		}
		if(-not $featureCsFilePath){
			throw "featureCsFilePath Parameter missing"
		}
		if(-not $featureFileCsName){
			throw "featureFileCsName Parameter missing"
		}
		if(-not $featureFilePath){
			throw "featureFilePath Parameter missing"
		}
		# Add Compile XML Node
		$AutoGen = $project.CreateElement("AutoGen")
		$DesignTime = $project.CreateElement("DesignTime")
		$DependentUpon = $project.CreateElement("DependentUpon")

		$AutoGen.InnerText="True"
		$DesignTime.InnerText="True"
		$DependentUpon.InnerText=[System.IO.Path]::GetFileName($featureFilePath)

		$Compile = $project.CreateElement("Compile")
		$include = $project.CreateAttribute("Include")
		$include.Value = $featureCsFilePath
		$Compile.Attributes.Append($include)
		$Compile.AppendChild($AutoGen)
		$Compile.AppendChild($DesignTime)
		$Compile.AppendChild($DependentUpon)
		# $nodes["Nodes"]+=$Compile

		# Add None XML Node
		$Generator = $project.CreateElement("Generator")
		$LastGenOutput = $project.CreateElement("LastGenOutput")

		$Generator.InnerText="SpecFlowSingleFileGenerator"
		$LastGenOutput.InnerText=$featureFileCsName

		$Compile = $project.CreateElement("None")
		$include = $project.CreateAttribute("Include")
		$include.Value = $featureFilePath
		$Compile.Attributes.Append($include)
		$Compile.AppendChild($Generator)
		$Compile.AppendChild($LastGenOutput)
		# $nodes["Nodes"]+=$Compile

	
	}
	Function PopulateWorkItemSteps([string]$workId, [hashtable]$fillTable, [object]$steps){
		
		
		foreach($step in $steps.ChildNodes){
			if($step.Name -eq "compref") {
				
				$compref = $step
					
				$sharedId = $compref.ref
				
				# If we already added this shared step, skip it
				if($VSTSWorkItems["SharedSteps"] | Where { $_["id"] -eq $sharedId }){
					$sharedStep = $VSTSWorkItems["SharedSteps"] | Where { $_["id"] -eq $sharedId }
				}
				else{
					$sharedStep = @{}
					$sharedStep["SharedSteps"] = @()
					$sharedStep["Steps"]       = @()
					PopulateWorkItem $sharedId $sharedStep
					
					# foreach($SS_step in $sharedStep["Steps"]){
					# 	$SS_step["Text"]="Shared Steps $($sharedId): $($SS_step['Type']) $($SS_step['Text'])"
					# 	$SS_step["Type"]="Given"
					# }
					
					# Add Running of Shared Step to Steps of Shared Step (Used to Print in CS File)
					# $runningStep = @{}
					# $runningStep["Text"]                 =  $sharedStep["Title"]
					# $runningStep["Type"]                 = "Given"
					# $runningStep["Action"] 		 		 = "RunSharedStep"
					# $runningStep["AssociatedSharedStep"] = $sharedStep
					# $runningStep["Index"]                = $sharedStep["Steps"].Count
					# $sharedStep["Steps"] += $runningStep
					
					# Add the Shared Step
					$VSTSWorkItems["SharedSteps"] += $sharedStep
				}
				
				
				# Add Running of Shared Step to Original Test Case (Used to Print in Feature File)
				$stepDef3 = @{}
				$stepDef3["Type"]         = "Given"
				$stepDef3["Text"]         = $sharedStep["Title"]
				$stepDef3["IsSharedStep"] = $true
				$stepDef3["Index"]        = $fillTable["Steps"].Count
				$fillTable["Steps"]+=$stepDef3
				
				
				PopulateWorkItemSteps $workId $fillTable $step
			}
			if($step.Name -eq "step") {
				$when = $step.parameterizedString[0].InnerText
				$then = $step.parameterizedString[1].InnerText
				
				if(($when) -and (-not [String]::IsNullOrWhiteSpace($when))){
					$stepDef = @{}
					$stepDef["Index"] = $fillTable["Steps"].Count
					$stepDef["Type"]="When"
					$stepDef["Text"]=$when
					$fillTable["Steps"]+=$stepDef
				}
				
				if(($then) -and (-not [String]::IsNullOrWhiteSpace($then))){
					$stepDef = @{}
					$stepDef["Index"] = $fillTable["Steps"].Count
					$stepDef["Type"]="Then"
					$stepDef["Text"]=$then
					$fillTable["Steps"]+=$stepDef
				}
				# Write-Host "Creating Step: $($when) -> $($then)"
			}
		}
		
	}
	Function PopulateWorkItem([string]$workId, [hashtable]$fillTable){
		
		$testCaseID = $workId
		
		$workItem = $vstsInstance.GetWorkItem($testCaseID)
		if(-not $workItem){
			throw "Work Item $($testCaseID) was not loaded"
		}
		
		if(-not $workItem.id){
			throw "No Work Item found"
		}
		# $workItem.fields.'Microsoft.VSTS.TCM.Steps'
		$steps = $workItem.fields.'Microsoft.VSTS.TCM.Steps'
		if(-not $steps){
			throw "Test Case didnt have any steps"
		}
		
		# Cleanup some of HTML encoding... That way I can just consume the text with no HTML
		$steps = $steps -replace '\"','"'
		$steps = $steps -replace '&lt;','<'
		$steps = $steps -replace '&gt;','>'
		
		# Convert to XML
		[XML] $workItemXML = $steps
		if(-not $workItemXML){
			throw "Work Item Steps parsing for XML was unsuccessfull"
		}
				
		$id    = $workItem.id
		$title = $workItem.fields.'System.Title'
		
		# Set up some work item properties
		$fillTable["Title"] = "$id - $($title)"
		$fillTable["Id"]    = "$id"
		
		PopulateWorkItemSteps $workId $fillTable $workItemXML.steps
		
	
	}
	
	StartProcessWithEnvironment $PSBoundParameters $parameters -callback {
		Write-Host "`r`nAvailable Variables (After Setting): " -ForegroundColor DarkBlue
		Get-ChildItem env: | %{ Write-Host ("{0,-50}: {1}" -f $_.Name,$_.Value) -ForegroundColor DarkCyan }
		
		# -----------------------------------------------------------
		$FeatureFolderPath = GetEnv "FeatureFilePath"
		$testProjFile = GetEnv "ProjectFile"
		$testProjFolder = [System.IO.Path]::Combine($testProjFile,'..\')
		$testsFolder = [System.IO.Path]::Combine($testProjFolder,"$($FeatureFolderPath)")
		$generateDBScriptsFile = [System.IO.Path]::Combine($PSScriptRoot,'.\GenerationScripts\GenerateDBClasses.ps1')
		$generatePSScriptsFile = [System.IO.Path]::Combine($PSScriptRoot,'.\GenerationScripts\GeneratePSClasses.ps1')
		$itemGroupSelector='//MsBuild:ItemGroup'
		$testName = GetEnv "Name"
		$namespaceSuffix = $FeatureFolderPath -replace "\\","."
		
		# Test CSPROJ File
		if(-not [System.IO.File]::Exists($testProjFile)){
			throw "csproj file $($testProjFile) does not exists"
		}

		[XML]$project = Get-Content $testProjFile

		$nsmgr = New-Object System.Xml.XmlNamespaceManager $project.NameTable
		$nsmgr.AddNamespace('MsBuild',$project.Project.xmlns)

		# Test ItemGroup node
		$node = $project.Project.SelectSingleNode($itemGroupSelector, $nsmgr)
		if(-not $node){
			throw "Generated ItemGroup tag was not found"
		}

		# Test Input File
		if(-not [System.IO.Directory]::Exists($testsFolder)){
			mkdir $testsFolder
		}

		# Generate Feature File
        $sharedStepFolder 		  = [System.IO.Path]::Combine($testsFolder,  "Shared")
		$sharedStepRelativeFolder = [System.IO.Path]::Combine($FeatureFolderPath,  "Shared")
        if(-not [System.IO.Directory]::Exists($sharedStepFolder)){
            [System.IO.Directory]::CreateDirectory($sharedStepFolder)
        }

        $featureSharedCsRelativeFile = [System.IO.Path]::Combine($sharedStepRelativeFolder, "$($testName).SharedSteps.feature.cs")
        $featureSharedRelativeFile = [System.IO.Path]::Combine($sharedStepRelativeFolder, "$($testName).SharedSteps.feature")
        $featureSharedCsFile = [System.IO.Path]::Combine($sharedStepFolder, "$($testName).SharedSteps.feature.cs")
        $featureSharedFile = [System.IO.Path]::Combine($sharedStepFolder, "$($testName).SharedSteps.feature")

		$featureCsRelativeFile = [System.IO.Path]::Combine($FeatureFolderPath, "$($testName).feature.cs")
		$featureRelativeFile = [System.IO.Path]::Combine($FeatureFolderPath, "$($testName).feature")
		$featureCsFile = [System.IO.Path]::Combine($testsFolder, "$($testName).feature.cs")
		$featureFile = [System.IO.Path]::Combine($testsFolder, "$($testName).feature")
        
		if([System.IO.File]::Exists($featureFile)){
			throw "Feature already exists for this user story, please either delete or change user story id"
		}
		# -----------------------------------------------------------
		
		$personalAccessToken = GetEnv "PersonalAccessToken"
		$VSTSAccount = GetEnv "VSTSAccount"
		$vstsInstance = [VSTSModule]::new($personalAccessToken,$VSTSAccount)
		
		$queryStr = $env:VSTSQuery
		
		
		$query = $vstsInstance.QueryWorkItems($queryStr)
		$VSTSWorkItems = @{}
		$VSTSWorkItems["TestCases"]   = @()
		$VSTSWorkItems["SharedSteps"] = @()
		
		$alreadyUsedTestCases = @{}
		
		## Work Item
		if($query.workItems)
		{
			foreach($vstsWorkItem in $query.workItems){
				
				if(PerformXMLEnvironmentVarReplacment ([ref]$settings) $true){
					FullyPopulateWorkItem $vstsWorkItem.id $VSTSWorkItems $alreadyUsedTestCases
				}
				else{
					Throw "Failed to perform variable replacment"
				}
				
			}
		}
		
		## Work Item Relations
		if($query.workItemRelations)
		{
			
			foreach($vstsWorkItemRelation in $query.workItemRelations){
			
				if(PerformXMLEnvironmentVarReplacment ([ref]$settings)  $true){
					# Extract Artifact Information
					if($vstsWorkItemRelation.source){
						FullyPopulateWorkItem $vstsWorkItemRelation.source.id $VSTSWorkItems $alreadyUsedTestCases
					}
					if($vstsWorkItemRelation.target){
						FullyPopulateWorkItem $vstsWorkItemRelation.target.id $VSTSWorkItems $alreadyUsedTestCases
					}
				}
				else{
					Throw "Failed to perform variable replacment"
				}
				
			}
		}
		
		
		$FileContents = @()
		
		
		
		# --------------------------- Generate Feature file ---------------------------------
		
		$featureFileContent = @()
		
		
		
		foreach($testCase in $VSTSWorkItems["TestCases"])
		{
			$testTitle = $testCase['Title']
			$testID    = $testCase['Id']

			$featureCsRelativeFile = [System.IO.Path]::Combine($FeatureFolderPath, "TC_$($testID)\TC_$($testID).feature.cs")
			$featureRelativeFile   = [System.IO.Path]::Combine($FeatureFolderPath, "TC_$($testID)\TC_$($testID).feature")
			$featureCsFile         = [System.IO.Path]::Combine($testsFolder, "TC_$($testID)\TC_$($testID).feature.cs")
			$featureFile           = [System.IO.Path]::Combine($testsFolder, "TC_$($testID)\TC_$($testID).feature")

		    $featureFileContent = @()
		    $featureFileContent += "Feature: Test Case $($testID)"
		    $featureFileContent += ""
			$featureFileContent += ""
			$featureFileContent += "@$($testName)"
			$featureFileContent += "@TC_$($testID)"
            $featureFileContent += "@workitem:$($testID)"
			$featureFileContent += "Scenario: $testTitle"
			
			$newLine = $false
			foreach($step in $testCase["Steps"])
			{
				
				
				$stepType    = $step['Type']
				$stepContent = $step['Text']
				$featureFileContent += "   $($stepType) $($stepContent)"
				
				if($newLine) { $featureFileContent += "" }
				
				$newLine = $stepType -eq "When"
			}
			$featureFileContent -join "`r`n"
		
			$newFile = @{}	
			$newFile["FileContent"] = $featureFileContent
			$newFile["FilePath"] = $featureFile
			$newFile["Nodes"] = @()
			GenerateFeatureFileXMLNodeForProj $newFile $project $featureCsRelativeFile ([System.IO.Path]::GetFileName($featureCsFile)) $featureRelativeFile
		
			$FileContents += $newFile
		}
		
		# --------------------------- Generate Feature file ---------------------------------
		
		
		
		# --------------------------- Generate TC CS files ---------------------------------
		
		foreach($TC in $VSTSWorkItems["TestCases"]){
			$id = $TC["Id"]
			$name = $TC["Tite"]
			
			$testcaseFileRelative = [System.IO.Path]::Combine($FeatureFolderPath,  "TC_$($id)\TC_$($id).cs")
			$testcaseFile = [System.IO.Path]::Combine($testsFolder, "TC_$($id)\TC_$($id).cs")
			if([System.IO.File]::Exists($testcaseFile)){
				throw "Testcase Steps already exists for this user story, please either delete or change user story id"
			}

			$fileContent=@()
			$fileContent+=@"
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.UI.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace MC.Track.TestSuite.UI.$($namespaceSuffix)
{
	[Binding]
	[Scope(Tag = "workitem:$($id)")]
	public class TC_$($id)_Implementation : BaseTestStep
	{
	
		#region Constructor
		public TC_$($id)_Implementation(IResolver resolver) : base(resolver)
		{
			// 
			// DO NOT PUT ANY CODE IN THIS SECTION
			//
		}
		#endregion			
		
		#region Test Case Data		
		/// <summary>
        /// Is Run before the test case and is meant to setup the test case data
        /// </summary>
        [Before]
        public void SetupTestData()
        {
            Db.TC_$($id)Setup();
        }

        /// <summary>
        /// Is Run after the test case and is meant to tear down the test case data
        /// </summary>
        [After]
		public void TeardownTestData()
		{
			Db.TC_$($id)Teardown();
		}
		#endregion				

		#region Test Powershell Scripting
		/// <summary>
        /// Is Run before the test case and is meant to setup the test case data
        /// </summary>
        [Before]
        public void SetupTestPowershellScripting()
        {
            PS.TC_$($id)Setup();
        }

        /// <summary>
        /// Is Run after the test case and is meant to tear down the test case data
        /// </summary>
        [After]
		public void TeardownTestPowershellScripting()
		{
			PS.TC_$($id)Teardown();
		}
		#endregion		


"@
			$distinctSteps = @()
			$TC["Steps"] | Foreach {
				$orgStep = $_
				if(-not ($distinctSteps | Where { $_.Type -eq $orgStep.Type -and $_.Text -eq $orgStep.Text }))
				{
					$distinctSteps += $orgStep
				}
			}
			
			foreach($Step in $distinctSteps){
			
				# We want to skip shared steps because we want to implment the running inside of the shared step class
				if($Step["IsSharedStep"]){
					continue
				}
				
				$stepType = $Step["Type"]
				$stepText = CleanStepForStepClass $Step["Text"]
				$camleText = ConvertStepToToUseForTestMethod $Step
				# if($Step["Common"]){
				# 	continue
				# }
				
				if($Step["Type"] -eq "When"){
					$fileContent+="`t`t[When(@`"$($stepText)`")]"
					$fileContent+="`t`tpublic void $($camleText)()"
					$fileContent+="`t`t{"
					$fileContent+="`t`t`t// Remove when ready to implment"
					$fileContent+="`t`t`tScenarioContext.Current.Pending();"
					$fileContent+="`t`t`t"
					$fileContent+="`t`t`t// Add Code to implment here"
					$fileContent+="`t`t}"
					continue
				}
				if($Step["Type"] -eq "Then"){
					$fileContent+="`t`t[Then(@`"$($stepText)`")]"
					$fileContent+="`t`tpublic void $($camleText)()"
					$fileContent+="`t`t{"
					$fileContent+="`t`t`t// Remove when ready to implment"
					$fileContent+="`t`t`tScenarioContext.Current.Pending();"
					$fileContent+="`t`t`t"
					$fileContent+="`t`t`t// Add Code to implment here"
					$fileContent+="`t`t}"
					continue
				}
				if($Step["Type"] -eq "Given"){
					$fileContent+="`t`t[Given(@`"$($stepText)`")]"
					$fileContent+="`t`tpublic void $($camleText)()"
					$fileContent+="`t`t{"
					$fileContent+="`t`t`t// Remove when ready to implment"
					$fileContent+="`t`t`tScenarioContext.Current.Pending();"
					$fileContent+="`t`t`t"
					$fileContent+="`t`t`t// Add Code to implment here"
					$fileContent+="`t`t}"
					continue
				}
				if($Step["Type"] -eq "Unkown"){
					$fileContent+="`t// Unkown: $($stepText)"
					continue
				}
				throw "Unkown Step type '$($stepType)'"
			}
			$fileContent+=""
			
			$fileContent+="`t}"
			$fileContent+="}"
			$newFile = @{}	

			$csFile         = ([System.IO.Path]::ChangeExtension($testcaseFile,".Implementation.cs"))
			$csRelativeFile = ([System.IO.Path]::ChangeExtension($testcaseFileRelative,".Implementation.cs"))
			$featureFile    = ([System.IO.Path]::GetFileName(([System.IO.Path]::ChangeExtension($testcaseFileRelative,".feature"))))
			$newFile["FileContent"] = $fileContent
			$newFile["FilePath"] = $csFile
			$newFile["Nodes"] = @()
			GenerateCompileItemWithDependentXMLNodeForProj $newFile $project $csRelativeFile $featureFile
			
			$FileContents += $newFile
		}
		# --------------------------- Generate TC CS files ---------------------------------
		
		# --------------------------- Generate TC Sql files ---------------------------------
		
		foreach($TC in $VSTSWorkItems["TestCases"]){
			$id = $TC["Id"]
			$name = $TC["Tite"]
			
			$testcaseFileRelativeDataFolder = [System.IO.Path]::Combine($FeatureFolderPath,  "TC_$($id)\Data")
			$testcaseFileDataFolder = [System.IO.Path]::Combine($testsFolder, "TC_$($id)\Data")
			
			 if(-not [System.IO.Directory]::Exists($testcaseFileDataFolder)){
                #md $featureFileRootFolder
                [System.IO.Directory]::CreateDirectory($testcaseFileDataFolder)
                
            }
			
			$testcaseFileRelative = [System.IO.Path]::Combine($testcaseFileRelativeDataFolder,  "TC_$($id)Setup.sql")
			$testcaseFile = [System.IO.Path]::Combine($testcaseFileDataFolder, "TC_$($id)Setup.sql")

			if([System.IO.File]::Exists($testcaseFile)){
				throw "Testcase Steps already exists for this user story, please either delete or change user story id"
			}

			$fileContent=@()
			$fileContent+=@"
DECLARE @InputVariable VARCHAR(100) = '`$(InputVariable)'
"@

			
			$fileContent+=""
			
			$newFile = @{}	
			$newFile["FileContent"] = $fileContent
			$newFile["FilePath"] = $testcaseFile
			$newFile["AfterCreatingFile"] = {
				Param(
					[string] $fileFullPath
				)
				&$generateDBScriptsFile $testProjFile $fileFullPath
			}
			$newFile["Nodes"] = @()
			GenerateNormalFileXMLNodeForProj $newFile $project $testcaseFileRelative
			GenerateCompileItemWithDependentXMLNodeForProj  $newFile $project ([System.IO.Path]::ChangeExtension($testcaseFileRelative,".sql.cs")) "$(([System.IO.Path]::GetFileNameWithoutExtension($testcaseFileRelative))).sql"
			$FileContents += $newFile
			
			
			# --------------------
			
			$testcaseFileRelative = [System.IO.Path]::Combine($testcaseFileRelativeDataFolder,  "TC_$($id)Teardown.sql")
			$testcaseFile = [System.IO.Path]::Combine($testcaseFileDataFolder, "TC_$($id)Teardown.sql")

			if([System.IO.File]::Exists($testcaseFile)){
				throw "Testcase Steps already exists for this user story, please either delete or change user story id"
			}

			$fileContent=@()
			$fileContent+=@"
DECLARE @InputVariable VARCHAR(100) = '`$(InputVariable)'
"@

			
			$fileContent+=""
			
			$newFile = @{}	
			$newFile["FileContent"] = $fileContent
			$newFile["FilePath"] = $testcaseFile
			$newFile["AfterCreatingFile"] = {
				Param(
					[string] $fileFullPath
				)
				&$generateDBScriptsFile $testProjFile $fileFullPath
			}
			$newFile["Nodes"] = @()
			GenerateNormalFileXMLNodeForProj $newFile $project $testcaseFileRelative
			GenerateCompileItemWithDependentXMLNodeForProj  $newFile $project ([System.IO.Path]::ChangeExtension($testcaseFileRelative,".sql.cs")) "$(([System.IO.Path]::GetFileNameWithoutExtension($testcaseFileRelative))).sql"
			$FileContents += $newFile
			
			# --------------------
			
			$testcaseFileRelative = [System.IO.Path]::Combine($testcaseFileRelativeDataFolder,  "Readme.md")
			$testcaseFile = [System.IO.Path]::Combine($testcaseFileDataFolder, "Readme.md")

			if([System.IO.File]::Exists($testcaseFile)){
				throw "Testcase Steps already exists for this user story, please either delete or change user story id"
			}

			$fileContent=@()
			$fileContent+=@"
# SQL File Usage

How the sql file is intergrated into the framework and how it can be leveraged

## Scripting

Their is nothing different here, write your sql like you know how...

The only addition that can be leveraged are parameters. 

### Example 

Assuming your file '```TC_$($id)Setup.sql```' looks like this
```sql
DECLARE @SomeParameter VARCHAR(200) = '`$(SomeParameter)'
```

You can then use the sql like so
```c#
[Before]
public void DataSetup(){
    Db.TC_$($id)Setup( SomeParameter = "Some parameter passed in from C#");
}
```
"@

			$fileContent+=""
			
			$newFile = @{}	
			$newFile["FileContent"] = $fileContent
			$newFile["FilePath"] = $testcaseFile
			$newFile["Nodes"] = @()
			GenerateUnImportantFileXMLNodeForProj $newFile $project $testcaseFileRelative
			
			
			$FileContents += $newFile
			

			
			
			
		}
		
# --------------------------- Generate TC Ps1 files ---------------------------------
		
		foreach($TC in $VSTSWorkItems["TestCases"]){
			$id = $TC["Id"]
			$name = $TC["Tite"]
			
			$testcaseFileRelativeDataFolder = [System.IO.Path]::Combine($FeatureFolderPath,  "TC_$($id)\PowerShell")
			$testcaseFileDataFolder = [System.IO.Path]::Combine($testsFolder, "TC_$($id)\PowerShell")
			
			 if(-not [System.IO.Directory]::Exists($testcaseFileDataFolder)){
                #md $featureFileRootFolder
                [System.IO.Directory]::CreateDirectory($testcaseFileDataFolder)
                
            }
			
			$testcaseFileRelative = [System.IO.Path]::Combine($testcaseFileRelativeDataFolder,  "TC_$($id)Setup.ps1")
			$testcaseFile = [System.IO.Path]::Combine($testcaseFileDataFolder, "TC_$($id)Setup.ps1")

			if([System.IO.File]::Exists($testcaseFile)){
				throw "Testcase Steps already exists for this user story, please either delete or change user story id"
			}

			$fileContent=@()
			$fileContent+=@"
Param(
	[string] $InputVariable = "`$(InputVariable)"
)
"@

			
			$fileContent+=""
			
			$newFile = @{}	
			$newFile["FileContent"] = $fileContent
			$newFile["FilePath"] = $testcaseFile
			$newFile["AfterCreatingFile"] = {
				Param(
					[string] $fileFullPath
				)
				&$generatePSScriptsFile $testProjFile $fileFullPath
			}
			$newFile["Nodes"] = @()
			GenerateNormalFileXMLNodeForProj $newFile $project $testcaseFileRelative
			GenerateCompileItemWithDependentXMLNodeForProj  $newFile $project ([System.IO.Path]::ChangeExtension($testcaseFileRelative,".ps1.cs")) "$(([System.IO.Path]::GetFileNameWithoutExtension($testcaseFileRelative))).ps1"
			
			$FileContents += $newFile
			
			
			# --------------------
			
			$testcaseFileRelative = [System.IO.Path]::Combine($testcaseFileRelativeDataFolder,  "TC_$($id)Teardown.ps1")
			$testcaseFile = [System.IO.Path]::Combine($testcaseFileDataFolder, "TC_$($id)Teardown.ps1")

			if([System.IO.File]::Exists($testcaseFile)){
				throw "Testcase Steps already exists for this user story, please either delete or change user story id"
			}

			$fileContent=@()
			$fileContent+=@"
Param(
	[string] $InputVariable = "`$(InputVariable)"
)
"@

			
			$fileContent+=""
			
			$newFile = @{}	
			$newFile["FileContent"] = $fileContent
			$newFile["FilePath"] = $testcaseFile
			$newFile["AfterCreatingFile"] = {
				Param(
					[string] $fileFullPath
				)
				&$generatePSScriptsFile $testProjFile $fileFullPath
			}
			$newFile["Nodes"] = @()
			GenerateNormalFileXMLNodeForProj $newFile $project $testcaseFileRelative
			GenerateCompileItemWithDependentXMLNodeForProj  $newFile $project ([System.IO.Path]::ChangeExtension($testcaseFileRelative,".ps1.cs")) "$(([System.IO.Path]::GetFileNameWithoutExtension($testcaseFileRelative))).ps1"
			$FileContents += $newFile
			
			# --------------------
			
			$testcaseFileRelative = [System.IO.Path]::Combine($testcaseFileRelativeDataFolder,  "Readme.md")
			$testcaseFile = [System.IO.Path]::Combine($testcaseFileDataFolder, "Readme.md")

			if([System.IO.File]::Exists($testcaseFile)){
				throw "Testcase Steps already exists for this user story, please either delete or change user story id"
			}

			$fileContent=@()
			$fileContent+=@"
# PS1 File Usage

How the ps1 file is intergrated into the framework and how it can be leveraged

## Scripting

Their is nothing different here, write your ps1 like you know how...

The only addition that can be leveraged are parameters. 

### Example 

Assuming your file '```TC_$($id)Setup.ps1```' looks like this
```sql
Param(
	[string] $SomeParameter = "`$(SomeParameter)"
)
```

You can then use the sql like so
```c#
[Before]
public void PS1Setup(){
    PS.TC_$($id)Setup( SomeParameter = "Some parameter passed in from C#");
}
```
"@

			$fileContent+=""
			
			$newFile = @{}	
			$newFile["FileContent"] = $fileContent
			$newFile["FilePath"] = $testcaseFile
			
			$newFile["Nodes"] = @()
			GenerateUnImportantFileXMLNodeForProj $newFile $project $testcaseFileRelative
			
			
			$FileContents += $newFile
		}	

		foreach($TC in $VSTSWorkItems["TestCases"]){
			$id = $TC["Id"]
			$name = $TC["Tite"]
			
			
			$testcaseFileRelative = [System.IO.Path]::Combine($FeatureFolderPath,  "TC_$($id)\TC_$($id).cs")
			$testcaseFile = [System.IO.Path]::Combine($testsFolder, "TC_$($id)\TC_$($id).cs")

			if([System.IO.File]::Exists($testcaseFile)){
				throw "Testcase Steps already exists for this user story, please either delete or change user story id"
			}

			$fileContent=@()
			$fileContent+=@"
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.UI.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace MC.Track.TestSuite.UI
{
	public partial class Keywords
	{
	
		// public void NewKeyword(){
		// 
		// }
	}
}
"@

			
			$fileContent+=""
			$csFile          = ([System.IO.Path]::ChangeExtension($testcaseFile,".Keywords.cs"))
			$csFileRelative  = ([System.IO.Path]::ChangeExtension($testcaseFileRelative,".Keywords.cs"))
			$featureFile     = ([System.IO.Path]::GetFileName(([System.IO.Path]::ChangeExtension($testcaseFileRelative,".feature"))))
			$newFile = @{}	
			$newFile["FileContent"] = $fileContent
			$newFile["FilePath"] = $csFile
			$newFile["Nodes"] = @()
			GenerateCompileItemWithDependentXMLNodeForProj $newFile $project $csFileRelative $featureFile
			
			$FileContents += $newFile
			
			
		}			
		# --------------------------- Generate TC CS files ---------------------------------
		
		
		# --------------------------- Generate SharedSteps Feature file ---------------------------------
		
		$featureFileContent = @()
		
		foreach($sharedSteps in $VSTSWorkItems["SharedSteps"]){
		
			$testTitle = $sharedSteps['Title']
			$testID    = $sharedSteps['Id']
			
			
			
			$sharedStepRootFolder 		  = [System.IO.Path]::Combine($sharedStepFolder,  "SS_$($testID)")
			$sharedStepRootRelativeFolder = [System.IO.Path]::Combine($sharedStepRelativeFolder,  "SS_$($testID)")
			if(-not [System.IO.Directory]::Exists($sharedStepRootFolder)){
				[System.IO.Directory]::CreateDirectory($sharedStepRootFolder)
			}
			
			$featureSharedCsRelativeFile = [System.IO.Path]::Combine($sharedStepRootRelativeFolder, "SS_$($testID).SharedSteps.feature.cs")
			$featureSharedRelativeFile = [System.IO.Path]::Combine($sharedStepRootRelativeFolder, "SS_$($testID).SharedSteps.feature")
			$featureSharedCsFile = [System.IO.Path]::Combine($sharedStepRootFolder, "SS_$($testID).SharedSteps.feature.cs")
			$featureSharedFile = [System.IO.Path]::Combine($sharedStepRootFolder, "SS_$($testID).SharedSteps.feature")
			

		    if([System.IO.File]::Exists($featureSharedFile)){
			    throw "Feature already exists for this user story, please either delete or change user story id"
		    }
			if([System.IO.File]::Exists($featureSharedCsFile)){
			    throw "Feature already exists for this user story, please either delete or change user story id"
		    }
			
			$featureFileContent = @()
			$featureFileContent += "Feature: Shared Step $($testID)"
			$featureFileContent += ""
			$featureFileContent += ""
			$featureFileContent += ""
			$featureFileContent += "@$($testName)"
			$featureFileContent += "@SS_$($testID)"
            $featureFileContent += "@workitem:$($testID)"
			$featureFileContent += "Scenario: $testTitle"
			
			$newLine = $false
			foreach($step in $sharedSteps["Steps"])
			{
				
				
				$stepType    = $step['Type']
				$stepContent = $step['Text']
				$featureFileContent += "   $($stepType) $($stepContent)"
				
				if($newLine) { $featureFileContent += "" }
				
				$newLine = $stepType -eq "When"
			}
			
			$featureFileContent -join "`r`n"
			
			$newFile = @{}	
			$newFile["FileContent"] = $featureFileContent
			$newFile["FilePath"] = $featureSharedFile
			$newFile["Nodes"] = @()
			GenerateFeatureFileXMLNodeForProj $newFile $project $featureSharedCsRelativeFile ([System.IO.Path]::GetFileName($featureSharedCsFile)) $featureSharedRelativeFile
			
			$FileContents += $newFile
		}
		
		
		
		# --------------------------- Generate SharedSteps Feature file ---------------------------------
		
		
		
		
		# --------------------------- Generate SharedSteps CS files ---------------------------------
		
		foreach($TC in $VSTSWorkItems["SharedSteps"]){
			$id = $TC["Id"]
			$name = $TC["Tite"]
			
			$testcaseFileRelative = [System.IO.Path]::Combine($sharedStepRelativeFolder,  "SS_$($id)\SS_$($id).cs")
			$testcaseFile = [System.IO.Path]::Combine($sharedStepFolder, "SS_$($id)\SS_$($id).cs")
			if([System.IO.File]::Exists($testcaseFile)){
				throw "Testcase Steps already exists for this user story, please either delete or change user story id"
			}

			$fileContent=@()
			$fileContent+=@"
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.UI.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace MC.Track.TestSuite.UI.$($namespaceSuffix).Shared
{
	[Binding]
	[Scope(Tag = "workitem:$($id)")]
	public class SS_$($id)_Implementation : BaseTestStep
	{
	
		#region Contructor
		public SS_$($id)_Implementation(IResolver resolver) : base(resolver)
		{
			// 
			// DO NOT PUT ANY CODE IN THIS SECTION
			//
		}
		#endregion			
		
		#region Test Case Data		
		/// <summary>
        /// Is Run before the test case and is meant to setup the test case data
        /// </summary>
        [Before]
        public void SetupTestData()
        {
            Db.SS_$($id)Setup();
        }

        /// <summary>
        /// Is Run after the test case and is meant to tear down the test case data
        /// </summary>
        [After]
		public void TeardownTestData()
		{
			Db.SS_$($id)Teardown();
		}
		#endregion				

		#region Test Powershell Scripting
		/// <summary>
        /// Is Run before the test case and is meant to setup the test case data
        /// </summary>
        [Before]
        public void SetupTestPowershellScripting()
        {
            PS.SS_$($id)Setup();
        }

        /// <summary>
        /// Is Run after the test case and is meant to tear down the test case data
        /// </summary>
        [After]
		public void TeardownTestPowershellScripting()
		{
			PS.SS_$($id)Teardown();
		}
		#endregion		


"@
			$distinctSteps = @()
			$TC["Steps"] | Foreach {
				$orgStep = $_
				if(-not ($distinctSteps | Where { $_.Type -eq $orgStep.Type -and $_.Text -eq $orgStep.Text }))
				{
					$distinctSteps += $orgStep
				}
			}
			
			foreach($Step in $distinctSteps){
			
				# We want to skip shared steps because we want to implment the running inside of the shared step class
				# if($Step["IsSharedStep"]){
				# 	continue
				# }
				
				$stepType = $Step["Type"]
				$stepText = CleanStepForStepClass $Step["Text"]
				$camleText = ConvertStepToToUseForTestMethod $Step
				# if($Step["Common"]){
				# 	continue
				# }
				
				if($Step["Type"] -eq "When"){
					$fileContent+="`t`t[When(@`"$($stepText)`")]"
					$fileContent+="`t`tpublic void $($camleText)()"
					$fileContent+="`t`t{"
					$fileContent+="`t`t`t// Remove when ready to implment"
					$fileContent+="`t`t`tScenarioContext.Current.Pending();"
					$fileContent+="`t`t`t"
					$fileContent+="`t`t`t// Add Code to implment here"
					$fileContent+="`t`t}"
					continue
				}
				if($Step["Type"] -eq "Then"){
					$fileContent+="`t`t[Then(@`"$($stepText)`")]"
					$fileContent+="`t`tpublic void $($camleText)()"
					$fileContent+="`t`t{"
					$fileContent+="`t`t`t// Remove when ready to implment"
					$fileContent+="`t`t`tScenarioContext.Current.Pending();"
					$fileContent+="`t`t`t"
					$fileContent+="`t`t`t// Add Code to implment here"
					$fileContent+="`t`t}"
					continue
				}
				if($Step["Type"] -eq "Given"){
					$fileContent+="`t`t[Given(@`"$($stepText)`")]"
					$fileContent+="`t`tpublic void $($camleText)()"
					$fileContent+="`t`t{"
					$fileContent+="`t`t`t// Remove when ready to implment"
					$fileContent+="`t`t`tScenarioContext.Current.Pending();"
					$fileContent+="`t`t`t"
					$fileContent+="`t`t`t// Add Code to implment here"
					$fileContent+="`t`t}"
					continue
				}
				if($Step["Type"] -eq "Unkown"){
					$fileContent+="`t// Unkown: $($stepText)"
					continue
				}
				throw "Unkown Step type '$($stepType)'"
			}
			$fileContent+=""
			
			$fileContent+="`t}"
			$fileContent+="}"
			$csFile          = ([System.IO.Path]::ChangeExtension($testcaseFile,".SharedSteps.Implementation.cs"))
			$csFileRelative  = ([System.IO.Path]::ChangeExtension($testcaseFileRelative,".SharedSteps.Implementation.cs"))
			$featureFile     = ([System.IO.Path]::GetFileName(([System.IO.Path]::ChangeExtension($testcaseFileRelative,".SharedSteps.feature"))))
			$newFile = @{}	
			$newFile["FileContent"] = $fileContent
			$newFile["FilePath"] = $csFile
			
			$newFile["Nodes"] = @()
			GenerateCompileItemWithDependentXMLNodeForProj $newFile $project $csFileRelative $featureFile

			$FileContents += $newFile
		}
		
		# --------------------------- Generate SharedSteps setup sql files ---------------------------------
		
		
		# --------------------------- Generate SharedSteps teardown sql files ---------------------------------
		foreach($TC in $VSTSWorkItems["SharedSteps"]){
			$id = $TC["Id"]
			$name = $TC["Tite"]
			
			
			$sharedStepRelativeDataFolder = [System.IO.Path]::Combine($sharedStepRelativeFolder,  "SS_$($id)\Data")
			$sharedStepDataFolder = [System.IO.Path]::Combine($sharedStepFolder, "SS_$($id)\Data")
			
			if(-not [System.IO.Directory]::Exists($sharedStepDataFolder)){
                #md $featureFileRootFolder
                [System.IO.Directory]::CreateDirectory($sharedStepDataFolder)
            }

			$testcaseFileRelative = [System.IO.Path]::Combine($sharedStepRelativeDataFolder,  "SS_$($id)Teardown.sql")
			$testcaseFile = [System.IO.Path]::Combine($sharedStepDataFolder, "SS_$($id)Teardown.sql")
			if([System.IO.File]::Exists($testcaseFile)){
				throw "Testcase Steps already exists for this user story, please either delete or change user story id"
			}

			$fileContent=@()
			$fileContent+=@"
-- Sql Statments here
"@
			$distinctSteps = @()
			$TC["Steps"] | Foreach {
				$orgStep = $_
				if(-not ($distinctSteps | Where { $_.Type -eq $orgStep.Type -and $_.Text -eq $orgStep.Text }))
				{
					$distinctSteps += $orgStep
				}
			}
			
			$fileContent+=""
			$newFile = @{}	
			$newFile["FileContent"] = $fileContent
			$newFile["FilePath"] = $testcaseFile
			$newFile["AfterCreatingFile"] = {
				Param(
					[string] $fileFullPath
				)
				&$generateDBScriptsFile $testProjFile $fileFullPath
			}
			$newFile["Nodes"] = @()
			GenerateNormalFileXMLNodeForProj $newFile $project $testcaseFileRelative
			GenerateCompileItemWithDependentXMLNodeForProj  $newFile $project ([System.IO.Path]::ChangeExtension($testcaseFileRelative,".sql.cs")) "$(([System.IO.Path]::GetFileNameWithoutExtension($testcaseFileRelative))).sql"

			$FileContents += $newFile

			# ----------------------------------------------- 

			$testcaseFileRelative = [System.IO.Path]::Combine($sharedStepRelativeDataFolder,  "SS_$($id)Setup.sql")
			$testcaseFile = [System.IO.Path]::Combine($sharedStepDataFolder, "SS_$($id)Setup.sql")
			if([System.IO.File]::Exists($testcaseFile)){
				throw "Testcase Steps already exists for this user story, please either delete or change user story id"
			}

			$fileContent=@()
			$fileContent+=@"
-- Sql Statments here
"@
			
			$fileContent+=""
			$newFile = @{}	
			$newFile["FileContent"] = $fileContent
			$newFile["FilePath"] = $testcaseFile
			$newFile["AfterCreatingFile"] = {
				Param(
					[string] $fileFullPath
				)
				&$generateDBScriptsFile $testProjFile $fileFullPath
			}
			$newFile["Nodes"] = @()
			GenerateNormalFileXMLNodeForProj $newFile $project $testcaseFileRelative
			GenerateCompileItemWithDependentXMLNodeForProj  $newFile $project ([System.IO.Path]::ChangeExtension($testcaseFileRelative,".sql.cs")) "$(([System.IO.Path]::GetFileNameWithoutExtension($testcaseFileRelative))).sql"

			$FileContents += $newFile









			#############################################






			$sharedStepRelativeDataFolder = [System.IO.Path]::Combine($sharedStepRelativeFolder,  "SS_$($id)\PowerShell")
			$sharedStepDataFolder = [System.IO.Path]::Combine($sharedStepFolder, "SS_$($id)\PowerShell")
			
			if(-not [System.IO.Directory]::Exists($sharedStepDataFolder)){
                #md $featureFileRootFolder
                [System.IO.Directory]::CreateDirectory($sharedStepDataFolder)
            }

			$testcaseFileRelative = [System.IO.Path]::Combine($sharedStepRelativeDataFolder,  "SS_$($id)Teardown.ps1")
			$testcaseFile = [System.IO.Path]::Combine($sharedStepDataFolder, "SS_$($id)Teardown.ps1")
			if([System.IO.File]::Exists($testcaseFile)){
				throw "Testcase Steps already exists for this user story, please either delete or change user story id"
			}

			$fileContent=@()
			$fileContent+=@"
Param()
"@
			
			$fileContent+=""
			$newFile = @{}	
			$newFile["FileContent"] = $fileContent
			$newFile["FilePath"] = $testcaseFile
			$newFile["AfterCreatingFile"] = {
				Param(
					[string] $fileFullPath
				)
				&$generatePSScriptsFile $testProjFile $fileFullPath
			}
			$newFile["Nodes"] = @()
			GenerateNormalFileXMLNodeForProj $newFile $project $testcaseFileRelative
			GenerateCompileItemWithDependentXMLNodeForProj  $newFile $project ([System.IO.Path]::ChangeExtension($testcaseFileRelative,".ps1.cs")) "$(([System.IO.Path]::GetFileNameWithoutExtension($testcaseFileRelative))).ps1"

			$FileContents += $newFile

			# ----------------------------------------------- 

			$testcaseFileRelative = [System.IO.Path]::Combine($sharedStepRelativeDataFolder,  "SS_$($id)Setup.ps1")
			$testcaseFile = [System.IO.Path]::Combine($sharedStepDataFolder, "SS_$($id)Setup.ps1")
			if([System.IO.File]::Exists($testcaseFile)){
				throw "Testcase Steps already exists for this user story, please either delete or change user story id"
			}

			$fileContent=@()
			$fileContent+=@"
Param()
"@
			
			$fileContent+=""
			$newFile = @{}	
			$newFile["FileContent"] = $fileContent
			$newFile["FilePath"] = $testcaseFile
			$newFile["AfterCreatingFile"] = {
				Param(
					[string] $fileFullPath
				)
				&$generatePSScriptsFile $testProjFile $fileFullPath
			}
			$newFile["Nodes"] = @()
			GenerateNormalFileXMLNodeForProj $newFile $project $testcaseFileRelative
			GenerateCompileItemWithDependentXMLNodeForProj  $newFile $project ([System.IO.Path]::ChangeExtension($testcaseFileRelative,".ps1.cs")) "$(([System.IO.Path]::GetFileNameWithoutExtension($testcaseFileRelative))).ps1"

			$FileContents += $newFile


			# ----------------------------------------------- 

			$testcaseFileRelative = [System.IO.Path]::Combine($sharedStepRelativeFolder,  "SS_$($id)\SS_$($id).cs")
			$testcaseFile = [System.IO.Path]::Combine($sharedStepFolder, "SS_$($id)\SS_$($id).cs")
			if([System.IO.File]::Exists($testcaseFile)){
				throw "Testcase Steps already exists for this user story, please either delete or change user story id"
			}

			$fileContent=@()
			$fileContent+=@"
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.UI.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace MC.Track.TestSuite.UI
{
	public partial class Keywords
	{
	
		// public void NewKeyword(){
		// 
		// }
	}
}
"@

			
			$fileContent+=""
			$csFile          = ([System.IO.Path]::ChangeExtension($testcaseFile,".SharedSteps.Keywords.cs"))
			$csFileRelative  = ([System.IO.Path]::ChangeExtension($testcaseFileRelative,".SharedSteps.Keywords.cs"))
			$featureFile     = ([System.IO.Path]::GetFileName(([System.IO.Path]::ChangeExtension($testcaseFileRelative,".SharedSteps.feature"))))

			$newFile = @{}	
			$newFile["FileContent"] = $fileContent
			$newFile["FilePath"] = $csFile
			$newFile["Nodes"] = @()
			GenerateCompileItemWithDependentXMLNodeForProj $newFile $project $csFileRelative $featureFile

			$FileContents += $newFile




		}
		
		# --------------------------- Generate SharedStep CS files ---------------------------------
		
		
		foreach($fileContent in $FileContents){
		
			foreach($nodeToAdd in $fileContent["Nodes"]){
				$node.AppendChild($nodeToAdd)
			}
			
			if($fileContent["BeforeCreatingFile"]){
				&$fileContent["BeforeCreatingFile"] $fileContent["FilePath"]
			}
			
			$fileContent["FileContent"] | Set-Content $fileContent["FilePath"]
			
			if($fileContent["AfterCreatingFile"]){
				&$fileContent["AfterCreatingFile"] $fileContent["FilePath"]
			}
		}

		$project.Save($testProjFile)
		
		$projectContent = Get-Content $testProjFile
		$projectContent = $projectContent -replace 'xmlns=""',''
		$projectContent | Set-Content $testProjFile
	}
	
	
	
	
	
}



