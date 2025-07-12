@description('Name of the Key Vault')
param name string

@description('Location of the Key Vault')
param location string

@description('User-Assigned Managed Identity ID that will get access')
param userAssignedIdentityId string

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
        objectId: userAssignedIdentityId
        permissions: {
          secrets: [
            'get'
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

resource otpSecret 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: keyVault
  name: 'OtpSecretKey'
  properties: {
    value: ''
  }
}

resource sqlSecret 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: keyVault
  name: 'SqlAdminPassword'
  properties: {
    value: ''
  }
}

output keyVaultName string = keyVault.name
output keyVaultUri string = keyVault.properties.vaultUri
