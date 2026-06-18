
# If \WOW6432Node\Pure\PureInstallation exists then move the registry
$regKeyPath = "HKLM:\SOFTWARE\WOW6432Node\Pure\PureInstallation"
$pureregKeyPath = "HKLM:\SOFTWARE\Pure"
if ((Test-Path $regKeyPath) -and (-not(Test-Path $pureregKeyPath)))
{

# Get Application Path	
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition

$registryKeyPath = "HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Pure"
$outputRegFilePath = Join-Path -Path $scriptDir -ChildPath "exportedRegistry.reg"

& reg.exe export $registryKeyPath $outputRegFilePath /y *> $null

# Check the exit code to ensure the operation was successful
if ($LASTEXITCODE -eq 0) {
    Write-Output "Registry key exported successfully to $outputRegFilePath."
	
	#Begin (Section to update the registry)
	$inputFilePath = Join-Path -Path $scriptDir -ChildPath "exportedRegistry.reg"

	$outputFilePath = Join-Path -Path $scriptDir -ChildPath "ImportRegistry.reg"

	$oldString = "WOW6432Node"
	$newString = ""

	# Read the content of the file
	$fileContent = Get-Content -Path $inputFilePath

	# Replace the old string with the new string
	$updatedContent = $fileContent -replace $oldString, $newString
	 Write-Output "Registry content is updated successfully." 

	# Write the updated content back to the file
	Set-Content -Path $outputFilePath -Value $updatedContent
	#END (Section to update the registry)

    #Begin (Section to Import the registry)
	$regFilePath = Join-Path -Path $scriptDir -ChildPath "ImportRegistry.reg"

	# Check if the .reg file exists
	if (Test-Path $regFilePath) {
		& reg.exe import $regFilePath *> $null    
		# Check the exit code to ensure the operation was successful
		if ($LASTEXITCODE -eq 0) {       

			#remove pure registry child.
			$registryKeyPath = "HKLM:\SOFTWARE\WOW6432Node\Pure"

			# Check if the registry key exists
			if (Test-Path $registryKeyPath) {
				# Get all child keys of the specified registry key
				$childKeys = Get-ChildItem -Path $registryKeyPath
				
				# Remove each child key
				foreach ($childKey in $childKeys) {
					Remove-Item -Path $childKey.PSPath -Recurse -Force
				}	
				
			} else {
				Write-Output "Registry key not found: $registryKeyPath"
			}
	
			Write-Output "Registry moved successfully."  
		   
		} else {
			Write-Output "Failed to import registry. Exit code: $LASTEXITCODE"
		}
	} else {
		Write-Output "Registry file not found: $regFilePath"
	}
	#END (Section to Import the registry) 
	
} else {
    Write-Output "Failed to get valid registry key."
}

}else {
     Write-Output "Not a valid registry key exists"
}

# Define the service name
$serviceName = "PureWindowsService"
# Get the service object
$service = Get-Service -Name $serviceName
# Check if the service is running
if ($service.Status -eq 'Running') {
    # Stop the service
    Stop-Service -Name $serviceName -Force 
}

# Rename Pure\Application Folder
$folderPath = "C:\Pure\Application"
$newFolderName = "ApplicationBackup"
# Get the parent directory of the current folder
$parentPath = Split-Path $folderPath
# Define the new folder path by combining the parent path with the new folder name
$newFolderPath = Join-Path $parentPath $newFolderName
# Rename the folder
Rename-Item -Path $folderPath -NewName $newFolderPath
Write-Host "Pure Application folder renamed to '$newFolderName'."







