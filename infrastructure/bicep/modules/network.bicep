@description('Deployment environment name (e.g., dev, test, prod)')
param environment string

@description('Location of the VNet')
param location string

@description('Address space for the VNet')
param addressSpace string = '10.10.0.0/16'

@description('Address prefix for the subnet')
param subnetPrefix string = '10.10.0.0/24'

var vnetName = 'vnet-otp-${environment}'
var subnetName = 'snet-private-endpoints-${environment}'

resource vnet 'Microsoft.Network/virtualNetworks@2022-09-01' = {
  name: vnetName
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        addressSpace
      ]
    }
    subnets: [
      {
        name: subnetName
        properties: {
          addressPrefix: subnetPrefix
          privateEndpointNetworkPolicies: 'Disabled'
          privateLinkServiceNetworkPolicies: 'Disabled'
        }
      }
    ]
  }
}

output subnetId string = vnet.properties.subnets[0].id
output vnetId string = vnet.id
