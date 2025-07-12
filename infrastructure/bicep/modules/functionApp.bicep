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

@description('Runtime stack (e.g., dotnet)')
@allowed([
  'dotnet'
  'node'
  'python'
])
param runtime string = 'dotnet'

@description('Should the Function App only be accessible privately (no public access)?')
param disablePublicAccess bool = true

// Reference to existing Storage Account
resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' existing = {
  name: storageAccountName
  scope: resourceGroup(storageAccountResourceGroup)
}

// App Service Plan (Consumption)
resource hostingPlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: 'Y1' // Consumption plan
    tier: 'Dynamic'
  }
  kind: 'functionapp,linux'
}

// Function App with System-Assigned Managed Identity
resource functionApp 'Microsoft.Web/sites@2022-03-01' = {
  name: name
  location: location
  kind: 'functionapp,linux'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: hostingPlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: storageAccount.properties.primaryEndpoints.blob
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
      ]
      ftpsState: 'Disabled'
    }
    httpsOnly: true
    publicNetworkAccess: disablePublicAccess ? 'Disabled' : 'Enabled'
  }
}

output functionAppName string = functionApp.name
output functionAppIdentityPrincipalId string = functionApp.identity.principalId
