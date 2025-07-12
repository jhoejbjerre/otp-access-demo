# Infrastructure as Code (IaC) - Azure Bicep

This folder contains Bicep modules and environment-specific parameter files for deploying the OTP Proof of Concept infrastructure on Azure.

## Structure

- `main.bicep` - Deploys resource groups at subscription level
- `main.otp.bicep` - Deploys OTP-specific resources (Function App, SQL, Key Vault, etc.) in resource groups
- `modules/` - Reusable Bicep modules (Function App, Key Vault, Managed Identity, etc.)
- `env/` - Environment parameter files (dev, test, prod)

## Deployment

### Prerequisites

- Azure CLI installed and updated
- Azure Service Principal credentials configured as GitHub Secrets for CI/CD
- Azure subscription and networking prepared

### Manual Deployment

```bash
az deployment sub create --template-file main.bicep --parameters location=westeurope

az deployment group create --resource-group rg-environment-dev --template-file main.otp.bicep --parameters @env/dev/main.otp.dev.parameters.json
