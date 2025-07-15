# OTP Access Demo

This project implements a simple and secure one-time password (OTP) system using Clean Architecture and .NET 8. The solution is designed as a Proof of Concept (PoC).

---

## ✅ Features

- **6-digit numeric OTP generation**  
  User-friendly and easy to input manually. Ensures uniqueness per request.

- **OTP validity of 10 minutes**  
  Enforces temporary and time-limited access for enhanced security.

- **One-time use enforcement**  
  Guarantees that OTPs cannot be reused.

- **Tied to a user or resource-specific context (e.g., SessionId or Email)**  
  OTPs are scoped to a specific context to prevent misuse.  
  **TODO:** Ensure validation is fully context-aware if not already in place.

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
  **TODO:** Evaluate rate-limiting options (Azure Front Door, API Management, custom middleware).

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
      Implement a timer-triggered Function to remove expired codes regularly.

- [ ] **Offline / Resilient Design (Future)**  
      Could be expanded with local caching or device-based OTPs for offline use scenarios.

- [ ] **Validation Context (SessionId / Resource binding)**  
      Ensure all OTPs are scoped to a specific, valid user/session/resource context for security.

---

## 📂 Project Structure

```text
otp-access-demo/
├── src/                             # Clean Architecture projects
│   ├── OtpAccess.Functions.OtpApi/  # Azure Function API (.NET 8 isolated)
│   ├── Application/                 # Application Layer (Business Logic)
│   ├── Domain/                      # Domain Layer (Entities, Interfaces)
│   └── Infrastructure/              # Infrastructure Layer (EF Core, Repositories)
├── infrastructure/                  # Azure Bicep IaC templates
│   ├── main.bicep
│   ├── main.otp.bicep
│   ├── env/                         # Environment parameter files
│   │   ├── dev/
│   │   ├── test/
│   │   └── prod/
│   └── modules/                     # Reusable Bicep modules (FunctionApp, KeyVault, etc.)
├── .github/
│   └── workflows/                   # GitHub Actions CI/CD pipeline files
│       └── deploy-otp.yml
├── README.md                        # Project overview and instructions
