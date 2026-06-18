# CSI Acceleration Program — Training Curriculum

9 hours of hands-on training. Participants go from zero to a deployed, tested feature on modern infrastructure — using AI-assisted development throughout.

## Audience

- Developers who have worked on legacy monolithic applications
- Little to no experience with cloud infrastructure, Kubernetes, microservices, or modern CI/CD
- Each participant has: an AWS or Azure account, a GitHub account with Copilot license, and an Amazon Kiro license

## Philosophy

Every exercise is done WITH AI tools (GitHub Copilot and Kiro), not by hand. The goal is not to memorize Terraform syntax or Kubernetes YAML — it's to learn how to direct AI to build, deploy, and troubleshoot modern software. The AI is the force multiplier that makes 9 hours enough.

Throughout the training, concepts are framed as: "In your monolith, you did X. In this architecture, we do Y instead — here's why."

## Prerequisites

Before the training day, participants must complete two steps: install an AI coding assistant, then use it to install everything else.

### Step 1: Install ONE AI Tool (Do This Manually)

Pick one and install it — this is the last thing you install by hand:

- **Kiro** (recommended)
  - IDE: Download from [kiro.dev/downloads](https://kiro.dev/downloads)
  - CLI: [Install guide](https://kiro.dev/docs/cli/installation/)
- **GitHub Copilot**
  - VS Code extension: [Setup guide](https://code.visualstudio.com/docs/copilot/setup)
  - CLI: [Install guide](https://docs.github.com/en/copilot/how-tos/copilot-cli/install-copilot-cli)

### Step 2: Use the AI to Install Everything Else

Once Kiro or Copilot is working, give it this prompt:

> "I'm preparing for a developer training workshop. Check my machine and install/configure anything that's missing from this list. For each tool, verify it's working after installation:
>
> 1. Git — configured with my name and email
> 2. VS Code — with GitHub Copilot extension
> 3. AWS CLI — configured and authenticated (run `aws sts get-caller-identity` to verify) — OR — Azure CLI — configured and authenticated (run `az account show` to verify)
> 4. kubectl
> 5. Terraform
> 6. Podman Desktop (free, open-source Docker alternative — do NOT install Docker Desktop)
> 7. Node.js 20+
>
> I'm on [Windows / macOS]. Use Homebrew on Mac or winget on Windows where possible."

The AI will detect your OS, check what's already installed, and walk you through the rest.

### Step 3: Verify Access

- [ ] GitHub account with Copilot enabled
- [ ] Kiro authenticated
- [ ] Access to the training repository provided by the Business Transformation team (you should have received an invite — check your GitHub notifications)

---

# Hour 1 — Architecture Foundations

**Goal:** Understand why we build this way, what each piece does, and make your first architecture decisions — all with AI assistance.

**Facilitators:** Riley, Jeevan, Ian

## Concepts (20 min)

### From Monolith to Modern

| In your monolith... | In this architecture... | Why |
|---|---|---|
| One big application, one database | Separate services, each with a purpose | Independent deployment, independent scaling, independent failure |
| Application handles its own login | Keycloak handles all authentication | One login system for everything, standards-based (OAuth/OIDC) |
| Business logic mixed with retry/error handling | Temporal orchestrates multi-step workflows | Workflows survive crashes, automatic retry, no lost state |
| Direct service-to-service calls, hope for the best | Istio service mesh encrypts and manages all traffic | Automatic encryption, retries, circuit breaking, observability |
| if/else permission checks scattered in code | OPA/Cedar evaluates authorization as policy | Auditable, testable, consistent across every service |
| FTP files to a server, pray | GitHub Actions → ArgoCD → Kubernetes | Automated build, test, deploy on every merge |

### Architecture Overview

Walk through the architecture templates. Participants follow along reading the docs:

- `architecture/core-technologies.md` — What Temporal, Keycloak, Istio, and OPA do and why
- `architecture/aws.md` or `architecture/azure.md` — The full stack for their cloud (participants read the one matching their account)

Key points to emphasize:

- Kubernetes is the operating system for modern applications — it runs containers, restarts them when they crash, scales them up and down
- You don't need to understand every component today — the AI tools know these technologies and will help you use them
- The architecture templates are reference designs, not rigid rules

## Exercise 1: Architecture Decision Records (40 min)

An Architecture Decision Record (ADR) is a short document that captures a technical decision — what was decided, why, and what alternatives were considered. They're how teams avoid re-debating the same decisions.

### Steps

1. **Clone the training repository provided by the Business Transformation team.** Prompt:

   > "Clone the repository at &lt;your-training-repo-url&gt; and open it in VS Code"

2. **Open your AI tool (Kiro, Copilot, or your preferred assistant)**

3. **Generate your first ADR.** Prompt:

   > "Create an Architecture Decision Record for choosing PostgreSQL as our database. We're building a new enterprise application. Consider PostgreSQL vs MySQL vs SQL Server vs MongoDB. The team has SQL Server experience from our monolith. Format it as a markdown file in docs/adrs/001-database-choice.md"

4. **Review what the AI generated.** Does it capture real trade-offs? Did it consider your team's SQL Server background? Edit it to reflect your actual opinion.

5. **Generate ADRs for the rest of the stack.** Work through these one at a time — review each before moving to the next. All files go in `docs/adrs/`.

   - `002-kubernetes.md` — "ADR for choosing Kubernetes over traditional VM deployment for our application platform"
   - `003-react.md` — "ADR for choosing React over Angular for the frontend"
   - `004-temporal.md` — "ADR for choosing Temporal over building our own job queue and workflow engine"
   - `005-keycloak.md` — "ADR for choosing Keycloak for authentication over building our own login system"
   - `006-istio.md` — "ADR for choosing Istio service mesh over handling retries, TLS, and observability in application code"
   - `007-opa.md` — "ADR for choosing OPA (Open Policy Agent) for authorization over hardcoded permission checks in code"
   - `008-github-actions.md` — "ADR for choosing GitHub Actions and ArgoCD for CI/CD over Jenkins or manual deployment"
   - `009-vite-spa.md` — "ADR for choosing Vite SPA over Next.js for our internal enterprise application"
   - `010-tanstack-query.md` — "ADR for choosing TanStack Query and Zustand for state management over Redux"

6. **Group review.** Each table shares one ADR. Discuss: Did the AI miss anything? What would you change?

### Checkpoint

- [ ] You have a `docs/adrs/` folder with ADRs covering the full stack (database, Kubernetes, React, Temporal, Keycloak, Istio, OPA, CI/CD, frontend tooling)
- [ ] You've experienced using AI to generate technical documentation
- [ ] You can articulate why this architecture uses Kubernetes, PostgreSQL, and a service mesh

---

# Hour 2 — Agentic Infrastructure: First Steps

**Goal:** Get comfortable using AI to control cloud infrastructure. Understand what Terraform does and how to talk to your cloud account through the CLI.

**Facilitators:** Jeevan, Hammad, Cristian, Riley

## Concepts (15 min)

### Infrastructure as Code

| Old way | New way |
|---|---|
| Click through AWS/Azure console to create resources | Write Terraform code that describes what you want |
| No record of what was created or why | Infrastructure is versioned in Git like application code |
| "It works on my account" | Same code creates identical environments every time |
| Manual changes drift from documentation | Terraform detects drift and can fix it |

### The Tools

- **Terraform** — Describes your infrastructure as code. You write what you want (a database, a Kubernetes cluster), Terraform figures out how to create it.
- **AWS CLI / Azure CLI** — Command-line access to your cloud account. Check what's running, read logs, troubleshoot.
- **kubectl** — Command-line access to Kubernetes. Check what's deployed, read pod logs, troubleshoot.
- **Your AI tool (Kiro, Copilot, etc.)** — Your AI pair programmer. It knows Terraform, AWS, Azure, and Kubernetes. Ask it to write the code, explain errors, and troubleshoot problems.

## Exercise 2: Cloud CLI Orientation (15 min)

Get familiar with your cloud account through the command line — with AI help.

### AWS Track

Open your AI tool and work through these prompts:

1. > "Show me how to list all AWS regions available to my account using the AWS CLI"

2. > "What EC2 instances are running in us-east-1? Show me the CLI command and explain the output"

3. > "Show me how to check my current AWS spending for this month"

### Azure Track

1. > "Show me how to list all Azure regions available to my subscription using the Azure CLI"

2. > "What resource groups exist in my Azure subscription? Show me the CLI command and explain the output"

3. > "Show me how to check my current Azure spending for this month"

### Checkpoint

- [ ] You can run CLI commands against your cloud account
- [ ] You've used AI to generate and explain CLI commands
- [ ] You understand the difference between the cloud CLI (aws/az) and kubectl

## Exercise 3: Terraform the Architecture (30 min)

In Hour 1 you wrote ADRs that define your technology choices. Now the AI turns those decisions into real infrastructure. The ADRs in `docs/adrs/` are the spec — Terraform is the implementation.

### AWS Track

1. Prompt your AI:

   > "Read the Architecture Decision Records in docs/adrs/. Based on those decisions, create a Terraform configuration in infra/terraform/ that provisions:
   > - A VPC with 2 public subnets and 2 private subnets in us-east-1
   > - Security groups for the services we'll deploy later (Kubernetes, PostgreSQL, Istio)
   >
   > Use the AWS provider. Reference which ADR each resource implements in code comments. Include a README explaining what each resource does."

2. Review the generated code. Ask your AI:

   > "Explain this Terraform file line by line. Map each resource back to the ADR that drove the decision. I'm new to Terraform and come from a monolith background."

3. Prompt your AI:

   > "Initialize Terraform in infra/terraform/, run a plan, and explain what it will create and whether it will cost money."

4. Review the plan output with the AI. Ask follow-up questions if anything is unclear.

5. Prompt your AI:

   > "Apply this Terraform configuration and verify the resources were created using the AWS CLI."

### Azure Track

1. Prompt your AI:

   > "Read the Architecture Decision Records in docs/adrs/. Based on those decisions, create a Terraform configuration in infra/terraform/ that provisions:
   > - A resource group and a virtual network with 2 subnets in eastus
   > - Network security groups for the services we'll deploy later (Kubernetes, PostgreSQL, Istio)
   >
   > Use the AzureRM provider. Reference which ADR each resource implements in code comments. Include a README explaining what each resource does."

2. Same as AWS track — ask the AI to explain, plan, apply, and verify.

### Checkpoint

- [ ] AI read your ADRs and generated Terraform that implements those decisions
- [ ] Terraform code comments reference the ADRs that drove each resource
- [ ] You have real cloud resources running
- [ ] You understand the init → plan → apply workflow

---

# Hours 3 and 4 — Agentic Infrastructure: Kubernetes and Services

**Goal:** Get a functional Kubernetes cluster running with a database. This is your development environment for the rest of the training.

**Facilitators:** Jeevan, Hammad, Cristian, Riley

## Concepts (15 min)

### Kubernetes in 5 Minutes

Kubernetes runs containers. That's it. Everything else is details.

| Concept | What it means | Monolith equivalent |
|---|---|---|
| Cluster | A set of machines that Kubernetes manages | Your server rack / VM |
| Pod | One running instance of your application | A process on your server |
| Deployment | "I want 3 copies of this app running at all times" | Nothing — you did this manually |
| Service | A stable network address for a set of pods | localhost:8080 |
| Namespace | A folder for organizing resources | Different folders on a shared drive |
| Ingress | How external traffic reaches your services | Your load balancer / reverse proxy |

### What We're Building

By the end of hour 4, each participant will have:

```
Cloud Account
└── Kubernetes Cluster (EKS or AKS)
    ├── PostgreSQL database (via Helm chart or managed service)
    ├── Istio service mesh (installed)
    └── Your namespace (ready for deployments)
```

## Exercise 4: Provision a Kubernetes Cluster (60 min)

This is the longest exercise. The AI will generate the Terraform, but provisioning takes 15-20 minutes. Use the wait time to explore kubectl.

### AWS Track

1. Prompt your AI:

   > "Read docs/adrs/002-kubernetes.md and docs/adrs/006-istio.md. Add to my existing Terraform in infra/terraform/: an EKS cluster with a managed node group (2 x t3.medium nodes) in the VPC we created. Use the terraform-aws-modules/eks/aws module. Include the IAM roles and security groups needed. Reference the ADRs in code comments."

2. Prompt your AI:

   > "Review the Terraform I just generated, run terraform plan, explain what it will create, then apply it."

   While the cluster provisions (~15 min), move to Exercise 4b.

3. Once the cluster is ready, prompt your AI:

   > "Update my kubeconfig to connect to the new EKS cluster, then verify the connection by listing nodes and namespaces."

### Azure Track

1. Prompt your AI:

   > "Read docs/adrs/002-kubernetes.md and docs/adrs/006-istio.md. Add to my existing Terraform in infra/terraform/: an AKS cluster with a default node pool (2 x Standard_B2s nodes) in the resource group and VNet we created. Enable the Istio add-on. Include the necessary role assignments. Reference the ADRs in code comments."

2. Prompt your AI:

   > "Review the Terraform I just generated, run terraform plan, explain what it will create, then apply it."

   While the cluster provisions (~10 min), move to Exercise 4b.

3. Once the cluster is ready, prompt your AI:

   > "Get AKS credentials and connect kubectl to my new cluster, then verify by listing nodes and namespaces."

## Exercise 4b: Explore Kubernetes While You Wait (15 min)

While your cluster provisions, use your AI to learn kubectl:

1. > "Explain the output of 'kubectl get namespaces' — what are kube-system, kube-public, and default?"

2. > "Create a namespace called 'training' in my cluster and explain what namespaces are for"

3. > "What's the difference between 'kubectl get pods' and 'kubectl get pods -A'?"

4. > "Run a temporary busybox pod in my cluster and exec into it to test network connectivity"

## Exercise 5: Deploy PostgreSQL (30 min)

### AWS Track — Aurora PostgreSQL

1. Prompt your AI:

   > "Read docs/adrs/001-database-choice.md. Add to my Terraform: an Aurora PostgreSQL Serverless v2 cluster in my private subnets. Minimum 0.5 ACU, maximum 2 ACU. Create a database called 'training'. Store the password in AWS Secrets Manager. Create a Kubernetes secret from the Secrets Manager value so pods can connect. Reference the ADR in code comments."

2. Prompt your AI:

   > "Review the Terraform, plan and apply it, then verify the Aurora cluster is running."

### Azure Track — Azure Database for PostgreSQL

1. Prompt your AI:

   > "Read docs/adrs/001-database-choice.md. Add to my Terraform: an Azure Database for PostgreSQL Flexible Server (burstable B1ms) in my subnet. Create a database called 'training'. Store the connection string as a Kubernetes secret. Reference the ADR in code comments."

2. Prompt your AI:

   > "Review the Terraform, plan and apply it, then verify the PostgreSQL server is running."

### Both Tracks — Verify

1. Prompt your AI:

   > "Run a temporary PostgreSQL client pod in Kubernetes, connect to my database, and run 'SELECT version()' to verify it's working."

### Checkpoint (End of Hour 4)

- [ ] Kubernetes cluster is running (`kubectl get nodes` shows Ready)
- [ ] PostgreSQL is accessible from inside the cluster
- [ ] Istio is installed (AWS: `istioctl version`, Azure: AKS add-on enabled)
- [ ] You have a `training` namespace
- [ ] You've used AI to generate all Terraform and troubleshoot any issues

---

# Hours 5 and 6 — Build Your First Feature

**Goal:** Build a complete feature — database, API, authentication, and UI — entirely with AI code generation. Running locally on your laptop.

**Facilitators:** AI Champions

## Concepts (10 min)

### What We're Building

A user, group, and role management system with proper authentication:

- **Keycloak** handles authentication — login, token issuance, password storage, token refresh. No passwords in our database.
- **PostgreSQL** stores the application's RBAC model — users (synced from Keycloak), groups, roles, and permissions. This is business data that the identity provider doesn't own.
- **Express API** validates Keycloak-issued JWTs and enforces permissions from the RBAC model.
- **React admin UI** to manage users, groups, roles, and permissions.

This isn't a throwaway exercise — it's the foundation for every application you'll build after this training. Every product needs user management, groups, and RBAC. What you build here carries forward.

### Why Two Systems?

| Keycloak (identity) | Your API (authorization) |
|---|---|
| "Who are you?" | "What can you do?" |
| Login, passwords, MFA, SSO | Users, groups, roles, permissions |
| Issues JWT tokens | Validates tokens, checks permissions |
| Manages identity across all apps | Manages domain-specific access control |
| Standard (works with any app) | Custom (specific to your product) |

In your monolith, one system did both. In modern architecture, identity and authorization are separate concerns. Keycloak is portable across every app. Your RBAC model is specific to your product.

### The Modern Stack vs Your Monolith

| Monolith | What we're building | Why |
|---|---|---|
| One project, one language | Separate API and UI projects | Deploy and scale independently |
| Server-rendered HTML (JSP, Razor, PHP) | React SPA calling a REST API | Rich interactive UI, API reusable by mobile/integrations |
| Custom login page, passwords in app DB | Keycloak OIDC login, passwords never touch our code | Battle-tested security, MFA/SSO for free |
| Session cookies | JWT tokens from Keycloak | Stateless, works across services |
| Database queries in page handlers | Database migrations + ORM/query builder | Schema versioned in Git, safe rollbacks |
| Hardcoded if/else permission checks | RBAC with groups, roles, and permissions | Add new roles without code changes |

## Exercise 6: Keycloak Setup (15 min)

1. Prompt your AI:

   > "Set up Keycloak for local development:
   > - Add Keycloak to docker-compose.yml (use the official keycloak/keycloak image, dev mode)
   > - Configure a realm called 'training'
   > - Create a client called 'training-api' (confidential, with service account)
   > - Create a client called 'training-ui' (public, with PKCE, redirect to localhost:5173)
   > - Create an admin user in the realm
   > - Export the realm config to keycloak/realm-export.json so it auto-imports on container start
   >
   > Start the containers and verify Keycloak is running at localhost:8080."

2. Prompt your AI:

   > "Explain what just happened. What's a realm? What's the difference between a confidential and public client? What is PKCE and why does the UI client need it?"

## Exercise 7: Database Schema (15 min)

1. Prompt your AI:

   > "Create database migrations for a user, group, and role management system with these tables:
   > - users: id (UUID), keycloak_id (unique, not null — links to Keycloak), email (unique, not null), name (not null), is_active (boolean, default true), created_at, updated_at. Note: NO password field — Keycloak owns authentication.
   > - groups: id (UUID), name (unique, not null — e.g. 'engineering', 'finance', 'operations'), description
   > - roles: id (UUID), name (unique, not null — e.g. 'admin', 'manager', 'viewer'), description
   > - permissions: id (UUID), name (unique, not null — e.g. 'users:read', 'users:write', 'roles:manage'), description
   > - user_groups: user_id, group_id (composite primary key, foreign keys)
   > - group_roles: group_id, role_id (composite primary key, foreign keys)
   > - role_permissions: role_id, permission_id (composite primary key, foreign keys)
   >
   > The permission model is: Users belong to Groups. Groups have Roles. Roles have Permissions. A user's effective permissions are the union of all permissions from all roles of all their groups.
   >
   > Put schema migrations in api/migrations/. Keep these as structure only — no data.
   >
   > Then create a separate seed script at api/seeds/rbac.ts. The seed should:
   > - Be idempotent (safe to run multiple times — use upserts)
   > - Create default groups: administrators, managers, viewers
   > - Create default roles: admin, manager, viewer
   > - Create default permissions: users:read, users:write, users:delete, groups:read, groups:manage, roles:read, roles:manage
   > - Assign all permissions to admin role, assign admin role to administrators group
   > - Assign users:read + users:write + groups:read to manager role, assign to managers group
   > - Assign users:read + groups:read to viewer role, assign to viewers group
   >
   > The seed script should be runnable independently: 'npm run seed'"

2. Prompt your AI:

   > "Apply the migrations and run the seed script against the local PostgreSQL. Verify the tables and seed data."

## Exercise 8: REST API (40 min)

1. Prompt your AI:

   > "Create a REST API in api/ with these endpoints:
   >
   > User management (requires 'users:read' or 'users:write' permission):
   > - GET /api/users — list all users with their groups, roles, and effective permissions
   > - GET /api/users/:id — get one user with full group/role/permission detail
   > - GET /api/users/me — get the current authenticated user with their effective permissions (used by the UI to determine what to show)
   > - PUT /api/users/:id — update user details
   > - DELETE /api/users/:id — deactivate a user (soft delete, requires 'users:delete')
   > - POST /api/users/sync — sync user from Keycloak token (called on first login to create the local user record)
   >
   > Group management (requires 'groups:manage' permission):
   > - GET /api/groups — list all groups with their roles
   > - POST /api/groups — create a group
   > - PUT /api/groups/:id — update a group
   > - POST /api/groups/:id/users — add a user to a group
   > - DELETE /api/groups/:id/users/:userId — remove a user from a group
   > - POST /api/groups/:id/roles — assign a role to a group
   > - DELETE /api/groups/:id/roles/:roleId — remove a role from a group
   >
   > Role management (requires 'roles:manage' permission):
   > - GET /api/roles — list all roles with their permissions
   >
   > Use Express with TypeScript. Connect to PostgreSQL. Include input validation. Add a health check endpoint at GET /health.
   >
   > Create authentication middleware that:
   > 1. Validates the JWT token against Keycloak's public key (fetch from Keycloak's JWKS endpoint, cache it)
   > 2. On first request from a new user, auto-creates a local user record linked by keycloak_id
   > 3. Loads the user's effective permissions from the database (groups → roles → permissions)
   > 4. Attaches the permissions to the request context
   >
   > Create authorization middleware that:
   > 1. Reads the required permission for each endpoint
   > 2. Checks against the user's effective permissions from the request context
   > 3. Returns 403 if the user lacks the required permission
   >
   > Important: Permissions come from OUR database, not from the JWT claims. The JWT only proves identity. Our RBAC model controls access."

2. Prompt your AI:

   > "Start the API and Keycloak. Get a token from Keycloak for the admin user, then test the full flow: call /api/users/sync to create the local user, list users, list groups, assign the admin user to the administrators group. Show me the results."

3. Ask your AI to explain the code:

   > "Walk me through the authentication and authorization middleware. How does it validate the Keycloak token? How does it resolve permissions from the database? Why do we keep permissions in our database instead of in the JWT claims?"

## Exercise 9: Test Permission Boundaries (20 min)

1. Prompt your AI:

   > "Create a second user in Keycloak. Get a token for that user and call /api/users/sync. Add the user to the 'viewers' group. Then test:
   > 1. List users (should succeed — viewers have users:read)
   > 2. Update a user (should fail — viewers lack users:write)
   > 3. Delete a user (should fail — viewers lack users:delete)
   > 4. Manage groups (should fail — viewers lack groups:manage)
   >
   > Show me the response for each request. Verify the API returns 403 for unauthorized actions."

2. Prompt your AI:

   > "Now use the admin user to move the viewer into the 'managers' group. Get a fresh token for that user and retry the actions that previously failed. Verify the permissions changed."

## Exercise 10: React UI (30 min)

1. Prompt your AI:

   > "Create a React app in ui/ using Vite and TypeScript. Build a user management admin UI:
   > - Login via Keycloak using OIDC redirect (use a library like oidc-client-ts or react-oidc-context — NOT a custom login form)
   > - User list page (table showing all users with their groups and roles, search/filter)
   > - User detail page (edit user info, manage group membership)
   > - Group management page (create groups, assign roles to groups, add/remove users)
   > - Role list page (view roles and their permissions)
   > - Permission-aware UI: hide or disable buttons/actions the current user doesn't have permission for (fetch effective permissions from GET /api/users/me after login)
   >
   > Use TanStack Query for API calls. Use Tailwind CSS for styling. Include a navigation bar that shows the current user's name and groups. Proxy API requests to localhost:3000 in Vite config."

2. Prompt your AI:

   > "Install the dependencies and start the React dev server."

3. Open the browser. You should be redirected to Keycloak's login page — no custom login form. Log in as admin — you should see all management options. Log in as the viewer — management options should be hidden or disabled.

4. Ask your AI:

   > "Explain the OIDC login flow. What happens when the user clicks 'Log in'? How does the token get from Keycloak to the React app? How does token refresh work? What happens if someone bypasses the UI and calls the API directly — is it still protected?"

### Checkpoint (End of Hour 6)

- [ ] Keycloak running locally, handling all authentication (no passwords in our code or database)
- [ ] PostgreSQL running with users, groups, roles, permissions (schema and seed data separated)
- [ ] API validates Keycloak tokens and enforces permissions from its own RBAC model
- [ ] React UI uses OIDC redirect to Keycloak (no custom login form)
- [ ] Permission boundaries work: viewers can't do admin actions, group changes update permissions
- [ ] All code was generated by AI — you directed it, reviewed it, and tested it

---

# Hour 7 — Deploy Your First Feature

**Goal:** Push your feature to the cloud through a CI/CD pipeline. See it running on Kubernetes.

**Facilitators:** AI Champions

## Concepts (10 min)

### How Deployment Works Now

```
You push code to GitHub
        ↓
Self-hosted runner (VM in your cloud) picks up the job
        ↓
Runner builds a Docker image
        ↓
Image is pushed to a container registry (ECR / ACR)
        ↓
Runner applies Kubernetes manifests
        ↓
Your feature is live
```

No FTP. No SSH. No "it works on my machine." The pipeline is the only way code reaches production.

### Why Self-Hosted Runners?

GitHub Actions needs credentials to push images to your container registry and deploy to your Kubernetes cluster. In production, you'd use OIDC federation (GitHub Actions assumes an IAM role or Entra ID service principal — no stored secrets). But OIDC federation requires admin-level cloud configuration that isn't available in this training environment.

Instead, we use a self-hosted GitHub Actions runner — a small VM in your cloud account that already has access to your resources. The runner uses your cloud CLI credentials and kubeconfig directly.

> **⚠️ Training Environment Limitation:** Self-hosted runners with personal credentials are acceptable for this training. When you take this solution back to your business, replace this with:
>
> - **AWS:** OIDC federation — GitHub Actions assumes an IAM role via `aws-actions/configure-aws-credentials` with `role-to-assume`. No stored keys.
> - **Azure:** OIDC federation — GitHub Actions authenticates via `azure/login` with a federated service principal. No stored secrets.
> - **Kubernetes:** The runner should authenticate via a service account token with scoped RBAC, not a user's kubeconfig.
>
> Your facilitators can help you plan this transition.

## Exercise 11: Containerize (15 min)

1. Prompt your AI:

   > "Create Dockerfiles for both the API and the UI:
   > - API: multi-stage build, Node.js, runs on port 3000
   > - UI: multi-stage build, npm run build, serve with nginx on port 80
   >
   > Update docker-compose.yml to run the API, UI, PostgreSQL, and Keycloak together. The Keycloak container should import the realm config from keycloak/realm-export.json on startup."

2. Prompt your AI:

   > "Build and start the containers using docker compose, then verify the app works at http://localhost."

## Exercise 12: Set Up Self-Hosted Runner (15 min)

1. Prompt your AI:

   ### AWS Track

   > "Add to my Terraform: a small EC2 instance (t3.small, Amazon Linux 2023) in a public subnet that will serve as a GitHub Actions self-hosted runner. Install:
   > - Docker
   > - AWS CLI (already authenticated via instance profile)
   > - kubectl (configured to connect to my EKS cluster)
   > - The GitHub Actions runner agent (use a registration token from my repository settings)
   >
   > Create an IAM instance profile with permissions to push to ECR and describe EKS clusters. Tag the instance as 'github-runner'."

   ### Azure Track

   > "Add to my Terraform: a small Azure VM (Standard_B2s, Ubuntu 22.04) that will serve as a GitHub Actions self-hosted runner. Install:
   > - Docker
   > - Azure CLI (authenticated with my credentials)
   > - kubectl (configured to connect to my AKS cluster)
   > - The GitHub Actions runner agent (use a registration token from my repository settings)
   >
   > Assign a managed identity with permissions to push to ACR and access AKS. Tag the VM as 'github-runner'."

2. Prompt your AI:

   > "Plan and apply the Terraform. Then verify the runner appears as 'Idle' in my GitHub repository under Settings → Actions → Runners."

3. Ask your AI:

   > "Explain what a self-hosted runner is. How is it different from GitHub's hosted runners? Why are we using one here instead of the default runners?"

## Exercise 13: GitHub Actions Pipeline (15 min)

1. Prompt your AI:

   ### AWS Track

   > "Create a GitHub Actions workflow in .github/workflows/deploy.yml that:
   > - Triggers on push to main
   > - Runs on self-hosted runner (use `runs-on: self-hosted`)
   > - Builds the API, UI, and Keycloak Docker images
   > - Pushes them to Amazon ECR (the runner already has ECR permissions via instance profile)
   > - Applies Kubernetes manifests to deploy to the training namespace
   >
   > Create the Kubernetes manifests in k8s/ using Kustomize with a dev overlay. Include Deployments, Services, and an Ingress for the API, UI, and Keycloak."

   ### Azure Track

   > "Create a GitHub Actions workflow in .github/workflows/deploy.yml that:
   > - Triggers on push to main
   > - Runs on self-hosted runner (use `runs-on: self-hosted`)
   > - Builds the API, UI, and Keycloak Docker images
   > - Pushes them to Azure Container Registry (the runner already has ACR permissions via managed identity)
   > - Applies Kubernetes manifests to deploy to the training namespace
   >
   > Create the Kubernetes manifests in k8s/ using Kustomize with a dev overlay. Include Deployments, Services, and an Ingress for the API, UI, and Keycloak."

2. Review the generated workflow and manifests. Ask your AI:

   > "Explain this GitHub Actions workflow step by step. What does each job do? How does the runner authenticate to the container registry and Kubernetes without stored secrets?"

## Exercise 14: Push and Deploy (10 min)

1. Prompt your AI:

   > "Commit all changes with the message 'Add user management and RBAC system with CI/CD', push to main, and tell me when the GitHub Actions pipeline starts."

2. Watch the pipeline run in GitHub Actions (Actions tab in your repo). It should run on your self-hosted runner.

3. Once the pipeline completes, prompt your AI:

   > "Check the status of my pods, services, and ingress in the training namespace. Then set up port-forwarding so I can access the app in my browser."

### Checkpoint

- [ ] Self-hosted runner is registered and running in your cloud account
- [ ] GitHub Actions pipeline ran on the self-hosted runner
- [ ] Docker images are in your container registry
- [ ] Pods are running in Kubernetes (API, UI, Keycloak)
- [ ] You can access the app through the cluster

---

# Hour 8 — Unit Testing and Integration Testing

**Goal:** Add automated tests that run before code can be merged. Understand what to test at each level.

**Facilitators:** AI Champions

## Concepts (10 min)

### The Testing Pyramid

```
        ╱  E2E  ╲          Few, slow, expensive
       ╱─────────╲         (Hour 9)
      ╱Integration╲        Some, medium speed
     ╱─────────────╲       (This hour)
    ╱  Unit Tests   ╲      Many, fast, cheap
   ╱─────────────────╲     (This hour)
```

### When Tests Run — The Full Pipeline

This is the part most teams get wrong. Each test type has a specific place in the pipeline, tied to a specific environment. Running the wrong tests at the wrong time either slows you down (running E2E on every PR) or lets bugs through (skipping integration tests before deploy).

```
PR opened          Merge to main       Deploy to dev       Promote to test     Promote to prod
    │                    │                   │                    │                    │
    ▼                    ▼                   ▼                    ▼                    ▼
┌────────┐        ┌────────────┐      ┌────────────┐      ┌────────────┐      ┌────────────┐
│ Unit   │        │ Unit       │      │ Integration│      │ E2E        │      │ Smoke      │
│ Tests  │        │ Tests      │      │ Tests      │      │ Tests      │      │ Tests      │
│        │        │ + Build    │      │ (against   │      │ (full user │      │ (critical  │
│ (fast, │        │ + Push     │      │  real DB   │      │  flows in  │      │  paths     │
│  no    │        │ images     │      │  in dev)   │      │  browser)  │      │  only)     │
│  infra)│        │            │      │            │      │            │      │            │
└────┬───┘        └─────┬──────┘      └─────┬──────┘      └─────┬──────┘      └─────┬──────┘
     │                  │                   │                    │                    │
  Pass? ──▶ Allow     Pass? ──▶ Deploy    Pass? ──▶ Allow     Pass? ──▶ Allow     Pass? ──▶ Live
            merge       to dev              promotion           promotion
                                            to test             to prod
```

| Stage | Test type | Environment | What it proves | Gate |
|---|---|---|---|---|
| PR opened | Unit tests | None (CI runner only) | Code logic is correct, no regressions | Block merge if fails |
| Merge to main | Unit tests + build | None (CI runner only) | Images build successfully, tests still pass after merge | Block deploy if fails |
| Deploy to dev | Integration tests | Dev (real DB, real Keycloak) | API works against real infrastructure, RBAC enforced correctly | Block promotion to test |
| Promote to test | E2E tests | Test (browser, full stack) | User flows work end-to-end, UI renders correctly, permissions enforced in browser | Block promotion to prod |
| Promote to prod | Smoke tests | Prod (subset of E2E) | Critical paths work after production deploy — login, core feature, health checks | Alert/rollback if fails |

### Why This Order?

- **Unit tests are fast and cheap** — run them on every PR and every merge. They catch logic bugs in seconds. No infrastructure needed.
- **Integration tests need real infrastructure** — run them in dev after deploy. They catch database issues, API contract breaks, and auth problems. Too slow for PRs.
- **E2E tests are slow and brittle** — run them in test, not dev. They catch UI bugs, browser rendering issues, and full-flow regressions. Running them on every PR would take 10+ minutes and block developers.
- **Smoke tests are a safety net** — a small subset of E2E tests that run in prod after every deploy. They verify the deployment didn't break critical paths. If they fail, roll back immediately.

### What NOT to Do

- **Don't run E2E tests on PRs.** They're too slow and too brittle. Developers will start ignoring failures.
- **Don't skip integration tests.** "Unit tests pass" doesn't mean the API works with a real database.
- **Don't run the full E2E suite in prod.** Smoke tests only — you don't want test data in production.
- **Don't promote without passing tests.** Every environment gate exists for a reason. If integration tests fail in dev, the bug goes to test. If E2E fails in test, the bug goes to prod.

### Why Not Git Hooks?

You may have seen teams use git hooks (pre-commit, pre-push) to run linters or unit tests before code leaves the developer's machine. This sounds like a good idea. It isn't.

| Git hooks (local) | GitHub Actions (server) |
|---|---|
| Runs on the developer's machine | Runs in a consistent, controlled environment |
| Different OS, Node version, dependencies | Same environment every time |
| Bypassed with `git commit --no-verify` | Cannot be bypassed — branch protection enforces it |
| Slows down every commit/push | Runs in the background after push |
| Can't be enforced across the team | Enforced by the repository, not by individual discipline |
| Breaks the AI workflow — the AI commits frequently | AI pushes freely, server validates |

The enforcement point for code quality is the server, not the developer's laptop. GitHub Actions runs unit tests on every PR. Branch protection blocks the merge if they fail. This is a gate that cannot be skipped.

In an AI-first workflow, this matters even more. Your AI tool commits and pushes frequently as it iterates. Git hooks that run tests on every commit add friction to that loop and slow down the AI. Let the AI work fast locally. Let the server enforce quality.

## Exercise 15: Unit Tests (20 min)

1. Prompt your AI:

   > "Write unit tests for the user management API:
   > - Test input validation (missing required fields, invalid email format)
   > - Test the authentication middleware (valid Keycloak JWT, expired token, missing token, invalid signature)
   > - Test the authorization middleware (user with admin permissions, user with viewer permissions attempting write, user with no groups)
   > - Test permission resolution logic (user in multiple groups gets combined permissions)
   > - Mock the database and Keycloak JWKS endpoint — don't connect to real services
   >
   > Use Vitest. Put tests in api/tests/unit/"

2. Prompt your AI:

   > "Run the unit tests and show me the results."

3. Ask your AI:

   > "Explain what mocking is and why we mock the database in unit tests"

4. Break a test on purpose — change a validation rule in the API and watch the test fail. Fix it.

## Exercise 16: Integration Tests (20 min)

1. Prompt your AI:

   > "Write integration tests for the user management API that:
   > - Start PostgreSQL and Keycloak in Docker (use testcontainers)
   > - Run migrations, then run the seed script
   > - Get a real token from Keycloak, call /api/users/sync to create the local user
   > - Test the full request/response cycle: list users, create group, add user to group, assign role, verify permissions change, remove from group, deactivate user
   > - Test permission boundaries: viewer can't write, manager can't manage roles, admin can do everything
   >
   > Put tests in api/tests/integration/"

2. Prompt your AI:

   > "Run the integration tests and show me the results."

3. Discuss: Why are these slower than unit tests? Why do we need both?

## Exercise 17: Add Tests to the Pipeline (10 min)

1. Prompt your AI:

   > "Update the GitHub Actions workflow to implement the full testing pipeline:
   > - On pull request: run unit tests. Block merge if they fail.
   > - On merge to main: run unit tests again, build images, push to registry, deploy to dev.
   > - After deploy to dev: run integration tests against the dev environment. If they fail, do NOT promote.
   > - Add a manual promotion step to test environment (GitHub Actions environment with required reviewers).
   > - After deploy to test: run E2E tests. If they fail, do NOT promote to prod."

2. Prompt your AI:

   > "Create a branch called 'add-tests', commit all changes, push it, and open a Pull Request on GitHub."

3. Watch the checks run on the PR in GitHub.

### Checkpoint

- [ ] Unit tests pass locally
- [ ] Integration tests pass locally (with testcontainers)
- [ ] GitHub Actions runs tests on PR
- [ ] You understand the difference between unit and integration tests

---

# Hour 9 — End-to-End Testing

**Goal:** Build a test that validates the feature works from a real user's perspective — through the browser.

**Facilitators:** AI Champions

## Concepts (10 min)

### What E2E Tests Do

An E2E test opens a real browser, clicks buttons, fills forms, and verifies what the user sees. It tests the entire system — UI, API, database, authentication — working together.

### Why E2E Tests Are Special

- They catch bugs that unit and integration tests miss (CSS hiding a button, a form not submitting, a redirect loop)
- They're slow and brittle compared to other tests — use them sparingly for critical user flows
- They run against a deployed environment, not locally

### Playwright

Playwright is a browser automation framework. It controls Chrome, Firefox, and Safari programmatically. It's faster and more reliable than Selenium (which you may have heard of from the monolith world).

## Exercise 18: Write an E2E Test (25 min)

1. Prompt your AI:

   > "Create a Playwright E2E test in e2e/ that tests the complete user management flow:
   >
   > 1. Navigate to the app
   > 2. Get redirected to Keycloak login page
   > 3. Log in as admin
   > 4. View the user list
   > 5. Create a new group and assign the viewer role to it
   > 6. Add a user to the new group
   > 7. Log out and log in as that user via Keycloak
   > 8. Verify the user can see options appropriate to their role
   > 9. Verify the user cannot access admin-only actions (group management should be hidden/disabled)
   > 10. Log back in as admin and deactivate the user
   >
   > Use Playwright's test runner. Include proper waits and assertions. Take screenshots on failure."

2. Prompt your AI:

   > "Start the app with docker compose, install Playwright browsers, run the E2E tests, and show me the results."

3. Prompt your AI:

   > "Open the Playwright test report so I can see the results in my browser."

4. Ask your AI:

   > "Explain how Playwright locators work. What's the difference between getByRole, getByText, and CSS selectors? Which should I prefer?"

## Exercise 19: Add E2E to the Pipeline (15 min)

1. Prompt your AI:

   > "Update the GitHub Actions workflow so that:
   > - E2E tests run automatically after deployment to the test environment (not dev)
   > - The tests run against the test environment URL
   > - Upload the Playwright HTML report as a GitHub Actions artifact
   > - Upload screenshots on failure as artifacts
   > - Add a smoke test job that runs a subset of E2E tests (login + one critical path) after deployment to prod"

2. Prompt your AI:

   > "Commit all changes with the message 'Add E2E tests with Playwright', push to main, and tell me when the pipeline starts."

## Exercise 20: Break It and Fix It (10 min)

1. Prompt your AI:

   > "Introduce a bug in the API — change the RBAC middleware to skip permission checking so all users can access admin endpoints. Commit and push it to main."

2. Watch the E2E test catch it in the pipeline.

3. Prompt your AI:

   > "Fix the bug you just introduced, commit and push."

4. Watch it pass.

This is the safety net. Every change goes through unit → integration → E2E before it reaches users.

### Checkpoint

- [ ] Playwright E2E test passes locally
- [ ] E2E test runs in GitHub Actions after deployment
- [ ] You've seen a test catch a real bug in the pipeline
- [ ] You understand the testing pyramid: unit (fast, many) → integration (medium) → E2E (slow, few)

---

# What You Built Today

```
GitHub Repository
├── api/                    # REST API with RBAC
│   ├── migrations/         # Database schema (versioned)
│   ├── seeds/              # Seed data (idempotent, env-aware)
│   ├── tests/unit/         # Unit tests (run on PR)
│   └── tests/integration/  # Integration tests (run on merge)
├── ui/                     # React SPA (OIDC login via Keycloak)
├── keycloak/               # Realm config (auto-imported on start)
├── e2e/                    # Playwright E2E tests (run after deploy)
├── k8s/                    # Kubernetes manifests (Kustomize)
│   ├── base/
│   └── overlays/dev/
├── infra/terraform/        # Infrastructure as code
├── docs/adrs/              # Architecture Decision Records
├── Dockerfile (api)
├── Dockerfile (ui)
├── docker-compose.yml
└── .github/workflows/
    └── deploy.yml          # CI/CD pipeline
```

Running in your cloud account:

```
Cloud Account
├── VPC / VNet
├── Kubernetes Cluster (EKS / AKS)
│   ├── Istio service mesh
│   ├── training namespace
│   │   ├── API pods (your code)
│   │   ├── UI pods (nginx + React)
│   │   ├── Keycloak pods
│   │   └── Ingress
│   └── PostgreSQL (Aurora / Flexible Server)
└── Container Registry (ECR / ACR)
```

Pipeline:

```
PR ──▶ Unit Tests ──▶ Merge ──▶ Build + Unit Tests ──▶ Deploy to Dev ──▶ Integration Tests
                                                                              │
                                                              Promote to Test ◀┘ (manual gate)
                                                                     │
                                                                E2E Tests
                                                                     │
                                                              Promote to Prod ◀┘ (manual gate)
                                                                     │
                                                                Smoke Tests
```

## Tips & Tricks: Troubleshooting with AI

Things will break during this training. That's expected — and it's the best way to learn. When something goes wrong, don't try to debug it yourself. Tell the AI what you expected to happen and what happened instead. Let it investigate.

### The Pattern

Every troubleshooting prompt follows the same structure: **"I tried to [goal]. Instead, [what happened]. Diagnose and fix it."**

The AI will run the right commands, read the logs, interpret the output, and either fix the problem or explain what you need to do. You never need to know which commands to run.

### During Infrastructure Setup (Hours 2-4)

> "I asked you to apply the Terraform but it failed. Investigate the error, explain what went wrong, and fix it."

> "The Kubernetes cluster was created but kubectl can't connect to it. Figure out why and fix my connection."

> "You deployed PostgreSQL but the verification query failed. Check if the database is running, whether the Kubernetes secret has the right credentials, and fix whatever is broken."

> "Terraform is saying the state is locked by another process. Explain what that means and resolve it safely."

> "Terraform wants to destroy and recreate my database instead of updating it. Explain why and find a way to apply the change without losing data."

### During Feature Development (Hours 5-6)

> "The API starts but crashes immediately. Check the logs, figure out what's wrong, and fix it."

> "The React app loads but shows 'Network Error' when I try to log in. Investigate the API connection, check the proxy config, and fix it."

> "I can see the Keycloak login page but after logging in I get redirected back with an error. Debug the OIDC flow — check the Keycloak client config, redirect URIs, and CORS settings. Fix whatever is broken."

> "The user list page is empty even though I created users. Check the API response, the database, the JWT permissions, and the React query. Find the disconnect."

### During Deployment (Hour 7)

> "The GitHub Actions pipeline failed. Check the workflow run, find the failing step, explain what went wrong, and fix it."

> "The pipeline passed but my pods are in CrashLoopBackOff. Investigate the pod logs, check the container image, verify the environment variables, and fix it."

> "The app is deployed but I get 502 Bad Gateway in the browser. Trace the request path — ingress, service, pod — and find where it's breaking."

> "Everything was working, then I redeployed and now the database connection fails. Check if the Kubernetes secret is still correct and if the database is reachable from the new pods."

### During Testing (Hours 8-9)

> "The unit tests pass locally but fail in GitHub Actions. Compare the two environments and figure out what's different."

> "The integration tests hang and eventually time out. Check if the testcontainers PostgreSQL is starting correctly and whether the migrations are running."

> "The Playwright E2E test fails on the login step. Run the test in headed mode, check what the browser is actually seeing, and figure out why the login form isn't working."

### General Troubleshooting

At any point, you can ask the AI to do a full health check:

> "Run a full health check on my environment: verify the Kubernetes cluster is healthy, all pods are running, the database is accessible, and the app responds to requests. Report anything that's wrong."

Or ask it to explain something you don't understand:

> "You just ran a bunch of commands to fix that issue. Explain what you did and why, so I understand what went wrong."

### The Rule

You describe the problem. The AI investigates, diagnoses, and fixes. If the first fix doesn't work, just say "that didn't work" — the AI will try a different approach. You're the director, not the debugger.

## What Comes Next

This training covered the development workflow. In production, you'd add:

- **Keycloak** deployed to Kubernetes (you ran it locally in Docker — same container, different environment)
- **Temporal** for multi-step business workflows (see `architecture/core-technologies.md`)
- **OPA/Cedar** for authorization policies (see `architecture/core-technologies.md`)
- **Observability** — Prometheus, Grafana, distributed tracing
- **Multiple environments** — dev → test → prod promotion (see `ci-cd/environment-strategy.md`)
- **ArgoCD** for GitOps-based deployment (see `ci-cd/pipeline-patterns.md`)

The architecture templates (`architecture/aws.md`, `architecture/azure.md`, `architecture/hybrid.md`) describe the full production stack. Today you built the foundation.
