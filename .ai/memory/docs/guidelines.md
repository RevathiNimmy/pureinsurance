# Pure Insurance - Development Guidelines

## Code Quality Standards

### C# Conventions (REST API Handler & New Components)

**Namespace pattern:** `SSP.PureInsuranceRestAPIHandler.{Layer}` — always matches folder structure exactly.

**Class naming:**
- Commands: `{Action}{Entity}Command` (e.g., `DeleteRenewalCommand`)
- Queries: `Get{Entity}Query` (e.g., `GetInsuranceFileInformationQuery`, `GetDatasetDefinitionQuery`)
- Responses: `{Action}{Entity}CommandResponse` / `{Action}{Entity}QueryResponse` (e.g., `UpdateRIAmendmentStatusCommandResponse`)
- Base classes: `{ClassName}Base` — concrete classes extend base with an empty body when no overrides needed

**Empty class body pattern** — concrete command/query classes are intentionally thin, inheriting all logic from base:
```csharp
namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class DeleteRenewalCommand : DeleteRenewalCommandBase
    {
    }
}
```

**One class per file** — always. File name matches class name exactly.

**No using directives** in files where the base class is in the same namespace.

### VB.NET Conventions (Legacy Components)

- Follow existing patterns in the file being modified — do not introduce C# idioms
- Prefix classes with module code: `bACTAccount`, `dSIRContact`, `aPMNav`
- Use `clsXxx` prefix for utility/helper classes (e.g., `clsCaching`, `clsFormatting`, `clsMoney`)
- Use `funcXxx` prefix for function modules (e.g., `funcDate`, `funcDB`, `funcEmail`, `funcUtils`)

### JavaScript (Portal Utilities)

- No `var`/`let`/`const` — legacy code uses implicit globals (match existing style)
- Functions are standalone, not wrapped in modules or IIFEs
- DOM manipulation via `oElement.form` and `oForm.elements[name]` patterns
- Brace style: opening brace on same line for functions, new line for `if` blocks (mixed — match file)

---

## Structural Conventions

### BaseClasses Layer (SSP.PureInsuranceRestAPIHandler)

All commands and queries live in `BaseClasses/`. The pattern is:

```
BaseClasses/
  {Action}{Entity}Command.cs          ← thin concrete class
  {Action}{Entity}CommandBase.cs      ← base with implementation
  {Action}{Entity}CommandResponse.cs  ← thin response class
  {Action}{Entity}CommandBaseResponse.cs ← base response with implementation
  Get{Entity}Query.cs                 ← thin concrete query
  Get{Entity}QueryBase.cs             ← base with implementation
```

Concrete classes always inherit from `{ClassName}Base` and add nothing unless there is a specific override requirement.

### Project Layer Separation

| Layer | Prefix | Responsibility |
|-------|--------|---------------|
| Business | `b` | Business rules, orchestration |
| Data | `d` | Data access, stored procedure calls |
| Application/UI | `a` | Navigation, UI logic |
| Interface/Controls | `i` | Reusable UI controls |

Never mix layers — a `b` project must not directly call SQL; it calls `d` layer components.

### Stored Procedure Naming

All procedures use `spu_` prefix. Follow the domain prefix:

```sql
spu_SAM_{Entity}_{action}   -- SAM API layer
spu_SIR_{Entity}_{action}   -- Sirius/Underwriting
spu_ACT_{Entity}_{action}   -- Accounting (Orion)
spu_CLM_{Entity}_{action}   -- Claims
spu_TXN_{Entity}_{action}   -- Transactions
spu_calculate_{what}        -- Calculations
```

CRUD suffixes: `_add`, `_upd`, `_del`, `_sel`, `_saa` (select all).

---

## Semantic Patterns

### Command/Query Separation (CQRS-style)

The REST API handler strictly separates reads from writes:
- **Commands** — mutate state: `Create`, `Update`, `Delete`, `Cancel`, `Amend`
- **Queries** — read state: `Get`, `Find`, `List`

