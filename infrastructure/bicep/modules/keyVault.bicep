@description('Name of the Key Vault')
param name string

@description('Location of the Key Vault')
param location string

@description('User-Assigned Managed Identity resource ID')
param userAssignedIdentityId string

@description('Object ID of the GitHub Actions Service Principal')
param githubSpObjectId string


resource uami 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' existing = {
  scope: resourceGroup()
  name: last(split(userAssignedIdentityId, '/'))
}

resource keyVault 'Microsoft.KeyVault/vaults@2023-02-01' = {
  name: name
  location: location
  properties: {
    tenantId: subscription().tenantId
    sku: {
      name: 'standard'
      family: 'A'
    }
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: uami.properties.principalId
        permissions: {
          secrets: [
            'get'
            'list'
          ]
        }
      }
      {
        tenantId: subscription().tenantId
        objectId: githubSpObjectId
        permissions: {
          secrets: [
            'get'
            'set'
            'list'
          ]
        }
      }
    ]
    enableRbacAuthorization: false
    enabledForDeployment: true
    enabledForTemplateDeployment: true
    enabledForDiskEncryption: false
  }
}

output keyVaultName string = keyVault.name
output keyVaultUri string = keyVault.properties.vaultUri
