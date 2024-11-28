param containerAppEnvironmentName string
param servicebusScopes array
param serviceBusName string
param identityName string 

resource serviceBus 'Microsoft.ServiceBus/namespaces@2024-01-01' existing = {
  name: serviceBusName
}

resource containerAppEnvironment 'Microsoft.App/managedEnvironments@2024-02-02-preview' existing = {
  name: containerAppEnvironmentName
}
resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' existing = {
  name: identityName
}

resource caeMiRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(
    serviceBus.id,
    managedIdentity.id,
    subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '090c5cfd-751d-490a-894a-3ce6f1109419')
  )
  scope: serviceBus
  properties: {
    principalId: managedIdentity.properties.principalId
    principalType: 'ServicePrincipal'
    roleDefinitionId: subscriptionResourceId(
      'Microsoft.Authorization/roleDefinitions',
      '090c5cfd-751d-490a-894a-3ce6f1109419'
    )
  }
}
resource pubsub 'Microsoft.App/managedEnvironments/daprComponents@2024-03-01' = {
  name: 'pubsub'
  parent: containerAppEnvironment
  properties: {
    componentType: 'pubsub.azure.servicebus'
    version: 'v1'
    metadata: [
      {
        name: 'namespaceName'
        value: '${serviceBus.name}.servicebus.windows.net'
      }
      {
        name: 'azureClientId'
        value: managedIdentity.properties.clientId
      }
      { name: 'consumerID', value: managedIdentity.properties.clientId }
    ]
    scopes: servicebusScopes
  }
}
