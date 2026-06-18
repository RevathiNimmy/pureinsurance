# Architecture Template: Hybrid / Portable

## When to Use

The company needs to run on infrastructure they don't fully control, can't commit to a single cloud, or operates in environments like on-premises data centers, Oracle Cloud, Rancher-managed clusters, or mixed cloud estates. This template uses only CNCF-standard and open-source components so the entire stack is portable across any conformant Kubernetes distribution: AKS, EKS, GKE, OKE, RKE2, k3s, OpenShift, Tanzu, bare metal.

This is also the right starting point when the long-term cloud strategy is undecided — everything here can be migrated to the AWS or Azure template later by swapping portable components for managed equivalents.

## Architecture Overview

```
                         ┌──────────────┐
                         │ CDN          │  ← Cloudflare, Fastly, or cloud-native CDN
                         │ (external)   │     (environment-dependent)
                         └──────┬───────┘
                                │
                    ┌───────────▼────────────┐
                    │    Kubernetes Cluster   │  ← AKS, OKE, RKE2, k3s, bare metal
                    │  ┌──────────────────┐  │
                    │  │   Istio Mesh     │  │
                    │  │  ┌────────────┐  │  │
                    │  │  │ Istio      │  │  │  ← Istio Gateway replaces cloud LB
                    │  │  │ Gateway    │  │  │     TLS termination, routing, rate limiting
                    │  │  ├────────────┤  │  │
                    │  │  │ React UIs  │  │  │  ← nginx containers
                    │  │  │ (nginx)    │  │  │
                    │  │  ├────────────┤  │  │
                    │  │  │ API Svcs   │  │  │
                    │  │  ├────────────┤  │  │
                    │  │  │ Temporal   │  │  │
                    │  │  │ Workers    │  │  │
                    │  │  └────────────┘  │  │
                    │  └──────────────────┘  │
                    │                        │
                    │  ┌──────────────────┐  │
                    │  │ CloudNativePG    │  │  ← PostgreSQL operator (in-cluster)
                    │  │ PostgreSQL       │  │
                    │  ├──────────────────┤  │
                    │  │ MinIO            │  │  ← S3-compatible object storage (in-cluster)
                    │  ├──────────────────┤  │
                    │  │ Temporal Server  │  │  ← self-hosted, PostgreSQL backend
                    │  └──────────────────┘  │
                    └────────────────────────┘
```

## Core Platform

### Kubernetes — Any CNCF-Conformant Distribution

The cluster itself is environment-dependent. All Helm charts and manifests target standard Kubernetes APIs — no cloud-specific CRDs in the application layer. Choose the distribution based on the company's existing infrastructure, team skills, and operational requirements.

#### AKS (Azure Kubernetes Service)

Managed control plane on Azure. Best when the company is on Azure but doesn't want the full Azure-native template (e.g., they need portability or run workloads across Azure and other environments).

- Pros: Zero control plane ops, managed Istio add-on available, Azure AD integration, node pool autoscaling, strong GPU node support for AI workloads.
- Cons: Tied to Azure for the control plane. AKS-specific features (AGIC, Azure Policy, managed Istio) create soft lock-in if adopted. Kubernetes version availability can lag upstream by weeks.
- Best for: Companies on Azure that want managed infrastructure but portable application workloads.

#### OKE (Oracle Kubernetes Engine)

Managed control plane on Oracle Cloud Infrastructure. OKE's basic clusters have no control plane fee — only pay for worker nodes. Scales to 5,000 worker nodes per cluster.

- Pros: Lowest cost among managed Kubernetes providers for most workloads. Free basic cluster control plane. Strong ARM (Ampere) instance support for cost-efficient compute. Virtual nodes (serverless) available. Deep OCI networking integration.
- Cons: Smaller ecosystem and community compared to EKS/AKS/GKE. Fewer third-party integrations and marketplace add-ons. OCI-specific networking concepts (VCN, security lists) have a learning curve. Talent pool is smaller — fewer engineers with OCI experience.
- Best for: Companies already on Oracle Cloud, cost-sensitive workloads, or organizations with Oracle enterprise agreements.

#### RKE2 (Rancher Kubernetes Engine 2)

SUSE/Rancher's security-focused Kubernetes distribution. Runs on any Linux VMs — on-prem, cloud, or hybrid. Rancher provides multi-cluster management UI.

