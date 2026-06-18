<#
.SYNOPSIS
    Initializes a new feature specification workspace in .aidlc/specs/.

.DESCRIPTION
    Creates the standard AI-SDLC feature workspace structure:
    - .aidlc/specs/{feature-name}/requirements.md
    - .aidlc/specs/{feature-name}/design.md
    - .aidlc/specs/{feature-name}/tasks.md
    - .aidlc/specs/{feature-name}/aidlc-state.md
    - .aidlc/specs/{feature-name}/audit.md

    Also creates an integration branch for the feature if requested.

    Run from the ROOT of your target repository:
        .\scripts\initialize-feature.ps1 -FeatureName "user-authentication" -EpicId 12345

.PARAMETER FeatureName
    The name of the feature (kebab-case recommended, e.g., "user-authentication").

.PARAMETER EpicId
    The ADO Epic ID for this feature (e.g., 12345).

.PARAMETER CreateBranch
    Automatically create the integration branch (feature/ADO-{epic-id}-{feature-name}).

.PARAMETER BaseBranch
    The branch to create the integration branch from (default: main).

.PARAMETER Verbose
    Enable verbose output for debugging.

.EXAMPLE
    .\scripts\initialize-feature.ps1 -FeatureName "user-auth" -EpicId 12345
    .\scripts\initialize-feature.ps1 -FeatureName "payment-gateway" -EpicId 67890 -CreateBranch
    .\scripts\initialize-feature.ps1 -FeatureName "api-v2" -EpicId 11111 -CreateBranch -BaseBranch develop
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [string]$FeatureName,

    [Parameter(Mandatory=$true)]
    [int]$EpicId,

    [switch]$CreateBranch,

    [string]$BaseBranch = 'main'
)

$ErrorActionPreference = 'Stop'

# ============================================================================
# Helpers
# ============================================================================

function Print-Header($msg) {
    Write-Host ('=' * 50) -ForegroundColor Blue
    Write-Host $msg -ForegroundColor Blue
    Write-Host ('=' * 50) -ForegroundColor Blue
}

function Print-Ok($msg)   { Write-Host "✓ $msg" -ForegroundColor Green  }
function Print-Err($msg)  { Write-Host "✗ $msg" -ForegroundColor Red    }
function Print-Info($msg) { Write-Host "ℹ $msg" -ForegroundColor Cyan   }
function Print-Warn($msg) { Write-Host "⚠ $msg" -ForegroundColor Yellow }

# ============================================================================
# Validation
# ============================================================================

function Test-GitRepo {
    if (-not (Test-Path '.git')) {
        Print-Err 'Not a git repository. Run from the root of your target repo.'
        exit 1
    }
}

function Test-AidlcInstalled {
    if (-not (Test-Path '.aidlc')) {
        Print-Err 'AI-SDLC not installed. Run setup scripts first.'
        Print-Info 'See: guides\setup\tier-1-setup.md'
        exit 1
    }
}

function Test-FeatureName {
    if ($FeatureName -notmatch '^[a-z0-9-]+$') {
        Print-Warn "Feature name should be kebab-case (lowercase, hyphens only): $FeatureName"
        $response = Read-Host "Continue anyway? (y/N)"
        if ($response -notmatch '^[Yy]$') {
            Print-Info 'Cancelled.'
            exit 0
        }
    }
}

# ============================================================================
# Main
# ============================================================================

Print-Header 'AI-SDLC Feature Workspace Initializer'
Write-Host ''

Print-Info 'Validation checks...'
Test-GitRepo
Test-AidlcInstalled
Test-FeatureName
Print-Ok 'Checks passed'
Write-Host ''

$specDir = ".aidlc\specs\$FeatureName"
$branchName = "feature/ADO-$EpicId-$FeatureName"

# Check if feature already exists
if (Test-Path $specDir) {
    Print-Err "Feature workspace already exists: $specDir"
    Print-Info 'Use a different feature name or delete the existing workspace.'
    exit 1
}

# Create spec directory
Print-Info "Creating feature workspace: $specDir"
New-Item -ItemType Directory -Path $specDir -Force | Out-Null
Print-Ok "Created directory: $specDir"

# Create requirements.md
$requirementsContent = @"
# Requirements: $FeatureName

**Epic ID**: ADO-$EpicId
**Created**: $(Get-Date -Format 'yyyy-MM-dd')
**Status**: Draft

## Overview

[Provide a high-level description of this feature]

## User Stories

### Story 1: [Story Title]
**As a** [user type]
**I want** [goal]
**So that** [benefit]

