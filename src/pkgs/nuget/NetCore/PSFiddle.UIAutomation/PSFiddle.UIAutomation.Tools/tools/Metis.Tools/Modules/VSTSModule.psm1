class VSTSModule{
	[string] $PersonalAccessToken
	[string] $Account
	VSTSModule([string]$personalAccessToken, [string]$account){
		$this.PersonalAccessToken = $personalAccessToken
		$this.Account             = $account
	}
	#{
	#	"allChangesIncluded": true,
	#	"changeCounts": {
	#		"Edit": 16
	#	},
	#	"changes": [
	#	{
	#		"item": {
	#		"objectId": "5fa82b08cb1e03f38dbc4a3948e51cb44d4179cc",
	#		"originalObjectId": "f386dd79e29eafa06fc31d1cacf54b5ef15ddff4",
	#		"gitObjectType": "tree",
	#		"commitId": "2e3bb2e640680987482241cc3a07428aa2c89fe7",
	#		"path": "/src",
	#...
	[object]GetTestPointsOfTestSute([string]$testPlanId, [string]$testSuiteId){	    
		$url = "https://dev.azure.com/$($this.Account)/ProjectAlpha/_apis/test/Plans/$($testPlanId)/Suites/$($testSuiteId)/points?api-version=5.0-preview.2"	
		$commitObj =$this.InvokeRestCall($url, $null, "Get")
		
		$ids = $commitObj.value | Foreach { $_.id}
		Write-Host "$($ids)"
		return $ids
	}
	#{
	#	"allChangesIncluded": true,
	#	"changeCounts": {
	#		"Edit": 16
	#	},
	#	"changes": [
	#	{
	#		"item": {
	#		"objectId": "5fa82b08cb1e03f38dbc4a3948e51cb44d4179cc",
	#		"originalObjectId": "f386dd79e29eafa06fc31d1cacf54b5ef15ddff4",
	#		"gitObjectType": "tree",
	#		"commitId": "2e3bb2e640680987482241cc3a07428aa2c89fe7",
	#		"path": "/src",
	#...
	[object]CreateTestRun([string] $body){	    
		$url = "https://dev.azure.com/$($this.Account)/ProjectAlpha/_apis/test/runs?api-version=5.0-preview.2"
		
		
		
		$commitObj =$this.InvokeRestCall($url, $body, "Post")
		
		
		return $commitObj
	}
	#{
	#	"allChangesIncluded": true,
	#	"changeCounts": {
	#		"Edit": 16
	#	},
	#	"changes": [
	#	{
	#		"item": {
	#		"objectId": "5fa82b08cb1e03f38dbc4a3948e51cb44d4179cc",
	#		"originalObjectId": "f386dd79e29eafa06fc31d1cacf54b5ef15ddff4",
	#		"gitObjectType": "tree",
	#		"commitId": "2e3bb2e640680987482241cc3a07428aa2c89fe7",
	#		"path": "/src",
	#...
	[object]GetCommitChanges([string]$repositoryId, [string]$previousCommit, [string]$currentCommit){
		$url = "https://$($this.Account).visualstudio.com/DefaultCollection/_apis/git/repositories/$($repositoryId)/diffs/commits?baseVersionType=commit&baseVersion=$($previousCommit)&targetVersionType=commit&targetVersion=$($currentCommit)&api-version=1.0"	
		$commitObj =$this.InvokeRestCall($url, $null, "Get")
		
		return $commitObj
	}
	#{
	#	"count": 1,
	#	"value": [
	#	{
	#		"commitId": {
	#		"author...
	#...
	# Example:
	# https://mcprojectalpha.visualstudio.com/DefaultCollection/_apis/git/repositories/5a75af4e-9b71-42f8-8166-d331b693e206/commits?searchCriteria.itemPath=src\infrastructure\scripts\configurations&searchCriteria.toCommitId=c1ae8b1af1dbed961e3d5312452eb2dca0a31b30&$top=1	
	[object]GetWorkItem([string]$workId){
		$url = "https://$($this.Account).visualstudio.com/DefaultCollection/_apis/wit/workitems/$($workId)?api-version=4.1"
		$resultObj =$this.InvokeRestCall($url, $null, "Get")
		
		return $resultObj
	}
	
	#{
	#	"count": 1,
	#	"value": [
	#	{
	#		"commitId": {
	#		"author...
	#...
	# Example:
	# https://mcprojectalpha.visualstudio.com/DefaultCollection/_apis/git/repositories/5a75af4e-9b71-42f8-8166-d331b693e206/commits?searchCriteria.itemPath=src\infrastructure\scripts\configurations&searchCriteria.toCommitId=c1ae8b1af1dbed961e3d5312452eb2dca0a31b30&$top=1	
	[object]QueryWorkItems([string]$query){
		$url = "https://$($this.Account).visualstudio.com/_apis/wit/wiql?api-version=4.1"
		
		$body = @{}
		$body["query"] = $query
		$body = ConvertTo-Json $body
		
		$body = $body -replace '\\u0027',"'"
		$body = $body -replace '\\u003c',"<"
		$body = $body -replace '\\u003e',">"		
		
		$resultObj =$this.InvokeRestCall($url, $body, "Post")
		
		return $resultObj
	}
	
	[object] InvokeRestCall([string]$url, [object]$body, [string]$method){
		Write-Host $url
		Write-Host $body
		# Base64-encodes the Personal Access Token (PAT) appropriately
		$base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f "",$this.PersonalAccessToken)))
		$obj = Invoke-RestMethod $url -Method $method -Headers @{Authorization=("Basic {0}" -f $base64AuthInfo); "Content-Type"="application/json"} -Body $body
		
		return $obj
	}
}
Function Conntect-ToVSTS{
	Param(
		[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
		[string]$personalAccessToken,
		[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
		[string]$account
	)
	
	return [VSTSModule]::new($personalAccessToken, $account)
}
Export-ModuleMember -Function Conntect-ToVSTS
# Function Invoke-VSTSRestMethod{
	# Param(
		# [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
		# [string]$url,
		# [ValidateNotNullOrEmpty()]
		# [string]$method = "Get",
		# [Parameter(Mandatory=$false)]
		# [object]$body
		
	# )

	# # Write-Host "$($method)ing '$($url)'" -ForegroundColor Cyan
	
	# # Base64-encodes the Personal Access Token (PAT) appropriately
	# $base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f "",$personalAccessToken)))
	# $obj = Invoke-RestMethod $url -Method $method -Headers @{Authorization=("Basic {0}" -f $base64AuthInfo)} -Body $body
	
	# return $obj
# }

# Function Get-Repositories(){
	# $url = "https://$($account).visualstudio.com/DefaultCollection/_apis/git/repositories?api-version=1.0"
	# $obj =Invoke-VSTSRestMethod -url $url 
	
	# $repositories = @()
	# foreach($repository in $obj.value){
		# $repositories+=$repository
		# Write-Host "Name($($repository.name)), Id($($repository.id))"
	# }
	# return $repositories
# }
# Function Get-Projects(){
	# $url = "https://$($account).visualstudio.com/DefaultCollection/_apis/projects?api-version=1.0"
	# $obj =Invoke-VSTSRestMethod -url $url 
	
	# $projects = @()
	# foreach($project in $obj.value){
		# $projects+=$project
		# Write-Host "Name($($project.Name)), Id($($project.Id))"
	# }
	# return $projects
# }
# Function Get-ProjectId([string]$projectName){
	
	# $projects = Get-Projects
	# $myProject = $projects | Where{$_.Name -eq $projectName}
	# return $myProject.Id
# }
# Function Get-RepositoryId([string]$repositoryName){
	
	# $repositories = Get-Repositories
	# $myRepository = $repositories | Where{$_.name -eq $repositoryName}
	# return $myRepository.id
# }
# Function GetCommitChanges(){
	# $url = "https://$($account).visualstudio.com/DefaultCollection/_apis/git/repositories/$($repositoryId)/commits/$($commit)?changeCount=100000&api-version=1.0"
	
	# $commitObj =Invoke-VSTSRestMethod -url $url 
	# $changes=@()
	# foreach($change in $commitObj.changes){
		# $changes+=$change.item.path
	# }
	
	# return $changes
# }
# Function Tag-Build([string]$buildNumber, [string]$tag){
	
	# $url = "https://$($account).visualstudio.com/$($projectId)/_apis/build/builds/$($buildNumber)/tags/$($tag)?api-version=2.0"
	
	# $commitObj =Invoke-VSTSRestMethod -url $url -Method Put 
# }

