# CI/CD: Git Platform Selection

## Overview

The Git platform is the foundation of the entire delivery pipeline. It hosts source code, triggers CI pipelines, stores container image references, and (via GitOps) defines the desired state of every environment. Choose based on the company's existing tooling, cloud alignment, and team familiarity.

## GitHub

The default choice for most teams and the broadest ecosystem.

**CI/CD Engine:** GitHub Actions — workflow files in `.github/workflows/`. Marketplace has thousands of pre-built actions. Matrix builds, reusable workflows, and environment protection rules built in.

- Pros: Largest developer community and ecosystem. GitHub Actions is tightly integrated — no separate CI system to manage. Best marketplace of reusable actions. GitHub Packages for container registry. Copilot integration. Free tier is generous for public repos. GitHub Advanced Security (GHAS) for code scanning, secret detection, dependency review.
- Cons: GitHub Actions runners are cloud-hosted by default — self-hosted runners needed for private network access to cloud resources. Actions YAML can become complex at scale. GitHub is owned by Microsoft, which may matter for some organizations.
- Best for: Most teams. Especially those on AWS (no native Git platform in AWS — GitHub is the de facto standard). Open-source-heavy organizations. Teams that want the largest ecosystem of integrations.

## Azure DevOps

Microsoft's integrated ALM platform. Includes Repos (Git), Pipelines (CI/CD), Boards (work tracking), Artifacts (packages), and Test Plans. Note: Microsoft has adopted a GitHub-first strategy — GitHub is now Microsoft's primary investment for developer tooling. Azure DevOps remains supported and widely used in enterprise, but new features and innovation are landing in GitHub first.

- Pros: Deepest Azure integration — service connections, managed identity, Azure Resource Manager tasks. Pipelines support YAML and classic (visual) editor. Built-in work item tracking and sprint planning. Azure Artifacts for packages and container images. Strong enterprise governance — granular permissions, audit logs, compliance. Supports both cloud-hosted and self-hosted agents.
- Cons: Microsoft's investment is shifting to GitHub — Azure DevOps is in maintenance mode for new capabilities. Smaller community than GitHub. Marketplace is less rich. UI feels dated compared to GitHub. Pipeline YAML syntax differs from GitHub Actions (not transferable). Less adoption outside enterprise Microsoft shops.
- Best for: Companies already deeply invested in Azure DevOps with existing pipelines and boards. Teams migrating should consider GitHub as the forward-looking choice, even on Azure.

## Bitbucket

Atlassian's Git platform. Integrates with Jira, Confluence, and the Atlassian ecosystem.

- Pros: Native Jira integration — branch names, commits, and PRs link to Jira issues automatically. Bitbucket Pipelines for CI/CD (built-in, YAML-based). Free private repos for small teams. Atlassian ecosystem is deeply embedded in many enterprises.
- Cons: Smallest ecosystem of the three. Bitbucket Pipelines is less powerful than GitHub Actions or Azure Pipelines. Atlassian Cloud has had reliability issues. Bitbucket Server (self-hosted) is being sunset — Atlassian is pushing cloud-only. Fewer third-party integrations. Community and documentation are thinner.
- Best for: Teams already deep in the Atlassian ecosystem (Jira + Confluence). Organizations that need Jira-linked traceability for compliance. Not recommended as a new adoption — GitHub or Azure DevOps are stronger choices.

## Selection Guide

| Scenario | Recommended Platform |
|---|---|
| AWS-native architecture | GitHub |
| Azure-native architecture | GitHub |
| Hybrid / portable architecture | GitHub |
| Company already uses Jira + Atlassian heavily | Bitbucket (or GitHub with Jira integration) |
| Company deeply invested in Azure DevOps already | Azure DevOps (but plan migration to GitHub) |
| Open-source or developer-first culture | GitHub |
| Need integrated project management | GitHub Projects (or Azure DevOps Boards if already using) |

## Platform-Agnostic Principle

Regardless of Git platform, the CI/CD pattern is the same:

1. **Git platform** triggers CI on push/PR (build, test, scan, push container image)
2. **CI updates a GitOps manifest repo** (image tag in Helm values or Kustomize overlay)
3. **ArgoCD** (running in the cluster) detects the manifest change and syncs to the target environment

This means the Git platform is swappable — the CI pipeline syntax changes, but the GitOps deployment pattern stays identical. ArgoCD doesn't care whether the commit came from GitHub Actions, Azure Pipelines, or Bitbucket Pipelines.
