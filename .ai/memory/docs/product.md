# Pure Insurance - Product Overview

## Purpose & Value Proposition

Pure is a commercial insurance management platform built by Software Solutions Partners (SSP). It provides end-to-end policy lifecycle management for insurance companies, brokers, and underwriters — covering the full journey from quoting through claims settlement and accounting.

## Core Domains

| Domain | Description |
|--------|-------------|
| Policy Management | Quotes, new business, MTAs, renewals, cancellations, reinstatements |
| Claims Processing | Open, reserve, payment, recovery, close; coinsurance and RI recoveries |
| Reinsurance | RI 2007 model, treaty management, XOL/CAT XOL, quota share, FAC placement |
| Accounting (Orion) | Ledger, cash lists, allocations, document posting, premium finance, credit control |
| Party Management | Clients (AG/AH/CC/EX/FP/GC/IN/NC/OT/PC), agents, brokers, insurers |
| Web Portals | Nexus broker portal (ASP.NET Web Forms), SSP self-service portal |
| Document Management | DME document production, spooler, templates, SharePoint integration |
| GIS Integration | Geographic risk data for underwriting, product builder |
| Work Manager | Task management, workflow orchestration, scheduled jobs |

## Key Features & Capabilities

- Full policy lifecycle: quote → bind → MTA → renewal → cancellation → reinstatement
- Multi-peril, multi-risk policy structures with user-defined data models
- Reinsurance treaty management with CAT XOL, quota share, FAC, and RI 2007 model
- Integrated accounting via Orion ledger engine with full audit trail
- Claims workflow: open → reserve → payment → recovery → close
- Coinsurance and reinsurance recovery calculations per claim peril
- Broker self-service portal (Nexus) with ASP.NET Web Forms UI
- REST API handler (SSP.PureInsuranceRestAPIHandler) for external integrations
- SAM (Service Agent Middleware) WCF service layer for back-office integration
- Windows background service for scheduled/async jobs (batch renewals, notifications)
- Crystal Reports and SSRS for regulatory and management reporting
- Product Builder tooling for scheme configuration (Excel-based)
- Premium finance (instalments, SDD, credit control, chase cycles)
- Cover note management, MID (Motor Insurance Database) integration
- Multi-currency support with exchange rate management
- Lock mechanism for concurrent user protection on all write operations

## Target Users

| User | Primary Activities |
|------|--------------------|
| Underwriters | Policy creation, rating, risk assessment, RI model maintenance |
| Brokers | Self-service via Nexus portal for quotes and policy management |
| Claims Handlers | Claims registration, reserve management, payment authorisation |
| Accountants | Premium collection, commission, allocations, cash management |
| System Administrators | Product configuration, user management, scheme setup |
| Reinsurers | Treaty management, RI arrangement review, portfolio transfers |

## Key Terminology

| Term | Meaning |
|------|---------|
| Insurance File | A policy record containing versions across its lifecycle |
| Insurance Folder | Groups related insurance files for a party |
| InsuranceFileCnt | Version counter for policy records — the primary versioning key |
| Risk | An insurable item within a policy (e.g., vehicle, property) |
| Peril | A type of loss covered (e.g., fire, theft) |
| MTA | Mid-Term Adjustment — a change to a live policy |
| MTC | Mid-Term Cancellation |
| MTR | Mid-Term Reinstatement |
| RI Arrangement | Reinsurance structure applied to risks |
| Treaty | A reinsurance agreement with external parties |
| CAT XOL | Catastrophe Excess of Loss reinsurance |
| FAC | Facultative reinsurance placement |
| SDD | Standing Direct Debit |
| SFB | Sirius For Broking |
| SFI | Sirius For Insurance |
| SAM | Service Agent Middleware — the WCF service layer |
| dPMDAO | The core COM-based data access object (legacy) |
| Source | A branch/office configuration in the system |
| DataModel | The product-specific risk data schema |

## Architecture Direction

The system is undergoing a Strangler Fig migration from a monolithic VB.NET/COM architecture toward a component-based microservices architecture on Azure (AKS, Service Bus, API Management), with C# Clean Architecture for new components. The REST API handler (SSP.PureInsuranceRestAPIHandler) is the current bridge layer.
