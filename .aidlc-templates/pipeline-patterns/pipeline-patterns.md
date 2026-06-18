# CI/CD: Pipeline Patterns

## Overview

Every architecture template (AWS, Azure, Hybrid) follows the same two-phase pattern: **CI builds and validates**, **GitOps deploys**. The CI engine varies by Git platform, but the pipeline stages and GitOps deployment are identical.

## The Two-Phase Pattern

```
Phase 1: CI (runs in Git platform)          Phase 2: CD (runs in cluster)
┌─────────────────────────────────┐         ┌──────────────────────────┐
│  Push / PR                      │         │  ArgoCD                  │
│  ├─ Lint & static analysis      │         │  ├─ Watches manifest repo│
│  ├─ Unit tests                  │         │  ├─ Detects image tag    │
│  ├─ Build container image       │         │  │  change               │
│  ├─ Security scan (image + deps)│         │  ├─ Syncs to target      │
│  ├─ Push image to registry      │  ────►  │  │  environment          │
│  └─ Update manifest repo        │         │  └─ Health checks &      │
│     (new image tag)             │         │     rollback if needed   │
└─────────────────────────────────┘         └──────────────────────────┘
```

**Why two phases?** CI is push-based (Git platform pushes builds). CD is pull-based (ArgoCD pulls desired state from Git). This separation means the CI system never needs cluster credentials — ArgoCD handles deployment from inside the cluster.

## CI Pipeline Stages

These stages are the same regardless of Git platform. Only the YAML syntax differs.

### On Pull Request

| Stage | What It Does |
|---|---|
| Lint | Code formatting, linting rules (ESLint, Prettier, golangci-lint, etc.) |
| Static Analysis | Type checking, security linting (Semgrep, SonarQube) |
| Unit Tests | Run unit test suite, fail PR if tests fail |
| Build | Compile / build application (verify it builds cleanly) |
| Image Build | Build container image (but don't push — just verify Dockerfile works) |
| Image Scan | Scan image for CVEs (Trivy, Grype, Snyk) |

PR pipelines are fast feedback — they validate but don't deploy.

### On Merge to Main

| Stage | What It Does |
|---|---|
| All PR stages | Re-run everything (lint, test, build, scan) |
| Push Image | Tag image with git SHA, push to container registry |
| Update Manifests | Commit new image tag to the GitOps manifest repo (dev environment) |

Merge to main automatically deploys to dev. Promotion to test/prod is a separate, gated step.

### On Release Tag

| Stage | What It Does |
|---|---|
| Promote Image | Re-tag the existing image with the release version (no rebuild) |
| Update Manifests | Commit new image tag to the GitOps manifest repo (prod environment) |

Release tags promote a known-good image — they never rebuild from source.

## GitOps with ArgoCD

ArgoCD is the deployment engine across all three architecture templates. It runs inside each Kubernetes cluster and continuously reconciles the cluster state with the desired state defined in Git.

**Why ArgoCD over FluxCD?** Both are CNCF projects. ArgoCD has a richer UI for visualizing deployments, better multi-cluster support, and broader adoption. FluxCD is lighter and more composable. Either works — ArgoCD is the default recommendation for teams that value visibility.

### Manifest Repository Structure

Separate repo from application code. Contains Helm values or Kustomize overlays per environment:

```
gitops-manifests/
├── apps/
│   ├── service-a/
│   │   ├── base/                  ← shared Helm values or Kustomize base
│   │   ├── overlays/
│   │   │   ├── dev/               ← dev-specific values (image tag, replicas, config)
│   │   │   ├── test/              ← test-specific values
│   │   │   └── prod/              ← prod-specific values
│   ├── service-b/
│   │   ├── base/
│   │   ├── overlays/
│   │   │   ├── dev/
│   │   │   ├── test/
│   │   │   └── prod/
├── platform/
│   ├── istio/                     ← Istio configuration per environment
│   ├── temporal/                  ← Temporal server config
│   ├── monitoring/                ← Prometheus, Grafana, Loki
│   └── keycloak/                  ← Identity provider config
```

### Environment Promotion

```
dev  ──(auto on merge)──►  test  ──(manual approval)──►  prod
```

- **Dev:** Auto-deployed on every merge to main. Unstable. Used for integration testing.
- **Test:** Promoted manually or via scheduled pipeline. Stable enough for QA and stakeholder review.
- **Prod:** Promoted via release tag with manual approval gate. ArgoCD syncs only after approval.

ArgoCD Application Sets can automate the creation of ArgoCD Applications per environment, reducing boilerplate.

## CI Engine Mapping by Git Platform

The pipeline stages above map to each Git platform's CI engine:

| Concept | GitHub Actions | Azure Pipelines | Bitbucket Pipelines |
|---|---|---|---|
| Pipeline definition | `.github/workflows/*.yml` | `azure-pipelines.yml` | `bitbucket-pipelines.yml` |
| Trigger on PR | `on: pull_request` | `pr:` trigger | `pull-requests:` |
| Trigger on merge | `on: push: branches: [main]` | `trigger: branches: [main]` | `branches: main:` |
| Secrets | GitHub Secrets / OIDC | Variable Groups / Key Vault | Repository Variables / Pipes |
| Container registry auth | OIDC to ECR/ACR/registry | Service Connection | Pipes or manual auth |
| Reusable components | Reusable workflows, composite actions | Templates, task groups | Pipes |
| Approval gates | Environment protection rules | Stage approvals | Deployment permissions |
| Self-hosted runners | Self-hosted runners | Self-hosted agents | Runners (limited) |

## Container Registry by Architecture

| Architecture | Primary Registry | Notes |
|---|---|---|
| AWS | Amazon ECR | Private repos, image scanning, lifecycle policies. OIDC auth from GitHub Actions via IRSA. |
| Azure | Azure Container Registry (ACR) | Geo-replication, Tasks for in-registry builds, integrated with AKS. |
| Hybrid | Any OCI-compliant registry | Harbor (self-hosted, open source) is the portable default. Or use cloud registry if available. |
