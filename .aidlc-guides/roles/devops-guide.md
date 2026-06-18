# DevOps Guide: Working with AI-Evolved SDLC

Your agent automatically follows security requirements and references infrastructure patterns. Your workflow depends on your team's Volaris maturity level.

## Your Role by Maturity Level

| Stage | Volaris L1 (AI Assisted) | Volaris L2 (AI Directed) | Volaris L3 (AI Delegated) |
|-------|--------------------------|--------------------------|---------------------------|
| Deployment | Write scripts with AI help | Review AI-generated scripts | Agent deploys, you approve prod |
| IaC | Write IaC with AI help | Review AI-generated IaC | Agent manages IaC |
| Security Scans | Run manually with AI help | AI runs and reports | Agent runs automatically |
| Audits | Manual with templates (Tier 2) | Manual with AI assistance | Agent runs scheduled audits |
| Monitoring | Configure with AI help | AI generates config | Agent monitors and alerts |
| Incidents | Triage with AI help | AI correlates and suggests | Agent triages automatically |

---

## Volaris L1: AI Assisted

You manage infrastructure. AI helps with scripts, IaC, and troubleshooting.

### Daily Prompts
```
Help me write deployment config for [service]
```
```
Generate IaC for [resource]
```
```
Help me troubleshoot this deployment issue: [description]
```
```
Suggest monitoring alerts for [service]
```

---

## Volaris L2: AI Directed

You set intent. AI generates deployment configs, IaC, and monitoring. You review.

### Daily Prompts
```
Generate deployment configuration for [service]
```
```
Create CI/CD pipeline for [service]
```
```
Run security audit
```
```
Run infrastructure audit
```
```
Generate monitoring configuration for [service]
```
```
Analyse this incident: [description]
```
```
Run weekly maintenance checks
```

---

## Volaris L3: AI Delegated

Agents handle deployment, monitoring, and audits. You oversee and approve production.

### Daily Prompts
```
Review the agent's deployment plan for [service]
```
```
Approve production deployment for [release]
```
```
Review this week's automated audit results
```
```
Review agent's incident analysis for [issue]
```

---

## Setup Responsibilities

You own the setup of all tiers:
- [Tier 1+2 Setup](../tier1-2-ado-agent-setup.md) - Repository structure and templates
- [Tier 3 Setup](../setup/tier-3-setup.md) - GitHub agents and workflows (L3)

## Tips

- At L1: AI is your infrastructure assistant
- At L2: AI generates infrastructure, you validate and deploy
- At L3: Agents deploy and monitor, you approve production and oversee
- At all levels: keep .ai/rules/security-requirements.md strict
- Tier 3 agents need 2 weeks of monitoring before trusting fully
