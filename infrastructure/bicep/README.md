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

## Notes on Deployment

- **Module dependencies:**  
  Ensure that modules are declared in the correct order with appropriate `dependsOn` properties to guarantee resources are provisioned in sequence. For example, the Function App depends on App Insights, Key Vault, and Managed Identity.

- **Parameter values:**  
  Replace placeholder values such as SQL admin password with secure secrets, preferably stored and retrieved from Azure Key Vault or GitHub Secrets.

- **Resource group scope:**  
  Make sure the resource group names and scopes align correctly between modules, especially when referencing existing resources like the Storage Account.

- **Environment parameters:**  
  Use the environment-specific parameter files (`env/dev/*.json`, `env/test/*.json`, `env/prod/*.json`) to customize resource names, SKUs, and other settings per environment.

- **App Insights connection string:**  
  Pass the App Insights connection string output from the `applicationInsights.bicep` module to the Function App module, enabling telemetry.

- **Testing deployments:**  
  Use `az deployment sub validate` and `az deployment group validate` commands to pre-validate your templates and parameters before deploying.

- **CI/CD integration:**  
  Ensure your pipeline YAML includes validation and deployment steps in the correct order, respecting `dependsOn` logic from your Bicep code.

