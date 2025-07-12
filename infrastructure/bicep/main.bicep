targetScope = 'subscription'

@description('The Azure region to deploy the resource groups to')
param location string = 'westeurope'

resource rg_dev 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: 'rg-environment-dev'
  location: location
}

resource rg_test 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: 'rg-environment-test'
  location: location
}

resource rg_prod 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: 'rg-environment-prod'
  location: location
}
