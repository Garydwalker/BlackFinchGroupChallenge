param name string
param location string = resourceGroup().location
param tags object = {}

param identityName string
param applicationInsightsName string
param containerAppsEnvironmentName string
param containerRegistryName string
param containerRegistryHostSuffix string
param keyVaultName string
param serviceName string = 'blazor'
param exists bool

resource apiIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' existing = {
  name: identityName
}

// module apiKeyVaultAccess '../../security/keyvault-access.bicep' = {
//   name: 'api-keyvault-access'
//   params: {
//     keyVaultName: keyVaultName
//     principalId: apiIdentity.properties.principalId
//   }
// }

module app '../container-app-upsert.bicep' = {
  name: 'ca-${serviceName}'
  params: {
    name: name
    location: location
    tags: union(tags, { 'azd-service-name': serviceName })
    identityName: apiIdentity.name
    exists: exists
    containerAppsEnvironmentName: containerAppsEnvironmentName
    containerRegistryName: containerRegistryName
    containerRegistryHostSuffix: containerRegistryHostSuffix
    containerCpuCoreCount: '1.0'
    containerMemory: '2.0Gi'
    daprEnabled: true
    daprAppId: serviceName
    env: [
      {
        name: 'AZURE_CLIENT_ID'
        value: apiIdentity.properties.clientId
      }
      {
        name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
        value: applicationInsights.properties.ConnectionString
      }
      {
        name: 'ASPNETCORE_ENVIRONMENT'
        value: 'Production'
      }
      {
        name:'ASPNETCORE_FORWARDEDHEADERS_ENABLED'
        value:'true'
      }
      {
        name:'ASPNETCORE_FORWARDEDHEADERS_ENABLED'
        value:'true'
      }
      {
        name:'HTTP_PORTS'
        value:'8080'
      }
      {
        name:'OTEL_DOTNET_EXPERIMENTAL_OTLP_EMIT_EVENT_LOG_ATTRIBUTES'
        value:'true'
      }
      {
        name:'OTEL_DOTNET_EXPERIMENTAL_OTLP_EMIT_EXCEPTION_LOG_ATTRIBUTES'
        value:'true'
      }
    ]
  }
}


resource applicationInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: applicationInsightsName
}


output SERVICE_CHANGEFEED_IDENTITY_PRINCIPAL_ID string = apiIdentity.properties.principalId
output SERVICE_CHANGEFEED_NAME string = app.outputs.name
output SERVICE_CHANGEFEED_URI string = app.outputs.uri
output SERVICE_CHANGEFEED_IMAGE_NAME string = app.outputs.imageName
