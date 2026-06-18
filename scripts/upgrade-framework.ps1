<#
.SYNOPSIS
    Upgrades the AI-SDLC framework in the current repository to the latest version.

.DESCRIPTION
    Updates AI-SDLC framework files to the latest version from the ai-dlc repository.
    Preserves customizations in .ai/ directory and feature specs in .aidlc/specs/.

    Safely upgrades:
    - Rule files (.aidlc-rule-details/)
    - Templates (.aidlc-templates/)
    - Agent configurations (with merge strategy)
    - Configuration schema (.aidlc/config.json structure)

    Run from the ROOT of your target repository:
        .\scripts\upgrade-framework.ps1

.PARAMETER SourcePath
    Path to the ai-dlc framework repository (default: C:\source\insurer\AI\ai-dlc).

.PARAMETER Tier
    Which tier to upgrade. Valid values:
    - 1 (Upgrade Tier 1: AIDLC Core)
    - 2 (Upgrade Tier 2: Templates)
    - 3 (Upgrade Tier 3: GitHub layer and agent configs)
    - all (Upgrade everything)

.PARAMETER DryRun
    Show what would be upgraded without making changes.

.PARAMETER Force
    Overwrite customized files without prompting.

.PARAMETER Verbose
    Enable verbose output for debugging.

.EXAMPLE
    .\scripts\upgrade-framework.ps1 -Tier all
    .\scripts\upgrade-framework.ps1 -Tier 1 -DryRun
    .\scripts\upgrade-framework.ps1 -SourcePath "C:\my-ai-dlc" -Force
#>

[CmdletBinding()]
param(
    [string]$SourcePath = 'C:\source\insurer\AI\ai-dlc',

    [ValidateSet('1', '2', '3', 'all')]
    [string]$Tier = 'all',

    [switch]$DryRun,
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
    if ($Force -or $DryRun) { return $true }
    $response = Read-Host "$prompt (y/N)"
    return $response -match '^[Yy]$'
}