- Pros: CIS-hardened by default (DISA STIG compliant out of the box). Runs anywhere — on-prem data centers, any cloud VMs, edge locations. Rancher multi-cluster management provides a single pane of glass across all environments. SELinux support. Air-gap installable. FIPS 140-2 compliant builds available.
- Cons: You manage the underlying VMs and OS (patching, scaling, networking). No managed control plane — you own HA, etcd backups, and upgrades. Rancher itself needs to be hosted and maintained. More operational overhead than managed cloud Kubernetes.
- Best for: On-premises deployments, regulated industries requiring CIS/STIG compliance, multi-cluster hybrid environments, companies that need a single management plane across heterogeneous infrastructure.

#### k3s

Lightweight Kubernetes from SUSE/Rancher. Single binary (~70MB), CNCF certified. Replaces etcd with SQLite (or external DB) by default. Strips out cloud provider and storage drivers not needed for edge/small deployments.

- Pros: Minimal resource footprint — runs on 512MB RAM. Installs in under a minute. Perfect for edge, IoT, branch offices, and resource-constrained environments. Fully CNCF conformant despite being lightweight. Built-in Traefik ingress and local-path storage. Easy to automate with Ansible/Terraform.
- Cons: Not designed for large-scale production clusters (hundreds of nodes). SQLite default is single-node; HA requires external datastore (PostgreSQL, MySQL, etcd). Stripped-down components mean some enterprise features need to be added back. Less battle-tested at scale than RKE2 or managed offerings.
- Best for: Edge deployments, branch offices, development/staging environments, IoT gateways, small single-application clusters, resource-constrained hardware.

#### OpenShift (Red Hat)

Red Hat's enterprise Kubernetes platform. Adds an opinionated layer on top of Kubernetes with built-in CI/CD, image registry, developer console, and security policies.

- Pros: Most comprehensive enterprise platform — includes everything (registry, CI/CD, monitoring, logging, service mesh) out of the box. Strong security posture — restricted pod security by default, SELinux enforcement, built-in image scanning. Red Hat support with SLAs. Operator Hub provides curated, certified add-ons. Strong in regulated industries (finance, government, healthcare).
- Cons: Most expensive option — requires Red Hat subscriptions per node. Opinionated — fights you if you deviate from the OpenShift way (e.g., custom ingress controllers, non-Red Hat operators). Heavier resource footprint than vanilla Kubernetes. Slower to adopt upstream Kubernetes versions. Vendor lock-in to Red Hat ecosystem and tooling.
- Best for: Large enterprises that already use Red Hat, regulated industries that value certified/supported stacks, organizations that want an all-in-one platform and are willing to pay for it.

#### Talos Linux

A minimal, immutable Linux OS built specifically for Kubernetes. No SSH, no shell, no package manager — the entire OS is managed via API and a single configuration file. Kubernetes is the only workload it runs.

- Pros: Smallest attack surface of any option — no SSH, no shell access, no way to install packages. Immutable and declarative — entire OS state defined in one config file. Upgrades are atomic (A/B partition scheme). API-driven management via `talosctl`. Cloud-agnostic — runs on bare metal, any cloud, or VMs. Extremely consistent — eliminates OS drift across nodes.
- Cons: Steep learning curve — no SSH means traditional debugging approaches don't work. Smaller community than k3s or RKE2. Troubleshooting requires familiarity with `talosctl` and Talos-specific tooling. Less ecosystem integration than established distributions. Not suitable if the team expects traditional Linux server management.
- Best for: Bare metal deployments where security is paramount, teams that want fully declarative infrastructure, air-gapped or high-security environments, organizations willing to invest in learning a new operational model.

#### kubeadm

The upstream Kubernetes bootstrapping tool. Provides the most vanilla Kubernetes experience — nothing added, nothing removed.

- Pros: Closest to upstream Kubernetes — no vendor modifications or opinions. Full control over every component. Best for learning Kubernetes internals. No licensing costs.
- Cons: Highest operational burden — you manage everything: etcd, certificates, upgrades, networking, storage, monitoring. No built-in HA automation. Requires deep Kubernetes expertise for production. No vendor support unless you add it separately.
- Best for: Teams with deep Kubernetes expertise that want full control, learning/training environments, or as a base for building a custom platform.

#### Selection Guide

| Scenario | Recommended Distribution |
|---|---|
| Company is on Azure, wants managed K8s | AKS |
| Company is on Oracle Cloud | OKE |
| On-premises data center, compliance requirements | RKE2 |
| Edge, IoT, branch office, resource-constrained | k3s |
| Large enterprise, Red Hat shop, regulated industry | OpenShift |
| Bare metal, maximum security, declarative ops | Talos Linux |
| Full control, deep K8s expertise available | kubeadm |
| Multi-cluster across mixed environments | RKE2 + Rancher management |

### Database — CloudNativePG PostgreSQL Operator

