<#
.SYNOPSIS
    Deploys a specific AI agent configuration to the current repository.

.DESCRIPTION
    Copies an agent configuration template from the ai-dlc framework to the appropriate
    location in your target repository. Supports all major AI agents:
    - GitHub Copilot (.github/copilot-instructions.md)
    - Amazon Q Developer (.amazonq/rules/ai-sdlc.md)
    - Kiro (.kiro/steering/ai-sdlc.md)
    - Claude Code (CLAUDE.md + AGENTS.md)
    - ChatGPT (.chatgpt/instructions.md)
    - Cursor (.cursor/rules.md) [legacy]

    Run from the ROOT of your target repository:
        & "C:\source\insurer\AI\ai-dlc\scripts\deploy-agent-config.ps1" -Agent copilot

.PARAMETER Agent
    The agent to deploy configuration for. Valid values:
    - copilot (GitHub Copilot)
    - amazonq (Amazon Q Developer)
    - kiro (Kiro)
    - claude (Claude Code)
    - chatgpt (ChatGPT)
    - cursor (Cursor IDE)
    - all (Deploy all agents)

.PARAMETER Force
    Overwrite existing configuration files without prompting.

.PARAMETER Verbose
    Enable verbose output for debugging.

.EXAMPLE
    .\scripts\deploy-agent-config.ps1 -Agent copilot
    .\scripts\deploy-agent-config.ps1 -Agent all
    .\scripts\deploy-agent-config.ps1 -Agent claude -Force
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [ValidateSet('copilot', 'amazonq', 'kiro', 'claude', 'chatgpt', 'cursor', 'all')]
    [string]$Agent,

    [switch]$Force
)

$ErrorActionPreference = 'Stop'

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$AiDlcRoot = Split-Path -Parent $ScriptDir
$AgentConfigsDir = Join-Path $AiDlcRoot 'guides\setup\agent-configs'

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

function Confirm-Continue($prompt) {
    if ($Force) { return $true }
    $response = Read-Host "$prompt (y/N)"
    return $response -match '^[Yy]$'
}

function Extract-InstructionContent($templatePath) {
    # Read template file and extract content between "## Instruction file content" markers
    $content = Get-Content $templatePath -Raw

    # Find the instruction content section - use greedy match to capture inner code blocks too
    if ($content -match '(?s)## Instruction file content\s+```markdown\s+(.*)\s+```\s*\r?\n\s*\r?\n---') {
        return $Matches[1]
    }

    Print-Err "Could not extract instruction content from template: $templatePath"
    exit 1
}

function Deploy-AgentConfig($agentName, $templateFile, $targetPath, $description) {
    Print-Info "Deploying $description..."

    $templateFullPath = Join-Path $AgentConfigsDir $templateFile
    if (-not (Test-Path $templateFullPath)) {
        Print-Err "Template not found: $templateFullPath"
        return $false
    }

    # Check if target already exists
    if (Test-Path $targetPath) {
        Print-Warn "Configuration already exists: $targetPath"
        if (-not (Confirm-Continue "Overwrite existing file?")) {
            Print-Info "Skipped $description"
            return $false
        }
    }

    # Extract content from template
    $content = Extract-InstructionContent $templateFullPath

    # Create target directory if needed
    $targetDir = Split-Path -Parent $targetPath
    if (-not (Test-Path $targetDir)) {
        New-Item -ItemType Directory -Path $targetDir -Force | Out-Null
    }

    # Write to target
    Set-Content -Path $targetPath -Value $content -Encoding UTF8
    Print-Ok "$description deployed to $targetPath"
    return $true
}

# ============================================================================
# Agent Deployment Functions
# ============================================================================

function Deploy-Copilot {
    Deploy-AgentConfig `
        'copilot' `
        'copilot.md' `
        '.github\copilot-instructions.md' `
        'GitHub Copilot configuration'
}

function Deploy-AmazonQ {
    Deploy-AgentConfig `
        'amazonq' `
        'amazonq.md' `
        '.amazonq\rules\ai-sdlc.md' `
        'Amazon Q Developer configuration'
}

function Deploy-Kiro {
    Deploy-AgentConfig `
        'kiro' `
        'kiro.md' `
        '.kiro\steering\ai-sdlc.md' `
        'Kiro configuration'
}

