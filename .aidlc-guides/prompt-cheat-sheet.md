# AI-SDLC Prompt Cheat Sheet

A single-page reference for the most common prompts across all three tiers. Organised by what you're trying to do, not by tier.

For full prompt libraries see: [Setup Prompts](./prompts/setup-prompts.md) · [Daily Prompts](./prompts/daily-prompts.md) · [Maintenance Prompts](./prompts/maintenance-prompts.md)

---

## How to Read This Sheet

**Auto** = Agent does this automatically during the AIDLC workflow — you don't need to ask
**Prompt** = You need to ask explicitly
**Tier** = Which tier must be installed for this to work

---

## Tier 1 — AIDLC Core

### Starting a feature

| What you want | Prompt | Mode |
|---------------|--------|------|
| Start a structured feature | `Create AIDLC spec for [feature name]` | Prompt |
| Start a structured bug fix | `Create AIDLC bugfix spec for [bug description]` | Prompt |
| Resume after a break | `Continue the [feature-name] spec` | Prompt |
| Run the next task in the plan | `Execute task [number] from the [feature-name] spec` | Prompt |
| Run all remaining tasks | `Execute remaining tasks from the [feature-name] spec` | Prompt |

### During the AIDLC workflow

| What happens | Mode |
|--------------|------|
| Requirements gathered and written to `requirements.md` | Auto |
| Design document created (`design.md`) | Auto |
| Task breakdown generated (`tasks.md`) | Auto |
| Progress tracked in `aidlc-state.md` | Auto |
| Audit trail maintained in `audit.md` | Auto |
| `.ai/` context files updated after changes | Auto (L2) / Prompt (L1) |

> **L1 note**: At Volaris L1 the agent may not update `.ai/` automatically. Use this as a safety net:
> `Update .ai/ files for the changes I just made to [component]`

### Design phase

| What you want | Prompt | Mode |
|---------------|--------|------|
| Generate design from approved requirements | `Generate design for [feature] based on approved requirements` | Prompt |
| Record an architectural decision | `Record this as an ADR: [decision and rationale]` | Prompt |
| Evaluate a technology choice | `Evaluate [technology] for [use case] and recommend` | Prompt |

### Testing

| What you want | Prompt | Mode |
|---------------|--------|------|
| Generate tests for a component | `Generate tests for [component]` | Prompt |
| Generate property-based tests | `Generate property-based tests for [component] with 100+ iterations` | Prompt |
| Map test coverage to requirements | Auto during AIDLC construction phase | Auto |

### Status and tracking

| What you want | Prompt |
|---------------|--------|
| See all active specs | `Show status of all specs in .aidlc/specs/` |
| Check progress on a specific feature | `Show progress on the [feature-name] spec` |
| Sync tickets with a spec | `Sync tickets with spec .aidlc/specs/[feature-name]/` |
| Check for stale specs | `List any specs that haven't been updated in the last 30 days` |

### Onboarding

| What you want | Prompt |
|---------------|--------|
| Onboard a new developer to the project | `I'm new to this project. Summarise .ai/memory/ and give me a 10-minute overview` |
| Orient a new AI agent | `Read .ai/memory/architecture.md and .ai/memory/conventions.md. Confirm you understand before we start` |
| Troubleshoot agent ignoring standards | `Before starting, read .ai/memory/conventions.md and .ai/rules/coding-standards.md. Confirm you'll follow these.` |

---

## Tier 2 — Reference Templates

> Tier 2 templates live in `.aidlc-templates/`. The agent knows they exist because the agent config file points to them. During the AIDLC design phase the agent will reference architecture templates automatically — but audits and compliance checks always need an explicit prompt.

### Design phase — when to expect automatic template use

| Scenario | What the agent references | Mode |
|----------|--------------------------|------|
| Designing a cloud-native feature | `architecture/aws.md` or `azure.md` | Auto |
| Designing a frontend component | `architecture/react.md` | Auto |
| Designing a new CI/CD pipeline | `ci-cd/pipeline-patterns.md` + `environment-strategy.md` | Auto |
| Creating a spec document | `docs/spec-template.md` as the format | Auto |
| Recording an ADR | `docs/adr-template.md` as the format | Auto |

> **If the agent isn't picking up templates automatically**, reference them explicitly:
> `When designing this feature, reference .aidlc-templates/architecture/aws.md for patterns`

### Audits — always explicit

Audits do not run automatically at Tier 2 (that requires Tier 3). Run them on demand:

| Audit type | Prompt |
|------------|--------|
| Security audit | `Run a security audit using the templates in .aidlc-templates/audits/security/` |
| Infrastructure audit | `Run an infrastructure audit using .aidlc-templates/audits/infrastructure/` |
| Team process audit | `Run a team audit using .aidlc-templates/audits/team/` |
| AWS hosting audit | `Run an AWS hosting audit using .aidlc-templates/audits/hosting/aws/` |
| Azure hosting audit | `Run an Azure hosting audit using .aidlc-templates/audits/hosting/azure/` |
| Full audit (all areas) | `Run a full audit across security, infrastructure, and team using the templates in .aidlc-templates/audits/` |

### Compliance — always explicit

| What you want | Prompt |
|---------------|--------|
| Validate against a compliance standard | `Validate this codebase against .aidlc-templates/compliance/[hipaa\|pci-dss\|soc2].md and report findings` |
| Check a specific feature for compliance | `Check the [feature-name] implementation against .aidlc-templates/compliance/pci-dss.md` |
| Generate a compliance report | `Generate a compliance report for [standard] based on the current codebase and .aidlc-templates/compliance/` |

### Forcing template use during design

Use these if you want to explicitly invoke a template rather than waiting for the agent to find it:

| What you want | Prompt |
|---------------|--------|
| Apply architecture patterns | `Design [feature] following the patterns in .aidlc-templates/architecture/aws.md` |
| Use CI/CD reference | `Design the pipeline for [feature] following .aidlc-templates/ci-cd/pipeline-patterns.md` |
| Apply environment strategy | `Plan the environment rollout following .aidlc-templates/ci-cd/environment-strategy.md` |
| Use spec template format | `Create the spec for [feature] using the format in .aidlc-templates/docs/spec-template.md` |
| Create a properly formatted ADR | `Record this decision as an ADR following .aidlc-templates/docs/adr-template.md: [decision]` |

---

## Tier 3 — GitHub Platform Layer

> Most of Tier 3 runs automatically via GitHub Actions. The prompts below are for the things you still control manually.

### Copilot Coding Agent

| What you want | How |
|---------------|-----|
| Assign a task to the Copilot Coding Agent | Assign the GitHub Issue to `@github-copilot` |
| Give the agent a specific brief | Add a comment to the issue: `@github-copilot implement this following the spec in .aidlc/specs/[feature-name]/` |
| Have the agent continue from a spec | Comment: `@github-copilot continue from .aidlc/specs/[feature-name]/tasks.md task [number]` |

### On-demand triggers (things that are also automated but can be run manually)

| What you want | How |
|---------------|-----|
| Trigger an immediate security audit | Run `run-audit.yml` workflow in GitHub Actions |
| Request an architecture review on a PR | Comment on PR: `@github-copilot review this PR for architecture concerns` |
| Request a technical review on a PR | Comment on PR: `@github-copilot review this PR for technical issues` |
| Trigger doc drift detection | Run `doc-drift-detector.yml` workflow manually |

### What runs automatically at Tier 3 (no prompt needed)

| Trigger | What happens automatically |
|---------|---------------------------|
| PR opened | PR validation, enrichment, assignment |
| PR merged to main | Deploy to dev environment |
| Dev approved | Deploy to test environment |
| Scheduled (nightly/weekly) | Security, infrastructure, and team audits |
| Kubernetes events | Cluster guardian monitoring |
| Issue created | Issue triage and labelling |

---

## Quick-Decision Guide

**Should I use AIDLC or just ask the agent directly?**

| Task type | Approach |
|-----------|----------|
| Quick question / explanation | Just ask — no AIDLC |
| Small isolated bug fix | Just ask — no AIDLC |
| Code snippet or example | Just ask — no AIDLC |
| Multi-file feature | `Create AIDLC spec for...` |
| Complex bug with root cause analysis | `Create AIDLC bugfix spec for...` |
| Anything needing an audit trail | `Create AIDLC spec for...` |
| Anything involving compliance | `Create AIDLC spec for...` + compliance audit prompt |

**Do I need to reference the Tier 2 templates explicitly?**

| Situation | Answer |
|-----------|--------|
| Working through AIDLC design phase | No — agent finds relevant templates automatically |
| Running an audit | Yes — always use the explicit audit prompts above |
| Validating compliance | Yes — always use the explicit compliance prompts above |
| Agent isn't referencing the right patterns | Yes — point it at the specific template file |

---

## See Also

- [Setup Prompts](./prompts/setup-prompts.md) — initial setup and verification
- [Daily Prompts](./prompts/daily-prompts.md) — full daily workflow reference
- [Maintenance Prompts](./prompts/maintenance-prompts.md) — weekly checks, drift detection, onboarding
- [Spec Location Policy](./spec-location-policy.md) — where to put specs for different scenarios
- [Ticket Integration Policy](./ticket-integration-policy.md) — ADO and GitHub Issues sync