CloudNativePG manages the full PostgreSQL lifecycle inside Kubernetes: provisioning, high availability (streaming replication with automatic failover), backups (to MinIO or any S3-compatible target), point-in-time recovery, rolling updates, and monitoring.

This eliminates the need for a managed database service. PostgreSQL runs as a primary + replica(s) with synchronous or asynchronous replication. Backups are scheduled to object storage via Barman.

When a managed PostgreSQL service is available and preferred (e.g., Azure Database for PostgreSQL, OCI Database), CloudNativePG can be swapped out — the application connection string is the only change.

### Service Mesh — Istio

Istio installed via Helm (not a cloud add-on) for full version control. Provides mTLS, traffic management, authorization policies, and observability. Istio Gateway serves as the primary ingress controller — no dependency on cloud-specific ingress controllers.

## Environment-Specific Services

The hybrid template minimizes environment-specific dependencies. Where cloud services are available, they can optionally replace the portable defaults:

### Ingress & Egress

| Concern | Portable Default | Optional Cloud Replacement |
|---|---|---|
| Ingress Controller | Istio Gateway | Cloud LB + Istio Gateway |
| TLS Certificates | cert-manager + Let's Encrypt | Cloud-managed certificates |
| DNS Automation | external-dns operator | Cloud DNS integration |
| WAF | ModSecurity (Istio/nginx filter) or Coraza WAF | Cloud WAF (Azure WAF, AWS WAF) |
| CDN | Cloudflare (external) | Cloud CDN (Front Door, CloudFront) |

Istio Gateway handles TLS termination, routing, and rate limiting. cert-manager automates Let's Encrypt certificate issuance and renewal. external-dns syncs Kubernetes ingress/service records to DNS providers.

For environments without a cloud load balancer (bare metal, edge), MetalLB provides LoadBalancer-type service support.

### Object Storage

MinIO deployed in-cluster provides S3-compatible object storage. Used for file uploads, document storage, backups (CloudNativePG Barman backups), and Temporal archival. MinIO is portable across every Kubernetes environment.

When a cloud object store is available (Azure Blob, S3, OCI Object Storage), applications can target it directly — MinIO is not required. The S3 API compatibility means application code doesn't change.

### Business Logic & Compute — Where Code Runs

Business logic lives in three tiers, chosen by complexity and durability requirements:

**1. Temporal Workers (Durable Workflows)**
For multi-step, long-running, or failure-sensitive business logic. Temporal handles retries, state persistence, timeouts, and compensation (saga patterns). Use for: order processing, onboarding flows, data pipelines, approval chains, multi-service orchestration, anything that needs to survive restarts or run for hours/days.

**2. Kubernetes Deployments & Services (Stateless APIs)**
Standard containers behind Istio for request/response business logic that doesn't need Temporal's durability. Use for: REST/gRPC APIs, CRUD operations, authentication endpoints, real-time queries, synchronous business rules. These are always-on pods scaled via HPA. This is where most simple business logic lives — a container running your API framework of choice.

**3. Fission.io or Knative (In-Cluster Serverless)**
For short-lived, event-triggered logic that doesn't justify an always-on pod — without depending on cloud serverless.

**Fission** is a Kubernetes-native serverless framework. Functions deploy in milliseconds (pre-warmed containers), support multiple languages, and trigger from HTTP, message queues, timers, and Kubernetes watches. Lightweight, fast cold starts, and simple to operate. Best for teams that want FaaS without the complexity of Knative.

**Knative Serving** is the CNCF-backed serverless platform. More feature-rich than Fission — scale-to-zero, revision-based traffic splitting, and deeper Kubernetes integration. Heavier to operate but more mature ecosystem. Best for teams already invested in the CNCF stack or needing advanced traffic management.

Either option provides event-driven, scale-to-zero compute without leaving the cluster. Use for: webhook receivers, image/PDF processing, scheduled jobs, lightweight data transformations, event consumers.

If the environment happens to have cloud serverless available (Azure Functions, Lambda), those can be used instead — but the architecture doesn't depend on them.

**Decision guide:**

| Question | → Use |
|---|---|
| Does it need retries, state, or run longer than a few seconds? | Temporal |
| Is it a synchronous API serving user requests? | Kubernetes service |
| Is it triggered by an event and stateless? | Fission / Knative function |
| Is it a scheduled job? | Temporal (if complex) or Kubernetes CronJob / Fission timer (if simple) |
| Does it need scale-to-zero? | Fission or Knative (not a regular Deployment) |

## Frontend Strategy — React

Identical to AWS/Azure templates: React apps built as static bundles, served from nginx containers inside the cluster behind Istio mesh. nginx handles SPA routing and API proxying.

