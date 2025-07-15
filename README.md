# OTP Access Demo

This project implements a simple and secure one-time password (OTP) system using Clean Architecture and .NET 8.
The solution is designed as a Proof of Concept (PoC).

---

## ✅ Features

- **6-digit numeric OTP generation**  
  Generates simple 6-digit numeric codes that are easy to communicate and transfer via e.g., SMS, email, or verbally. Ensures uniqueness per request.

- **OTP validity of 10 minutes**  
  Enforces temporary and time-limited access for enhanced security.

- **One-time use enforcement**  
  Guarantees that OTPs cannot be reused.

- **Tied to a user or resource-specific context (Email and OptCode)**  
  OTPs are scoped to a specific context for security to prevent misuse. So they can be validated as valid and then used once and then they will be marked as IsUsed
  
- **Validation via Entity Framework Core with Azure SQL Database**  
  Secure and reliable persistence of OTP data.

- **Logging and cleanup of expired OTPs**  
  Ensures auditability and keeps the dataset clean over time.  
  **TODO:** Implement scheduled cleanup job.

- **Optional rate limiting support**  
  **TODO:** Implement middleware-based throttling or Azure-native rate limiting (e.g., API Management or Front Door policies).

- **Clean Architecture structure for maintainability**  
  Ensures separation between Domain, Application, Infrastructure, and API layers.

- **Brute-force prevention planned via Azure-native or middleware-based solutions**  
  **TODO:** Evaluate rate-limiting options (Azure Front Door, API Management, CloudFlare, custom middleware).

- **A few unit tests is applied for one of the func apps**
  **TODO:** More could be added in the future
---

## ✅ Infrastructure & Deployment

- **Infrastructure as Code (IaC) with Azure Bicep**  
  Automates deployment of resources including Function App, Key Vault, SQL, Storage, Networking, and RBAC.

- **Azure Functions on Linux Consumption Plan (.NET 8 isolated worker model)**  
  Cost-effective, scalable, and aligned with modern .NET best practices.

- **Application Insights integration for logging and monitoring**  
  All OTP-related operations are logged via ILogger, with telemetry automatically routed to Azure Application Insights.

- **CI/CD with GitHub Actions**  
  Multi-environment pipelines supporting validation, sequential deployments, and manual approvals.  
  **TODO:** Finalize production pipeline incl. manual approvals. 

---

## 🔄 CI/CD Pipeline Overview

This project uses GitHub Actions for continuous integration and deployment (CI/CD).  
The pipeline deploys sequentially through environments (Dev → Test → Prod) with manual approval required for Test and Prod.
Deployed under one subscription for the POC in separate resource groups for each environment

### Deployment Flow:
```markdown
+------------+     +-------------+     +-------------+
| deploy-dev | --> | deploy-test | --> | deploy-prod |
+------------+     +-------------+     +-------------+
     ✅              🔶 (manual)        🔶 (manual)

**TODO:** Finalize test and prod

## 📝 Design Principles

- Follows Clean Architecture principles to ensure separation of concerns, maintainability, and testability.
- Uses Entity Framework Core for data access and Azure SQL Database for persistence.
- Infrastructure is provisioned through Azure Bicep for consistent dev/test/prod environments.
- Secrets management:
  - GitHub Secrets are used to store Service Principal credentials for pipeline deployments.
  - A User-Assigned Managed Identity (UAMI) is used by the Function App for runtime access to Azure resources (Key Vault, SQL).
- Hosted as Azure Function App (.NET 8 isolated worker model) for scalability and low operational cost.
- GitHub Actions handles CI/CD pipelines with environment-specific deployment.
- Application Insights integration provides centralized telemetry for all environments.

---

## 🔐 Security & Access Control

- **Deployment Identity:**  
  GitHub Actions uses a Service Principal (stored securely via GitHub Secrets) to deploy Azure resources through Bicep.

- **Runtime Identity:**  
  The Azure Function App uses a **User-Assigned Managed Identity (UAMI)** to access protected resources (e.g., Key Vault, Azure SQL Database).  
  No secrets are stored in code or in the Function App configuration.

---

## 🚧 Known Gaps / TODOs / Backlog

- [ ] **Rate Limiting & Brute Force Protection**  
      Consider Azure Front Door, API Management, or middleware-based solutions for throttling and IP-based rate limiting.

- [ ] **Scheduled Cleanup of Expired OTPs**  
      A timer-triggered Azure Function will be implemented to perform regular cleanup of expired OTP records from the database. This ensures that the dataset remains clean and only contains active, relevant entries.

- [ ] **Offline / Resilient Design (Future)**  
      Instead of device-based offline support, resilience can be achieved through Azure-native features
      such as SQL Geo-replication, zone-redundant storage and cross-region deployment.
---

## 📂 Project Structure

```text
otp-access-demo/
├── src/                                  # Clean Architecture projects
│   ├── OtpAccess.Functions.OtpApi/       # Azure Function API for OTP generation (.NET 8 isolated)
│   ├── OtpValidate.Functions.OtpApi/     # Azure Function API for OTP validation (.NET 8 isolated)
│   ├── OtpAccess.Functions.OtpApi.Tests/ # Unit tests for OtpAccess.Functions.OtpApi
│   ├── Application/                      # Application Layer (Business Logic)
│   ├── Domain/                           # Domain Layer (Entities, Interfaces)
│   │   ├── Common/                       # Shared base entities (e.g., BaseEntity)
│   │   ├── Entities/                     # Domain entities (e.g., OtpRequest)
│   │   └── Interfaces/                   # Domain interfaces (e.g., repository contracts)
│   └── Infrastructure/                   # Infrastructure Layer (EF Core, Repositories)
├── infrastructure/                       # Azure Bicep IaC templates
│   ├── main.bicep
│   ├── main.otp.bicep
│   ├── env/                              # Environment parameter files
│   │   ├── dev/
│   │   ├── test/
│   │   └── prod/
│   └── modules/                          # Reusable Bicep modules (FunctionApp, KeyVault, etc.)
├── .github/
│   └── workflows/                        # GitHub Actions CI/CD pipeline files
│       └── deploy-otp.yml
├── README.md                             # Project overview and instructions

