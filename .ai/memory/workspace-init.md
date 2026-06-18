# Workspace Initialization — Pure Insurance

> **This file is read automatically at the start of EVERY session.**
> It MUST be executed before any task work, QA review, or code changes begin.
> It is referenced by `copilot-instructions.md` in the Quick Memory Checklist.

**Last Updated**: 2025-07-18
**Owned By**: Pure Insurance Team

---

## Purpose

Pure Insurance spans **two repositories** that are cloned as sibling directories.
Not all workspaces have both repos open. This initialization ensures the AI agent
discovers all available codebases on disk, confirms with the user, and stores
the paths for the session so that all downstream operations (code search, API
inspection, Pre-PR Gate, QA reviews) work across the full system.

**IMPORTANT — Folder names are NOT reliable.** Users may clone repos into any
directory name (e.g., `pi-main`, `rest-api-dev`, `PureInsurance_backup`).
Detection MUST use **internal structure fingerprinting** and **git remote URL
matching** — never rely on the directory name alone.

---

## STEP 1: Fingerprint the Current Workspace

Do **NOT** check the folder name. Instead, identify the repo type by checking
for structural markers inside the workspace root. Run the checks below in order
and stop at the first match.

### Method A: Git Remote URL (most reliable)

```powershell
git -C "{workspace_root}" remote get-url origin
```

| Remote URL contains | Repo identified as |
|---------------------|--------------------|
| `_git/PureInsurance.REST` | **PureInsurance.REST** (REST API) |
| `_git/PureInsurance` (without `.REST`) | **PureInsurance** (Back Office / Portal) |

If the remote URL matches either pattern, the repo type is confirmed.
Proceed to STEP 2.

### Method B: Directory Structure Fingerprints (fallback)

If git remote is unavailable or does not match, scan the workspace root for
known marker directories and files.

**PureInsurance (Back Office / Portal / DB)** — match if **2 or more** exist:

| Marker | Type | What it indicates |
|--------|------|-------------------|
| `Navigator XM Roadmaps/` | Directory | Navigator XM workflow XML files |
| `Sirius Architecture/` | Directory | Back Office architecture components |
| `Web Portal/` | Directory | ASP.NET Web Forms Portal (Nexus) |
| `Databases/` | Directory | Stored procedures and migrations |
| `Orion/Components/` | Directory | VB.NET business components (b*/i* projects) |
| `DME/` | Directory | Document Management Engine |
| `.aidlc/config.json` | File | AIDLC spec configuration |

**PureInsurance.REST (REST API microservices)** — match if **2 or more** exist:

| Marker | Type | What it indicates |
|--------|------|-------------------|
| `MicroServices/` | Directory | CQRS microservice projects |
| `ApiGateway/` | Directory | API Gateway routing + auth |
| `PureInsurance.REST.Common/` | Directory | Shared REST common library |
| `PureInsurance.REST.Common.Domain/` | Directory | Domain models |
| `PureInsurance.REST.Common.Repositories/` | Directory | Data access layer |
| `IdentityServer/` | Directory | Authentication server |

Run the fingerprint check:
```powershell
# Back Office fingerprint (check any 2)
$boMarkers = @("Navigator XM Roadmaps","Sirius Architecture","Web Portal","Databases","Orion\Components","DME",".aidlc\config.json")
$boHits = ($boMarkers | Where-Object { Test-Path (Join-Path "{workspace_root}" $_) }).Count

# REST API fingerprint (check any 2)
$restMarkers = @("MicroServices","ApiGateway","PureInsurance.REST.Common","PureInsurance.REST.Common.Domain","PureInsurance.REST.Common.Repositories","IdentityServer")
$restHits = ($restMarkers | Where-Object { Test-Path (Join-Path "{workspace_root}" $_) }).Count
```

