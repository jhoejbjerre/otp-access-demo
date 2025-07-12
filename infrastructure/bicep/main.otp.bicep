@description('Deployment location')
param location string = 'westeurope'

@description('Environment name')
param environment string

@description('Role definitions')
param roles object

// Modules
module storage 'modules/storageAccount.bicep' = {
  name: 'deploy-storage'
  params: {
    name: 'stor${environment}'
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

module keyVault 'modules/keyVault.bicep' = {
  name: 'deploy-kv'
  params: {
    name: 'kv-${environment}'
    location: location
    userAssignedIdentityId: managedIdentity.outputs.resourceId        
  }
  dependsOn: [storage]
}

module sqlServer 'modules/sqlServer.bicep' = {
  name: 'deploy-sqlserver'
  params: {
    name: 'sql-${environment}'
    location: location
    administratorLogin: 'sqladminuser'
    administratorPassword: 'PLACEHOLDER'
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

module functionApp 'modules/functionApp.bicep' = {
  name: 'deploy-funcapp'
  params: {
    name: 'func-${environment}'
    location: location
    hostingPlanName: 'plan-${environment}'
    storageAccountName: storage.outputs.storageAccountName
    storageAccountResourceGroup: resourceGroup().name
    runtime: 'dotnet'
    disablePublicAccess: true
  }
  dependsOn: [keyVault, managedIdentity]
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

module network 'modules/network.bicep' = {
  name: 'deploy-network'
  params: {
    location: location
    environment: environment
  }
}

module privateEndpoint 'modules/privateEndpoint.bicep' = {
  name: 'deploy-pe'
  params: {
    name: 'pe-${environment}'
    location: location
    subnetId: network.outputs.subnetId
    privateLinkResourceId: sqlServer.outputs.sqlServerId
    groupId: 'sqlServer'
    subresourceName: 'sqlServer'
  }  
}
