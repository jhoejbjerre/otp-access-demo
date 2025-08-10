# CI/CD Pipelines – GitHub Actions

This folder contains the workflows that format, review and deploy the **OTP** repo.

## Workflows (overview)

| File | Purpose | Trigger |
| --- | --- | --- |
| `auto-fix.yml` | Auto-formats code (C#, Bicep, JS/TS) and commits fixes back to the PR branch. | `pull_request` (opened / synchronize / reopened) |
| `code-review.yml` | Runs language linters and SonarQube review **after** Auto Fix finished. Publishes per-stage PR checks. | `workflow_run` of **Auto Fix Style** (on `completed`) |
| `deploy-otp.yml` | Deploys OTP infrastructure (dev/test/prod) with gated approvals. | `push` to `main` |

---

## 1) Auto Fix Style (`auto-fix.yml`)

**What it does**
- Detects changed file types (C#, Bicep, JS/TS).
- **C#**: `dotnet format` (solution: `src/OTP/OTP.sln`) + analyzer fixes.
- **Bicep**: `bicep format` (under `infrastructure/bicep/`).
- **JS/TS** *(if present)*: `eslint --fix`.
- Commits changes back to the PR branch with message  
  `chore: apply auto-format [autofix] [skip ci]`.

**Key details**
- Uses a `paths-filter` step to skip work when no relevant files changed.
- Skips entirely when the actor is the bot (prevents infinite loops):  
  `github.actor != 'github-actions[bot]'`.
- Adds `[skip ci]` to the commit so this workflow doesn’t retrigger itself.

**Assumptions / paths**
- C# solution is at: `src/OTP/OTP.sln`
- Bicep files live under: `infrastructure/bicep/**`

---

## 2) Automated Code Review (`code-review.yml`)

**Why it’s a separate workflow**
- It is triggered by `workflow_run` **after** Auto Fix completes, so PR checks reflect
  the post-formatted code. This avoids noisy “fail → format → pass” flips.

**What it does**
- Finds the PR for the Auto Fix run and resolves the PR’s **current head SHA**
  (handles extra autofix commits).
- Detects changed file types.
- Runs language checks:
  - **C#**: restore, build, and **verify** formatting (fails if formatting differs from `.editorconfig`).
  - **Bicep**: `bicep build` and `bicep lint` (under `infrastructure/bicep/**`).
  - **Frontend** *(if present)*: `npm ci` and `npm run lint`.
- Optionally runs **SonarQube** (containerized) *only when previous linters passed*.
  We intentionally **skip Sonar** on linter failure to fail fast.
- Publishes **per-stage PR checks** on the PR (so the main PR view shows each job), and
  also publishes a compact umbrella check called **Automated Code Review**.

**Skip/Run logic**
- `lint-*` jobs only run if their file types changed (via `paths-filter` outputs).
- `sonar-review` only runs if at least one language changed **and** linters passed.
  (If you prefer to always run Sonar, change the job condition to `if: always()`.)

**Checks that appear on the PR**
- `Automated Code Review / lint-csharp`
- `Automated Code Review / lint-bicep`
- `Automated Code Review / lint-frontend`
- `Automated Code Review / sonar-review`
- (Umbrella) `Automated Code Review`

---

## 3) Deploy OTP (`deploy-otp.yml`)

**What it does**
- Validates Bicep templates and deploys infra to **dev → test → prod**.
- Uses environment protection rules for **manual approvals** in test and prod.

**Prereqs**
- Azure Service Principals per environment.
- Each environment stores credentials in an Environment Secret named `AZURE_CREDENTIALS`.
- Required reviewers set up on the **test** and **prod** environments.

---

## Branch protection & required checks

To make the PR widget show pass/fail correctly and block merges until everything is green:

1. Go to **Settings → Branches → Branch protection rules** (or “Rulesets”).
2. Protect `main` (and optionally `develop`).
3. Enable **Require status checks to pass before merging**.
4. Add these required checks (exact names):

   - `Auto Fix Style / detect-changes (pull_request)`
   - `Auto Fix Style / autofix (pull_request)`
   - `Auto Fix Style / Automated Code Review (pull_request)` *(umbrella check)*
   - `Auto Fix Style / Automated Code Review / lint-csharp (pull_request)`
   - `Auto Fix Style / Automated Code Review / lint-bicep (pull_request)`
   - `Auto Fix Style / Automated Code Review / lint-frontend (pull_request)`
   - `Auto Fix Style / Automated Code Review / sonar-review (pull_request)`

5. (Optional) Disable “Merge without waiting for requirements (bypass rules)”
   unless admins should be able to override.

> Tip: If you change the workflow names, update the required checks to match.

---

## Local conventions

- **EditorConfig**: Formatting is enforced via `.editorconfig` at repo root.
- **Solution path**: `src/OTP/OTP.sln`. Update the workflows if this moves.
- **Bicep path**: `infrastructure/bicep/**`.

---

## Reruns & troubleshooting

- If Auto Fix makes a commit, it uses `[skip ci]` to avoid re-triggering itself.
- If the Code Review checks are **expected** but not reported, confirm:
  - The **Auto Fix Style** workflow actually ran and completed.
  - Branch protection required check names match what the workflow publishes.
  - The PR head SHA in **Automated Code Review** `init` job is resolved (see logs).

---
