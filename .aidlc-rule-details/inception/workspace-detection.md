# Workspace Detection

**Purpose**: Determine workspace state and check for existing AI-DLC projects

## Step 1: Check for Existing AI-DLC Project

Check if `aidlc-docs/aidlc-state.md` exists:
- **If exists**: Resume from last phase (load context from previous phases)
- **If not exists**: Continue with new project assessment

## Step 2: Scan Workspace for Existing Code

**Determine if workspace has existing code:**
- Scan workspace for source code files (.java, .py, .js, .ts, .jsx, .tsx, .kt, .kts, .scala, .groovy, .go, .rs, .rb, .php, .c, .h, .cpp, .hpp, .cc, .cs, .fs, etc.)
- Check for build files (pom.xml, package.json, build.gradle, etc.)
- Look for project structure indicators
- Identify workspace root directory (NOT aidlc-docs/)

**Record findings:**
```markdown
## Workspace State
- **Existing Code**: [Yes/No]
- **Programming Languages**: [List if found]
- **Build System**: [Maven/Gradle/npm/etc. if found]
- **Project Structure**: [Monolith/Microservices/Library/Empty]
- **Workspace Root**: [Absolute path]
```

## Step 3: Determine Next Phase

**IF workspace is empty (no existing code)**:
- Set flag: `brownfield = false`
- Next phase: Requirements Analysis

**IF workspace has existing code**:
- Set flag: `brownfield = true`
- Check for existing reverse engineering artifacts in `aidlc-docs/inception/reverse-engineering/`
- **IF reverse engineering artifacts exist**:
    - Check if artifacts are stale (compare artifact timestamps against codebase's last significant modification)
    - **IF artifacts are current**: Load them, skip to Requirements Analysis
    - **IF artifacts are stale**: Next phase is Reverse Engineering (rerun to refresh artifacts)
    - **IF user explicitly requests rerun**: Next phase is Reverse Engineering regardless of staleness
- **IF no reverse engineering artifacts**: Next phase is Reverse Engineering

## Step 3a: Verify .ai/memory/ Against Actual Codebase (Brownfield Only)

**Execute when**: `brownfield = true` AND `.ai/memory/` files already exist (skipped for greenfield or first-run brownfield with no memory files).

Before generating any spec artefacts, verify that the key architectural claims in `.ai/memory/` reflect what the actual codebase contains. Memory files are only useful if they are correct.

**Verification checklist**:
```
1. Read .ai/memory/architecture.md
2. Identify claims about how layers communicate, e.g.:
   - "Portal calls WCF/SAM"
   - "Portal calls REST API in repo X"
   - "b* libraries are called directly from Portal"
   - "REST API is external / hosted separately"
3. For each claim, find an actual source file that confirms or contradicts it:
   - Portal claim → open an .aspx.vb code-behind file and check what it imports/calls
   - REST API claim → check if the stated repo/folder exists and contains an API project
   - Business logic claim → open a b* or service class and check its callers
4. If any claim is WRONG or INCOMPLETE:
   a. Inspect actual code to determine the correct pattern
   b. Update .ai/memory/architecture.md (and api-documentation.md if applicable)
   c. Append a correction entry to audit.md:
      "[timestamp] — INCEPTION — [agent] — Architecture Memory Correction
       Claim: [what was wrong]
       Verified against: [file(s) inspected]
       Correction: [what the codebase actually shows]
       Files updated: [which .ai/memory/ files were changed]"
5. Only proceed once .ai/memory/ reflects actual codebase
```

**Note**: This step protects against the most common failure mode — building requirements, design, and tasks against an incorrect architectural assumption. It takes 5–10 minutes and saves hours of rework.

## Step 4: Create Initial State File

Create `aidlc-docs/aidlc-state.md`:

```markdown
# AI-DLC State Tracking

## Project Information
- **Project Type**: [Greenfield/Brownfield]
- **Start Date**: [ISO timestamp]
- **Current Stage**: INCEPTION - Workspace Detection

## Workspace State
- **Existing Code**: [Yes/No]
- **Reverse Engineering Needed**: [Yes/No]
- **Workspace Root**: [Absolute path]

## Code Location Rules
- **Application Code**: Workspace root (NEVER in aidlc-docs/)
- **Documentation**: aidlc-docs/ only
- **Structure patterns**: See code-generation.md Critical Rules

## Stage Progress
[Will be populated as workflow progresses]
```

## Step 5: Present Completion Message

**For Brownfield Projects:**
```markdown
# 🔍 Workspace Detection Complete

Workspace analysis findings:
• **Project Type**: Brownfield project
• [AI-generated summary of workspace findings in bullet points]
• **Next Step**: Proceeding to **Reverse Engineering** to analyze existing codebase...
```

**For Greenfield Projects:**
```markdown
# 🔍 Workspace Detection Complete

Workspace analysis findings:
• **Project Type**: Greenfield project
• **Next Step**: Proceeding to **Requirements Analysis**...
```

## Step 6: Automatically Proceed

- **No user approval required** - this is informational only
- Automatically proceed to next phase:
  - **Brownfield**: Reverse Engineering (if no existing artifacts) or Requirements Analysis (if artifacts exist)
  - **Greenfield**: Requirements Analysis
