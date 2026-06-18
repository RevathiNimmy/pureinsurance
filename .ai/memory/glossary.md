# Glossary — Pure Insurance

Domain terms, acronyms, and project-specific vocabulary for Pure Insurance. AI agents should use these terms precisely when discussing the system.

**Last Updated**: 2026-04-28
**Owned By**: Pure Insurance Team

---

## Insurance Domain Terms

| Term | Definition |
|------|-----------|
| **NB (New Business)** | The process of writing a new insurance policy for the first time |
| **MTA (Mid-Term Adjustment)** | A change to an in-force policy mid-policy period (e.g. change of vehicle, address) |
| **Renewal** | The process of continuing an insurance policy for a new period |
| **Lapse** | A policy that has expired without renewal |
| **Cancellation** | Deliberate termination of a policy before its natural expiry |
| **Premium** | The amount charged to the insured for coverage |
| **Reserve** | Estimated funds set aside to pay future claim costs |
| **Reinsurance (RI)** | Insurance purchased by insurers to limit their own risk exposure |
| **RI Transfer** | Movement of reinsurance ceded amounts between treaties or periods |
| **FAC (Facultative Reinsurance)** | Reinsurance negotiated per policy/risk (not via a standing treaty) |
| **Treaty** | A standing reinsurance agreement covering a category of risks |
| **Coinsurance** | A risk shared between multiple insurers on a single policy |
| **COB (Class of Business)** | The category or class of insurance (e.g. motor, property, liability) |
| **Peril** | A specific risk or cause of loss covered (or excluded) under a policy |
| **Peril Type** | Configuration record defining a category of peril |
| **Excess** | The amount the insured pays before insurance cover applies (also called deductible) |
| **Sum Insured** | The maximum amount an insurer will pay under a policy |
| **Cover Note** | A temporary certificate of insurance issued before a full policy document |
| **Instalment** | A scheduled partial payment of premium |
| **Broker** | An intermediary who arranges insurance on behalf of clients |
| **Underwriter** | The person or system that assesses and accepts or declines insurance risk |
| **Claims Handler** | The person processing a claim on behalf of the insurer |
| **Third Party Recovery (TPR)** | Recovering money from a third party responsible for a loss |
| **Salvage** | Recovering value from damaged or total-loss insured property |
| **Loss Schedule** | A detailed record of losses associated with a claim |
| **Policy Version** | A snapshot of policy data at a specific point in time (NB, MTA, renewal) |

---

## System / Technical Terms

| Term | Definition |
|------|-----------|
| **Pure** | The brand name for the Pure Insurance platform (also used as the main database name) |
| **Sirius** | The underlying architecture layer and database schema name (pre-dates the Pure brand) |
| **Navigator XM** | The XML-based roadmap/workflow engine that drives screen navigation in the WinForms client |
| **Roadmap** | A Navigator XM XML file defining the sequence of screens for a business process (e.g. `PFNEWQUOTE.XML`) |
| **GIS (Generic Information System)** | A configurable data-capture framework allowing custom fields and screens without code changes |
| **Product Builder** | The configuration tool for defining insurance products, schemes, and associated rules |
| **DME (Document Management Engine)** | The subsystem for storing, retrieving, and linking documents to business entities |
| **DRE (Decision/Rules Engine)** | An external service providing underwriting rating, rules evaluation, decline/referral decisions |
| **STS (Secure Token Service)** | The SSP-developed identity server (Thinktecture IdentityServer v2) that issues authentication tokens |
| **iMarket** | An insurance market e-trade integration for electronic placement and quotation |
| **dPMDAO** | The core data access object component used by all modules to call SQL Server stored procedures |
| **PM\* components** | Pure Management platform components (e.g. `PMUser`, `PMTask`, `PMSystem`) — the Sirius Architecture layer |
| **b/i project split** | Convention where a feature has a `b` (business logic) VB project and an `i` (interface/UI) VB project |
| **gLibraries** | Global shared VB.NET libraries: `gPMLibrary` (platform), `gSIRLibrary` (Sirius), `gCLMLibrary` (claims), `gACTLibrary` (activities) |
| **Pure Service** | The Windows Service that runs background/batch jobs (renewals, notifications, scheduling) |
| **ISS / PS ticket** | Azure DevOps work item reference used in code comments to trace a change back to its requirement |
| **ACTIPAY** | Activity payment roadmap (`ACTIPAY.XML`) — one of the Navigator XM roadmap definitions |
| **Party** | The core entity representing any person or organisation in the system (insured, broker, insurer, etc.) |
| **Insurance File** | The top-level record grouping a party's policies (analogous to a client file) |
| **Insurance Folder** | A sub-grouping within an insurance file |
| **Risk** | A specific insured item or subject (e.g. a vehicle, a property) attached to a policy |
| **Risk Type** | Configuration defining the type of risk and its associated data fields |
| **Orion** | An external system with a dedicated integration link component |
| **SAM (Sirius Application Module)** | Term used for modular application components within the Sirius architecture |
| **SSP** | Software Solutions Partners Ltd — the company that developed and maintains Pure Insurance |

---

## Acronyms Quick Reference

| Acronym | Expansion |
|---------|-----------|
| NB | New Business |
| MTA | Mid-Term Adjustment |
| RI | Reinsurance |
| FAC | Facultative (reinsurance) |
| COB | Class of Business |
| GIS | Generic Information System |
| DME | Document Management Engine |
| DRE | Decision/Rules Engine |
| STS | Secure Token Service |
| DAO | Data Access Object |
| PM | Pure Management (architecture layer) |
| TPR | Third Party Recovery |
| SAM | Sirius Application Module |
| SSP | Software Solutions Partners Ltd |
