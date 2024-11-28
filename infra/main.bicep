targetScope = 'subscription'

@minLength(1)
@maxLength(64)
@description('Name of the environment that can be used as part of naming resource convention, the name of the resource group for your application will use this name, prefixed with rg-')
param environmentName string

@minLength(1)
@description('The location used for all deployed resources')
param location string

@description('Id of the user or app to assign application roles')
param principalId string = ''

var tags = {
  'azd-env-name': environmentName
}

param applicationInsightsDashboardName string = ''
param applicationInsightsName string = ''
param containerAppsEnvironmentName string = ''
param containerRegistryName string = ''
param cosmosAccountName string = ''
param keyVaultName string = ''
param logAnalyticsName string = ''
param webContainerAppName string = ''
param loanapiContainerAppName string = ''
param changefeedAppExists bool = false
param reportingAppExists bool =false
param decisionAppExists bool = false
param containerRegistryHostSuffix string = 'azurecr.io'
var resourceToken = toLower(uniqueString(subscription().id, environmentName, location))

var abbrs = loadJsonContent('./abbreviations.json')
resource rg 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: 'rg-${environmentName}'
  location: location
  tags: tags
}

module monitoring './monitor/monitoring.bicep' = {
  name: 'monitoring'
  scope: rg
  params: {
    location: location
    tags: tags
    logAnalyticsName: !empty(logAnalyticsName)
      ? logAnalyticsName
      : '${abbrs.operationalInsightsWorkspaces}${resourceToken}'
    applicationInsightsName: !empty(applicationInsightsName)
      ? applicationInsightsName
      : '${abbrs.insightsComponents}${resourceToken}'
    applicationInsightsDashboardName: !empty(applicationInsightsDashboardName)
      ? applicationInsightsDashboardName
      : '${abbrs.portalDashboards}${resourceToken}'
  }
}

module containerApps 'containerappenvironment/container-apps.bicep' = {
  name: 'container-apps'
  scope: rg
  params: {
    name: 'app'
    location: location
    tags: tags
    containerAppsEnvironmentName: !empty(containerAppsEnvironmentName)
      ? containerAppsEnvironmentName
      : '${abbrs.appManagedEnvironments}${resourceToken}'
    containerRegistryName: !empty(containerRegistryName)
      ? containerRegistryName
      : '${abbrs.containerRegistryRegistries}${resourceToken}'
    logAnalyticsWorkspaceName: monitoring.outputs.logAnalyticsWorkspaceName
    applicationInsightsName: monitoring.outputs.applicationInsightsName
    principalId: principalId
  }
}
// Storage Account 
module storage './storage/storage.module.bicep' = {
  name: 'storage'
  scope: rg
  params: {
    location: location
  }
}

// Service Bus
module serviceBus './Messaging/servicebus.bicep' = {
  name: 'serviceBus'
  scope: rg
  params: {
    name: '${abbrs.serviceBusNamespaces}${resourceToken}'
    location: location
    tags: tags
  }
}

// Store secrets in a keyvault
module keyVault './security/keyvault.bicep' = {
  name: 'keyvault'
  scope: rg
  params: {
    name: !empty(keyVaultName) ? keyVaultName : '${abbrs.keyVaultVaults}${resourceToken}'
    location: location
    tags: tags
    principalId: principalId
  }
}

// The application database
module cosmos './cosmos/cosmos.bicep' = {
  name: 'cosmos'
  scope: rg
  params: {
    accountName: !empty(cosmosAccountName) ? cosmosAccountName : '${abbrs.documentDBDatabaseAccounts}${resourceToken}'
    location: location
    tags: tags
    keyVaultName: keyVault.outputs.name
  }
}

//changefeed
module changefeed './apps/changefeed/changefeed.bicep' = {
  name: 'changefeed'
  scope: rg
  params: {
    name: !empty(webContainerAppName) ? webContainerAppName : '${abbrs.appContainerApps}changefeed-${resourceToken}'
    location: location
    tags: tags
    identityName : containerApps.outputs.identityName
    containerAppsEnvironmentName: containerApps.outputs.environmentName
    containerRegistryName: containerApps.outputs.registryName
    containerRegistryHostSuffix: containerRegistryHostSuffix
    keyVaultName: keyVault.outputs.name
    applicationInsightsName: monitoring.outputs.applicationInsightsName
    cosmosConnectionString: cosmos.outputs.connectionString
    exists: changefeedAppExists
    storageAccountConectionString: storage.outputs.blobEndpoint
  }
}

