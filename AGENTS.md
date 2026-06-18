# AI Agents Context

This repository uses the **AI-SDLC framework** for structured, traceable software development.

## Framework Structure

### `.ai/` — Project Context (Read First)
Repository-specific knowledge that AI agents reference before any work:
- `memory/` — Architecture, conventions, decisions, dependencies, glossary
- `workflows/` — Feature development, bug fixing, code review, deployment
- `rules/` — Coding standards, security requirements, testing requirements
- `context/` — Business domain, user personas, integration points

**Critical files**:
- `.ai/memory/architecture.md` — System architecture and design patterns
- `.ai/memory/conventions.md` — Naming, file structure, error handling, logging
- `.ai/rules/coding-standards.md` — Code style and quality standards
- `.ai/rules/testing-requirements.md` — Test coverage and test patterns

### `.aidlc-rule-details/` — SDLC Workflow Engine
Phase-gated workflow rules for structured feature development:
- `common/` — Process overview, session continuity, content validation
- `inception/` — Requirements analysis, user stories, application design
- `construction/` — Functional design, NFR requirements, code generation, build & test
- `operations/` — Operations and deployment rules
- `extensions/` — Security baseline, compliance, property-based testing

### `.aidlc-templates/` — Reference Patterns
Generic templates and checklists (product-agnostic):
- `architecture-patterns/` — AWS, Azure, hybrid, React reference architectures
- `audit-templates/` — Security, infrastructure, team, hosting audits
- `pipeline-patterns/` — CI/CD templates
- `compliance/` — HIPAA, PCI-DSS, SOC 2 frameworks

### `.aidlc/specs/` — Specs Workspace
Feature specifications live here, one directory per feature:
- `{feature-name}/requirements.md` — What to build
- `{feature-name}/design.md` — How to build it
- `{feature-name}/tasks.md` — Work breakdown with dependencies
- `{feature-name}/aidlc-state.md` — Live execution state (Available/Claimed/Done)
- `{feature-name}/audit.md` — Append-only action log

### `.aidlc/config.json` — Path Configuration