| Result | Repo identified as |
|--------|--------------------|
| `$boHits >= 2` | **PureInsurance** (Back Office / Portal) |
| `$restHits >= 2` | **PureInsurance.REST** (REST API) |
| Neither reaches 2 | Not a Pure Insurance workspace — skip all remaining steps |

### Method C: Solution File Names (last resort)

If Methods A and B both fail, scan for `.sln` files whose names contain known
patterns:

```powershell
Get-ChildItem "{workspace_root}" -Recurse -Filter "*.sln" -Depth 2 | Select-Object -First 20 -ExpandProperty Name
```

| Pattern in `.sln` name | Repo identified as |
|------------------------|--------------------|
| Starts with `b` or `i` + uppercase (e.g., `bACT*.sln`, `iPM*.sln`) | **PureInsurance** |
| Contains `PureInsurance.REST` | **PureInsurance.REST** |

### Identification Result

Store the result:
```
DETECTED_REPO_TYPE = PureInsurance | PureInsurance.REST | Unknown
DETECTED_VIA       = RemoteURL | Fingerprint | SolutionName
```

If `Unknown`, inform the user:
> *"I could not identify this workspace as a Pure Insurance repository.
> Workspace initialization is skipped. If this is a Pure Insurance repo
> cloned under a custom name, please confirm which repo it is."*

---

## STEP 2: Locate the Sibling Repository

The sibling repo is the **other** Pure Insurance repo. Based on the detected
repo type, determine what to look for:

| Current repo type | Sibling repo type | Sibling remote URL contains |
|-------------------|-------------------|-----------------------------|
| `PureInsurance` | `PureInsurance.REST` | `_git/PureInsurance.REST` |
| `PureInsurance.REST` | `PureInsurance` | `_git/PureInsurance` (without `.REST`) |

### Search Strategy — execute in order, stop at first match

#### Strategy 1: Scan sibling directories by git remote URL

List all directories in the same parent folder and check each one for a
matching git remote URL:

```powershell
$parentDir = Split-Path "{workspace_root}" -Parent
$siblings = Get-ChildItem $parentDir -Directory | Where-Object { $_.FullName -ne "{workspace_root}" }
foreach ($dir in $siblings) {
    $remote = git -C $dir.FullName remote get-url origin 2>$null
    if ($remote -match "{sibling_remote_pattern}") {
        # Found the sibling
        $siblingPath = $dir.FullName
        break
    }
}
```

Where `{sibling_remote_pattern}` is:
- `_git/PureInsurance\.REST` if looking for the REST repo
- `_git/PureInsurance$` (end of string, no `.REST`) if looking for Back Office

#### Strategy 2: Scan sibling directories by fingerprint

If Strategy 1 finds nothing (git not initialised, or sibling is on a different
drive), fall back to fingerprinting each sibling directory:

```powershell
foreach ($dir in $siblings) {
    # Use the same marker checks from STEP 1 Method B
    # but check for the SIBLING repo type markers
}
```

#### Strategy 3: Ask the user

If neither strategy finds the sibling:

> *"I identified this workspace as `{DETECTED_REPO_TYPE}` but could not
> find the sibling `{sibling_repo_type}` repository in the parent directory
> `{parentDir}`.*
>
> *Options:*
> *1. Provide the full path to the sibling repo*
> *2. Skip — cross-repo features will be limited*
>
> *Please enter the path or type 'skip':"*

If the user provides a path, validate it using the same fingerprint/remote
checks before accepting.

---

## STEP 3: Validate the Sibling Repository

Once a candidate sibling path is found, confirm it is the correct repo:

```powershell
# Verify the sibling exists and is a git repo
Test-Path (Join-Path "{sibling_path}" ".git")

# Double-check remote URL matches expected sibling type
git -C "{sibling_path}" remote get-url origin
```

If validation fails, treat as "sibling not found" and inform the user.

---

## STEP 4: Confirm with User

Ask the user:

