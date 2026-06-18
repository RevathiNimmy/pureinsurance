# Architecture Template: Azure

## When to Use

The company runs on Azure or has Microsoft enterprise agreements driving cloud choice. Best fit when the team wants native AKS integration, Azure AD identity, and managed PaaS services. Azure's first-party Istio add-on for AKS reduces mesh operational overhead compared to self-managed Istio.

## Architecture Overview

```
                         ┌──────────────┐
                         │ Azure Front   │  ← Global CDN / WAF / TLS
                         │ Door          │
                         └──────┬────────┘
                                │
                         ┌──────▼────────┐
                         │ Application   │  ← L7 load balancing, WAF policies,
                         │ Gateway       │     TLS termination
                         └──────┬────────┘
                                │
                    ┌───────────▼────────────┐
                    │        AKS             │
                    │  ┌──────────────────┐  │
                    │  │   Istio Mesh     │  │  ← AKS Istio add-on (managed)
                    │  │  ┌────────────┐  │  │
                    │  │  │ React UIs  │  │  │  ← nginx containers
                    │  │  │ (nginx)    │  │  │
                    │  │  ├────────────┤  │  │
                    │  │  │ API Svcs   │  │  │
                    │  │  ├────────────┤  │  │
                    │  │  │ Temporal   │  │  │
                    │  │  │ Workers    │  │  │
                    │  │  └────────────┘  │  │
                    │  └──────────────────┘  │
                    └───────────┬────────────┘
                                │
              ┌─────────────────┼─────────────────┐
              │                 │                  │
     ┌────────▼──────┐  ┌──────▼───────┐  ┌──────▼───────┐
     │ Azure DB for  │  │ Temporal     │  │ Azure Blob   │
     │ PostgreSQL    │  │ Server       │  │ Storage      │
     │ Flexible Svr  │  │ (on AKS)     │  │              │
     └───────────────┘  └──────────────┘  └──────────────┘
```

## Core Platform

### Kubernetes — Azure Kubernetes Service (AKS)

AKS with system and user node pools. AKS supports a managed Istio add-on — Microsoft maintains the Istio control plane lifecycle, reducing mesh ops. KEDA for event-driven autoscaling. Cluster autoscaler for node scaling.

### Database — Azure Database for PostgreSQL Flexible Server

Fully managed PostgreSQL with zone-redundant high availability. Supports up to 96 vCores and 672 GB RAM. Built-in connection pooling via PgBouncer (server-side, no sidecar needed). Automated backups with point-in-time restore up to 35 days. Private endpoint access from AKS via VNet integration.

CloudNativePG operator is an alternative for running PostgreSQL inside AKS when portability is a priority.

### Service Mesh — Istio (AKS Add-on)

AKS provides Istio as a managed add-on — Microsoft handles upgrades and control plane availability. Provides mTLS, traffic management, observability, and authorization policies. The Istio ingress gateway integrates with Azure Application Gateway for external traffic.

## Environment-Specific Services

### Ingress & Egress

| Concern | Azure Service |
|---|---|
| CDN | Azure Front Door — global edge caching, built-in WAF, TLS |
| WAF | Azure WAF on Application Gateway and/or Front Door — OWASP 3.2 rules, bot protection |
| Load Balancer | Application Gateway (L7) with AGIC add-on, or Azure Load Balancer (L4) |
| TLS Certificates | Azure Key Vault certificates integrated with Application Gateway; or App Service Certificates |
| DNS | Azure DNS — alias records, integration with external-dns operator |

### Object Storage

Azure Blob Storage for file uploads, documents, backups, Temporal archival. Static React assets can be served from Blob Storage with Front Door CDN in front. Azure AD Workload Identity for pod-level access without storage keys.

### Business Logic & Compute — Where Code Runs

Business logic lives in three tiers, chosen by complexity and durability requirements:

**1. Temporal Workers (Durable Workflows)**
For multi-step, long-running, or failure-sensitive business logic. Temporal handles retries, state persistence, timeouts, and compensation (saga patterns). Use for: order processing, onboarding flows, data pipelines, approval chains, multi-service orchestration, anything that needs to survive restarts or run for hours/days.

