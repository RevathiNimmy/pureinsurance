# Core Technologies

These four open-source technologies appear in every infrastructure architecture template (AWS, Azure, Hybrid). This document explains what each one does, why it was chosen, and how they work together.

## Temporal

**What it is:** A durable workflow orchestration engine. Temporal lets you write long-running business processes as ordinary code — if a step fails, crashes, or the server restarts, Temporal automatically retries from exactly where it left off. No lost state, no manual recovery.

**What it replaces:** Message queues with manual retry logic, state machines stored in database tables, cron jobs with failure tracking, custom saga pattern implementations. Teams typically build some version of this themselves — Temporal is the productized version.

**Why it matters:** Enterprise software is full of multi-step processes that span minutes to months — onboarding workflows, approval chains, billing cycles, data migrations, report generation. Without Temporal, developers build fragile orchestration out of queues, database flags, and retry loops. When something fails at step 7 of 12, recovery is manual and error-prone. Temporal makes these workflows reliable by default.

**How it fits the architecture:** Temporal is the top tier of the three-tier business logic model used across all templates. The tiers are:

1. **Temporal** — durable workflows (multi-step, long-running, must survive failures)
2. **Kubernetes services** — stateless APIs and request/response logic
3. **Serverless functions** — event-driven, short-lived, spiky workloads

Temporal also orchestrates AI pipelines — document processing, RAG ingestion, multi-step agent workflows — where each step may call external APIs that can fail or take unpredictable time.

**Deployment:** Runs as a set of services on Kubernetes. Backed by PostgreSQL for persistence (shares the same database cluster used by the application). Temporal Cloud is available as a managed option.

## Keycloak

**What it is:** An open-source identity and access management server. Keycloak handles user authentication (login), single sign-on (SSO), user federation (connecting to Active Directory or LDAP), social login, and token issuance (OAuth 2.0 / OpenID Connect).

**What it replaces:** Custom login systems, hand-rolled JWT token management, direct LDAP integration code in every application, per-app user databases. Also replaces vendor-specific identity services when portability is needed.

**Why it matters:** Authentication is the one thing every application needs and no one should build from scratch. Keycloak provides a complete, standards-based identity layer that works the same way regardless of where the application runs. It federates with existing corporate directories (Active Directory, Azure AD, LDAP) so users keep their existing credentials. It issues standard OAuth 2.0 / OIDC tokens that every service in the architecture can validate independently.

**How it fits the architecture:** Keycloak is the default authentication provider across all three templates. It runs as a container on Kubernetes, backed by PostgreSQL.

- **AWS template:** Keycloak is the default. Cognito is an alternative for simple consumer-facing apps, but Keycloak is preferred for enterprise scenarios because of its directory federation and customization capabilities.
- **Azure template:** Keycloak is the default for portability. Entra ID (Azure AD) is acceptable when the company is committed to the Microsoft ecosystem and doesn't need to run outside Azure.
- **Hybrid template:** Keycloak is the only option — it's the only choice that runs anywhere without cloud dependencies.

**Key capability:** Multi-tenancy. A single Keycloak instance can manage multiple realms (tenants), each with its own users, roles, identity providers, and login flows. This is critical for SaaS products serving multiple customers.

## Istio

**What it is:** A service mesh for Kubernetes. Istio injects a sidecar proxy (Envoy) alongside every application container. All network traffic between services flows through these proxies, giving the platform automatic mutual TLS encryption, traffic management, observability, and policy enforcement — without changing application code.

**What it replaces:** Application-level TLS configuration, custom retry/timeout/circuit-breaker code, per-service rate limiting, manual distributed tracing instrumentation, network policy YAML for every service pair.

**Why it matters:** In a microservices architecture, every service talks to other services over the network. Without a mesh, each team must independently implement encryption, retries, timeouts, circuit breaking, and observability. This leads to inconsistent implementations, security gaps (unencrypted internal traffic), and blind spots in debugging. Istio makes all of this automatic and consistent across every service.