**Acceptance Criteria**:
- [ ] Criterion 1
- [ ] Criterion 2
- [ ] Criterion 3

### Story 2: [Story Title]
**As a** [user type]
**I want** [goal]
**So that** [benefit]

**Acceptance Criteria**:
- [ ] Criterion 1
- [ ] Criterion 2

## Functional Requirements

1. **[Requirement 1]**
   - Description: [Details]
   - Priority: [High/Medium/Low]

2. **[Requirement 2]**
   - Description: [Details]
   - Priority: [High/Medium/Low]

## Non-Functional Requirements

1. **Performance**: [Define performance expectations]
2. **Security**: [Define security requirements]
3. **Scalability**: [Define scalability requirements]
4. **Accessibility**: [Define accessibility requirements]

## Out of Scope

- [Item 1 that is explicitly NOT part of this feature]
- [Item 2 that is explicitly NOT part of this feature]

## Dependencies

- [External system or service dependency]
- [Another feature or component dependency]

## Success Metrics

- [Metric 1]: [Target value]
- [Metric 2]: [Target value]

## Notes

[Any additional context, considerations, or decisions]
"@

Set-Content -Path "$specDir\requirements.md" -Value $requirementsContent -Encoding UTF8
Print-Ok "Created: requirements.md"

# Create design.md
$designContent = @"
# Design: $FeatureName

**Epic ID**: ADO-$EpicId
**Created**: $(Get-Date -Format 'yyyy-MM-dd')
**Status**: Draft

## Architecture Overview

[Describe the high-level architecture for this feature]

## Component Design

### Component 1: [Name]
- **Purpose**: [What it does]
- **Location**: [File path or module]
- **Dependencies**: [What it depends on]
- **Interfaces**: [Public APIs or contracts]

### Component 2: [Name]
- **Purpose**: [What it does]
- **Location**: [File path or module]
- **Dependencies**: [What it depends on]
- **Interfaces**: [Public APIs or contracts]

## Data Model

### Entity: [EntityName]
``````
{
  "id": "string",
  "field1": "type",
  "field2": "type"
}
``````

## API Design

### Endpoint: [Method] /api/path
- **Description**: [What it does]
- **Request**:
  ``````json
  {
    "param1": "value"
  }
  ``````
- **Response**:
  ``````json
  {
    "result": "value"
  }
  ``````
- **Error Codes**: 400, 404, 500

## Security Considerations

- [Authentication approach]
- [Authorization rules]
- [Data protection measures]
- [Input validation requirements]

## Testing Strategy

- **Unit Tests**: [What to test]
- **Integration Tests**: [What to test]
- **E2E Tests**: [What to test]

## Deployment Considerations

- [Database migrations needed]
- [Configuration changes required]
- [Infrastructure changes needed]
- [Rollback strategy]

## Technical Decisions

### Decision 1: [Title]
- **Context**: [Why we needed to decide]
- **Options Considered**: [What we evaluated]
- **Decision**: [What we chose]
- **Rationale**: [Why we chose it]

## Open Questions

- [ ] [Question 1 that needs resolution]
- [ ] [Question 2 that needs resolution]
"@

Set-Content -Path "$specDir\design.md" -Value $designContent -Encoding UTF8
Print-Ok "Created: design.md"

# Create tasks.md
$tasksContent = @"
# Tasks: $FeatureName

**Epic ID**: ADO-$EpicId
**Created**: $(Get-Date -Format 'yyyy-MM-dd')

## Task List

### ADO-[ID]: [Task Title]
- **Priority**: [1-10, lower = higher priority]
- **Estimate**: [Story points or hours]
- **Dependencies**: [List other task IDs this depends on, or "None"]
- **Description**: [What needs to be done]
- **Acceptance Criteria**:
  - [ ] Criterion 1
  - [ ] Criterion 2

### ADO-[ID]: [Task Title]
- **Priority**: [1-10]
- **Estimate**: [Story points or hours]
- **Dependencies**: [Task IDs or "None"]
- **Description**: [What needs to be done]
- **Acceptance Criteria**:
  - [ ] Criterion 1
  - [ ] Criterion 2

## Task Dependency Graph

``````
ADO-[ID1] (Task 1)
  └─> ADO-[ID2] (Task 2 - depends on Task 1)
      └─> ADO-[ID3] (Task 3 - depends on Task 2)
  └─> ADO-[ID4] (Task 4 - depends on Task 1)
``````

## Notes

- Tasks should be atomic and independently testable
- Add ADO task IDs once created in Azure DevOps
- Update dependencies as the task breakdown evolves
"@

