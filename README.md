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

---

## 📝 TODO / Backlog

- Implement context-aware OTP validation (SessionId, UserId)
- Implement cleanup of expired OTPs (timer-triggered function or similar)
- Implement rate limiting (middleware or Azure-native)
- Finalize GitHub Actions pipelines for production


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
