@description('The location for the resource(s) to be deployed.')
param location string = resourceGroup().location

param name string

param tags object = {}

resource serviceBus 'Microsoft.ServiceBus/namespaces@2024-01-01' = {
  name: name
  location: location
  tags: tags
  sku: {
    capacity: 1
    name: 'Standard'
    tier: 'Standard'
  } 
  properties: {
    minimumTlsVersion: '1.2'
  }
}

output serviceBusName string = serviceBus.name