//loanapplicationapi
module loanapplicationapi './apps/loanapplicationapi/loanapplicationapi.bicep' = {
  name: 'loanapplicationapi'
  scope: rg
  params: {
    name: !empty(loanapiContainerAppName) ? loanapiContainerAppName : '${abbrs.appContainerApps}loanapi-${resourceToken}'
    location: location
    tags: tags
    identityName : containerApps.outputs.identityName
    containerAppsEnvironmentName: containerApps.outputs.environmentName
    containerRegistryName: containerApps.outputs.registryName
    containerRegistryHostSuffix: containerRegistryHostSuffix
    keyVaultName: keyVault.outputs.name
    applicationInsightsName: monitoring.outputs.applicationInsightsName
    cosmosConnectionString: cosmos.outputs.connectionString
    exists: changefeedAppExists
  }
}
//blazor
module blazor './apps/blazor/blazor.bicep' = {
  name: 'blazor'
  scope: rg
  params: {
    name: !empty(loanapiContainerAppName) ? loanapiContainerAppName : '${abbrs.appContainerApps}blazor-${resourceToken}'
    location: location
    tags: tags
    identityName : containerApps.outputs.identityName
    containerAppsEnvironmentName: containerApps.outputs.environmentName
    containerRegistryName: containerApps.outputs.registryName
    containerRegistryHostSuffix: containerRegistryHostSuffix
    keyVaultName: keyVault.outputs.name
    applicationInsightsName: monitoring.outputs.applicationInsightsName
    exists: changefeedAppExists
  }
}
//decision
module decision './apps/decisionapi/decisionapi.bicep' = {
  name: 'decision'
  scope: rg
  params: {
    name: !empty(loanapiContainerAppName) ? loanapiContainerAppName : '${abbrs.appContainerApps}decision-${resourceToken}'
    location: location
    tags: tags
    identityName : containerApps.outputs.identityName
    containerAppsEnvironmentName: containerApps.outputs.environmentName
    containerRegistryName: containerApps.outputs.registryName
    containerRegistryHostSuffix: containerRegistryHostSuffix
    applicationInsightsName: monitoring.outputs.applicationInsightsName
    exists: decisionAppExists
  }
}
//reporting
module reporting './apps/reportingapi/reportingapi.bicep' = {
  name: 'reporting'
  scope: rg
  params: {
    name: !empty(loanapiContainerAppName) ? loanapiContainerAppName : '${abbrs.appContainerApps}reporting-${resourceToken}'
    location: location
    tags: tags    
    identityName : containerApps.outputs.identityName
    containerAppsEnvironmentName: containerApps.outputs.environmentName
    containerRegistryName: containerApps.outputs.registryName
    containerRegistryHostSuffix: containerRegistryHostSuffix
    applicationInsightsName: monitoring.outputs.applicationInsightsName
    exists: reportingAppExists
  }
}
//dapr
module daprComponents 'dapr/daprcomponents.bicep' ={
  dependsOn:[blazor, changefeed,loanapplicationapi, reporting, decision]
  name: 'dapr'
  scope: rg
  params:{    
    serviceBusName: serviceBus.outputs.serviceBusName    
    identityName : containerApps.outputs.identityName
    containerAppEnvironmentName: containerApps.outputs.environmentName
    servicebusScopes: ['changefeed','blazor', 'loanapi', 'reporting', 'decision']    
  }
}

output AZURE_LOG_ANALYTICS_WORKSPACE_NAME string = monitoring.outputs.logAnalyticsWorkspaceName
output AZURE_CONTAINER_REGISTRY_ENDPOINT string = containerApps.outputs.registryLoginServer

output AZURE_CONTAINER_REGISTRY_NAME string = containerApps.outputs.registryName
output AZURE_CONTAINER_APPS_ENVIRONMENT_NAME string = containerApps.outputs.environmentName
output AZURE_CONTAINER_APPS_ENVIRONMENT_ID string = containerApps.outputs.environmentId
output AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN string = containerApps.outputs.defaultDomain
output SERVICE_BINDING_KV_ENDPOINT string = keyVault.outputs.endpoint
output SERVICE_BINDING_KV_NAME string = keyVault.outputs.name