CDN is environment-dependent. Cloudflare is the most portable option (works regardless of hosting). For environments without CDN, nginx caching headers and Istio Gateway caching provide baseline performance.

## Workflow Orchestration — Temporal

Temporal Server deployed via Helm chart, using CloudNativePG PostgreSQL as its persistence and visibility backend. This is fully self-hosted — no external dependency.

Workers run as standard Kubernetes deployments, scaled via HPA. Temporal Web UI deployed for workflow visibility.

PostgreSQL is sufficient as the Temporal backend for most workloads. For very high throughput (>10K workflows/second), Cassandra can be introduced, but PostgreSQL handles the majority of use cases.

## Observability

All observability is in-cluster and portable:

| Concern | Tool |
|---|---|
| Metrics | Prometheus (via kube-prometheus-stack Helm chart) |
| Logs | Fluent Bit → Loki (in-cluster) or external log aggregator |
| Traces | Istio → OpenTelemetry Collector → Jaeger or Tempo |
| Dashboards | Grafana (Istio, Temporal, PostgreSQL, application dashboards) |
| Alerting | Alertmanager (Prometheus) |

The entire observability stack runs inside the cluster. No cloud-specific monitoring dependencies.

## Authentication & Authorization

### Authentication (Identity)

**Keycloak on Kubernetes** — the only portable, full-featured identity provider that runs anywhere. Deployed via Helm, backed by CloudNativePG PostgreSQL. Provides OIDC/OAuth2/SAML, user federation (LDAP, Active Directory), social login, MFA, and a full admin console. CNCF incubating project.

Keycloak can federate with upstream corporate identity providers (Entra ID, Okta, Google Workspace) when the company has an existing directory. This makes Keycloak the portable identity broker — applications always talk to Keycloak regardless of the upstream IdP.

**Token flow:** User authenticates → Keycloak issues OIDC JWT → JWT passed in Authorization header → Istio sidecar validates JWT at the mesh level → backend services extract claims for application-level decisions.

### Authorization

Three layers, each handling a different scope:

| Layer | Tool | Scope |
|---|---|---|
| Service-to-service | Istio Authorization Policies | Which services can call which services (east-west mesh rules) |
| Application-level | OPA (Open Policy Agent) | Fine-grained user/role/resource authorization (e.g., "user with role X can edit resource Y") |
| Kubernetes admission | OPA Gatekeeper | What can be deployed to the cluster (pod security, image policies, resource limits) |

**Why OPA for hybrid:** OPA is the CNCF graduated policy engine — runs anywhere Kubernetes runs, no cloud dependencies. Rego policies are version-controlled and portable across every environment. OPA integrates with Istio as an external authorizer for request-level policy decisions, and Gatekeeper handles admission control.

**Istio authorization policies** handle service-to-service access control natively without OPA. They evaluate based on source identity (mTLS peer), JWT claims, HTTP method/path, and request headers. For most service mesh authorization, Istio policies are sufficient — OPA adds value for complex, data-driven application authorization logic.

## Security

- Istio mTLS for all east-west traffic.
- Keycloak for user authentication; OIDC JWT tokens validated at mesh edge.
- OPA for fine-grained application authorization; OPA Gatekeeper for admission control.
- Istio authorization policies for service-level access control.
- cert-manager for automated TLS certificate management.
- Sealed Secrets or External Secrets Operator for secret management (backend depends on environment — Vault, cloud KMS, or Kubernetes secrets).
- Network policies via Calico or Cilium.
- ModSecurity or Coraza WAF filter on Istio Gateway for north-south protection.
- Pod Security Standards (restricted) enforced via admission controller.
- RBAC with least-privilege service accounts.

## AI Capabilities

### LLM Gateway — LiteLLM on Kubernetes

LiteLLM proxy is the critical portability layer for AI. Deployed as a Kubernetes service inside the Istio mesh, it provides a unified OpenAI-compatible API that all application code targets. LiteLLM routes to any backend: self-hosted vLLM, Ollama, or cloud APIs (Bedrock, Azure OpenAI, OpenAI direct) — configurable per environment without code changes.

LiteLLM provides: provider routing, model fallback (if primary is down, route to secondary), load balancing across model instances, cost tracking and budget enforcement per team/project, rate limiting, and response caching.

### Self-Hosted Inference — vLLM (Production) / Ollama (Development)

**vLLM** is the production standard for self-hosted LLM inference on Kubernetes. It provides 3x+ throughput over alternatives via PagedAttention and continuous batching. Requires GPU nodes (NVIDIA A100/H100/L4). Deployed as a Kubernetes Deployment with Prometheus metrics, scaled via HPA. Serves open-source models with no per-token costs after GPU infrastructure.