function Deploy-Claude {
    Print-Info "Deploying Claude Code configuration..."

    # Claude uses two files: CLAUDE.md and AGENTS.md
    $claudeTemplate = Join-Path $AgentConfigsDir 'claude.md'
    if (-not (Test-Path $claudeTemplate)) {
        Print-Err "Claude template not found: $claudeTemplate"
        return $false
    }

    $templateContent = Get-Content $claudeTemplate -Raw

    # Extract CLAUDE.md content
    if ($templateContent -match '(?s)## File 1: CLAUDE\.md.*?```markdown\s+(.*?)\s+```') {
        $claudeContent = $Matches[1]
    } else {
        Print-Err "Could not extract CLAUDE.md content"
        return $false
    }

    # Extract AGENTS.md content
    if ($templateContent -match '(?s)## File 2: AGENTS\.md.*?```markdown\s+(.*?)\s+```') {
        $agentsContent = $Matches[1]
    } else {
        Print-Err "Could not extract AGENTS.md content"
        return $false
    }

    # Deploy CLAUDE.md
    if ((Test-Path 'CLAUDE.md') -and -not $Force) {
        Print-Warn "CLAUDE.md already exists"
        if (-not (Confirm-Continue "Overwrite CLAUDE.md?")) {
            Print-Info "Skipped CLAUDE.md"
        } else {
            Set-Content -Path 'CLAUDE.md' -Value $claudeContent -Encoding UTF8
            Print-Ok "CLAUDE.md deployed"
        }
    } else {
        Set-Content -Path 'CLAUDE.md' -Value $claudeContent -Encoding UTF8
        Print-Ok "CLAUDE.md deployed"
    }

    # Deploy AGENTS.md
    if ((Test-Path 'AGENTS.md') -and -not $Force) {
        Print-Warn "AGENTS.md already exists"
        if (-not (Confirm-Continue "Overwrite AGENTS.md?")) {
            Print-Info "Skipped AGENTS.md"
        } else {
            Set-Content -Path 'AGENTS.md' -Value $agentsContent -Encoding UTF8
            Print-Ok "AGENTS.md deployed"
        }
    } else {
        Set-Content -Path 'AGENTS.md' -Value $agentsContent -Encoding UTF8
        Print-Ok "AGENTS.md deployed"
    }

    return $true
}

function Deploy-ChatGPT {
    Deploy-AgentConfig `
        'chatgpt' `
        'chatgpt.md' `
        '.chatgpt\instructions.md' `
        'ChatGPT configuration'
}

function Deploy-Cursor {
    Print-Warn "Cursor support is legacy — consider using Copilot, Kiro, or Claude instead"
    Deploy-AgentConfig `
        'cursor' `
        'cursor.md' `
        '.cursor\rules.md' `
        'Cursor configuration (legacy)'
}

# ============================================================================
# Main
# ============================================================================

Print-Header 'AI-SDLC Agent Configuration Deployment'
Write-Host ''

# Check we're in a git repo
if (-not (Test-Path '.git')) {
    Print-Err 'Not a git repository. Run from the root of your target repo.'
    exit 1
}

$success = $false

if ($Agent -eq 'all') {
    Print-Info 'Deploying all agent configurations...'
    Write-Host ''

    Deploy-Copilot
    Write-Host ''
    Deploy-AmazonQ
    Write-Host ''
    Deploy-Kiro
    Write-Host ''
    Deploy-Claude
    Write-Host ''
    Deploy-ChatGPT
    Write-Host ''

    $success = $true
} else {
    switch ($Agent) {
        'copilot' { $success = Deploy-Copilot }
        'amazonq' { $success = Deploy-AmazonQ }
        'kiro'    { $success = Deploy-Kiro }
        'claude'  { $success = Deploy-Claude }
        'chatgpt' { $success = Deploy-ChatGPT }
        'cursor'  { $success = Deploy-Cursor }
    }
}

Write-Host ''
Print-Header 'Deployment Complete'
Write-Host ''

if ($success) {
    Print-Info 'Next steps:'
    Write-Host '  1. Review and customise the deployed configuration file(s)'
    Write-Host '  2. Update project-specific sections (name, stack, coding standards)'
    Write-Host '  3. Stage and commit:'
    Write-Host '       git add .'
    Write-Host "       git commit -m 'chore: add AI agent configuration'"
    Write-Host ''
    Print-Info "Configuration guide: $AgentConfigsDir\README.md"
} else {
    Print-Warn 'Some configurations were not deployed'
}
