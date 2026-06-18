# AI-SDLC Quick Reference

## Your Repository Setup

### Directory Structure
```
your-repo/
├── .aidlc/                          # ⭐ Specs & workflow data (agent-agnostic)
│   ├── config.json                  # Path configuration
│   ├── specs/{feature-name}/        # Your feature specs
│   └── extensions/                  # Organization rules
│
├── .ai/                              # ⭐ AI context and project memory
│   ├── memory/                      # Project context files
│   ├── rules/                       # Coding standards, security, testing
│   ├── workflows/                   # Development workflows
│   └── context/                     # Additional context
│
├── .aidlc-rule-details/             # ✅ Workflow rules (deployed by framework)
│   ├── common/
│   ├── inception/
│   ├── construction/
│   └── operations/
│
└── [your existing content]
```

---

## Quick Setup (Copy-Paste)

```bash
# Run from your repository directory
mkdir -p .aidlc/specs
mkdir -p .ai/memory .ai/workflows .ai/rules .ai/context

cat > .aidlc/config.json << 'EOF'
{
  "version": "1.0",
  "projectName": "[Your Project Name]",
  "team": "[Your Team Name]",
  "workTracking": {
    "system": "azuredevops",
    "organization": "[your-ado-org]",
    "project": "[Your Project Name]"
  },
  "workItemTypes": {
    "epic": "Epic",
    "story": "User Story",
    "task": "Task",
    "bug": "Bug",
    "issue": "Issue"
  },
  "paths": {
    "specs": ".aidlc/specs",
    "ruleDetails": ".aidlc-rule-details",
    "aiContext": ".ai"
  },
  "requiredApprovals": ["architecture", "security"],
  "deploymentTargets": ["dev", "test", "prod"]
}
EOF

echo "✅ Setup complete!"
```

---

## Creating a New Spec

**Prompt for your AI agent**:
```
Create a new AIDLC spec for feature: [feature-name]

Description: [brief description]

Please use standardized paths:
- Spec location: .aidlc/specs/[feature-name]/
- Rule details: .aidlc-rule-details/

Follow the AI-SDLC workflow.
```

**Expected Files Created**:
```
.aidlc/specs/[feature-name]/
├── requirements.md          # OR bugfix.md for bugfixes
├── design.md
├── tasks.md
├── aidlc-state.md          # Workflow state
├── audit.md                # Audit trail
└── .config.aidlc           # Spec config
```

---

## Key Paths

| What | Where | Why |
|------|-------|-----|
| **New Specs** | `.aidlc/specs/{feature}/` | Agent-agnostic, interoperable |
| **Rule Details** | `.aidlc-rule-details/` | Workflow rules and standards |
| **Extensions** | `.aidlc-rule-details/extensions/` | Organization-specific rules |
| **AI Context** | `.ai/memory/` | Project architecture, conventions |
| **User Guides** | `.aidlc-guides/` | Deployed operational documentation |

---

## Critical Rules

1. ✅ **DO** use `.aidlc/specs/` for new specs
2. ✅ **DO** load rule details from `.aidlc-rule-details/`
3. ✅ **DO** check `.aidlc/config.json` for paths
4. ✅ **DO** populate `.ai/memory/` with project context
5. ❌ **DON'T** move or rename deployed framework directories

---

## Verification

```bash
# Check setup
ls -la .aidlc/
cat .aidlc/config.json

# Check rule details (should exist after framework deployment)
ls -la .aidlc-rule-details/

# Check AI context
ls -la .ai/
```

---

## Agent Interoperability

Your setup works with:
- ✅ GitHub Copilot (configure via `.github/copilot-instructions.md`)
- ✅ Amazon Q (configure via `.amazonq/instructions.md`)
- ✅ Kiro (configure via `.kiro/steering/`)
- ✅ Claude Code (configure via `CLAUDE.md`)
- ✅ ChatGPT (configure per session)
- ✅ Other agents (see `.aidlc-guides/agent-interoperability-guide.md`)

All agents read from `.aidlc/specs/` - no data migration needed when switching!

---

## Common Commands

### Create New Feature Spec
```
Create AIDLC spec for: [feature-name]
Use .aidlc/specs/[feature-name]/ for spec files.
```

### Resume Existing Spec
```
Continue AIDLC spec: [feature-name]
Read from .aidlc/specs/[feature-name]/
```

### Execute Tasks
```
Execute tasks from .aidlc/specs/[feature-name]/tasks.md
Update aidlc-state.md as you progress.
```

---

## Troubleshooting

### Agent creates specs in wrong location
**Fix**: Ensure agent configuration file references `.aidlc/specs/` as the spec location

### Can't find rule details
**Fix**: Rule details are in `.aidlc-rule-details/` (deployed by framework)

### Agent doesn't see existing specs
**Fix**: Ensure agent is configured to read from `.aidlc/specs/` (see `.aidlc-guides/agent-interoperability-guide.md`)

### Missing AI context
**Fix**: Populate `.ai/memory/` with architecture.md, conventions.md, decisions.md

---

## Full Documentation

- **Working Guide**: `.aidlc-guides/working-guide.md` (step-by-step workflow)
- **Interoperability**: `.aidlc-guides/agent-interoperability-guide.md`
- **User Guide**: `.aidlc-guides/user-guide.md`
- **Prompt Cheat Sheet**: `.aidlc-guides/prompt-cheat-sheet.md`

---

## Status

✅ **Ready to use AI-SDLC with standardized paths!**

Your repository is configured for:
- Agent-agnostic spec storage
- Interoperability between AI agents
- Framework rule details in place
- Future-proof structure

**Next**: Create your first spec using the prompt above! 🚀
