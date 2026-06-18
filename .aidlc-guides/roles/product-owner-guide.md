# Product Owner Guide: Working with AI-Evolved SDLC

Your agent automatically understands the business domain from .ai/ context. Your workflow depends on your team's Volaris maturity level.

## Your Role by Maturity Level

| Stage | Volaris L1 (AI Assisted) | Volaris L2 (AI Directed) | Volaris L3 (AI Delegated) |
|-------|--------------------------|--------------------------|---------------------------|
| Requirements | Write requirements with AI help | Review AI-generated requirements | Approve agent-generated requirements |
| Backlog | Prioritise manually | Prioritise with AI suggestions | Agent auto-prioritises, you override |
| Approval | Approve requirements | Approve requirements | Approve requirements |
| Tracking | Manual tracking | Track via spec progress | Track via agent metrics |

---

## Volaris L1: AI Assisted

You write requirements. AI helps structure them and identify gaps.

### Daily Prompts
```
Help me write requirements for [feature]
```
```
Check these requirements for gaps or contradictions
```
```
Suggest acceptance criteria for [user story]
```
```
Prioritise this backlog based on business value
```

---

## Volaris L2: AI Directed

You describe intent. AI generates structured requirements. You review and approve.

### Daily Prompts

> **Before creating a spec:** Ensure the ADO MCP server is running. The agent must create an Epic in ADO before generating any files or branches (Rule 12). If ADO MCP is unavailable, the agent will stop and ask you to resolve it first.

```
Create AIDLC spec for [feature name]
Feature: [what it does]
Why: [business value]
Users: [who benefits]
```
```
Create AIDLC bugfix spec for [bug description]
```
```
Summarise progress on [feature/sprint]
```

---

## Volaris L3: AI Delegated

Agent generates requirements from issues automatically. You approve at the gate.

### Daily Prompts
```
Review the agent's requirements for [issue] - approve or request changes
```
```
The agent's requirements need changes: [feedback]
```
```
Show me agent metrics: % tickets deflected, completion rate
```

---

## Tips

- At all levels: requirements approval is always your decision
- At L1: AI helps you write better requirements
- At L2: AI writes requirements from your intent, you validate business accuracy
- At L3: AI writes requirements from issues, you approve the gate
- Keep .ai/context/business-domain.md current so AI understands your domain
- Specs always live in the repo where the code changes - see [Spec Location Policy](../spec-location-policy.md)
