# Architect Guide: Working with AI-Evolved SDLC

Your agent automatically reads architecture docs, validates against existing patterns, and uses ADR format. Your workflow depends on your team's Volaris maturity level.

## Your Role by Maturity Level

| Stage | Volaris L1 (AI Assisted) | Volaris L2 (AI Directed) | Volaris L3 (AI Delegated) |
|-------|--------------------------|--------------------------|---------------------------|
| Requirements | Review feasibility | Review feasibility | Review feasibility |
| Design | Create design with AI help | Review AI-generated design | Approve agent-generated design |
| Implementation | Consult on decisions | Consult on decisions | Informed |
| Code Review | Review architecture alignment | Review architecture alignment | Review agent PR for alignment |

---

## Volaris L1: AI Assisted

You create designs. AI helps with diagrams, pattern suggestions, and validation.

### Daily Prompts
```
Help me design [component/feature]
```
```
Suggest architecture patterns for [use case]
```
```
Generate a diagram for [system interaction]
```
```
Record this as an ADR: [decision and rationale]
```
```
Review this code for architecture alignment
```

---

## Volaris L2: AI Directed

You set intent. AI generates the full design. You review and approve.

### Daily Prompts
```
Generate design for [feature] based on approved requirements
```
```
Review the generated design.md - does it align with our architecture?
```
```
Evaluate [technology] for [use case] and draft an ADR
```
```
Review this PR against the approved design for [feature]
```

---

## Volaris L3: AI Delegated

Agent generates designs autonomously. You approve at the gate.

### Daily Prompts
```
Review the agent's design for [feature] - approve or request changes
```
```
The agent's design needs changes: [feedback]
```

---

## Tips

- At all levels: keep .ai/memory/architecture.md current - it's the AI's source of truth
- At L1: AI helps you create, you own the output
- At L2: AI creates, you validate and approve
- At L3: AI creates and validates, you approve the gate
- Architecture approval is always a human decision, regardless of level
- Specs always live in the repo where the code changes - see [Spec Location Policy](../spec-location-policy.md)
