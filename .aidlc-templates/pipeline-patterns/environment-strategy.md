# CI/CD: Environment Strategy

## Overview

Every application needs dev, test, and prod environments. The strategy for how these environments are provisioned differs by architecture template, but the GitOps deployment pattern is identical across all three.

## Environment Isolation: Namespaces vs. Clusters

Two approaches, chosen based on the company's risk tolerance and scale:

### Namespace-per-Environment (Shared Cluster)

Dev, test, and prod run as separate namespaces in the same Kubernetes cluster. Isolation enforced via RBAC, network policies, resource quotas, and Istio authorization policies.

```
┌─────────────────────────────────────┐
│          Kubernetes Cluster          │
│  ┌─────────┐ ┌─────────┐ ┌───────┐ │
│  │ ns: dev │ │ ns: test│ │ns:prod│ │
│  └─────────┘ └─────────┘ └───────┘ │
└─────────────────────────────────────┘
```

- Pros: Lower cost (one cluster). Simpler to manage. Faster to provision new environments. Good enough for most small-to-mid-size applications.
- Cons: Noisy neighbor risk — a runaway dev workload can starve prod. Blast radius is larger — a cluster-level misconfiguration affects all environments. Some compliance frameworks require physical separation for prod.
- Best for: Early-stage projects, cost-sensitive deployments, teams with fewer than ~5 services.

### Cluster-per-Environment (Separate Clusters)

Dev, test, and prod each get their own cluster. ArgoCD manages all clusters from a single management cluster (or from the prod cluster).

```
┌──────────┐  ┌──────────┐  ┌──────────┐
│ Cluster: │  │ Cluster: │  │ Cluster: │
│   dev    │  │   test   │  │   prod   │
└──────────┘  └──────────┘  └──────────┘
       ▲            ▲             ▲
       └────────────┼─────────────┘
                    │
            ┌───────┴────────┐
            │ ArgoCD manages │
            │  all clusters  │
            └────────────────┘
```

- Pros: Full blast radius isolation. Independent scaling and upgrade schedules. Meets compliance requirements for environment separation. Prod cluster can have different node types, security policies, and network rules.
- Cons: Higher cost (3x clusters). More operational overhead. Cross-cluster networking adds complexity.
- Best for: Production-critical applications, regulated industries, teams with 10+ services, organizations where a dev incident cannot be allowed to impact prod.

### Hybrid Approach (Recommended Default)

Shared cluster for dev + test, separate cluster for prod. Balances cost with production isolation.

```
┌─────────────────────────────┐  ┌──────────┐
│     Non-Prod Cluster        │  │  Prod    │
│  ┌─────────┐ ┌─────────┐   │  │ Cluster  │
│  │ ns: dev │ │ ns: test│   │  │          │
│  └─────────┘ └─────────┘   │  │          │
└─────────────────────────────┘  └──────────┘
```

## Environment Strategy by Architecture Template

### AWS (EKS)

| Component | Dev | Test | Prod |
|---|---|---|---|
| Kubernetes | EKS cluster (shared with test) | Same cluster, separate namespace | Dedicated EKS cluster |
| Database | Aurora PostgreSQL (small instance, shared) | Same instance, separate database | Aurora PostgreSQL (Multi-AZ, dedicated) |
| Object Storage | S3 bucket with `/dev` prefix | S3 bucket with `/test` prefix | Dedicated S3 bucket |
| Container Registry | ECR (shared, images tagged by env) | Same ECR | Same ECR |
| DNS | `dev.app.internal` | `test.app.internal` | `app.company.com` |
| WAF | Disabled or permissive | Enabled, test rules | Full OWASP ruleset |
| Secrets | Secrets Manager (dev namespace) | Secrets Manager (test namespace) | Secrets Manager (prod, separate KMS key) |

**Infrastructure as Code:** Terraform with workspaces or separate state files per environment. GitHub Actions triggers `terraform plan` on PR, `terraform apply` on merge. EKS clusters provisioned via Terraform EKS module.

**CI/CD Flow:**

1. GitHub Actions builds and pushes image to ECR
2. GitHub Actions updates GitOps manifest repo (dev overlay)
3. ArgoCD on non-prod cluster syncs dev namespace
4. Manual promotion: update test overlay → ArgoCD syncs test namespace
5. Release tag: update prod overlay → ArgoCD on prod cluster syncs (with approval gate)

