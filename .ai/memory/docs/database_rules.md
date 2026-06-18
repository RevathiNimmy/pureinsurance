# Insurance Database â€” Rules & Query Patterns

> **Single source of truth for all AI coding assistants.**  
> Full table/column/metric reference: [database_knowledge.md](database_knowledge.md)  
> Referenced by both `.github/copilot-instructions.md` and `.amazonq/rules/database.md`.

---

## Contents

1. [Query Rules by Module](#query-rules-by-module)
2. [Verified Join Patterns](#verified-join-patterns)
3. [Currency Rules](#currency-rules)
4. [Schema Design Patterns](#schema-design-patterns)
5. [Key Lookup Codes Quick Reference](#key-lookup-codes-quick-reference)

---

## Query Rules by Module

### Policy Management

#### Latest Policy Version
*Tables*: `insurance_file`

Checking the latest Policy Version

```sql
max(insurance_file_cnt) for given Insurance_ref
```

#### Live Policy Version
*Tables*: `insurance_file`

Checking the Live Policy Version

```sql
insurance_status_id is NULL And Insurance_file_type_id in (2,5,6,9)
```

#### Cancelled Policy Version
*Tables*: `insurance_file, insurance_file_status`

Checking the Cancelled Policy Version

```sql
code = 'CAN' or Insurance_file_type_id =2
```

#### Lapsed Policy Version
*Tables*: `insurance_file_status`

Checking the Lapsed Policy Version

```sql
code = 'LAP'
```

#### New Business Quotes
*Tables*: `insurance_file_type`

Checking the NB Quotes

```sql
code = 'QUOTE'
```

#### All the Clients
*Tables*: `party_type_id`

Get all the Clients

```sql
code in ('PC','CC','GC')
```

#### All the Brokers
*Tables*: `party_type_id`

Get all the Agents

```sql
code = 'AG'
```

#### Policy in Force at Given Date
*Tables*: `insurance_file`

Returns the live policy version that was in force on a specific date — i.e. the policy had already started and had not yet expired on that date. Combine with the Live Policy Version rule to exclude cancelled/lapsed.

```sql
cover_start_date <= @date AND expiry_date >= @date AND insurance_file_status_id IS NULL AND insurance_file_type_id IN (2,5,6,9)
```

#### Policies Incepted in a Date Range
*Tables*: `insurance_file`

Returns all policy versions (new business and renewals) whose cover started within a given date range. Use this for written premium and new business count reports for a period.

```sql
cover_start_date BETWEEN @start_date AND @end_date AND insurance_file_status_id IS NULL AND insurance_file_type_id IN (2,5,6,9)
```

#### Policies Expiring in a Custom Date Range
*Tables*: `insurance_file`

Returns live policies expiring between two given dates. Wider version of the 30-day renewal diary — useful for 60- or 90-day renewal pipeline reports.

```sql
expiry_date BETWEEN @start_date AND @end_date AND insurance_file_status_id IS NULL AND insurance_file_type_id IN (2,5,6,9)
```

#### All Versions of a Single Policy
*Tables*: `insurance_file`

A policy in this system has multiple rows in insurance_file — one per transaction (NB, MTA, MTC, Renewal, etc.). All versions share the same insurance_ref. To see the full history of a policy, filter by insurance_ref.

```sql
WHERE insurance_ref = @policy_ref  -- returns all versions (NB, MTA, renewal, etc.) in order of insurance_file_cnt
```

#### Direct Business (No Broker)
*Tables*: `insurance_file`

A policy placed directly — without a broker — has NULL in lead_agent_cnt. Use LEFT JOIN to party for the broker and filter WHERE lead_agent_cnt IS NULL to isolate direct business.

```sql
insurance_file.lead_agent_cnt IS NULL
```

### Claims Management

#### All Claim Transactions
*Tables*: `transdetail, document, documenttype`

Get All Claims Transactions

```sql
documenttype.code in ('CLO','CLR','CLP','CLA')
```

#### Claims for a Policy Version
*Tables*: `claim, insurance_file`

Claims are linked to a specific policy version via claim.policy_id. To get all claims for a policy (across all versions), join to insurance_file on insurance_ref and retrieve all matching insurance_file_cnt values.

```sql
claim.policy_id = insurance_file.insurance_file_cnt
```

#### Open Claims (Not Yet Settled)
*Tables*: `claim`

An open claim is one where the claim has not been closed/settled. date_closed IS NULL indicates the claim is still in progress. Use this filter for outstanding reserve reports and claims workload.

```sql
claim.date_closed IS NULL
```

#### Closed / Settled Claims
*Tables*: `claim`

A settled claim has a date_closed populated. Use this for claims development, average days to settle, and historical loss analysis.

```sql
claim.date_closed IS NOT NULL
```

#### Claims Opened (FNOL) in a Date Range
*Tables*: `claim`

Returns claims first reported within a date range using date_opened (the FNOL date). Used for claims frequency reports by period.

```sql
claim.date_opened BETWEEN @start_date AND @end_date
```

#### Claims for a Specific Risk
*Tables*: `claim, risk`

A claim can be linked to the risk it relates to via claim.risk_type_id. Join to risk on risk_cnt to get risk details for the claim.

```sql
claim.risk_type_id = risk.risk_cnt
```

### Finance Management

#### Outstanding Premium
*Tables*: `transdetail`

Checking the Outstanding Premium

```sql
outstaning_amount<>0 
```

#### All Policy Transactions
*Tables*: `transdetail, document, documenttype`

Get All Policy Transactions

```sql
documenttype.code in ('SND','SEC','SED','SRD','SID')
```

#### All Receipt Transactions
*Tables*: `transdetail, document, documenttype`

Get All Receipt Transactions 

```sql
documenttype.code in ('SRP')
```

#### All Payment Transactions
*Tables*: `transdetail, document, documenttype`

Get All Payment Transactions 

```sql
documenttype.code in ('SPY')
```

#### Receipt for a Policy
*Tables*: `transdetail, document, documenttype, Allocation, AllocationDetail`

Get Receipt Details for a Policy

```sql
Get records from AllocationDetail for Allocation_id for transdetail id selected for Policy and then filter on SRP
```

#### Transaction in Base Currency
*Tables*: `transdetail`

transdetail.amount holds the amount in the transaction currency. For reports requiring a single currency, use transdetail.base_amount which holds the amount converted to the system base currency. Always use base_amount for totals across multi-currency portfolios.

```sql
Use transdetail.base_amount for currency-neutral totals; transdetail.amount is in transaction currency
```

#### Outstanding Premium on a Transaction
*Tables*: `transdetail, document, documenttype`

transdetail.outstanding_amount holds the unpaid balance of a single debit transaction line. Zero means fully paid/allocated. Non-zero means the broker/client still owes money. Sum across all lines for total outstanding on a policy or account.

```sql
transdetail.outstanding_amount <> 0  -- also filter by documenttype for policy debits only
```

#### Account Balance for a Party (Broker or Client)
*Tables*: `account, party, transdetail`

A party's financial account is linked via account.account_key = party.party_cnt. The net balance is the sum of all outstanding_amount values on transdetail for that account. A positive balance means the party is a debtor (owes money to the insurer).

```sql
account.account_key = party.party_cnt  AND SUM(transdetail.outstanding_amount) per account_id
```

### Reinsurance

#### All RI (Reinsurance) Cession Transactions
*Tables*: `ri_arrangement, ri_arrangement_line, insurance_file`

Reinsurance cession transactions are linked to policies and reinsurance arrangement lines. Join ri_arrangement_line to the relevant policy transdetail via the document. The ri_arrangement_line holds the reinsurer party, the cession percentage and the net RI premium.

```sql
JOIN ri_arrangement_line ON ri_arrangement_line.ri_arrangement_id = ri_arrangement.ri_arrangement_id AND ri_arrangement.covers policy product/class
```

#### All Active Reinsurers
*Tables*: `ri_arrangement_line, party`

Reinsurance companies (parties) are identified by their party_type. They appear as the counterparty in ri_arrangement_line. Join to party via the reinsurer_cnt field on ri_arrangement_line to get reinsurer names and details.

```sql
ri_arrangement_line.reinsurer_cnt = party.party_cnt
```

### Renewals

#### Renewal Quotes
*Tables*: `insurance_file_type`

Checking the Quotes in Renewal

```sql
code = 'RENEWAL'
```

#### Policies Due for Renewal (Next 30 Days)
*Tables*: `insurance_file`

Returns live policies whose expiry date falls within the next 30 days — the renewal diary or work list. Adjust the day interval as required.

```sql
expiry_date BETWEEN GETDATE() AND DATEADD(day, 30, GETDATE()) AND insurance_file_status_id IS NULL AND insurance_file_type_id IN (2,5,6,9)
```

#### Renewal Policy Identification
*Tables*: `insurance_file, insurance_file_type`

A renewal policy is one where the insurance_file_type.code = RENEWAL (for the renewal quote) or the bound renewal version linked to it. Renewal policies share the same insurance_ref as the preceding year's policy and have a cover_start_date equal to the prior policy's expiry_date. The renewal quote status is identified by insurance_file_type.code = 'RENEWAL'.

```sql
insurance_file_type.code = 'RENEWAL'  -- applies to the renewal quote/offer version
```

#### Policies in a Batch Renewal Run
*Tables*: `Batch_Renewal_Job_Run_Policy, Batch_Renewal_Job_Runs, Batch_Renewal_Job, insurance_file`

Batch_Renewal_Job_Run_Policy holds the individual policies processed in each renewal run. Join to Batch_Renewal_Job_Runs for run metadata and to insurance_file for policy details.

```sql
Batch_Renewal_Job_Run_Policy.batch_renewal_job_runs_id = Batch_Renewal_Job_Runs.batch_renewal_job_runs_id
```

### GIS

#### Sum Insured and Premium by Rating Section
*Tables*: `risk, rating_section, insurance_file_risk_link, insurance_file`

The rating_section table holds the breakdown of sum insured and calculated premium by individual cover section within a risk. Join risk → rating_section to get section-level exposure and premium details.

```sql
rating_section.risk_cnt = risk.risk_cnt
```

#### GIS (Risk) Property Values
*Tables*: `risk, GIS_Property, GIS_Data_Model`

Risk attributes (e.g. postcode, building type, alarm type) are stored as GIS properties linked to risks. To get a specific property value, join risk → GIS_Data_Model (to find property definition) and filter on property_name. Values are stored in GIS_Property.

```sql
GIS_Property.risk_cnt = risk.risk_cnt AND GIS_Property.gis_data_model_id = GIS_Data_Model.gis_data_model_id AND GIS_Data_Model.property_name = @property_name
```

### System Administration

#### Active Chase Cycle Items
*Tables*: `Chase_cycle_item, Chase_Cycle_Step, Chase_cycle_rule, insurance_file`

Chase_cycle_item holds individual follow-up items generated for a policy as part of a chase cycle workflow (e.g. chasing outstanding premium). Items that have not been completed are the active workload. Filter on Chase_cycle_item where there is no completion date or the status is open.

```sql
Chase_cycle_item WHERE completed_date IS NULL  -- open items awaiting action
```

#### Background Jobs Pending or Failed
*Tables*: `Background_Job`

Background_Job table holds scheduled background processing jobs. Jobs that are pending, scheduled, or have recorded a failure need attention. Filter on the job status / is_active fields to find jobs that have not completed successfully.

```sql
Background_Job WHERE is_active = 1  -- or filter on last_run_result indicating failure
```

---

## Verified Join Patterns

All joins below are confirmed correct from working SQL queries.

| From Table | Column | To Table | Column | Type | Purpose |
|---|---|---|---|---|---|
| `insurance_file` | `insurance_file_cnt` | `insurance_file_risk_link` | `insurance_file_cnt` | INNER JOIN | Gets Risks linked to a Policy version |
| `insurance_file_risk_link` | `risk_cnt` | `risk` | `risk_cnt` | INNER JOIN | Gets Risks details for a Policy Version |
| `insurance_file` | `insured_cnt` | `party` | `party_cnt` | INNER JOIN | Client associated with the Policy Version |
| `insurance_file` | `lead_agent_cnt` | `party` | `party_cnt` | INNER JOIN | Broker/Agent associated with the Policy version |
| `account` | `account_key` | `party` | `party_cnt` | INNER JOIN | Account details of a Party |
| `insurance_file` | `insurance_file_cnt` | `document` | `insurance_file_cnt` | INNER JOIN | Gets Financial Document for a Policy version |
| `document` | `document_id` | `transdetail` | `document_id` | INNER JOIN | Gets all the Financial details of for a document |
| `risk` | `risk_cnt` | `rating_section` | `risk_cnt` | INNER JOIN | Gets Sum Insured and Premium Breakup for a Risk |
| `insurance_file` | `insurance_file_type_id` | `insurance_file_type` | `insurance_file_type_id` | INNER JOIN | Get Type of a Policy Version |
| `insurance_file` | `insurance_file_status_id` | `insurance_file_status` | `insurance_file_type_id` | INNER JOIN | Get status of a Policy Version |
| `claim` | `policy_id` | `insurance_file` | `insurance_file_cnt` | INNER JOIN | Get Claims linked to a Policy Version |
| `claim` | `risk_type_id` | `risk` | `risk_cnt` | INNER JOIN | Get Risk linked to a Claim |
| `document` | `documenttype_id` | `documenttype` | `documenttype_id` | INNER JOIN | Gets type of a Finance document |
| `transdetail` | `account_id` | `account` | `account_id` | INNER JOIN | Account linked to a Finance transaction |

---

## Currency Rules

| Column | Use When |
|---|---|
| `transdetail.base_amount` | **Always use for totals** â€” converted to system base currency |
| `transdetail.amount` | Original transaction currency â€” single-currency queries only |

> **Never** `SUM(transdetail.amount)` across mixed-currency portfolios. Always `SUM(transdetail.base_amount)`.

---

## Schema Design Patterns

### Policy Versioning
Every transaction on a policy creates a **new row** in `insurance_file`.  
All versions share the same `insurance_ref`. Higher `insurance_file_cnt` = newer version.

```sql
-- All versions of a policy
SELECT * FROM insurance_file WHERE insurance_ref = @ref ORDER BY insurance_file_cnt

-- Latest version only
SELECT * FROM insurance_file
WHERE insurance_file_cnt = (SELECT MAX(insurance_file_cnt) FROM insurance_file WHERE insurance_ref = @ref)

-- Live / in-force version
WHERE insurance_file_status_id IS NULL AND insurance_file_type_id IN (2,5,6,9)
```

### Policy â†’ Transaction path
```sql
insurance_file
  JOIN document    ON document.insurance_file_cnt = insurance_file.insurance_file_cnt
  JOIN transdetail ON transdetail.document_id     = document.document_id
  JOIN documenttype ON documenttype.documenttype_id = document.documenttype_id
```

### Client / Broker links
```sql
JOIN party client         ON insurance_file.insured_cnt    = client.party_cnt
LEFT JOIN party broker    ON insurance_file.lead_agent_cnt = broker.party_cnt   -- NULL = direct
```

### Risk hierarchy
```sql
JOIN insurance_file_risk_link ON insurance_file_risk_link.insurance_file_cnt = insurance_file.insurance_file_cnt
JOIN risk                     ON risk.risk_cnt = insurance_file_risk_link.risk_cnt
JOIN rating_section           ON rating_section.risk_cnt = risk.risk_cnt         -- sum insured & premium
```

### Claim â†’ Policy
```sql
-- claim.policy_id links to a specific policy version
JOIN insurance_file ON insurance_file.insurance_file_cnt = claim.policy_id
JOIN risk           ON risk.risk_cnt = claim.risk_type_id
```

### Party â†’ Account
```sql
JOIN account ON account.account_key = party.party_cnt
JOIN transdetail ON transdetail.account_id = account.account_id
```

### Reinsurance path
```sql
ri_arrangement
  JOIN ri_arrangement_line ON ri_arrangement_line.ri_arrangement_id = ri_arrangement.ri_arrangement_id
  JOIN party reinsurer ON ri_arrangement_line.reinsurer_cnt = reinsurer.party_cnt
```

### GIS / Risk attributes
```sql
JOIN GIS_Property   ON GIS_Property.risk_cnt           = risk.risk_cnt
JOIN GIS_Data_Model ON GIS_Data_Model.gis_data_model_id = GIS_Property.gis_data_model_id
WHERE GIS_Data_Model.property_name = @property_name
```
User-defined dropdown lists: `GIS_User_Def_Header` (list type) â†’ `GIS_User_Def_Detail` (valid items)

### Soft deletes
Always add `WHERE is_deleted = 0` to lookup table queries unless historical data is needed.

### Multilingual captions
`PMCaption` holds every UI label â€” keyed by `caption_id`.  
Use `.description` column directly for English. JOIN `PMCaption` only when multilingual output is required.

### Commission rates (`Commission_Arrangement`)
- `party_cnt = 0` â†’ global default (applies to all brokers not specifically listed)
- `is_value = 0` â†’ `rate` is a percentage; `is_value = 1` â†’ `rate` is a flat amount

### Claims status (`progress_status`)
- `is_closed_check_status = 1` â†’ counts as "closed" in reports
- `is_claim_payment_valid = 1` â†’ further payments are allowed in this status

### Receipt allocation path
```sql
-- From a policy's transdetail, follow through Allocation to find the matching receipt:
transdetail (policy debit)
  JOIN AllocationDetail ON AllocationDetail.allocation_id = Allocation.allocation_id
-- Filter receipts: documenttype.code = 'SRP'
```

---

## Key Lookup Codes Quick Reference

### insurance_file_type.code â€” Policy Version Types

| Code | Meaning |
|---|---|
| `QUOTE` | New business quotation (not yet bound) |
| `RENEWAL` | Renewal quotation/offer |
| `MTA PERM` | Permanent mid-term adjustment |
| `MTA TEMP` | Temporary mid-term adjustment |
| `MTC` | Mid-term cancellation |
| `MTR` | Mid-term reinstatement |
| IDs 2,5,6,9 | Live/bound policy versions (insurance_file_type_id) |

### insurance_file_status.code â€” Policy Status

| Code | Meaning |
|---|---|
| NULL (status_id IS NULL) | Live / in-force |
| `CAN` | Cancelled |
| `LAP` | Lapsed |

### documenttype.code â€” Financial Document Types

| Code(s) | Meaning |
|---|---|
| `SND`, `SEC`, `SED`, `SRD`, `SID` | Policy debit/credit transactions |
| `CLO`, `CLR`, `CLP`, `CLA` | Claims transactions |
| `SRP` | Receipt (cash received) |
| `SPY` | Payment (cash paid out) |

### party_type.code â€” Party Classification

| Code | Meaning |
|---|---|
| `PC` | Personal client |
| `CC` | Corporate client |
| `GC` | Group client |
| `AG` | Agent / Broker |

### progress_status.code â€” Claim Status

| Code | Meaning |
|---|---|
| `OPEN` | Claim open â€” payments valid (is_claim_payment_valid=1) |
| `CLOSED` | Claim settled â€” no further payments (is_closed_check_status=1) |

### Date_for_Treaty_XOL_Calculation.code

| Code | Meaning |
|---|---|
| `RISK` | Risk Attachment Date |
| `TRANS` | Transaction Posting Date |
| `LOSS` | Date of Loss |

### XOL_Treaty_To_Recover_From.code

| Code | Meaning |
|---|---|
| `RISK` | Risk Attaching basis |
| `LOSS` | Loss Occurring basis |

### Proportional_RI_Calculation_Method.code

| Code | Meaning |
|---|---|
| `UNDERWRYR` | Allocate RI premium by Underwriting Year |
| `ACCOUNTYR` | Allocate RI premium by Accounting Year |
