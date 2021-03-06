# CRM PowerShell Library
# Copyright (C) 2017 Arjan Meskers / AMSoftware
# 
# This program is free software: you can redistribute it and/or modify
# it under the terms of the GNU Affero General Public License as published
# by the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
# 
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU Affero General Public License for more details.
# 
# You should have received a copy of the GNU Affero General Public License
# along with this program.  If not, see <http://www.gnu.org/licenses/>.
@{
	# Version number of this module.
	ModuleVersion = '1.3.0.0'
	# ID used to uniquely identify this module
	GUID = '1e7f1ebc-e035-4d73-86af-3c07a6a85260'
	# Author of this module
	Author = 'Arjan Meskers'
	# Company or vendor of this module
	CompanyName = 'AMSoftware'
	# Copyright statement for this module
	Copyright = 'Copyright (C) 2017 Arjan Meskers / AMSoftware'
	# Description of the functionality provided by this module
	Description = 'Manage Dynamics CRM metadata and content, and administer the organization. Use on-premises and online from Dynamics CRM 2011 to Dynamics 365 CRM.'
	# Minimum version of the Windows PowerShell engine required by this module
	PowerShellVersion = '3.0'
	# Minimum version of Microsoft .NET Framework required by this module
	DotNetFrameworkVersion = '4.0'
	# Minimum version of the common language runtime (CLR) required by this module
	CLRVersion = '4.0'
	# Assemblies that must be loaded prior to importing this module
	RequiredAssemblies = @('AMSoftware.Crm.Powershell.Commands.dll', 'AMSoftware.Crm.Powershell.Common.dll', 'Microsoft.Xrm.Sdk.dll')
	# Type files (.ps1xml) to be loaded when importing this module
	TypesToProcess = 'AMSoftware.Crm.Powershell.Types.ps1xml'
	# Format files (.ps1xml) to be loaded when importing this module
	FormatsToProcess = 'AMSoftware.Crm.Powershell.Format.ps1xml'
	# Modules to import as nested modules of the module specified in RootModule/ModuleToProcess
	NestedModules = @('AMSoftware.Crm.Powershell.Commands.dll')
	# Cmdlets to export from this module
	CmdletsToExport = 'New-Team', 'New-BusinessUnit', 'New-Role', 'Get-TeamUsers', 
				   'Get-RolePrincipals', 'Get-Team', 'Get-BusinessUnit', 'Get-Role', 
				   'Get-UserTeams', 'Get-PrincipalRoles', 'Remove-UserParent', 'Set-Owner', 
				   'Set-TeamUsers', 'Set-RolePrincipals', 'Set-UserTeams', 
				   'Set-PrincipalRoles', 'New-User', 'Get-User', 'Remove-Language', 
				   'Add-Language', 'Get-Language', 'Get-Process', 'Stop-Process', 
				   'Test-SolutionComponent', 'Test-Solution', 'New-Publisher', 
				   'Get-Publisher', 'New-Solution', 'Get-SolutionComponent', 
				   'Remove-SolutionComponent', 'Export-Solution', 
				   'Add-SolutionComponent', 'Import-Solution', 'Import-Translation', 
				   'Publish-Component', 'Set-Webresource', 'New-Webresource', 
				   'Remove-Webresource', 'Export-Translation', 'Import-Webresource', 
				   'Export-Webresource', 'Get-Webresource', 'Get-Solution', 
				   'Connect-Deployment', 'Get-Organization', 'Connect-Organization', 
				   'Add-EntityKey', 'Add-Attribute', 'Add-StringAttribute', 
				   'Add-MemoAttribute', 'Add-IntegerAttribute', 'Add-BigIntAttribute', 
				   'Add-DecimalAttribute', 'Add-DoubleAttribute', 'Add-MoneyAttribute', 
				   'Add-DateTimeAttribute', 'Add-BooleanAttribute', 
				   'Add-OptionSetAttribute', 'Add-ImageAttribute', 'Get-EntityKey', 
				   'Remove-EntityKey', 'Set-Relationship', 
				   'Set-RelationshipCascadeConfig', 'New-Entity', 'New-OptionSet', 
				   'Add-Relationship', 'Get-Attribute', 'Get-Entity', 'Get-OptionSet', 
				   'Get-Relationship', 'New-OptionSetValue', 'Remove-Attribute', 
				   'Remove-Relationship', 'Remove-Entity', 'Set-OptionSetValue', 
				   'Set-OptionSet', 'Remove-OptionSetValue', 'Remove-OptionSet', 
				   'Set-Attribute', 'Set-StringAttribute', 'Set-MemoAttribute', 
				   'Set-IntegerAttribute', 'Set-BigIntAttribute', 'Set-DecimalAttribute', 
				   'Set-DoubleAttribute', 'Set-MoneyAttribute', 'Set-DateTimeAttribute', 
				   'Set-BooleanAttribute', 'Set-OptionSetAttribute', 
				   'Set-ImageAttribute', 'Set-Entity', 'Split-Content', 'Join-Content', 
				   'Invoke-Request', 'Get-Content', 'Add-Content', 'Remove-Content', 
				   'Set-Content', 'Start-Process', 'Set-ServiceEndpoint', 
				   'Register-ServiceEndpoint', 'Unregister-PluginAssembly', 
				   'Unregister-ServiceEndpoint', 'Unregister-Plugin', 
				   'Unregister-PluginStep', 'Unregister-PluginStepImage', 
				   'Enable-PluginStep', 'Disable-PluginStep', 'Set-PluginStepImage', 
				   'Register-PluginStepImage', 'Set-PluginStep', 'Register-PluginStep', 
				   'Set-Plugin', 'Register-Plugin', 'Get-ServiceEndpoint', 
				   'Get-PluginStepImage', 'Get-PluginStep', 'Get-Plugin', 'Get-PluginAssembly', 
				   'Use-Solution', 'Use-Language'
	# Private data to pass to the module specified in RootModule/ModuleToProcess
	# PrivateData = ''
	PrivateData = @{ 
		PSData = @{
			# Tags applied to this module. These help with module discovery in online galleries.
			Tags = 'Dynamics CRM', 'Dynamics 365'
			# A URL to the license for this module.
			LicenseUri = 'https://github.com/AMSoftwareNL/crmpowershell/blob/master/LICENSE'
			# A URL to the main website for this project.
			ProjectUri = 'https://github.com/AMSoftwareNL/crmpowershell'
			# ReleaseNotes of this module
			ReleaseNotes = 'https://github.com/AMSoftwareNL/crmpowershell/releases/tag/v1.3.0.0'
		} 
	}
	# HelpInfo URI of this module
	HelpInfoURI = 'http://www.amsoftware.nl/tools/crmpowershell/help/'
	# Default prefix for commands exported from this module. Override the default prefix using Import-Module -Prefix.
	DefaultCommandPrefix = 'Crm'
}

