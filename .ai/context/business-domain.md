# Business Domain — Pure Insurance

This document describes the business domain for Pure Insurance to provide AI agents with essential context.

**Last Updated**: 2026-04-28

---

## Overview

Pure Insurance is a general insurance management system developed and maintained by **SSP (Software Solutions Partners Ltd)**. It is supplied to insurance companies, managing general agents (MGAs), and brokers operating primarily in the UK market.

The system supports the full insurance lifecycle — from quotation and new business binding through to mid-term adjustments, renewals, claims settlement, reinsurance management, and premium accounting.

---

## Company Context

- **Software Vendor**: SSP (Software Solutions Partners Ltd)
- **Market**: UK general insurance
- **Customers**: Insurance companies, MGAs, brokers
- **Regulatory Environment**: FCA-regulated; GDPR applies to all personal data (PII)
- **Motor Insurance**: Potential MID (Motor Insurance Database) reporting obligations for motor lines

---

## Primary Users

| Role | Description |
|------|-------------|
| Underwriter | Creates quotes, binds policies, processes MTAs and renewals, manages risk |
| Claims Handler | Opens and manages claims, sets reserves, processes payments, liaises with TPAs |
| Broker | Intermediary — places business, manages client relationships, submits claims |
| Back-Office Staff | Premium accounting, cash handling, ledger reconciliation, MI reporting |
| System Administrator | User management, system configuration, code table maintenance |

---

## Core Business Processes

### 1. New Business (NB)

1. Receive enquiry (direct or via broker)
2. Create a quote — capture risk details, run rating via DRE rules engine
3. Present premium to broker/client
4. Accept and bind policy — create Insurance File and initial Policy/Risk
5. Produce policy documentation (schedule, wording, invoice) via Word/Crystal Reports
6. Collect initial premium

### 2. Mid-Term Adjustment (MTA)

1. Record change of circumstances against an in-force policy
2. Re-rate the risk via DRE
3. Calculate additional premium (AP) or return premium (RP)
4. Amend policy version — maintain full version history
5. Produce endorsement documentation
6. Settle additional/return premium transaction

### 3. Renewal

1. Generate renewal notices prior to expiry
2. Re-rate risk at renewal (using Navigator XM renewal roadmap)
3. Present renewal invitation to broker/client
4. Bind renewal or lapse if not accepted

### 4. Cancellation / Lapse

1. Cancel policy mid-term (at-fault or not-at-fault)
2. Calculate return premium and apply cancellation terms
3. Lapse (non-renewal) at expiry if not renewed

### 5. Claims Management

1. FNOL (First Notification of Loss) — record claim and open claim file
2. Set initial reserves (outstanding loss reserve, expense reserve)
3. Investigate, appoint TPA if required
4. Process claim payments (partial or final settlement)
5. Manage subrogation (TPR — Third Party Recovery) and salvage
6. Close claim on final settlement; reconcile to zero reserve

### 6. Reinsurance (RI)

1. Attach cessions to policies under RI treaties or facultative arrangements
2. Report bordereau to reinsurers
3. Collect RI premium and process RI recoveries on claims

### 7. Premium Accounting

1. Record premium transactions against policy
2. Manage instalment schedules (direct debit / credit)
3. Cash matching — allocate received cash to outstanding debt
4. Produce broker statements and manage commission

---

## Key Business Entities

| Entity | Description |
|--------|-------------|
| Party | Any person or organisation — insured, broker, insurer, TPA, claimant |
| Insurance File | The top-level folder grouping all policies for an insured party |
| Policy | A specific insurance contract, with one or more versions (NB, MTA, renewal) |
| Risk | The subject of insurance (motor vehicle, property address, liability exposure) |
| Risk Type | Configuration definition of a type of risk and its data structure |
| Claim | A claim event against a policy |
| Reserve | Estimated outstanding liability for a claim (loss reserve, expense reserve) |
| Transaction | Financial event — premium, return premium, claim payment, commission |
| Instalment | Scheduled payment of premium in parts |
| Reinsurance Cession | Proportion of risk passed to a reinsurer under a treaty or FAC arrangement |

---

## Business Rules and Configuration

- **Rating rules**: Managed by the **DRE (Decision/Rules Engine)** — an external rules engine integrated via OAuth2 REST API. Rating logic is not embedded in the application.
- **Workflow navigation**: Driven by **Navigator XM** XML roadmaps — define the screens and transitions for each business process (NB, MTA, renewal, claims, etc.)
- **Products**: Configured in **GIS / Product Builder** — define risk types, data structures, rating factors, and premium calculations
- **Code tables**: Configurable lookup values (peril types, risk types, claim types) maintained in the database

---

## Regulatory and Compliance Context

- **FCA**: Products must meet FCA conduct requirements; TCF (Treating Customers Fairly) principles apply
- **GDPR**: PII must not be logged or transmitted insecurely; right-to-erasure requests must be supportable
- **MID**: Motor policies may require notification to the Motor Insurance Database
- **AML**: Anti-money laundering checks may be required for certain premium thresholds
- **Sanctions Screening**: Parties may be subject to sanctions screening before binding

---

## Integration Points

| System | Purpose |
|--------|---------|
| DRE (Decision/Rules Engine) | External rating and business rules — called for quotes, MTAs, renewals |
| iMarket | SOAP-based market submission / quote aggregation |
| AWS S3 | Document storage — policy documents, claim attachments |
| STS (IdentityServer) | Authentication token service — issues JWT/OAuth2 tokens for internal services |
| Azure AD / Entra | Identity for newer components and REST API access |
| Orion | MI and reporting data warehouse integration |
| Crystal Reports | Embedded reporting — policy schedules, claim bordereaux, MI reports |
| Microsoft Word | Policy document production via Word interop |

---

## Domain Vocabulary Quick Reference

See `.ai/memory/glossary.md` for full domain terminology definitions.

| Term | Meaning |
|------|---------|
| NB | New Business |
| MTA | Mid-Term Adjustment |
| RI | Reinsurance |
| FAC | Facultative reinsurance |
| TPR | Third Party Recovery |
| FNOL | First Notification of Loss |
| AP | Additional Premium |
| RP | Return Premium |
| COB | Close of Business |
| SIR | Self-Insured Retention |
