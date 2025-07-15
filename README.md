# OTP Access Demo

This project implements a simple and secure one-time password (OTP) system using Clean Architecture and .NET 8. The solution is designed as a Proof of Concept (PoC).

## ✅ Features

- **6-digit numeric OTP generation**  
  User-friendly and easy to input manually. Ensures uniqueness per request.

- **OTP validity of 10 minutes**  
  Enforces temporary and time-limited access for enhanced security.

- **One-time use enforcement**  
  Guarantees that OTPs cannot be reused.

- **Tied to a user or resource-specific context (e.g., SessionId or Email)**  
  OTPs are scoped to a specific context to prevent misuse.  
  **TODO:** Ensure validation is context-aware if not already in place.

- **Validation via Entity Framework Core with Azure SQL Database**  
  Secure and reliable persistence of OTP data.

- **Logging and cleanup of expired OTPs**  
  Ensures auditability and keeps the dataset clean over time.  
  **TODO:** Implement scheduled cleanup job.

- **Optional rate limiting support**  
  **TODO:** Implement middleware-based throttling or Azure-native rate limiting (e.g., API Management or Front Door with rate limit policies).

---

## ✅ Infrastructure & Deployment

- **Infrastructure as Code (IaC) with Azure Bicep**  
  Automates deployment of resources including Function App, Key Vault, SQL, Storage, Networking, and RBAC.

- **Azure Functions on Linux Consumption Plan (.NET 8 isolated worker model)**  
  Cost-effective, scalable, and aligned with modern .NET best practices.

- **CI/CD with GitHub Actions**  
  Multi-environment pipelines supporting validation, sequential deployments, and manual approvals.  
  **TODO:** Review and finalize production pipeline incl. approvals.

## 📝 Design Principles

- Follows Clean Architecture principles to ensure separation of concerns, maintainability, and testability.
- Uses Entity Framework Core for data access and Azure SQL Database for persistence.
- Infrastructure is provisioned through Azure Bicep for consistent dev/test/prod environments.
- Secrets management via Azure Key Vault.
- Hosted as Azure Function App (.NET 8 isolated) for scalability and low operational cost.
- GitHub Actions handles CI/CD pipelines with environment-specific deployment.

---

## 🚧 Known Gaps / TODOs / backlog

- **Rate Limiting & Brute Force Protection**  
  Consider using Azure Front Door, API Management, or a middleware-based solution for throttling and IP-based rate limiting.

- **Audit Logging (Long-Term Storage)**  
  Store audit logs (requests, validations) in Azure Storage or Application Insights for long-term retention and analysis.

- **Scheduled Cleanup of Expired OTPs**  
  Implement a timer-triggered Function to remove expired codes regularly.

- **Offline / Resilient Design (Future)**  
  Could be expanded with local caching / device-based OTPs for offline use scenarios.

- **Validation Context (SessionId / Resource binding)**  
  Ensure all OTPs are scoped to a specific, valid user/session/resource context for security.


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