**2. Kubernetes Deployments & Services (Stateless APIs)**
Standard containers behind Istio for request/response business logic that doesn't need Temporal's durability. Use for: REST/gRPC APIs, CRUD operations, authentication endpoints, real-time queries, synchronous business rules. These are always-on pods scaled via HPA. This is where most simple business logic lives — a container running your API framework of choice.

**3. Azure Functions (Event-Driven / Serverless)**
For short-lived, event-triggered logic that doesn't justify a persistent pod. Functions scale to zero and handle burst traffic without capacity planning. Use for: webhook receivers, image/PDF processing, Blob Storage event triggers, scheduled jobs, lightweight data transformations, Service Bus consumers.

Azure Service Bus provides async messaging (queues and topics). Azure Event Grid routes events between services and Functions.

**Decision guide:**

| Question | → Use |
|---|---|
| Does it need retries, state, or run longer than a few seconds? | Temporal |
| Is it a synchronous API serving user requests? | Kubernetes service |
| Is it triggered by an event and stateless? | Azure Functions |
| Is it a scheduled job? | Temporal (if complex) or Functions with timer trigger (if simple) |

## Frontend Strategy — React

Same pattern as AWS: React apps built as static bundles, served from nginx containers inside AKS behind Istio mesh. nginx handles SPA routing and API proxying.

Azure Front Door provides global CDN caching. Alternative: serve static assets from Blob Storage + Front Door without containers for simpler apps.

## Workflow Orchestration — Temporal

Temporal Server deployed on AKS via Helm chart. Uses Azure Database for PostgreSQL Flexible Server as persistence backend. Workers run as Kubernetes deployments inside the Istio mesh, scaled via HPA or KEDA.

Temporal Cloud is available as a managed alternative — workers remain on AKS.

## Observability

| Concern | Tool |
|---|---|
| Metrics | Prometheus → Azure Managed Grafana, or Azure Monitor managed Prometheus |
| Logs | Fluent Bit → Azure Monitor Logs (Log Analytics workspace) |
| Traces | Istio → OpenTelemetry Collector → Azure Monitor Application Insights or Jaeger |
| Dashboards | Azure Managed Grafana (native Istio and Temporal dashboards) |

Azure Monitor Container Insights provides AKS-specific monitoring out of the box.

## Authentication & Authorization

### Authentication (Identity)

**Primary: Microsoft Entra ID (Azure AD)** — if the company is already a Microsoft shop with Entra ID licenses. Provides OIDC/OAuth2/SAML, conditional access policies, MFA, and deep AKS integration via Azure AD Workload Identity. Most enterprise Azure customers already have it. Entra ID issues JWT tokens consumed by all services in the mesh.

**Alternative: Keycloak on AKS** — when the company is not a Microsoft shop, needs multi-cloud portability, or requires Keycloak's richer admin console and user federation capabilities. Deployed via Helm, backed by Azure Database for PostgreSQL Flexible Server. Keycloak can federate with Entra ID as an upstream identity provider, giving you Keycloak's flexibility with Entra ID as the corporate directory.

**Avoid: Azure AD B2C** — Microsoft's external identity service is notoriously difficult to configure (custom XML policies), has limited admin tooling, and is being superseded by Microsoft Entra External ID. If external/consumer identity is needed, Keycloak is a better choice.

**Token flow:** User authenticates → Entra ID or Keycloak issues OIDC JWT → JWT passed in Authorization header → Istio sidecar validates JWT at the mesh level → backend services extract claims for application-level decisions.

### Authorization

Three layers, each handling a different scope:

| Layer | Tool | Scope |
|---|---|---|
| Service-to-service | Istio Authorization Policies | Which services can call which services (east-west mesh rules) |
| Application-level | OPA (Open Policy Agent) | Fine-grained user/role/resource authorization (e.g., "user with role X can edit resource Y") |
| Kubernetes admission | OPA Gatekeeper or Azure Policy | What can be deployed to the cluster (pod security, image policies, resource limits) |

**Why OPA on Azure:** OPA is the CNCF graduated policy engine with the broadest Kubernetes ecosystem integration. Rego policies are portable and well-understood. Azure Policy for Kubernetes is built on OPA Gatekeeper under the hood, so the concepts align. OPA runs as a sidecar or standalone service, evaluating authorization decisions against Rego policies with JWT claims as input.