**Ollama** is suitable for development, testing, and air-gapped single-user scenarios. Lightweight, runs on CPU (slower) or GPU. Not recommended for production multi-user workloads.

LiteLLM routes to vLLM/Ollama using the same OpenAI-compatible API — application code doesn't know or care which backend is serving.

### Cloud API Fallback

For hybrid environments that have cloud access but want portability, LiteLLM can route to cloud LLM APIs as a fallback or for models not available locally:

| Provider | LiteLLM Support | Use Case |
|---|---|---|
| AWS Bedrock | Native | Access to foundation models without self-hosting |
| Azure OpenAI | Native | Access to OpenAI models without self-hosting |
| OpenAI Direct | Native | Direct API access |
| Anthropic Direct | Native | Direct Claude API access |

This enables a "self-hosted first, cloud fallback" strategy — run what you can locally, route overflow or specialized models to cloud APIs.

### Vector Database — pgvector on CloudNativePG

CloudNativePG PostgreSQL supports the pgvector extension. Since the architecture already uses CloudNativePG as the primary database, enabling pgvector adds vector storage and similarity search without introducing a separate vector database (no Pinecone, Weaviate, or Milvus needed). Handles up to ~5-10M vectors for most enterprise RAG use cases. Fully portable — runs anywhere CloudNativePG runs.

### AI Workflow Orchestration — Temporal

Temporal (already in the stack) orchestrates multi-step AI pipelines: document ingestion → chunking → embedding → storage, RAG query chains, agent tool-use loops, batch processing, and model evaluation workflows. Temporal's durability guarantees are critical for long-running AI operations that may involve retries, human-in-the-loop steps, or multi-model chains.

### Guardrails — Open Source

Without cloud-managed guardrails (Bedrock Guardrails, Azure Content Safety), use open-source alternatives:

- **Guardrails AI** — open-source framework for input/output validation, hallucination detection, PII filtering, and custom validators.
- **NVIDIA NeMo Guardrails** — open-source toolkit for adding safety rails to LLM applications (topic control, fact-checking, jailbreak prevention).

Both run as services inside Kubernetes and integrate with LiteLLM or application code directly.

## Portability Matrix

Shows which components change when moving between environments:

| Component | Portable (stays same) | Environment-Specific (may change) |
|---|---|---|
| Application containers | ✅ | |
| Istio mesh config | ✅ | |
| Temporal workflows/workers | ✅ | |
| React UI containers | ✅ | |
| Helm charts / manifests | ✅ | |
| Keycloak (authentication) | ✅ | Can federate with cloud IdP |
| OPA / Gatekeeper (authorization) | ✅ | Can swap for Cedar (AWS) or Azure Policy |
| LiteLLM (LLM gateway) | ✅ | Config changes per environment |
| pgvector (vector DB) | ✅ | Can swap for cloud vector search |
| vLLM (self-hosted inference) | ✅ | Can swap for cloud LLM APIs |
| Guardrails AI / NeMo Guardrails | ✅ | Can swap for cloud guardrails |
| CloudNativePG PostgreSQL | ✅ | Can swap for managed PG |
| MinIO | ✅ | Can swap for cloud object store |
| cert-manager | ✅ | Can swap for cloud certs |
| Prometheus/Grafana/Loki | ✅ | Can swap for cloud monitoring |
| Load balancer | | Cloud LB or MetalLB |
| DNS provider | | Cloud DNS or external |
| WAF | | Cloud WAF or ModSecurity |
| CDN | | Cloud CDN or Cloudflare |
| Storage class | | Cloud block storage or local |

## Trade-offs

| Advantage | Disadvantage |
|---|---|
| Zero vendor lock-in — runs anywhere | Higher ops burden — you manage everything |
| Full version control of all components | No managed database — CloudNativePG requires PostgreSQL expertise |
| LiteLLM makes AI tier cloud-agnostic | Self-hosted LLMs require GPU infrastructure and expertise |
| vLLM eliminates per-token costs at scale | GPU nodes are expensive; capacity planning is complex |
| pgvector avoids separate vector DB | pgvector scales to ~5-10M vectors; beyond that needs dedicated vector DB |
| Consistent across all environments | No cloud-native serverless |
| Can migrate to AWS/Azure template later | MinIO requires storage capacity planning |
| Open-source everything — no licensing | Open-source guardrails are less mature than Bedrock/Azure equivalents |
| Works on-prem, edge, air-gapped | CDN requires external provider or none |
