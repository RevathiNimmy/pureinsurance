# Governance: Ensuring AI-SDLC Compliance

## The Reality

AI agents follow the most recent instruction. If someone tells the agent to ignore rules, it will. You cannot technically lock an AI agent to only follow your rules.

This means governance is about making the right path the easiest path, and catching deviations after the fact.

## Risk Scenarios

| Scenario | Impact | Likelihood |
|----------|--------|:---:|
| Developer skips AIDLC workflow for complex feature | No documentation, no traceability, no audit trail | High |
| Developer uses unconfigured agent (web ChatGPT, personal account) | Code doesn't follow conventions, no context | High |
| Developer overrides conventions ("do it this way instead") | Inconsistent patterns in codebase | Medium |
| Developer uses different agent without repo config | Rules not loaded, inconsistent output | Medium |
| Developer copies AI-generated code from outside the repo | No .ai/ context applied, doesn't fit codebase | Medium |

## Defence Layers

### Layer 1: Make the Right Path Easiest (Preventive)

This is the most effective layer. If following the process is easier than bypassing it, most people will follow it.

| Control | How It Works |
|---------|-------------|
| Auto-loaded agent rules | Rules load automatically every session - developer doesn't need to remember |
| Simple prompts | "Create spec for X" is easier than figuring it out manually |
| .ai/ context always available | Agent reads conventions without being asked |
| One-prompt setup | Getting started is trivial, no excuse not to |

### Layer 2: Detect Deviations (Detective)

Catch non-compliant work at review time, not at coding time.

| Control | How It Works | Tier |
|---------|-------------|:---:|
| PR validation workflow | Automated checks on every PR (linting, standards, test coverage) | Tier 3 |
| Code review checklist | Reviewer checks: does this follow .ai/ conventions? | All |
| Tech reviewer agent | Automated deep review of PRs against standards | Tier 3 |
| CI linters and formatters | Catch coding standard violations in pipeline | All |
| Test coverage requirements | Enforce minimum coverage in CI | All |
| .ai/ structure validation | CI check that .ai/ files exist and aren't empty | All |

### Layer 3: Correct After the Fact (Corrective)

When deviations are found, fix them.

| Control | How It Works |
|---------|-------------|
| Weekly validation prompt | Checks .ai/ is current, flags drift |
| Documentation drift detector | Detects when docs don't match code (Tier 3) |
| Audit templates | Periodic audits catch systemic issues |
| Retrospectives | Team discusses what's working and what's being bypassed |

## What You Can Enforce Technically

| Enforceable | How |
|-------------|-----|
| Coding standards | CI linters fail the build |
| Test coverage | CI coverage gates fail the build |
| PR required | Branch protection rules prevent direct push to main |
| Review required | Branch protection rules require approvals |
| .ai/ directory exists | CI check fails if missing |
| Secrets not in code | GitHub secret scanning / pre-commit hooks |

## What You Cannot Enforce Technically

| Not Enforceable | Why | Mitigation |
|----------------|-----|-----------|
| Using AIDLC workflow for complex features | Can't detect "this should have been a spec" | Team culture, retrospectives |
| Using the configured agent (not web ChatGPT) | Can't control what tool someone uses | Training, make configured agent easier |
| Following .ai/ conventions in generated code | Agent may be overridden by user | PR review catches it |
| Keeping .ai/ files current | Agent may not auto-update | Weekly validation prompt |
| Not copying code from external AI | Can't detect origin of code | PR review, style checks |

## Recommended Governance Model

### For All Teams (Minimum)

1. **Branch protection**: Require PRs, require reviews, no direct push to main
2. **CI checks**: Linters, formatters, test coverage gates
3. **Agent configuration**: Ensure every developer has the configured agent (not just web ChatGPT)
4. **Training**: Ensure everyone knows the prompts and workflow
5. **Weekly validation**: Run the validation prompt to catch drift

### Add at Tier 3 (GitHub Layer)

6. **PR validation workflow**: Automated quality checks on every PR
7. **Tech reviewer agent**: Automated deep review
8. **Documentation drift detector**: Catches stale .ai/ files
9. **Scheduled audits**: Periodic automated compliance checks

### Team Culture (Most Important)

10. **Champions**: People already at L2-L3 help others
11. **Retrospectives**: Discuss what's being bypassed and why
12. **Make it easy**: If people bypass the process, ask why - maybe the process needs simplifying
13. **Celebrate compliance**: Recognise teams that follow the process well
14. **Don't punish**: If someone bypasses accidentally, help them, don't blame them

## CI Validation Check

Add this to your CI pipeline to enforce the minimum structure:

```yaml
# GitHub Actions example
- name: Validate AI-SDLC structure
  run: |
    # Check .ai/ exists
    if [ ! -d ".ai" ]; then echo "FAIL: .ai/ directory missing"; exit 1; fi
    
    # Check required files exist
    for file in .ai/memory/architecture.md .ai/memory/conventions.md .ai/rules/coding-standards.md; do
      if [ ! -f "$file" ]; then echo "FAIL: $file missing"; exit 1; fi
    done
    
    # Check files aren't empty
    for file in .ai/memory/*.md .ai/rules/*.md; do
      if [ ! -s "$file" ]; then echo "WARN: $file is empty"; fi
    done
    
    echo "PASS: AI-SDLC structure valid"
```

```yaml
# Azure DevOps example
- script: |
    if [ ! -d ".ai" ]; then echo "##vso[task.logissue type=error].ai/ directory missing"; exit 1; fi
    for file in .ai/memory/architecture.md .ai/memory/conventions.md .ai/rules/coding-standards.md; do
      if [ ! -f "$file" ]; then echo "##vso[task.logissue type=error]$file missing"; exit 1; fi
    done
    echo "AI-SDLC structure valid"
  displayName: 'Validate AI-SDLC structure'
```

## Key Principle

**You cannot prevent bypass. You can make compliance easier than bypass, and detect deviations quickly.**

The governance model is:
1. Make the right path easiest (auto-loaded rules, simple prompts)
2. Detect deviations at PR time (CI checks, review)
3. Correct quickly (weekly validation, retrospectives)
4. Build culture (champions, training, don't punish)
