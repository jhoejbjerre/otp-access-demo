# OTP Access Demo

This project implements a simple and secure one-time password (OTP) system using Clean Architecture and .NET 8. The solution is designed as a Proof of Concept (PoC).

## ✅ Features

- 6-digit numeric OTP generation  
  Easy to input manually and unique per request.
- OTP validity of 10 minutes  
  Ensures temporary and time-limited access.
- One-time use enforcement  
  Prevents reuse of the same code.
- Tied to SessionId or resource/user reference  
  OTPs are bound to a valid context for security.
- Validation via Entity Framework Core with Azure SQL Database  
  Provides reliable data access and persistence.
- Logging and cleanup of expired OTPs  
  Maintains auditability and system hygiene.
- Optional rate limiting support  
  Middleware-based or leveraging Azure-native rate limiting resources.
- Infrastructure as Code with Azure Bicep  
  Deploys resources for Function App, Key Vault, SQL, Storage, Networking, and RBAC.
- Azure Functions hosted on Linux Consumption Plan  
  Using .NET 8 isolated worker model for cost-effective and scalable hosting.
- GitHub Actions CI/CD pipeline  
  Supports multi-environment deployment with validation, sequential jobs, and manual gates.

## 📂 Project Structure

```text
otp-access-demo/
├── src/                      # Clean Architecture codebase
│   ├── OtpAccess.Api/
│   ├── OtpAccess.Application/
│   ├── OtpAccess.Domain/
│   └── OtpAccess.Infrastructure/
├── bicep/                    # Azure Bicep IaC templates
│   ├── main.bicep
│   ├── main.otp.bicep
│   ├── env/                  # Environment parameter files
│   │   ├── dev/
│   │   │   └── main.otp.dev.parameters.json
│   │   ├── test/
│   │   │   └── main.otp.test.parameters.json
│   │   └── prod/
│   │       └── main.otp.prod.parameters.json
│   └── modules/              # Reusable Bicep modules (FunctionApp, KeyVault, etc.)
├── .github/
│   └── workflows/            # GitHub Actions CI/CD pipeline files
│       └── deploy-otp.yml
├── README.md                 # Project overview and instructions
