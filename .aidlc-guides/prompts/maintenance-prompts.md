# Maintenance Prompts

> **Note:** The Foundation (Tier 1+2) is now the default starting point for all teams. Tier 1-only setup is considered legacy and not recommended for new projects. All new adoptions should begin with both .ai/ and .aidlc-templates/.

## Weekly Validation

### Structure Check
```
Validate the repository AI structure:
1. Check .ai/ - all required files exist and have content
2. Check .aidlc-rule-details/ - workflow rules present
3. Check .aidlc-templates/ - templates present (if Tier 2)
4. Check architecture.md matches actual code structure
5. Check conventions.md reflects actual patterns
6. Check dependencies.md matches package files
7. Identify stale content (not updated in 90+ days)
8. Report findings and suggest updates
```

### Dependency Check
```
Check dependencies:
1. Read .ai/memory/dependencies.md
2. Compare against package.json / requirements.txt / go.mod
3. Flag new dependencies not documented
4. Flag removed dependencies still documented
5. Update dependencies.md
```

## After Code Changes

### Update Documentation
```
I've changed [component/feature]. Update relevant .ai/ files:

Changes: [describe]

Update only files affected:
- .ai/memory/architecture.md (if architecture changed)
- .ai/memory/conventions.md (if new patterns introduced)
- .ai/memory/decisions.md (if architectural decision made)
- .ai/memory/dependencies.md (if dependencies changed)
- .ai/memory/glossary.md (if new domain terms)
```

### Record a Decision
```
Record this architectural decision:

Decision: [what was decided]
Context: [why needed]
Options: [what was considered]
Chosen: [which option]
Rationale: [why]

Add to .ai/memory/decisions.md using ADR format.
```

## Periodic Reviews

### Architecture Drift Check
```
Check for architecture drift:
1. Read .ai/memory/architecture.md
2. Scan actual codebase structure
3. Compare documented vs actual architecture
4. Flag any drift (new components, removed components, changed patterns)
5. Suggest updates to architecture.md
```

### Convention Drift Check
```
Check for convention drift:
1. Read .ai/memory/conventions.md
2. Scan recent code changes
3. Identify patterns that don't match documented conventions
4. Flag new patterns that should be documented
5. Suggest updates to conventions.md
```

### Security Review
```
Periodic security review:
1. Read .ai/rules/security-requirements.md
2. Scan codebase for violations
3. Check dependencies for known vulnerabilities
4. Review access patterns and authentication
5. Generate security report with findings
```

## Onboarding

### New Team Member
```
I'm new to this project. Help me understand it:
1. Summarise .ai/memory/architecture.md
2. Highlight key patterns from .ai/memory/conventions.md
3. Explain important decisions from .ai/memory/decisions.md
4. List critical dependencies from .ai/memory/dependencies.md
5. Define key terms from .ai/memory/glossary.md
Give me a 10-minute overview.
```

### New AI Agent
```
I'm using you for the first time on this project.
Read and summarise:
1. .ai/memory/architecture.md
2. .ai/memory/conventions.md
3. .ai/rules/coding-standards.md
Confirm you understand the project before we start.
```

## Cross-Repository

### Multi-Repo Consistency
```
This repository is part of a multi-repo system.
1. Check .ai/ structure matches our standard
2. Check .ai/context/integration-points.md lists all external dependencies
3. Ensure conventions match other repositories
4. Flag inconsistencies
```

## Troubleshooting

### AI Not Following Standards
```
Before starting, read .ai/memory/conventions.md and .ai/rules/coding-standards.md.
Confirm you understand and will follow these standards.
Show me an example of how you'd implement [pattern] following our conventions.
```

### AI Ignoring Context
```
This is important: Read .ai/memory/architecture.md before proceeding.
Summarise what you understand about [component].
Then help me with [task].
```

### Stale Documentation
```
Check which .ai/ files need updating:
1. Compare file modification dates against recent code changes
2. Flag files not updated in 90+ days
3. Suggest specific updates needed
4. Prioritise by impact
```