> *"I identified this workspace as **{DETECTED_REPO_TYPE}** (detected via
> {DETECTED_VIA}).*
>
> *I also found the sibling **{sibling_repo_type}** repository at:*
> *`{sibling_path}`*
>
> *Should I include it in scope for this session?*
> *This enables:*
> - *Cross-repo code search*
> - *API inspection and Pre-PR Gate checks across both repos*
> - *QA reviews that read Controllers, Handlers, Validators from the REST repo*
> - *Branch alignment verification*
>
> *Include it? (Yes / No)"*

- **Yes** → Proceed to STEP 5.
- **No** → Warn the user that cross-repo features will be incomplete. Skip to STEP 6.

---

## STEP 5: Verify Sibling Repo Branch Alignment

Check the current branch of both repos:

```powershell
$currentBranch = git -C "{workspace_root}" branch --show-current
$siblingBranch = git -C "{sibling_path}" branch --show-current
```

Report both to the user:

> *"Current workspace ({DETECTED_REPO_TYPE}): branch `{currentBranch}`*
> *Sibling repo ({sibling_repo_type}): branch `{siblingBranch}`"*

If the user is working on a task, verify both repos are on the same
feature/task branch. If they are misaligned, warn:

> *"⚠️ Branch mismatch: `{DETECTED_REPO_TYPE}` is on `{currentBranch}` but
> `{sibling_repo_type}` is on `{siblingBranch}`. Changes may not be
> aligned. Consider switching the sibling repo to the same branch."*

---

## STEP 6: Record Session State

Store the following session state for use by all downstream operations:

```
SESSION_WORKSPACE_ROOT     = {current workspace path}
SESSION_WORKSPACE_REPO     = PureInsurance | PureInsurance.REST
SESSION_DETECTED_VIA       = RemoteURL | Fingerprint | SolutionName | UserProvided
SESSION_SIBLING_AVAILABLE  = true | false
SESSION_SIBLING_PATH       = {resolved sibling path} | null
SESSION_SIBLING_REPO       = PureInsurance | PureInsurance.REST | null
SESSION_SIBLING_INCLUDED   = true | false  (user confirmed)
SESSION_SIBLING_BRANCH     = {branch name} | null
SESSION_REST_REPO_PATH     = {path to whichever repo is the REST API} | null
SESSION_BACKOFFICE_REPO_PATH = {path to whichever repo is Back Office} | null
```

**Derived convenience values:**
- `SESSION_REST_REPO_PATH` = the workspace root if current is REST, else the sibling path if sibling is REST, else null
- `SESSION_BACKOFFICE_REPO_PATH` = the workspace root if current is Back Office, else the sibling path if sibling is Back Office, else null

### How Downstream Operations Use Session State

| Operation | Reads from |
|-----------|-----------|
| **Pre-PR API Gate** (Gate Step 1) | `SESSION_REST_REPO_PATH` — scans changed API files; `SESSION_BACKOFFICE_REPO_PATH` — scans changed Portal/DB files |
| **Phase 2 Step 6** (API Analysis) | `SESSION_REST_REPO_PATH` — reads Controllers, Handlers, Validators |
| **Code search** | If `SESSION_SIBLING_INCLUDED = true`, search both `SESSION_REST_REPO_PATH` and `SESSION_BACKOFFICE_REPO_PATH` |
| **File reads** | When reading `Controller.cs`, `QueryHandler.cs`, etc., resolve from `SESSION_REST_REPO_PATH` |
| **Branch alignment** | Compare `SESSION_SIBLING_BRANCH` with current branch before PR creation |
| **Architecture context** | Read `.ai/memory/` files from `SESSION_BACKOFFICE_REPO_PATH` (spec files always live in Back Office repo) |

---

## When to Re-run This Initialization

- **Every new session** — always run on session start
- **After cloning a sibling repo** — re-run to pick up the new repo
- **After switching branches** — re-run STEP 5 to verify alignment
- **If the user says "re-detect repos"** — re-run the full initialization
- **If the user changes workspace** — re-run from STEP 1