**Istio authorization policies** handle service-to-service access control natively without OPA. They evaluate based on source identity (mTLS peer), JWT claims, HTTP method/path, and request headers.

**Azure Policy for Kubernetes** provides built-in governance policies (enforce image sources, deny privileged pods, require labels) and can be extended with custom OPA Gatekeeper constraints.

## Security

- Istio mTLS for east-west traffic.
- Azure WAF for north-south protection.
- Entra ID or Keycloak for user authentication; OIDC JWT tokens validated at mesh edge.
- OPA for fine-grained application authorization.
- Azure AD Workload Identity for pod-level Azure API access — no static credentials.
- Private AKS cluster with private endpoint for API server.
- VNet integration for PostgreSQL Flexible Server — no public endpoint.
- Azure Key Vault for secrets, synced via External Secrets Operator or CSI Secrets Store Driver.
- Azure Policy for Kubernetes and OPA Gatekeeper for cluster governance and compliance.
- Network policies via Azure Network Policy Manager or Calico.

## AI Capabilities

### LLM Gateway — LiteLLM on AKS

LiteLLM proxy deployed as a Kubernetes service inside the Istio mesh. Provides a unified OpenAI-compatible API that all application code targets. LiteLLM handles routing to Azure OpenAI, model fallback, cost tracking, rate limiting, and caching. Application code never calls Azure OpenAI directly — it calls LiteLLM, making the AI tier swappable without code changes.

### Foundation Models — Azure AI Foundry / Azure OpenAI Service

Azure OpenAI Service provides dedicated capacity for OpenAI models with enterprise SLAs and data residency guarantees. Azure AI Foundry (formerly Azure AI Studio) extends access to additional models from Meta, Mistral, Microsoft, Cohere, and others via the model catalog.

Key Azure AI capabilities beyond raw inference:

| Capability | What It Does |
|---|---|
| Azure AI Content Safety | Content filtering, prompt shields, groundedness detection, PII detection. Applied by default to Azure OpenAI models. |
| Azure AI Search | Managed vector + keyword hybrid search for large-scale RAG. Integrates with Azure OpenAI for end-to-end RAG pipelines. |
| Prompt Flow | Visual orchestration for AI workflows — prototyping and evaluation. (Temporal is preferred for production orchestration.) |
| Azure AI Agent Service | Multi-step agent orchestration with tool use, code interpreter, and file search. |

### Vector Database — pgvector on Azure Database for PostgreSQL

Azure Database for PostgreSQL Flexible Server supports the pgvector extension natively. Since the architecture already uses Flexible Server as the primary database, enabling pgvector adds vector storage and similarity search without a separate vector database. Handles up to ~5-10M vectors for most enterprise RAG use cases.

For larger-scale RAG or hybrid keyword+vector search, Azure AI Search provides a fully managed alternative with built-in Azure OpenAI integration.

### AI Workflow Orchestration — Temporal

Temporal (already in the stack) orchestrates multi-step AI pipelines: document ingestion → chunking → embedding → storage, RAG query chains, agent tool-use loops, batch processing, and model evaluation workflows. Temporal's durability guarantees are critical for long-running AI operations that may involve retries, human-in-the-loop steps, or multi-model chains.

### Custom Model Hosting

For fine-tuned or open-source models, AKS with GPU node pools (NVIDIA A100/H100) can host models via vLLM. LiteLLM routes to these self-hosted endpoints alongside Azure OpenAI. Azure Machine Learning managed endpoints are an alternative for teams that prefer managed GPU infrastructure.

## Trade-offs

| Advantage | Disadvantage |
|---|---|
| Managed Istio add-on reduces mesh ops | Istio add-on version may lag upstream |
| Azure OpenAI provides dedicated capacity with SLAs | Vendor lock-in to Azure identity and networking |
| Best access to OpenAI models | Azure OpenAI regional availability can be limited |
| Azure AD integration is seamless for Microsoft shops | Azure AI Search adds cost on top of PostgreSQL |
| Application Gateway + WAF is well-integrated | AGIC add-on has known limitations vs standalone ingress |
| PostgreSQL Flexible Server has built-in PgBouncer | Fewer PostgreSQL engine options than Aurora |
| Front Door provides global anycast routing | Front Door pricing can be complex |
