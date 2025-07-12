@description('Name of the SQL Database')
param name string

@description('Name of the SQL Server to attach this database to')
param sqlServerName string

@description('Location of the SQL Database')
param location string

@description('Service Objective Tier (e.g., Basic, S0, GP_S_Gen5_1)')
@allowed([
  'Basic'
  'S0'
  'S1'
  'S2'
])
param skuName string = 'Basic'

resource sqlDb 'Microsoft.Sql/servers/databases@2022-02-01-preview' = {
  name: '${sqlServerName}/${name}'
  location: location
  sku: {
    name: skuName
  }
  properties: {
    readScale: 'Disabled'
  }
}

output sqlDatabaseName string = sqlDb.name
output sqlDatabaseId string = sqlDb.id
