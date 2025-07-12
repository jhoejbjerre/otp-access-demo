@description('The principal to assign the role to (Object ID of the managed identity or user)')
param principalId string

@description('Role name to assign (must exist in the roles.json input)')
param roleName string

@description('Mapping of role names to GUIDs (from roles.json)')
param roles object

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2020-10-01-preview' = {
  name: guid(subscription().id, principalId, roleName)
  scope: resourceGroup()
  properties: {
    principalId: principalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', roles[roleName])
    principalType: 'ServicePrincipal'
  }
}
