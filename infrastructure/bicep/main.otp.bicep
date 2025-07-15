@description('Deployment location')
param location string = 'westeurope'

@description('Environment name')
param environment string

@description('Role definitions')
param roles object

@description('Object ID of the GitHub Actions Service Principal')
param githubSpObjectId string


@secure()
param sqlAdminPassword string

var storageAccountName = toLower('stotp${environment}${uniqueString(resourceGroup().id)}')

module network 'modules/network.bicep' = {
  name: 'deploy-network'
  params: {
    environment: environment
    location: location
    addressSpace: '10.10.0.0/16'
    privateEndpointSubnetPrefix: '10.10.0.0/24'
    functionAppSubnetPrefix: '10.10.1.0/27'
  }
}

module storage 'modules/storageAccount.bicep' = {
  name: 'deploy-storage'
  params: {
    name: storageAccountName
    location: location
  }
}

module managedIdentity 'modules/managedIdentity.bicep' = {
  name: 'deploy-uami'
  params: {
    name: 'uami-${environment}'
    location: location
  }
}

var uniqueSuffix = uniqueString(resourceGroup().id)

module keyVault 'modules/keyVault.bicep' = {
  name: 'deploy-kv'
  params: {
    name: 'kv-${environment}-${uniqueSuffix}'
    location: location
    userAssignedIdentityId: managedIdentity.outputs.resourceId
    githubSpObjectId: githubSpObjectId
  }
  dependsOn: [storage]
}

module sqlServer 'modules/sqlServer.bicep' = {
  name: 'deploy-sqlserver'
  params: {
    name: 'sql-${environment}-${uniqueSuffix}'
    location: location
    administratorLogin: 'sqladminuser'
    administratorPassword: sqlAdminPassword
    userAssignedIdentityId: managedIdentity.outputs.resourceId
  }
}

module sqlDb 'modules/sqlDatabase.bicep' = {
  name: 'deploy-sqldb'
  params: {
    name: 'otpdb'
    location: location
    sqlServerName: sqlServer.outputs.sqlServerName
    skuName: 'Basic'
  }  
}

module appInsights 'modules/applicationInsights.bicep' = {
  name: 'deploy-appinsights-${environment}'
  params: {
    name: 'ai-otp-${environment}'
    location: location
  }
}

module privateDnsZone 'modules/privateDnsZone.bicep' = {
  name: 'deploy-private-dns-zone'
  params: {
    location: 'global'
    environment: environment
    vnetResourceId: network.outputs.vnetId
  }
}

module privateEndpoint 'modules/privateEndpoint.bicep' = {
  name: 'deploy-pe'
  params: {
    name: 'pe-${environment}'
    location: location
    subnetId: network.outputs.privateEndpointSubnetId
    privateLinkResourceId: sqlServer.outputs.sqlServerId
    groupId: 'sqlServer'
    subresourceName: 'sqlServer'
    privateDnsZoneId: privateDnsZone.outputs.privateDnsZoneId
  }
}

module functionApp 'modules/functionApp.bicep' = {
  name: 'deploy-funcapp'
  params: {
    name: 'func-otp-${environment}-${uniqueSuffix}'
    location: location
    hostingPlanName: 'plan-${environment}'
    storageAccountName: storage.outputs.storageAccountName
    storageAccountResourceGroup: resourceGroup().name    
    disablePublicAccess: true
    appInsightsConnectionString: appInsights.outputs.connectionString
    keyVaultName: keyVault.outputs.keyVaultName
    userAssignedIdentityId: managedIdentity.outputs.resourceId
    subnetResourceId: network.outputs.functionAppSubnetId
  }
  dependsOn: [
    privateDnsZone
  ]
}

module roleAssignment 'modules/roleAssignments.bicep' = {
  name: 'assign-role-kv'
  params: {    
    principalId: managedIdentity.outputs.principalId
    roleName: 'Key Vault Secrets User'
    roles: roles
  }
  dependsOn: [keyVault]
}

output keyVaultName string = keyVault.outputs.keyVaultName
output functionAppName string = functionApp.outputs.functionAppName
output userAssignedIdentityPrincipalId string = managedIdentity.outputs.principalId
output privateDnsZoneId string = privateDnsZone.outputs.privateDnsZoneId
