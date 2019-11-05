Function Run-Command{
	Param(
		[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
		[ref][XML]$runNode2
	)
	
	$runNode=[ref]($runNode2.Value.Clone())
	
	if(-not (PerformXMLEnvironmentVarReplacment ($runNode))){
		Throw "Unable to paramerterlize all variables in Running Node"
		break
	}
	
	$print = {
		Param(
			[string] $color
		)
		if($taskGroup.Value.Description){
			
		}
		else{
			Write-Host ": $($taskGroup.Value.Name)" -ForegroundColor $color
		}
	}
	if($runNode.Value.Condition){
		# Write-Host "+ Condition: $($runNode.Value.Condition)" -ForegroundColor Magenta
		if(-not $(iex $runNode.Value.Condition)){
			Write-Host ": $($runNode.Value.TaskGroupName) Skipped" -ForegroundColor Yellow
			return $true
		}
	}
	if($runNode.Value.TaskGroupName){
		
		Write-Host "Found Task Group '$($runNode.Value.TaskGroupName)'"
		$taskGroupName=$runNode.Value.TaskGroupName
		$taskGroupFound = $runNode2.Value.SelectSingleNode("../TaskGroup[@Name='$($taskGroupName)']")
		
		return Run-TaskGroup $([ref]$taskGroupFound) $($runNode)
	}
	elseif($runNode.Value.TaskGroup -and $runNode.Value.Task){
		Write-Host "Having both Task Groups and Task as children of a run command is not allowed" -ForegroundColor Red
		return $false
	}
	elseif($runNode.Value.TaskGroup){
		$isPassed=$true
		foreach($taskGroup in $runNode.Value.TaskGroup){
			
			$isPassed = $isPassed -and $(Run-TaskGroup $([ref]$taskGroup) $($runNode))
		}
		return $isPassed
	}
	elseif($runNode.Value.Task){
		
		$isPassed=$true
		foreach($task in $runNode.Value.Task){
			
			$isPassed = $isPassed -and $(Run-Task $([ref]$task) $([ref]$runNode))
			if(-not $isPassed){
				break
			}
		}
		return $isPassed
	}
	else{
		Write-Host "Nothing to run" -ForegroundColor Red
	}
	return $false
}
Export-ModuleMember -Function Run-Command
Function Run-TaskGroup{
	Param(
		[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
		[ref][XML]$taskGroup2,
		[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
		[ref][XML]$runNode
	)
	$taskGroup=[ref]($taskGroup2.Value.Clone())
	
	$print = {
		Param(
			[string] $color
		)
		if($taskGroup.Value.Description){
			Write-Host ": $($taskGroup.Value.Description)" -ForegroundColor $color
		}
		else{
			Write-Host ": $($taskGroup.Value.Name)" -ForegroundColor $color
		}
	}
	if(-not (PerformXMLEnvironmentVarReplacment ($taskGroup))){
		Throw "Unable to paramerterlize all variables in task group"
		break
	}
	if($taskGroup.Value.Condition){
		# Write-Host "+ Condition: $($taskGroup.Value.Condition)" -ForegroundColor Magenta
		if(-not $(iex $taskGroup.Value.Condition)){
			Write-Host ": $($taskGroup.Value.Name) Skipped" -ForegroundColor Yellow
			return $true
		}
	}
	
	$passed=$true
	foreach($task in $taskGroup2.Value.Task){
		if(-not (Run-Task $([ref]$task) $($runNode))){
			$passed = $false
		}
		if(-not $passed){
			break
		}
	}
	
	if(-not $passed){
		&$print -color "Red"
		return $false 
	}	
	
	&$print -color "Green"
	return $true
	
}
Export-ModuleMember -Function Run-TaskGroup
Function Run-Task{
	Param(
		[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
		[ref][XML]$task2,
		[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
		[ref][XML]$runNode
	)
	
	$task=[ref]($task2.Value.Clone())
	if(-not (PerformXMLEnvironmentVarReplacment $task $true)){
		Throw "Unable to paramerterlize all variables in task"
		break
	}
	if($task.Value.Condition){
		# Write-Host "+ Condition: $($task.Value.Condition)" -ForegroundColor Magenta
		if(-not $(iex $task.Value.Condition)){
			Write-Host ": $($task.Value.Name) Skipped" -ForegroundColor Yellow
			return $true
		}
	}
	if(-not $taskTypes[$task.Value.Type]){
		Write-Host "Unkown Task type '$($task.Value.Type)'" -ForegroundColor Red
		return $false
	}
	
	try{
		&$taskTypes[$task.Value.Type] $task $task2 $runNode
		return $true
	}
	catch{
		Write-Host "Task Failed $($_.Exception.Message)" -ForegroundColor Red
		return $false
	}
	
}
Export-ModuleMember -Function Run-Task
Function Get-MyIP{
	return Invoke-RestMethod http://ipinfo.io/json | Select -exp ip
}
Export-ModuleMember -Function Get-MyIP
Function AppService-AddMyIPToRestrictions{
	 Param(   
		[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
		[string] $ResourceGroup,
		[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
		[string] $ResourceName
	)

	Write-Host "Adding your IP to resource ($($ResourceName))"
	$resource = Get-AzureRmResource -ResourceGroupName $ResourceGroup -ResourceType Microsoft.Web/sites/config -ResourceName "$($ResourceName)/web" -ApiVersion 2016-03-01
	if($resource){
	
		# Properties
		$properties =$resource.Properties

		# Get my IP
		$myIP = Get-MyIP

		# Add IP/Or Not
		$myIPFound = $properties.ipSecurityRestrictions | where ipAddress -eq $myIP
		if($myIPFound) {
			Write-Host "   Your IP was already added" -ForegroundColor Cyan
		}
		else{
			$restriction = @{}
			$restriction.Add("ipAddress",$myIP)
			$properties.ipSecurityRestrictions += $restriction

			$test = Set-AzureRmResource -ResourceGroupName $ResourceGroup -ResourceType Microsoft.Web/sites/config -ResourceName "$($ResourceName)/web" -ApiVersion 2016-03-01 -PropertyObject $properties -Force
			Write-Host "   Successfully Added $myIP" -ForegroundColor Green
		}
	}
	else{
		Write-Error "Resource $($ResourceName) was not found"
	}
}
Export-ModuleMember -Function AppService-AddMyIPToRestrictions

function SQLServer-AddMyIPToFirewall{
	Param(
		[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
		[string] $ResourceGroup,
		[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
		[string] $ResourceName
	)
	Write-Host "Adding your IP to firewall of SQL server ($($ResourceName))"
	$databaseFirewallRules = Get-AzureRmSqlServerFirewallRule -ResourceGroupName $ResourceGroup -ServerName $ResourceName
	if($databaseFirewallRules){
		$filteredRules = ($databaseFirewallRules | Where {$_.FirewallRuleName -eq $env:USERNAME})
		# Get my IP
		$myIP = Get-MyIP

		if($filteredRules.Count){

			if($filteredRules[0].StartIpAddress -eq $myIP){
				Write-Host "   Your IP was already added" -ForegroundColor Cyan
			}
			else{
				$rule = Set-AzureRmSqlServerFirewallRule -ResourceGroupName $ResourceGroup -ServerName $ResourceName -FirewallRuleName $env:USERNAME -StartIpAddress $myIP -EndIpAddress $myIP
				Write-Host "   Successfully Updated your rule to $myIP" -ForegroundColor Green
			}
		}
		else{
			$rule = New-AzureRmSqlServerFirewallRule -FirewallRuleName $env:USERNAME -StartIpAddress $myIP -EndIpAddress $myIP -ResourceGroupName $ResourceGroup -ServerName $ResourceName
			Write-Host "   Successfully Added $myIP" -ForegroundColor Green
		}
	}
	else{
		Write-Error "Resource $($ResourceName) was not found"
	}
}
Export-ModuleMember -Function SQLServer-AddMyIPToFirewall
Function PerformXMLEnvironmentVarReplacment([ref][XML]$item, [boolean]$deep = $false){
	$nodes=@()
	
	if($deep){
		foreach($node in $item.Value.SelectNodes(".//*")){
		  $nodes+=$node
		}
	}
	
	$nodes+=$item.Value
	$foundReplacment = $false
	$anyErrors=$false
	do{
		$foundReplacment = $false
		foreach($node in $nodes) {
			if($node.'#cdata-section'){
				$text = $node.'#cdata-section'
				$success=$true
				$value=PerformEnvironmentVarReplacment $text $([ref]$success)
				if($value -ne $text){
					$foundReplacment=$true
					Write-Verbose "   Updating Node $($node.LocalName) value fron $text > $($value)"
					$node.'#cdata-section' = $value
				}
			}
			if($node.'#text'){
				$text = $node.'#text'
				$success=$true
				$value=PerformEnvironmentVarReplacment $text $([ref]$success)
				if($value -ne $text){
					$foundReplacment=$true
					Write-Verbose "   Updating Node $($node.LocalName) value fron $text > $($value)"
					$node.'#text' = $value
				}
			}
			foreach($_ in $node.Attributes){
				Write-Verbose "Attribute [$($_.Name)]"
				$success=$true
				$value=PerformEnvironmentVarReplacment $node.$($_.Name) $([ref]$success)
				if($success){
					if($value -ne $node.$($_.Name)){
						$foundReplacment=$true
						Write-Verbose "   Updating Node $($node.LocalName) value fron $($node.$($_.Name)) > $($value)"
						$node.$($_.Name) = $value
					}
				}
				else{
					$anyErrors=$true
				}
				
			}
		}
	}while($foundReplacment)
	
	return -not $anyErrors
}

Export-ModuleMember -Function PerformXMLEnvironmentVarReplacment
Function PerformEnvironmentVarReplacment([string]$value, [ref][bool]$success){
	
	$pattern = '[@$][{(]([^)]*?)[})]'
	$containsVariables=$value -match $pattern
	Write-Verbose "Checking Value $value for variables"
	if(-not $containsVariables){
		$success.Value=$true
		return $value
	}
	$variable =$value -replace "[\s\S]*$($pattern)[\s\S]*", '$1'
	
	Write-Verbose "   Injecting dependencies $($variable ) Value('$($value)')"
	$thisPattern = '[@$][{(]'+$variable+'[})]'
	
	if(-not $(HasEnv($variable))) {
	
		if(IsEnvMockEnabled){
			Write-Verbose "Variable '$($variable)' (referenced from '$($value)') was not found - But was mocked..."
			SetEnv $variable "[MOCKED]-$($variable)"
			
			$value=$value -replace $thisPattern, $(GetEnv($variable))
			$success.Value=$true
		}
		else{	
			Write-Error "Variable '$($variable)' (referenced from '$($value)') was not found"
			$passedValidation=$false
			$value=$value -replace $thisPattern, "{null}"
			$success.Value=$false
		}
	}
	else{
		Write-Verbose "   Putting correct value for variable '$($variable)' in '$($value)'"
		$value=$value -replace $thisPattern, $(GetEnv($variable))
		$success.Value=$true
	}
	
	return $value
}
Function StartProcessWithEnvironment($arguments, $parameters, [ScriptBlock]$callback){
	try {
		Write-Verbose "Setting up Environment"
		
		$inputs = $parameters | Where {$_.Type -eq "Input"}
		foreach($parameter in $inputs){
			$value = $arguments[$parameter.Name]
			if(-not $value){
				continue
			}
			Write-Host "Checking Parameter $($parameter.Name) Value $($value)"
			if($parameter.DataType -eq "Array"){
				if($value -is [System.Object[]]){
					$value = $value -join ","
					Write-Host "Converting Array Parameter $($parameter.Name) to a string value $($value)"
				}
				else{
					Throw "Unexpected place to be: Code(2020)"
				}
			}
			Write-Verbose "   Consuming Input Variable '$($parameter.Name)'"
			SetEnv $parameter.Name $value
		}
		
		Write-Verbose "Testing Parameters:"
		$passedValidation = $true
		foreach($parameter in $parameters) {
			
			if($parameter.Type -eq "EnvironmentVariable") {
				
				# Handle Default Values that are back up if the environment variable wasnt found
				# But... It can be overwritten by incoming inputs
				if(-not (HasEnv($parameter.Name))){
					Write-Error "'$($parameter.Name)': Expected to be a environment variable. Not found"
					$passedValidation = $false
					continue
				}
				
				Write-Verbose "   Consuming Input Variable '$($parameter.Name)' from Environment Var '$(GetEnv($parameter.Name))'"
				SetEnv $parameter.Name $(GetEnv($parameter.Name))
			}
		}
		
		foreach($parameter in $parameters){
			if($parameter.Type -eq "Variable") {
				# Handle Static Variables that are read only
				# Dont pick up environment variables, cant be passed in
				if($parameter.Value) {
					Write-Verbose "'$($parameter.Name)': Set variable '$($parameter.Value)'"
					SetEnv $parameter.Name $parameter.Value 
					continue
				}
			}
			if($parameter.Type -eq "Input"){
				# Handle Default Values that are back up if the environment variable wasnt found
				# But... It can be overwritten by incoming inputs
				if((HasEnv($parameter.Name))){
					Write-Verbose "   Consuming Input Variable from Environment Var '$($_.Key)'"
					SetEnv $parameter.Name $(GetEnv($parameter.Name))
					continue
				}
				
				# Handle Default Values that are back up if the environment variable wasnt found
				# But... It can be overwritten by incoming inputs
				if($parameter.DefaultValue -and -not (HasEnv($parameter.Name))){
					Write-Verbose "'$($parameter.Name)': Input not found, using Default Value('$($parameter.DefaultValue)')"
					SetEnv $parameter.Name $parameter.DefaultValue
					continue
				}
				
			}
			
			if(-not $(HasEnv($parameter.Name))){
			
				if(IsEnvMockEnabled){
					Write-Verbose "Variable '$($variable)' (referenced from '$($value)') was not found - But was mocked..."
					
					SetEnv $($parameter.Name) "[MOCKED]-$($variable)"
					continue
				}
				
				Write-Error "'$($parameter.Name)': Missing Variable`r`n  As a Environment Variable or a Input Variable"
				$passedValidation = $false
				continue
			}
			if([String]::IsNullOrEmpty($(GetEnv($parameter.Name)))){
				Write-Error "'$($parameter.Name)': Variable is Empty"
				$passedValidation = $false
				continue
			}
			
			
			
		}
		
		# Temporary - TODO (Move the check not contains after as a post deployment item which will be added later)
		if(IsEnvMockEnabled) {
			foreach($parameter in $parameters) {
				if($parameter.CheckNotContainAfter){
					if(-not (HasEnv($parameter.Name))){
						continue;
					}
					$value = GetEnv($parameter.Name)
					SetEnv $($parameter.Name) "[MOCKED]-$($parameter.Name)"
				}
				if($parameter.CheckContainAfter){
					if(-not (HasEnv($parameter.Name))){
						continue;
					}
					$value = GetEnv($parameter.Name)
					SetEnv $($parameter.Name) "[MOCKED]-$($parameter.Name)"
				}
			}
		}
		
		$iteration = 0
		do{
			$foundVariablesWithChanges = $false
			Write-Host "Parameter Replacing - #$($iteration)"
			foreach($parameter in $parameters) {
				$parameterValue = $(GetEnv($parameter.Name))
				$pattern = '[@$][{(]([^)]*?)[})]'
				$containsVariables=$parameterValue -match $pattern
				if(-not $containsVariables){
					continue
				}
				$foundVariablesWithChanges =$true
				Write-Verbose "   Injecting dependencies $($parameter.Name) Value('$($parameterValue)')"
				
				$variable = $parameterValue -replace ".*$($pattern).*", '$1'
				$thisPattern = '[@$][{(]'+$variable+'[})]'
				
				
				if(-not $(HasEnv($variable))) {
					if(IsEnvMockEnabled){
						Write-Verbose "Variable '$($variable)' (referenced from '$($value)') was not found - But was mocked..."
						
						SetEnv $variable "[MOCKED]-$($variable)"
						$parameterValue = $parameterValue -replace $thisPattern, $(GetEnv($variable))
					}
					else{
						Write-Error "Variable '$($variable)' (referenced from '$($parameter.Name)') was not found"
						$passedValidation=$false
						$parameterValue = $parameterValue -replace $thisPattern, "{null}"
					}
					
				}
				else{
					Write-Verbose "   Putting correct value for variable '$($variable)' in variable '$($parameter.Name)'"
					$parameterValue = $parameterValue -replace $thisPattern, $(GetEnv($variable))
				}
				
				if(GetEnv($parameter.Name) -neq $parameterValue){
					Write-Verbose "  Updating Variable '$($parameter.Name)': '$(GetEnv($parameter.Name))' -> '$parameterValue'"
				}
				SetEnv $parameter.Name $parameterValue 
			}
			
			$iteration += 1
		}
		while($foundVariablesWithChanges -and $passedValidation)
		
		
		$iteration = 0
		do{
			Write-Host "Conditional Parameters - #$($iteration)"
		
			$numberOfMissingParametersFoundInConditions = 0
			$numberOfFoundParametersFoundInConditions = 0
			$numberOfHitConditions=0
			foreach($parameter in $parameters) {
				if($parameter.Type -eq "Variable" -and $parameter.Conditions) {
					foreach($condition in $parameter.Conditions.Condition){
						Write-Verbose "$($parameter.Name) - Testing Condition for Parameter '$($condition.VariableName)'"
						$numberOfHitConditions+=1
						
						$variableName = $condition.VariableName
						$variableValue = $condition.VariableValue
						$value = $condition.Value
						
						if($(HasEnv($variableName))) {
							$numberOfFoundParametersFoundInConditions += 1
							
							Write-Verbose "  $($parameter.Name) Found Variable '$($variableName)' to start Matching ('$variableValue' -eq '$(GetEnv($variableName))'"
							if($variableValue -eq $(GetEnv($variableName))){
								Write-Verbose "  $($parameter.Name) Found Condition to Match '$($variableValue)', Setting to '$($value)'"
								SetEnv $parameter.Name $value
							}
						}
						else{
							$numberOfMissingParametersFoundInConditions += 1
						}
					}
				}
			}
			
			if($numberOfFoundParametersFoundInConditions -eq 0 -and `
			   $numberOfMissingParametersFoundInConditions -ne 0 -and `
			   $numberOfHitConditions -ne 0){
				Write-Error "No longer opportunities to set variable using conditions"
				$passedValidation=$false
				break
			}
			$iteration += 1
			
		}while($numberOfMissingParametersFoundInConditions)
		
		if(-not $passedValidation){
			Throw "Not all Parameters passed inspection"
		}
		
		Write-Host "Input Parameters:"
		foreach($parameter in $parameters){
			
			Write-Host ([String]::Format("  '{0,5}':'{1}'",$parameter.Name,$(GetEnv($parameter.Name))))
		}
		
		# Start Proccess
		.$callback
		
		# End Proccess
		
		foreach($parameter in $parameters){
			if($parameter.CheckNotContainAfter){
				if(-not (HasEnv($parameter.Name))){
					Write-Error "Variable '$($parameter.Name)' was not found on 'CheckNotContainAfter' check"
					$passedValidation = $false;
					continue;
				}
				$value = GetEnv($parameter.Name)
				
				if($value -match $parameter.CheckNotContainAfter){
					Write-Error "Variable '$($parameter.Name)' contained '$($parameter.CheckNotContainAfter)' on 'CheckNotContainAfter' check which is not expected"
					$passedValidation = $false;
					continue;
				}
				
				Write-Verbose "Variable '$($parameter.Name)' did not contain '$($parameter.CheckNotContainAfter)' on 'CheckNotContainAfter' check which is expected"
			}
			if($parameter.CheckContainAfter){
				if(-not (HasEnv($parameter.Name))){
					Write-Error "Variable '$($parameter.Name)' was not found on 'CheckContainAfter' check"
					$passedValidation = $false;
					continue;
				}
				$value = GetEnv($parameter.Name)
				
				if(-not ($value -match $parameter.CheckContainAfter)){
					Write-Error "Variable '$($parameter.Name)' contained '$($parameter.CheckContainAfter)' on 'CheckContainAfter' check which is not expected"
					$passedValidation = $false;
					continue;
				}
				
				Write-Verbose "Variable '$($parameter.Name)' contained '$($parameter.CheckNotContainAfter)' on 'CheckNotContainAfter' check which is expected"
			}
		}
		
	}
	catch{
		$formatstring = "{0} : {1}`n{2}`n" +
				"    + CategoryInfo          : {3}`n" +
				"    + FullyQualifiedErrorId : {4}`n"
		$fields = $_.InvocationInfo.MyCommand.Name,
				  $_.ErrorDetails.Message,
				  $_.InvocationInfo.PositionMessage,
				  $_.CategoryInfo.ToString(),
				  $_.FullyQualifiedErrorId

		$formatstring -f $fields
		Write-Host -Foreground Red -Background Black ($formatstring -f $fields)
		Write-Error "Exception $($_.Exception.Message)"
	}
	finally{
		# Flushing out all tracked variables
		 FlushTrackedVars
	}
}
Export-ModuleMember -Function StartProcessWithEnvironment

Function GenerateSupportedTasks(){
	Set-Variable -Name "taskTypes" -Value $([hashtable]::new()) -Scope Global -ErrorAction SilentlyContinue
	
	$taskTypes.Add("SetVariable",{`
		Param(
			[ref][XML]$task
		)
		if(-not $task.Value.VarName -or -not $task.Value.VarValue){
			Throw "Unable to Set Variable"
		}
		
		$value=$task.Value.VarValue
		if($value."#text"){
			$value=$value."#text"
		}
		elseif($value."#cdata-section"){
			$value=$value."#cdata-section"
		}
		
		
		# Write-Host " Setting Var $($task.Value.VarName) to $($value)" -ForegroundColor Cyan
		SetEnv $task.Value.VarName $value
	})
	
	$taskTypes.Add("SqlDeployment",{
		Param(
			[ref][XML]$task
		)
		
		if(-not $task.Value.Arguments){
			Throw "Unable to Deploy using datpac without arguments"
		}
		
		Write-Host "Executing Dacpac (FAKE FOR NOW)"
		# .\SqlPackage.exe $task.Arguments
	})
	$taskTypes.Add("KeyVaultSecret",{
		Param(
			[ref][XML]$task
		)
		
		
		
		Write-Host "Executing Dacpac (FAKE FOR NOW)"
		# .\SqlPackage.exe $task.Arguments
	})
	$taskTypes.Add("Exe",{
		Param(
			[ref][XML]$task
		)
		
		$exe=$task.Value.Exe
		$workingDirectory = $task.Value.WorkingDirectory
		$NewWindow = $task.Value.NewWindow
		$Wait = $task.Value.Wait
		$arguments=@()
		$arg=$task.Value.Argument | foreach { $arguments+=$_.Value }
		if(-not $exe){
			Throw "Executable not found" 
		}
		
		# Some variable Checks
		if($NewWindow -and $NewWindow -ne "true" -and $NewWindow -ne "false"){
			Throw "Unkown Value for New Window '$NewWindow'"
		}
		if($Wait -and $Wait -ne "true" -and $Wait -ne "false"){
			Throw "Unkown Value for New Wait '$Wait'"
		}
		
		
		$finalScript='Start-Process $exe  '
		
		# Some Dynamic Parameters
		if($workingDirectory) { $finalScript +=' -WorkingDirectory $workingDirectory ' }
		if($NewWindow -ne "true") { $finalScript +=" -NoNewWindow " }
		if($Wait -ne "false") { $finalScript +=" -Wait " }
		if($task.Value.Argument) { $finalScript +=' -ArgumentList $arguments ' }
		
		$finalScript += " -PassThru"
		$finalScriptBlock=[ScriptBlock]::Create($finalScript)
		
		# Run the main script
		Write-Host "$finalScript"
		&$finalScriptBlock
		
		# .\SqlPackage.exe $task.Arguments
	})
	$taskTypes.Add("TaskGroup",{
		Param(
			[ref][XML]$task,
			[ref][XML]$originalTask
		)
		$taskGroupName=$task.Value.TaskGroupName
		$taskGroupFound = $originalTask.Value.SelectSingleNode("../../TaskGroup[@Name='$($taskGroupName)']")
		if(-not (Run-TaskGroup $([ref]$taskGroupFound) $($task))){
			Throw "Failed to run task group $($taskGroupName)"
		}
		
		# .\SqlPackage.exe $task.Arguments
	})
	$taskTypes.Add("Script",{
		Param(
			[ref][XML]$task
		)
		
		$script=$task.Value.Script
		if(-not $script){
			Throw "Executable not found" 
		}
		try{
			Write-Host "$script"
			iex $script
		}
		catch{
			Throw "$_.Exception.Message" 
		}
		# .\SqlPackage.exe $task.Arguments
	})
	$taskTypes.Add("AddKeyVaultSecret",{
		Param(
			[ref][XML]$task
		)
		
		if(-not $task.Value.KeyVaultName -or -not $task.Value.SecretName -or -not $task.Value.SecretValue){
			Throw "Executable not found" 
		}
		try{
			$keyVaultName = $task.Value.KeyVaultName
			$secretName   = $task.Value.SecretName
			$secretValue  = $task.Value.SecretValue
			
			$secretvalueFromKeyVault = Get-AzureKeyVaultSecret -VaultName $keyVaultName -Name $secretName
			$secretvalueFromKeyVault = $secretvalueFromKeyVault| ConvertTo-SecureString -AsPlainText -Force 
			if($secretvalueFromKeyVault -eq $secretValue){
				Write-Host "Secret '$secretName' already set to value" -ForegroundColor Cyan
				return
			}
			
			$secretvalue = $secretvalue| ConvertTo-SecureString -AsPlainText -Force 
			$secret = Set-AzureKeyVaultSecret -VaultName $keyVaultName -Name $secretName -SecretValue $secretvalue
			Write-Host "Set Secret '$secretName' to new value" -ForegroundColor Cyan
		}
		catch{
			Throw "$_.Exception.Message" 
		}
		# .\SqlPackage.exe $task.Arguments
	})
	$taskTypes.Add("FetchKeyVaultSecret",{
		Param(
			[ref][XML]$task
		)
		
		if(-not $task.Value.KeyVaultName -or -not $task.Value.SecretName -or -not $task.Value.ExportTo){
			Throw "Executable not found" 
		}
		try{
			$keyVaultName = $task.Value.KeyVaultName
			$secretName   = $task.Value.SecretName
			$exportTo  = $task.Value.ExportTo
			$defaultValue  = $task.Value.defaultValue
			
			$secretvalueFromKeyVault = Get-AzureKeyVaultSecret -VaultName $keyVaultName -Name $secretName
			if(-not $secretvalueFromKeyVault -and -not $defaultValue){
				Throw "Secret $($secretName) doesnt exists in key vault $($keyVaultName)"
			}
			elseif(-not $secretvalueFromKeyVault){
				SetEnv $exportTo $defaultValue
			}
			else{
				SetEnv $exportTo $secretvalueFromKeyVault.SecretValueText
			}
			
		}
		catch{
			Throw "$_.Exception.Message" 
		}
		# .\SqlPackage.exe $task.Arguments
	})
	$taskTypes.Add("GetWebServiceUrl",{
		Param(
			[ref][XML]$task
		)
		
		if(-not $task.Value.ResourceName -or -not $task.Value.ExportTo){
			Throw "Required Variables for GetWebServiceUrl not found" 
		}
		try{
			$url          = Get-AzureWebAppUrl $task.Value.ResourceName
			$variableName = $task.Value.ExportTo
			if([String]::IsNullOrEmpty($url) -and $task.Value.DefaultValue){
				$url = $task.Value.DefaultValue
			}
			SetEnv $variableName $url
			
			
			Write-Host "Set Variable '$($variableName)' to '$($url)'" -ForegroundColor Cyan
		}
		catch{
			Throw "$_.Exception.Message" 
		}
		# .\SqlPackage.exe $task.Arguments
	})
	
	
	
}
Function AddParmeter([ref][Object[]]$parameters, [String] $name, [string] $defaultValue, [string] $type, [string] $CheckNotContainAfter) {

	$obj = @{}
	$obj.Add("Name",$name)
	$obj.Add("DefaultValue",$defaultValue)
	$obj.Add("Type","Input")
	$obj.Add("DataType",$type)
	$obj.Add("CheckNotContainAfter", $parameter.CheckNotContainAfter)
	$parameters.Value+=$obj
}
Export-ModuleMember -Function AddParmeter
Function GenerateDynamicVariableDictionary([XML]$settings, [ref][Object[]]$parameters) {

	 GenerateSupportedTasks

	# Parameter Attributes...
	 $MandatoryAttr  = New-Object System.Management.Automation.ParameterAttribute
	 $MandatoryAttr.Mandatory  = $true
	 $MandatoryAttr.ParameterSetName  = '__AllParameterSets'   
	 
	 $GenericAttr  = New-Object System.Management.Automation.ParameterAttribute
	 $GenericAttr.ParameterSetName  = '__AllParameterSets'     	 
	 
	 # The collection of parameters...
	 $AttribColl = New-Object  System.Collections.ObjectModel.Collection[System.Attribute]
	 $AttribColl.Add($ParamAttrib)

	 # The collection of parameters...
	 $RuntimeParamDic  = New-Object System.Management.Automation.RuntimeDefinedParameterDictionary

	 # Flushing out all tracked variables
	 FlushTrackedVars
	 
	 # Get Required Parameters
	 $requiredParam = $settings.Settings.Parameters
	 if(-not $requiredParam) {
		Write-Warning "No Required Parameters found..."
	 }
	 # Consume all input parameters
	 foreach($parameter in $requiredParam.Parameter){
		$obj = @{}
		$obj.Add("Name",$parameter.Name)
		$obj.Add("DefaultValue",$parameter.DefaultValue)
		$obj.Add("Type","Input")
		$obj.Add("DataType",$parameter.Type)
		$obj.Add("CheckNotContainAfter", $parameter.CheckNotContainAfter)
		$parameters.Value+=$obj
	 }
	 # Consume all input parameters
	 foreach($parameter in $settings.Settings.Variables.Variable){
		$obj = @{}
		$obj.Add("Name",$parameter.Name)
		$obj.Add("Value",$parameter.Value)
		$obj.Add("Type","Variable")
		$obj.Add("Conditions",$parameter.Conditions)
		$obj.Add("CheckNotContainAfter", $parameter.CheckNotContainAfter)
		$parameters.Value+=$obj
	 }
	 # Consume all input parameters
	 foreach($parameter in $settings.Settings.EnvironmentVariables.Variable){
		$obj = @{}
		$obj.Add("Name",$parameter.Name)
		$obj.Add("Type","EnvironmentVariable")
		$obj.Add("CheckNotContainAfter", $parameter.CheckNotContainAfter)
		$parameters.Value+=$obj
	 }
	 
	 # Go through parameters
	 foreach($parameter in $parameters.Value) {
	 
		$AttribColl = New-Object  System.Collections.ObjectModel.Collection[System.Attribute]
		
		if($parameter.Type -eq "EnvironmentVariable"){
			Write-Verbose "'$($parameter.Name)': Skipping Environment Variables"
			continue
		}
		if($parameter.Type -eq "Variable"){
			Write-Verbose "'$($parameter.Name)': Skipping Variables"
			continue
		}
		if($parameter.Type -eq "Input"){
			
			# Handle If Environment Variable already exists
			if((HasEnv($parameter.Name))){
				$AttribColl.Add($GenericAttr)
			}
				
			# Handle Missing Environment Variables that need to be passed in
			if(-not (HasEnv($parameter.Name))){
				$AttribColl.Add($GenericAttr)
			}
			
			$RuntimeParam  = New-Object System.Management.Automation.RuntimeDefinedParameter($parameter.Name,  [string], $AttribColl)
			if($parameter.DataType -eq "Array"){
				$RuntimeParam  = New-Object System.Management.Automation.RuntimeDefinedParameter($parameter.Name,  [System.Object[]], $AttribColl)
			}
			$RuntimeParamDic.Add($parameter.Name,  $RuntimeParam)
		}
	 }
	
	 return $RuntimeParamDic
}
Export-ModuleMember -Function GenerateDynamicVariableDictionary

Function Get-AzureWebAppUrl ([string]$name){
	$rootpath = $PSScriptRoot
	$tmp = (New-TemporaryFile).FullName
	$resource = Get-AzureRmResource -Name $name -ResourceType  "Microsoft.web/sites"
	if(-not $resource){
		return $null
	}
	
	$resource=$resource[0]
	Write-Host (ConvertTo-Json $resource)
	$xmlcontent = (Get-AzureRmWebAppPublishingProfile -Name $name -ResourceGroupName $resource.ResourceGroupName -OutputFile $tmp )	
	$xml = New-Object -TypeName System.Xml.XmlDocument
	$xml.LoadXml($xmlcontent)
	$url = $xml.SelectNodes("//publishProfile[@publishMethod='FTP']/@destinationAppUrl").value
	del $tmp
	
	# Make it https
	$url = $url -replace "http\:","https:"
	
	#Fix End Route Slash
	$url = $url -replace "([^/])$",'$1/'
	
	return $url
}

# TODO - Move Mocking to a dynamic stable instead of 1 individual mock
Function IsEnvMockEnabled(){
	if(-not $(HasEnv("CngMockMissingParameters"))){
		return $false
	}
	
	if($(GetEnv("CngMockMissingParameters")) -eq "false"){
		return $false
	}
	
	return $true
}
Function AllowMockedAppSettings(){
	if(-not $(HasEnv("CngAllowMockedAppSettings"))){
		return $false
	}
	
	if($(GetEnv("CngAllowMockedAppSettings")) -eq "false"){
		return $false
	}
	
	return $true
}
Function HasEnv([string] $str){
	$test = [Environment]::GetEnvironmentVariable($str)
	if($test){
		return $true
	}
	return $false
}
Function GetEnv([string] $str){
	$test = [Environment]::GetEnvironmentVariable($str)
	if($test){
		return $test
	}
	Write-Error "Unkown Environment Variable '$str'"
}
Function GetVar([string] $str){
	$test = Get-Variable -Name $str   -ErrorAction SilentlyContinue
	if($test){
		return $test.Value
	}
	Write-Error "Unkown Variable '$str'"
}
Function HasVar([string] $str){
	$test = Get-Variable -Name $str -ErrorAction SilentlyContinue
	if($test){
		return $true
	}
	return $false
}
Function GetGlobalTraceOfEnvVariables{
	if(-not $Global:ENV_TRACKING){
		$Global:ENV_TRACKING = @{}
	}
	return $Global:ENV_TRACKING
}
Function SetGlobalTraceOfEnvVariables([hashtable]$table){
	$Global:ENV_TRACKING = $table
}
Function AddToTrackedVars([string]$name, [string]$value){
	$vars = GetGlobalTraceOfEnvVariables
	if(-not $vars.ContainsKey($name)){
		$vars.Add($name, $value)
	}
}
Function FlushTrackedVars(){
	$vars = GetGlobalTraceOfEnvVariables
	$vars.GetEnumerator() | % {
		Write-Verbose "Reverting '$($_.Key)' back to '$($_.Value)'"
		[Environment]::SetEnvironmentVariable($_.Key, $_.Value, "Process") 
	}
	SetGlobalTraceOfEnvVariables(@{})
}
Function SetVar([string] $str, [string]$value) {
	Write-Host "Setting variable $($key) -> $($value)"
	$test = Set-Variable -Name $str -Value $value -Scope Script -ErrorAction SilentlyContinue
}
Function GetEnvNC([string] $str){
	$test = Get-ChildItem env: | Where-Object {$_.Name -eq $str}
	if($test){
		return $test.Value
	}
	return $null
}
Function SetEnv([string] $key, [string]$value){
	AddToTrackedVars $key $(GetEnvNC($key))
	Write-Verbose "Setting environment variable $($key) -> $($value)"
	[Environment]::SetEnvironmentVariable($key, $value, "Process") 
}
Export-ModuleMember -Function AllowMockedAppSettings
Export-ModuleMember -Function IsEnvMockEnabled
Export-ModuleMember -Function GetEnv
Export-ModuleMember -Function HasEnv
Export-ModuleMember -Function SetEnv
Export-ModuleMember -Function GetVar
Export-ModuleMember -Function HasVar
Export-ModuleMember -Function SetVar
Export-ModuleMember -Function GetEnvNC
Export-ModuleMember -Function FlushTrackedVars
Export-ModuleMember -Function AddToTrackedVars
Export-ModuleMember -Function SetGlobalTraceOfEnvVariables
Export-ModuleMember -Function GetGlobalTraceOfEnvVariables
Export-ModuleMember -Function PerformEnvironmentVarReplacment


Function Export-KeyVaultValuesToEnvVar([Provision] $provision){
	Write-Host "Exporting Env from Key Vault $($provision.SiteName)"
	$provision.Get("Secrets").GetEnumerator() | % {
		Write-Host "   Env $($_.Key) Exported from Key Vault $($provision.SiteName)"
		[Environment]::SetEnvironmentVariable($_.Key, $_.Value.Value, "Process") 
	}   
}
Export-ModuleMember -Function Export-KeyVaultValuesToEnvVar

Function SendEmail([string]$emailTo, [string]$nameToName,[string]$emailFromName, [string]$subject, [string]$body, [bool]$isHtml){
	Write-Warning "DISABLING ALL NOTIFICATIONS FOR SECURITY PURPOSES"
	return
	# $secpasswd = ConvertTo-SecureString (GetEnv "emailUserPassword") -AsPlainText -Force
	# $mycreds = New-Object System.Management.Automation.PSCredential ((GetEnv "emailUserName"), $secpasswd)
	
	# if($isHtml){
	# 	Send-MailMessage -To "$nameToName <$($emailTo)>" -From "$emailFromName <$(GetEnv("emailFrom"))>" -Subject $subject -SmtpServer ((GetEnv "emailSMTPServer")) -Credential $mycreds -UseSsl -Port 587 -BodyAsHtml $body
	# }
	# else{
	# 	Send-MailMessage -To "$nameToName <$($emailTo)>" -From "$emailFromName <$(GetEnv("emailFrom"))>" -Subject $subject -SmtpServer ((GetEnv "emailSMTPServer")) -Credential $mycreds -UseSsl -Port 587 -Body $body
	# }
	
}
Export-ModuleMember -Function SendEmail


class AppInsights {
	[string]$InstrumentalKey 

	AppInsights() {
		$this.Init("N\A")
	}
	AppInsights([string]$InstrumentalKey) {
		$this.Init($InstrumentalKey)
	}
	hidden Init([string]$InstrumentalKey) {
		$this.InstrumentalKey=$InstrumentalKey
	}
	[string] ToString() {
		return "InstrumentalKey($($this.InstrumentalKey))"
	}
}

class KeyValuePair{
	[string]$Key
	[string]$Value
	[bool]$_allowEmptyValues
	KeyValuePair([string]$key, [string]$value){
		$this.Key=$key;
		$this.Value=$value;
		$this._allowEmptyValues = $false
	}
	
	[bool] HasNullValue(){
		if($this._allowEmptyValues){
			return ($this.Value -eq $null)
		}
		
		return ($this.Value -eq $null) -or ($this.Value.Length -eq 0)
	}
	
	[KeyValuePair] AllowEmptyValues(){
		$this._allowEmptyValues = $true;
		return $this
	}
}
class DifferencePair{
	[object]$First
	[object]$Second

	DifferencePair([object]$first, [object]$second){
		$this.First=$first
		$this.Second=$second
	}
}

class ValidationState{

	[bool]$Valid
	[string]$Message

	ValidationState([bool]$valid, [string]$message){
		$this.Valid=$valid
		$this.Message=$message
	}
}
class KeyValuePairCollection {
	[KeyValuePair[]] $_items

	KeyValuePairCollection(){
		$this._items=@()
	}

	[object] Get($key){
		$foundItem = $null

		foreach ($element in $this._items) {
			if ( $element.Key -eq $key) {
				$foundItem = $element
			}
		}
		
		return $foundItem
	}
	[System.Collections.IEnumerator] GetEnumerator() {
		return $this._items.GetEnumerator()
	}

	[KeyValuePairCollection] Not([KeyValuePairCollection] $filterOutKeys){
		$notFiltered=[KeyValuePairCollection]::new()
		$this.GetEnumerator() % {
			if( -not $filterOutKeys.ContainsKey($_.Key) ){
				$notFiltered.Add($_.Key, $_.Value)
			}
		}

		return $notFiltered
	}
	[KeyValuePairCollection] Join([KeyValuePairCollection] $filterOutKeys){
		$notFiltered=[KeyValuePairCollection]::new()
		$this.GetEnumerator() % {
			if( $filterOutKeys.ContainsKey($_.Key) ){
				$notFiltered.Add($_.Key, $_.Value)
			}
		}

		return $notFiltered
	}
	[KeyValuePairCollection] CombineWithDifferentValue([KeyValuePairCollection] $filterKeys){
		$merged=[KeyValuePairCollection]::new()
		$filtered = $this.Join($filterKeys)
		$this.GetEnumerator() % {
			$thisItem = $_
			$thatItem = $filtered.Get($_.Key)
			if ($filtered.Value -ne $filtered.Value ) {
				$merged.Add($_.Key, [DifferencePair]::new($thisItem.Value, $thatItem.Value))
			}
		}

		return $merged
	}
	[bool] HasDuplicates(){
		$hash = @{}
		$hasDuplicates = $false
		$this.GetEnumerator() | % {
			if($hash.ContainsKey($_.Key)){
				$hasDuplicates =  $true               
			}
			else{
				$hash.Add($_.Key, $_.Value)
			}
		}
		return $hasDuplicates
	}
	[bool] HasNoUnexpectedMockData(){
		if(AllowMockedAppSettings) {
			return $false
		}
		$hasUnexpectedMock = $false
		$this.GetEnumerator() | % {
			if($_.Value -match "\[MOCKED\]"){
				$hasUnexpectedMock = $true
			}
		}
		return $hasUnexpectedMock
	}
	[string] GetNoUnexpectedMockData(){
		if(AllowMockedAppSettings) {
			return $false
		}
		$hash = @()
		$hasUnexpectedMock = $false
		$this.GetEnumerator() | % {
			if($_.Value -match "\[MOCKED\]"){
				$hasUnexpectedMock = $true
				$hash += $_.Key
			}
		}
		return [String]::Join(",",$hash)
	}
	[string] GetDuplicates(){
		$hash = @{}
		$keys= ""
		$hasDuplicates = $false
		$this.GetEnumerator() | % {
			if($hash.ContainsKey($_.Key)){
				if($keys.Length -gt 0){
					$keys+=", "
				}      
				$keys+=$_.Key
			}
			else{
				$hash.Add($_.Key, $_.Value)
			}
		}
		return $keys
	}
	[bool] HasNullValues(){
		$hash = @{}
		$hasNullValues = $false
		$this.GetEnumerator() | % {
			$hasNullValues = $hasNullValues -or $_.HasNullValue()
		}
		return $hasNullValues
	}
	[string] GetNullValueKeys(){
		$keys= ""
		$hasNullValues = $false
		$this.GetEnumerator() | % {
			if ($_.HasNullValue()){
				if($keys.Length -gt 0){
					$keys += ", "
				}
				$keys +="'$($_.Key)'"
			}
		}
		return $keys
	}
	[ValidationState] ValidateCollection(){
		if($this.HasDuplicates()){
			return [ValidationState]::new($false, "Has Duplicate Values: $($this.GetDuplicates())")
		}
		if($this.HasNullValues()){
			return [ValidationState]::new($false, "Contains null values: $($this.GetNullValueKeys())")
		}
		if($this.HasNoUnexpectedMockData()){
			return [ValidationState]::new($false, "Contains mocked values: $($this.GetNoUnexpectedMockData())")
		}
		return [ValidationState]::new($true, "All Valid")
	}
	[object] ContainsKey($key) {
		return $this.Get($key) -ne $null
	}

	[KeyValuePair] Add([string]$key, [string]$value) {
		$keyItem = [KeyValuePair]::new($key,$value)
		$this._items+=$keyItem
		
		return $keyItem
	}
}
class SetupServiceDeploymentParameters {
	[string]   $resourceGroup
	[string]   $site
	[string]   $keyVaultName
	[hashtable]$keyVaultSecrets
	[hashtable]$appConfiguration
	[hashtable]$connectionString

	SetupServiceDeploymentParameters([string]$resourceGroup, 
									 [string]$site, 
									 [string]$keyVaultName,
									 [hashtable]$keyVaultSecrets,
									 [hashtable]$appConfiguration,
									 [hashtable]$connectionString) {
		

	}
}
class KeyVaultDeploymentItem{
	[Provision]              $KeyVault
	[KeyValuePairCollection] $KeyVaultSecrets

	KeyVaultDeploymentItem([Provision] $KeyVault){
		$this.KeyVault=$KeyVault
		$this.KeyVaultSecrets=[KeyValuePairCollection]::new()
	}
	
	[ValidationState] ValidateCollection(){
		return $this.KeyVaultSecrets.ValidateCollection()
	}
}
class KeyVaultDeployment {

	[KeyVaultDeploymentItem[]]$_items
	KeyVaultDeployment(){

	}
	[KeyVaultDeploymentItem[]] KeyVaults(){
		return $this._items
	}
	[KeyVaultDeploymentItem] Get([Provision]$KeyVault){
		$foundItem = $null

		foreach ($element in $this._items) {
			if ( $element.KeyVault.Name -eq $KeyVault.Name) {
				$foundItem = $element
			}
		}
		
		return $foundItem
	}

	[object] ContainsKey($key) {
		return $this.Get($key) -ne $null
	}
	[ValidationState] ValidateCollection(){
		foreach($keyVaultDeployment in $this._items){
			$validation = $keyVaultDeployment.ValidateCollection()
			if(-not $validation.Valid){
				return [ValidationState]::new($false, "KeyVault($($keyVaultDeployment.KeyVault.Name)):`r`n $($validation.Message)")
			}
		}
		
		return [ValidationState]::new($true, "All Valid")
	}
	[KeyValuePair] Add([Provision]$KeyVault, [string]$key, [string]$value) {
		$foundKeyVault = $this.Get($KeyVault)
		if($foundKeyVault -eq $null){
			$foundKeyVault=[KeyVaultDeploymentItem]::new($KeyVault)
			$this._items += $foundKeyVault
		}

		return $foundKeyVault.KeyVaultSecrets.Add($key,$value)
	}

}
class Deployment {
	[string]                 $ProjectName
	[KeyVaultDeployment]     $KeyVaultSecrets
	[KeyValuePairCollection] $AppConfiguration
	[KeyValuePairCollection] $ConnectionStrings
	[AADApplication]         $RegisteredAdd

	Deployment($ProjectName){
		$this.Init($ProjectName,
				   @{},
				   @{},
				   @{})
	}
	Deployment([string]   $ProjectName, 
			   [hashtable]$KeyVaultSecrets,
			   [hashtable]$AppConfiguration,
			   [hashtable]$ConnectionStrings) {
		
		$this.Init($ProjectName,
				   $KeyVaultSecrets,
				   $AppConfiguration,
				   $ConnectionStrings)
	}
	AssociateToAAD([AADApplication]$RegisteredAdd){
		$this.RegisteredAdd = $RegisteredAdd
	}

	hidden Init([string]   $ProjectName, 
				[hashtable]$KeyVaultSecrets,
				[hashtable]$AppConfiguration,
				[hashtable]$ConnectionStrings) {
		$this.ProjectName=$ProjectName;
		$this.RegisteredAdd = $null
		$this.AppConfiguration=[KeyValuePairCollection]::new()
		$this.ConnectionStrings=[KeyValuePairCollection]::new()
		$this.KeyVaultSecrets=[KeyVaultDeployment]::new()
		$KeyVaultSecrets.GetEnumerator() | % {
			$this.KeyVaultSecrets.Add($_.key,$_.value)
		}
		$AppConfiguration.GetEnumerator() | % {
			$this.AppConfiguration.Add($_.key,$_.value)
		}
		$ConnectionStrings.GetEnumerator() | % {
			$this.ConnectionStrings.Add($_.key,$_.value)
		}
	}
}

class Provision {
	[string]       $Group
	[string]       $Name
	[string]       $ResourceGroup
	[string]       $SiteName 
	[string]       $ResourceType 
	[string]       $Url
	[hashtable]    $Properties
	[Deployment]   $PrimaryDeployment
	[Deployment[]] $DuplicateDeployments
	[string[]]     $Warnings 
	[bool]         $Skip
	[bool]         $ValidateOnly

	[string]       $errorReport
	[string]       $updatesReport
	[string]       $fullReport
	
	[Notification] $notification
	
	Provision() : base()  {
		$this.Init("N\A", "N\A","N\A","N\A",@{}) 
	}
	Provision([string]     $group, 
			  [string]     $name, 
			  [string]     $ResourceType,
			  [string]     $ResourceGroup,
			  [string]     $SiteName,
			  [hashtable]  $Properties)  : base(){
		$this.Init($group, $name, $ResourceType, $ResourceGroup, $SiteName, $Properties) 
	}
	NotifyOnErrors([string]$email, [string]$name = "N/A"){
		$this.notification.NotifyOnErrors($email,$name)
	}
	NotifyOnUpdates([string]$email, [string]$name = "N/A"){
		$this.notification.NotifyOnUpdates($email,$name)
	}
	NotifyOnFinished([string]$email, [string]$name = "N/A"){
		$this.notification.NotifyOnFinished($email, $name)
	}
	NotifyForErrors(){
		$this.notification.NotifyForErrors()
	}
	NotifyForUpdates(){
		$this.notification.NotifyForUpdates()
	}
	NotifyForFinished(){
		$this.notification.NotifyForFinished()
	}
	AddWarnings([string]$warningText) {
		$this.Warnings+=$warningText
	}
	SkipConfig([string]$reason){
		$this.AddWarnings("Skipping: $($reason)")
		$this.Skip = $true
	}
	SetUrl([string]$url){
		$this.Url = $url
	}
	[bool] FetchSecret([string]$name, [string]$asName, [string]$defaultValue){
		
		
		try {
			if($this.ValidateOnly){
				$secretValue = @{}
				
				$secretValue.SecretValueText ="[MOCKED]-$name"
				
				if($asName) {
					$secretValue.Name=$asName
				}
			}
			else{
				Write-Host "`r`nFetching Key Vault Secret $($name) for key vault ($($this.SiteName)). " -NoNewline
				if($this.ResourceType -ne "Microsoft.KeyVault/vaults"){
					Write-Error "Resource $($this.Name) is not a Key Vault meaning you are not able to fetch Secrets"
					return $false
				}
				Write-Host "Success"
				$secretValue = Get-AzureKeyVaultSecret -VaultName $this.SiteName -Name $name		
				if(-not $secretValue ){
					if(-not $defaultValue){
						Write-Error "Secret $name was not found in Key Vault $($this.SiteName) and no Default Value was given"
						return $false
					}
					$secretValue = @{}
					
					Write-Host "Failed. Using Default Value instead"
					$secretValue.SecretValueText = $defaultValue
					$secretValue.Name=$name
				}
				
				if($asName){
					$secretValue.Name=$asName
				}
				
				
			}
			if(-not $secretValue){
				Throw "No Key to Add"
			}
			
			$key  = @{Name = $secretValue.Name;
					 Value = $secretValue.SecretValueText}
			
			SetEnv $key.Name $key.Value
			if(-not $this.Properties["Secrets"]){
				$this.Properties["Secrets"]=@{}
			}
			if(-not $this.Properties["Secrets"][$key.Name]){
				$this.Properties["Secrets"].Add($key.Name, $key)					
			}
			else {
				throw "Secret '$($key.Name)' was found more then once for '$($this.SiteName)' "
				return $false
			}
			return $true
		}
		catch {
			Write-Error "$($_.Exception.Message)"
			Write-Error "Failed to fetch secret $($name) from Key Vault '$($this.SiteName)'"
			throw $_.Exception.Message
		}
		
		
		return $true
	}
	FetchAllSecrets() {
		Write-Host "`r`nFetching all Key Vault Values($($this.SiteName)):"

		$keys = @{}
		$count = 0
		$secretvalueFromKeyVault = Get-AzureKeyVaultSecret -VaultName $this.SiteName 
		$secretvalueFromKeyVault.GetEnumerator() | % {
			Write-Host ("   {0:D2}: {1}" -f ($count, $_.Name))
			$secretValue = Get-AzureKeyVaultSecret -VaultName $this.SiteName -Name $_.Name 
			$keys.Add($_.Name, @{Name = $_.Name;
								 Value = $secretValue.SecretValueText})

			$count += 1
		}

		$this.Properties["Secrets"] = $keys
		
	}
	[object] Get([string]$name){
		if($this.Properties.ContainsKey($name)){
			return $this.Properties[$name];
		}
		Write-Error "Unkown Property $name";
		return $null
	}
	hidden Init([string]     $group, 
				[string]     $name, 
				[string]     $ResourceType,
				[string]     $ResourceGroup,
				[string]     $SiteName,
				[hashtable]  $Properties) {

		$this.Group                    = $group
		$this.Name                     = $name
		$this.ResourceType             = $ResourceType
		$this.ResourceGroup            = $ResourceGroup
		$this.SiteName                 = $SiteName
		$this.Properties               = $Properties
		$this.PrimaryDeployment        = $null
		$this.DuplicateDeployments     = $()
		$this.Skip                     = $false
		$this.ValidateOnly             = $false
		
		$this.errorReport              = ""
		$this.updatesReport            = ""
		$this.fullReport               = ""
		
		$this.notification             = [Notification]::new($this.Name)
		$this.notification.AddProvision($this)
		if($ResourceType -eq "Microsoft.KeyVault/vaults"){
			$this.Properties["Secrets"]=@{}
		}
	}
	[Deployment] LatestDeployment() {
		if($this.HasDeployment()){
			return $this.PrimaryDeployment
		}
		$this.AddDeployment([Deployment]::new($this.Name))
		return $this.PrimaryDeployment
	}
	[void] AddDeployment([Deployment] $deployment) {
		

		if(-not $this.HasDeployment()){
			$this.PrimaryDeployment=$deployment
			return
		}
		$this.DuplicateDeployments += $deployment
	}
	[bool] HasDeployment(){
		if($this.PrimaryDeployment -eq $null){
			return $false
		}

		return $true
	}
	[bool] HasDuplicateDeployments(){
		if($this.DuplicateDeployments.Length -gt 0){
			return $true
		}

		return $false
	}
	[void] AddToErrorReport($error){
		$this.errorReport += $error
	}
	[void] AddToUpdateReport($error){
		$this.updatesReport += $error
	}
	[void] AddToFinishedReport($error){
		$this.fullReport += $error
	}
	[ValidationState] ValidateDeployment(){
		$finalErrorMessage = ""
		$hasError = $false
		if($this.Skip){
			$this.errorReport += "Skipped"
			return [ValidationState]::new($true, "Skipped")
		}

		if(-not $this.HasDeployment()){
			$this.errorReport += "<div style='color:rgb(200,0,0);font-weight:bold'>No Configuration Found</div>"
			return [ValidationState]::new($false, "No Configuration Found")
		}

		if($this.HasDuplicateDeployments()){
			$message = "Has Duplicate Deployments `r`n"
			$message += "   $($this.ToString()) `r`n"
			foreach($deployment in $this.DuplicateDeployments){
				$message += "   $($deployment.ToString()) `r`n"
			}
			$this.errorReport += "<div style='color:rgb(200,0,0);font-weight:bold'>$($message)</div>"
			return [ValidationState]::new($false, "$message")
		}
		
		if($this.PrimaryDeployment.RegisteredAdd -eq $null){
			$this.errorReport += "<div style='color:rgb(200,0,0);font-weight:bold'>No Associated AAD found</div>"
			$this.errorReport += "No Associated AAD found"
			return [ValidationState]::new($false, "No Associated AAD found")
		}
		
		$message = "App Settings `r`n"
		$validation = $this.PrimaryDeployment.AppConfiguration.ValidateCollection()
		$message += "$($validation.Message) `r`n"
		if(-not $validation.Valid) {
			$finalErrorMessage+=$message
			$hasError = $true
			$this.errorReport += "<div style='color:rgb(200,0,0);font-weight:bold'>$($message)</div>"
		}
		
		$message = "Connection Strings `r`n"
		$validation = $this.PrimaryDeployment.ConnectionStrings.ValidateCollection()
		$message += "$($validation.Message) `r`n"
		if(-not $validation.Valid) {
			$finalErrorMessage+=$message
			$hasError = $true
			$this.errorReport += "<div style='color:rgb(200,0,0);font-weight:bold'>$($message)</div>"
		}
		
		$message = "Key Vault Secrets `r`n"
		$validation = $this.PrimaryDeployment.KeyVaultSecrets.ValidateCollection()
		$message += "$($validation.Message) `r`n"
		if(-not $validation.Valid) {
			$finalErrorMessage+=$message
			$hasError = $true
			$this.errorReport += "<div style='color:rgb(200,0,0);font-weight:bold'>$($message)</div>"
		}
		
		if($hasError) {
			return [ValidationState]::new($false, $finalErrorMessage)
		}
		
		return [ValidationState]::new($true, "All Valid")
	}
	[void] ApplyDeployment(){
		$this.ApplyDeployment($true)
	}
	[void] ApplyDeployment([bool]$SkipAppendingAppSettings){
		$_keyVaultChanges = ""
		$_appSettingsChanges = ""
		$_connectionStringsChanges = ""
		
		foreach($warning in $this.Warnings){
			Write-Warning "$($this.Name): $($warning)"
		}

		if($this.Skip){
			return
		}

		# Set-up Key Vault Values
		$__ = "`r`n  Configure Key Vault Secrets"; Write-Host $__ ; $__="<h2>$__</h2>"; $this.fullReport+=$__; 
		foreach($keyVault in $this.PrimaryDeployment.KeyVaultSecrets.KeyVaults()){
			$_keyVaultHeader  = ""
			$_keyVaultChanges = ""
			
			$__ = "`r`n    Secrets in Key Vault($($keyVault.KeyVault.Name)) for $($this.Name):"; Write-Host $__ ;$__="<h3 style='margin-left:30px'>$__</h3>";$this.fullReport+=$__; $_keyVaultHeader=$__;			
			$__ = "      Found Changes";Write-Host $__ ;$__="<h4  style='margin-left:60px'>$__</h4>"; $this.fullReport+=$__; $_keyVaultHeader+=$__
			
			$_ = "<table style='margin-left:80px'>"; $this.fullReport+=$_;$_keyVaultHeader +=$_
			$keyVault.KeyVaultSecrets.GetEnumerator() | % {
				Try {
					$keyVaultSecretName = $_.key
					$oldValue = $keyVault.KeyVault.Get("Secrets")[$_.key]
					if($oldValue){
						$oldValue = $oldValue.Value
					}
					else{
						$keyVault.KeyVault.FetchSecret($_.key,$_.key,"")
						$oldValue = $keyVault.KeyVault.Get("Secrets")[$_.key]
						if($oldValue){
							$oldValue = $oldValue.Value
						}
					}
					$newValue = $_.value
					
					$secretvalue = $_.value | ConvertTo-SecureString -AsPlainText -Force 
					
					if($oldValue -eq $newValue){
						$_ = "<tr>"; $this.fullReport += $_; 
						$_ = "      '$($keyVaultSecretName)' up-to-date "; Write-Host $_ ;$_="<td><b>$($keyVaultSecretName)</b></td><td><span style='color:rgb(0,180,0)'>up-to-date</span></td>"; $this.fullReport+=$_
						$_ = "</tr>"; $this.fullReport += $_; 
					}
					else{
						$_ = "<tr>"; $this.fullReport += $_; $_keyVaultChanges+=$_
						$_ = "      '$($keyVaultSecretName)' *Changed* ";  Write-Host $_ ;$_="<td><b>$($keyVaultSecretName)</b></td><td><div style='text-decoration: line-through;color:rgb(200,0,0)'>$oldValue</div><div style='font-weight:bold;color:rgb(0,200,0)'>$newValue</div></td>"; $_keyVaultChanges+=$_; $this.fullReport += $_
						$_ = "         [OLD] '$($oldValue)'"; Write-Host $_ 
						$_ = "         [NEW] '$($newValue)'"; Write-Host $_ 
						$_ = "</tr>"; $this.fullReport += $_; $_keyVaultChanges+=$_
						
						$secret = Set-AzureKeyVaultSecret -VaultName $keyVault.KeyVault.SiteName -Name $keyVaultSecretName -SecretValue $secretvalue
						$secretvalueFromKeyVault = Get-AzureKeyVaultSecret -VaultName $keyVault.KeyVault.SiteName -Name $keyVaultsecretName
					}
				}
				catch {
					$_ = "Error(Adding Key Vault Secrets)"; Write-Host $_ ;$_="<div style='color:rgb(200,0,0)'>$($_)</div>"; $this.errorReport += $_; $this.fullReport+=$_
					$_ = "  Message: $($_.Exception.Message)";  Write-Host $_ ;$_="<div style='color:rgb(200,0,0)'>$($_)</div>"; $this.errorReport += $_;$this.fullReport+=$_
					$_ = "  Item...: $($_.Exception.FailedItem)";  Write-Host $_ ;$_="<div style='color:rgb(200,0,0)'>$($_)</div>"; $this.errorReport += $_;$this.fullReport+=$_
				}
			 }
			 $_ = "</table>";$this.fullReport+=$_;
			 
			 if(-not [string]::IsNullOrEmpty($_keyVaultChanges)){
				$this.updatesReport += "$($_keyVaultHeader)`r`n$($_keyVaultChanges)</table>"
			 }
				
			 $_ = "`r`n      Full List"; Write-Host $_ ;$_="<h4  style='margin-left:60px'>$_</h4>"; $this.fullReport+=$_
			 $_ = "<table style='margin-left:80px'>"; $this.fullReport+=$_;
			 $keyVault.KeyVaultSecrets.GetEnumerator() | % {
				$key=$_.key
				$value=$_.value
				$_ = "<tr>"; $this.fullReport += $_; 
				$_ = "        '$($key)' => '$($value)'"; Write-Host $_ ;$_="<td style='font-weight:bold;'>$($key)</td><td>$($value)</td>"; $this.fullReport+=$_
				$_ = "</tr>"; $this.fullReport += $_; 
			 }
			 $_ = "</table>";$this.fullReport+=$_;
			 
			 $_ = "`r`n      Access Control"; Write-Host $_ ;$_="<h4  style='margin-left:60px'>$_</h4>"; $this.fullReport+=$_
			 if ($keyVault.KeyVault.SiteName -ne "") {
				 $_ = "         Give Access to ($($this.PrimaryDeployment.RegisteredADD.ClientId))";  Write-Host $_ ;$_="<div style='margin-left:90px'>$($_)</div>"; $this.fullReport+=$_
				 # Giving access to the Client API for retrieving the Secrets
				 Set-AzureRmKeyVaultAccessPolicy -VaultName $keyVault.KeyVault.SiteName -ServicePrincipalName $this.PrimaryDeployment.RegisteredADD.ClientId -PermissionsToSecrets    get,list,set,delete,backup,restore,recover,purge 
			 }
		}
		
		$_ = "`r`n  Successfully Updated Key Vault Secrets for App Service($($this.SiteName))"; Write-Host $_ ;$_="<div style='color:rgb(0,150,0);font-weight:bold'>$($_)</div>"; $this.fullReport+=$_
		
		$appSettingList        = (Invoke-AzureRMResourceAction -ResourceGroupName $this.ResourceGroup -ResourceType 'Microsoft.Web/sites/Config' -Name "$($this.SiteName)/appsettings" -Action list -ApiVersion 2015-08-01 -Force).Properties
		$connectionStringsList = (Invoke-AzureRMResourceAction -ResourceGroupName $this.ResourceGroup -ResourceType 'Microsoft.Web/sites/Config' -Name "$($this.SiteName)/connectionstrings" -Action list -ApiVersion 2015-08-01 -Force).Properties

		$_ = "`r`n  Azure App Settings for $($this.Name)"; Write-Host $_ ;$_="<h2>$_</h2>"; $_appSettingsHeader +=$_; $this.fullReport += $_
		# Build App Settings
		$appSettings = @{}
		if($SkipAppendingAppSettings){
			$appSettingList.psobject.properties | Foreach { $appSettings[$_.Name] = $_.Value}
		}

		$appSettings["WEBSITE_NODE_DEFAULT_VERSION"] = "6.9.1"
		$_ = "      Changes Found"; Write-Host $_ ;$_="<h4  style='margin-left:60px'>$_</h4>"; $this.fullReport+=$_;$_appSettingsHeader +=$_
		
		$_ = "<table style='margin-left:80px'>"; $this.fullReport+=$_;$_appSettingsHeader +=$_
		$this.PrimaryDeployment.AppConfiguration.GetEnumerator() | % {
			$appKey = $_.key
			$oldValue = $appSettings[$appKey]
			$newValue = $_.value
			if($appSettings.ContainsKey($appKey)){
				if(-not ($oldValue -eq $newValue)){
					
					$_ = "<tr>"; $this.fullReport += $_; $_appSettingsChanges+=$_
					$_ = "      '$($appKey)' *Changed* ";  Write-Host $_ ;$_="<td><b>$($appKey)</b></td><td><div style='text-decoration: line-through;color:rgb(200,0,0)'>$oldValue</div><div style='color:font-weight:bold;rgb(0,200,0)'>$newValue</div></td>"; $this.fullReport += $_; $_appSettingsChanges+=$_
					$_ = "         [OLD] '$($oldValue)'";  Write-Host $_ 
					$_ = "         [NEW] '$($newValue)'";  Write-Host $_ 
					$_ = "</tr>"; $this.fullReport += $_; $_appSettingsChanges+=$_
				}
				else{
					$_ = "<tr>"; $this.fullReport += $_; 
					$_ = "      '$($appKey)' up-to-date "; Write-Host $_ ;$_="<td><b>$($appKey)</b></td><td><span style='color:rgb(0,180,0)'>up-to-date</span></td>"; $this.fullReport += $_; 
					$_ = "</tr>"; $this.fullReport += $_;
				}
			}
			$appSettings[$appKey] =  $newValue
		}
		$_ = "</table>";$this.fullReport+=$_;
		
		$_ = "    Full List"; Write-Host $_ ;$_="<h4  style='margin-left:60px'>$_</h4>"; $this.fullReport+=$_
		$_ = "<table style='margin-left:80px'>"; $this.fullReport+=$_;
		$appSettings.GetEnumerator() | % {
		   $key = $_.key
		   $value=$_.value
		   $_ = "<tr>"; $this.fullReport += $_; 
		   $_ = "        '$($key)' => '$($value)'"; Write-Host $_ ;$_="<td><b>$($key)</b></td><td>$($value)</td>"; $this.fullReport+=$_
		   $_ = "</tr>"; $this.fullReport += $_;
		}
		$_ = "</table>"; $this.fullReport+=$_;
		
		if(-not [string]::IsNullOrEmpty($_appSettingsChanges)){
			$this.updatesReport += "$($_appSettingsHeader)`r`n$($_appSettingsChanges)</table>"
		}
		
		$_ = "`r`n  Azure Connection Strings for $($this.Name)"; Write-Host $_ ;$_="<h2  style='margin-left:60px'>$_</h2>"; $_connectionStringsHeader+=$_; $this.fullReport += $_
		# Build Connection Strings
		$connectionStrings = @{}
		if($SkipAppendingAppSettings){
			$connectionStringsList.psobject.properties | Foreach { $connectionStrings[$_.Name] = @{Type=$_.Value.Type; Value=$_.Value.value}}
		}
		
		$_ = "      Changes Found"; Write-Host $_ ;$_="<h4  style='margin-left:60px'>$_</h4>"; $this.fullReport+=$_;$_connectionStringsHeader +=$_
		$_ = "<table style='margin-left:80px'>"; $this.fullReport+=$_;$_connectionStringsHeader +=$_
		$this.PrimaryDeployment.ConnectionStrings.GetEnumerator() | % {
			$key = $_.key
			$value=$_.value
			$oldValue = $connectionStrings[$key]
			if($oldValue){
				$oldValue = $oldValue.Value
			}
			$newValue = $value
			
			if(-not ($oldValue -eq $newValue)){
				$_ = "<tr>"; $this.fullReport += $_; $_connectionStringsChanges+=$_
				$_ = "      '$($key)' *Changed* ";  Write-Host $_ ;$_="<td><b>$($key)</b></td><td><div style='text-decoration: line-through;color:rgb(200,0,0)'>$oldValue</div><div style='font-weight:bold;color:rgb(0,200,0)'>$newValue</div></td>"; $this.fullReport += $_; $_connectionStringsChanges+=$_
				$_ = "         [OLD] '$($oldValue)'"; Write-Host $_ ;
				$_ = "         [NEW] '$($newValue)'"; Write-Host $_ ;
				$_ = "</tr>"; $this.fullReport += $_; $_connectionStringsChanges+=$_
			}
			else{
				$_ = "<tr>"; $this.fullReport += $_; 
				$_ = "      '$($key)' up-to-date "; Write-Host $_ ;$_="<td><b>$($key)</b></td><td><span style='color:rgb(0,180,0)'>up-to-date</span></td>"; $this.fullReport += $_; 
				$_ = "</tr>"; $this.fullReport += $_; 
			}
			if($connectionStrings[$key]){
				$connectionStrings[$key] = @{ Type = 'SQLAzure'; Value = $value}
			}
			else{
				$connectionStrings.add($key,@{ Type = 'SQLAzure'; Value = $value})
			}
			
		}
		$_ = "</table>"; $this.fullReport+=$_;
		
		if(-not [string]::IsNullOrEmpty($_connectionStringsChanges)){
			$this.updatesReport += "$($_connectionStringsHeader)`r`n$($_connectionStringsChanges)</table>"
		}
				
		if($connectionStrings.Count -eq 0){
			$connectionStrings["NONE"] = @{ Type = 'SQLAzure'; Value = "NONE"}
		}
		
		$_ = "    Full List"; Write-Host $_ ;$_=$_="<h4  style='margin-left:60px'>$_</h4>"; $this.fullReport+=$_
		$_ = "<table style='margin-left:80px'>"; $this.fullReport+=$_;
		$connectionStrings.GetEnumerator() | % {
			$key = $_.key
			$value=$_.value.Value
			$_ = "<tr>"; $this.fullReport += $_; 
			$_ = "        '$($key)' => '$($value)'"; Write-Host $_ ;$_="<td style='font-weight:bold;'>$($key)</td><td>$($value)</td>"; $this.fullReport+=$_
			$_ = "</tr>"; $this.fullReport += $_; 
		}
		$_ = "</table>"; $this.fullReport+=$_;
		
		try{
			Set-AzureRMWebAppSlot -ResourceGroupName $this.ResourceGroup -Name $this.SiteName -AppSettings $appSettings -ConnectionStrings $connectionStrings -Slot Production
			
			$_ = "`r`n  Successfully Updated Azure App Settings for App Service($($this.SiteName))"; Write-Host $_ ;$_="<div style='color:rgb(0,150,0);font-weight:bold'>$($_)</div>"; $this.fullReport+=$_
		}
		catch  [Exception] {
			$_ = "Error(Setting Configuration to App Services)";$_="<div style='color:rgb(200,0,0)'>$($_)</div>"; $this.errorReport += $_; $errorMessage=$_; 
			$_ = "  Message: $($_.Exception.Message)";$_="<div style='color:rgb(200,0,0)'>$($_)</div>"; $this.errorReport += $_; $errorMessage+=$_; 
			$_ = "  Item...: $($_.Exception.FailedItem)";$_="<div style='color:rgb(200,0,0)'>$($_)</div>"; $this.errorReport += $_; $errorMessage+=$_; 

			echo $_.Exception|format-list -force
			
			Write-Error $errorMessage
		}
		
	}
	[string] ToString() {
		return "Group($($this.Group)), Name($($this.Name)), ResourceGroup($($this.ResourceGroup)), SiteName($($this.SiteName)))"
	}
}
class Notification{
	[hashtable]    $EmailsToNotifyOnError
	[hashtable]    $EmailsToNotifyOnChange
	[hashtable]    $EmailsToNotifyOnFinished
	
	[Provision[]] $_provisions
	
	[string]       $errorReport
	[string]       $updatesReport
	[string]       $fullReport
	
	[string]       $Name
	Notification([string]$name){
		$this._provisions              = @()
		$this.EmailsToNotifyOnError    = @{}
		$this.EmailsToNotifyOnChange   = @{}
		$this.EmailsToNotifyOnFinished = @{}
		$this.Name=$name
	}
	AddProvision([Provision] $provision){
		Write-Host "*** Adding Provision $($provision.Name)"
		$this._provisions+=$provision
	}
	AddProvisions([Provision[]] $provisions){
		foreach($provision in $provisions){
			$this.AddProvision($provision)
		}
	}
	UpdateLocalReports(){
		$this.errorReport=""
		$this.updatesReport=""
		$this.fullReport=""
		
		foreach($provision in $this._provisions){
			if($provision.ValidateOnly){
				continue
			}
			if(-not [string]::IsNullOrEmpty($provision.errorReport)){
				$this.errorReport+="<div>$($provision.errorReport)</div>"
			}
			if(-not [string]::IsNullOrEmpty($provision.updatesReport)){
				$this.updatesReport+="<div>$($provision.updatesReport)</div>"
			}
			if(-not [string]::IsNullOrEmpty($provision.fullReport)){
				$this.fullReport+="<div>$($provision.fullReport)</div>"
			}
		}		
	}
	
	NotifyOnErrors([string]$email, [string]$name = "N/A"){
		if([string]::IsNullOrEmpty($email)){
			Write-Error "Attempting to subscribe null or empty email to the notifications for deployment errors of '$($this.Name)"
			return
		}
		if($this.EmailsToNotifyOnError.ContainsKey($email)){
			Write-Error "Email '$(email)' added to error notifications for deployment errors of '$($this.Name)"
			return
		}
		$this.EmailsToNotifyOnError[$email] = $name
	}
	NotifyOnUpdates([string]$email, [string]$name = "N/A"){
		if([string]::IsNullOrEmpty($email)){
			Write-Error "Attempting to subscribe null or empty email to the notifications for deployment updates of '$($this.Name)"
			return
		}
		if($this.EmailsToNotifyOnChange.ContainsKey($email)){
			Write-Error "Email '$(email)' added to error notifications for deployment updates of '$($this.Name)"
			return
		}
		$this.EmailsToNotifyOnChange[$email] = $name
	}
	NotifyOnFinished([string]$email, [string]$name = "N/A"){
		if([string]::IsNullOrEmpty($email)){
			Write-Error "Attempting to subscribe null or empty email to the notifications for deployment finished of '$($this.Name)"
			return
		}
		if($this.EmailsToNotifyOnFinished.ContainsKey($email)){
			Write-Error "Email '$(email)' added to error notifications for deployment finished of '$($this.Name)"
			return
		}
		$this.EmailsToNotifyOnFinished[$email] = $name
	}
	NotifyForErrors(){
		$this.UpdateLocalReports()
		
		if([string]::IsNullOrEmpty($this.errorReport)){
			Write-Host "Notify on Errors: Skipped, Nothing to report"
			return 
		}
		if([string]::IsNullOrEmpty($env:RELEASE_RELEASENAME)){
			Write-Warning "Notify on Errors: Skipped, script not running in pipeline"
			return 
		}
		$this.EmailsToNotifyOnError.GetEnumerator() | % {
			SendEmail $_.key $_.value "Config Pipeline (FAILED) ($(GetEnv("RELEASE_DEFINITIONNAME")))  ($(GetEnv("RELEASE_ENVIRONMENTNAME")))" "Config Errors - $($this.Name) - $(GetEnv("RELEASE_RELEASENAME"))" $this.errorReport $true
		}
	}
	NotifyForUpdates(){
		$this.UpdateLocalReports()
		
		if([string]::IsNullOrEmpty($this.updatesReport)){
			Write-Host "Notify on Updates: Skipped, Nothing to report"
			return 
		}
		if([string]::IsNullOrEmpty($env:RELEASE_ENVIRONMENTNAME)){
			Write-Warning "Notify on Updates: Skipped, script not running in pipeline"
			return 
		}
		$this.EmailsToNotifyOnChange.GetEnumerator() | % {
			SendEmail $_.key $_.value "Config Pipeline (CHANGES) ($(GetEnv("RELEASE_DEFINITIONNAME"))) ($(GetEnv("RELEASE_ENVIRONMENTNAME")))" "Config Updated - $($this.Name) - $(GetEnv("RELEASE_RELEASENAME"))" $this.updatesReport $true
		}
	}
	NotifyForFinished(){
		$this.UpdateLocalReports()
		
		if([string]::IsNullOrEmpty($this.fullReport)){
			Write-Host "Notify on Finished: Skipped, Nothing to report"
			return 
		}
		if([string]::IsNullOrEmpty($env:RELEASE_ENVIRONMENTNAME)){
			Write-Warning "Notify on Finished: Skipped, script not running in pipeline"
			return 
		}
		$this.EmailsToNotifyOnFinished.GetEnumerator() | % {
			SendEmail $_.key $_.value "Config Pipeline (RELEASED) ($(GetEnv("RELEASE_DEFINITIONNAME")))  ($(GetEnv("RELEASE_ENVIRONMENTNAME")))" "Config Released - $($this.Name) - $(GetEnv("RELEASE_RELEASENAME"))" $this.fullReport $true
		}
	}
	
}
class ProvisionCollection {
	
	[Provision[]] $_provisions
	[Notification[]] $_notifications
	
	ProvisionCollection() {
		
	}
	[Notification] CreateNotification([string]$name){
		$notification = [Notification]::new($name)
		$this._notifications+=$notification
		return $notification
	}
	NotifyForErrors(){
		foreach($notification in $this._notifications){
			$notification.NotifyForErrors()
		}
	}
	NotifyForUpdates(){
		foreach($notification in $this._notifications){
			$notification.NotifyForUpdates()
		}
	}
	NotifyForFinished(){
		foreach($notification in $this._notifications){
			$notification.NotifyForFinished()
		}
	}
	[Provision] AddFakeResource([string]$group, [string]$name, [hashtable]$Properties){
		$provision = $this.Add($group,
				  $name,
				  "[MOCKED]-RESOURCE-TYPE",
				  "[MOCKED]-RESOURCE-GROUP",
				  "[MOCKED]-SITE-NAME-$name",
				  $Properties)
		$provision.ValidateOnly = $true
		$provision.Url = "https://[MOCKED]-Url-$name"
		return $provision
	}
	[Provision] AddAzureResource([string]$group, [string]$displayName, [string]$name){
		return $this.AddAzureResource($group, $displayName, $name, $null)
	}
	[Provision] AddAzureResource([string]$group, [string]$displayName, [string]$name, [string]$appInsightsInstrumentalKey){
		$resource = Get-AzureRmResource -Name $name -ExpandProperties 
		
		if(-not $resource){
			return $this.AddFakeResource($group, $name, @{})
		}
		
		Write-Host "Adding Resource $($group).$($name): $($displayName) - $($resource.Tags.displayName)"
		$properties=@{}
		# Using ResourceTypes and ResourceType (singular) for compatibility reasons
		if($resource.ResourceTypes -ne "Microsoft.KeyVault/vaults" -and $resource.ResourceType -ne "Microsoft.KeyVault/vaults"){
			try{
				($resource.Properties | Foreach { $properties[$_.Name] = $_.Value })
			}
			catch{
				Write-Warning "Unable to fetch resource properties"
			}
		}
			
		# Add Instrumental Key
		$properties["APPINSIGHTS_INSTRUMENTATIONKEY"] = $appInsightsInstrumentalKey
		
		$provision= $this.Add($group,
				  $displayName,
				  $resource.ResourceType,
				  $resource.ResourceGroupName,
				  $name,
				  $properties)
				  
		$provision.Url = Get-AzureWebAppUrl $name
		return $provision
		
	}
	[Provision] Get([string]$group, [string] $name) {
		
		$foundProvision = $null
		foreach ($element in $this._provisions) {
			if ( $element.Name -eq $name -and $element.Group -eq $group) {
				$foundProvision = $element
			}
		}

		if($foundProvision -eq $null) {
			Throw "Provisioned Resource $($group)[$($name)] was not found"
		}
		return $foundProvision
	}
	[Provision[]] Get([string]$group) {
		$foundProvisions = [Provision[]] @()
		foreach ($element in $this._provisions) {
			if ( $element.Group -eq $group) {
				$foundProvisions += $element
			}
		}

		return $foundProvisions
	}
	[Provision[]] Query([string]$queryIds) {
		
		$RG_and_RN = [Regex]::Match($queryIds, "^([^\/]*)\/([^\/]*)$")
		if(-not $RG_and_RN.Success){
			throw "Query for configurable resources is no in the correct format '$($queryIds)' ('/configName', 'configGroup/', 'configGroup/configName')"
		}
		
		$group  = $RG_and_RN.Groups[1].Value
		$name   = $RG_and_RN.Groups[2].Value
		
		$foundProvisions = [Provision[]] @()
		foreach ($element in $this._provisions) {
			
			if(-not ([String]::IsNullOrEmpty($group) -or $group -eq  $element.Group)){
				continue
			}
			if(-not ([String]::IsNullOrEmpty($name) -or $name -eq  $element.Name)){
				continue
			}
			$foundProvisions += $element
		}

		return $foundProvisions
	}
	[Provision] Add([string]$Group, 
		[string]$Name, 
		[string]$ResourceType,
		[string]$ResourceGroup,
		[string]$SiteName,
		[hashtable]$Properties) {
		
		$provision = ([Provision]::new($Group, $Name,$ResourceType, $ResourceGroup, $SiteName,$Properties))
		$this._provisions += $provision
		return $provision
	}
}
class AADApplication {
	[string]$Group
	[string]$Name
	[string]$TenantId
	[string]$Domain
	[string]$ClientId
	[string]$AppUrl
	[string]$Secret

	AADApplication() {
		$this.Init("N\A",
				   "N\A",
				   "N\A",
				   "N\A",
				   "N\A",
				   "N\A",
				   "N\A") 
	}
	AADApplication([string]$Group,
					[string]$Name,
					[string]$TenantId,
					[string]$Domain,
					[string]$ClientId,
					[string]$AppUrl,
					[string]$Secret) {
		$this.Init($Group,
				   $Name,
				   $TenantId,
				   $Domain,
				   $ClientId,
				   $AppUrl,
				   $Secret)

	}
	hidden Init( [string]$Group,
					[string]$Name,
					[string]$TenantId,
					[string]$Domain,
					[string]$ClientId,
					[string]$AppUrl,
					[string]$Secret ) {
		$this.Group=$Group
		$this.Name=$Name
		$this.TenantId = $TenantId
		$this.Domain = $Domain
		$this.ClientId=$ClientId
		$this.AppUrl=$AppUrl
		$this.Secret=$Secret

	 }
	 [string] ToString() {
		return "Group($($this.Group)), Name($($this.Name)), ClientId($($this.ClientId)), AppUrl($($this.AppUrl)), Secret($($this.Secret))"
	}
}
class AADApplicationCollection {
	
	[AADApplication[]] $_aadApplication
	AADApplicationCollection() {
		
	}
	[AADApplication] Get([string]$group, [string] $name) {
		$foundAadApplication = $null
		$found = $false
		foreach ($element in $this._aadApplication) {
			if ( $element.Name -eq $name -and $element.Group -eq $group) {
				$found =$true
				$foundAadApplication = $element
			}
		}

		if(-not $found){
			Throw "Azure ADD $($group)[$($name)] was not found"
		}
		return $foundAadApplication
	}

	Add([string]$Group,
		[string]$Name,
		[string]$TenantId,
		[string]$Domain,
		[string]$ClientId,
		[string]$AppUrl,
		[string]$Secret) {
		
		$this._aadApplication += ([AADApplication]::new($Group,
														$Name, 
														$TenantId,
														$Domain,
														$ClientId, 
														$AppUrl,
														$Secret))
	}
}

class ConfigRunState {
	[bool]       $Successfull
	[Deployment] $Deployment
	[string]     $Message

	ConfigRunState([bool] $Successfull, [Deployment] $Deployment, [string] $Message) {
		$this.Successfull=$Successfull
		$this.Deployment=$Deployment
		$this.Message=$Message
	}

	ConfigRunState() {
		$this.Successfull=$false
		$this.Deployment=$null
		$this.Message=""
	}

}
class ConfigurationSetup {
	[string]                  $PowershellScript
	[ProvisionCollection]     $Provisions
	[AADApplicationCollection]$AadCollection
	[int]                     $Id
	static [int]              $NumberInitialized = 0
	ConfigurationSetup([string]$powershellScript, [ProvisionCollection]$provisions, [AADApplicationCollection]$aadCollection) {
		$this.PowershellScript = $powershellScript
		$this.Provisions = $provisions
		$this.AadCollection = $aadCollection
		$this.Id = [ConfigurationSetup]::NumberInitialized+1
		[ConfigurationSetup]::NumberInitialized += 1
	}

	[void] RunConfigSetup() {
		
		$finalState=[ConfigRunState]::new()
	   
		$arguments = $this.AadCollection, $this.Provisions
		$exe=$this.PowershellScript 
		
		$powershellContent = [System.IO.File]::ReadAllText($exe)
		$newPowerShell=$powershellContent -replace ('\$env\:([\w_\d]*)', '$(GetEnv("$1"))')
		[System.IO.File]::WriteAllText( $exe, $newPowerShell)
		&$exe $this.AadCollection $this.Provisions 
		[System.IO.File]::WriteAllText( $exe, $powershellContent)
	}
	[string] ToString(){
		return ("CNG({0:D8}): {1}" -f $this.Id, $this.PowershellScript)

	}
}
class ConfigurationDirectorySetup{
	[string]                  $Folder
	[string]                  $FileName
	[ProvisionCollection]     $Provisions
	[AADApplicationCollection]$AadCollection
	[ConfigurationSetup[]]    $ConfigurationSetups
	hidden [bool] $ConfigsSearched
	hidden [bool] $ConfigsRun

	ConfigurationDirectorySetup([string]$Folder,[string]$FileName, [ProvisionCollection]$provisions, [AADApplicationCollection]$aadCollection) {
		$this.Folder              = $Folder
		$this.FileName            = $FileName
		$this.Provisions          = $provisions
		$this.AadCollection       = $aadCollection
		$this.ConfigurationSetups = $()
		$this.ConfigsSearched=$false
		$this.ConfigsRun=$false
	}

	[void] StartConfigBroadcast(){
		Write-Host "`r`nSearching for '$($this.FileName)' in '$($this.Folder)'"
		$this.ConfigsSearched = $true
		$Files = Get-ChildItem -Path $this.Folder -Filter $this.FileName -Recurse 
		$Files | Foreach-Object {
			if(-not ($_.FullName -like "*PackageTmp*" )){
				$config = [ConfigurationSetup]::new($_.FullName, $this.Provisions, $this.AadCollection)
				$this.ConfigurationSetups += $config
				Write-Host "  $($config.ToString())"
			}
		}

	}
	[void] RunConfigs(){
		if(-not $this.ConfigsSearched){
			$this.StartConfigBroadcast()
		}

		Write-Host "`r`nRunning Configs"
		foreach ($config in $this.ConfigurationSetups) {     
			
			Write-Host "  $($config.ToString())"
			try{
				$config.RunConfigSetup()
				Write-Host "   Successfull"
			}
			catch{
			   Write-Error "Config Failed `r`n  Message: $($_.Exception.Message)`r`n  Item: $($_.Exception.ItemName)"
			}

		}
		$this.ConfigsRun = $true
	}
	[void] ValidateAllConfigs([string]$ProvisionGroup){
		 if(-not $this.ConfigsRun){
			$this.RunConfigs()
		}

		Write-Host "`r`Validate Configs"
		foreach ($provision in $this.Provisions.Get($ProvisionGroup)) {
			$validation = $provision.ValidateDeployment()

			if($validation.Valid) {
				Write-Host "$($provision.Name) Validation Passed"
			}
			else {
				Write-Error "$($provision.Name)`r`n$($validation.Message)"
			}
			Write-Host "`r`n"
		}  
	}
	[void] DeployConfigs([string]$configureQuery, [bool]$SkipAppendingAppSettings){
		if(-not $this.ConfigsRun){
			$this.RunConfigs()
		}

		Write-Host "`r`nDeploying Configs"
		foreach ($provision in $this.Provisions.Query($configureQuery)) {
			$validation = $provision.ValidateDeployment()

			if($validation.Valid) {
				Write-Host "$($provision.Name)"
				
				if(-not $provision.ValidateOnly){
					$provision.ApplyDeployment($SkipAppendingAppSettings)
				}
			}
			else {
				Write-Error "$($provision.Name)`r`n$($validation.Message)"
			}
			Write-Host "`r`n"
			
			$provision.NotifyForErrors()
			$provision.NotifyForUpdates()
			$provision.NotifyForFinished()
		}  
		$this.Provisions.NotifyForErrors()
		$this.Provisions.NotifyForUpdates()
		$this.Provisions.NotifyForFinished()

	}


}


Function New-AlphaCmConfigurationSetup([string]$ConfigScript, [ProvisionCollection]$Provisions, [AADApplicationCollection]$AADCollection) {
	return [ConfigurationSetup]::new($ConfigScript, $Provisions, $AADCollection)
}
Function New-AlphaCmAADApplicationCollection(){
	return [AADApplicationCollection]::new()
}
Function New-AlphaCmProvisionCollection(){
	return [ProvisionCollection]::new()
}

