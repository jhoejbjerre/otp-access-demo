@description('Location of the DNS Zone resource (usually global)')
param location string = 'global'

@description('Environment name (e.g., dev, test, prod)')
param environment string

@description('VNet Resource ID to link the Private DNS Zone')
param vnetResourceId string

@description('Private Endpoint IP Address for SQL Server')
param privateEndpointIpAddress string

var dnsZoneName = 'privatelink.database.windows.net'

resource privateDnsZone 'Microsoft.Network/privateDnsZones@2020-06-01' = {
  name: dnsZoneName
  location: location
}

resource vnetLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2020-06-01' = {
  name: 'vnetlink-${environment}'
  parent: privateDnsZone
  location: location
  properties: {
    virtualNetwork: {
      id: vnetResourceId
    }
    registrationEnabled: false
  }
}

resource sqlARecord 'Microsoft.Network/privateDnsZones/A@2020-06-01' = {
  name: 'sql-${environment}' // sql-dev
  parent: privateDnsZone
  properties: {
    ttl: 3600
    aRecords: [
      {
        ipv4Address: privateEndpointIpAddress
      }
    ]
  }
}

output privateDnsZoneId string = privateDnsZone.id