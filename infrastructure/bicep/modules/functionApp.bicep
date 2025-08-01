@description('Name of the Function App')
param name string

@description('Name of the App Service plan')
param hostingPlanName string

@description('Deployment region')
param location string

@description('Name of the associated Storage Account')
param storageAccountName string

@description('Resource Group name of the Storage Account')
param storageAccountResourceGroup string

@description('Name of the Key Vault')
param keyVaultName string

@description('App Insights connection string')
param appInsightsConnectionString string

@description('Should the Function App only be accessible privately (no public access)?')
param disablePublicAccess bool = true

@description('User Assigned Identity Id')
param userAssignedIdentityId string

@description('Name of the subnet')
param subnetResourceId string

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' existing = {
  name: storageAccountName
  scope: resourceGroup(storageAccountResourceGroup)
}

resource appServicePlan 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: hostingPlanName
  location: location
  kind: 'functionapp,linux'
  sku: {
    name: 'EP1'
    tier: 'ElasticPremium'
  }  
  properties: {
    reserved: true
    maximumElasticWorkerCount: 1
  }
}

resource functionApp 'Microsoft.Web/sites@2024-04-01' = {
  name: name
  location: location
  kind: 'functionapp,linux'
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${userAssignedIdentityId}':{}
      }
    }
  properties: {
    serverFarmId: appServicePlan.id
    virtualNetworkSubnetId: subnetResourceId
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${listKeys(resourceId(storageAccountResourceGroup, 'Microsoft.Storage/storageAccounts', storageAccountName), '2022-09-01').keys[0].value};EndpointSuffix=core.windows.net'
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '1'
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsightsConnectionString
        }
        {
          name: 'OtpSecretKey'
          value: '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/OtpSecretKey/)'
        }
      ]
      ftpsState: 'Disabled'
      linuxFxVersion: 'DOTNET-ISOLATED|8.0'      
      minTlsVersion: '1.2'
    }
    httpsOnly: true
    publicNetworkAccess: disablePublicAccess ? 'Disabled' : 'Enabled'
  }
}

output functionAppName string = functionApp.name