Never put query logic in a command class or vice versa.

### Inheritance Over Composition for API Classes

Base classes carry all implementation. Concrete classes are empty shells that allow the type system to distinguish operations without duplicating code:

```csharp
// Correct pattern
public class GetInsuranceFileInformationQuery : GetInsuranceFileInformationQueryBase { }

// Do NOT inline logic in the concrete class
```

### Insurance Domain Rules in Code

- Always validate policy period dates — check for overlapping MTAs and renewal gaps
- Premium calculations must account for short-period rates and pro-rata adjustments
- Reinsurance cession must respect treaty order and XOL/quota share limits
- All state-changing operations require an audit trail entry
- Lock mechanism must be checked before any write operation in the Portal

### Error Handling

- Business rule violations must return a structured error, not throw unhandled exceptions
- Use domain-specific error codes (see API standards): `BUSINESS_RULE_VIOLATION`, `VALIDATION_ERROR`, `NOT_FOUND`
- Never swallow exceptions silently — log with correlation ID

### REST API Standards

Resource URLs use plural nouns, lowercase, hyphen-separated:
```
GET  /api/v1/policies/{id}
POST /api/v1/policies/quotes
PUT  /api/v1/policies/{id}/mta
```

Standard response envelope:
```json
{
  "data": { ... },
  "meta": { "timestamp": "...", "version": "v1" }
}
```

Error envelope:
```json
{
  "error": {
    "code": "BUSINESS_RULE_VIOLATION",
    "message": "...",
    "details": [{ "field": "...", "message": "..." }]
  }
}
```

HTTP status codes: 200 (OK), 201 (Created), 204 (No Content), 400 (Bad Request), 401 (Unauthorized), 403 (Forbidden), 404 (Not Found), 422 (Validation), 500 (Server Error).

### Authentication Pattern

- JWT Bearer tokens in `Authorization` header
- Include `X-Correlation-ID` on all requests for distributed tracing
- Portal uses OWIN middleware with OpenID Connect / WS-Federation
- New components target Azure AD B2C / IdentityServer4

---

## Practices Throughout the Codebase

### Knowledge Base First

Before implementing any insurance domain logic, consult the local knowledge base at `.amazonq/knowledge/` (76 Pure Insurance 6.3 `.md` files). Priority order:
1. Official `.md` user guides (Back Office, Portal, Configuration, Technical, API)
2. `key-business-rules.md`
3. Workspace / Azure DevOps documents

### Audit Trail (Mandatory)

Every state-changing operation on a policy, claim, or accounting record must create an audit trail entry. This is a regulatory requirement — never omit it.

### Testing Standards

- New C# components: xUnit + FsCheck (property-based) + FluentAssertions + Moq
- Coverage target: 80% for new components
- Test categories: positive scenarios, negative scenarios, edge cases, boundary conditions, business rule validations, concurrent user scenarios
- Do not add tests to legacy VB.NET unless explicitly requested

### Shell Script / executeBash Discipline

- Use `fsRead`, `listDirectory`, `fileSearch` first — only fall back to `executeBash` when those are insufficient
- Combine multiple checks into a single `executeBash` call
- Never run a script to verify a result already confirmed by a tool call
- `executeBash` IS required for file deletion — always run a single confirmation check after

### Performance Targets (New APIs)

| Operation | Target |
|-----------|--------|
| Simple GET | < 100ms |
| Complex GET (joins) | < 300ms |
| POST/PUT/PATCH | < 500ms |
| Batch operations | < 2000ms |

### Documentation

- Complex insurance logic must have inline comments explaining the business rule
- New APIs require OpenAPI 3.0 specification
- Architecture decisions go in `docs/Architecture/` as ADRs
- Feature specs go in `.kiro/specs/{feature-name}/` with `requirements.md`, `design.md`, `tasks.md`
