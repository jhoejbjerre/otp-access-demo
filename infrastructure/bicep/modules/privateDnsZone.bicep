@description('Location of the DNS Zone resource (usually global)')
param location string = 'global'

@description('Environment name (e.g., dev, test, prod)')
param environment string

@description('VNet Resource ID to link the Private DNS Zone')
param vnetResourceId string

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

output privateDnsZoneId string = privateDnsZone.id
