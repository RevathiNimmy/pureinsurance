# Spec Location Policy: Where Requirements & Designs Live

## The Problem

We have multiple scenarios where specs are created:
- New standalone applications
- New features in existing applications
- Bug fixes in existing applications
- Changes to shared components used by multiple applications
- Cross-repo features that span multiple applications

Without a clear policy, specs end up scattered and disconnected from the code they relate to.

## The Policy

**Specs live in the repo where the code changes happen.**

| Scenario | Spec Location | Rationale |
|----------|--------------|-----------|
| New application | `.aidlc/specs/` in the new repo | Spec travels with the code |
| New feature in existing app | `.aidlc/specs/` in that app's repo | Spec is next to the code it changes |
| Bug fix in existing app | `.aidlc/specs/` in that app's repo | Fix and spec are co-located |
| Shared component change | `.aidlc/specs/` in the shared component's repo | Change is owned by the component |
| Cross-repo feature | `.aidlc/specs/` in the primary repo, with references to other repos | One source of truth, linked to related repos |

---

## Scenario Details

### 1. New Application

```
new-app/
├── .ai/                          # Repository context
├── .aidlc/specs/
│   └── initial-build/            # The spec for building this app
│       ├── requirements.md
│       ├── design.md
│       └── tasks.md
└── src/
```

Straightforward. Spec and code are in the same repo from the start.

**Prompt:**
```
Create AIDLC spec for initial build of [application name]

Purpose: [what this application does]
Users: [who uses it]
Tech stack: [languages, frameworks]
Key features:
- [feature 1]
- [feature 2]
- [feature 3]

Create the full spec with requirements, design, and tasks.
This is a new application - there is no existing code.
```

### 2. New Feature in Existing Application

```
existing-app/
├── .ai/                          # Existing repository context
├── .aidlc/specs/
│   ├── payment-webhook/          # New feature spec
│   │   ├── requirements.md
│   │   ├── design.md
│   │   └── tasks.md
│   └── user-notifications/       # Another feature spec
│       ├── requirements.md
│       ├── design.md
│       └── tasks.md
└── src/
```

Each feature gets its own spec directory. Specs accumulate over time, providing a history of what was built and why.

**Prompt:**
```
Create AIDLC spec for [feature name]

Feature: [what it does]
Why: [business reason]
Affected areas: [which parts of the codebase this touches]

Check .ai/memory/architecture.md for where this fits.
Check .ai/context/integration-points.md for any cross-repo impact.
If other repos are affected, flag them and suggest whether this needs a cross-repo spec.
```

The AI checks integration-points.md automatically and will tell you if other repos need changes - you don't need to know upfront.

### 3. Bug Fix in Existing Application

```
existing-app/
├── .aidlc/specs/
│   └── fix-quantity-zero-crash/   # Bug fix spec
│       ├── requirements.md        # (or bugfix.md for AIDLC bugfix workflow)
│       ├── design.md
│       └── tasks.md
└── src/
```

Bug fix specs live alongside feature specs. Use the AIDLC bugfix workflow for systematic fixes.

**Simple bug (no spec needed):**
```
Fix this bug: [description]
Expected behaviour: [what should happen]
```

**Complex bug (use AIDLC bugfix workflow):**
```
Create AIDLC bugfix spec for [bug description]

Bug: [what's happening]
Expected: [what should happen]
Steps to reproduce: [if known]
Impact: [severity, who's affected]
Suspected area: [if known]
```

Use the bugfix workflow when the bug is complex, affects multiple files, or needs investigation. Use a simple prompt for straightforward fixes.

### 4. Shared Component Change

When a shared component (used by multiple applications) needs changes:

```
shared-component/
├── .ai/
├── .aidlc/specs/
│   └── add-retry-logic/          # Spec lives HERE, in the component repo
│       ├── requirements.md
│       ├── design.md
│       └── tasks.md
└── src/
```

The spec lives in the shared component's repo because that's where the code changes. Consuming applications may need their own specs if they need to adapt to the component change.

**Prompt (in the shared component repo):**
```
Create AIDLC spec for [change name] in this shared component

Change: [what's changing]
Why: [reason for the change]
Breaking changes: [yes/no, describe if yes]

Check .ai/context/integration-points.md for consuming applications.
For each consuming app:
- Assess whether they need changes to adopt this
- If yes, generate a summary of what each app needs to do
- Flag any breaking changes that require coordinated deployment

Create the spec for THIS component's changes.
List consuming apps that need their own adaptation specs.
```

The AI reads integration-points.md to identify all consuming apps and assess impact automatically.

**If consuming apps need changes too:**

