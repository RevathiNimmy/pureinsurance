<#
.SYNOPSIS
    Validates the integrity of the AI-SDLC framework in the current repository.

.DESCRIPTION
    Checks that:
      - All required directories exist
      - Key configuration files are present and non-empty
      - JSON config files are valid
      - AWS AI-DLC rule files have been synced (no empty phase folders)
      - Framework scripts are present

    Run from the ROOT of a repository that has AI-SDLC installed:
        & "C:\source\insurer\AI\ai-dlc\scripts\validate-structure.ps1"

    Or from the ai-dlc repo itself:
        .\scripts\validate-structure.ps1

.PARAMETER Verbose
    Enable verbose output showing every check.

.EXAMPLE
    .\scripts\validate-structure.ps1
    .\scripts\validate-structure.ps1 -Verbose

.OUTPUTS
    Exit code 0 if all checks pass; 1 if any check fails.
#>

[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

$FailedChecks = 0

# ============================================================================
# Helpers
# ============================================================================

function Print-Header($msg) {
    Write-Host ('=' * 44) -ForegroundColor Blue
    Write-Host $msg       -ForegroundColor Blue
    Write-Host ('=' * 44) -ForegroundColor Blue
}

function Print-Section($msg) {
    Write-Host ''
    Write-Host $msg -ForegroundColor Blue
}

function Print-Ok($msg)   { Write-Host "  [OK]   $msg" -ForegroundColor Green  }
function Print-Err($msg)  { Write-Host "  [ERR]  $msg" -ForegroundColor Red;   $script:FailedChecks++ }
function Print-Warn($msg) { Write-Host "  [WARN] $msg" -ForegroundColor Yellow }
function Print-Info($msg) { Write-Host "  [INFO] $msg" -ForegroundColor Cyan   }

function Check-Dir($path, $label, [switch]$Optional) {
    if (Test-Path $path -PathType Container) {
        Write-Verbose "  DIR OK: $path"
        Print-Ok "$label ($path)"
    } elseif ($Optional) {
        Print-Warn "$label not found (optional): $path"
    } else {
        Print-Err "$label not found: $path"
    }
}

function Check-File($path, $label, [switch]$Optional) {
    if (Test-Path $path -PathType Leaf) {
        Write-Verbose "  FILE OK: $path"
        Print-Ok "$label ($path)"
    } elseif ($Optional) {
        Print-Warn "$label not found (optional): $path"
    } else {
        Print-Err "$label not found: $path"
    }
}

function Check-FileNotEmpty($path, $label) {
    if (-not (Test-Path $path -PathType Leaf)) {
        Print-Err "$label not found: $path"
        return
    }
    $size = (Get-Item $path).Length
    if ($size -gt 0) {
        Write-Verbose "  NON-EMPTY OK: $path ($size bytes)"
        Print-Ok "$label is non-empty"
    } else {
        Print-Err "$label is empty: $path"
    }
}

function Check-JsonValid($path, $label) {
    if (-not (Test-Path $path -PathType Leaf)) {
        Print-Err "$label JSON not found: $path"
        return
    }
    try {
        $content = Get-Content $path -Raw
        $null = $content | ConvertFrom-Json
        Print-Ok "$label JSON is valid"
    } catch {
        Print-Err "$label JSON is invalid: $path — $_"
    }
}

function Check-DirPopulated($path, $label) {
    if (-not (Test-Path $path -PathType Container)) {
        Print-Warn "$label not present: $path"
        return
    }
    $count = (Get-ChildItem -Path $path -Recurse -File |
              Where-Object { $_.Name -ne '.gitkeep' }).Count
    if ($count -gt 0) {
        Print-Ok "$label has $count files"
    } else {
        Print-Err "$label is empty (only placeholders): $path"
    }
}

# ============================================================================
# Checks
# ============================================================================

function Test-CoreDirectories {
    Print-Section 'Core Directory Structure'
    Check-Dir '.aidlc-rule-details'  'AIDLC rule details root'
    Check-Dir '.aidlc'               'AIDLC configuration'
    Check-Dir '.ai'                  'AI context root'
    Check-Dir '.github'              'GitHub directory' -Optional
    Check-Dir 'scripts'              'Scripts directory'
    Check-Dir '.cursor'              'Cursor IDE directory' -Optional
}

function Test-RuleDetailDirectories {
    Print-Section 'Rule Detail Folders (.aidlc-rule-details/)'
    foreach ($folder in @('common', 'inception', 'construction', 'operations')) {
        $path = ".aidlc-rule-details\$folder"
        if (Test-Path $path -PathType Container) {
            Check-DirPopulated $path $folder
        } else {
            Print-Err "$folder rules folder not found: $path"
        }
    }
    # extensions is optional
    Check-Dir '.aidlc-rule-details\extensions' 'extensions rules' -Optional
}

function Test-AiSubdirectories {
    Print-Section 'AI Context Subdirectories (.ai/)'
    if (Test-Path '.ai' -PathType Container) {
        # Framework standard subdirectories (all optional, created during deployment)
        foreach ($sub in @('memory', 'rules', 'workflows', 'context')) {
            Check-Dir ".ai\$sub" ".ai/$sub" -Optional
        }
    } else {
        Print-Warn '.ai/ not present — skipping subdirectory checks'
    }
}

function Test-GithubSubdirectories {
    Print-Section 'GitHub Subdirectories (.github/)'
    if (Test-Path '.github' -PathType Container) {
        Check-Dir '.github\agents' '.github/agents' -Optional
        Check-Dir '.github\workflows' '.github/workflows' -Optional
    } else {
        Print-Warn '.github/ not present — skipping subdirectory checks'
    }
}

function Test-ConfigFiles {
    Print-Section 'Configuration Files'
    Check-File  '.aidlc\config.json' 'Main config' -Optional
    if (Test-Path '.aidlc\config.json' -PathType Leaf) {
        Check-JsonValid '.aidlc\config.json' 'Main config'
    }
    Check-File 'README.md'       'Repository README'   -Optional
    Check-File 'CHANGE-LOG.md'   'Change log'          -Optional
    Check-File 'CONTRIBUTING.md' 'Contributing guide'  -Optional
}

function Test-AgentFiles {
    Print-Section 'Agent Configuration Files'

    # GitHub Copilot
    if (Test-Path '.github\copilot-instructions.md' -PathType Leaf) {
        Check-FileNotEmpty '.github\copilot-instructions.md' 'Copilot instructions'
    } else {
        Print-Warn '.github\copilot-instructions.md not found (optional for Tier 1/2; required for Tier 3)'
    }

    # Amazon Q Developer
    if (Test-Path '.amazonq\rules\ai-sdlc.md' -PathType Leaf) {
        Check-FileNotEmpty '.amazonq\rules\ai-sdlc.md' 'Amazon Q rules'
    } else {
        Print-Info 'Amazon Q configuration not found (optional)'
    }

    # Kiro
    if (Test-Path '.kiro\steering\ai-sdlc.md' -PathType Leaf) {
        Check-FileNotEmpty '.kiro\steering\ai-sdlc.md' 'Kiro steering file'
    } else {
        Print-Info 'Kiro configuration not found (optional)'
    }

    # Claude Code
    if (Test-Path 'CLAUDE.md' -PathType Leaf) {
        Check-FileNotEmpty 'CLAUDE.md' 'Claude Code instructions'
    } else {
        Print-Info 'Claude Code configuration not found (optional)'
    }
    if (Test-Path 'AGENTS.md' -PathType Leaf) {
        Check-FileNotEmpty 'AGENTS.md' 'AGENTS.md context file'
    } else {
        Print-Info 'AGENTS.md not found (optional, used by Claude Code and Kiro)'
    }

    # ChatGPT
    if (Test-Path '.chatgpt\instructions.md' -PathType Leaf) {
        Check-FileNotEmpty '.chatgpt\instructions.md' 'ChatGPT instructions'
    } else {
        Print-Info 'ChatGPT configuration not found (optional)'
    }

    # Cursor (legacy support)
    Check-File '.cursor\rules.md' 'Cursor rules' -Optional
    if (Test-Path '.cursor\rules.md' -PathType Leaf) {
        Check-FileNotEmpty '.cursor\rules.md' 'Cursor rules'
    }
}

function Test-ContentIntegrity {
    Print-Section 'Content Integrity (placeholder check)'
    $patterns = @('\[PLACEHOLDER\]', '\[TODO\]', '\[FIXME\]')
    $criticalFiles = @('CHANGE-LOG.md', 'README.md', '.aidlc\config.json')
    $found = $false
    foreach ($file in $criticalFiles) {
        if (Test-Path $file -PathType Leaf) {
            $content = Get-Content $file -Raw -ErrorAction SilentlyContinue
            foreach ($p in $patterns) {
                if ($content -match $p) {
                    Print-Warn "Placeholder '$p' found in: $file"
                    $found = $true
                }
            }
        }
    }
    if (-not $found) { Print-Ok 'No placeholder patterns found in critical files' }
}

function Test-MarkdownFiles {
    Print-Section 'Markdown Files'
    $mdCount = (Get-ChildItem -Recurse -Filter '*.md' -ErrorAction SilentlyContinue |
                Where-Object { $_.FullName -notmatch '\\.git\\' }).Count
    if ($mdCount -gt 0) {
        Print-Ok "Found $mdCount markdown files"
    } else {
        Print-Warn 'No markdown files found'
    }
    if (Test-Path 'README.md' -PathType Leaf) {
        Check-FileNotEmpty 'README.md' 'Root README.md'
    }
}

function Test-Tier2Templates {
    Print-Section 'Tier 2 Templates (.aidlc-templates/)'
    if (-not (Test-Path '.aidlc-templates' -PathType Container)) {
        Print-Warn 'Tier 2 templates not installed (.aidlc-templates/ not found)'
        Print-Info 'Run deploy-to-repo.ps1 -Tier 2 from the ai-dlc repository to install Tier 2'
        return
    }

    Check-Dir '.aidlc-templates\architecture-patterns' 'Architecture patterns'
    Check-Dir '.aidlc-templates\audit-templates' 'Audit templates'
    Check-Dir '.aidlc-templates\pipeline-patterns' 'Pipeline patterns'
    Check-Dir '.aidlc-templates\compliance' 'Compliance templates' -Optional

    # Count template files
    $templateCount = (Get-ChildItem -Path '.aidlc-templates' -Recurse -File |
                      Where-Object { $_.Name -ne '.gitkeep' }).Count
    if ($templateCount -gt 50) {
        Print-Ok "Tier 2 templates installed ($templateCount files)"
    } elseif ($templateCount -gt 0) {
        Print-Warn "Tier 2 templates partially installed ($templateCount files, expected 50+)"
    } else {
        Print-Err 'Tier 2 templates directory exists but is empty'
    }
}

function Test-Scripts {
    Print-Section 'Setup and Deployment Scripts'

    # Detect if we're in the ai-dlc source repo or a target repo
    # Logic: deploy-to-repo.ps1 only exists in ai-dlc source repo (never copied to targets)
    $isAiDlcSourceRepo = Test-Path 'scripts\deploy-to-repo.ps1'

    if ($isAiDlcSourceRepo) {
        Write-Verbose "  Detected ai-dlc source repo - checking all scripts"

        # Setup scripts (ai-dlc repo only - archived/deprecated)
        Check-File 'scripts\setup-tier1.ps1'       'Tier 1 setup script' -Optional
        Check-File 'scripts\setup-tier2.ps1'       'Tier 2 setup script' -Optional
        Check-File 'scripts\setup-tier3.ps1'       'Tier 3 setup script' -Optional
        Check-File 'scripts\setup-all.ps1'         'Complete setup script' -Optional

        # Deployment scripts (ai-dlc repo only)
        Check-File 'scripts\deploy-to-repo.ps1'    'Framework deployment'

        # Sync scripts (ai-dlc repo only)
        Check-File 'scripts\download-ai-dlc.ps1'   'AWS AI-DLC download script' -Optional
        Check-File 'scripts\sync-ai-dlc.ps1'       'AWS AI-DLC sync script' -Optional
        Check-File 'scripts\sync-denver.ps1'       'Denver sync script' -Optional

        # Utility scripts (should be in ai-dlc AND get copied to target repos)
        Check-File 'scripts\deploy-agent-config.ps1' 'Agent config deployment'
        Check-File 'scripts\initialize-feature.ps1' 'Feature initialization'
        Check-File 'scripts\upgrade-framework.ps1' 'Framework upgrade'
        Check-File 'scripts\validate-structure.ps1' 'Structure validation script'
        Check-File 'scripts\remove-framework.ps1'  'Framework removal' -Optional
    } else {
        Write-Verbose "  Detected target repo - checking utility scripts only"

        # Utility scripts (copied to target repo during deployment)
        Check-File 'scripts\deploy-agent-config.ps1' 'Agent config deployment'
        Check-File 'scripts\initialize-feature.ps1' 'Feature initialization'
        Check-File 'scripts\upgrade-framework.ps1' 'Framework upgrade'
        Check-File 'scripts\validate-structure.ps1' 'Structure validation script'
        Check-File 'scripts\remove-framework.ps1'  'Framework removal' -Optional

        Print-Info 'Deployment scripts (deploy-to-repo.ps1, sync-*.ps1) remain in ai-dlc repo'
    }
}

# ============================================================================
# Main
# ============================================================================

Print-Header 'AI-SDLC Framework Structure Validator'

Test-CoreDirectories
Test-RuleDetailDirectories
Test-AiSubdirectories
Test-GithubSubdirectories
Test-ConfigFiles
Test-Tier2Templates
Test-AgentFiles
Test-ContentIntegrity
Test-MarkdownFiles
Test-Scripts

# ============================================================================
# Summary
# ============================================================================

Write-Host ''
Print-Header 'Validation Summary'
Write-Host ''

if ($FailedChecks -eq 0) {
    Print-Ok 'All validation checks passed!'
    Write-Host ''
    Print-Info 'Framework structure is intact and ready for use.'
    exit 0
} else {
    Print-Err "$FailedChecks check(s) failed — review the output above."
    Write-Host ''
    Print-Info 'Common fixes:'
    Write-Host '  - Missing rule files: run .\scripts\download-ai-dlc.ps1 in the ai-dlc repo'
    Write-Host '  - Missing tier files: run the appropriate setup-tierN.ps1 script'
    exit 1
}
