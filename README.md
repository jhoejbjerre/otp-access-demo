# OTP Access Demo

This project implements a simple and secure one-time password (OTP) system using Clean Architecture and .NET 8. The solution is designed as a Proof of Concept (PoC) for interview purposes.

## ✅ Features

- 6-digit numeric OTP generation
- OTP validity: 10 minutes
- One-time use enforcement
- Tied to a `SessionId` (or other resource/user reference)
- Input-friendly and secure
- Validated using EF Core + SQLite
- Logging and cleanup of expired OTPs
- Optional rate limiting (middleware or Azure-native)
- Infrastructure-as-Code via Bicep

## 📂 Project Structure

otp-access-demo/

├── src/ # Clean Architecture codebase
│ ├── OtpAccess.Api/
│ ├── OtpAccess.Application/
│ ├── OtpAccess.Domain/
│ ├── OtpAccess.Infrastructure/
├── infra/ # Bicep files for environment deployment
│ ├── main.bicep
│ └── parameters/
├── .github/workflows/ # GitHub Actions deployment
│ └── deploy.yml
├── README.md # This file
