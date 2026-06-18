# Developer Guide: Working with AI-Evolved SDLC

Your agent automatically reads .ai/ context, follows coding standards, and applies security rules. Your daily workflow depends on your team's Volaris maturity level.

## Your Role by Maturity Level

| Stage | Volaris L1 (AI Assisted) | Volaris L2 (AI Directed) | Volaris L3 (AI Delegated) |
|-------|--------------------------|--------------------------|---------------------------|
| Requirements | Clarify feasibility | Clarify feasibility | Informed (agent handles) |
| Design | Provide input | Review AI-generated design | Informed (agent handles) |
| Implementation | Write code with AI suggestions | Review AI-generated code | Review final PR only |
| Testing | Write tests with AI help | Review AI-generated tests | Review test results only |
| Code Review | Create PR, review peer code | Create PR, review AI code | Review agent-created PR |
| Git Operations | Manual (branch naming: `task/ADO-[id]-[name]`) | Manual | Agent handles automatically |

---

## Volaris L1: AI Assisted

You write code. AI helps with suggestions, debugging, and drafting.

### Daily Prompts
```
Help me implement [feature/task]
```
```
Suggest how to approach [problem]
```
```
Debug this error: [description]
```
```
Write tests for [component]
```
```
Review my code and suggest improvements
```

---

## Volaris L2: AI Directed

You set intent. AI generates code, tests, and documentation. You review and approve.

### Daily Prompts

> **Before creating a spec:** Ensure the ADO MCP server is running. The agent must create an Epic in ADO before generating any files or branches (Rule 12). If ADO MCP is unavailable, the agent will stop and ask you to resolve it first.

```
Create AIDLC spec for [feature name]
```

> **After spec review:** Request INCEPTION approval before executing tasks (Rule 5). The agent will not proceed to construction without human sign-off.

```
INCEPTION phase is complete for [feature-name].
Please update aidlc-state.md with approval request and summarise
the spec for human review.
```
```
Execute task [number]
```
```
Create PR for my changes, link to [issue/spec number]
```
```
Update .ai/ files for changes to [component]
```
```
Fix this bug: [description]
```

---

## Volaris L3: AI Delegated

Agent works autonomously. You review the final PR and approve.

### Daily Prompts
```
Assign [issue number] to the coding agent
```
```
Review the PR created by the agent for [issue]
```
```
The agent's PR needs changes: [feedback]
```

---

## Tips

- At L1: AI is your assistant - you drive, it helps
- At L2: AI is your team member - you direct, it executes
- At L3: AI is your delivery system - you review, it delivers
- At all levels: the AI already knows your standards from .ai/ rules
- Specs always live in the repo where the code changes - see [Spec Location Policy](../spec-location-policy.md)