**Core capabilities:**

- **Mutual TLS (mTLS):** Every service-to-service call is encrypted and authenticated automatically. No certificates to manage in application code. This satisfies zero-trust networking requirements.
- **Traffic management:** Canary deployments (route 5% of traffic to a new version), blue-green deployments, retries, timeouts, and circuit breaking — all configured declaratively, not in code.
- **Observability:** Automatic distributed tracing, metrics, and access logs for every request. Integrates with Prometheus, Grafana, and Jaeger/Zipkin without application changes.
- **Authorization policies:** Control which services can talk to which other services at the mesh level.

**How it fits the architecture:**

- **AWS template:** Self-managed Istio on EKS.
- **Azure template:** AKS has a first-party Istio add-on (managed by Microsoft), reducing operational overhead. Self-managed Istio is also an option.
- **Hybrid template:** Self-managed Istio on any Kubernetes distribution.

**Deployment:** Istio control plane runs on Kubernetes. Envoy sidecar proxies are automatically injected into application pods. Resource overhead is roughly 50-100MB RAM per sidecar.

## Open Policy Agent (OPA)

**What it is:** A general-purpose policy engine. OPA evaluates authorization decisions — "can this user perform this action on this resource?" — using a declarative policy language called Rego. Policies are written as code, versioned in Git, tested like software, and evaluated at runtime with sub-millisecond latency.

**What it replaces:** Hardcoded if/else authorization checks scattered throughout application code, role-based access control tables in databases, custom policy evaluation engines.

**Why it matters:** Authorization logic is one of the most error-prone parts of enterprise software. It starts simple ("admins can do everything, users can read") and grows into a tangled web of role checks, resource ownership rules, tenant isolation, and regulatory constraints. When this logic lives inside application code, it's duplicated across services, hard to audit, and dangerous to change. OPA externalizes authorization into a dedicated engine with a single policy language, making it auditable, testable, and consistent across every service.

**How it fits the architecture:**

- **Azure template:** OPA is the default authorization engine. Runs as a sidecar or standalone service on Kubernetes. Policies are deployed via GitOps alongside application manifests.
- **Hybrid template:** OPA is the default — it's fully open-source and runs anywhere.
- **AWS template:** Cedar (AWS's authorization language, used with Amazon Verified Permissions) is the default instead of OPA. Cedar offers 42-60x faster policy evaluation than Rego and has a more intuitive syntax for RBAC/ABAC patterns. OPA remains an option on AWS for teams that want portability.

**Policy-as-code workflow:** Policies are written in Rego, stored in Git, tested with OPA's built-in test framework (`opa test`), and deployed through the same CI/CD pipeline as application code. Policy changes go through pull request review just like code changes.

## How They Work Together

These four technologies form the platform layer that sits between Kubernetes and application code:

```
┌─────────────────────────────────────────────┐
│                Application Code             │
├──────────┬──────────┬───────────┬───────────┤
│ Temporal │ Keycloak │   Istio   │ OPA/Cedar │
│ workflow │ identity │   mesh    │  authz    │
│ engine   │ provider │  (mTLS,   │  engine   │
│          │ (authn,  │  traffic, │           │
│          │  SSO,    │  observe) │           │
│          │  tokens) │           │           │
├──────────┴──────────┴───────────┴───────────┤
│              Kubernetes + PostgreSQL         │
└─────────────────────────────────────────────┘
```

A typical request flow:

1. User authenticates via **Keycloak** → receives an OAuth 2.0 access token
2. Request hits the API gateway → **Istio** validates the JWT, encrypts the call via mTLS, and routes it to the correct service
3. The service calls **OPA** (or Cedar) with the token claims and the requested action → OPA returns allow/deny
4. If the action triggers a multi-step process, the service starts a **Temporal** workflow that orchestrates the remaining steps durably

Each technology handles one concern. Application code focuses on business logic.
