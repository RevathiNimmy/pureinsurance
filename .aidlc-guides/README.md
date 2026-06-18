# AI-SDLC Practitioner Guides

## How to Use These Guides

| Your Role | Start Here | Then Read |
|-----------|-----------|-----------|
| **Choosing a workflow** | [Fast Track](./fast-track-guide.md) or [Full Lifecycle](./full-lifecycle-guide.md) | Pick based on feature complexity |
| **Developer** | [Developer Guide](./roles/developer-guide.md) | [Daily Prompts](./prompts/daily-prompts.md) |
| **Architect** | [Architect Guide](./roles/architect-guide.md) | [Daily Prompts](./prompts/daily-prompts.md) |
| **Product Owner** | [Product Owner Guide](./roles/product-owner-guide.md) | [Daily Prompts](./prompts/daily-prompts.md) |
| **QA/Tester** | [QA Tester Guide](./roles/qa-tester-guide.md) | [Daily Prompts](./prompts/daily-prompts.md) |
| **DevOps** | [DevOps Guide](./roles/devops-guide.md) | [Maintenance Prompts](./prompts/maintenance-prompts.md) |
| **Setting up a repo (ADO)** | [Tier 1+2 ADO Setup](./tier1-2-ado-agent-setup.md) | [Setup Prompts](./prompts/setup-prompts.md) |
| **Tier 1+2 (ADO, agent-driven)** | [Tier 1-2 ADO Agent Setup](./tier1-2-ado-agent-setup.md) | Agent-driven setup for Tier 1+2 with Azure DevOps |
| **Migrate ADO to GitHub/Tier 3** | [Tier 3 Setup](./setup/tier-3-setup.md) | Step-by-step Tier 3 GitHub setup |
| **Tier 3 (GitHub only)** | [Tier 3 Setup](./setup/tier-3-setup.md) | Agent-driven setup for GitHub projects |

## Contents


### Workflow Guides
Choose the right workflow for your feature:
- [Fast Track Guide](./fast-track-guide.md) — Simple features with clear requirements (~22 min setup, single prompt auto-generates spec)
- [Full Lifecycle Guide](./full-lifecycle-guide.md) — Complex features requiring discovery (interactive inception, 2-4 hours for requirements + design)

**When to use which:**
- **Fast Track**: CRUD operations, UI components, API endpoints, clear requirements, experienced team
- **Full Lifecycle**: Unclear requirements, architectural decisions, cross-team coordination, high risk/impact

### Setup Guides
Step-by-step instructions for each scenario:
- [Tier 1+2 ADO Setup](./tier1-2-ado-agent-setup.md) — Greenfield and brownfield, Azure DevOps (Volaris L1→L2)
- [Tier 3: GitHub Platform Layer](./setup/tier-3-setup.md) (1-2 days, Volaris L2→L3, GitHub only)

### Role Guides
How each role works with the AI-evolved SDLC:
- [Developer Guide](./roles/developer-guide.md)
- [Architect Guide](./roles/architect-guide.md)
- [Product Owner Guide](./roles/product-owner-guide.md)
- [QA Tester Guide](./roles/qa-tester-guide.md)
- [DevOps Guide](./roles/devops-guide.md)

### Prompt Collections
Ready-to-use prompts organised by activity:
- [Setup Prompts](./prompts/setup-prompts.md)
- [Daily Prompts](./prompts/daily-prompts.md)
- [Maintenance Prompts](./prompts/maintenance-prompts.md)

### Policies & Protocols
- [Spec Location Policy](./spec-location-policy.md) - Where requirements and designs live for each scenario
- [Ticket Integration Policy](./ticket-integration-policy.md) - How specs connect to ADO/GitHub tickets
- [Context Memory Guide](./context-memory-guide.md) - **MANDATORY**: How to use `.ai/memory/` folder for architectural context
- [Governance](./governance.md) - Ensuring compliance, what's enforceable, what's not
