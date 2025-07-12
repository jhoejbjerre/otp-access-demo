@description('Name of the User-Assigned Managed Identity')
param name string

@description('Location of the Managed Identity')
param location string

resource uami 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: name
  location: location
}

output resourceId string = uami.id
output principalId string = uami.properties.principalId
output clientId string = uami.properties.clientId