### Azure (AKS)

| Component | Dev | Test | Prod |
|---|---|---|---|
| Kubernetes | AKS cluster (shared with test) | Same cluster, separate namespace | Dedicated AKS cluster |
| Database | Azure DB for PostgreSQL Flexible (burstable, shared) | Same instance, separate database | Flexible Server (HA, dedicated) |
| Object Storage | Blob Storage container (dev) | Blob Storage container (test) | Dedicated storage account |
| Container Registry | ACR (shared, images tagged by env) | Same ACR | Same ACR (geo-replicated) |
| DNS | `dev.app.internal` | `test.app.internal` | `app.company.com` |
| WAF | Application Gateway (no WAF policy) | WAF in detection mode | WAF in prevention mode |
| Secrets | Key Vault (dev) | Key Vault (test) | Key Vault (prod, separate access policies) |

**Infrastructure as Code:** Terraform or Bicep. Azure Pipelines triggers plan on PR, apply on merge. AKS clusters provisioned via Terraform AzureRM provider or Bicep modules.

**CI/CD Flow:**

1. Azure Pipelines builds and pushes image to ACR
2. Azure Pipelines updates GitOps manifest repo (dev overlay)
3. ArgoCD on non-prod cluster syncs dev namespace
4. Manual promotion: update test overlay → ArgoCD syncs test namespace
5. Release tag: update prod overlay → ArgoCD on prod cluster syncs (with stage approval)

### Hybrid / Portable

| Component | Dev | Test | Prod |
|---|---|---|---|
| Kubernetes | Cluster (shared with test) | Same cluster, separate namespace | Dedicated cluster |
| Database | CloudNativePG (single replica) | CloudNativePG (single replica, separate namespace) | CloudNativePG (HA, 2+ replicas) |
| Object Storage | MinIO (shared, `/dev` prefix) | MinIO (shared, `/test` prefix) | MinIO (dedicated, or cloud object store) |
| Container Registry | Harbor (self-hosted) | Same Harbor | Same Harbor |
| DNS | `dev.app.internal` | `test.app.internal` | `app.company.com` |
| WAF | Disabled | Coraza WAF in detection mode | Coraza WAF in prevention mode |
| Secrets | Sealed Secrets or Vault (dev path) | Vault (test path) | Vault (prod path, separate policy) |

**Infrastructure as Code:** Terraform for cluster provisioning (provider depends on target: AzureRM, OCI, vSphere, bare metal provisioner). Helm charts and Kustomize for in-cluster resources. GitHub Actions (or any CI) triggers the pipeline.

**CI/CD Flow:**

1. CI builds and pushes image to Harbor
2. CI updates GitOps manifest repo (dev overlay)
3. ArgoCD on non-prod cluster syncs dev namespace
4. Manual promotion: update test overlay → ArgoCD syncs test namespace
5. Release tag: update prod overlay → ArgoCD on prod cluster syncs (with approval gate)

## Common Across All Templates

### ArgoCD Configuration

ArgoCD is deployed in every cluster (or in a management cluster that targets others). Each environment is an ArgoCD Application pointing at the corresponding overlay in the manifest repo:

```yaml
# Example: ArgoCD Application for dev
apiVersion: argoproj.io/v1alpha1
kind: Application
metadata:
  name: service-a-dev
spec:
  source:
    repoURL: https://github.com/org/gitops-manifests
    path: apps/service-a/overlays/dev
  destination:
    server: https://kubernetes.default.svc
    namespace: dev
  syncPolicy:
    automated:
      prune: true
      selfHeal: true
```

### Environment Parity

All environments run the same container images, same Helm charts, same Istio configuration. The only differences between environments are:

- Image tags (which version is deployed)
- Replica counts (dev: 1, prod: 3+)
- Resource requests/limits (dev: small, prod: production-sized)
- Database connection strings
- External service endpoints
- Feature flags
- TLS certificates and DNS names

This is enforced by Kustomize overlays or Helm value files — the base configuration is shared, overlays contain only the deltas.

### Promotion as a Git Commit

Promoting from dev → test → prod is a Git commit that changes an image tag in the target environment's overlay. This means:

- Every promotion is auditable (Git history)
- Every promotion is reversible (Git revert)
- Every promotion is reviewable (PR to manifest repo)
- No one needs cluster credentials to deploy — only Git write access to the manifest repo