```
shared-component/.aidlc/specs/add-retry-logic/
├── requirements.md               # Component-level requirements
├── design.md                     # Component-level design
└── tasks.md                      # Component-level tasks

app-a/.aidlc/specs/adopt-retry-logic/
├── requirements.md               # App A's adaptation requirements
└── tasks.md                      # App A's integration tasks

app-b/.aidlc/specs/adopt-retry-logic/
├── requirements.md               # App B's adaptation requirements
└── tasks.md                      # App B's integration tasks
```

**Prompt (in each consuming app repo):**
```
Create AIDLC spec to adopt [change name] from [shared-component]

The shared component [shared-component] has changed: [summary of change]
Component spec: [shared-component]/.aidlc/specs/[change-name]/

What this app needs to do: [adapt to the change]
Breaking changes to handle: [if any]

Create requirements and tasks for THIS app's adaptation only.
Reference the component spec as the source of the change.
```

### 5. Cross-Repo Feature

When a feature spans multiple repos, you need a coordinated approach. Here's how to do it step by step.

#### Step 1: Identify the Primary Repo

The primary repo is the one that:
- Owns the main business logic for this feature
- Has the most code changes
- Is the natural "home" for the feature

If unclear, pick the repo closest to the user-facing change.

#### Step 2: Create the Primary Spec

Open the primary repo in your AI agent and create the full spec:

```
Create AIDLC spec for [feature name]

This feature spans multiple repos:
- [this repo]: [what changes here]
- [repo-2]: [what changes there]
- [repo-3]: [what changes there]

Create the full requirements and design covering ALL repos.
Mark which requirements and tasks belong to which repo.
```

This creates the complete picture in one place:

```
primary-app/.aidlc/specs/payment-processing/
├── requirements.md       # ALL requirements for the feature (tagged by repo)
├── design.md             # Full design showing cross-repo interactions
├── tasks.md              # Tasks for THIS repo only
└── cross-repo.md         # Coordination document (created in Step 4)
```

#### Step 3: Create Subset Specs in Other Repos

For each additional repo, open it in your AI agent and create a subset spec that references the primary:

```
Create AIDLC spec for [feature name] in this repo.

This is a SUBSET of a cross-repo feature.
Primary spec: [primary-repo]/.aidlc/specs/[feature-name]/

This repo's role: [what this repo needs to do]
Requirements from primary spec that apply here: [list or describe]

Create requirements.md with only THIS repo's requirements.
Create design.md with only THIS repo's design.
Create tasks.md with only THIS repo's tasks.
Reference the primary spec in each document.
```

This creates a focused spec per repo:

```
secondary-api/.aidlc/specs/payment-processing-api/
├── requirements.md       # Only THIS repo's requirements
├── design.md             # Only THIS repo's design
└── tasks.md              # Only THIS repo's tasks
```

Each subset spec should include a reference header:

```markdown
# Requirements: Payment Processing API

**Cross-repo feature**: This is part of the payment-processing feature.
**Primary spec**: primary-app/.aidlc/specs/payment-processing/
**This repo's role**: Provide the payment API endpoints consumed by primary-app.
```

#### Step 4: Create the Coordination Document

In the primary repo, create cross-repo.md:

```
Create cross-repo.md for .aidlc/specs/[feature-name]/:

Document:
1. All repos involved and their role
2. The spec location in each repo
3. Dependencies between repos (what must be built/deployed first)
4. Deployment order
5. Integration test plan (how to verify cross-repo interactions)
6. Shared contracts (API specs, message formats, data models)
```

Example cross-repo.md:

```markdown
# Cross-Repo Coordination: Payment Processing

## Repos Involved

| Repo | Role | Spec Location | Deploy Order |
|------|------|--------------|:---:|
| shared-models | Shared data models | .aidlc/specs/payment-models/ | 1st |
| payment-api | Payment API endpoints | .aidlc/specs/payment-processing-api/ | 2nd |
| primary-app | UI and orchestration | .aidlc/specs/payment-processing/ | 3rd |

## Dependencies

- primary-app depends on payment-api (API contract)
- payment-api depends on shared-models (data models)
- primary-app depends on shared-models (data models)

## Shared Contracts

### API Contract
- Endpoint: POST /api/payments
- Request: PaymentRequest (defined in shared-models)
- Response: PaymentResponse (defined in shared-models)
- OpenAPI spec: payment-api/.aidlc/specs/payment-processing-api/design.md

### Data Models
- PaymentRequest, PaymentResponse defined in shared-models
- Both payment-api and primary-app consume these models

## Deployment Order

1. Deploy shared-models (no dependencies)
2. Deploy payment-api (depends on shared-models)
3. Deploy primary-app (depends on payment-api and shared-models)

## Integration Testing

- After step 2: Test payment-api against shared-models contract
- After step 3: Test primary-app end-to-end through payment-api
- Integration test location: primary-app/tests/integration/payment/

## Status

| Repo | Spec Status | Implementation | Deployed |
|------|:-----------:|:--------------:|:--------:|
| shared-models | ✅ Complete | ✅ Complete | ✅ |
| payment-api | ✅ Complete | 🔄 In progress | ❌ |
| primary-app | ✅ Complete | ❌ Not started | ❌ |
```

