param (   
    [string]$username,
    [string]$password,
    [string]$instance,
    [string]$organisation
)
Import-Module Microsoft.Xrm.Data.Powershell

$securePassword = ConvertTo-SecureString $password -AsPlainText -Force
$credentials = New-Object System.Management.Automation.PSCredential ($username, $securePassword)

$connection = Get-CrmConnection -Credential $credentials -ServerUrl $instance -OrganizationName $organisation

Import-CrmSolution -SolutionFilePath Packt_managed.zip -conn $connection -PublishChanges  
