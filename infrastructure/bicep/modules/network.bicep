@description('Deployment environment name (e.g., dev, test, prod)')
param environment string

@description('Location of the VNet')
param location string

@description('Address space for the VNet')
param addressSpace string = '10.10.0.0/16'

@description('Address prefix for the PE subnet')
param privateEndpointSubnetPrefix string = '10.10.0.0/24'

@description('Address prefix for the Function App subnet')
param functionAppSubnetPrefix string = '10.10.1.0/27'

var vnetName = 'vnet-otp-${environment}'
var peSubnetName = 'snet-private-endpoints-${environment}'
var functionAppSubnetName = 'snet-functionapps-${environment}'

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
        name: peSubnetName
        properties: {
          addressPrefix: privateEndpointSubnetPrefix
          privateEndpointNetworkPolicies: 'Disabled'
          privateLinkServiceNetworkPolicies: 'Disabled'
        }
      }
      {
        name: functionAppSubnetName
        properties: {
          addressPrefix: functionAppSubnetPrefix
          delegations: [
            {
              name: 'Microsoft.Web.serverFarms'
              properties: {
                serviceName: 'Microsoft.Web/serverFarms'
              }
            }
          ]
          privateEndpointNetworkPolicies: 'Disabled'
          privateLinkServiceNetworkPolicies: 'Enabled'
        }
      }
    ]
  }
}

output privateEndpointSubnetId string = vnet.properties.subnets[0].id
output functionAppSubnetId string = vnet.properties.subnets[1].id
output vnetId string = vnet.id
