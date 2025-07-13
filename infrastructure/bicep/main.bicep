targetScope = 'subscription'

@description('The Azure region to deploy the resource group to')
param location string = 'westeurope'

@allowed([
  'dev'
  'test'
  'prod'
])
@description('The environment to deploy the resource group for')
param environment string

resource resourceGroup 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: 'rg-otp-${environment}'
  location: location
}