function Copy-FrameworkDir($src, $dst, $label) {
    if (-not (Test-Path $src)) {
        Print-Warn "$label — source not found: $src (skipping)"
        return 0
    }

    $fileCount = (Get-ChildItem -Path $src -Recurse -File |
                  Where-Object { $_.Name -ne '.gitkeep' }).Count

    if ($fileCount -eq 0) {
        Print-Warn "$label — source folder is empty (skipping)"
        return 0
    }

    if ($DryRun) {
        Print-Info "[DRY RUN] Would upgrade: $label ($fileCount files)"
        return $fileCount
    }

    if (-not (Confirm-Action "Upgrade $label ($fileCount files)?")) {
        Print-Info "Skipped: $label"
        return 0
    }

    New-Item -ItemType Directory -Path $dst -Force | Out-Null

    $files = Get-ChildItem -Path $src -Recurse -File | Where-Object { $_.Name -ne '.gitkeep' }
    $copied = 0

    foreach ($file in $files) {
        $relPath = $file.FullName.Substring($src.Length).TrimStart('\')
        $dstFile = Join-Path $dst $relPath
        $dstDir  = Split-Path -Parent $dstFile

        if (-not (Test-Path $dstDir)) {
            New-Item -ItemType Directory -Path $dstDir -Force | Out-Null
        }

        Copy-Item -Path $file.FullName -Destination $dstFile -Force
        Write-Verbose "  Copied: $relPath"
        $copied++
    }

    Print-Ok "$label upgraded ($copied files)"
    return $copied
}

function Copy-FrameworkFile($src, $dst, $label, [switch]$PreserveCustomizations) {
    if (-not (Test-Path $src)) {
        Print-Warn "$label — source file not found: $src (skipping)"
        return $false
    }

    if ($DryRun) {
        if (Test-Path $dst) {
            Print-Info "[DRY RUN] Would update: $label"
        } else {
            Print-Info "[DRY RUN] Would create: $label"
        }
        return $true
    }

    $exists = Test-Path $dst
    $action = if ($exists) { 'Update' } else { 'Create' }

    if ($exists -and $PreserveCustomizations) {
        Print-Warn "$label exists and may have customizations"
        if (-not (Confirm-Action "$action $label? (customizations will be lost)")) {
            Print-Info "Skipped: $label"
            return $false
        }
    }

    if ($exists -and -not $PreserveCustomizations) {
        if (-not (Confirm-Action "$action $label?")) {
            Print-Info "Skipped: $label"
            return $false
        }
    }

    $dstDir = Split-Path -Parent $dst
    if (-not (Test-Path $dstDir)) {
        New-Item -ItemType Directory -Path $dstDir -Force | Out-Null
    }

    Copy-Item -Path $src -Destination $dst -Force
    Print-Ok "$action`: $label"
    return $true
}

# ============================================================================
# Validation
# ============================================================================

function Test-GitRepo {
    if (-not (Test-Path '.git')) {
        Print-Err 'Not a git repository. Run from the root of your target repo.'
        exit 1
    }
}

function Test-SourceRepo {
    if (-not (Test-Path $SourcePath)) {
        Print-Err "Source framework not found: $SourcePath"
        Print-Info 'Set -SourcePath to the location of your ai-dlc repository'
        exit 1
    }

    $frameworkTier1 = Join-Path $SourcePath 'framework\tier1'
    if (-not (Test-Path $frameworkTier1)) {
        Print-Err "Invalid source repository: $SourcePath"
        Print-Info 'SourcePath must point to the ai-dlc repository root'
        exit 1
    }
}

function Test-AidlcInstalled {
    if (-not (Test-Path '.aidlc')) {
        Print-Err 'AI-SDLC not installed in this repository'
        Print-Info 'Run setup scripts first, or use upgrade on a repository with AI-SDLC'
        exit 1
    }
}

# ============================================================================
# Upgrade Functions
# ============================================================================

function Upgrade-Tier1 {
    Print-Info 'Upgrading Tier 1 (AIDLC Core)...'
    Write-Host ''

    $frameworkTier1 = Join-Path $SourcePath 'framework\tier1'
    $upgraded = 0

    # Upgrade rule details
    $folders = @('common', 'inception', 'construction', 'operations', 'extensions')
    foreach ($folder in $folders) {
        $upgraded += Copy-FrameworkDir `
            (Join-Path $frameworkTier1 ".aidlc-rule-details\$folder") `
            ".aidlc-rule-details\$folder" `
            "Rule details: $folder"
    }

    # Note: .ai/ is NOT upgraded (contains project-specific customizations)
    Print-Info '.ai/ directory preserved (contains project customizations)'

    # Note: .aidlc/specs/ is NOT upgraded (contains active feature work)
    Print-Info '.aidlc/specs/ preserved (contains feature specifications)'

    # Offer to update config.json schema (but preserve values)
    if (Test-Path '.aidlc\config.json') {
        Print-Info '.aidlc/config.json exists (manual review recommended for schema changes)'
    }

    Write-Host ''
    if ($upgraded -gt 0) {
        Print-Ok "Tier 1 upgrade: $upgraded items updated"
    } else {
        Print-Info 'No Tier 1 items upgraded'
    }
}

function Upgrade-Tier2 {
    Print-Info 'Upgrading Tier 2 (Reference Templates)...'
    Write-Host ''

    $frameworkTier2 = Join-Path $SourcePath 'framework\tier2'
    $upgraded = 0

    $upgraded += Copy-FrameworkDir `
        (Join-Path $frameworkTier2 '.aidlc-templates') `
        '.aidlc-templates' `
        'Templates library'

    Write-Host ''
    if ($upgraded -gt 0) {
        Print-Ok "Tier 2 upgrade: $upgraded items updated"
    } else {
        Print-Info 'No Tier 2 items upgraded'
    }
}

function Upgrade-Tier3 {
    Print-Info 'Upgrading Tier 3 (GitHub Layer & Agent Configs)...'
    Write-Host ''

    $agentConfigs = Join-Path $SourcePath 'guides\setup\agent-configs'
    $upgraded = 0

    # Note: GitHub workflows would go here when Tier 3 is fully defined

    # Agent configurations (mark as customizable)
    Print-Info 'Agent configurations may contain customizations'
    Print-Warn 'Review templates after upgrade and merge changes manually if needed'
    Write-Host ''

    # We don't auto-upgrade agent configs because they're heavily customized
    # Instead, show what's available
    Print-Info 'Available agent config templates (in ai-dlc repo):'
    Write-Host "  - $agentConfigs\copilot.md"
    Write-Host "  - $agentConfigs\amazonq.md"
    Write-Host "  - $agentConfigs\kiro.md"
    Write-Host "  - $agentConfigs\claude.md"
    Write-Host "  - $agentConfigs\chatgpt.md"
    Write-Host ''
    Print-Info 'Use scripts\deploy-agent-config.ps1 to deploy new agent configs'

    Write-Host ''
    if ($upgraded -gt 0) {
        Print-Ok "Tier 3 upgrade: $upgraded items updated"
    } else {
        Print-Info 'Tier 3: Review and deploy agent configs manually'
    }
}

# ============================================================================
# Main
# ============================================================================

Print-Header 'AI-SDLC Framework Upgrade'
Write-Host ''

if ($DryRun) {
    Print-Info 'DRY RUN MODE - No changes will be made'
    Write-Host ''
}

Print-Info 'Validation checks...'
Test-GitRepo
Test-SourceRepo
Test-AidlcInstalled
Print-Ok 'Checks passed'
Write-Host ''

# Git status check
$gitStatus = git status --porcelain
if ($gitStatus -and -not $DryRun) {
    Print-Warn 'Repository has uncommitted changes'
    if (-not (Confirm-Action 'Continue with upgrade?')) {
        Print-Info 'Cancelled. Commit or stash changes first.'
        exit 0
    }
    Write-Host ''
}

# Perform upgrade
switch ($Tier) {
    '1' {
        Upgrade-Tier1
    }
    '2' {
        Upgrade-Tier2
    }
    '3' {
        Upgrade-Tier3
    }
    'all' {
        Upgrade-Tier1
        Write-Host ''
        Upgrade-Tier2
        Write-Host ''
        Upgrade-Tier3
    }
}

Write-Host ''
Print-Header 'Upgrade Complete'
Write-Host ''

if ($DryRun) {
    Print-Info 'Dry run complete. No changes were made.'
    Print-Info 'Run without -DryRun to apply the upgrade.'
} else {
    Print-Info 'Next steps:'
    Write-Host '  1. Review changes: git status'
    Write-Host '  2. Test the upgraded framework'
    Write-Host '  3. Review .ai/ and agent configs for needed updates'
    Write-Host '  4. Commit changes:'
    Write-Host '       git add -A'
    Write-Host "       git commit -m 'chore: upgrade AI-SDLC framework'"
    Write-Host ''
    Print-Warn 'Customized files in .ai/ and agent configs were preserved'
    Print-Warn 'Review template changes and merge manually if needed'
}
