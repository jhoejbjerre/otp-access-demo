@description('Name of the Private Endpoint')
param name string

@description('Location for the Private Endpoint')
param location string

@description('Name of the subnet where the PE will reside')
param subnetId string

@description('Resource ID of the private link service (e.g., SQL server, KV, etc.)')
param privateLinkResourceId string

@description('Private link sub-resource name (e.g., "sqlServer", "vault", "blob")')
param subresourceName string

@description('Optional group ID override (some resources require specific group IDs)')
param groupId string = subresourceName

resource privateEndpoint 'Microsoft.Network/privateEndpoints@2023-05-01' = {
  name: name
  location: location
  properties: {
    subnet: {
      id: subnetId
    }
    privateLinkServiceConnections: [
      {
        name: '${name}-pls'
        properties: {
          privateLinkServiceId: privateLinkResourceId
          groupIds: [
            groupId
          ]
        }
      }
    ]
  }
}

resource nic 'Microsoft.Network/networkInterfaces@2022-09-01' existing = {
  name: '${name}.nic.0'
}

output privateEndpointId string = privateEndpoint.id
output privateEndpointIpAddress string = nic.properties.ipConfigurations[0].properties.privateIPAddress

