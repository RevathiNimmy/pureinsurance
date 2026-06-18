<#
.SYNOPSIS
    Removes the AI-SDLC framework from the current repository.

.DESCRIPTION
    Safely removes AI-SDLC framework files and directories from your repository.
    Provides options for complete removal or selective cleanup.

    CAUTION: This will delete framework files. Ensure you have backups or committed
    work before running this script.

    Run from the ROOT of your target repository:
        .\scripts\remove-framework.ps1

.PARAMETER Tier
    Which tier to remove. Valid values:
    - 1 (Remove Tier 1: AIDLC Core only)
    - 2 (Remove Tier 2: Templates only)
    - 3 (Remove Tier 3: GitHub layer only)
    - all (Remove everything)

.PARAMETER KeepSpecs
    Keep .aidlc/specs/ directory with feature specifications.

.PARAMETER KeepAiContext
    Keep .ai/ directory with project context.

.PARAMETER Force
    Skip confirmation prompts.

.PARAMETER Verbose
    Enable verbose output for debugging.

.EXAMPLE
    .\scripts\remove-framework.ps1 -Tier all
    .\scripts\remove-framework.ps1 -Tier 1 -KeepSpecs
    .\scripts\remove-framework.ps1 -Tier all -KeepAiContext -Force
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [ValidateSet('1', '2', '3', 'all')]
    [string]$Tier,

    [switch]$KeepSpecs,
    [switch]$KeepAiContext,
    [switch]$Force
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

function Confirm-Action($prompt) {
    if ($Force) { return $true }
    $response = Read-Host "$prompt (y/N)"
    return $response -match '^[Yy]$'
}

function Remove-Directory($path, $label) {
    if (Test-Path $path) {
        if (Confirm-Action "Delete $label ($path)?") {
            Remove-Item -Path $path -Recurse -Force
            Print-Ok "Removed: $label"
            return $true
        } else {
            Print-Info "Kept: $label"
            return $false
        }
    } else {
        Write-Verbose "Not found (skipping): $path"
        return $false
    }
}

function Remove-File($path, $label) {
    if (Test-Path $path) {
        if (Confirm-Action "Delete $label ($path)?") {
            Remove-Item -Path $path -Force
            Print-Ok "Removed: $label"
            return $true
        } else {
            Print-Info "Kept: $label"
            return $false
        }
    } else {
        Write-Verbose "Not found (skipping): $path"
        return $false
    }
}

# ============================================================================
# Removal Functions
# ============================================================================

function Remove-Tier1 {
    Print-Info 'Removing Tier 1 (AIDLC Core)...'
    Write-Host ''

    $removed = 0

    # Remove rule details
    $removed += Remove-Directory '.aidlc-rule-details' 'AIDLC rule details'

    # Remove AIDLC config (but keep specs if requested)
    if ($KeepSpecs) {
        Print-Warn '.aidlc/specs/ will be preserved'
        $removed += Remove-File '.aidlc\config.json' 'AIDLC config file'
    } else {
        $removed += Remove-Directory '.aidlc' 'AIDLC configuration and specs'
    }

    # Remove AI context
    if ($KeepAiContext) {
        Print-Warn '.ai/ directory will be preserved'
    } else {
        $removed += Remove-Directory '.ai' 'AI context directory'
    }

    # Remove agent configs (Tier 1 installs these)
    $removed += Remove-File '.github\copilot-instructions.md' 'GitHub Copilot config'

    Write-Host ''
    if ($removed -gt 0) {
        Print-Ok "Tier 1 removal: $removed items deleted"
    } else {
        Print-Info 'No Tier 1 items found or all skipped'
    }
}

function Remove-Tier2 {
    Print-Info 'Removing Tier 2 (Reference Templates)...'
    Write-Host ''

    $removed = 0
    $removed += Remove-Directory '.aidlc-templates' 'AIDLC templates'

    Write-Host ''
    if ($removed -gt 0) {
        Print-Ok "Tier 2 removal: $removed items deleted"
    } else {
        Print-Info 'No Tier 2 items found or all skipped'
    }
}

function Remove-Tier3 {
    Print-Info 'Removing Tier 3 (GitHub Layer)...'
    Write-Host ''

    $removed = 0

    # Remove GitHub workflows
    if (Test-Path '.github\workflows\ai-sdlc*.yml') {
        $workflows = Get-ChildItem '.github\workflows\ai-sdlc*.yml'
        foreach ($wf in $workflows) {
            $removed += Remove-File $wf.FullName "GitHub workflow: $($wf.Name)"
        }
    }

    # Remove additional agent configs (Tier 3)
    $removed += Remove-File '.amazonq\rules\ai-sdlc.md' 'Amazon Q config'
    $removed += Remove-File '.kiro\steering\ai-sdlc.md' 'Kiro config'
    $removed += Remove-File 'CLAUDE.md' 'Claude Code config'
    $removed += Remove-File 'AGENTS.md' 'AGENTS.md context file'
    $removed += Remove-File '.chatgpt\instructions.md' 'ChatGPT config'
    $removed += Remove-File '.cursor\rules.md' 'Cursor config'

    # Clean up empty directories
    foreach ($dir in @('.amazonq\rules', '.amazonq', '.kiro\steering', '.kiro', '.chatgpt', '.cursor')) {
        if ((Test-Path $dir) -and ((Get-ChildItem $dir | Measure-Object).Count -eq 0)) {
            Remove-Item $dir -Force
            Write-Verbose "Removed empty directory: $dir"
        }
    }

    Write-Host ''
    if ($removed -gt 0) {
        Print-Ok "Tier 3 removal: $removed items deleted"
    } else {
        Print-Info 'No Tier 3 items found or all skipped'
    }
}

# ============================================================================
# Main
# ============================================================================

Print-Header 'AI-SDLC Framework Removal'
Write-Host ''

# Validation
if (-not (Test-Path '.git')) {
    Print-Err 'Not a git repository. Run from the root of your target repo.'
    exit 1
}

# Warning
Print-Warn 'CAUTION: This will delete AI-SDLC framework files.'
Print-Warn 'Ensure you have committed or backed up any important work.'
Write-Host ''

if (-not $Force) {
    $proceed = Read-Host "Are you sure you want to remove Tier $Tier? (yes/N)"
    if ($proceed -ne 'yes') {
        Print-Info 'Cancelled.'
        exit 0
    }
}

Write-Host ''

# Perform removal
switch ($Tier) {
    '1' {
        Remove-Tier1
    }
    '2' {
        Remove-Tier2
    }
    '3' {
        Remove-Tier3
    }
    'all' {
        Remove-Tier1
        Write-Host ''
        Remove-Tier2
        Write-Host ''
        Remove-Tier3
    }
}

Write-Host ''
Print-Header 'Removal Complete'
Write-Host ''

# Git status
Print-Info 'Files have been deleted. Review changes:'
Write-Host '  git status'
Write-Host ''
Print-Info 'If satisfied, commit the removal:'
Write-Host "  git add -A"
Write-Host "  git commit -m 'chore: remove AI-SDLC framework Tier $Tier'"
Write-Host ''

if ($KeepSpecs) {
    Print-Info 'Feature specs preserved in .aidlc/specs/'
}

if ($KeepAiContext) {
    Print-Info 'AI context preserved in .ai/'
}