Set-Content -Path "$specDir\tasks.md" -Value $tasksContent -Encoding UTF8
Print-Ok "Created: tasks.md"

# Create aidlc-state.md
$stateContent = @"
# AIDLC State: $FeatureName

**Epic ID**: ADO-$EpicId
**Branch**: $branchName
**Last Updated**: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')

## Task Execution State

| Task ID | Status | Agent | Claimed At | Completed At |
|---------|--------|-------|------------|--------------|
| ADO-[ID] | Available | - | - | - |
| ADO-[ID] | Available | - | - | - |

## Status Values

- **Available**: Ready to be claimed by an agent
- **Claimed**: Agent is working on it
- **Done**: Completed and merged
- **Blocked**: Waiting on dependencies

## Instructions for Agents

1. **Claim a task**: Find a task with Status: Available
2. **Update this file**: Set Status: Claimed | Agent: [your-agent-id]
3. **Commit immediately**: Push the state change to the integration branch
4. **Work on task branch**: Create `task/ADO-[id]-[task-name]` from integration branch
5. **Mark as Done**: Update Status: Done after PR is merged
6. **Promote tasks**: Change Blocked → Available when dependencies complete

## Notes

- Always pull latest integration branch before reading/writing this file
- Only one agent should claim a task at a time
- If conflict occurs, last write wins (other agent must re-claim a different task)
"@

Set-Content -Path "$specDir\aidlc-state.md" -Value $stateContent -Encoding UTF8
Print-Ok "Created: aidlc-state.md"

# Create audit.md
$auditContent = @"
# Audit Log: $FeatureName

**Epic ID**: ADO-$EpicId
**Created**: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')

## Event Log

This is an append-only log of all state changes for this feature.

### Format
Each entry should include:
- Timestamp (ISO 8601)
- Event type (CLAIM, DONE, PROMOTE, BLOCK, NOTE)
- Task ID
- Agent identifier
- Details

---

### $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss') | NOTE | - | system
Feature workspace initialized

---

## Instructions

- **NEVER** delete or modify existing entries
- **ALWAYS** append new entries at the end
- Use this log for:
  - Debugging conflicts
  - Understanding task progression
  - Audit trail for compliance
  - Performance analysis
"@

Set-Content -Path "$specDir\audit.md" -Value $auditContent -Encoding UTF8
Print-Ok "Created: audit.md"

Write-Host ''
Print-Ok "Feature workspace created: $specDir"

# Create integration branch if requested
if ($CreateBranch) {
    Write-Host ''
    Print-Info "Creating integration branch: $branchName"

    try {
        # Check if branch already exists
        $branchExists = git branch --list $branchName
        if ($branchExists) {
            Print-Warn "Branch already exists: $branchName"
        } else {
            # Fetch latest
            git fetch origin $BaseBranch 2>&1 | Out-Null

            # Create and checkout new branch
            git checkout -b $branchName "origin/$BaseBranch" 2>&1 | Out-Null
            Print-Ok "Created branch: $branchName from $BaseBranch"

            # Stage and commit the spec files
            git add $specDir 2>&1 | Out-Null
            git commit -m "chore: initialize feature workspace for $FeatureName (ADO-$EpicId)" 2>&1 | Out-Null
            Print-Ok "Committed spec files to $branchName"

            Print-Info "Push the branch with: git push -u origin $branchName"
        }
    } catch {
        Print-Warn "Branch creation failed: $_"
        Print-Info "You can create it manually with:"
        Write-Host "  git checkout -b $branchName"
    }
}

Write-Host ''
Print-Header 'Initialization Complete'
Write-Host ''

Print-Info 'Next steps:'
Write-Host "  1. Edit $specDir\requirements.md — define user stories and requirements"
Write-Host "  2. Edit $specDir\design.md — design the architecture and components"
Write-Host "  3. Edit $specDir\tasks.md — break down work into atomic tasks"
Write-Host "  4. Add ADO task IDs to tasks.md and aidlc-state.md"

if (-not $CreateBranch) {
    Write-Host "  5. Create integration branch: git checkout -b $branchName"
    Write-Host "  6. Commit spec files: git add $specDir && git commit -m 'chore: initialize $FeatureName'"
    Write-Host "  7. Push: git push -u origin $branchName"
} else {
    Write-Host "  5. Push: git push -u origin $branchName"
}

Write-Host ''
Print-Info 'Working guide: docs\operational-guides\aidlc-working-guide.md'
Print-Info 'Prompt cheat sheet: guides\prompt-cheat-sheet.md'
