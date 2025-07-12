@description('SQL Server name')
param name string

@description('Location for SQL Server')
param location string

@description('SQL Administrator login (not used if AAD-only)')
param administratorLogin string

@secure()
@description('Password for SQL Administrator (if required)')
param administratorPassword string

resource sqlServer 'Microsoft.Sql/servers@2022-02-01-preview' = {
  name: name
  location: location
  properties: {
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorPassword
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Disabled' // Ensure access only through Private Endpoint if added
  }
}

output sqlServerName string = sqlServer.name
output sqlServerId string = sqlServer.id