#### Step 5: Work the Specs in Dependency Order

Execute tasks in each repo following the deployment order:

1. Complete shared-models spec tasks first
2. Then payment-api spec tasks
3. Then primary-app spec tasks

At each step, the AI agent in that repo reads its own subset spec and .ai/ context.

#### Step 6: Create Tickets

Create tickets that mirror the cross-repo structure:

```
Create tickets for cross-repo feature [feature name]:

1. One parent feature ticket (in primary repo's project board)
2. Child tickets per repo:
   - [repo-1]: tasks from .aidlc/specs/[spec-name]/tasks.md
   - [repo-2]: tasks from .aidlc/specs/[spec-name]/tasks.md
   - [repo-3]: tasks from .aidlc/specs/[spec-name]/tasks.md
3. Link all child tickets to parent
4. Add deployment order to parent ticket description
5. Add labels: ai-sdlc, cross-repo, [feature-name]
```

See [Ticket Integration Policy](./ticket-integration-policy.md) for ticket content details.

#### Summary: Cross-Repo Workflow

```
1. Pick primary repo
2. Create full spec in primary repo
3. Create subset specs in each other repo (referencing primary)
4. Create cross-repo.md in primary repo (coordination)
5. Create tickets mirroring the structure
6. Work specs in dependency order
7. Integration test after each repo is deployed
```

#### Simplified: Single Prompt for Cross-Repo Spec (L2)

Because related repos and integration points are documented during setup (in .ai/context/integration-points.md), the AI already knows the ecosystem. This means you can create the entire cross-repo spec structure with one prompt:

```
Create cross-repo AIDLC spec for [feature name]:

Primary repo: [this repo]
Feature: [what it does]
Repos affected (check .ai/context/integration-points.md):
- [repo-2]: [what changes there]
- [repo-3]: [what changes there]

Please:
1. Create full spec in this repo (.aidlc/specs/[feature]/) with requirements, design, tasks
2. Create cross-repo.md with:
   - All repos involved and their role
   - Dependencies and deployment order (from .ai/context/integration-points.md)
   - Shared contracts affected
   - Integration test plan
3. For each other repo, generate a subset spec document I can use to create specs there
4. Create tickets: one parent + children per repo
5. Report what was created and what I need to do in other repos
```

The AI uses the integration-points.md from setup to automatically identify dependencies, contracts, and deployment order - you don't need to specify these manually.

#### Cross-Repo Status Check

```
Show cross-repo status for [feature]:
- Check .aidlc/specs/ in each repo listed in cross-repo.md
- Report completion status per repo
- Flag blockers or dependency issues
- Show deployment readiness
```

---

## Rules

1. **Specs live with the code they change** - Never in a separate documentation repo
2. **One spec per feature/fix** - Don't combine unrelated changes
3. **Cross-repo features have a primary** - One repo owns the full requirements, others have their subset
4. **Shared component changes are owned by the component** - Not by consuming apps
5. **Specs are permanent** - Don't delete completed specs, they're the history of what was built
6. **Naming is kebab-case** - e.g. `payment-webhook`, `fix-quantity-zero-crash`

## Agent Configuration

Add this to the agent rules (in Tier 1 setup):

```markdown
## Spec Location Rules
- Create specs in .aidlc/specs/ in the current repository
- Name spec directories in kebab-case
- For cross-repo features, create cross-repo.md linking related specs
- Never create specs outside the repository they relate to
- Don't delete completed specs
```

---

## FAQ

**Q: What if a feature touches 5 repos?**
A: Pick the primary repo (usually the one with the most changes). Create the full spec there. Create subset specs in the other repos. Link them with cross-repo.md.

**Q: Should we archive old specs?**
A: No. Keep them in .aidlc/specs/. They provide history and context for future work. The AI can reference them to understand past decisions.

**Q: What about specs for infrastructure changes?**
A: Same policy. If you're changing CloudFormation in an infra repo, the spec lives in that infra repo.

**Q: What if the shared component doesn't have .aidlc/ set up?**
A: Set it up. Run the Foundation setup prompt. Every repo that gets AI-assisted changes should have the Foundation.
