# Insurance Database â€” Full Knowledge Base

> Module-wise reference for AI coding assistants.
> Query rules and join patterns: [database_rules.md](database_rules.md)

---

## Contents

1. [Policy Management](#policy-management) â€” 37 tables
2. [Claims Management](#claims-management) â€” 17 tables
3. [Finance Management](#finance-management) â€” 61 tables
4. [Cash Management](#cash-management) â€” 18 tables
5. [Reinsurance](#reinsurance) â€” 27 tables
6. [Renewals](#renewals) â€” 10 tables
7. [Party Management](#party-management) â€” 46 tables
8. [GIS](#gis) â€” 2 tables
9. [Document Management](#document-management) â€” 19 tables
10. [Tax](#tax) â€” 10 tables
11. [Reporting](#reporting) â€” 4 tables
12. [User Management](#user-management) â€” 10 tables
13. [System Administration](#system-administration) â€” 26 tables
14. [Core](#core) â€” 3 tables

15. [Business Terms Glossary](#business-terms-glossary)
16. [Business Metrics & KPIs](#business-metrics--kpis)
17. [SQL Query Templates](#sql-query-templates)

---

## Policy Management

**Tables in this module**: 37

### Query Rules

**Latest Policy Version** â€” `max(insurance_file_cnt) for given Insurance_ref`  
*Checking the latest Policy Version*

**Live Policy Version** â€” `insurance_status_id is NULL And Insurance_file_type_id in (2,5,6,9)`  
*Checking the Live Policy Version*

**Cancelled Policy Version** â€” `code = 'CAN' or Insurance_file_type_id =2`  
*Checking the Cancelled Policy Version*

**Lapsed Policy Version** â€” `code = 'LAP'`  
*Checking the Lapsed Policy Version*

**New Business Quotes** â€” `code = 'QUOTE'`  
*Checking the NB Quotes*

**All the Clients** â€” `code in ('PC','CC','GC')`  
*Get all the Clients*

**All the Brokers** â€” `code = 'AG'`  
*Get all the Agents*

**Policy in Force at Given Date** â€” `cover_start_date <= @date AND expiry_date >= @date AND insurance_file_status_id IS NULL AND insurance_file_type_id IN (2,5,6,9)`  
*Returns the live policy version that was in force on a specific date — i.e. the policy had already started and had not yet expired on that date. Combine with the Live Policy Version rule to exclude cancelled/lapsed.*

**Policies Incepted in a Date Range** â€” `cover_start_date BETWEEN @start_date AND @end_date AND insurance_file_status_id IS NULL AND insurance_file_type_id IN (2,5,6,9)`  
*Returns all policy versions (new business and renewals) whose cover started within a given date range. Use this for written premium and new business count reports for a period.*

**Policies Expiring in a Custom Date Range** â€” `expiry_date BETWEEN @start_date AND @end_date AND insurance_file_status_id IS NULL AND insurance_file_type_id IN (2,5,6,9)`  
*Returns live policies expiring between two given dates. Wider version of the 30-day renewal diary — useful for 60- or 90-day renewal pipeline reports.*

**All Versions of a Single Policy** â€” `WHERE insurance_ref = @policy_ref  -- returns all versions (NB, MTA, renewal, etc.) in order of insurance_file_cnt`  
*A policy in this system has multiple rows in insurance_file — one per transaction (NB, MTA, MTC, Renewal, etc.). All versions share the same insurance_ref. To see the full history of a policy, filter by insurance_ref.*

**Direct Business (No Broker)** â€” `insurance_file.lead_agent_cnt IS NULL`  
*A policy placed directly — without a broker — has NULL in lead_agent_cnt. Use LEFT JOIN to party for the broker and filter WHERE lead_agent_cnt IS NULL to isolate direct business.*

### Tables

#### `Event_Insurance_File`
**Domain: Policy**  **Owner: Underwriting Department**  **Refresh: Real-time**

Snapshot of insurance file (policy) data captured at the time an event was raised

*Keywords*: Event, Insurance File, Policy, Snapshot, Premium, Cover

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `insurance_file_cnt` | `int` |  | FK to Insurance_File (policy) |  |
| `insurance_file_status_id` | `int` |  | Policy status at time of event |  |
| `insurance_folder_cnt` | `int` |  | FK to Insurance_Folder |  |
| `insurance_ref` | `varchar` |  | Policy reference number at time of event |  |
| `product_id` | `int` |  | FK to product at time of event |  |
| `source_id` | `int` |  | FK to source/branch at time of event |  |
| `lead_insurer_cnt` | `int` |  | Lead insurer party at time of event |  |
| `lead_agent_cnt` | `int` |  | Lead agent party at time of event |  |
| `insured_cnt` | `int` |  | Insured party at time of event |  |
| `cover_start_date` | `datetime` |  | Cover start date at time of event |  |
| `expiry_date` | `datetime` |  | Expiry date at time of event |  |
| `renewal_date` | `datetime` |  | Renewal date at time of event |  |
| `annual_premium` | `numeric` |  | Annual premium at time of event |  |
| `this_premium` | `numeric` |  | Transaction premium at time of event |  |
| `net_premium` | `numeric` |  | Net premium at time of event |  |
| `commission_amount` | `numeric` |  | Commission amount at time of event |  |
| `commission_percentage` | `numeric` |  | Commission percentage at time of event |  |
| `ipt_percentage` | `numeric` |  | IPT percentage at time of event |  |
| `currency_id` | `smallint` |  | Currency at time of event |  |
| `Policy_type_id` | `int` |  | Policy type at time of event |  |
| `renewal_premium` | `money` |  | Renewal premium at time of event |  |
| `underwriting_year_id` | `int` |  | FK to Underwriting_Year |  |
| `policy_status_id` | `int` |  | Policy status identifier at time of event |  |
| `insured_name` | `varchar` |  | Insured name at time of event |  |
| `alternate_reference` | `varchar` |  | Alternate policy reference at time of event |  |
| `broker_cnt` | `int` |  | Broker party at time of event |  |
| `date_issued` | `datetime` |  | Date the policy was issued |  |

*Foreign Keys*:
- `Event_Insurance_File.insurance_file_cnt` â†’ `Insurance_File (insurance_file_cnt)` (Many-to-One)

#### `Event_Insurance_File_System`
**Domain: Policy**  **Owner: Underwriting Department**  **Refresh: Real-time**

Snapshot of insurance file system data (last transaction) captured at the time an event was raised

*Keywords*: Event, Insurance File, System, Last Transaction, Snapshot

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `insurance_file_cnt` | `int` |  | FK to Insurance_File |  |
| `endorsement_count` | `int` |  | Endorsement count at time of event |  |
| `created_by_id` | `smallint` |  | FK to PMUser who created |  |
| `date_created` | `datetime` |  | Date created |  |
| `modified_by_id` | `smallint` |  | FK to PMUser who last modified |  |
| `last_modified` | `datetime` |  | Date last modified |  |
| `last_trans_date` | `datetime` |  | Last transaction date at time of event |  |
| `last_trans_type_id` | `int` |  | FK to Transaction_Type |  |
| `last_trans_description` | `varchar` |  | Last transaction description at time of event |  |
| `last_trans_debit_credit` | `char` |  | D/C indicator for last transaction | D,C |
| `last_trans_document_ref` | `varchar` |  | Last transaction document reference |  |
| `last_trans_cover_start_date` | `datetime` |  | Cover start date of last transaction |  |
| `last_trans_expiry_date` | `datetime` |  | Expiry date of last transaction |  |

*Foreign Keys*:
- `Event_Insurance_File_System.insurance_file_cnt` â†’ `Insurance_File (insurance_file_cnt)` (Many-to-One)

#### `Event_Insurance_Folder`
**Domain: Policy**  **Owner: Underwriting Department**  **Refresh: Real-time**

Snapshot of insurance folder (client folder) data captured at the time an event was raised

*Keywords*: Event, Insurance Folder, Client, Snapshot

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `insurance_folder_cnt` | `int` |  | FK to Insurance_Folder |  |
| `insurance_folder_id` | `int` |  | Insurance folder identifier at time of event |  |
| `source_id` | `int` |  | FK to source/branch at time of event |  |
| `insurance_holder_cnt` | `int` |  | Insured/holder party at time of event |  |
| `code` | `varchar` |  | Folder code at time of event |  |
| `description` | `varchar` |  | Folder description at time of event |  |
| `inception_date` | `datetime` |  | Folder inception date at time of event |  |
| `quote_insurance_ref` | `varchar` |  | Quote reference at time of event |  |
| `next_insurance_ref` | `varchar` |  | Next insurance reference at time of event |  |
| `last_insurance_ref` | `varchar` |  | Last insurance reference at time of event |  |
| `renewal_count` | `int` |  | Renewal count at time of event |  |
| `arc_archive_folder_id` | `int` |  | Archive folder link |  |

*Foreign Keys*:
- `Event_Insurance_Folder.insurance_folder_cnt` â†’ `Insurance_Folder (insurance_folder_cnt)` (Many-to-One)

#### `GIS_Data_Model`
**Domain: Policy**  **Owner: IT Department**  **Refresh: Real-time**

GIS data model definition — a versioned schema describing the objects and properties used for a product

*Keywords*: GIS, Data Model, Schema, Product, Property Rating

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `gis_data_model_id` | `int` |  | Unique data model identifier (PK) |  |
| `code` | `char` |  | Short code for the data model |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `description` | `varchar` |  | Description of the data model |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `gis_data_model_type_id` | `int` |  | FK to GIS_Data_Model_Type |  |
| `product_option` | `int` |  | Product option flags for this data model |  |
| `is_imported_marketplace_data_model` | `tinyint` |  | Flag: imported from marketplace | 0,1 |
| `is_marketplace_data_model` | `tinyint` |  | Flag: this is a marketplace data model | 0,1 |

*Foreign Keys*:
- `GIS_Data_Model.gis_data_model_type_id` â†’ `GIS_Data_Model_Type (gis_data_model_type_id)` (Many-to-One)

#### `GIS_Object`
**Domain: Policy**  **Owner: IT Department**  **Refresh: Real-time**

An object (table mapping) within a GIS data model — defines which DB table holds the data for a rating object

*Keywords*: GIS, Object, Table Mapping, Data Model, Rating

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `gis_object_id` | `int` |  | Unique object identifier (PK) |  |
| `gis_data_model_id` | `int` |  | FK to GIS_Data_Model |  |
| `object_name` | `varchar` |  | Logical name of the GIS object |  |
| `table_name` | `varchar` |  | Database table this object maps to |  |
| `max_instances` | `int` |  | Maximum number of instances allowed per policy |  |
| `is_quote_object` | `tinyint` |  | Flag: object is used in quoting | 0,1 |
| `parent_object_id` | `int` |  | FK to parent GIS_Object (for nested objects) |  |
| `polaris_object_id` | `int` |  | Polaris internal object identifier |  |
| `is_selectable_for_screen` | `tinyint` |  | Flag: object can be added to a screen | 0,1 |
| `is_non_gis` | `tinyint` |  | Flag: object is non-GIS (system table only) | 0,1 |
| `edit_flags` | `tinyint` |  | Bitmask of edit permission flags |  |

*Foreign Keys*:
- `GIS_Object.gis_data_model_id` â†’ `GIS_Data_Model (gis_data_model_id)` (Many-to-One)
- `GIS_Object.parent_object_id` â†’ `GIS_Object (gis_object_id)` (Many-to-One)

#### `GIS_Policy_Link`
**Domain: Policy**  **Owner: Underwriting Department**  **Refresh: Real-time**

Links a policy (insurance file) to a GIS data model and external scheme for geographic/property data integration

*Keywords*: GIS, Policy Link, Data Model, Property, Geographic

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `gis_policy_link_id` | `int` |  | Unique GIS policy link identifier |  |
| `gis_data_model_id` | `int` |  | FK to GIS_Data_Model |  |
| `insurance_file_cnt` | `int` |  | FK to Insurance_File |  |
| `quote_ref` | `char` |  | Quote reference used in GIS integration |  |
| `quote_ref_password` | `varchar` |  | Password for the GIS quote reference |  |
| `guaranteed_quote_date` | `datetime` |  | Date the GIS quote was guaranteed |  |
| `gis_scheme_id` | `int` |  | GIS scheme identifier |  |
| `transact_date` | `datetime` |  | Date of the linked GIS transaction |  |
| `transact_type` | `varchar` |  | Type of GIS transaction (e.g. NB, MTA, RN) |  |
| `party_cnt` | `int` |  | FK to Party (insured party on GIS transaction) |  |
| `risk_id` | `int` |  | FK to risk record |  |
| `claim_id` | `int` |  | FK to claim (if GIS link is claim-related) |  |
| `gis_data_model_type_id` | `int` |  | Type of GIS data model |  |
| `work_claim_id` | `int` |  | FK to work claim |  |
| `case_id` | `int` |  | FK to Case |  |

*Foreign Keys*:
- `GIS_Policy_Link.gis_data_model_id` â†’ `GIS_Data_Model (gis_data_model_id)` (Many-to-One)
- `GIS_Policy_Link.insurance_file_cnt` â†’ `Insurance_File (insurance_file_cnt)` (Many-to-One)
- `GIS_Policy_Link.case_id` â†’ `Case (case_id)` (Many-to-One)

#### `GIS_Property`
**Domain: Policy**  **Owner: IT Department**  **Refresh: Real-time**

A property (column mapping) on a GIS object — defines the field, data type and search/input behaviour

*Keywords*: GIS, Property, Column Mapping, Data Model, Rating Field

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `gis_property_id` | `int` |  | Unique property identifier (PK) |  |
| `gis_object_id` | `int` |  | FK to GIS_Object |  |
| `property_name` | `varchar` |  | Logical name of the property |  |
| `column_name` | `varchar` |  | Database column this property maps to |  |
| `data_type` | `tinyint` |  | Data type code for this property |  |
| `is_input_property` | `tinyint` |  | Flag: property is an input field | 0,1 |
| `is_identifying_property` | `tinyint` |  | Flag: property uniquely identifies the object instance | 0,1 |
| `is_primary_key` | `tinyint` |  | Flag: property is the primary key of the object | 0,1 |
| `polaris_property_id` | `int` |  | Polaris internal property identifier |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `is_search_property` | `tinyint` |  | Flag: property is used in search/lookup | 0,1 |
| `index_linking_id` | `int` |  | FK to index_linking (for dropdown/lookup binding) |  |
| `Edit_Flags` | `tinyint` |  | Bitmask of edit permission flags |  |
| `Specials_Type` | `int` |  | Special behaviour type code |  |
| `Specials_Type_Reference` | `varchar` |  | Reference value for special type |  |
| `is_in_mis_export` | `tinyint` |  | Flag: included in MIS export | 0,1 |
| `is_formatted_text` | `tinyint` |  | Flag: property holds formatted/RTF text | 0,1 |
| `is_chase_cycle_property` | `int` |  | Flag: used in chase cycle processing |  |
| `is_claim360display` | `tinyint` |  | Flag: shown in Claim 360 display | 0,1 |

*Foreign Keys*:
- `GIS_Property.gis_object_id` â†’ `GIS_Object (gis_object_id)` (Many-to-One)

#### `GIS_Screen`
**Domain: Policy**  **Owner: IT Department**  **Refresh: Real-time**

A GIS screen definition — defines a data entry or display screen layout for a product

*Keywords*: GIS, Screen, Layout, Product, Data Entry

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `gis_screen_id` | `int` |  | Unique screen identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code for the screen |  |
| `description` | `varchar` |  | Description of the screen |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `parent_id` | `int` |  | FK to parent GIS_Screen (for nested/tabbed screens) |  |
| `is_maintainable` | `tinyint` |  | Flag: screen can be edited in screen designer | 0,1 |
| `gis_data_model_id` | `int` |  | FK to GIS_Data_Model this screen belongs to |  |
| `screen_type` | `tinyint` |  | Screen type code (data entry, display, etc.) |  |
| `screen_height` | `int` |  | Height of the screen in pixels |  |
| `screen_width` | `int` |  | Width of the screen in pixels |  |
| `product_option` | `int` |  | Product option flags |  |
| `risk_type_rule_set_type_id` | `int` |  | FK to risk_type_rule_set_type |  |
| `file_name_Defaults` | `varchar` |  | Script file for screen defaults |  |
| `file_name_Validation` | `varchar` |  | Script file for screen validation |  |
| `script_defaults` | `text` |  | Inline defaults script for the screen |  |
| `script_dynamic_logic` | `text` |  | Inline dynamic logic script for the screen |  |

*Foreign Keys*:
- `GIS_Screen.gis_data_model_id` â†’ `GIS_Data_Model (gis_data_model_id)` (Many-to-One)
- `GIS_Screen.parent_id` â†’ `GIS_Screen (gis_screen_id)` (Many-to-One)

#### `GIS_Screen_Detail`
**Domain: Policy**  **Owner: IT Department**  **Refresh: Real-time**

Individual control/field placement within a GIS screen — position, caption, object and property binding

*Keywords*: GIS, Screen Detail, Control, Field, Layout, Position

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `gis_screen_id` | `int` |  | FK to GIS_Screen |  |
| `screen_detail_cnt` | `int` |  | Unique detail record identifier (PK) |  |
| `gis_object_id` | `int` |  | FK to GIS_Object this control is bound to |  |
| `gis_property_id` | `int` |  | FK to GIS_Property this control is bound to |  |
| `is_frame` | `tinyint` |  | Flag: control is a container frame | 0,1 |
| `tab_number` | `tinyint` |  | Tab number this control appears on |  |
| `caption` | `varchar` |  | Display label/caption for this control |  |
| `item_top` | `int` |  | Top position in pixels |  |
| `item_left` | `int` |  | Left position in pixels |  |
| `item_height` | `int` |  | Height in pixels |  |
| `item_width` | `int` |  | Width in pixels |  |
| `column_width` | `int` |  | Column width for grid controls |  |
| `pre_quote_requirement` | `tinyint` |  | Flag: field is required before quoting | 0,1 |
| `post_quote_requirement` | `tinyint` |  | Flag: field is required after quoting | 0,1 |
| `purchase_requirement` | `tinyint` |  | Flag: field is required at purchase | 0,1 |
| `parent_id` | `int` |  | FK to parent screen detail (for nested frames/groups) |  |
| `help_text` | `varchar` |  | Help/tooltip text for this control |  |
| `default_object_id` | `int` |  | FK to default GIS_Object |  |
| `default_property_id` | `int` |  | FK to default GIS_Property |  |
| `is_valuation` | `tinyint` |  | Flag: control is a valuation field | 0,1 |
| `is_rate_and_premium` | `tinyint` |  | Flag: control is a rate/premium display field | 0,1 |
| `child_screen_id` | `int` |  | FK to child GIS_Screen (embedded screen) |  |
| `PMFormat` | `int` |  | Polaris format code for display formatting |  |
| `column_position` | `int` |  | Column position in a grid layout |  |
| `tab_set_index` | `int` |  | Tab set index for multi-tab screens |  |
| `data_model_type` | `tinyint` |  | Data model type for this detail control |  |

*Foreign Keys*:
- `GIS_Screen_Detail.gis_screen_id` â†’ `GIS_Screen (gis_screen_id)` (Many-to-One)
- `GIS_Screen_Detail.gis_object_id` â†’ `GIS_Object (gis_object_id)` (Many-to-One)
- `GIS_Screen_Detail.gis_property_id` â†’ `GIS_Property (gis_property_id)` (Many-to-One)
- `GIS_Screen_Detail.child_screen_id` â†’ `GIS_Screen (gis_screen_id)` (Many-to-One)

#### `Insurance_File_System`
**Domain: Policy**  **Owner: Underwriting Department**  **Refresh: Real-time**

System-level metadata for an insurance file tracking the last transaction posted against a policy

*Keywords*: Insurance File, Last Transaction, System, Policy Status

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `insurance_file_cnt` | `int` |  | FK/PK to Insurance_File |  |
| `endorsement_count` | `int` |  | Total number of endorsements processed on this policy |  |
| `created_by_id` | `smallint` |  | FK to PMUser who created the record |  |
| `date_created` | `datetime` |  | Date the insurance file system record was created |  |
| `modified_by_id` | `smallint` |  | FK to PMUser who last modified the record |  |
| `last_modified` | `datetime` |  | Date the record was last modified |  |
| `last_trans_date` | `datetime` |  | Date of the most recent transaction on this policy |  |
| `last_trans_type_id` | `int` |  | FK to Transaction_Type — type of the last transaction |  |
| `last_trans_description` | `varchar` |  | Description of the last transaction |  |
| `last_trans_debit_credit` | `char` |  | Debit or Credit indicator for the last transaction | D,C |
| `last_trans_document_ref` | `varchar` |  | Document reference of the last transaction |  |
| `last_trans_cover_start_date` | `datetime` |  | Cover start date of the last transaction |  |
| `last_trans_expiry_date` | `datetime` |  | Expiry date associated with the last transaction |  |

*Foreign Keys*:
- `Insurance_File_System.insurance_file_cnt` â†’ `Insurance_File (insurance_file_cnt)` (One-to-One)
- `Insurance_File_System.last_trans_type_id` â†’ `Transaction_Type (transaction_type_id)` (Many-to-One)

#### `Peril_Group`
**Domain: Policy**  **Owner: Underwriting**  **Refresh: Admin**

Lookup grouping related peril types together for fee, reserve and allocation purposes. A peril group (e.g. Death, Capital Benefits, PTD) contains one or more peril types via Peril_Type_Usage. Referenced from Fee_Amounts and policy_fee_u to apply fees at the peril group level.

*Keywords*: peril group, peril types, fee group, reserve group, PAPG, DEATH, PTD, peril_group_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `peril_group_id` | `int` | âœ“ | Primary key for the peril group. |  |
| `code` | `varchar` |  | Short code for the peril group (e.g. PAPG, DEATH, PTD, CAPBENEFIT). | PAPG,DEATH,PTD,CAPBENEFIT |
| `description` | `varchar` |  | Description of the peril group (e.g. Death, Capital Benefits). | PA peril Group,Death,Capital Benefits,Permanent Total Disablement |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this peril group is effective. |  |

#### `Peril_Type_Usage`
**Domain: Policy**  **Owner: Underwriting**  **Refresh: Admin**

Many-to-many link between Peril_Group and Peril_Type. Defines which peril types belong to each peril group, with an allocation percentage indicating how to distribute amounts across perils within the group.

*Keywords*: peril type usage, peril group membership, allocation percentage, peril_group_id, peril_type_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `peril_group_id` | `int` | âœ“ | Foreign key to Peril_Group — the group this peril type belongs to. |  |
| `peril_type_id` | `int` | âœ“ | Foreign key to Peril_Type — the peril type within the group. |  |
| `allocate_percent` | `numeric` |  | Percentage of amount allocated to this peril type within the group (100.00 if sole member). | 100.00,10.00,20.00 |

*Foreign Keys*:
- `Peril_Type_Usage.peril_group_id` â†’ `Peril_Group.peril_group_id` (Many-to-One)
- `Peril_Type_Usage.peril_type_id` â†’ `Peril_Type.peril_type_id` (Many-to-One)

#### `Product_Risk_Type_Group`
**Domain: Policy**  **Owner: Underwriting**  **Refresh: Static**

Junction table linking products to the risk type groups that are permitted for use on policies of that product. Drives which risk types are available when creating a policy.

*Keywords*: product, risk type group, product configuration, allowed risk types, product setup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `product_id` | `int` |  | Foreign key to Product. |  |
| `risk_type_group_id` | `int` |  | Foreign key to Risk_Type_Group — the group allowed for this product. |  |

*Foreign Keys*:
- `Product_Risk_Type_Group.product_id` â†’ `Product.product_id` (Many-to-One)
- `Product_Risk_Type_Group.risk_type_group_id` â†’ `Risk_Type_Group.risk_type_group_id` (Many-to-One)

#### `Renewal_Status`
**Domain: Policy**  **Owner: Operations**  **Refresh: Real-time**

Tracks the renewal processing status of individual policies. Records the renewal insurance file, invite print status, broker transfer status, renewal exception reason, email notification, and critical date. Central table for renewal workflow management.

*Keywords*: renewal status, renewal, policy renewal, invite, renewal exception, broker transfer, email, renewal_status_cnt, renewal workflow

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `renewal_status_cnt` | `int` | âœ“ | Primary key — surrogate counter for the renewal status record. |  |
| `product_id` | `int` |  | Foreign key to Product — the product of the policy being renewed. |  |
| `renewal_status_type_id` | `int` |  | Foreign key to Renewal_Status_Type — current renewal workflow status (e.g. Invited, Accepted, Declined). |  |
| `insurance_holder_cnt` | `int` |  | Foreign key to Insurance_Folder — the policy folder being renewed. |  |
| `insurance_file_cnt` | `int` |  | Foreign key to Insurance_File — the expiring policy version. |  |
| `lead_agent_cnt` | `int` |  | Foreign key to Party — the lead agent on the policy. |  |
| `date_created` | `datetime` |  | Date the renewal record was created (usually at invite generation time). |  |
| `critical_date` | `datetime` |  | Critical action date for the renewal (e.g. must be actioned by date). |  |
| `is_invite_printed` | `tinyint` |  | Whether the renewal invite has been printed/generated (1=yes). | 0,1 |
| `date_invite_printed` | `datetime` |  | Date and time the renewal invite was printed. |  |
| `renewal_insurance_file_cnt` | `int` |  | Foreign key to Insurance_File — the new renewal policy version created. |  |
| `broker_xfer_status_type_id` | `int` |  | Foreign key to Renewal_Status_Type — the broker transfer/handover status. |  |
| `renewal_exception_reason_id` | `int` |  | Foreign key to Renewal_Exception_Reason — reason if the renewal was excepted from automated processing. |  |
| `renewal_exception_notes` | `varchar` |  | Free-text notes explaining the renewal exception. |  |
| `email_sent` | `tinyint` |  | Whether a renewal email notification has been sent (1=yes). | 0,1 |
| `email_sent_date` | `datetime` |  | Date and time the renewal email was sent. |  |
| `created_by_id` | `smallint` |  | Foreign key to PMUser — the user who created this renewal record. |  |

*Foreign Keys*:
- `Renewal_Status.renewal_status_type_id` â†’ `Renewal_Status_Type.renewal_status_type_id` (Many-to-One)
- `Renewal_Status.broker_xfer_status_type_id` â†’ `Renewal_Status_Type.renewal_status_type_id` (Many-to-One)
- `Renewal_Status.renewal_exception_reason_id` â†’ `Renewal_Exception_Reason.renewal_exception_reason_id` (Many-to-One)

#### `Risk_Folder`
**Domain: Policy**  **Owner: Underwriting**  **Refresh: Real-time**

Groups related risks into a folder linked to an insurance folder. Supports risk management workflows where multiple risks are managed as a unit under a client folder.

*Keywords*: risk folder, risk grouping, insurance folder, risk management, risk_folder_cnt

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `risk_folder_cnt` | `int` | âœ“ | Primary key — surrogate counter for the risk folder record. |  |
| `risk_folder_id` | `int` |  | Business identifier for the risk folder. |  |
| `source_id` | `int` |  | Foreign key to Source — the branch this risk folder belongs to. |  |
| `risk_folder_type_id` | `int` |  | Foreign key to Risk_Folder_Type — classifies the type of risk folder. |  |
| `code` | `varchar` |  | Reference code for the risk folder. |  |
| `description` | `varchar` |  | Description of the risk folder. |  |
| `insurance_folder_cnt` | `int` |  | Foreign key to Insurance_Folder — the client policy folder this risk folder is linked to. |  |

*Foreign Keys*:
- `Risk_Folder.insurance_folder_cnt` â†’ `Insurance_Folder.insurance_folder_cnt` (Many-to-One)
- `Risk_Folder.risk_folder_type_id` â†’ `Risk_Folder_Type.risk_folder_type_id` (Many-to-One)
- `Risk_Folder.source_id` â†’ `Source.source_id` (Many-to-One)

#### `Risk_Status`
**Domain: Policy**  **Owner: Underwriting**  **Refresh: Static**

Lookup defining the lifecycle status of a risk: e.g. Live, Cancelled, Lapsed. Applied to individual risk records to reflect their current state.

*Keywords*: risk status, live, cancelled, lapsed, risk lifecycle, lookup, risk_status_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `risk_status_id` | `int` | âœ“ | Primary key for the risk status. |  |
| `code` | `char` |  | Short code for the risk status. | LIV,CAN,LAP |
| `description` | `varchar` |  | Description of the risk status (e.g. Live, Cancelled, Lapsed). | Live,Cancelled,Lapsed |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this status is effective. |  |

#### `Risk_Type_Group`
**Domain: Policy**  **Owner: Underwriting**  **Refresh: Static**

Lookup defining named groups of risk types used to configure product rules, RI models, and tax groups. Examples: Private Motor, Commercial Property, Casualty.

*Keywords*: risk type group, risk grouping, product rule, RI model, tax group, classification, risk_type_group_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `risk_type_group_id` | `int` | âœ“ | Primary key for the risk type group. |  |
| `code` | `char` |  | Short code identifying the risk type group. | PVTMTR,CMPROP |
| `description` | `varchar` |  | Human-readable description of the group. | Private Motor,Commercial Property |
| `caption_id` | `int` |  | Foreign key to caption/label resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag (1 = deleted). | 0,1 |
| `effective_date` | `datetime` |  | Date from which this group is effective. |  |

#### `Risk_Type_Rating_Section_Type`
**Domain: Policy**  **Owner: Underwriting**  **Refresh: Static**

Junction table linking risk types to the rating section types that are valid for them. Controls which premium rating sections (e.g. Material Damage, BI, Liability) can appear on a policy for a given risk type.

*Keywords*: risk type, rating section type, valid sections, premium sections, cover sections, product configuration

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `risk_type_id` | `int` |  | Foreign key to Risk_Type — the risk type being configured. |  |
| `rating_section_type_id` | `int` |  | Foreign key to Rating_Section_Type — a valid rating section type for this risk type. |  |

*Foreign Keys*:
- `Risk_Type_Rating_Section_Type.risk_type_id` â†’ `Risk_Type.risk_type_id` (Many-to-One)
- `Risk_Type_Rating_Section_Type.rating_section_type_id` â†’ `Rating_Section_Type.rating_section_type_id` (Many-to-One)

#### `Risk_Type_Usage`
**Domain: Policy**  **Owner: Underwriting**  **Refresh: Static**

Junction table linking risk types to risk type groups. Determines which risk types belong to each group for product configuration, RI model selection, and reporting.

*Keywords*: risk type, risk type group, risk grouping, risk classification, product configuration

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `risk_type_id` | `int` |  | Foreign key to Risk_Type. |  |
| `risk_type_group_id` | `int` |  | Foreign key to Risk_Type_Group — the group this risk type belongs to. |  |

*Foreign Keys*:
- `Risk_Type_Usage.risk_type_id` â†’ `Risk_Type.risk_type_id` (Many-to-One)
- `Risk_Type_Usage.risk_type_group_id` â†’ `Risk_Type_Group.risk_type_group_id` (Many-to-One)

#### `Risk_group`
**Domain: Policy**  **Owner: Underwriting**  **Refresh: Static**

Defines named risk groups used to classify risks into high-level product categories (e.g. Private Motor, Commercial Property). Associates risks with ABI codes, GIS screens, country, and FSA product classification. Links to Brokerlink policy types.

*Keywords*: risk group, product category, ABI code, motor, property, FSA product, GIS screen, country, risk_group_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `risk_group_id` | `int` | âœ“ | Primary key for the risk group. |  |
| `code` | `char` |  | Short code for the risk group. | PVTMTR,CMPROP |
| `description` | `varchar` |  | Description of the risk group. | Private Motor,Commercial Property |
| `abi_code` | `varchar` |  | ABI (Association of British Insurers) product code for regulatory reporting. |  |
| `gis_screen_id` | `int` |  | Foreign key to GIS_Screen — the quote/new business screen for this group. |  |
| `post_quote_gis_screen_id` | `int` |  | Foreign key to GIS_Screen — the post-quote screen for this group. |  |
| `FSA_Product_id` | `int` |  | Foreign key to FSA product classification for FCA reporting. |  |
| `Country_id` | `int` |  | Foreign key to Country — the country this risk group is applicable in. |  |
| `Midnight_Renewal` | `tinyint` |  | Whether renewals for this group are processed at midnight. | 0,1 |
| `Brokerlink_Policy_Type_id` | `int` |  | Foreign key to Brokerlink_Policy_Type for Brokerlink EDI integration. |  |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this group is effective. |  |

#### `agent_commission`
**Domain: Underwriting**  **Owner: Underwriting Department**  **Refresh: Real-time**

Commission record for an agent earned on a specific insurance file transaction. Stores rate, gross and net commission amounts, account references and posting status. Created when a policy transaction is processed.

*Keywords*: agent commission, commission, broker commission, rate, gross commission, net commission, agent fee, policy transaction, posting

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `insurance_file_cnt` | `int` |  | Policy for which commission is received |  |
| `is_lead_agent` | `tinyint` |  | Is it a Broker or SubAgent |  |
| `party_cnt` | `int` |  | Broker or SubAgent Link |  |
| `risk_type_id` | `int` |  | Risk Type associated with the Commission |  |
| `commission_band_id` | `int` |  | Commission Band Link |  |
| `premium` | `numeric` |  | Policy Premium  |  |
| `commission_percentage` | `numeric` |  | Commission Percent |  |
| `commission_value` | `numeric` |  | Commission Amount |  |
| `tax_group_id` | `int` |  | Tax Group Linking |  |
| `tax_amount` | `money` |  | Tax on commission |  |
| `agent_commission_cnt` | `int` |  | Agent Commission Unique identifier |  |
| `is_value` | `tinyint` |  | Is commission_percentage a fixed amount or rate |  |
| `peril_type_id` | `int` |  | linked Peril |  |
| `class_of_business_id` | `int` |  | linked Class of Business |  |

*Foreign Keys*:
- `agent_commission.peril_type_id` â†’ `peril_type(peril_type_id)` (Many-to-One)
- `agent_commission.class_of_business_id` â†’ `class_of_business(class_of_business_id)` (Many-to-One)
- `agent_commission.insurance_file_cnt` â†’ `insurance_file (insurance_file_cnt)` (Many-to-One)
- `agent_commission.tax_group_id` â†’ `tax_group(tax_group_id)` (Many-to-One)
- `agent_commission.party_cnt` â†’ `party(party_cnt)` (Many-to-One)
- `agent_commission.risk_type_id` â†’ `risk_type(risk_type_id)` (Many-to-One)
- `agent_commission.commission_band_id` â†’ `commission_band(commission_band_id)` (Many-to-One)

#### `insurance_file`
**Domain: Underwriting**  **Owner: Underwriting Department**  **Refresh: Real-time**

A specific transactional version of a policy: New Business (NB), Renewal (REN), Mid-Term Adjustment (MTA), or Cancellation (CANC). Holds cover start/end dates, sum insured, premium, insured party, lead agent, product, status, and branch. The primary policy table.

*Keywords*: policy, insurance file, policy version, cover dates, cover start, cover end, premium, sum insured, new business, NB, renewal, REN, MTA, mid-term adjustment, cancellation, CANC, insured, lead agent, quote, insurance_file_cnt

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `insurance_file_cnt` | `int` |  | Unique policy version identifier | 1,2,3,4,34343, 32434 |
| `insurance_ref` | `varchar` |  | Policy Number or Quote Number | POL1234555, POL3213123, POL23123123 |
| `insured_cnt` | `int` |  | Insurer or Client attached to a policy |  |
| `lead_agent_cnt` | `int` |  | Broker or Agent Attached with a policy |  |
| `inception_date` | `date` |  | inception date of a policy |  |
| `cover_start_date` | `date` |  | policy cover start date for each version |  |
| `expiry_date` | `date` |  | policy cover expiry date for each version |  |
| `insurance_file_type_id` | `int` |  | type of version (New Business quotation, MTA Quation, Live MTA, Live Policy etc) |  |
| `insurance_file_status_id` | `int` |  | status of version (live, cancelled, lapsed), NULL means Live |  |
| `source_id` | `int` |  | Branch where the policy belong to |  |
| `currency_id` | `int` |  | Transaction Currency |  |
| `product_id` | `int` |  | Insurance Product  |  |

*Foreign Keys*:
- `insurance_file.insurance_folder_cnt` â†’ `insurance_folder (insurance_folder_cnt)` (Many-to-One)
- `insurance_file.insured_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `insurance_file.lead_agent_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `insurance_file.insurance_file_type_id` â†’ `insurance_file_type (insurance_file_type_id)` (Many-to-One)
- `insurance_file.insurance_file_status_id` â†’ `insurance_file_status (insurance_file_status_id)` (Many-to-One)
- `insurance_file.source_id` â†’ `insurance_file (insurance_file_cnt)` (Many-to-One)
- `insurance_file.currency_id` â†’ `currency(currency_id)` (Many-to-One)
- `insurance_file.product_id` â†’ `product(product_id)` (Many-to-One)

#### `insurance_file_agent`
**Domain: Policy**  **Owner: Underwriting Department**  **Refresh: Real-time**

Associates Sub agents (parties) with an insurance file and records their commission split percentage and amount

*Keywords*: Insurance File, Sub-Agent, Commission, Percentage, Split

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `insurance_file_cnt` | `int` |  | FK/PK to Insurance_File |  |
| `party_cnt` | `int` |  | FK/PK to Party (agent) |  |
| `percentage` | `numeric` |  | Commission split percentage allocated to this agent |  |
| `amount` | `numeric` |  | Commission amount allocated to this agent |  |

*Foreign Keys*:
- `insurance_file_agent.insurance_file_cnt` â†’ `Insurance_File (insurance_file_cnt)` (Many-to-One)
- `insurance_file_agent.party_cnt` â†’ `Party (party_cnt)` (Many-to-One)

#### `insurance_file_risk_link`
**Domain: Underwriting**  **Owner: Risk Department**  **Refresh: Real-time**

Links risks to insurance files/policy versions. Records which risks are covered on each policy version with a changed/unchanged flag indicating whether the risk was modified in the current transaction.

*Keywords*: insurance file risk link, policy risk, risk link, changed, unchanged, MTA, policy cover, risk on policy

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `insurance_file_cnt` | `int` |  | link to Policy |  |
| `risk_cnt` | `int` |  | link to Risk |  |
| `status_flag` | `varchar` |  | Status of Risk whether C(changed) or U (unchanged) |  |
| `original_risk_cnt` | `int` |  | link to previous version of Risk |  |

*Foreign Keys*:
- `insurance_file_risk_link.insurance_file_cnt` â†’ `insurance_file (insurance_file_cnt)` (Many-to-One)
- `insurance_file_risk_link.risk_cnt` â†’ `risk (risk_cnt)` (Many-to-One)

#### `insurance_file_status`
**Domain: Underwriting**  **Owner: Underwriting Department**  **Refresh: Real-time**

Lookup defining the lifecycle status of a policy/insurance file: Live, Cancelled, Renewed, Lapsed, Quote, On-Risk.

*Keywords*: policy status, insurance file status, live, cancelled, renewed, lapsed, quote, on-risk, lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `code` | `char` |  | insurance_file_status code which describes whether a policy is Cancelled, Lapsed, Under Renewal, Replaced etc | CAN,LAP,REN,REP,REPDRI,REPPT,REPBDMTA   |
| `description` | `varchar` |  | insurance_file_status code which describes whether a policy is Cancelled, Lapsed, Under Renewal, Replaced etc | Cancelled,Lapsed,Under Renewal,Replaced,Replaced - Deferred Reinsurance,
Replaced - Portfolio Transfer, Replaced: Backdated Endorsement |

#### `insurance_file_type`
**Domain: Underwriting**  **Owner: Underwriting Department**  **Refresh: Real-time**

Lookup defining the transaction type of an insurance file: New Business (NB), Renewal (REN), Mid-Term Adjustment (MTA), Cancellation (CANC), Quote.

*Keywords*: policy type, insurance file type, new business, NB, renewal, REN, MTA, mid-term adjustment, cancellation, CANC, quote, endorsement, lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `insurance_file_type_id` | `int` |  | Unique policy type identifier |  |
| `code` | `char` |  | insurance_file_type code which describes whether it's a quote, MTA quote, Live Policy, Live MTA, Renewal Quote | QUOTE,POLICY,RENEWAL,MTAQUOTE,MTA PERM ,MTA TEMP,MTAQTETEMP,MTACAN,MTAREINS,MTAQREINS,WRITTEN,MTAQCAN |
| `description` | `varchar` |  | insurance_file_type description which describes whether it's a quote, MTA quote, Live Policy, Live MTA, Renewal Quote | Quotation,Live Policy,Policy under Renewal,MTA Quotation Permanent,MTA Permanent,MTA Temporary,MTA Quotation Temporary,MTA Cancelled,MTA Reinstated,MTA Quotation Reinstatement,Written,MTA Quotation Cancellation |

#### `insurance_folder`
**Domain: Underwriting**  **Owner: Underwriting Department**  **Refresh: Real-time**

Client policy folder that groups all versions of a policy throughout its lifecycle — new business, renewals, mid-term adjustments, and cancellations. A folder has one active insurance file at any time. Top-level entity for policy enquiries.

*Keywords*: policy folder, insurance folder, client folder, policy grouping, policy lifecycle, policy enquiry, insurance_folder_cnt

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `insurance_folder_cnt` | `int` |  | Unique policy folder identifier | 1,2,3,4,34343, 32434 |
| `code` | `varchar` |  | policy folder number | POL1234555, POL3213123, POL23123123 |
| `source_id` | `int` |  | Policy Branch | 1,2,3,4,34343, 32434 |

#### `mta_insurance_file_link`
**Domain: Policy**  **Owner: Underwriting Department**  **Refresh: Real-time**

Tracks mid-term adjustment (MTA) relationships between original, cancelled, and new insurance files

*Keywords*: MTA, Mid-Term Adjustment, Insurance File, Endorsement, Policy Change

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `insurance_file_cnt` | `int` |  | FK/PK to Insurance_File (the MTA policy) |  |
| `sequence_number` | `int` |  | Sequence number for multiple MTA records on same policy (PK) |  |
| `type_ind` | `smallint` |  | Type indicator of the MTA adjustment |  |
| `processed_ind` | `tinyint` |  | Flag: MTA has been processed | 0,1 |
| `original_linked_insurance_file_cnt` | `int` |  | Policy ID of the original (pre-MTA) insurance file |  |
| `cancelled_linked_insurance_file_cnt` | `int` |  | Policy ID of the cancelled insurance file during MTA |  |
| `new_linked_insurance_file_cnt` | `int` |  | Policy ID of the new insurance file created by MTA |  |
| `original_insurance_file_status_id` | `int` |  | Status of the original insurance file before MTA |  |
| `IsDirty` | `bit` |  | Flag: record has unsaved/pending changes | 0,1 |

*Foreign Keys*:
- `mta_insurance_file_link.insurance_file_cnt` â†’ `Insurance_File (insurance_file_cnt)` (Many-to-One)
- `mta_insurance_file_link.original_linked_insurance_file_cnt` â†’ `Insurance_File (insurance_file_cnt)` (Many-to-One)
- `mta_insurance_file_link.cancelled_linked_insurance_file_cnt` â†’ `Insurance_File (insurance_file_cnt)` (Many-to-One)
- `mta_insurance_file_link.new_linked_insurance_file_cnt` â†’ `Insurance_File (insurance_file_cnt)` (Many-to-One)

#### `peril`
**Domain: Underwriting**  **Owner: Risk Department**  **Refresh: Real-time**

A peril (cover section) attached to a risk on an insurance file. Records the covered peril type, sum insured, premium and risk reference. Perils are the lowest level of cover detail on a policy.

*Keywords*: peril, cover, section, sum insured, premium, insured peril, risk cover, perils on policy, peril_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `risk_cnt` | `int` |  | Peril's link to Risk |  |
| `rating_section_id` | `int` |  | Peril's link to Rating Section |  |
| `peril_id` | `int` |  | Unique identifier |  |
| `peril_type_id` | `int` |  | Peril Type link |  |
| `class_of_business_id` | `int` |  | Class of Business link |  |
| `sequence_number` | `int` |  | Sequence Number |  |
| `description` | `varchar` |  | Peril Description |  |
| `sum_insured` | `numeric` |  | Sum Insured |  |
| `rating_sum_insured` | `numeric` |  | Rating Sum Insured |  |
| `rate_type_id` | `int` |  | Rate Type Link |  |
| `annual_rate` | `numeric` |  | Rate |  |
| `annual_premium` | `numeric` |  | Annual Premium |  |
| `this_premium` | `numeric` |  | Pro rated Premium |  |
| `ri_band` | `int` |  | RI Band link |  |

*Foreign Keys*:
- `peril.risk_cnt` â†’ `risk (risk_cnt)` (Many-to-One)
- `peril.peril_type_id` â†’ `peril_type(peril_type_id)` (Many-to-One)

#### `peril_type`
**Domain: Underwriting**  **Owner: Risk Department**  **Refresh: Real-time**

Lookup defining the type of peril/cover: Fire, Theft, Flood, Third Party Liability, Accidental Damage, Business Interruption etc.

*Keywords*: peril type, fire, theft, flood, accidental damage, third party liability, business interruption, cover type, lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `peril_type_id` | `int` |  | Peril Type Unique Identifer |  |
| `code` | `char` |  | Peril Type Code |  |
| `description` | `varchar` |  | Peril Type Description |  |

#### `product`
**Domain: Underwriting**  **Owner: Underwriting Department**  **Refresh: Real-time**

Insurance product definition (line of business). Each policy belongs to a product that defines available risk types, rating sections, perils, GIS screens, documents, RI models and workflow rules.

*Keywords*: product, insurance product, line of business, motor, property, liability, product configuration, combined, LOB, product_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `product_id` | `int` |  | Product Unique ID |  |
| `code` | `char` |  | Product Code |  |
| `description` | `varchar` |  | Product Description |  |

#### `rating_section`
**Domain: Underwriting**  **Owner: Risk Department**  **Refresh: Real-time**

Premium and sum insured breakdown for one rating section of a risk on an insurance file. Contains calculated premium, sum insured, rate, and earned/unearned amounts for a specific cover section.

*Keywords*: rating section, premium breakdown, sum insured, rate, earned premium, unearned premium, cover section, risk premium, rating

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `risk_cnt` | `int` |  | Rating Section's link to Risk |  |
| `rating_section_id` | `int` |  | Unique identifier |  |
| `rating_section_type_id` | `int` |  | Rating Section Type link |  |
| `sum_insured` | `numeric` |  | Sum Insured |  |
| `annual_premium` | `numeric` |  | Annual Premium |  |
| `this_premium` | `numeric` |  | Pro rated Premium |  |
| `original_flag` | `tinyint` |  | Whether it's an old premium (if 1) or new premium (if 0) |  |

*Foreign Keys*:
- `rating_section.risk_cnt` â†’ `risk (risk_cnt)` (Many-to-One)
- `rating_section.rating_section_type_id` â†’ `rating_section_type (rating_section_type_id)` (Many-to-One)

#### `rating_section_type`
**Domain: Underwriting**  **Owner: Risk Department**  **Refresh: Real-time**

Lookup defining the type/category of a rating section (e.g. Material Damage, Business Interruption, Liability). Controls how premiums are calculated and reported.

*Keywords*: rating section type, material damage, business interruption, liability, premium section, cover section type, lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `rating_section_type_id` | `int` |  | Rating Section Type Unique identifier |  |
| `code` | `char` |  | Rating Section Type Code |  |
| `description` | `varchar` |  | Rating Section Type Description |  |

#### `risk`
**Domain: Underwriting**  **Owner: Risk Department**  **Refresh: Real-time**

An insured risk — the subject of insurance on an insurance file. Represents what is being insured: property, vehicle, person, liability etc. One insurance file may cover multiple risks. Holds risk-level data and links to risk_type.

*Keywords*: risk, insured risk, subject of insurance, property, vehicle, liability, person, risk details, sum insured, risk_cnt, insured item

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `risk_cnt` | `int` |  | Unique risk identifier |  |
| `risk_status_id` | `int` |  | Risk Status Id |  |
| `risk_type_id` | `int` |  | Risk Type ID |  |
| `description` | `varchar` |  | risk description |  |
| `total_sum_insured` | `numeric` |  | risk total sum insured |  |
| `total_annual_premium` | `numeric` |  | risk total annual premium |  |
| `total_this_premium` | `numeric` |  | risk total this premium |  |
| `risk_number` | `int` |  | risk_number |  |
| `risk_folder_cnt` | `int` |  | risk folder grouping |  |

*Foreign Keys*:
- `risk.risk_type_id` â†’ `risk_type(risk_type_id)` (Many-to-One)

#### `risk_type`
**Domain: Underwriting**  **Owner: Risk Department**  **Refresh: Real-time**

Lookup defining the type of risk being insured (e.g. Motor Private, Property Commercial, Employers Liability). Controls which GIS data model, rating sections, perils and RI models apply to policies of that type.

*Keywords*: risk type, motor, property, liability, employers liability, risk category, product risk, line of business, risk_type_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `risk_type_id` | `int` |  | risk type id |  |
| `code` | `char` |  | risk type code |  |
| `description` | `varchar` |  | risk type description |  |

#### `risk_type_rule_set`
**Domain: Policy**  **Owner: Underwriting**  **Refresh: Static**

Defines a rating/underwriting rule set (script) associated with a risk type. Contains the quote/validation logic, DRE (Decision Rules Engine) integration settings, and PRE (Pure Rating Engine) configuration. Controls how premiums are calculated for a risk type.

*Keywords*: rule set, rating engine, quote script, underwriting rules, DRE, PRE, rating script, premium calculation, risk type, validation, risk_type_rule_set_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `risk_type_rule_set_id` | `int` | âœ“ | Primary key for the rule set. |  |
| `risk_type_id` | `int` |  | Foreign key to Risk_Type — the risk type this rule set applies to. |  |
| `code` | `char` |  | Short code for the rule set. |  |
| `description` | `varchar` |  | Description of the rule set. |  |
| `risk_type_rule_set_type_id` | `int` |  | Foreign key to risk_type_rule_set_type — the type of this rule set. |  |
| `file_name` | `varchar` |  | Script file name associated with this rule set. |  |
| `live` | `tinyint` |  | Whether this rule set is the active/live version (1=live). | 0,1 |
| `type` | `varchar` |  | Type classifier for the rule set (e.g. script type). |  |
| `dre_executor_url` | `varchar` |  | URL of the DRE (Decision Rules Engine) executor for external rating. |  |
| `dre_default` | `tinyint` |  | Whether DRE is the default rating engine for this rule set. | 0,1 |
| `dre_quote` | `tinyint` |  | Whether DRE is used at quote stage. | 0,1 |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this rule set is effective. |  |

*Foreign Keys*:
- `risk_type_rule_set.risk_type_id` â†’ `Risk_Type.risk_type_id` (Many-to-One)
- `risk_type_rule_set.risk_type_rule_set_type_id` â†’ `risk_type_rule_set_type.risk_type_rule_set_type_id` (Many-to-One)

#### `risk_type_rule_set_type`
**Domain: Policy**  **Owner: Underwriting**  **Refresh: Static**

Lookup classifying the type of a rule set associated with a risk type: e.g. Rating, Validation, GIS Screen logic. Controls how and when a rule set is invoked.

*Keywords*: rule set type, rating type, validation, GIS screen, script type, rule set classification, lookup, risk_type_rule_set_type_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `risk_type_rule_set_type_id` | `int` | âœ“ | Primary key for the rule set type. |  |
| `code` | `varchar` |  | Short code for the rule set type. | RATE,VAL,GIS |
| `description` | `varchar` |  | Description of the rule set type (e.g. Rating, Validation, GIS Screen). | Rating,Validation,GIS Screen |
| `file_name` | `varchar` |  | Default script file name for this type of rule set. |  |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this type is effective. |  |

---

## Claims Management

**Tables in this module**: 17

### Query Rules

**All Claim Transactions** â€” `documenttype.code in ('CLO','CLR','CLP','CLA')`  
*Get All Claims Transactions*

**Claims for a Policy Version** â€” `claim.policy_id = insurance_file.insurance_file_cnt`  
*Claims are linked to a specific policy version via claim.policy_id. To get all claims for a policy (across all versions), join to insurance_file on insurance_ref and retrieve all matching insurance_file_cnt values.*

**Open Claims (Not Yet Settled)** â€” `claim.date_closed IS NULL`  
*An open claim is one where the claim has not been closed/settled. date_closed IS NULL indicates the claim is still in progress. Use this filter for outstanding reserve reports and claims workload.*

**Closed / Settled Claims** â€” `claim.date_closed IS NOT NULL`  
*A settled claim has a date_closed populated. Use this for claims development, average days to settle, and historical loss analysis.*

**Claims Opened (FNOL) in a Date Range** â€” `claim.date_opened BETWEEN @start_date AND @end_date`  
*Returns claims first reported within a date range using date_opened (the FNOL date). Used for claims frequency reports by period.*

**Claims for a Specific Risk** â€” `claim.risk_type_id = risk.risk_cnt`  
*A claim can be linked to the risk it relates to via claim.risk_type_id. Join to risk on risk_cnt to get risk details for the claim.*

### Tables

#### `Catastrophe_Code`
**Domain: Claims**  **Owner: Underwriting**  **Refresh: Static**

Defines named catastrophe events (e.g. Hurricane Ivan, UK Winter Snow) with a code, date range, affected region and associated primary/secondary cause. Used to tag claims as part of a catastrophe for accumulation and RI reporting.

*Keywords*: catastrophe, CAT code, hurricane, storm, flood, catastrophe event, accumulation, claims, RI, catastrophe_code_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `catastrophe_code_id` | `int` | âœ“ | Primary key for the catastrophe event. |  |
| `code` | `char` |  | Short code for the catastrophe event (e.g. IVAN, UKWINTER). | IVAN,UKWINTER |
| `description` | `varchar` |  | Description of the catastrophe event (e.g. Hurricane Ivan). | Hurricane Ivan,UK Winter Snow Dec 2010 |
| `from_date` | `datetime` |  | Start date of the catastrophe event. |  |
| `to_date` | `datetime` |  | End date of the catastrophe event. |  |
| `claims_catastrophe_region_id` | `int` |  | Foreign key to Claims_Catastrophe_Region — the geographical region affected. |  |
| `primary_cause_id` | `int` |  | Foreign key to primary_cause — the main cause of loss for this catastrophe (e.g. Flood, Storm). |  |
| `secondary_cause_id` | `int` |  | Foreign key to secondary_cause — the secondary cause of loss for this catastrophe. |  |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this catastrophe code is effective. |  |

*Foreign Keys*:
- `Catastrophe_Code.claims_catastrophe_region_id` â†’ `Claims_Catastrophe_Region.claims_catastrophe_region_id` (Many-to-One)
- `Catastrophe_Code.primary_cause_id` â†’ `primary_cause.primary_cause_id` (Many-to-One)
- `Catastrophe_Code.secondary_cause_id` â†’ `secondary_cause.secondary_cause_id` (Many-to-One)

#### `Claims_Rating_Agency`
**Domain: Claims**  **Owner: Claims Department**  **Refresh: Real-time**

Lookup: external claims rating agency codes

*Keywords*: Claims, Rating, Agency, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `Claims_Rating_Agency_id` | `int` |  | Unique agency identifier (PK) |  |
| `code` | `char` |  | Short code |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `description` | `varchar` |  | Description |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

#### `Primary_Cause_Risk_Type_Group`
**Domain: Claims**  **Owner: Underwriting**  **Refresh: Static**

Links primary causes of loss (e.g. Fire, Storm, Theft) to risk type groups, controlling which causes of loss are available for claims on particular risk types.

*Keywords*: primary cause, risk type group, cause of loss, claims, peril cause, claim cause mapping

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `primary_cause_risk_type_group_id` | `int` | âœ“ | Primary key. |  |
| `primary_cause_id` | `int` |  | Foreign key to primary_cause — the cause of loss (e.g. Fire, Storm, Theft). |  |
| `risk_type_group_id` | `int` |  | Foreign key to Risk_Type_Group — risk types in this group can have the linked primary cause. |  |

*Foreign Keys*:
- `Primary_Cause_Risk_Type_Group.primary_cause_id` â†’ `primary_cause.primary_cause_id` (Many-to-One)
- `Primary_Cause_Risk_Type_Group.risk_type_group_id` â†’ `Risk_Type_Group.risk_type_group_id` (Many-to-One)

#### `Recovery_type`
**Domain: Claims**  **Owner: Claims**  **Refresh: Static**

Lookup defining the types of claim recovery: Salvage, Third Party, Recovery of Excess, Subrogation, Recovery of Property. Used to classify incoming recoveries against a claim. Salvage and property recoveries are flagged with is_salvage.

*Keywords*: recovery type, salvage, subrogation, third party, recovery of excess, claims recovery, Recovery_type_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `Recovery_type_id` | `int` | âœ“ | Primary key for the recovery type. |  |
| `code` | `char` |  | Short code for the recovery type (e.g. SALVAGE, THIRDPARTY, XS, SUB, PROP). | SALVAGE,THIRDPARTY,XS,SUB,PROP |
| `Description` | `varchar` |  | Description of the recovery type (e.g. Salvage, Third Party, Subrogation). | Salvage,Third Party,Subrogation,Recovery of Excess |
| `is_salvage` | `tinyint` |  | Whether this recovery type is a salvage/property recovery (1=yes). | 0,1 |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this recovery type is effective. |  |

#### `Reserve_type`
**Domain: Claims**  **Owner: Claims**  **Refresh: Static**

Lookup defining the types of claims reserves that can be held against a claim. Each type is categorised as excess, indemnity or expense, and flagged for inclusion in total reserve calculations. Examples: Excess, Indemnity, Fees, Medical Expenses.

*Keywords*: reserve type, claims reserve, excess, indemnity, expense, medical, claims financials, Reserve_type_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `Reserve_type_id` | `int` | âœ“ | Primary key for the reserve type. |  |
| `name` | `varchar` |  | Short name/code for the reserve type (e.g. Excess, Indemnity, MED, HOSP). | Excess,Indemnity,MED,HOSP |
| `Description` | `varchar` |  | Full description of the reserve type. | Excess,Indemnity,Fees,Medical Expenses |
| `Include_in_Total` | `bit` |  | Whether this reserve type is included in the total reserve calculation (1=yes). | 0,1 |
| `Is_Excess` | `tinyint` |  | Whether this reserve type represents an excess component (1=yes). | 0,1 |
| `is_indemnity` | `tinyint` |  | Whether this reserve type represents an indemnity component (1=yes). | 0,1 |
| `is_expense` | `tinyint` |  | Whether this reserve type represents an expense component (1=yes). | 0,1 |

#### `claim`
**Domain: Claim**  **Owner: Claim Department**  **Refresh: Real-time**

A claim record representing a loss event against an insurance policy. Holds claim reference, reported date, loss date, loss description, claim status, incident details, linked insured party and handler. Each claim version has associated claim_peril, reserve, claim_payment and recovery records.

*Keywords*: claim, loss, incident, claim reference, loss date, reported date, claim status, reserve, payment, claims, claimant, claim_cnt, claims management

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `claim_id` | `int` |  | Claim Unique Identifier |  |
| `policy_id` | `int` |  | Policy ID linked with Claim |  |
| `policy_number` | `varchar` |  | Policy Number linked with Claim |  |
| `claim_number` | `varchar` |  | Claim Number |  |
| `description` | `varchar` |  | User Description |  |
| `claim_status_id` | `int` |  | Claim Status ID linked with Claim |  |
| `progress_status_id` | `int` |  | Progress Status ID linked with Claim |  |
| `primary_cause_id` | `int` |  | Primary Cause ID linked with Claim |  |
| `secondary_cause_id` | `int` |  | Seondary Cause ID linked with Claim |  |
| `catastrophe_code_id` | `int` |  | Catastrophe Code linked with Claim |  |
| `loss_from_date` | `datetime` |  | Loss From Date |  |
| `loss_to_date` | `datetime` |  | Loss To Date |  |
| `reported_date` | `datetime` |  | Claim Reported Date |  |
| `last_modified_date` | `datetime` |  | Last Modified Date |  |
| `handler_id` | `int` |  | Claim Handler Id |  |
| `currency_id` | `int` |  | Claim Loss Currency Id |  |
| `info_only` | `Boolean` |  | Is it a Claim notification onky |  |
| `likely_claim` | `Boolean` |  | Is it Likely to be a full Claim |  |
| `location` | `varchar` |  | Claim location |  |
| `town` | `int` |  | Claim town |  |
| `risk_type_id` | `int` |  | Risk linked with the Claim |  |
| `base_claim_id` | `int` |  | ID of first Claim Version to link all of them together |  |

*Foreign Keys*:
- `claim.policy_id` â†’ `insurance_file (insurance_file_cnt)` (Many-to-One)
- `claim.risk_type_id` â†’ `risk (risk_cnt)` (Many-to-One)
- `claim.base_claim_id` â†’ `claim(claim_id)` (Many-to-One)
- `claim.currency_id` â†’ `currency(currency_id)` (Many-to-One)

#### `claim_payment`
**Domain: Claim**  **Owner: Claim Department**  **Refresh: Real-time**

A claim payment transaction — money paid out to a claimant, solicitor, or third party under a claim. Holds payment amount, currency, payee details, cheque/BACS reference, payment status and link to the claim version.

*Keywords*: claim payment, payment, claimant, third party, settlement, cheque, BACS, payment amount, payment status, claim disbursement, loss payment

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `claim_payment_id` | `int` |  | Claim Payment Unique Identifier |  |
| `claim_id` | `int` |  | Claim linked with Claim Payment |  |
| `claim_peril_id` | `int` |  | Claim Peril linked with Claim Payment |  |
| `date_of_payment` | `datetime` |  | Payment Date |  |
| `amount` | `money` |  | Claim Payment Amount |  |
| `tax_amount` | `money` |  | Tax on Claim Payment Amount |  |
| `party_cnt` | `int` |  | Party to which payment is made (if any) |  |
| `comments` | `varchar` |  | comments |  |
| `is_referred` | `tinyint` |  | Is Payment due to approval |  |

*Foreign Keys*:
- `claim_payment.claim_id` â†’ `claim(claim_id)` (Many-to-One)
- `claim_payment.claim_peril_id` â†’ `claim_peril(claim_peril_id)` (Many-to-One)
- `claim_payment.party_cnt` â†’ `party(party_cnt)` (Many-to-One)

#### `claim_payment_item`
**Domain: Claim**  **Owner: Claim Department**  **Refresh: Real-time**

Individual line of a claim payment broken down by reserve type (indemnity, fees, expenses). Allows a single payment to span multiple reserve categories.

*Keywords*: claim payment item, indemnity, fees, expenses, reserve type, payment breakdown, payment allocation, claim payment detail

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `claim_payment_item_id` | `int` |  | Claim Payment Breakup Unique Identifier |  |
| `claim_payment_id` | `int` |  | Claim Payment Link |  |
| `reserve_id` | `int` |  | Reserve ID for which payment is done |  |
| `currency_id` | `smallint` |  | Payment Currency |  |
| `tax_group_id` | `int` |  | Tax Group Linking |  |
| `this_payment` | `money` |  | Payment amount |  |
| `tax_amount` | `money` |  | Tax on Payment amount |  |

*Foreign Keys*:
- `claim_payment_item.claim_payment_id` â†’ `claim_payment(claim_payment_id)` (Many-to-One)
- `claim_payment_item.reserve_id` â†’ `reserve(reserve_id)` (Many-to-One)
- `claim_payment_item.currency_id` â†’ `currency(currency_id)` (Many-to-One)
- `claim_payment_item.tax_group_id` â†’ `tax_group(tax_group_id)` (Many-to-One)

#### `claim_peril`
**Domain: Claim**  **Owner: Claim Department**  **Refresh: Real-time**

A peril (cover type) attached to a specific claim version. Represents what is being claimed under which cover. Records notified, outstanding and paid amounts by reserve type. Each claim has one or more claim_peril records.

*Keywords*: claim peril, claim cover, reserve, outstanding reserve, notified, paid, claim version, peril type, indemnity, claim peril detail

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `claim_peril_id` | `int` |  | Claim Peril Unique ID |  |
| `claim_id` | `int` |  | Claim linked with Perils |  |
| `peril_type_id` | `int` |  | Type of Peril |  |
| `description` | `varchar` |  | Description |  |
| `comments` | `varchar` |  | Comments |  |
| `sum_insured` | `numeric` |  | Sum Insured |  |
| `version_id` | `int` |  | Version Number |  |

*Foreign Keys*:
- `claim_peril.claim_id` â†’ `claim(claim_id)` (Many-to-One)
- `claim_peril.peril_type_id` â†’ `peril_type(peril_type_id)` (Many-to-One)

#### `claim_receipt`
**Domain: Claim**  **Owner: Claim Department**  **Refresh: Real-time**

A claim receipt — money received as part of a recovery (from a third party, salvage, or subrogation). Records receipt amount, source party, bank details, allocation status and claim reference.

*Keywords*: claim receipt, recovery receipt, salvage receipt, third party, subrogation, receipt amount, recovery income, claim recovery receipt

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `claim_receipt_id` | `int` |  | Claim Recovery Receipt Unique Identifier |  |
| `claim_id` | `int` |  | Claim linked to the Recovery Receipt  |  |
| `claim_peril_id` | `int` |  | Claim Peril linked to the Recovery Receipt  |  |
| `date_of_receipt` | `datetime` |  | Recovery Receipt Date |  |
| `party_cnt` | `int` |  | Party to which Recovery Receipt amount is paid |  |
| `Amount` | `money` |  | Amount Recovered |  |
| `tax_amount` | `money` |  | Tax on Amount Recovered |  |
| `comments` | `varchar` |  | Comments |  |
| `version_id` | `int` |  | Version Number |  |

*Foreign Keys*:
- `claim_receipt.claim_id` â†’ `claim(claim_id)` (Many-to-One)
- `claim_receipt.claim_peril_id` â†’ `claim_peril(claim_peril_id)` (Many-to-One)
- `claim_receipt.party_cnt` â†’ `party(party_cnt)` (Many-to-One)

#### `claim_receipt_item`
**Domain: Claim**  **Owner: Claim Department**  **Refresh: Real-time**

Individual line of a claim receipt broken down by recovery/reserve type (indemnity, fees, expenses, RI recovery).

*Keywords*: claim receipt item, recovery type, salvage, third party, subrogation, receipt breakdown, recovery breakdown, receipt allocation

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `claim_receipt_item_id` | `int` |  | Claim Recovery Receipt Breakup Unique Identifier |  |
| `claim_receipt_id` | `int` |  | Claim Recovery Receipt linked |  |
| `recovery_id` | `int` |  | Recovery against which Claim Recovery Receipt is made |  |
| `recovery_type_id` | `int` |  | Type of Claim Recovery Receipt is made |  |
| `reserve_id` | `int` |  | Linked Reserve |  |
| `currency_id` | `smallint` |  | Claim Recovery Receipt Currency |  |
| `this_receipt` | `money` |  | Amount Recovered |  |
| `tax_group_id` | `int` |  | Tax Group linked |  |
| `tax_amount` | `money` |  | Tax on Amount Recovered |  |

*Foreign Keys*:
- `claim_receipt_item.claim_receipt_id` â†’ `claim_receipt(claim_receipt_id)` (Many-to-One)
- `claim_receipt_item.recovery_id` â†’ `recovery(recovery_id)` (Many-to-One)
- `claim_receipt_item.recovery_type_id` â†’ `recovery_type(recovery_type_id)` (Many-to-One)
- `claim_receipt_item.reserve_id` â†’ `reserve(reserve_id)` (Many-to-One)
- `claim_receipt_item.currency_id` â†’ `currency(currency_id)` (Many-to-One)
- `claim_receipt_item.tax_group_id` â†’ `tax_group(tax_group_id)` (Many-to-One)

#### `peril_type_reserve_type`
**Domain: Claims**  **Owner: Claims**  **Refresh: Admin**

Maps which reserve types are applicable to each peril type, and flags which reserve type is the main reserve for that peril. Used by the claims module to determine the reserve categories available when setting reserves on a claim peril.

*Keywords*: peril type reserve, claims reserve, peril reserve mapping, is_main_reserve, peril_type_id, Reserve_type_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `peril_type_reserve_type_id` | `int` | âœ“ | Primary key for the peril-reserve type link. |  |
| `peril_type_id` | `int` |  | Foreign key to Peril_Type — the peril type this reserve mapping applies to. |  |
| `Reserve_type_id` | `int` |  | Foreign key to Reserve_type — the reserve type that can be used for this peril. |  |
| `is_main_reserve` | `tinyint` |  | Whether this reserve type is the primary/main reserve for this peril type (1=yes). | 0,1 |

*Foreign Keys*:
- `peril_type_reserve_type.Reserve_type_id` â†’ `Reserve_type.Reserve_type_id` (Many-to-One)
- `peril_type_reserve_type.peril_type_id` â†’ `Peril_Type.peril_type_id` (Many-to-One)

#### `primary_cause`
**Domain: Claims**  **Owner: Operations**  **Refresh: Static**

Lookup of primary causes of loss used on claims: e.g. Bodily Injury, Fire, Flood, Storm, Theft, Malicious Damage. Maps to ABI cause of loss codes for regulatory reporting.

*Keywords*: primary cause, cause of loss, fire, theft, flood, storm, bodily injury, ABI code, claims cause, peril cause, primary_cause_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `primary_cause_id` | `int` | âœ“ | Primary key for the primary cause of loss. |  |
| `code` | `char` |  | Short code for the cause (e.g. FIRE, STORM, BICOL). | FIRE,STORM,THEFT,FLOOD,BIDEATH |
| `description` | `varchar` |  | Description of the primary cause (e.g. Fire, Storm, Bodily Injury - Death). | Fire,Storm,Bodily Injury - Collision with an animal |
| `ABI_List_Code` | `char` |  | ABI (Association of British Insurers) regulatory list code for this cause of loss. |  |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this cause is effective. |  |

#### `progress_status`
**Domain: Claims**  **Owner: Claims Department**  **Refresh: Static**

Lookup table for claim progress statuses — the current state in the claims workflow (e.g. OPEN, CLOSED). is_closed_check_status=1 means this status counts as "closed" for reporting. is_claim_payment_valid=1 means claims with this status can still have payment transactions posted to them.

*Keywords*: claim, progress, status, open, closed, workflow, payment valid

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `progress_status_id` | `int` | âœ“ | PK — unique identifier for this claim progress status. | 1 (Closed), 2 (Open) |
| `caption_id` | `int` |  | FK to PMCaption — multilingual display label for this status. | 716, 5900 |
| `code` | `char(10)` |  | Short code for this status. e.g. CLOSED, OPEN. Used in claim queries to filter by status. | CLOSED, OPEN |
| `description` | `varchar(50)` |  | Human-readable description of this status shown in the UI. | Closed, Open |
| `is_deleted` | `tinyint` |  | 0 = active, 1 = soft-deleted. | 0, 1 |
| `effective_date` | `datetime` |  | Date from which this status is effective. |  |
| `is_closed_check_status` | `tinyint` |  | 1 = this status is treated as "closed" for reporting purposes (e.g. CLOSED=1, OPEN=0). | 0, 1 |
| `is_claim_payment_valid` | `tinyint` |  | 1 = claim payments can still be posted when the claim has this status. 0 = no further payments allowed (e.g. for closed/settled claims). | 0, 1 |

#### `recovery`
**Domain: Claim**  **Owner: Claim Department**  **Refresh: Real-time**

Recovery estimate record against a claim peril. Represents expected money to be recovered from third parties, salvage sales, or reinsurance. Tracks notified and outstanding recovery amounts by recovery type.

*Keywords*: recovery, salvage, third party recovery, subrogation, recovery estimate, outstanding recovery, recovery type, claim recovery, salvage recovery

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `recovery_id` | `int` |  | Salvage or 3rd Party Recovery Unique ID |  |
| `claim_Peril_id` | `int` |  | Claim Peril linked with Recovery |  |
| `recovery_type_id` | `int` |  | Type of Recovery Link |  |
| `currency_id` | `int` |  | Recovery Currency |  |
| `initial_reserve` | `Currency` |  | Initial Recovery Estimate |  |
| `revised_reserve` | `Currency` |  | Amount revised to the total Recovery Estimate |  |
| `received_to_date` | `Currency` |  | Amount Received to date against this Recovery Estimate |  |
| `revision_count` | `int` |  | Number of times estimates were revised |  |
| `tax_amount` | `Currency` |  | Total tax collected against Amount Received  |  |
| `version_id` | `int` |  | Version Number |  |
| `recovery_party_type_id` | `int` |  | Type of Recovery Party (1 - Agent,2 - Client,3 -Insurer, 4 - Other Party) |  |
| `recovery_party_cnt` | `int` |  | Recovery Party link |  |
| `this_receipt_net` | `Currency` |  | Total Net Amount Received against this Recovery |  |

*Foreign Keys*:
- `recovery.claim_Peril_id` â†’ `claim_peril(claim_peril_id)` (Many-to-One)
- `recovery.recovery_type_id` â†’ `recovery_type(recovery_type_id)` (Many-to-One)
- `recovery.currency_id` â†’ `currency(currency_id)` (Many-to-One)

#### `reserve`
**Domain: Claim**  **Owner: Claim Department**  **Refresh: Real-time**

Financial reserve (estimate) against a claim peril. Tracks outstanding reserve amounts by reserve type (indemnity, fees, expenses). Updated by claim handlers as the claim progresses. Drives financial reporting of total claims liability.

*Keywords*: reserve, claim reserve, outstanding, indemnity, fees, expenses, IBNR, reserve type, estimate, claims liability, outstanding reserve

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `reserve_id` | `int` |  | Reserve Unique ID |  |
| `claim_peril_id` | `int` |  | Claim Peril attached with the Reserve/Estimate |  |
| `reserve_type_id` | `int` |  | Type of Estimate/Reserve |  |
| `initial_reserve` | `Currency` |  | Initial Estimate/Reserve |  |
| `paid_to_date` | `Currency` |  | Payment made to date against this estimate |  |
| `revised_reserve` | `Currency` |  | Amount revised to the total Estimate/Reserve |  |
| `sum_insured` | `Currency` |  | Sum Insured against this reserve/estimate |  |
| `revision_count` | `int` |  | Number of times estimates were revised |  |
| `this_revision` | `numeric` |  | Amount revised in current version of claim |  |
| `this_payment` | `numeric` |  | Payment made in current version of claim |  |

*Foreign Keys*:
- `reserve.claim_peril_id` â†’ `claim_peril(claim_peril_id)` (Many-to-One)
- `reserve.reserve_type_id` â†’ `reserve_type(reserve_type_id)` (Many-to-One)

#### `secondary_cause`
**Domain: Claims**  **Owner: Operations**  **Refresh: Static**

Lookup of secondary/sub-causes of loss linked to a primary cause. Provides additional detail on the cause of a claim (e.g. under Fire: Arson, Electrical Fault).

*Keywords*: secondary cause, sub-cause, cause of loss, arson, electrical fault, claims, claim cause detail, secondary_cause_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `secondary_cause_id` | `int` | âœ“ | Primary key for the secondary cause of loss. |  |
| `code` | `char` |  | Short code for the secondary cause (e.g. Arson, Elec). | Arson,Elec,Water |
| `description` | `varchar` |  | Description of the secondary cause (e.g. Arson, Electrical Fault). | Arson,Electrical Fault |
| `primary_cause_id` | `int` |  | Foreign key to primary_cause — the parent cause this secondary cause belongs to. |  |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this secondary cause is effective. |  |

*Foreign Keys*:
- `secondary_cause.primary_cause_id` â†’ `primary_cause.primary_cause_id` (Many-to-One)

---

## Finance Management

**Tables in this module**: 61

### Query Rules

**Outstanding Premium** â€” `outstaning_amount<>0 `  
*Checking the Outstanding Premium*

**All Policy Transactions** â€” `documenttype.code in ('SND','SEC','SED','SRD','SID')`  
*Get All Policy Transactions*

**All Receipt Transactions** â€” `documenttype.code in ('SRP')`  
*Get All Receipt Transactions *

**All Payment Transactions** â€” `documenttype.code in ('SPY')`  
*Get All Payment Transactions *

**Receipt for a Policy** â€” `Get records from AllocationDetail for Allocation_id for transdetail id selected for Policy and then filter on SRP`  
*Get Receipt Details for a Policy*

**Transaction in Base Currency** â€” `Use transdetail.base_amount for currency-neutral totals; transdetail.amount is in transaction currency`  
*transdetail.amount holds the amount in the transaction currency. For reports requiring a single currency, use transdetail.base_amount which holds the amount converted to the system base currency. Always use base_amount for totals across multi-currency portfolios.*

**Outstanding Premium on a Transaction** â€” `transdetail.outstanding_amount <> 0  -- also filter by documenttype for policy debits only`  
*transdetail.outstanding_amount holds the unpaid balance of a single debit transaction line. Zero means fully paid/allocated. Non-zero means the broker/client still owes money. Sum across all lines for total outstanding on a policy or account.*

**Account Balance for a Party (Broker or Client)** â€” `account.account_key = party.party_cnt  AND SUM(transdetail.outstanding_amount) per account_id`  
*A party's financial account is linked via account.account_key = party.party_cnt. The net balance is the sum of all outstanding_amount values on transdetail for that account. A positive balance means the party is a debtor (owes money to the insurer).*

### Tables

#### `AllocationBatch`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Daily**

Records each batch cash allocation run — grouping multiple individual allocation transactions processed together in one operation. Linked to the accounting period. Supports reversal: a batch can be reversed by creating a new batch and linking it via reversed_allocation_batch_id.

*Keywords*: allocation, batch, cash, payment, receipt, reversal, period

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `allocationbatch_id` | `int` | âœ“ | PK — unique identifier for this allocation batch run. | 1, 2, 3 |
| `allocationbatch_date` | `datetime` |  | Date and time when this allocation batch was processed. | 2011-10-27, 2011-11-08 |
| `period_id` | `int` |  | FK to Period — the accounting period this batch is posted to. | 34, 35 |
| `is_reversed` | `bit` |  | 1 = this batch has been reversed. 0 or NULL = active batch. | 0, 1, NULL |
| `reversed_allocation_batch_id` | `int` |  | FK to AllocationBatch (self-join) — the later batch that reversed this one. NULL if not reversed. |  |

*Foreign Keys*:
- `AllocationBatch.period_id` â†’ `Period` (Many-to-One)
- `AllocationBatch.reversed_allocation_batch_id` â†’ `AllocationBatch` (Self-join)

#### `BankAccount_Source`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: On Change**

Link table — associates bank accounts with business sources (channels). Controls which bank account is used for receipts originating from a particular source/channel. A bank account may serve one or many sources.

*Keywords*: bank account, source, channel, receipt, mapping

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `bankaccount_id` | `int` |  | FK to bank account — the bank account linked to a source. (No direct FK constraint; links to bank account table). |  |
| `source_id` | `int` |  | FK to Source — the business source/channel that uses this bank account for receipts. |  |

*Foreign Keys*:
- `BankAccount_Source.source_id` â†’ `Source` (Many-to-One)

#### `Bank_Payment_Type`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Lookup: bank payment method types (e.g. BACS, CHAPS)

*Keywords*: Bank, Payment, BACS, CHAPS, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `bank_payment_type_id` | `int` |  | Unique payment type identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `description` | `varchar` |  | Description of the payment type |  |
| `code` | `varchar` |  | Short code (e.g. BACS, CHAPS, CARD) |  |

#### `CashListItem_Claim_Link`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Links a cash list item to a claim payment or claim receipt

*Keywords*: CashList, Claim, Payment, Receipt, Link

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `cashlistitem_claim_link_id` | `int` |  | Unique record identifier (PK) |  |
| `claim_payment_id` | `int` |  | FK to Claim_Payment (if linked to a payment) |  |
| `claim_receipt_id` | `int` |  | FK to Claim_Receipt (if linked to a receipt) |  |
| `cashlistitem_id` | `int` |  | FK to cashlistitem |  |
| `is_deleted` | `int` |  | Soft delete flag | 0,1 |

*Foreign Keys*:
- `CashListItem_Claim_Link.claim_payment_id` â†’ `Claim_Payment (claim_payment_id)` (Many-to-One)
- `CashListItem_Claim_Link.claim_receipt_id` â†’ `Claim_Receipt (claim_receipt_id)` (Many-to-One)
- `CashListItem_Claim_Link.cashlistitem_id` â†’ `cashlistitem (cashlistitem_id)` (Many-to-One)

#### `CashListItem_Instalments`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Links a cash list item receipt to a premium finance instalment

*Keywords*: CashList, Instalments, Receipt, Premium Finance

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `cashlistitem_id` | `int` |  | FK to cashlistitem |  |
| `pfinstalments_id` | `int` |  | FK to PFInstalments |  |

*Foreign Keys*:
- `CashListItem_Instalments.cashlistitem_id` â†’ `cashlistitem (cashlistitem_id)` (Many-to-One)
- `CashListItem_Instalments.pfinstalments_id` â†’ `PFInstalments (pfinstalments_id)` (Many-to-One)

#### `Chase_Cycle_Step`
**Domain: Finance**  **Owner: Finance**  **Refresh: Admin**

Defines the individual steps within a chase cycle rule. Each step has a sequence number, number of days from the previous step, an optional document template to send, an optional workflow task to raise, the user group responsible, and flags for whether to check/trigger auto-cancellation. Steps are linked as a sequence via next_step/previous_step.

*Keywords*: chase cycle step, chase step, auto-cancel, document template, workflow task, user group, step sequence, step_number, chase_cycle_step_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `chase_cycle_step_id` | `int` | âœ“ | Primary key for the chase cycle step. |  |
| `chase_cycle_rule_id` | `int` |  | Foreign key to Chase_cycle_rule — the rule this step belongs to. |  |
| `step_number` | `smallint` |  | Sequence number of this step within the rule (e.g. 1, 2, 3). | 1,2,3 |
| `number_of_days` | `smallint` |  | Number of days after the previous step before this step triggers. | 14,30 |
| `step_description` | `varchar` |  | Description of what this step does (e.g. Chase for licence details). | Chase for licence details |
| `document_template_id` | `int` |  | Foreign key to Document_Template — the letter/document to generate at this step (optional). |  |
| `pmwrk_task_id` | `int` |  | Foreign key to PMWrk_Task — the workflow task to raise at this step (optional). |  |
| `pmwrk_task_group_id` | `int` |  | Foreign key to PMWrk_Task_Group — the workflow activity queue to assign the task to. |  |
| `pmuser_group_id` | `int` |  | Foreign key to PMUser_Group — the user group responsible for actioning this step. |  |
| `check_auto_cancel` | `tinyint` |  | Whether to check if auto-cancellation criteria are met at this step. | 0,1 |
| `auto_cancel_policy` | `tinyint` |  | Whether to automatically cancel the policy if criteria are met at this step. | 0,1 |
| `next_step` | `smallint` |  | Step number of the next step in the sequence (NULL if this is the last step). |  |
| `previous_step` | `smallint` |  | Step number of the preceding step (NULL if this is the first step). |  |

*Foreign Keys*:
- `Chase_Cycle_Step.chase_cycle_rule_id` â†’ `Chase_cycle_rule.chase_cycle_rule_id` (Many-to-One)
- `Chase_Cycle_Step.document_template_id` â†’ `Document_Template.document_template_id` (Many-to-One)
- `Chase_Cycle_Step.pmuser_group_id` â†’ `PMUser_Group.pmuser_group_id` (Many-to-One)
- `Chase_Cycle_Step.pmwrk_task_group_id` â†’ `PMWrk_Task_Group.pmwrk_task_group_id` (Many-to-One)

#### `Chase_cycle_item`
**Domain: Finance**  **Owner: Finance**  **Refresh: Real-time**

Active chase cycle records — one per policy/risk currently in a chasing sequence. Records the reason (NB=new business, REN=renewal), the current step, created and due dates, whether a letter has been sent, whether auto-cancellation is permitted/will occur, and the responsible user/group. This is the operational table tracking outstanding chases.

*Keywords*: chase cycle item, credit control, outstanding chase, auto-cancel, due date, letter sent, NB, REN, insurance folder, chase_cycle_item_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `chase_cycle_item_id` | `int` | âœ“ | Primary key for the active chase cycle item. |  |
| `insurance_folder_cnt` | `int` |  | Foreign key to Insurance_Folder — the policy folder being chased. |  |
| `Insurance_file_cnt` | `int` |  | Foreign key to Insurance_File — the specific policy version being chased. |  |
| `risk_cnt` | `int` |  | Foreign key to Risk — the risk being chased. |  |
| `chase_cycle_step_id` | `int` |  | Foreign key to Chase_Cycle_Step — the current step in the chase for this item. |  |
| `chase_cycle_reason` | `varchar` |  | Reason code for the chase: NB=New Business, REN=Renewal. | NB,REN |
| `created_date` | `datetime` |  | Date the chase item was created. |  |
| `due_date` | `datetime` |  | Date by which the chase must be resolved or will escalate to the next step. |  |
| `letter_sent` | `tinyint` |  | Whether a chase letter has been sent for the current step (1=yes). | 0,1 |
| `can_auto_cancel` | `tinyint` |  | Whether this item is eligible for auto-cancellation if unresolved. | 0,1 |
| `will_auto_cancel` | `tinyint` |  | Whether auto-cancellation will be triggered at the next processing run. | 0,1 |
| `pmuser_group_id` | `int` |  | Foreign key to PMUser_Group — the user group currently responsible for actioning this chase. |  |
| `pmuser_id` | `int` |  | Foreign key to PMUser — the individual user assigned to this chase item. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag (1=closed/resolved). | 0,1 |

*Foreign Keys*:
- `Chase_cycle_item.insurance_folder_cnt` â†’ `Insurance_Folder.insurance_folder_cnt` (Many-to-One)
- `Chase_cycle_item.Insurance_file_cnt` â†’ `Insurance_File.insurance_file_cnt` (Many-to-One)
- `Chase_cycle_item.risk_cnt` â†’ `Risk.risk_cnt` (Many-to-One)
- `Chase_cycle_item.chase_cycle_step_id` â†’ `Chase_Cycle_Step.chase_cycle_step_id` (Many-to-One)
- `Chase_cycle_item.pmuser_group_id` â†’ `PMUser_Group.pmuser_group_id` (Many-to-One)
- `Chase_cycle_item.pmuser_id` â†’ `PMUser.user_id` (Many-to-One)

#### `Chase_cycle_rule`
**Domain: Finance**  **Owner: Finance**  **Refresh: Admin**

Defines a credit control / document-chasing rule. Each rule targets a specific product, branch (source), and GIS data model/property combination. It specifies how many processing days to allow before triggering a chase, whether to use the effective date, and whether to include cancelled policies. Drives automatic generation of Chase_Cycle_Item records.

*Keywords*: chase cycle rule, credit control, chasing rule, processing days, effective date, product, source, GIS, cancelled policies, chase_cycle_rule_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `chase_cycle_rule_id` | `int` | âœ“ | Primary key for the chase cycle rule. |  |
| `Description` | `varchar` |  | Description of the chase cycle rule (e.g. Licence Type). | Licence Type |
| `product_id` | `int` |  | Foreign key to Product — the product this chase rule applies to. |  |
| `source_id` | `int` |  | Foreign key to Source — the branch/source this chase rule applies to. |  |
| `Gis_data_model_id` | `int` |  | Foreign key to GIS_Data_Model — the data model/section to monitor for the chase trigger value. |  |
| `Gis_property_id` | `int` |  | Foreign key to GIS_Property — the specific field whose value triggers the chase. |  |
| `chase_cycle_status_udl_value_id` | `int` |  | The UDL value ID that represents the status triggering this chase (e.g. a specific licence status). |  |
| `is_active` | `tinyint` |  | Whether this chase rule is currently active (1=yes). | 0,1 |
| `processing_days` | `smallint` |  | Number of days allowed before the first chase step is triggered. | 14,30,60 |
| `use_effective_date` | `tinyint` |  | Whether to use the policy effective date (rather than inception date) when calculating the chase trigger date. | 0,1 |
| `use_greater_of_transaction_and_effective_date` | `tinyint` |  | Whether to use whichever is later — the transaction date or effective date — as the chase start date. | 0,1 |
| `Include_cancelled_policies` | `tinyint` |  | Whether to include cancelled policies in this chase cycle. | 0,1 |
| `Cancelled_only` | `tinyint` |  | Whether this chase rule applies only to cancelled policies. | 0,1 |

*Foreign Keys*:
- `Chase_cycle_rule.product_id` â†’ `Product.product_id` (Many-to-One)
- `Chase_cycle_rule.source_id` â†’ `Source.source_id` (Many-to-One)

#### `Commission_Arrangement`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: On Change**

Defines the commission rate (% or flat value) payable to a broker or agent by product, risk type, and transaction type. Supports banded rates via commission_band_id and tax group linkage. A party_cnt of 0 means a default/global rate; non-zero targets a specific broker or agent. The is_value flag distinguishes percentage rate from a flat monetary amount.

*Keywords*: commission, broker, agent, rate, product, transaction type, arrangement

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `commission_arrangement_id` | `int` | âœ“ | PK (identity) — unique identifier for this commission arrangement record. |  |
| `Party_type` | `int` |  | FK to Party_Type — the type of agent/broker this arrangement applies to. 0 = applies to all party types. | 0 (all), specific party type id |
| `party_cnt` | `int` |  | FK to Party — the specific broker/agent this rate applies to. 0 = default rate for all agents. | 0 (default), specific party_cnt |
| `Product_id` | `int` |  | FK to Product — the product this commission rate applies to. 0 = all products. | 0 (all), specific product_id |
| `risk_type_id` | `int` |  | FK to Risk type — the risk type this rate applies to. 0 = all risk types. | 0 (all), specific risk_type_id |
| `transaction_type_id` | `int` |  | FK to Transaction_Type — the transaction type (NB, renewal, MTA) this rate applies to. 0 = all. | 0 (all), specific transaction_type_id |
| `commission_band_id` | `int` |  | FK to Commission Band — links to a tiered commission band definition if banded rates apply. |  |
| `effective_date` | `datetime` |  | Date from which this commission arrangement is effective. |  |
| `is_deleted` | `tinyint` |  | 0 = active, 1 = soft-deleted/superseded. | 0, 1 |
| `rate` | `numeric` |  | Commission rate — either a percentage (if is_value=0) or a flat monetary amount (if is_value=1). | 10.00, 12.50, 15.00 |
| `is_value` | `tinyint` |  | 0 = rate is a percentage of premium. 1 = rate is a flat monetary value (fixed commission amount). | 0, 1 |
| `commission_grouping` | `int` |  | Groups multiple arrangement lines that are processed together for commission calculation. |  |
| `tax_group_id` | `int` |  | FK to Tax_Group — the tax treatment applied to this commission (e.g. VAT group). |  |
| `Maximum_rate` | `numeric` |  | Maximum commission rate cap — if set, commission cannot exceed this rate/amount even if the arrangement rate is higher. |  |
| `commission_level_id` | `int` |  | FK to Agent_Commission_Level — the commission tier/level classification for this arrangement. |  |
| `UserId` | `int` |  | FK to PMUser — last user to modify this record. |  |
| `UniqueId` | `varchar(50)` |  | System GUID for upgrade/sync. |  |
| `ScreenHierarchy` | `varchar(500)` |  | Navigation path for the admin UI. |  |

*Foreign Keys*:
- `Commission_Arrangement.tax_group_id` â†’ `Tax_Group` (Many-to-One)

#### `Commission_level`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Lookup: commission tier/level codes

*Keywords*: Commission, Level, Tier, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `commission_level_id` | `int` |  | Unique commission level identifier (PK) |  |
| `code` | `varchar` |  | Short code |  |
| `description` | `varchar` |  | Description of the commission level |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |

#### `Credit_Control_Item`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

A credit control debt item tracking outstanding amounts against a policy or instalment

*Keywords*: Credit Control, Debt, Outstanding, Policy, Instalment

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `credit_control_item_id` | `int` |  | Unique credit control item (PK) |  |
| `credit_control_reason` | `varchar` |  | Reason for the credit control item |  |
| `account_id` | `int` |  | FK to account |  |
| `document_id` | `int` |  | FK to document |  |
| `document_date` | `datetime` |  | Date of the associated document |  |
| `insurance_file_cnt` | `int` |  | FK to insurance_file |  |
| `pfprem_finance_cnt` | `int` |  | FK to PFPremiumFinance (plan) |  |
| `pfprem_finance_version` | `int` |  | FK to PFPremiumFinance (version) |  |
| `amount` | `numeric` |  | Outstanding amount on this item |  |
| `can_auto_cancel` | `tinyint` |  | Flag: item can trigger auto-cancel | 0,1 |
| `will_auto_cancel` | `tinyint` |  | Flag: item will auto-cancel on next run | 0,1 |
| `credit_control_step_id` | `int` |  | FK to Credit_Control_Step |  |
| `created_date` | `datetime` |  | Date the item was created |  |
| `due_date` | `datetime` |  | Date the debt is/was due |  |
| `recurrence_count` | `int` |  | Number of times this item has recurred |  |
| `pfinstalments_id` | `int` |  | FK to PFInstalments |  |
| `pmuser_group_id` | `int` |  | FK to PMUser_Group (assigned group) |  |
| `pmuser_id` | `int` |  | FK to PMUser (assigned user) |  |
| `claim_id` | `int` |  | FK to claim (if claim-related) |  |
| `partial_amount` | `numeric` |  | Partial payment received amount |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |

*Foreign Keys*:
- `Credit_Control_Item.account_id` â†’ `account (account_id)` (Many-to-One)
- `Credit_Control_Item.insurance_file_cnt` â†’ `insurance_file (insurance_file_cnt)` (Many-to-One)
- `Credit_Control_Item.pfprem_finance_cnt` â†’ `PFPremiumFinance (pfprem_finance_cnt)` (Many-to-One)
- `Credit_Control_Item.pfinstalments_id` â†’ `PFInstalments (pfinstalments_id)` (Many-to-One)
- `Credit_Control_Item.credit_control_step_id` â†’ `Credit_Control_Step (credit_control_step_id)` (Many-to-One)

#### `Credit_Control_Rule`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Credit control rule definition linking frequency and products to a sequence of steps

*Keywords*: Credit Control, Rule, Frequency, Auto-cancel

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `credit_control_rule_id` | `int` |  | Unique rule identifier (PK) |  |
| `description` | `varchar` |  | Description of the rule |  |
| `source_id` | `int` |  | FK to Source/branch this rule applies to |  |
| `business_type` | `varchar` |  | Business type filter for this rule |  |
| `pffrequency_id` | `int` |  | FK to PFFrequency |  |
| `is_active` | `tinyint` |  | Flag: rule is active | 0,1 |
| `processing_days` | `smallint` |  | Processing days used in step calculations |  |
| `pfinstalments_result_id` | `int` |  | FK to PFInstalments_Result (applicable result) |  |
| `product_id` | `int` |  | FK to product (scope to product) |  |
| `use_due_date` | `tinyint` |  | Flag: use instalment due date for calculations | 0,1 |
| `UserId` | `int` |  | Last modified by user |  |

*Foreign Keys*:
- `Credit_Control_Rule.pffrequency_id` â†’ `PFFrequency (pffrequency_id)` (Many-to-One)
- `Credit_Control_Rule.pfinstalments_result_id` â†’ `PFInstalments_Result (pfinstalments_result_id)` (Many-to-One)
- `Credit_Control_Rule.product_id` â†’ `product (product_id)` (Many-to-One)

#### `Credit_Control_Step`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

A step within a credit control rule defining actions at each stage

*Keywords*: Credit Control, Step, Letters, Auto-cancel, Workflow

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `credit_control_step_id` | `int` |  | Unique step identifier (PK) |  |
| `credit_control_rule_id` | `int` |  | FK to Credit_Control_Rule |  |
| `step_number` | `smallint` |  | Step sequence number within the rule |  |
| `number_of_days` | `smallint` |  | Number of days from due date to trigger this step |  |
| `broker_days` | `smallint` |  | Number of days for broker variant of step |  |
| `client_document_template_id` | `int` |  | FK to Document_Template (client letter) |  |
| `oip_document_template_id` | `int` |  | FK to Document_Template (OIP letter) |  |
| `broker_report_id` | `int` |  | FK to Report (broker report) |  |
| `check_auto_cancel` | `tinyint` |  | Flag: check auto-cancel on this step | 0,1 |
| `auto_cancel_policy` | `tinyint` |  | Flag: automatically cancel policy at this step | 0,1 |
| `pmwrk_task_id` | `int` |  | FK to PMWrk_Task (workflow task triggered) |  |
| `pmuser_group_id` | `int` |  | FK to PMUser_Group (group to assign task to) |  |
| `write_off_reason_id` | `int` |  | FK to Write_Off_Reason |  |
| `instalment_failure_count` | `smallint` |  | Failure count that triggers this step |  |
| `step_description` | `varchar` |  | Free text description of this step |  |
| `UserId` | `int` |  | Last modified by user |  |

*Foreign Keys*:
- `Credit_Control_Step.credit_control_rule_id` â†’ `Credit_Control_Rule (credit_control_rule_id)` (Many-to-One)
- `Credit_Control_Step.client_document_template_id` â†’ `Document_Template (document_template_id)` (Many-to-One)
- `Credit_Control_Step.oip_document_template_id` â†’ `Document_Template (document_template_id)` (Many-to-One)
- `Credit_Control_Step.broker_report_id` â†’ `Report (report_id)` (Many-to-One)
- `Credit_Control_Step.pmwrk_task_id` â†’ `PMWrk_Task (pmwrk_task_id)` (Many-to-One)
- `Credit_Control_Step.pmuser_group_id` â†’ `PMUser_Group (pmuser_group_id)` (Many-to-One)
- `Credit_Control_Step.write_off_reason_id` â†’ `Write_Off_Reason (write_off_reason_id)` (Many-to-One)

#### `CurrencyRate`
**Domain: Finance**  **Owner: Finance**  **Refresh: Regular**

Historical exchange rates for each currency against the system base currency, keyed by currency and effective date. Used to convert foreign currency premiums, payments and financial amounts into the base currency for accounting and reporting.

*Keywords*: currency rate, exchange rate, FX, foreign currency, base currency, effective date, conversion, multi-currency, currency_rate_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `currency_rate_id` | `int` | âœ“ | Primary key for the currency rate record. |  |
| `currency_id` | `smallint` |  | Foreign key to Currency — the currency this rate applies to. |  |
| `company_id` | `smallint` |  | Company/entity for which this rate applies (supports multi-company setups). |  |
| `effective_from` | `datetime` |  | Date from which this exchange rate is effective. |  |
| `rate_against_base` | `numeric` |  | Exchange rate of this currency against the system base currency. | 1.4,1.25,0.85 |
| `UserId` | `int` |  | User ID of the person who last updated this rate. |  |
| `UniqueId` | `varchar` |  | Unique identifier for synchronisation/import purposes. |  |
| `ScreenHierarchy` | `varchar` |  | Screen navigation hierarchy used by the UI for this record. |  |

*Foreign Keys*:
- `CurrencyRate.currency_id` â†’ `Currency.currency_id` (Many-to-One)

#### `Debtor_User_Groups`
**Domain: Finance**  **Owner: Finance**  **Refresh: Admin**

Defines user groups for debtor management workflows (credit control, payment authorisation). Each group has a type, linked PMUser group, branch, and step number controlling the chase sequence. Drives which users receive credit control actions and payment authorisation tasks.

*Keywords*: debtor group, credit control, payment authorisation, user group, chase sequence, step, branch, debtor_user_groups_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `debtor_user_groups_id` | `int` | âœ“ | Primary key for the debtor user group. |  |
| `code` | `varchar` |  | Short code for the group (e.g. GROUP1, PAY1, PAYMENT1). | GROUP1,PAY1,PAYMENT1 |
| `description` | `varchar` |  | Description of the group (e.g. Payments 1, Payment Authorisers). | Payments 1,Payment Authorisers |
| `debtor_user_groups_type_id` | `int` |  | Foreign key to Debtor_User_Groups_Type — classifies the group type. |  |
| `pmuser_group_id` | `int` |  | Foreign key to PMUser_Group — the system user group linked to this debtor group. |  |
| `source_id` | `int` |  | Foreign key to Source — the branch this group operates in. |  |
| `step_number` | `int` |  | Step number in the credit control chase sequence for this group. | 1,2,3 |
| `Is_Payment_Type_Claim_Payment` | `tinyint` |  | Whether this group handles claim payments specifically (1=yes). | 0,1 |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this group is effective. |  |

*Foreign Keys*:
- `Debtor_User_Groups.debtor_user_groups_type_id` â†’ `Debtor_User_Groups_Type.debtor_user_groups_type_id` (Many-to-One)
- `Debtor_User_Groups.pmuser_group_id` â†’ `PMUser_Group.pmuser_group_id` (Many-to-One)
- `Debtor_User_Groups.source_id` â†’ `Source.source_id` (Many-to-One)

#### `Element`
**Domain: Finance**  **Owner: Finance**  **Refresh: Static**

Defines financial reporting elements (accounts, headings, subtotals) that form the building blocks of the chart-of-accounts structure tree.

*Keywords*: element, account, heading, reporting, chart of accounts, financial

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `element_id` | `int` | âœ“ | Primary key for the financial element. |  |
| `element_name` | `char` |  | Name/label for the element (account heading, subtotal line, etc.). | Net Premium Income,Claims Paid |
| `parent_id` | `int` |  | Self-referencing FK to parent element — forms sub-groups/headings. |  |

#### `ElementExtras`
**Domain: Finance**  **Owner: Finance**  **Refresh: Static**

Extended attributes for financial elements including descriptions, mapping references, totalling indicators, and GL export grouping flags.

*Keywords*: element, description, totalling, GL export, report map, account map

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `element_id` | `int` | âœ“ | Primary key / foreign key to Element. |  |
| `totalling_id` | `int` |  | Totalling indicator controlling how subtotals are accumulated. |  |
| `description` | `varchar` |  | Extended description of the element for reporting purposes. |  |
| `report_map_id` | `int` |  | Reference to a report mapping definition for this element. |  |
| `account_map_id` | `int` |  | Reference to an account mapping definition for this element. |  |
| `spare_number` | `int` |  | Spare numeric field for custom reporting purposes. |  |
| `spare_text` | `varchar` |  | Spare text field for custom reporting purposes. |  |
| `is_deletable` | `tinyint` |  | Whether this element can be deleted by users (1=yes). | 0,1 |
| `group_for_gl_export_ind` | `tinyint` |  | Whether this element is grouped for GL export output. | 0,1 |

*Foreign Keys*:
- `ElementExtras.element_id` â†’ `Element.element_id` (One-to-One)

#### `Export_Map_Detail`
**Domain: Finance**  **Owner: Finance**  **Refresh: Static**

Stores individual field mappings within an export map model, defining target field names and their export order sequence.

*Keywords*: export map, field mapping, target field, sequence, GL export

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `export_map_detail_id` | `int` | âœ“ | Primary key for the export map detail entry. |  |
| `export_map_model_id` | `int` |  | Foreign key to Export_Map_Model — the model this detail belongs to. |  |
| `target_field_name` | `varchar` |  | Name of the target field in the export destination. |  |
| `sequence` | `smallint` |  | Order in which this field appears in the export output. | 1,2,3 |

*Foreign Keys*:
- `Export_Map_Detail.export_map_model_id` â†’ `Export_Map_Model.export_map_model_id` (Many-to-One)

#### `Fee_Amounts`
**Domain: Finance**  **Owner: Finance**  **Refresh: Admin**

Defines the fee configuration records used to calculate broker/party fees at policy inception, renewal or MTA. Each record specifies the party (broker), peril group, risk type group, product, transaction type, fee rate (percentage or fixed amount), optional commission, tax group, currency and instalment handling. Referenced by policy_fee_u at the policy level.

*Keywords*: fee amounts, broker fee, fee rate, commission, peril group, risk group, product, transaction type, tax group, instalment, fee_amount_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `fee_amount_id` | `int` | âœ“ | Primary key for the fee amount configuration record. |  |
| `party_cnt` | `int` |  | Foreign key to Party — the broker/agent this fee is configured for. |  |
| `product_id` | `int` |  | Foreign key to Product — the product this fee applies to (null=all products). |  |
| `peril_group_id` | `int` |  | Foreign key to Peril_Group — the peril group this fee applies to. |  |
| `risk_type_group_id` | `int` |  | Foreign key to Risk_Type_Group — the risk type group this fee applies to. |  |
| `transaction_type_id` | `int` |  | Foreign key to Transaction_Type — the transaction type this fee applies to (e.g. NB, REN, MTA). |  |
| `transaction_sub_type` | `tinyint` |  | Sub-type of transaction this fee applies to. |  |
| `currency_id` | `smallint` |  | Foreign key to Currency — the currency this fee rate is defined in. |  |
| `fee_percentage` | `numeric` |  | Fee rate as a percentage of premium (0 if fixed amount). | 0.00,5.00 |
| `fee_amount` | `numeric` |  | Fixed fee amount (0 if percentage-based). | 10.00,20.00,50.00 |
| `commission_percentage` | `numeric` |  | Commission percentage associated with this fee (optional). |  |
| `commission_amount` | `numeric` |  | Fixed commission amount associated with this fee (optional). |  |
| `tax_group_id` | `int` |  | Foreign key to Tax_Group — the tax group applied to this fee. |  |
| `commission_tax_group_id` | `int` |  | Foreign key to Tax_Group — the tax group applied to the commission element. |  |
| `tax_rates_id` | `smallint` |  | Foreign key to Tax_Rates — the specific tax rate applied. |  |
| `is_fee_applied_to_cr` | `tinyint` |  | Whether this fee applies to credit transactions (1=yes). | 0,1 |
| `include_fee_in_instalments` | `tinyint` |  | Whether to include this fee in instalment calculations (1=yes). | 0,1 |
| `spread_fee_across_instalments` | `tinyint` |  | Whether to spread this fee evenly across all instalments (1=yes). | 0,1 |
| `display_on_quotes` | `int` |  | Whether to display this fee on quote documents. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this fee configuration is effective. |  |
| `extra_scheme_id` | `int` |  | Foreign key to Extra_Scheme — optional extra scheme this fee is linked to. |  |
| `FSA_Type_Of_Sale_id` | `int` |  | Foreign key to FSA_Type_Of_Sale — restricts this fee to a specific sale type. |  |
| `extra_amount_basis` | `tinyint` |  | Basis for calculating the extra amount component. |  |
| `Calculation_Basis` | `tinyint` |  | How the fee is calculated (e.g. 0=flat, 1=per section, 2=per risk). | 0,1,2 |
| `Is_Prorated` | `tinyint` |  | Whether the fee is pro-rated for mid-term transactions. | 0,1 |
| `Is_Override` | `tinyint` |  | Whether this fee can be manually overridden on the policy. | 0,1 |
| `Use_When_Deleted` | `tinyint` |  | Whether to still apply this fee even if the configuration is soft-deleted. | 0,1 |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `risk_group_id` | `int` |  | Foreign key to risk group — restricts fee to a specific risk group. |  |
| `MakeLiveOptions_id` | `int` |  | Foreign key to MakeLiveOptions — controls when this fee becomes live. |  |
| `DoPaymentTerms_id` | `int` |  | Foreign key to DOPaymentTerms — payment terms associated with this fee. |  |

*Foreign Keys*:
- `Fee_Amounts.currency_id` â†’ `Currency.currency_id` (Many-to-One)
- `Fee_Amounts.tax_group_id` â†’ `Tax_Group.tax_group_id` (Many-to-One)
- `Fee_Amounts.tax_rates_id` â†’ `Tax_Rates.tax_rates_id` (Many-to-One)
- `Fee_Amounts.risk_type_group_id` â†’ `Risk_Type_Group.risk_type_group_id` (Many-to-One)
- `Fee_Amounts.peril_group_id` â†’ `Peril_Group.peril_group_id` (Many-to-One)
- `Fee_Amounts.extra_scheme_id` â†’ `Extra_Scheme.Extra_Scheme_id` (Many-to-One)
- `Fee_Amounts.FSA_Type_Of_Sale_id` â†’ `FSA_Type_Of_Sale.FSA_Type_Of_Sale_id` (Many-to-One)

#### `LedgerType`
**Domain: Finance**  **Owner: Finance**  **Refresh: Static**

Lookup table of ledger types (e.g. General Ledger, Client Ledger, Insurer Ledger) used to classify financial postings across the system.

*Keywords*: ledger type, GL, client ledger, insurer ledger, posting, finance

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `ledgertype_id` | `smallint` | âœ“ | Primary key for the ledger type. |  |
| `code` | `char` |  | Short code for the ledger type. | GL,CL,IL |
| `description` | `varchar` |  | Description of the ledger type (e.g. General Ledger, Client Ledger). | General Ledger,Client Ledger |
| `caption_id` | `int` |  | Foreign key to caption/label resource. |  |
| `is_deleted` | `bit` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this ledger type is effective. |  |

#### `Mapping`
**Domain: Finance**  **Owner: Finance**  **Refresh: Static**

Defines financial account mappings for a company, controlled by a map type, used to map ledger accounts to reporting elements in StructureTree.

*Keywords*: mapping, company, chart of accounts, map type, financial reporting

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `mapping_id` | `int` | âœ“ | Primary key for the mapping record. |  |
| `company_id` | `int` |  | Company this mapping is defined for. |  |
| `maptype_id` | `smallint` |  | Foreign key to MapType — the type of mapping (e.g. GL, Reporting). |  |
| `description` | `varchar` |  | Description of the mapping definition. |  |

*Foreign Keys*:
- `Mapping.maptype_id` â†’ `MapType.maptype_id` (Many-to-One)

#### `PFEDIDefinition`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Lookup: EDI definition types for premium finance export

*Keywords*: PF EDI, Definition, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pfedidefinition_id` | `int` |  | Unique EDI definition identifier (PK) |  |
| `code` | `varchar` |  | Short code |  |
| `description` | `varchar` |  | Description |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |

#### `PFEDIDefinitionFields`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

EDI file field definitions for a premium finance provider

*Keywords*: PF EDI, Fields, Export, Direct Debit

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pfedidefinition_id` | `int` |  | FK to PFEDIDefinition |  |
| `output_order` | `tinyint` |  | Sequence order in the output file |  |
| `array_index` | `smallint` |  | Array index for repeating fields |  |
| `column_name` | `varchar` |  | Field/column name in the output |  |
| `column_size` | `tinyint` |  | Field size in characters |  |
| `column_type` | `varchar` |  | Data type of the field |  |
| `decimal_accuracy` | `tinyint` |  | Decimal places for numeric fields |  |
| `signed_field` | `varchar` |  | Whether field is signed |  |
| `section` | `varchar` |  | Section of the EDI file |  |

*Foreign Keys*:
- `PFEDIDefinitionFields.pfedidefinition_id` â†’ `PFEDIDefinition (pfedidefinition_id)` (Many-to-One)

#### `PFFrequency`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Lookup: instalment collection frequency codes (monthly, weekly etc.)

*Keywords*: PF Frequency, Instalment, Monthly, Weekly, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pffrequency_id` | `int` |  | Unique frequency identifier (PK) |  |
| `code` | `varchar` |  | Short code |  |
| `description` | `varchar` |  | Description (e.g. Monthly, Weekly) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `period` | `varchar` |  | Period type (Day, Week, Month) |  |
| `amount` | `smallint` |  | Number of periods per frequency |  |
| `is_available_on_client_screen` | `tinyint` |  | Flag: shown on client-facing screen | 0,1 |
| `is_available_on_instalment_screen` | `tinyint` |  | Flag: shown on instalment screen | 0,1 |

#### `PFInstalments`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Individual instalment records for a premium finance plan

*Keywords*: Instalments, Due Date, Amount, Status, Batch

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pfinstalments_id` | `int` |  | Unique instalment record identifier (PK) |  |
| `pfprem_finance_cnt` | `int` |  | FK to PFPremiumFinance (plan) |  |
| `pfprem_finance_version` | `int` |  | FK to PFPremiumFinance (version) |  |
| `InstalmentNumber` | `int` |  | Sequence number of this instalment (1=first) |  |
| `DueDate` | `datetime` |  | Date this instalment is due for collection |  |
| `Amount` | `numeric` |  | Instalment amount (net) |  |
| `Fee` | `numeric` |  | Fee component of this instalment |  |
| `commission` | `numeric` |  | Commission component of this instalment |  |
| `tax` | `numeric` |  | Tax component of this instalment |  |
| `TransactionCode` | `int` |  | FK to PFInstalments_Transaction (type) |  |
| `Status` | `int` |  | FK to PFInstalments_Status |  |
| `BatchNumber` | `int` |  | Batch number for collection export |  |
| `BatchExportDate` | `datetime` |  | Date this instalment was exported in a batch |  |
| `PostedDate` | `datetime` |  | Date the instalment was posted/collected |  |
| `PFTransaction_id` | `int` |  | FK to PFTransaction_Id |  |
| `pfinstalments_result_id` | `int` |  | FK to PFInstalments_Result (outcome of collection) |  |
| `batch_id` | `int` |  | FK to Batch |  |
| `pfmediatype_history_id` | `int` |  | FK to PFMediaTypeHistory (payment method used) |  |
| `failure_count` | `smallint` |  | Number of collection failure attempts |  |
| `write_off_reason_id` | `int` |  | FK to Write_Off_Reason (if written off) |  |
| `write_off_transdetail_id` | `int` |  | FK to transdetail (write-off transaction) |  |
| `original_DueDate` | `datetime` |  | Original due date before any rescheduling |  |

*Foreign Keys*:
- `PFInstalments.pfprem_finance_cnt` â†’ `PFPremiumFinance (pfprem_finance_cnt)` (Many-to-One)
- `PFInstalments.Status` â†’ `PFInstalments_Status (PFInstalments_Status_id)` (Many-to-One)
- `PFInstalments.TransactionCode` â†’ `PFInstalments_Transaction (PFInstalments_Transaction_id)` (Many-to-One)
- `PFInstalments.pfinstalments_result_id` â†’ `PFInstalments_Result (pfinstalments_result_id)` (Many-to-One)
- `PFInstalments.pfmediatype_history_id` â†’ `PFMediaTypeHistory (pfMediaTypeHistory_id)` (Many-to-One)
- `PFInstalments.batch_id` â†’ `Batch (batch_id)` (Many-to-One)
- `PFInstalments.write_off_reason_id` â†’ `Write_Off_Reason (write_off_reason_id)` (Many-to-One)
- `PFInstalments.write_off_transdetail_id` â†’ `transdetail (transdetail_id)` (Many-to-One)

#### `PFInstalments_History`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Audit history of status changes on individual instalments

*Keywords*: Instalments, History, Status, Audit

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pfinstalments_history_id` | `int` |  | Unique history record (PK) |  |
| `pfinstalments_id` | `int` |  | FK to PFInstalments |  |
| `pfinstalments_status_id` | `int` |  | FK to PFInstalments_Status |  |
| `pfinstalments_result_id` | `int` |  | FK to PFInstalments_Result |  |
| `posted_date` | `datetime` |  | Date this history record was posted |  |

*Foreign Keys*:
- `PFInstalments_History.pfinstalments_id` â†’ `PFInstalments (pfinstalments_id)` (Many-to-One)

#### `PFInstalments_Result`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Lookup: collection result codes for an instalment attempt

*Keywords*: Instalment Result, Collection, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pfinstalments_result_id` | `int` |  | Unique result identifier (PK) |  |
| `code` | `varchar` |  | Short code (e.g. OK, FAIL) |  |
| `description` | `varchar` |  | Result description |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |

#### `PFInstalments_Status`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Lookup: instalment status codes (e.g. Pending, Collected, Failed)

*Keywords*: Instalment Status, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `PFInstalments_Status_id` | `int` |  | Unique status identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code (e.g. P=Pending, C=Collected) |  |
| `description` | `varchar` |  | Status description |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

#### `PFInstalments_Transaction`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Lookup: instalment transaction type codes

*Keywords*: Instalment Transaction Type, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `PFInstalments_Transaction_id` | `int` |  | Unique transaction type (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code |  |
| `description` | `varchar` |  | Description |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

#### `PFMediaTypeHistory`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Audit history of payment method changes on a premium finance plan

*Keywords*: Premium Finance, Bank, Card, Payment Method, History

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pfMediaTypeHistory_id` | `int` |  | Unique history record (PK) |  |
| `pfprem_finance_cnt` | `int` |  | FK to PFPremiumFinance (plan) |  |
| `pfprem_finance_version` | `int` |  | FK to PFPremiumFinance (version) |  |
| `mediatype_validation_code` | `varchar` |  | Media type validation code |  |
| `action_code` | `varchar` |  | Type of change action (ADD, EDIT) |  |
| `BankAccountName` | `varchar` |  | Bank account name at time of change |  |
| `BankSortCode` | `varchar` |  | Bank sort code at time of change |  |
| `BankAccountNo` | `varchar` |  | Bank account number at time of change |  |
| `cc_number` | `varchar` |  | Card number (masked) at time of change |  |
| `cc_expiry_date` | `varchar` |  | Card expiry date |  |
| `cardholder_name` | `varchar` |  | Card holder name |  |
| `user_id` | `smallint` |  | FK to PMUser (who made the change) |  |
| `date_modified` | `datetime` |  | Date the change was made |  |
| `business_identifier_code` | `varchar` |  | BIC/SWIFT code |  |
| `international_bank_account_number` | `varchar` |  | IBAN |  |

*Foreign Keys*:
- `PFMediaTypeHistory.pfprem_finance_cnt` â†’ `PFPremiumFinance (pfprem_finance_cnt)` (Many-to-One)
- `PFMediaTypeHistory.user_id` â†’ `PMUser (user_id)` (Many-to-One)

#### `PFPaymentMethod`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Payment method export configuration (file format for direct debit)

*Keywords*: PF Payment Method, Direct Debit, Export

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `PFPaymentMethod_cnt` | `int` |  | Unique payment method identifier (PK) |  |
| `Description` | `varchar` |  | Description of the payment method |  |
| `Directory` | `varchar` |  | Output file directory |  |
| `Filename` | `varchar` |  | Output file name |  |
| `AllowAutoPost` | `tinyint` |  | Flag: allow automatic posting | 0,1 |
| `DisableExport` | `tinyint` |  | Flag: disable file export | 0,1 |
| `mediatype_validation_id` | `int` |  | FK to MediaType_Validation |  |

#### `PFPremiumFinance`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Premium finance plan record linking an insurance file to a finance provider scheme

*Keywords*: Premium Finance, Instalments, APR, Deposit, Plan

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pfprem_finance_cnt` | `int` |  | Unique premium finance plan identifier (PK part 1) |  |
| `pfprem_finance_version` | `int` |  | Version of the premium finance plan (PK part 2) |  |
| `ClientId` | `int` |  | Client party identifier |  |
| `CompanyNo` | `int` |  | FK to PFScheme (provider company number) |  |
| `SchemeNo` | `int` |  | FK to PFScheme (scheme number) |  |
| `SchemeVersion` | `int` |  | FK to PFScheme (scheme version) |  |
| `StartDate` | `datetime` |  | Plan start date |  |
| `EndDate` | `datetime` |  | Plan end date |  |
| `AmountToFinance` | `money` |  | Total amount being financed |  |
| `APR` | `float` |  | Annual Percentage Rate |  |
| `InterestRate` | `float` |  | Flat interest rate |  |
| `NoOfInstallments` | `int` |  | Number of instalments in the plan |  |
| `FirstInstallment` | `money` |  | Amount of the first instalment |  |
| `OthInstallments` | `money` |  | Amount of subsequent instalments |  |
| `Deposit` | `money` |  | Deposit amount |  |
| `NetAmount` | `money` |  | Net premium amount |  |
| `TotalCost` | `money` |  | Total cost of credit |  |
| `InterestCost` | `money` |  | Total interest charged |  |
| `StatusInd` | `varchar` |  | Plan status indicator |  |
| `IsQuote` | `tinyint` |  | Flag: plan is a quote (not live) | 0,1 |
| `IsParentPlan` | `tinyint` |  | Flag: this is a parent plan | 0,1 |
| `Insurance_File_Cnt` | `int` |  | FK to insurance_file |  |
| `source_id` | `int` |  | FK to source |  |
| `pfrf_id` | `int` |  | FK to PFRF (rate file) |  |
| `BankSortCode` | `varchar` |  | Bank sort code for direct debit |  |
| `BankAccountNo` | `varchar` |  | Bank account number for direct debit |  |
| `first_instalment_date` | `datetime` |  | Date of first instalment collection |  |
| `next_instalment_date` | `datetime` |  | Date of next scheduled collection |  |
| `last_instalment_date` | `datetime` |  | Date of last instalment |  |
| `agent_cnt` | `int` |  | FK to Party (agent) |  |
| `user_id` | `smallint` |  | FK to PMUser |  |
| `date_created` | `datetime` |  | Date the plan was created |  |
| `date_modified` | `datetime` |  | Date the plan was last modified |  |
| `date_confirmed` | `datetime` |  | Date the plan was confirmed/live |  |
| `tax_group_id` | `int` |  | FK to Tax_Group |  |
| `sub_branch_id` | `int` |  | FK to Sub_Branch |  |
| `batch_id` | `int` |  | FK to Batch |  |
| `pfpremiumfinance_cancel_reason_id` | `int` |  | FK to PFPremiumFinance_Cancel_Reason |  |
| `party_bank_id` | `int` |  | FK to Party_Bank |  |
| `business_identifier_code` | `varchar` |  | BIC/SWIFT code |  |
| `international_bank_account_number` | `varchar` |  | IBAN |  |
| `dd_cancelled` | `tinyint` |  | Flag: direct debit cancelled | 0,1 |
| `cc_cancelled` | `tinyint` |  | Flag: card collection cancelled | 0,1 |

*Foreign Keys*:
- `PFPremiumFinance.Insurance_File_Cnt` â†’ `insurance_file (insurance_file_cnt)` (Many-to-One)
- `PFPremiumFinance.agent_cnt` â†’ `Party (party_cnt)` (Many-to-One)
- `PFPremiumFinance.user_id` â†’ `PMUser (user_id)` (Many-to-One)
- `PFPremiumFinance.CompanyNo` â†’ `PFScheme (CompanyNo)` (Many-to-One)
- `PFPremiumFinance.pfrf_id` â†’ `PFRF (pfrf_id)` (Many-to-One)
- `PFPremiumFinance.tax_group_id` â†’ `Tax_Group (tax_group_id)` (Many-to-One)
- `PFPremiumFinance.pfpremiumfinance_cancel_reason_id` â†’ `PFPremiumFinance_Cancel_Reason (pfpremiumfinance_cancel_reason_id)` (Many-to-One)
- `PFPremiumFinance.party_bank_id` â†’ `Party_Bank (party_bank_id)` (Many-to-One)
- `PFPremiumFinance.batch_id` â†’ `Batch (batch_id)` (Many-to-One)

#### `PFPremiumFinance_Cancel_Reason`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Lookup: reason codes for cancelling a premium finance plan

*Keywords*: Premium Finance, Cancel Reason, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pfpremiumfinance_cancel_reason_id` | `int` |  | Unique cancel reason identifier (PK) |  |
| `code` | `varchar` |  | Short code |  |
| `description` | `varchar` |  | Description |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |

#### `PFPremiumFinance_Cancellation_Transactions`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Transactions posted when a premium finance plan is cancelled

*Keywords*: Premium Finance, Cancellation, Transactions

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pfprem_finance_cnt` | `int` |  | FK to PFPremiumFinance (plan) |  |
| `pfprem_finance_version` | `int` |  | FK to PFPremiumFinance (version) |  |
| `transdetail_id` | `int` |  | FK to transdetail |  |

*Foreign Keys*:
- `PFPremiumFinance_Cancellation_Transactions.pfprem_finance_cnt` â†’ `PFPremiumFinance (pfprem_finance_cnt)` (Many-to-One)
- `PFPremiumFinance_Cancellation_Transactions.transdetail_id` â†’ `transdetail (transdetail_id)` (Many-to-One)

#### `PFRF`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Rate file entries defining instalment rates and APRs for a PF scheme

*Keywords*: PF Rate File, APR, Interest Rate, Instalment Rates

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `CompanyNo` | `int` |  | FK to PFScheme (company no) |  |
| `SchemeNo` | `int` |  | FK to PFScheme (scheme no) |  |
| `SchemeVersion` | `int` |  | FK to PFScheme (version) |  |
| `pfrf_id` | `int` |  | Unique rate file entry identifier (PK) |  |
| `StartDate` | `datetime` |  | Rate file effective start date |  |
| `ProductFamily` | `varchar` |  | Product family this rate applies to |  |
| `ArrangementFee` | `numeric` |  | Arrangement fee charged to client |  |
| `DaysDelay` | `int` |  | Number of days delay before first collection |  |
| `pffrequency_id` | `int` |  | FK to PFFrequency (collection frequency) |  |
| `Min1` | `numeric` |  | Minimum amount for rate band 1 |  |
| `Max1` | `numeric` |  | Maximum amount for rate band 1 |  |
| `Rate1` | `numeric` |  | Flat rate for band 1 |  |
| `APR1` | `numeric` |  | APR for band 1 |  |
| `NoOfInstallments` | `int` |  | Number of instalments for this rate |  |
| `review_pmuser_group_id` | `int` |  | FK to PMUser_Group (review group) |  |
| `statement_report_id` | `int` |  | FK to Report |  |
| `statement_pffrequency_id` | `int` |  | FK to PFFrequency (statement frequency) |  |
| `UserId` | `int` |  | Last modified by user |  |

*Foreign Keys*:
- `PFRF.CompanyNo` â†’ `PFScheme (CompanyNo)` (Many-to-One)
- `PFRF.pffrequency_id` â†’ `PFFrequency (pffrequency_id)` (Many-to-One)
- `PFRF.statement_pffrequency_id` â†’ `PFFrequency (pffrequency_id)` (Many-to-One)
- `PFRF.review_pmuser_group_id` â†’ `PMUser_Group (pmuser_group_id)` (Many-to-One)
- `PFRF.statement_report_id` â†’ `Report (report_id)` (Many-to-One)

#### `PFScheme`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Premium finance provider scheme definition (rates, accounts, EDI config)

*Keywords*: PF Scheme, Finance Provider, Interest Rate

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `CompanyNo` | `int` |  | Finance company number (PK part 1) |  |
| `SchemeNo` | `int` |  | Scheme number (PK part 2) |  |
| `SchemeVersion` | `int` |  | Scheme version (PK part 3) |  |
| `Party_Cnt` | `int` |  | FK to Party (finance provider party) |  |
| `SchemeName` | `varchar` |  | Name of the finance scheme |  |
| `SchemeDescription` | `varchar` |  | Description of the scheme |  |
| `StartDate` | `datetime` |  | Scheme effective start date |  |
| `EndDate` | `datetime` |  | Scheme effective end date |  |
| `NoOfInstallments` | `int` |  | Default number of instalments |  |
| `IsInHouse` | `tinyint` |  | Flag: in-house finance (not external provider) | 0,1 |
| `currency_id` | `smallint` |  | FK to Currency |  |
| `bankaccount_id` | `int` |  | FK to BankAccount (scheme settlement account) |  |
| `mediatype_id` | `int` |  | FK to MediaType (default collection media) |  |
| `pfscheme_type_id` | `int` |  | FK to PFScheme_Type |  |
| `pfscheme_printtype_id` | `int` |  | FK to PFScheme_PrintType |  |
| `tax_group_id` | `int` |  | FK to Tax_Group |  |
| `suspense_account_id` | `int` |  | FK to Account (suspense) |  |
| `interest_account_id` | `int` |  | FK to Account (interest) |  |
| `admin_account_id` | `int` |  | FK to Account (admin fee) |  |
| `PFScheme_id` | `int` |  | Surrogate unique identifier |  |
| `UserId` | `int` |  | Last modified by user |  |

*Foreign Keys*:
- `PFScheme.currency_id` â†’ `currency (currency_id)` (Many-to-One)
- `PFScheme.tax_group_id` â†’ `Tax_Group (tax_group_id)` (Many-to-One)
- `PFScheme.bankaccount_id` â†’ `BankAccount (bankaccount_id)` (Many-to-One)
- `PFScheme.mediatype_id` â†’ `MediaType (mediatype_id)` (Many-to-One)
- `PFScheme.pfscheme_type_id` â†’ `PFScheme_Type (pfscheme_type_id)` (Many-to-One)
- `PFScheme.pfscheme_printtype_id` â†’ `PFScheme_PrintType (pfscheme_printtype_id)` (Many-to-One)
- `PFScheme.Party_Cnt` â†’ `Party (party_cnt)` (Many-to-One)

#### `PFSchemeProducts`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Products authorised for use under a premium finance scheme

*Keywords*: PF Scheme, Products, Authorisation

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `CompanyNo` | `int` |  | FK to PFScheme (company no) |  |
| `SchemeNo` | `int` |  | FK to PFScheme (scheme no) |  |
| `SchemeVersion` | `int` |  | FK to PFScheme (version) |  |
| `product_id` | `int` |  | FK to product |  |
| `UserId` | `int` |  | Last modified by user |  |
| `UniqueId` | `varchar` |  | Unique sync identifier |  |

*Foreign Keys*:
- `PFSchemeProducts.CompanyNo` â†’ `PFScheme (CompanyNo)` (Many-to-One)
- `PFSchemeProducts.product_id` â†’ `product (product_id)` (Many-to-One)

#### `PFSchemeSource`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Source/branch authorisations linked to a PF scheme

*Keywords*: PF Scheme, Source, Branch

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `CompanyNo` | `int` |  | FK to PFScheme |  |
| `SchemeNo` | `int` |  | FK to PFScheme |  |
| `SchemeVersion` | `int` |  | FK to PFScheme |  |
| `source_id` | `int` |  | FK to Source |  |
| `UserId` | `int` |  | Last modified by user |  |
| `UniqueId` | `varchar` |  | Unique sync identifier |  |

*Foreign Keys*:
- `PFSchemeSource.CompanyNo` â†’ `PFScheme (CompanyNo)` (Many-to-One)

#### `PFScheme_PrintType`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Lookup: print/output type codes for a PF scheme

*Keywords*: PF Scheme, Print Type, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pfscheme_printtype_id` | `int` |  | Unique print type identifier (PK) |  |
| `code` | `varchar` |  | Short code |  |
| `description` | `varchar` |  | Description |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |

#### `PFScheme_Type`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Lookup: premium finance scheme type codes

*Keywords*: PF Scheme Type, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pfscheme_type_id` | `int` |  | Unique scheme type identifier (PK) |  |
| `code` | `varchar` |  | Short code |  |
| `description` | `varchar` |  | Description |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |

#### `PFTransaction_Id`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Links a premium finance plan version to a policy transaction

*Keywords*: Premium Finance, Transaction, Policy

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pfprem_finance_cnt` | `int` |  | FK to PFPremiumFinance (plan) |  |
| `pfprem_finance_version` | `int` |  | FK to PFPremiumFinance (version) |  |
| `pftransaction_id` | `int` |  | Unique transaction identifier (PK) |  |
| `insurance_file_cnt` | `int` |  | FK to insurance_file |  |

#### `PF_business_code`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Lookup: business codes used on premium finance plans

*Keywords*: PF Business Code, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `PF_business_code_id` | `int` |  | Unique business code identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code |  |
| `description` | `varchar` |  | Description |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

#### `Party_Finance_Provider`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Finance provider details for a party (PF EDI, broker numbers)

*Keywords*: Finance, Premium Finance, Provider, EDI

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | FK to party (party_cnt) |  |
| `finance_provider_number` | `int` |  | Finance provider reference number |  |
| `agency_number` | `varchar` |  | Agency number with finance provider |  |
| `mailbox_number` | `varchar` |  | EDI mailbox number |  |
| `pfedidefinition_id` | `int` |  | FK to PF EDI definition |  |
| `dob` | `tinyint` |  | Flag: DOB required by provider | 0,1 |
| `companyreg` | `tinyint` |  | Flag: company reg required | 0,1 |
| `quote_available` | `tinyint` |  | Flag: quote available from provider | 0,1 |
| `payment_protection_available` | `tinyint` |  | Flag: PPI available from provider | 0,1 |
| `broker_number` | `varchar` |  | Broker reference number at provider |  |
| `MTA_available` | `tinyint` |  | Flag: MTA available from provider | 0,1 |

*Foreign Keys*:
- `Party_Finance_Provider.party_cnt` â†’ `party (party_cnt)` (Many-to-One)

#### `Party_Other_Posting_Type`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Lookup: posting type codes for other-type parties

*Keywords*: Posting Type, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_other_posting_type_id` | `int` |  | Unique posting type identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code |  |
| `description` | `varchar` |  | Description |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

#### `StructureTree`
**Domain: Finance**  **Owner: Finance**  **Refresh: Static**

Represents the hierarchical chart-of-accounts / financial reporting structure tree, linking companies, mappings, accounts, and elements in a parent-child node hierarchy.

*Keywords*: structure, tree, hierarchy, chart of accounts, element, mapping, company, financial

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `node_id` | `int` | âœ“ | Primary key — unique identifier for this node in the structure tree. |  |
| `company_id` | `int` |  | Company this tree node belongs to. |  |
| `mapping_id` | `int` |  | Foreign key to Mapping — the account mapping this node is part of. |  |
| `account_id` | `int` |  | Ledger account associated with this node. |  |
| `element_id` | `int` |  | Foreign key to Element — the element (heading/account) at this node. |  |
| `parent_node_id` | `int` |  | Self-referencing FK to parent node — defines the tree hierarchy. |  |
| `core_node` | `tinyint` |  | Indicates whether this is a core/system node that cannot be deleted. | 0,1 |

*Foreign Keys*:
- `StructureTree.element_id` â†’ `Element.element_id` (Many-to-One)
- `StructureTree.mapping_id` â†’ `Mapping.mapping_id` (Many-to-One)
- `StructureTree.parent_node_id` â†’ `StructureTree.node_id` (Many-to-One)

#### `Transaction_Export_Detail`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Line-level export detail for each Transaction_Export_Folder record. Contains individual transaction amounts, ledger codes, account references, tax breakdowns, and mapping codes for export to accounting systems.

*Keywords*: Transaction Detail, Ledger, Account, Tax, Commission, Sum Insured, Export, Mapping Code

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `transaction_export_folder_cnt` | `int` |  | Links to parent Transaction_Export_Folder record (composite PK) | 1,2,3 |
| `transaction_export_detail_id` | `int` |  | Unique line identifier within the export folder (composite PK) | 1,2,3 |
| `transaction_amount` | `numeric` |  | Amount for this transaction line | 1000.00,60.00 |
| `transaction_ledger_code` | `char` |  | Ledger code for the transaction e.g. AG (agent), NO (net own) | AG,NO |
| `account_type_code` | `varchar` |  | Account type code e.g. AGENTLEDGR, INCGWP | AGENTLEDGR,INCGWP |
| `transaction_account_key` | `int` |  | Account key for the transaction line (links to account.account_key) | 237 |
| `ceded_ref` | `varchar` |  | Ceded reinsurance reference |  |
| `cover_share_percent` | `numeric` |  | Cover share percentage | 0.0 |
| `sum_insured_total` | `numeric` |  | Total sum insured for the line | 1000000.00 |
| `charges_total` | `numeric` |  | Total charges on the line | 0.00 |
| `taxes_total` | `numeric` |  | Total taxes on the line | 60.00 |
| `recoveries_total` | `numeric` |  | Total recoveries on the line | 0.00 |
| `commission_excluded` | `numeric` |  | Commission excluded from calculation | 0.00 |
| `withholding_tax_excluded` | `numeric` |  | Withholding tax excluded from calculation | 0.00 |
| `mapping_code` | `varchar` |  | Mapping code for the external accounting system e.g. 1stBROKERS | 1stBROKERS |
| `spare` | `varchar` |  | Spare/notes field e.g. GROSS, TAX | GROSS,TAX |
| `purchase_order_no` | `varchar` |  | Purchase order number |  |
| `purchase_invoice_no` | `varchar` |  | Purchase invoice number |  |
| `base_transaction_amount` | `money` |  | Transaction amount in base/system currency |  |
| `base_taxes_amount` | `money` |  | Tax amount in base/system currency |  |
| `suspended` | `tinyint` |  | Flag indicating if the line is suspended from export | 0,1 |
| `release_to_income` | `tinyint` |  | Flag indicating if the line should be released to income | 0,1 |
| `release_account_code` | `varchar` |  | Account code to release to |  |
| `transdetail_type_code` | `varchar` |  | Transaction detail type code e.g. GROSS, TAX | GROSS,TAX |
| `tax_group_id` | `int` |  | Tax group identifier (links to tax_group) |  |
| `tax_band_id` | `int` |  | Tax band identifier (links to tax_band) |  |
| `manually_released` | `tinyint` |  | Flag indicating if the line was manually released | 0,1 |
| `released_on_full_settlement` | `tinyint` |  | Flag indicating release on full settlement | 0,1 |
| `released_for_whole_posting` | `tinyint` |  | Flag indicating release for whole posting period | 0,1 |
| `released_on_policy_effective` | `tinyint` |  | Flag indicating release on policy effective date | 0,1 |
| `fee_type` | `varchar` |  | Fee type code associated with the line |  |

*Foreign Keys*:
- `Transaction_Export_Detail.transaction_export_folder_cnt` â†’ `Transaction_Export_Folder (transaction_export_folder_cnt)` (Many-to-One)
- `Transaction_Export_Detail.transaction_account_key` â†’ `account (account_key)` (Many-to-One)
- `Transaction_Export_Detail.tax_group_id` â†’ `tax_group (tax_group_id)` (Many-to-One)
- `Transaction_Export_Detail.tax_band_id` â†’ `tax_band (tax_band_id)` (Many-to-One)

#### `Transaction_Export_Folder`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Pre-export header table for policy financial transactions. Mirrors stats_folder but includes export status tracking and additional account key references. Used to stage transactions before export to external accounting systems.

*Keywords*: Transaction Export, Policy Transaction, Premium, New Business, MTA, Renewal, Cancellation, Agent, Branch, Currency, Export Status

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `transaction_export_folder_cnt` | `int` |  | Unique export folder identifier (PK) | 1,2,3 |
| `product_id` | `int` |  | Product identifier | 12 |
| `transaction_export_folder_id` | `int` |  | Internal export folder ID (may differ from PK for versioning) | 0 |
| `source_id` | `int` |  | Policy branch/source identifier | 1,2,3 |
| `insurance_file_cnt` | `int` |  | Links to the policy version (insurance_file) | 1,2,3 |
| `debit_credit` | `char` |  | Indicates debit (D) or credit (C) transaction | D,C |
| `document_ref` | `varchar` |  | Document reference number | SND00000001 |
| `document_comment` | `varchar` |  | Optional comment on the document |  |
| `document_date` | `datetime` |  | Date the document was created |  |
| `is_payable_by_instalments` | `tinyint` |  | Flag indicating if the premium is payable by instalments | 0,1 |
| `accounting_date` | `datetime` |  | Accounting date for the transaction |  |
| `posting_period_year` | `datetime` |  | Financial year of the posting period |  |
| `posting_period_number` | `smallint` |  | Period number within the posting year | 34,35 |
| `premium_total` | `numeric` |  | Total premium amount for the transaction | 0.00 |
| `transaction_type_id` | `int` |  | Transaction type identifier | 4 |
| `transaction_type_code` | `varchar` |  | Transaction type code e.g. NB, MTA, REN, CAN | NB,MTA,REN,CAN |
| `insurance_ref` | `varchar` |  | Policy or quote reference number | MRULPOL0528 |
| `effective_date` | `datetime` |  | Effective date of the transaction on the policy |  |
| `cover_start_date` | `datetime` |  | Cover start date of the policy version |  |
| `expiry_date` | `datetime` |  | Expiry date of the policy version |  |
| `insurance_holder_cnt` | `int` |  | Client party identifier | 1155,1157 |
| `insurance_holder_id` | `int` |  | Internal ID of the insurance holder party | 0 |
| `insurance_holder_shortname` | `varchar` |  | Client short name | AMITAA |
| `insurance_holder_account_key` | `int` |  | Account key for the client party (links to account) | 1155 |
| `product_code` | `char` |  | Product code | AAA2011 |
| `business_type_id` | `smallint` |  | Business type identifier (Direct, Agency etc) | 1,7 |
| `business_type_code` | `char` |  | Business type code e.g. DIRECT, AGENCY | DIRECT,AGENCY |
| `account_handler_cnt` | `int` |  | Account handler party identifier |  |
| `account_handler_id` | `int` |  | Internal ID of account handler party |  |
| `account_handler_shortname` | `varchar` |  | Account handler short name |  |
| `account_handler_account_key` | `int` |  | Account key for the account handler (links to account) |  |
| `agent_cnt` | `int` |  | Broker/Agent party identifier | 237 |
| `agent_id` | `int` |  | Internal ID of the broker/agent party | 0 |
| `agent_shortname` | `varchar` |  | Broker/Agent short name | 1stBROKERS |
| `agent_account_key` | `int` |  | Account key for the broker/agent (links to account) | 237 |
| `branch_id` | `smallint` |  | Branch identifier | 1 |
| `branch_code` | `char` |  | Branch code e.g. HQ | HQ |
| `currency_code` | `char` |  | Currency of the transaction | GBP,USD,ZAR |
| `loss_id` | `int` |  | Claim/loss identifier if transaction relates to a claim |  |
| `loss_code` | `varchar` |  | Claim/loss reference code |  |
| `loss_date` | `datetime` |  | Date of the loss/claim |  |
| `created_by_user_id` | `smallint` |  | User ID who created the record | 1,23 |
| `created_by_username` | `varchar` |  | Username who created the record | sirius,ursula |
| `accounts_export_status` | `char` |  | Export status flag e.g. c=complete, p=pending | c,p |
| `reason` | `varchar` |  | Reason or notes for the export status |  |
| `real_insurance_file_cnt` | `int` |  | Reference to the actual policy version if this is a shadow record |  |
| `underwriting_year_id` | `int` |  | Underwriting year identifier (FK to Underwriting_Year) |  |
| `base_currency_id` | `smallint` |  | Base currency identifier (FK to Currency) |  |
| `terms_of_payment_id` | `int` |  | Terms of payment identifier |  |
| `payment_due_date` | `datetime` |  | Due date for payment of the premium |  |
| `event_log_id` | `int` |  | Event log identifier for audit trail |  |

*Foreign Keys*:
- `Transaction_Export_Folder.underwriting_year_id` â†’ `Underwriting_Year (underwriting_year_id)` (Many-to-One)
- `Transaction_Export_Folder.base_currency_id` â†’ `Currency (currency_id)` (Many-to-One)
- `Transaction_Export_Folder.product_id` â†’ `product (product_id)` (Many-to-One)
- `Transaction_Export_Folder.source_id` â†’ `source (source_id)` (Many-to-One)
- `Transaction_Export_Folder.insurance_file_cnt` â†’ `insurance_file (insurance_file_cnt)` (Many-to-One)
- `Transaction_Export_Folder.real_insurance_file_cnt` â†’ `insurance_file (insurance_file_cnt)` (Many-to-One)
- `Transaction_Export_Folder.insurance_holder_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `Transaction_Export_Folder.insurance_holder_account_key` â†’ `account (account_key)` (Many-to-One)
- `Transaction_Export_Folder.account_handler_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `Transaction_Export_Folder.account_handler_account_key` â†’ `account (account_key)` (Many-to-One)
- `Transaction_Export_Folder.agent_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `Transaction_Export_Folder.agent_account_key` â†’ `account (account_key)` (Many-to-One)
- `Transaction_Export_Folder.loss_id` â†’ `claim (claim_id)` (Many-to-One)
- `Transaction_Export_Folder.transaction_type_id` â†’ `transaction_type (transaction_type_id)` (Many-to-One)
- `Transaction_Export_Folder.business_type_id` â†’ `business_type (business_type_id)` (Many-to-One)
- `Transaction_Export_Folder.branch_id` â†’ `branch (branch_id)` (Many-to-One)
- `Transaction_Export_Folder.terms_of_payment_id` â†’ `terms_of_payment (terms_of_payment_id)` (Many-to-One)

#### `Transaction_Type`
**Domain: Finance**  **Owner: Finance**  **Refresh: Static**

Lookup defining all transaction types in the system: NB (New Business), REN (Renewal), MTA (Mid-term Adjustment), MTC (Mid-term Cancellation), MTR (Mid-term Reinstatement), claim transactions (C_CR, C_CO, C_CP) etc. The transaction_type_id is used throughout the system to classify every financial transaction.

*Keywords*: transaction type, NB, REN, MTA, MTC, MTR, cancellation, reinstatement, claim transaction, new business, renewal, transaction_type_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `transaction_type_id` | `int` | âœ“ | Primary key for the transaction type. |  |
| `code` | `varchar` |  | Short code: NB=New Business, REN=Renewal, MTA=Mid-term Adjustment, MTC=Mid-term Cancellation, MTR=Reinstatement, C_CR/C_CO/C_CP=Claim transactions. | NB,REN,MTA,MTC,MTR,C_CR,C_CO,C_CP |
| `description` | `varchar` |  | Description of the transaction type. | New Business,Renewal,Mid-term Adjustment,Open Claim,Pay Claim |
| `transaction_type_basis` | `char` |  | Basis classification of the transaction (e.g. A=Accounting). | A |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this type is effective. |  |

#### `Transdetail_Type`
**Domain: Finance**  **Owner: Finance**  **Refresh: Static**

Lookup defining the types of transaction detail lines (e.g. Transaction, Gross, Commission, Potential Commission, Fee, Tax). Used to categorise the breakdown of a financial transaction into its component parts.

*Keywords*: transaction detail type, gross, commission, fee, tax, transdetail_type_id, TRANS, GROSS, COMM

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `transdetail_type_id` | `int` | âœ“ | Primary key for the transaction detail type. |  |
| `code` | `varchar` |  | Short code (e.g. TRANS=Transaction, GROSS=Gross, COMM=Commission, TAX=Tax, FEE=Fee). | TRANS,GROSS,COMM,BROK,TAX,FEE |
| `description` | `varchar` |  | Description of the detail type. | Transaction,Gross,Commission,Potential Commission |
| `is_extended` | `tinyint` |  | Whether this is an extended/breakdown detail type (1=yes). | 0,1 |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `effective_date` | `datetime` |  | Date from which this type is effective. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |

#### `Write_Off_Reason`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Lookup: reason codes for writing off an instalment or debt

*Keywords*: Write Off, Reason, Instalment, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `write_off_reason_id` | `int` |  | Unique write-off reason identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `description` | `varchar` |  | Description of the write-off reason |  |
| `code` | `varchar` |  | Short code |  |
| `is_valid_for_instalments` | `tinyint` |  | Flag: reason can be used for instalment write-offs | 0,1 |

#### `account`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Financial ledger account for a party. Each party has accounts representing their financial position (premium debtor, commission creditor, etc.). Accounts are the target of transdetail postings. Key to all financial reporting.

*Keywords*: account, financial account, ledger account, party account, balance, debtor, creditor, premium account, commission account, posting, account_cnt

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `account_id` | `int` |  | Unique account identifier |  |
| `account_name` | `varchar` |  | Account Name |  |
| `short_code` | `char` |  | Account Short Code |  |
| `account_key` | `int` |  | Link to Party if this is a party otherwise this is null |  |
| `accounttype_id` | `int` |  | Account Type (Income, Expense, Asset, Liability) |  |
| `ledger_id` | `int` |  | Ledger Type |  |

*Foreign Keys*:
- `account.account_key` â†’ `party (party_cnt)` (Many-to-One)
- `account.accounttype_id` â†’ `accounttype(accounttype_id)` (Many-to-One)
- `account.ledger_id` â†’ `ledger(ledger_id)` (Many-to-One)

#### `accounttype`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Lookup defining the financial type of an account: Income, Expense, Asset, Liability, Capital. Controls how account balances appear in financial statements (P&L vs Balance Sheet).

*Keywords*: account type, income, expense, asset, liability, capital, P&L, balance sheet, financial statement, account classification, lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `accounttype_id` | `int` |  | Account Type Unique ID |  |
| `code` | `char` |  | Account Type Code |  |
| `description` | `varchar` |  | Account Type Description |  |

#### `allocation`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Allocation group header for settlement of financial transactions — receipts against premiums, claim payments, commission settlements. Groups related cashlist items and transaction lines being matched together.

*Keywords*: allocation, settlement, receipt allocation, payment allocation, commission settlement, matching, allocation group, unallocated

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `allocation_id` | `int` |  | Allocation Group Unique Identifier |  |
| `allocation_date` | `datetime` |  | Settlement/Allocation Date |  |

#### `allocationdetail`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Individual settlement line within an allocation group. Links a specific cashlist item or transaction detail as part of the settlement. Contains allocated amount, account and status.

*Keywords*: allocation detail, settlement detail, receipt, payment, commission, matching, allocated amount, allocation line, allocationdetail_cnt

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `allocationdetail_id` | `int` |  | Settlement/Allocation Detail Unique Identifier |  |
| `cashlistitem_id` | `int` |  | Linked Cashlist details |  |
| `allocation_id` | `int` |  | Allocation Group link |  |
| `original_currency` | `smallint` |  | Transaction Currency |  |
| `transdetail_id` | `int` |  | Transaction Link |  |
| `documenttype_id` | `smallint` |  | Type of Transaction Group |  |
| `accounting_date` | `datetime` |  | Transaction Date |  |
| `document_ref` | `varchar` |  | Transaction Group Reference |  |
| `original_date` | `datetime` |  | Settlement Date |  |
| `orig_base_amount` | `numeric` |  | Original Transaction Amount in Branch's base currency |  |
| `orig_ccy_amount` | `numeric` |  | Original Transaction Amount in Transaction currency |  |
| `os_base_amount` | `numeric` |  | Outstanding Transaction Amount in Branch's base currency |  |
| `os_ccy_amount` | `numeric` |  | Outstanding Transaction Amount in Transaction currency |  |
| `alloc_base_amount` | `numeric` |  | Settled Amount in Branch's base currency |  |
| `alloc_ccy_amount` | `numeric` |  | Settled Amount in Transaction currency |  |
| `write_off_amount` | `numeric` |  | Any Written off amount |  |
| `new_os_ccy_amount` | `numeric` |  | Outstanding Transaction Amount post settlement in Branch's base currency |  |
| `new_os_base_amount` | `numeric` |  | Outstanding Transaction Amount post settlement in Transaction currency |  |
| `is_reversed` | `tinyint` |  | is this Settlement now reversed |  |
| `allocation_reversed_date` | `datetime` |  | reversal date (if any) |  |

*Foreign Keys*:
- `allocationdetail.cashlistitem_id` â†’ `cashlistitem(cashlistitem_id)` (Many-to-One)
- `allocationdetail.allocation_id` â†’ `allocation(allocation_id)` (Many-to-One)
- `allocationdetail.original_currency` â†’ `currency(currency_id)` (Many-to-One)
- `allocationdetail.transdetail_id` â†’ `transdetail(transdetail_id)` (Many-to-One)
- `allocationdetail.documenttype_id` â†’ `documenttype(documenttype_id)` (Many-to-One)

#### `bank`
**Domain: Finance**  **Owner: Finance**  **Refresh: Admin**

Reference data for banks used in payment and bank account configuration. Stores the bank name, sort code/branch code, address, contact details and account type. Used when setting up company bank accounts and payment processing.

*Keywords*: bank, sort code, branch code, bank name, head office, bank address, bank_id, HSBC, Citibank

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `bank_id` | `int` | âœ“ | Primary key for the bank record. |  |
| `code` | `char` |  | Short code for the bank (e.g. HSBC, CITIUS, BANKSUSPAC). | HSBC,CITIUS,BANKSUSPAC |
| `bank_name` | `varchar` |  | Full name of the bank. | HSBC UK,Citibank US Dollars |
| `branch_code` | `varchar` |  | Sort code or branch code for the bank. |  |
| `head_office` | `int` |  | Foreign key to bank — the head office bank record (self-referential for branches). |  |
| `bank_country` | `smallint` |  | Foreign key to Country — the country where the bank is located. |  |
| `bank_address1` | `varchar` |  | Bank address line 1. |  |
| `bank_address2` | `varchar` |  | Bank address line 2. |  |
| `bank_postal_code` | `varchar` |  | Postal/zip code of the bank. |  |
| `bank_phone_number` | `varchar` |  | Bank telephone number. |  |
| `bank_account_type_id` | `int` |  | Foreign key to Bank_Account_Type. |  |
| `comments` | `varchar` |  | Free-text comments about this bank. |  |

*Foreign Keys*:
- `bank.head_office` â†’ `bank.bank_id` (Many-to-One)
- `bank.bank_country` â†’ `Country.country_id` (Many-to-One)

#### `document`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Financial transaction grouping/posting header. Each financial transaction (premium, claim payment, commission, RI payment etc.) generates a document record linking all debit/credit lines. NOTE: this is an accounting document (transaction header), NOT a physical letter/file — see DOC_document for physical documents.

*Keywords*: financial transaction, posting, document, ledger, premium transaction, claim payment, commission posting, debit, credit, accounting, transaction header, document_cnt

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `document_id` | `int` |  | Unique document identifier |  |
| `document_ref` | `varchar` |  | Document Reference Number |  |
| `document_date` | `datetime` |  | Effective Date of document  |  |
| `created_date` | `datetime` |  | Transaction Date of document  |  |
| `insurance_file_cnt` | `int` |  | link to Policy |  |
| `claim_id` | `int` |  | link to Claim |  |
| `documenttype_id` | `smallint` |  | Link to Document Type |  |

*Foreign Keys*:
- `document.insurance_file_cnt` â†’ `insurance_file (insurance_file_cnt)` (Many-to-One)
- `document.documenttype_id` â†’ `document_type(documenttype_id)` (Many-to-One)

#### `documenttype`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Lookup defining the financial transaction posting type — e.g. Standard New Business (SND), Endorsement Credit (SEC), Endorsement Debit (SED), Claim Payment (CLP), Commission. This classifies financial transactions, not physical documents.

*Keywords*: document type, transaction type, posting type, SND, SEC, SED, CLP, new business, endorsement credit, endorsement debit, claim payment, commission, financial posting type

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `documenttype_id` | `int` |  | Document Type Unique Identifer |  |
| `code` | `char` |  | Document Type Code |  |
| `description` | `varchar` |  | Document Type Description |  |

#### `ledger`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Lookup defining which financial ledger an account belongs to: Client Ledger, Agent/Broker Ledger, Commission Ledger, Nominal/General Ledger (GL), Insurer Ledger. Groups accounts for reporting and reconciliation.

*Keywords*: ledger, client ledger, agent ledger, broker ledger, nominal ledger, general ledger, GL, commission ledger, insurer ledger, ledger type

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `ledger_id` | `int` |  | Ledger Unique ID |  |
| `ledger_short_name` | `char` |  | Ledger Code |  |
| `ledger_name` | `varchar` |  | Ledger Description |  |
| `ledgertype_id` | `varchar` |  | Ledger Type |  |

#### `period`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Accounting period definition — monthly periods used to control which period financial transactions are posted to. Holds period start/end dates and open/closed status for period-end financial reporting and locking.

*Keywords*: accounting period, month, period end, financial period, year end, reporting period, period close, accounting month

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `year_name` | `varchar` |  | Account Period Year |  |
| `period_name` | `varchar` |  | Period |  |
| `period_end_date` | `datetime` |  | Period End Date |  |
| `period_end_complete` | `tinyint` |  | Is Period End |  |
| `period_id` | `int` |  | Period Unique ID |  |

#### `policy_fee_u`
**Domain: Finance**  **Owner: Finance**  **Refresh: Real-time**

The per-policy fee record — stores the actual fee charged on a specific insurance file/risk. Holds the calculated fee amounts in both policy currency and base currency, including any tax. Linked back to the Fee_Amounts configuration and to the risk, peril group, party and policy. This is the fee line on a policy transaction.

*Keywords*: policy fee, broker fee, fee charged, base currency fee, currency amount, tax amount, instalment, pro rata, fee override, policy_fee_u_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `policy_fee_u_id` | `int` | âœ“ | Primary key for the policy fee record. |  |
| `insurance_file_cnt` | `int` |  | Foreign key to Insurance_File — the policy version this fee was charged on. |  |
| `party_cnt` | `int` |  | Foreign key to Party — the broker/party who receives/is charged this fee. |  |
| `risk_cnt` | `int` |  | Foreign key to Risk — the risk on the policy this fee relates to. |  |
| `fee_amount_id` | `int` |  | Foreign key to Fee_Amounts — the fee configuration that generated this charge. |  |
| `product_id` | `int` |  | Foreign key to Product. |  |
| `branch_id` | `int` |  | Foreign key to Source/Branch. |  |
| `transaction_type_id` | `int` |  | Foreign key to Transaction_Type — the transaction this fee applies to. |  |
| `transaction_sub_type` | `tinyint` |  | Sub-type of the transaction. |  |
| `peril_group_id` | `int` |  | Foreign key to Peril_Group. |  |
| `risk_type_group_id` | `int` |  | Foreign key to Risk_Type_Group. |  |
| `tax_group_id` | `int` |  | Foreign key to Tax_Group — tax group applied to this fee. |  |
| `currency_id` | `int` |  | Currency of the policy for this fee. |  |
| `base_currency_id` | `smallint` |  | Foreign key to Currency — the base/reporting currency. |  |
| `fee_rate_currency_id` | `smallint` |  | Currency in which the fee rate is defined. |  |
| `fee_rate_percentage` | `numeric` |  | Fee rate as a percentage (0 if fixed amount). | 0.0000 |
| `fee_rate_amount` | `numeric` |  | Fixed fee rate amount. | 20.0000 |
| `base_fee_amount` | `money` |  | Calculated fee in the base/reporting currency. | 20.00 |
| `base_tax_amount` | `money` |  | Tax on the fee in the base currency. | 0.00 |
| `currency_amount` | `money` |  | Calculated fee in the policy currency. | 20.00 |
| `currency_tax_amount` | `money` |  | Tax on the fee in the policy currency. | 0.00 |
| `Fee_Premium` | `money` |  | The premium on which this fee was calculated. |  |
| `include_fee_in_instalments` | `tinyint` |  | Whether this fee is included in instalment calculations. | 0,1 |
| `spread_fee_across_instalments` | `tinyint` |  | Whether this fee is spread evenly across instalments. | 0,1 |
| `is_fee_applied_to_cr` | `tinyint` |  | Whether the fee applies to credit transactions. | 0,1 |
| `FeeTypePercent` | `bit` |  | Whether the fee is percentage-based (1) or fixed amount (0). | 0,1 |
| `Calculation_Basis` | `tinyint` |  | Basis used to calculate this fee. |  |
| `Is_Prorated` | `tinyint` |  | Whether the fee was pro-rated for a mid-term transaction. | 0,1 |
| `Pro_rata_rate` | `numeric` |  | The pro-rata rate applied when calculating a mid-term fee. |  |
| `Is_Override` | `tinyint` |  | Whether this fee was manually overridden from the default. | 0,1 |
| `MakeLiveOptions_id` | `int` |  | Foreign key to MakeLiveOptions. |  |
| `DoPaymentTerms_id` | `int` |  | Foreign key to DOPaymentTerms. |  |

*Foreign Keys*:
- `policy_fee_u.insurance_file_cnt` â†’ `Insurance_File.insurance_file_cnt` (Many-to-One)
- `policy_fee_u.risk_cnt` â†’ `Risk.risk_cnt` (Many-to-One)
- `policy_fee_u.base_currency_id` â†’ `Currency.currency_id` (Many-to-One)
- `policy_fee_u.fee_rate_currency_id` â†’ `Currency.currency_id` (Many-to-One)
- `policy_fee_u.tax_group_id` â†’ `Tax_Group.tax_group_id` (Many-to-One)
- `policy_fee_u.risk_type_group_id` â†’ `Risk_Type_Group.risk_type_group_id` (Many-to-One)
- `policy_fee_u.peril_group_id` â†’ `Peril_Group.peril_group_id` (Many-to-One)
- `policy_fee_u.fee_amount_id` â†’ `Fee_Amounts.fee_amount_id` (Many-to-One)

#### `transdetail`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Individual debit or credit posting line within a financial transaction (document). Records account, amount, currency, party, transaction type, period and allocation status. Core accounting table for all premium, claim, commission, and RI financial entries.

*Keywords*: transaction detail, transdetail, posting, debit, credit, account, premium, commission, claim payment, RI, ledger, accounting, financial line, transdetail_cnt

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `transdetail_id` | `int` |  | Unique transaction identifier |  |
| `period_id` | `int` |  | Transaction Period Link |  |
| `account_id` | `int` |  | link to account |  |
| `document_id` | `int` |  | link to document |  |
| `accounting_date` | `datetime` |  | Transaction Date of document  |  |
| `currency_id` | `int` |  | Base Currency |  |
| `amount` | `numeric` |  | Total Amount in Base Currency |  |
| `amount_currency_id` | `int` |  | Transaction Currency |  |
| `currency_amount` | `numeric` |  | Total Amount in Transaction Currency |  |
| `insurance_ref` | `varchar` |  | Policy Reference if this is a policy transaction |  |
| `spare` | `varchar` |  | Type of Transaction i.e. Gross, Tax, Commission, Fee |  |
| `account_currency_id` | `int` |  | Account Currency |  |
| `account_amount` | `money` |  | Total Amount in Account Currency |  |
| `system_currency_id` | `int` |  | System Currency |  |
| `system_amount` | `money` |  | Total Amount in System Currency |  |
| `outstanding_amount` | `money` |  | Total Outstanding Amount in Base Currency for a Policy, Claim or other transaction |  |
| `outstanding_currency_amount` | `money` |  | Total Outstanding Amount in Transaction Currency for a Policy, Claim or other transaction |  |
| `outstanding_account_amount` | `money` |  | Total Outstanding Amount in Account Currency for a Policy, Claim or other transaction |  |
| `outstanding_system_amount` | `money` |  | Total Outstanding Amount in System Currency for a Policy, Claim or other transaction |  |
| `transdetail_type_id` | `int` |  | Transaction Type |  |

*Foreign Keys*:
- `transdetail.account_id` â†’ `account (account_id)` (Many-to-One)
- `transdetail.document_id` â†’ `document (document_id)` (Many-to-One)
- `transdetail.currency_id` â†’ `currency(currency_id)` (Many-to-One)
- `transdetail.amount_currency_id` â†’ `currency(currency_id)` (Many-to-One)
- `transdetail.account_currency_id` â†’ `currency(currency_id)` (Many-to-One)
- `transdetail.system_currency_id` â†’ `currency(currency_id)` (Many-to-One)
- `transdetail.period_id` â†’ `period(period_id)` (Many-to-One)

---

## Cash Management

**Tables in this module**: 18

### Tables

#### `AllocationStatus`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Static**

Lookup: allocation status of a cashlistitem e.g. Unallocated (U), Posted (P), Allocated (A).

*Keywords*: Allocation Status

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `allocationstatus_id` | `int` |  | Unique allocation status identifier (PK) | 1,2,3,4 |
| `code` | `char` |  | Code: U=Unallocated, P=Posted, A=Allocated | U,P,A |
| `description` | `varchar` |  | Status description | Unallocated,Posted,Allocated |
| `effective_date` | `datetime` |  | Date status became effective |  |
| `is_deleted` | `bit` |  | Soft delete flag | 0,1 |

#### `BankAccount`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Company bank accounts used for receiving and disbursing funds. Linked to currency, branch and the internal account ledger.

*Keywords*: Bank Account, Banking, Currency, IBAN, BIC

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `bankaccount_id` | `int` |  | Unique bank account identifier (PK) | 3,4,5 |
| `currency_id` | `smallint` |  | Currency of this bank account (FK to Currency) | 26 |
| `company_id` | `int` |  | Company owning this bank account | 1 |
| `sub_branch_id` | `int` |  | Sub-branch linked to this bank account | 1 |
| `account_id` | `int` |  | Internal ledger account (FK to Account) | 3119 |
| `bank_id` | `int` |  | Bank identifier | 1,2 |
| `code` | `char` |  | Short code for the bank account | 123456 |
| `bank_account_no` | `varchar` |  | Bank account number |  |
| `bank_account_name` | `varchar` |  | Name of the bank account |  |
| `description` | `varchar` |  | Description of the bank account |  |
| `effective_date` | `datetime` |  | Date the bank account became effective |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `next_cheque_number` | `int` |  | Next cheque number to issue from this account |  |
| `reconciled_date` | `datetime` |  | Date the account was last reconciled |  |
| `bank_statement_balance` | `numeric` |  | Balance per bank statement at last reconciliation |  |
| `business_identifier_code` | `varchar` |  | BIC/SWIFT code for the bank |  |
| `international_bank_account_number` | `varchar` |  | IBAN for the bank account |  |

*Foreign Keys*:
- `BankAccount.currency_id` â†’ `currency (currency_id)` (Many-to-One)
- `BankAccount.account_id` â†’ `account (account_id)` (Many-to-One)

#### `Batch`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Groups transactions into processing batches. Tracks batch status, type, source, accounting date, amounts, interface code and export/import lifecycle.

*Keywords*: Batch, Transaction Batch, Accounting Batch, Export

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `batch_id` | `int` |  | Unique batch identifier (PK) | 5,6,7 |
| `batch_ref` | `varchar` |  | Batch reference code e.g. GLX1, BAC2 | GLX1,BAC2 |
| `batchstatus_id` | `smallint` |  | Batch status identifier | 5,6 |
| `company_id` | `int` |  | Company this batch belongs to | 1 |
| `user_id` | `smallint` |  | User who created the batch | 1,23 |
| `created_date` | `datetime` |  | Date the batch was created |  |
| `accounting_date` | `datetime` |  | Accounting date for the batch |  |
| `batch_type_id` | `int` |  | Type of batch | 1,4 |
| `batch_source_id` | `int` |  | Source system of the batch |  |
| `mediatype_id` | `int` |  | Media type associated with this batch |  |
| `total_amount` | `numeric` |  | Total amount of all items in the batch |  |
| `total_transactions` | `int` |  | Total number of transactions in the batch |  |
| `export_date` | `datetime` |  | Date the batch was exported |  |
| `interface_code` | `varchar` |  | Interface code for external system routing |  |
| `authorised_date` | `datetime` |  | Date the batch was authorised |  |
| `import_file_name` | `varchar` |  | File name used for import |  |
| `Description` | `varchar` |  | Batch description |  |

*Foreign Keys*:
- `Batch.mediatype_id` â†’ `MediaType (mediatype_id)` (Many-to-One)

#### `CashListItem_Bank`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Static**

Lookup: bank names available for cashlistitem bank references e.g. HSBC, Zanaco.

*Keywords*: Bank Name, Cashlist Bank

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `cashlistitem_bank_id` | `int` |  | Unique bank ID (PK) | 1,2,3 |
| `code` | `varchar` |  | Bank code e.g. HSBC, ZANACO | HSBC,ZANACO |
| `description` | `varchar` |  | Bank full name | HSBC,Zanaco |
| `effective_date` | `datetime` |  | Effective date |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |

#### `CashListItem_Payment_Status`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Static**

Lookup: payment status of a cashlistitem e.g. Issued (ISS), Presented (PRES), Cancelled (CAN).

*Keywords*: Payment Status

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `cashlistitem_payment_status_id` | `int` |  | Unique payment status identifier (PK) | 1,2,3 |
| `code` | `varchar` |  | Code: ISS=Issued, PRES=Presented, CAN=Cancelled | ISS,PRES,CAN |
| `description` | `varchar` |  | Status description | Issued,Presented,Cancelled |
| `effective_date` | `datetime` |  | Effective date |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |

#### `CashListItem_Payment_Type`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Static**

Lookup: payment type for a cashlistitem e.g. Claim (CLP), Commission (COMM), Re-Insurance (RI).

*Keywords*: Payment Type, Claim Payment, Commission Payment, RI Payment

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `cashlistitem_payment_type_id` | `int` |  | Unique payment type identifier (PK) | 1,2,3 |
| `code` | `varchar` |  | Code: CLP=Claim, COMM=Commission, RI=Re-Insurance | CLP,COMM,RI |
| `description` | `varchar` |  | Type description | Claim,Commission,Re-Insurance |
| `effective_date` | `datetime` |  | Effective date |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |

#### `CashListItem_Receipt_Status`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Static**

Lookup: receipt status of a cashlistitem e.g. Added (ADD), Exported (EXP), Processed (DONE).

*Keywords*: Receipt Status

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `cashlistitem_receipt_status_id` | `int` |  | Unique receipt status identifier (PK) | 1,2,3 |
| `code` | `varchar` |  | Code: ADD=Added, EXP=Exported, DONE=Processed | ADD,EXP,DONE |
| `description` | `varchar` |  | Status description | Added,Exported,Processed |
| `effective_date` | `datetime` |  | Effective date |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |

#### `CashListItem_Receipt_Type`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Static**

Lookup: receipt type of a cashlistitem e.g. Premium Debt (STD), Instalment Debt (INST), Miscellaneous (MISC).

*Keywords*: Receipt Type, Instalment

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `cashlistitem_receipt_type_id` | `int` |  | Unique receipt type identifier (PK) | 1,2,5 |
| `code` | `varchar` |  | Code: STD=Premium Debt, INST=Instalment Debt, MISC=Miscellaneous | STD,INST,MISC |
| `description` | `varchar` |  | Type description | Premium Debt,Instalment Debt,Miscellaneous |
| `is_instalment` | `tinyint` |  | Flag indicating instalment type | 0,1 |
| `effective_date` | `datetime` |  | Effective date |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |

#### `CashListItem_Reverse_Reason`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Static**

Lookup: reason for reversing a cashlistitem e.g. Declined, Duplicate Receipt, Incorrect Amount.

*Keywords*: Reversal Reason

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `cashlistitem_reverse_reason_id` | `int` |  | Unique reversal reason identifier (PK) | 1,2,3 |
| `code` | `varchar` |  | Code e.g. DECLINED, DUPE, AMOUNT | DECLINED,DUPE,AMOUNT |
| `description` | `varchar` |  | Reason description | Declined Eftpos/CC,Duplicate Receipt,Incorrect Amount |
| `effective_date` | `datetime` |  | Effective date |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |

#### `CashListStatus`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Static**

Lookup: status of a cashlist e.g. Entered (E), Opened (O), Closed (C).

*Keywords*: Cashlist Status

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `cashliststatus_id` | `int` |  | Unique status identifier (PK) | 1,2,3 |
| `code` | `char` |  | Status code: E=Entered, O=Opened, C=Closed | E,O,C |
| `description` | `varchar` |  | Status description | Entered,Opened,Closed |
| `effective_date` | `datetime` |  | Date status became effective |  |
| `is_deleted` | `bit` |  | Soft delete flag | 0,1 |

#### `CashListType`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Static**

Lookup: type of a cashlist e.g. Payments (P), Receipts (R), Claim Payment (CP).

*Keywords*: Cashlist Type

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `cashlisttype_id` | `int` |  | Unique type identifier (PK) | 1,2,3 |
| `code` | `char` |  | Type code: P=Payments, R=Receipts, CP=Claim Payment | P,R,CP |
| `description` | `varchar` |  | Type description | Payments,Receipts,claim payment |
| `effective_date` | `datetime` |  | Date type became effective |  |
| `is_deleted` | `bit` |  | Soft delete flag | 0,1 |

#### `CashList_Drawer`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Static**

Cash drawer configuration linked to a bank account and a media type. Defines float amounts, receipt type, auto-close rules and user group access.

*Keywords*: Cash Drawer, Bank Account, Float

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `cashlist_drawer_id` | `int` |  | Unique drawer identifier (PK) | 1,2 |
| `bankaccount_id` | `int` |  | Primary bank account for this drawer | 1,2 |
| `deposit_bankaccount_id` | `int` |  | Bank account for depositing from this drawer |  |
| `suspense_account_id` | `int` |  | Suspense ledger account for the drawer |  |
| `collection_account_id` | `int` |  | Collection ledger account for the drawer |  |
| `cashlistitem_receipt_type_id` | `int` |  | Default receipt type for this drawer | 1,2 |
| `code` | `varchar` |  | Short code for the drawer |  |
| `description` | `varchar` |  | Description of the drawer |  |
| `multi_user` | `tinyint` |  | Flag: multiple users can use this drawer | 0,1 |
| `cash_float` | `tinyint` |  | Flag: drawer holds a cash float | 0,1 |
| `cash_float_amount` | `numeric` |  | Amount of the cash float |  |
| `auto_close` | `tinyint` |  | Flag: drawer auto-closes | 0,1 |
| `closed` | `tinyint` |  | Flag: drawer is currently closed | 0,1 |

*Foreign Keys*:
- `CashList_Drawer.bankaccount_id` â†’ `BankAccount (bankaccount_id)` (Many-to-One)
- `CashList_Drawer.deposit_bankaccount_id` â†’ `BankAccount (bankaccount_id)` (Many-to-One)
- `CashList_Drawer.cashlistitem_receipt_type_id` â†’ `CashListItem_Receipt_Type (cashlistitem_receipt_type_id)` (Many-to-One)

#### `Exchange_Rate_Override_Reason`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Static**

Lookup: reason for overriding exchange rate on a payment e.g. Processing Delay (PRODEL).

*Keywords*: Exchange Rate Override

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `exchange_rate_override_reason_id` | `int` |  | Unique reason identifier (PK) | 1 |
| `code` | `varchar` |  | Code e.g. PRODEL=Processing Delay | PRODEL |
| `description` | `varchar` |  | Reason description | Processing Delay |
| `effective_date` | `datetime` |  | Effective date |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |

#### `MediaType`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Static**

Lookup: payment media types e.g. Cheque (CQ), Cash (CA), Direct Debit (DD), Credit Card. Defines whether a type supports receipts, payments, banking, stopping etc.

*Keywords*: Media Type, Cheque, Cash, Credit Card, Direct Debit, EFT

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `mediatype_id` | `int` |  | Unique media type identifier (PK) | 1,2,3 |
| `code` | `char` |  | Media code e.g. CQ=Cheque, CA=Cash, DD=Direct Debit | CQ,CA,DD |
| `description` | `varchar` |  | Media type description | Cheque,Cash,Direct Debit |
| `is_receipt` | `tinyint` |  | Flag: this media type supports receipts | 0,1 |
| `is_payment` | `tinyint` |  | Flag: this media type supports payments | 0,1 |
| `is_banking` | `tinyint` |  | Flag: this media type is a banking type | 0,1 |
| `is_stoppable` | `tinyint` |  | Flag: payments of this type can be stopped | 0,1 |
| `effective_date` | `datetime` |  | Date media type became effective |  |
| `is_deleted` | `bit` |  | Soft delete flag | 0,1 |

#### `MediaType_Issuer`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Static**

Issuer configuration for a media type (e.g. card schemes). Defines min/max amounts and whether the issuer is allowed for claims.

*Keywords*: Media Issuer, Card Issuer

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `MediaType_Issuer_id` | `int` |  | Unique issuer identifier (PK) | 1,2 |
| `code` | `char` |  | Issuer code |  |
| `description` | `varchar` |  | Issuer description |  |
| `mediatype_id` | `int` |  | Media type this issuer belongs to |  |
| `min_amount` | `money` |  | Minimum transaction amount for this issuer |  |
| `max_amount` | `money` |  | Maximum transaction amount for this issuer |  |
| `is_allowed` | `tinyint` |  | Flag: issuer is allowed for normal use | 0,1 |
| `is_allowed_for_claims` | `tinyint` |  | Flag: issuer is allowed for claims | 0,1 |
| `effective_date` | `datetime` |  | Effective date |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |

*Foreign Keys*:
- `MediaType_Issuer.mediatype_id` â†’ `MediaType (mediatype_id)` (Many-to-One)

#### `MediaType_Status`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Static**

Lookup: status of a media/payment type item e.g. Sent For Clearance, Cleared, Bounced.

*Keywords*: Media Status, Clearance, Bounced

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `MediaType_Status_Id` | `int` |  | Unique media status identifier (PK) | 1,2,3 |
| `code` | `char` |  | Status code e.g. SRPS=Sent For Clearance, SRPC=Cleared, SRPB=Bounced | SRPS,SRPC,SRPB |
| `description` | `varchar` |  | Status description | Sent For Clearance,Cleared,Bounced |
| `effective_date` | `datetime` |  | Date status became effective |  |
| `is_deleted` | `bit` |  | Soft delete flag | 0,1 |

#### `cashlist`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Cash list header grouping receipts and payments. Each cashlist represents a batch of financial transactions managed by a user, linked to a bank account and tracked by status.

*Keywords*: Cash List, Receipt, Payment, Banking, Cash, Cashlist

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `cashlist_id` | `int` |  | Unique cashlist identifier (PK) | 1,2,3 |
| `bankaccount_id` | `int` |  | Bank account this cashlist is banked to (FK to BankAccount) | 3,4,5 |
| `cashlisttype_id` | `int` |  | Type of cashlist: Payments, Receipts or Claim Payment | 1,2,3 |
| `cashliststatus_id` | `int` |  | Current status: Entered, Opened, Closed | 1,2,3 |
| `cashlist_ref` | `varchar` |  | Cashlist reference number |  |
| `company_id` | `int` |  | Company identifier | 1 |
| `sub_branch_id` | `int` |  | Sub-branch identifier | 1 |
| `currency_id` | `smallint` |  | Currency of the cashlist (FK to Currency) | 26,27 |
| `list_date` | `datetime` |  | Date the cashlist was created |  |
| `control_total` | `numeric` |  | Expected total amount for all items on the cashlist | 0.00 |
| `item_count` | `int` |  | Number of items on the cashlist | 0 |
| `cashlist_drawer_id` | `int` |  | Cash drawer associated with this cashlist |  |
| `batch_id` | `int` |  | Batch this cashlist belongs to |  |
| `pmuser_id` | `smallint` |  | User who created the cashlist | 1,23 |
| `confirm_pmuser_id` | `smallint` |  | First approving user |  |
| `confirm2_pmuser_id` | `smallint` |  | Second approving user |  |
| `date_approved` | `datetime` |  | Date the cashlist was approved |  |
| `banking_total` | `numeric` |  | Total amount banked |  |
| `cash_float_amount` | `numeric` |  | Float amount held in the cash drawer |  |
| `date_deposited` | `datetime` |  | Date the cashlist was physically deposited at the bank |  |
| `base_currency_id` | `smallint` |  | Base currency identifier (FK to Currency) | 26 |
| `is_split_receipt` | `tinyint` |  | Flag indicating if this cashlist contains split receipts | 0,1 |
| `PMNav_Batch_Key` | `int` |  | Integration key for PMNav batch processing |  |

*Foreign Keys*:
- `cashlist.bankaccount_id` â†’ `BankAccount (bankaccount_id)` (Many-to-One)
- `cashlist.cashlisttype_id` â†’ `CashListType (cashlisttype_id)` (Many-to-One)
- `cashlist.cashliststatus_id` â†’ `CashListStatus (cashliststatus_id)` (Many-to-One)
- `cashlist.currency_id` â†’ `currency (currency_id)` (Many-to-One)
- `cashlist.base_currency_id` â†’ `currency (currency_id)` (Many-to-One)
- `cashlist.pmuser_id` â†’ `PMUser (user_id)` (Many-to-One)
- `cashlist.confirm_pmuser_id` â†’ `PMUser (user_id)` (Many-to-One)
- `cashlist.confirm2_pmuser_id` â†’ `PMUser (user_id)` (Many-to-One)
- `cashlist.batch_id` â†’ `Batch (batch_id)` (Many-to-One)
- `cashlist.cashlist_drawer_id` â†’ `CashList_Drawer (cashlist_drawer_id)` (Many-to-One)

#### `cashlistitem`
**Domain: Finance**  **Owner: Finance Department**  **Refresh: Real-time**

Individual receipt or payment line within a cashlist. Contains full payment details including media type (cheque/cash/card), amount, allocation status, bank details, currency exchange rates and reversal tracking.

*Keywords*: Cash Item, Receipt, Payment, Cheque, Credit Card, Allocation, Bank

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `cashlistitem_id` | `int` |  | Unique cashlist item identifier (PK) | 1,2,3 |
| `cashlist_id` | `int` |  | Parent cashlist (FK to cashlist) | 1,2,3 |
| `account_id` | `int` |  | Account the item is posted to (FK to Account) | 4394,4419 |
| `allocationstatus_id` | `int` |  | Allocation status: Unallocated, Posted, Allocated | 1,2,3,4 |
| `mediatype_id` | `int` |  | Payment media type e.g. Cheque, Cash, Credit Card | 1,2,9 |
| `media_ref` | `varchar` |  | Media reference e.g. cheque number |  |
| `our_ref` | `varchar` |  | Our internal reference for this payment/receipt |  |
| `their_ref` | `varchar` |  | Counterparty reference for this payment/receipt |  |
| `amount` | `numeric` |  | Amount of the cashlist item (positive=receipt, negative=payment) | -600,90000 |
| `transdetail_id` | `int` |  | Links to the transaction detail record (FK to TransDetail) | 132,608 |
| `contact_name` | `varchar` |  | Name of the contact for this item |  |
| `address1` | `varchar` |  | Address line 1 for payment/receipt |  |
| `address2` | `varchar` |  | Address line 2 |  |
| `address3` | `varchar` |  | Address line 3 |  |
| `address4` | `varchar` |  | Address line 4 |  |
| `postal_code` | `varchar` |  | Postal code for payment/receipt address |  |
| `address_country` | `smallint` |  | Country code for the address | 1 |
| `payment_name` | `varchar` |  | Name of the payee/payer |  |
| `payment_account_code` | `varchar` |  | Bank account code for payment routing |  |
| `payment_branch_code` | `varchar` |  | Bank branch code for payment routing |  |
| `payment_expiry_date` | `datetime` |  | Expiry date relevant to the payment (e.g. card expiry) |  |
| `payment_reference1` | `varchar` |  | Additional payment reference 1 |  |
| `payment_reference2` | `varchar` |  | Additional payment reference 2 |  |
| `letter` | `tinyint` |  | Flag indicating if a letter has been generated for this item | 0,1 |
| `transaction_date` | `datetime` |  | Date of the transaction |  |
| `cashlistitem_receipt_type_id` | `int` |  | Receipt type: Premium Debt, Instalment Debt, Miscellaneous | 1,2,5 |
| `cashlistitem_receipt_status_id` | `int` |  | Receipt status: Added, Exported, Processed | 1,2,3 |
| `cashlistitem_payment_type_id` | `int` |  | Payment type: Claim, Commission, RI etc. | 1,2,3 |
| `cashlistitem_payment_status_id` | `int` |  | Payment status: Issued, Presented, Cancelled etc. | 1,2,3 |
| `cashlistitem_bank_id` | `int` |  | Bank name lookup for this item (FK to CashListItem_Bank) | 1,2 |
| `cheque_date` | `datetime` |  | Date on the cheque |  |
| `cc_number` | `varchar` |  | Credit card number (masked/tokenised) |  |
| `cc_expiry_date` | `varchar` |  | Credit card expiry date | 12/12 |
| `cc_start_date` | `varchar` |  | Credit card start date |  |
| `cc_issue` | `varchar` |  | Credit card issue number |  |
| `cc_pin` | `varchar` |  | Credit card PIN (sensitive/encrypted) |  |
| `cc_auth_code` | `varchar` |  | Credit card authorisation code |  |
| `original_amount` | `numeric` |  | Original amount before any changes | -600,90000 |
| `amount_tendered` | `numeric` |  | Amount tendered by the payer |  |
| `change` | `money` |  | Change given back to the payer | 0.00 |
| `batch_id` | `int` |  | Batch this item belongs to (FK to Batch) |  |
| `pmuser_id` | `smallint` |  | User who created this item | 1,23 |
| `receipt_details` | `varchar` |  | Free text receipt details |  |
| `cashlistitem_reverse_pmuser_id` | `smallint` |  | User who performed the reversal |  |
| `cashlistitem_reverse_reason_id` | `int` |  | Reason for reversing this item |  |
| `date_presented` | `datetime` |  | Date the cheque was presented |  |
| `cheque_in_possession` | `tinyint` |  | Flag indicating cheque is physically held | 0,1 |
| `stop_requested_date` | `datetime` |  | Date a stop was requested on the cheque |  |
| `stop_printed_date` | `datetime` |  | Date the stop notice was printed |  |
| `stop_confirmation_date` | `datetime` |  | Date the stop was confirmed |  |
| `reason` | `varchar` |  | Reason notes for this item |  |
| `replaces_cashlistitem_id` | `int` |  | ID of the item this one replaces (self-reference) |  |
| `superceded_by_id` | `int` |  | ID of the item that superseded this one (self-reference) |  |
| `xml_object` | `text` |  | XML data payload for the item |  |
| `cheque_reminder_print_date` | `datetime` |  | Date a cheque reminder was printed |  |
| `underwriting_year_id` | `int` |  | Underwriting year identifier (FK to Underwriting_Year) |  |
| `exchange_rate_override_reason_id` | `int` |  | Reason for overriding the exchange rate |  |
| `currency_base_xrate` | `float` |  | Exchange rate from transaction currency to base currency | 1.2,0.9,1.0 |
| `currency_base_date` | `datetime` |  | Date the currency base rate was applied |  |
| `account_base_xrate` | `float` |  | Exchange rate from account currency to base currency | 1.0 |
| `account_base_date` | `datetime` |  | Date the account base rate was applied |  |
| `system_base_xrate` | `float` |  | Exchange rate from system currency to base currency | 1.0 |
| `system_base_date` | `datetime` |  | Date the system base rate was applied |  |
| `is_reversed` | `smallint` |  | Flag indicating item has been reversed | 0,1 |
| `mediatype_issuer_id` | `int` |  | Media type issuer (card scheme) identifier |  |
| `cc_name` | `varchar` |  | Name on the credit card |  |
| `cc_customer` | `varchar` |  | Customer name for the card transaction |  |
| `cc_manual_auth_code` | `varchar` |  | Manually entered authorisation code |  |
| `cc_transaction_code` | `varchar` |  | Credit card transaction code |  |
| `is_exported` | `tinyint` |  | Flag indicating this item has been exported | 0,1 |
| `reversed_date` | `datetime` |  | Date this item was reversed |  |
| `cashlistitem_reversal_transdetail_id` | `int` |  | Transaction detail record for the reversal posting |  |
| `party_bank_id` | `int` |  | Party bank details used for this payment (FK to Party_Bank) |  |
| `collection_date` | `datetime` |  | Date the payment is due for collection |  |
| `comments` | `varchar` |  | General comments on the item |  |
| `MediaType_Status_Id` | `int` |  | Current status of the media item e.g. Sent For Clearance, Cleared | 1,2,3 |
| `bank_location` | `varchar` |  | Bank location description |  |
| `bank_branch` | `varchar` |  | Bank branch description |  |
| `chequetype_id` | `int` |  | Cheque type identifier (FK to ChequeType) |  |
| `Cheque_clearing_type_id` | `int` |  | Cheque clearing type identifier |  |
| `cc_bank_id` | `int` |  | Credit card bank identifier |  |
| `type_of_card_id` | `int` |  | Type of credit/debit card identifier |  |
| `cc_trans_slip_no` | `varchar` |  | Credit card transaction slip number |  |
| `cc_tracking_number` | `varchar` |  | Credit card tracking number |  |
| `tax_band_id` | `int` |  | Tax band identifier (FK to tax_band) |  |
| `is_lead` | `tinyint` |  | Flag indicating if this is the lead item in a split receipt | 0,1 |
| `split_total` | `numeric` |  | Total for split receipt grouping |  |
| `TaxAmount` | `numeric` |  | Tax amount applicable to this item |  |
| `business_identifier_code` | `varchar` |  | BIC/SWIFT code for international payments |  |
| `international_bank_account_number` | `varchar` |  | IBAN for international payments |  |
| `cashlistitem_Bank` | `varchar` |  | Free-text bank name for this item |  |
| `cc_insurance_file_cnt` | `int` |  | Policy version linked to a credit card payment |  |
| `cc_token_id` | `varchar` |  | Tokenised credit card identifier |  |
| `authorization_comment` | `varchar` |  | Authorisation comment for the payment |  |
| `insurance_ref` | `varchar` |  | Policy reference associated with this item |  |

*Foreign Keys*:
- `cashlistitem.cashlist_id` â†’ `cashlist (cashlist_id)` (Many-to-One)
- `cashlistitem.account_id` â†’ `account (account_id)` (Many-to-One)
- `cashlistitem.allocationstatus_id` â†’ `AllocationStatus (allocationstatus_id)` (Many-to-One)
- `cashlistitem.mediatype_id` â†’ `MediaType (mediatype_id)` (Many-to-One)
- `cashlistitem.cashlistitem_bank_id` â†’ `CashListItem_Bank (cashlistitem_bank_id)` (Many-to-One)
- `cashlistitem.cashlistitem_payment_status_id` â†’ `CashListItem_Payment_Status (cashlistitem_payment_status_id)` (Many-to-One)
- `cashlistitem.cashlistitem_payment_type_id` â†’ `CashListItem_Payment_Type (cashlistitem_payment_type_id)` (Many-to-One)
- `cashlistitem.cashlistitem_receipt_status_id` â†’ `CashListItem_Receipt_Status (cashlistitem_receipt_status_id)` (Many-to-One)
- `cashlistitem.cashlistitem_receipt_type_id` â†’ `CashListItem_Receipt_Type (cashlistitem_receipt_type_id)` (Many-to-One)
- `cashlistitem.cashlistitem_reverse_pmuser_id` â†’ `PMUser (user_id)` (Many-to-One)
- `cashlistitem.cashlistitem_reverse_reason_id` â†’ `CashListItem_Reverse_Reason (cashlistitem_reverse_reason_id)` (Many-to-One)
- `cashlistitem.batch_id` â†’ `Batch (batch_id)` (Many-to-One)
- `cashlistitem.mediatype_issuer_id` â†’ `MediaType_Issuer (MediaType_Issuer_id)` (Many-to-One)
- `cashlistitem.party_bank_id` â†’ `Party_Bank (party_bank_id)` (Many-to-One)
- `cashlistitem.MediaType_Status_Id` â†’ `MediaType_Status (MediaType_Status_Id)` (Many-to-One)
- `cashlistitem.chequetype_id` â†’ `ChequeType (ChequeType_id)` (Many-to-One)
- `cashlistitem.Cheque_clearing_type_id` â†’ `Cheque_Clearing_Type (Cheque_Clearing_Type_id)` (Many-to-One)
- `cashlistitem.underwriting_year_id` â†’ `Underwriting_Year (underwriting_year_id)` (Many-to-One)
- `cashlistitem.exchange_rate_override_reason_id` â†’ `Exchange_Rate_Override_Reason (exchange_rate_override_reason_id)` (Many-to-One)
- `cashlistitem.cashlistitem_reversal_transdetail_id` â†’ `transdetail (transdetail_id)` (Many-to-One)
- `cashlistitem.tax_band_id` â†’ `tax_band (tax_band_id)` (Many-to-One)
- `cashlistitem.transdetail_id` â†’ `transdetail (transdetail_id)` (Many-to-One)
- `cashlistitem.cc_insurance_file_cnt` â†’ `insurance_file (insurance_file_cnt)` (Many-to-One)
- `cashlistitem.replaces_cashlistitem_id` â†’ `cashlistitem (cashlistitem_id)` (Many-to-One)
- `cashlistitem.superceded_by_id` â†’ `cashlistitem (cashlistitem_id)` (Many-to-One)

---

## Reinsurance

**Tables in this module**: 27

### Query Rules

**All RI (Reinsurance) Cession Transactions** â€” `JOIN ri_arrangement_line ON ri_arrangement_line.ri_arrangement_id = ri_arrangement.ri_arrangement_id AND ri_arrangement.covers policy product/class`  
*Reinsurance cession transactions are linked to policies and reinsurance arrangement lines. Join ri_arrangement_line to the relevant policy transdetail via the document. The ri_arrangement_line holds the reinsurer party, the cession percentage and the net RI premium.*

**All Active Reinsurers** â€” `ri_arrangement_line.reinsurer_cnt = party.party_cnt`  
*Reinsurance companies (parties) are identified by their party_type. They appear as the counterparty in ri_arrangement_line. Join to party via the reinsurer_cnt field on ri_arrangement_line to get reinsurer names and details.*

### Tables

#### `Claim_RI_Arrangement`
**Domain: Reinsurance**  **Owner: Finance Department**  **Refresh: Real-time**

Reinsurance arrangement for a specific claim version. Links the claim to the RI model used and holds ceded reserve, ceded payment and RI recovery amounts. Same as Claim_RI_Arrangement.

*Keywords*: claim reinsurance, claim RI, ceded reserve, ceded payment, RI recovery, salvage recovery, claim RI arrangement, proportional, XOL

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `claim_id` | `int` |  | FK/PK to Claim |  |
| `ri_arrangement_id` | `int` |  | PK — RI arrangement version identifier |  |
| `risk_cnt` | `int` |  | FK to Risk |  |
| `ri_band_id` | `int` |  | FK to RI_Band |  |
| `ri_model_id` | `int` |  | FK to RI_Model |  |
| `claim_allocation_type` | `tinyint` |  | Allocation type code for claim RI |  |
| `sum_insured` | `money` |  | Sum insured ceded on this claim RI arrangement |  |
| `reserve` | `money` |  | Current ceded reserve amount |  |
| `payment` | `money` |  | Ceded payment amount settled to date |  |
| `salvage` | `money` |  | Ceded salvage recovered |  |
| `recovery` | `money` |  | Ceded recovery amount |  |
| `is_modified` | `tinyint` |  | Flag: arrangement was manually modified | 0,1 |
| `this_reserve` | `money` |  | Reserve movement on this transaction |  |
| `this_payment` | `money` |  | Payment movement on this transaction |  |
| `this_salvage` | `money` |  | Salvage movement on this transaction |  |
| `this_recovery` | `money` |  | Recovery movement on this transaction |  |
| `claim_ri_arrangement_id` | `int` |  | Surrogate unique identifier for this arrangement record |  |
| `base_claim_ri_arrangement_id` | `int` |  | Link to the base (original) arrangement record |  |
| `version_id` | `int` |  | Version number of this arrangement |  |
| `original_ri_arrangement_id` | `int` |  | Original RI arrangement identifier before modifications |  |
| `ri_arrangement_version` | `int` |  | Version of the linked RI arrangement |  |
| `Cloned` | `tinyint` |  | Flag: this record was cloned from another | 0,1 |
| `xol_ri_model_id` | `int` |  | FK to RI_Model for XOL layer |  |
| `incurred_to_date` | `money` |  | Total incurred amount ceded to date |  |
| `reserve_to_date` | `money` |  | Total reserve ceded to date |  |
| `payment_to_date` | `money` |  | Total payments ceded to date |  |
| `salvage_to_date` | `money` |  | Total salvage ceded to date |  |
| `recovery_to_date` | `money` |  | Total recovery ceded to date |  |
| `extended_limit_amount` | `money` |  | Extended limit amount applied to this arrangement |  |

*Foreign Keys*:
- `Claim_RI_Arrangement.claim_id` â†’ `Claim (Claim_id)` (Many-to-One)
- `Claim_RI_Arrangement.ri_band_id` â†’ `RI_Band (ri_band_id)` (Many-to-One)
- `Claim_RI_Arrangement.ri_model_id` â†’ `RI_Model (ri_model_id)` (Many-to-One)
- `Claim_RI_Arrangement.risk_cnt` â†’ `Risk (risk_cnt)` (Many-to-One)

#### `Claim_RI_Arrangement_Archive`
**Domain: Reinsurance**  **Owner: Finance Department**  **Refresh: Real-time**

Archived historical versions of claim reinsurance arrangements (mirrors Claim_RI_Arrangement)

*Keywords*: Reinsurance, RI, Claim, Archive, Reserve, Payment

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `claim_id` | `int` |  | FK/PK to Claim |  |
| `ri_arrangement_id` | `int` |  | PK — RI arrangement version identifier |  |
| `risk_cnt` | `int` |  | FK to Risk |  |
| `ri_band_id` | `int` |  | FK to RI_Band |  |
| `ri_model_id` | `int` |  | FK to RI_Model |  |
| `claim_allocation_type` | `tinyint` |  | Allocation type code for claim RI |  |
| `sum_insured` | `money` |  | Sum insured ceded on this claim RI arrangement |  |
| `reserve` | `money` |  | Current ceded reserve amount |  |
| `payment` | `money` |  | Ceded payment amount settled to date |  |
| `salvage` | `money` |  | Ceded salvage recovered |  |
| `recovery` | `money` |  | Ceded recovery amount |  |
| `is_modified` | `tinyint` |  | Flag: arrangement was manually modified | 0,1 |
| `this_reserve` | `money` |  | Reserve movement on this transaction |  |
| `this_payment` | `money` |  | Payment movement on this transaction |  |
| `this_salvage` | `money` |  | Salvage movement on this transaction |  |
| `this_recovery` | `money` |  | Recovery movement on this transaction |  |
| `claim_ri_arrangement_id` | `int` |  | Surrogate unique identifier for this arrangement record |  |
| `base_claim_ri_arrangement_id` | `int` |  | Link to the base (original) arrangement record |  |
| `version_id` | `int` |  | Version number of this arrangement |  |
| `original_ri_arrangement_id` | `int` |  | Original RI arrangement identifier before modifications |  |
| `ri_arrangement_version` | `int` |  | Version of the linked RI arrangement |  |
| `Cloned` | `tinyint` |  | Flag: this record was cloned from another | 0,1 |
| `xol_ri_model_id` | `int` |  | FK to RI_Model for XOL layer |  |
| `incurred_to_date` | `money` |  | Total incurred amount ceded to date |  |
| `reserve_to_date` | `money` |  | Total reserve ceded to date |  |
| `payment_to_date` | `money` |  | Total payments ceded to date |  |
| `salvage_to_date` | `money` |  | Total salvage ceded to date |  |
| `recovery_to_date` | `money` |  | Total recovery ceded to date |  |
| `extended_limit_amount` | `money` |  | Extended limit amount applied to this arrangement |  |

*Foreign Keys*:
- `Claim_RI_Arrangement_Archive.claim_id` â†’ `Claim (Claim_id)` (Many-to-One)

#### `Claim_RI_Arrangement_Line`
**Domain: Reinsurance**  **Owner: Finance Department**  **Refresh: Real-time**

Individual reinsurance line of a claim RI arrangement showing ceded amounts per reinsurer/treaty for a specific claim. Same as Claim_RI_Arrangement_Line.

*Keywords*: claim RI line, claim reinsurance, ceded reserve, ceded payment, treaty, RI party, claim RI split, reinsurer claim share

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `claim_id` | `int` |  | FK/PK to Claim |  |
| `ri_arrangement_line_id` | `int` |  | PK — RI arrangement line identifier |  |
| `ri_arrangement_id` | `int` |  | FK to Claim_RI_Arrangement |  |
| `type` | `varchar` |  | Type of RI line (Proportional, XOL) |  |
| `treaty_id` | `int` |  | FK to Treaty |  |
| `party_cnt` | `int` |  | FK to Party (reinsurer) |  |
| `xol_arrangement_id` | `int` |  | FK to Claim_XOL_Arrangement (for XOL layers) |  |
| `default_share_percent` | `float` |  | Default share percentage for this reinsurer |  |
| `this_share_percent` | `float` |  | Actual share percentage on this arrangement |  |
| `agreement_code` | `varchar` |  | Reinsurance agreement code |  |
| `priority` | `int` |  | Priority order of this line |  |
| `number_of_lines` | `smallint` |  | Number of lines on this arrangement |  |
| `line_limit` | `money` |  | Line limit amount |  |
| `sum_insured` | `money` |  | Sum insured ceded on this line |  |
| `reserve` | `money` |  | Current ceded reserve on this line |  |
| `payment` | `money` |  | Ceded payment on this line |  |
| `salvage` | `money` |  | Ceded salvage on this line |  |
| `recovery` | `money` |  | Ceded recovery on this line |  |
| `this_reserve` | `money` |  | Reserve movement on this transaction |  |
| `this_payment` | `money` |  | Payment movement on this transaction |  |
| `this_salvage` | `money` |  | Salvage movement on this transaction |  |
| `this_recovery` | `money` |  | Recovery movement on this transaction |  |
| `claim_ri_arrangement_line_id` | `int` |  | Surrogate unique identifier for this line record |  |
| `base_claim_ri_arrangement_line_id` | `int` |  | Link to the base (original) arrangement line record |  |
| `version_id` | `int` |  | Version number of this line |  |
| `original_ri_arrangement_line_id` | `int` |  | Original line identifier before modifications |  |
| `retained` | `float` |  | Retained percentage (not ceded) |  |
| `lower_limit` | `money` |  | Lower limit for XOL layers |  |
| `participation_percent` | `float` |  | Reinsurer participation percentage |  |
| `grouping` | `int` |  | Grouping identifier for layered XOL |  |
| `Is_Obligatory` | `tinyint` |  | Flag: obligatory treaty (vs facultative) | 0,1 |
| `ri_model_line_id` | `int` |  | FK to RI model line definition |  |
| `reserve_to_date` | `money` |  | Total reserve ceded to date on this line |  |
| `payment_to_date` | `money` |  | Total payments ceded to date on this line |  |
| `salvage_to_date` | `money` |  | Total salvage ceded to date on this line |  |
| `recovery_to_date` | `money` |  | Total recovery ceded to date on this line |  |
| `claim_incurred_to_date` | `money` |  | Total claim incurred amount to date on this line |  |
| `is_pt_archive` | `tinyint` |  | Flag/PK: marks if this is a proportional treaty archive record | 0,1 |

*Foreign Keys*:
- `Claim_RI_Arrangement_Line.claim_id` â†’ `Claim_RI_Arrangement (claim_id)` (Many-to-One)
- `Claim_RI_Arrangement_Line.ri_arrangement_id` â†’ `Claim_RI_Arrangement (ri_arrangement_id)` (Many-to-One)
- `Claim_RI_Arrangement_Line.treaty_id` â†’ `Treaty (treaty_id)` (Many-to-One)
- `Claim_RI_Arrangement_Line.party_cnt` â†’ `Party (party_cnt)` (Many-to-One)
- `Claim_RI_Arrangement_Line.xol_arrangement_id` â†’ `Claim_XOL_Arrangement (xol_arrangement_id)` (Many-to-One)

#### `Claim_RI_Arrangement_Line_Archive`
**Domain: Reinsurance**  **Owner: Finance Department**  **Refresh: Real-time**

Archived historical versions of claim reinsurance arrangement lines

*Keywords*: Reinsurance, RI, Claim, Line, Archive, Treaty

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `claim_id` | `int` |  | FK/PK to Claim |  |
| `ri_arrangement_line_id` | `int` |  | PK — RI arrangement line identifier |  |
| `ri_arrangement_id` | `int` |  | FK to Claim_RI_Arrangement |  |
| `type` | `varchar` |  | Type of RI line (Proportional, XOL) |  |
| `treaty_id` | `int` |  | FK to Treaty |  |
| `party_cnt` | `int` |  | FK to Party (reinsurer) |  |
| `xol_arrangement_id` | `int` |  | FK to Claim_XOL_Arrangement (for XOL layers) |  |
| `default_share_percent` | `float` |  | Default share percentage for this reinsurer |  |
| `this_share_percent` | `float` |  | Actual share percentage on this arrangement |  |
| `agreement_code` | `varchar` |  | Reinsurance agreement code |  |
| `priority` | `int` |  | Priority order of this line |  |
| `number_of_lines` | `smallint` |  | Number of lines on this arrangement |  |
| `line_limit` | `money` |  | Line limit amount |  |
| `sum_insured` | `money` |  | Sum insured ceded on this line |  |
| `reserve` | `money` |  | Current ceded reserve on this line |  |
| `payment` | `money` |  | Ceded payment on this line |  |
| `salvage` | `money` |  | Ceded salvage on this line |  |
| `recovery` | `money` |  | Ceded recovery on this line |  |
| `this_reserve` | `money` |  | Reserve movement on this transaction |  |
| `this_payment` | `money` |  | Payment movement on this transaction |  |
| `this_salvage` | `money` |  | Salvage movement on this transaction |  |
| `this_recovery` | `money` |  | Recovery movement on this transaction |  |
| `claim_ri_arrangement_line_id` | `int` |  | Surrogate unique identifier for this line record |  |
| `base_claim_ri_arrangement_line_id` | `int` |  | Link to the base (original) arrangement line record |  |
| `version_id` | `int` |  | Version number of this line |  |
| `original_ri_arrangement_line_id` | `int` |  | Original line identifier before modifications |  |
| `retained` | `float` |  | Retained percentage (not ceded) |  |
| `lower_limit` | `money` |  | Lower limit for XOL layers |  |
| `participation_percent` | `float` |  | Reinsurer participation percentage |  |
| `grouping` | `int` |  | Grouping identifier for layered XOL |  |
| `Is_Obligatory` | `tinyint` |  | Flag: obligatory treaty (vs facultative) | 0,1 |
| `ri_model_line_id` | `int` |  | FK to RI model line definition |  |
| `reserve_to_date` | `money` |  | Total reserve ceded to date on this line |  |
| `payment_to_date` | `money` |  | Total payments ceded to date on this line |  |
| `salvage_to_date` | `money` |  | Total salvage ceded to date on this line |  |
| `recovery_to_date` | `money` |  | Total recovery ceded to date on this line |  |
| `claim_incurred_to_date` | `money` |  | Total claim incurred amount to date on this line |  |
| `is_pt_archive` | `tinyint` |  | Flag/PK: marks if this is a proportional treaty archive record | 0,1 |

*Foreign Keys*:
- `Claim_RI_Arrangement_Line_Archive.claim_id` â†’ `Claim_RI_Arrangement (claim_id)` (Many-to-One)

#### `Claim_RI_Arrangement_line_Broker_Participants`
**Domain: Reinsurance**  **Owner: Finance Department**  **Refresh: Real-time**

Reinsurance broker participants on a claim arrangement line with their participation percentage

*Keywords*: Reinsurance, RI, Claim, Broker, Participant, Placement, Percentage

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `claim_ri_arrangement_line_id` | `int` |  | FK to Claim_RI_Arrangement_Line |  |
| `ri_party_cnt` | `int` |  | FK to Party (reinsurance broker) |  |
| `participation_percent` | `float` |  | Broker participation percentage on this line |  |

*Foreign Keys*:
- `Claim_RI_Arrangement_line_Broker_Participants.claim_ri_arrangement_line_id` â†’ `Claim_RI_Arrangement_Line (claim_ri_arrangement_line_id)` (Many-to-One)
- `Claim_RI_Arrangement_line_Broker_Participants.ri_party_cnt` â†’ `Party (party_cnt)` (Many-to-One)

#### `Claim_RI_Arrangement_line_Broker_Participants_Archive`
**Domain: Reinsurance**  **Owner: Finance Department**  **Refresh: Real-time**

Archived historical versions of reinsurance broker participants on claim arrangement lines

*Keywords*: Reinsurance, RI, Claim, Broker, Participant, Archive

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `claim_ri_arrangement_line_id` | `int` |  | FK to Claim_RI_Arrangement_Line |  |
| `ri_party_cnt` | `int` |  | FK to Party (reinsurance broker) |  |
| `participation_percent` | `float` |  | Broker participation percentage on this line |  |

*Foreign Keys*:
- `Claim_RI_Arrangement_line_Broker_Participants_Archive.claim_ri_arrangement_line_id` â†’ `Claim_RI_Arrangement_Line (claim_ri_arrangement_line_id)` (Many-to-One)

#### `Date_for_Treaty_XOL_Calculation`
**Domain: Reinsurance**  **Owner: Reinsurance Department**  **Refresh: Static**

Lookup defining which date basis is used when determining whether a claim or risk falls within an XOL (Excess of Loss) reinsurance treaty period. RISK = risk attachment date; TRANS = transaction posting date; LOSS = date of loss. The choice affects which treaty year responds.

*Keywords*: XOL, excess of loss, reinsurance, treaty, date basis, calculation

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `Date_for_Treaty_XOL_Calculation_id` | `int` | âœ“ | PK — unique identifier for this date basis option. | 1, 2, 3 |
| `code` | `varchar(20)` |  | Short code for the date basis. RISK = risk attachment date, TRANS = transaction date, LOSS = loss date. | RISK, TRANS, LOSS |
| `Description` | `varchar(50)` |  | Human-readable description. e.g. Risk Attachment Date, Transaction Date. | Risk Attachment Date, Transaction Date |
| `caption_id` | `int` |  | FK to PMCaption — multilingual label for this option. | 1361, 1362 |
| `effective_date` | `datetime` |  | Date from which this option is effective. |  |
| `is_deleted` | `tinyint` |  | 0 = active, 1 = soft-deleted. | 0, 1 |

#### `Proportional_RI_Calculation_Method`
**Domain: Reinsurance**  **Owner: Reinsurance Department**  **Refresh: Static**

Lookup controlling how proportional (quota share / surplus) reinsurance premiums are allocated to treaty periods: UNDERWRYR = by underwriting year (risk inception year), ACCOUNTYR = by accounting year (transaction posting year). The selected method affects RI premium cession timing.

*Keywords*: proportional, reinsurance, quota share, surplus, calculation method, underwriting year, accounting year

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `Proportional_RI_Calculation_Method_id` | `int` | âœ“ | PK — unique identifier for this calculation method option. | 1, 2 |
| `caption_id` | `int` |  | FK to PMCaption — multilingual display label. | 65430, 65438 |
| `code` | `char(10)` |  | Short code. UNDERWRYR = Underwriting Year, ACCOUNTYR = Accounting Year. | UNDERWRYR, ACCOUNTYR |
| `description` | `varchar(255)` |  | Full description shown in the UI. e.g. Underwriting Year, Accounting Year. | Underwriting Year, Accounting Year |
| `is_deleted` | `tinyint` |  | 0 = active, 1 = soft-deleted. | 0, 1 |
| `effective_date` | `datetime` |  | Date from which this method is effective. |  |

#### `RI_Arrangement_Line_Archive`
**Domain: Reinsurance**  **Owner: Finance Department**  **Refresh: Real-time**

Archived historical versions of reinsurance arrangement lines (mirrors RI_Arrangement_Line)

*Keywords*: Reinsurance, RI, Arrangement Line, Archive, Treaty, Party

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `ri_arrangement_line_id` | `int` |  | FK to RI_Arrangement_Line (original line) |  |
| `ri_arrangement_id` | `int` |  | FK to RI_Arrangement |  |
| `type` | `varchar` |  | Type of RI line (e.g. Proportional, XOL) |  |
| `treaty_id` | `int` |  | FK to Treaty |  |
| `party_cnt` | `int` |  | FK to Party (reinsurer) |  |
| `default_share_percent` | `float` |  | Default share percentage for this reinsurer |  |
| `this_share_percent` | `float` |  | Actual share percentage on this arrangement |  |
| `premium_percent` | `float` |  | Premium ceded percentage |  |
| `commission_percent` | `float` |  | Reinsurance commission percentage |  |
| `agreement_code` | `varchar` |  | Reinsurance agreement code |  |
| `priority` | `int` |  | Priority order of this line |  |
| `number_of_lines` | `smallint` |  | Number of lines on this arrangement |  |
| `line_limit` | `money` |  | Line limit amount |  |
| `sum_insured` | `money` |  | Sum insured ceded on this line |  |
| `premium_value` | `money` |  | Premium value ceded |  |
| `commission_value` | `money` |  | Commission value on this line |  |
| `premium_tax` | `money` |  | Premium tax on ceded premium |  |
| `commission_tax` | `money` |  | Tax on reinsurance commission |  |
| `is_commission_modified` | `tinyint` |  | Flag: commission was manually modified | 0,1 |
| `retained` | `float` |  | Retained percentage (not ceded) |  |
| `lower_limit` | `money` |  | Lower limit for XOL layers |  |
| `participation_percent` | `float` |  | Reinsurer participation percentage |  |
| `grouping` | `int` |  | Grouping identifier for layered XOL |  |
| `ri_model_line_id` | `int` |  | FK to RI model line definition |  |
| `Is_Obligatory` | `tinyint` |  | Flag: obligatory treaty (vs facultative) | 0,1 |
| `ri_arrangement_line_Version_id` | `tinyint` |  | Version identifier for this archived line |  |
| `created_date` | `datetime` |  | Date this archive record was created |  |

*Foreign Keys*:
- `RI_Arrangement_Line_Archive.ri_arrangement_id` â†’ `RI_Arrangement (ri_arrangement_id)` (Many-to-One)

#### `RI_Arrangement_line_Broker_Participants`
**Domain: Reinsurance**  **Owner: Finance Department**  **Refresh: Real-time**

Reinsurance broker participants on an arrangement line with their participation percentage

*Keywords*: Reinsurance, RI, Broker, Participant, Placement, Percentage

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `ri_arrangement_line_id` | `int` |  | FK to RI_Arrangement_Line |  |
| `ri_party_cnt` | `int` |  | FK to Party (reinsurance broker) |  |
| `participation_percent` | `float` |  | Broker participation percentage on this line |  |

*Foreign Keys*:
- `RI_Arrangement_line_Broker_Participants.ri_arrangement_line_id` â†’ `RI_Arrangement_Line (ri_arrangement_line_id)` (Many-to-One)
- `RI_Arrangement_line_Broker_Participants.ri_party_cnt` â†’ `Party (party_cnt)` (Many-to-One)

#### `RI_Arrangement_line_Broker_Participants_Archive`
**Domain: Reinsurance**  **Owner: Finance Department**  **Refresh: Real-time**

Archived historical versions of reinsurance broker participants on arrangement lines

*Keywords*: Reinsurance, RI, Broker, Participant, Archive

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `ri_arrangement_line_id` | `int` |  | FK to RI_Arrangement_Line (archived line) |  |
| `ri_party_cnt` | `int` |  | FK to Party (reinsurance broker) |  |
| `participation_percent` | `float` |  | Broker participation percentage (archived) |  |

*Foreign Keys*:
- `RI_Arrangement_line_Broker_Participants_Archive.ri_arrangement_line_id` â†’ `RI_Arrangement_Line (ri_arrangement_line_id)` (Many-to-One)

#### `Risk_RI_Status`
**Domain: Reinsurance**  **Owner: Underwriting**  **Refresh: Static**

Lookup defining the reinsurance status of a risk or RI arrangement: e.g. Active, Cancelled, Expired. Used to track the current state of RI arrangements on a risk.

*Keywords*: RI status, reinsurance status, active, cancelled, expired, risk RI, lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `status_id` | `int` | âœ“ | Primary key for the RI status. |  |
| `code` | `char` |  | Short code for the RI status. | ACT,CAN,EXP |
| `Description` | `varchar` |  | Description of the RI status (e.g. Active, Cancelled, Expired). | Active,Cancelled,Expired |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `bit` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this status is effective. |  |

#### `Risk_Type_RI_Limit_Version`
**Domain: Reinsurance**  **Owner: Underwriting**  **Refresh: Static**

Versions the RI limit configuration for a risk type with effective start and end dates, allowing RI thresholds to change over time without losing historical values.

*Keywords*: RI limit version, reinsurance limit, risk type, effective date, versioning, RI configuration history

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `risk_type_ri_limit_version_id` | `int` | âœ“ | Primary key for the RI limit version. |  |
| `risk_type_id` | `int` |  | Foreign key to Risk_Type — the risk type this version applies to. |  |
| `description` | `varchar` |  | Description of the RI limit version. |  |
| `ri_limit_start_date` | `datetime` |  | Start date for which this RI limit version is effective. |  |
| `ri_limit_end_date` | `datetime` |  | End date after which this RI limit version expires. |  |

*Foreign Keys*:
- `Risk_Type_RI_Limit_Version.risk_type_id` â†’ `Risk_Type.risk_type_id` (Many-to-One)

#### `Risk_Type_RI_Values`
**Domain: Reinsurance**  **Owner: Underwriting**  **Refresh: Static**

Stores RI limit/threshold values for a risk type tied to a version. Used with GIS user-defined properties to define the RI cession thresholds (e.g. XOL attachment points) for a risk type.

*Keywords*: RI values, reinsurance limit, risk type RI, XOL attachment, RI threshold, RI configuration, risk type

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `risk_type_id` | `int` |  | Foreign key to Risk_Type. |  |
| `risk_type_ri_limit_version_id` | `int` |  | Foreign key to Risk_Type_RI_Limit_Version — the versioned RI limit this value belongs to. |  |
| `value` | `numeric` |  | The RI threshold/limit value (e.g. attachment point amount). |  |
| `gis_user_def_header_inds_id1` | `int` |  | GIS user-defined property 1 used to define the RI limit dimension. |  |
| `gis_user_def_header_inds_id2` | `int` |  | GIS user-defined property 2 used to define the RI limit dimension. |  |
| `gis_user_def_header_inds_id3` | `int` |  | GIS user-defined property 3 used to define the RI limit dimension. |  |

*Foreign Keys*:
- `Risk_Type_RI_Values.risk_type_ri_limit_version_id` â†’ `Risk_Type_RI_Limit_Version.risk_type_ri_limit_version_id` (Many-to-One)

#### `Underwriting_Year`
**Domain: Reinsurance**  **Owner: Underwriting**  **Refresh: Admin**

Lookup defining underwriting years used for reinsurance and Lloyd's/London market reporting. Each year has a code (e.g. UW2011), description, start and end date. Policies can be assigned to an underwriting year for aggregate and RI purposes.

*Keywords*: underwriting year, UW year, Lloyd's, reinsurance year, start date, end date, underwriting_year_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `underwriting_year_id` | `int` | âœ“ | Primary key for the underwriting year. |  |
| `code` | `char` |  | Short code for the year (e.g. UW2011, UW2012). | UW2011,UW2012,UW2013 |
| `description` | `varchar` |  | Description of the underwriting year (e.g. 2011, 2012). | 2011,2012,2013 |
| `start_date` | `datetime` |  | Start date of the underwriting year. |  |
| `end_date` | `datetime` |  | End date of the underwriting year. |  |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this record is effective. |  |

#### `XOL_Treaty_To_Recover_From`
**Domain: Reinsurance**  **Owner: Reinsurance Department**  **Refresh: Static**

Lookup defining the basis on which an XOL reinsurance treaty responds to a claim: RISK = risk attaching (the treaty year in which the risk incepted responds), LOSS = loss occurring (the treaty year in which the loss occurred responds). Determines which treaty period is triggered for recovery.

*Keywords*: XOL, excess of loss, reinsurance, recovery basis, risk attaching, loss occurring

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `XOL_Treaty_To_Recover_From_id` | `int` | âœ“ | PK — unique identifier for this XOL recovery basis option. | 1, 2 |
| `code` | `varchar(20)` |  | Short code. RISK = risk attaching basis, LOSS = loss occurring basis. | RISK, LOSS |
| `Description` | `varchar(50)` |  | Human-readable description. e.g. Risk Attaching, Loss Occuring. | Risk Attaching, Loss Occuring |
| `caption_id` | `int` |  | FK to PMCaption — multilingual label. | 1363, 1364 |
| `effective_date` | `datetime` |  | Date from which this option is effective. |  |
| `is_deleted` | `tinyint` |  | 0 = active, 1 = soft-deleted. | 0, 1 |

#### `reinsurance_type`
**Domain: Underwriting**  **Owner: Reinsurance**  **Refresh: Real-time**

Lookup defining the high-level classification of reinsurance: Proportional (pro-rata) or Non-Proportional (excess of loss). Controls which calculation method is applied.

*Keywords*: reinsurance type, proportional, non-proportional, pro-rata, excess of loss, RI classification, treaty classification, lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `reinsurance_type_id` | `int` |  | Reinsurance Type Unique ID |  |
| `code` | `char` |  | Reinsurance Type Code |  |
| `description` | `varchar` |  | Reinsurance Type Description |  |
| `effective_date` | `datetime` |  | Reinsurance ldaked |  |

#### `ri_arrangement`
**Domain: Underwriting**  **Owner: Reinsurance**  **Refresh: Real-time**

Reinsurance arrangement for a specific policy/insurance file version. Groups the RI lines covering this policy under the applicable RI model. Holds total ceded premium and arrangement status.

*Keywords*: reinsurance, RI arrangement, policy RI, proportional, XOL, excess of loss, ceded, ceded premium, RI model, treaty, quota share, surplus

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `ri_arrangement_id` | `int` |  | RI Arrangement Unique Key |  |
| `risk_cnt` | `int` |  | Associated Risk Key  |  |
| `ri_band_id` | `int` |  | Associated RI Band |  |
| `ri_model_id` | `int` |  | Associated RI Model for Prop Treaties |  |
| `sum_insured` | `money` |  | Band Sum Insured |  |
| `premium` | `money` |  | Band Premium |  |
| `original_flag` | `tinyint` |  | Is it New or Reversal |  |
| `is_modified` | `tinyint` |  | Has User manually overriden the allocation |  |
| `extended_limit_amount` | `money` |  | RI Limits Amount |  |
| `is_extended_limit_applied` | `tinyint` |  | Is RI Limits applied to this arrangement |  |
| `cloned` | `tinyint` |  | is it a Cloned Model |  |
| `xol_ri_model_id` | `int` |  | Associated RI Model for Non Prop Treaties |  |
| `prop_calc_method_id` | `int` |  | Calculation Method for Prop Treaties (Accounting or Underwriting) |  |
| `xol_calc_method_id` | `int` |  | Calculation Method for Non Prop Treaties (Accounting or Underwriting) |  |
| `version_id` | `int` |  | Version (1 for New or 2 for Portolio Transfer) |  |
| `pro_rata_rate` | `float` |  | Pro rate rate for Portolio Transfer |  |
| `rI_version_type_id` | `int` |  | Version (1 for New or 2 for Portolio Transfer) |  |
| `effective_Date` | `datetime` |  | Effective Date |  |
| `ri_override_reason_id` | `int` |  | Reason for Manual Override |  |

*Foreign Keys*:
- `ri_arrangement.risk_cnt` â†’ `risk (risk_cnt)` (Many-to-One)
- `ri_arrangement.ri_band_id` â†’ `ri_band(ri_band_id)` (Many-to-One)
- `ri_arrangement.ri_model_id` â†’ `ri_model(ri_model_id)` (Many-to-One)
- `ri_arrangement.xol_ri_model_id` â†’ `ri_model(ri_model_id)` (Many-to-One)
- `ri_arrangement.prop_calc_method_id` â†’ `proportional_ri_calculation_method(proportional_ri_calculation_method_id)` (Many-to-One)
- `ri_arrangement.xol_calc_method_id` â†’ `xol_treaty_to_recover_from(xol_treaty_to_recover_from_id)` (Many-to-One)

#### `ri_arrangement_line`
**Domain: Underwriting**  **Owner: Reinsurance**  **Refresh: Real-time**

Individual reinsurance line within a policy RI arrangement. Records which treaty/party carries the risk, ceded percentage or layer limit, and the calculated ceded premium and commission for that reinsurer.

*Keywords*: RI arrangement line, reinsurance line, treaty, ceded premium, ceded percentage, quota share, surplus, excess of loss, RI party, reinsurer share

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `ri_arrangement_line_id` | `int` |  | RI Arrangement Line Unique Key |  |
| `ri_arrangement_id` | `int` |  | Associated RI Arrangement |  |
| `type` | `varchar` |  | Type of Treaty Line |  |
| `treaty_id` | `int` |  | Treaty ID link (if any) |  |
| `party_cnt` | `int` |  | FAC Party Link (if any) |  |
| `default_share_percent` | `float` |  | Default Share Percent |  |
| `this_share_percent` | `float` |  | This Share Percent |  |
| `premium_percent` | `float` |  | Premium Share |  |
| `commission_percent` | `float` |  | Commission Share |  |
| `priority` | `int` |  | Line Priority |  |
| `number_of_lines` | `smallint` |  | Number of lines |  |
| `line_limit` | `money` |  | Treaty limit |  |
| `sum_insured` | `money` |  | Treaty Sum insured Allocated  |  |
| `premium_value` | `money` |  | Treaty Premium Allocated |  |
| `commission_value` | `money` |  | Treaty Commission  |  |
| `premium_tax` | `money` |  | Treaty Premium Tax |  |
| `commission_tax` | `money` |  | Treaty Commission tax |  |
| `retained` | `float` |  | Is Retained |  |
| `lower_limit` | `money` |  | Lower limit |  |
| `participation_percent` | `float` |  | Participation percent |  |
| `grouping` | `int` |  | Grouping for Fac XOL |  |
| `ri_model_line_id` | `int` |  | Ri model line link |  |
| `is_obligatory` | `tinyint` |  | Is is an obligatory/compulsary treaty |  |

*Foreign Keys*:
- `ri_arrangement_line.ri_arrangement_id` â†’ `ri_arrangement(ri_arrangement_id)` (Many-to-One)
- `ri_arrangement_line.treaty_id` â†’ `treaty(treaty_id)` (Many-to-One)
- `ri_arrangement_line.party_cnt` â†’ `party(party_cnt)` (Many-to-One)
- `ri_arrangement_line.ri_model_line_id` â†’ `ri_model_line(ri_model_line_id)` (Many-to-One)

#### `ri_band`
**Domain: Underwriting**  **Owner: Reinsurance**  **Refresh: Real-time**

Reinsurance band definition. Segments policies by sum insured range so different RI models can apply depending on the size/exposure of the risk.

*Keywords*: RI band, reinsurance band, sum insured band, risk band, banding, band limit, RI banding

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `ri_band_id` | `int` |  | RI Band Unique Identifier |  |
| `code` | `char` |  | RI Band Code |  |
| `description` | `varchar` |  | RI Band Description |  |
| `date_for_treaty_xol_calculation_id` | `int` |  | Calculation Method for Excess of Loss (Accounting/Underwriting) |  |
| `xol_treaty_to_recover_from_id` | `int` |  | Calculation Method for Claims |  |
| `proportional_ri_cal_method` | `int` |  | Calculation Method for Proportional Treaty(Accounting/Underwriting) |  |

*Foreign Keys*:
- `ri_band.Date_for_Treaty_XOL_Calculation_id` â†’ `date_for_treaty_xol_calculation(date_for_treaty_xol_calculation_id)` (Many-to-One)
- `ri_band.XOL_Treaty_To_Recover_From_id` â†’ `xol_treaty_to_recover_from(xol_treaty_to_recover_from_id)` (Many-to-One)
- `ri_band.Proportional_RI_Cal_Method` â†’ `proportional_ri_calculation_method(proportional_ri_calculation_method_id)` (Many-to-One)

#### `ri_model`
**Domain: Underwriting**  **Owner: Reinsurance**  **Refresh: Real-time**

Reinsurance model configuration. Defines which RI structures (proportional treaties, XOL layers) apply to a risk type/band. The RI model drives automatic reinsurance calculation when policies are processed.

*Keywords*: RI model, reinsurance model, treaty configuration, risk type, band, proportional, XOL, excess of loss, RI configuration, RI setup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `ri_model_id` | `int` |  | Ri model Unique Identifier |  |
| `code` | `char` |  | RI Model Code |  |
| `description` | `varchar` |  | RI Model Description |  |
| `effective_date` | `datetime` |  | RI Model Effective_Date |  |
| `expiry_date` | `datetime` |  | RI Model Expiry_Date |  |
| `ri_model_type` | `tinyint` |  | Ri model Type |  |
| `fac_premium_type` | `tinyint` |  | FAC Premium Type (Prop or Non Prop) |  |
| `claim_allocation_type` | `tinyint` |  | Claim Allocation Type (Prop or Non Prop) |  |
| `currency_id` | `smallint` |  | RI Model Currency |  |
| `xol_clm_ri_model_id` | `int` |  | Linked Excess of Loss |  |
| `xol_clm_limit` | `money` |  | Linked Excess of Loss Trigger Limit |  |
| `xol_cat_ri_model_id` | `int` |  | Linked CAT Excess of Loss |  |
| `xol_cat_limit` | `money` |  | Linked CAT Excess of Loss Trigger Limit |  |
| `xol_cat_reinstatements` | `smallint` |  | CAT Reinstatements |  |

*Foreign Keys*:
- `ri_model.currency_id` â†’ `currency(currency_id)` (Many-to-One)
- `ri_model.xol_clm_ri_model_id` â†’ `ri_model(ri_model_id)` (Many-to-One)
- `ri_model.xol_cat_ri_model_id` â†’ `ri_model(ri_model_id)` (Many-to-One)

#### `ri_model_line`
**Domain: Underwriting**  **Owner: Reinsurance**  **Refresh: Real-time**

A specific treaty (reinsurance layer) attached to an RI model. Defines the treaty reference, cession type, attachment point, limit, and order/priority within the model.

*Keywords*: RI model line, reinsurance treaty, attachment point, limit, cession, excess of loss, quota share, surplus, layer order, RI layer

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `ri_model_line_id` | `int` |  | RI Model Line Unique ID |  |
| `ri_model_id` | `int` |  | RI Model link |  |
| `priority` | `smallint` |  | Line Priority |  |
| `number_of_lines` | `smallint` |  | Number of lines |  |
| `line_limit` | `money` |  | Line Limit |  |
| `treaty_id` | `int` |  | Treaty Link |  |
| `share_percent` | `float` |  | Default Share Percent |  |
| `lower_limit` | `money` |  | Lower limit |  |
| `ceding_rate` | `float` |  | Excess of loss or CAT ceeding Rate |  |
| `Treaty_Type_id` | `int` |  | Treaty Type (Prop or Non Prop) |  |
| `Is_Obligatory` | `tinyint` |  | Is it an Obligatory/Compulsory Treaty |  |
| `cede_premium_only` | `tinyint` |  | Cede premium only (No Sum Insured Allocation) |  |

*Foreign Keys*:
- `ri_model_line.ri_model_id` â†’ `ri_model(ri_model_id)` (Many-to-One)
- `ri_model_line.treaty_id` â†’ `treaty(treaty_id)` (Many-to-One)
- `ri_model_line.treaty_type_id` â†’ `treaty_type(treaty_type_id)` (Many-to-One)

#### `risk_type_ri_model_usage`
**Domain: Underwriting**  **Owner: Reinsurance**  **Refresh: Real-time**

Links a risk type to one or more RI models across band ranges. Defines which RI model applies for a given risk type within a specific band. Drives automatic RI model selection when policies are processed.

*Keywords*: risk type RI model, reinsurance configuration, risk type, band, RI model usage, RI assignment, automatic RI

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `risk_type_ri_model_usage_cnt` | `int` |  | RI Models linked with Risk Unique Identifier |  |
| `risk_type_id` | `int` |  | Risk Type Link |  |
| `ri_band` | `int` |  | RI Band Link |  |
| `ri_model_id` | `int` |  | RI Model Link |  |
| `description` | `varchar` |  | Description |  |
| `effective_date` | `datetime` |  | Effective Date |  |
| `expiry_date` | `datetime` |  | Expiry Date |  |
| `portfolio_transfer_from_cnt` | `int` |  | If Portfolio Transfer is Needed (Provide Previous Year's Model) |  |

*Foreign Keys*:
- `risk_type_ri_model_usage.risk_type_id` â†’ `risk_type(risk_type_id)` (Many-to-One)
- `risk_type_ri_model_usage.ri_band` â†’ `ri_band(ri_band_id)` (Many-to-One)
- `risk_type_ri_model_usage.ri_model_id` â†’ `ri_model(ri_model_id)` (Many-to-One)
- `risk_type_ri_model_usage.portfolio_transfer_from_cnt` â†’ `ri_model(ri_model_id)` (Many-to-One)

#### `risk_type_ri_properties`
**Domain: Reinsurance**  **Owner: Underwriting**  **Refresh: Static**

Links GIS properties on a risk type to their role in RI limit calculations for a specific RI limit version. Defines which data fields drive RI cession thresholds.

*Keywords*: RI properties, reinsurance, risk type, GIS property, RI limit, cession, risk_type_ri_properties

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `risk_type_id` | `int` |  | Foreign key to Risk_Type. |  |
| `risk_type_ri_properties_seq_id` | `int` |  | Sequence number — order of this property in the RI limit calculation. | 1,2,3 |
| `gis_property_id` | `int` |  | Foreign key to GIS_Property — the data field used as an RI limit dimension. |  |
| `risk_type_ri_limit_version_id` | `int` |  | Foreign key to Risk_Type_RI_Limit_Version — the version this property definition belongs to. |  |

*Foreign Keys*:
- `risk_type_ri_properties.risk_type_ri_limit_version_id` â†’ `Risk_Type_RI_Limit_Version.risk_type_ri_limit_version_id` (Many-to-One)

#### `treaty`
**Domain: Underwriting**  **Owner: Reinsurance**  **Refresh: Real-time**

Reinsurance treaty definition. Contains treaty code, type (Quota Share, Surplus, Excess of Loss, Facultative), currency, effective dates, and participating reinsurers/brokers.

*Keywords*: treaty, reinsurance treaty, quota share, surplus, XOL, excess of loss, facultative, treaty reference, treaty type, RI treaty, reinsurance contract

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `treaty_id` | `int` |  | Treaty Unique Identifier |  |
| `code` | `char` |  | Treaty Unique Code |  |
| `description` | `varchar` |  | Treaty Description |  |
| `effective_date` | `datetime` |  | Treaty Effective Date |  |
| `expiry_date` | `datetime` |  | Treaty Expiry Date |  |
| `agreement_code` | `varchar` |  | Agreement Code |  |
| `reinsurance_type_id` | `int` |  | Reinsurance Type (Quota Share, Excess of Loss, Surplus etc) |  |

*Foreign Keys*:
- `treaty.reinsurance_type_id` â†’ `reinsurance_type(reinsurance_type_id)` (Many-to-One)

#### `treaty_party`
**Domain: Underwriting**  **Owner: Reinsurance**  **Refresh: Real-time**

Reinsurer or RI broker participant on a treaty. Records the party (reinsurer), their participation percentage (line size), order, and capacity on the treaty.

*Keywords*: treaty party, reinsurer, RI participant, participation percentage, line size, capacity, reinsurer share, treaty participation, reinsurer name

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `treaty_party_id` | `int` |  | Treaty Participants Unique Key |  |
| `party_cnt` | `int` |  | Treaty Participants Party |  |
| `treaty_id` | `int` |  | Treaty |  |
| `share_percent` | `float` |  | Share Percent |  |
| `commission_percent` | `float` |  | Commission Share |  |
| `is_Reinsurer_Approved` | `tinyint` |  | Is Approved |  |

*Foreign Keys*:
- `treaty_party.party_cnt` â†’ `party(party_cnt)` (Many-to-One)
- `treaty_party.treaty_id` â†’ `treaty(treaty_id)` (Many-to-One)

#### `treaty_type`
**Domain: Underwriting**  **Owner: Reinsurance**  **Refresh: Real-time**

Lookup defining reinsurance treaty types: Quota Share, Surplus, Excess of Loss (XOL), Stop Loss, Facultative, Aggregate.

*Keywords*: treaty type, quota share, surplus, excess of loss, XOL, stop loss, facultative, aggregate, RI type, lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `treaty_type_id` | `int` |  | Treaty Type Unique ID |  |
| `code` | `char` |  | Treaty Type Code |  |
| `description` | `varchar` |  | Treaty Type Description |  |
| `effective_date` | `datetime` |  | Treaty ldaked |  |

---

## Renewals

**Tables in this module**: 10

### Query Rules

**Renewal Quotes** â€” `code = 'RENEWAL'`  
*Checking the Quotes in Renewal*

**Policies Due for Renewal (Next 30 Days)** â€” `expiry_date BETWEEN GETDATE() AND DATEADD(day, 30, GETDATE()) AND insurance_file_status_id IS NULL AND insurance_file_type_id IN (2,5,6,9)`  
*Returns live policies whose expiry date falls within the next 30 days — the renewal diary or work list. Adjust the day interval as required.*

**Renewal Policy Identification** â€” `insurance_file_type.code = 'RENEWAL'  -- applies to the renewal quote/offer version`  
*A renewal policy is one where the insurance_file_type.code = RENEWAL (for the renewal quote) or the bound renewal version linked to it. Renewal policies share the same insurance_ref as the preceding year's policy and have a cover_start_date equal to the prior policy's expiry_date. The renewal quote status is identified by insurance_file_type.code = 'RENEWAL'.*

**Policies in a Batch Renewal Run** â€” `Batch_Renewal_Job_Run_Policy.batch_renewal_job_runs_id = Batch_Renewal_Job_Runs.batch_renewal_job_runs_id`  
*Batch_Renewal_Job_Run_Policy holds the individual policies processed in each renewal run. Join to Batch_Renewal_Job_Runs for run metadata and to insurance_file for policy details.*

### Tables

#### `Batch_Renewal_Job`
**Domain: Policy**  **Owner: Operations**  **Refresh: Admin**

Defines a scheduled batch renewal job (e.g. Renewal Selection, Renewal Invites). Configures how many days before renewal to run, which agents/products/branches to include, document destination, and the WCF service endpoint. Master record for the batch renewal configuration.

*Keywords*: batch renewal job, renewal batch, renewal selection, renewal invites, scheduled job, automated renewal, batch_renewal_job_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `batch_renewal_job_id` | `int` | âœ“ | Primary key for the batch renewal job. |  |
| `code` | `varchar` |  | Short code for the job (e.g. RS1=Renewal Selection, RI1=Renewal Invites). | RS1,RI1 |
| `description` | `varchar` |  | Description of the batch renewal job. | Renewal Selection,Renewal Invites |
| `batch_renewal_job_type_id` | `int` |  | Foreign key to Batch_Renewal_Job_Type — Selection or Invitation. |  |
| `days_before_renewal_date` | `smallint` |  | How many days before the renewal date to include policies in the batch. | 0,30,60 |
| `is_active` | `tinyint` |  | Whether this batch renewal job is currently active (1=yes). | 0,1 |
| `all_agents` | `tinyint` |  | Whether to include policies for all agents (1) or only agents in Batch_Renewal_Job_Agents (0). | 0,1 |
| `renewal_docs_destination` | `tinyint` |  | Where to send generated renewal documents (0=print, 1=email, 2=both). | 0,1,2 |
| `include_direct_policies` | `tinyint` |  | Whether to include direct (non-broker) policies in this renewal batch. | 0,1 |
| `sam_server` | `varchar` |  | WCF service endpoint URL used to process this renewal job. |  |
| `date_created` | `datetime` |  | Date the job was created. |  |
| `date_updated` | `datetime` |  | Date the job configuration was last updated. |  |

*Foreign Keys*:
- `Batch_Renewal_Job.batch_renewal_job_type_id` â†’ `Batch_Renewal_Job_Type.batch_renewal_job_type_id` (Many-to-One)

#### `Batch_Renewal_Job_Agents`
**Domain: Policy**  **Owner: Operations**  **Refresh: Admin**

Lists the specific agents (brokers) included in a batch renewal job. When all_agents=0 on the job, only policies for agents listed here are selected for renewal processing.

*Keywords*: batch renewal, agents, broker filter, renewal selection, agent restriction

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `batch_renewal_job_id` | `int` |  | Foreign key to Batch_Renewal_Job. |  |
| `party_cnt` | `int` |  | Foreign key to Party — the agent included in this renewal job filter. |  |

*Foreign Keys*:
- `Batch_Renewal_Job_Agents.batch_renewal_job_id` â†’ `Batch_Renewal_Job.batch_renewal_job_id` (Many-to-One)
- `Batch_Renewal_Job_Agents.party_cnt` â†’ `Party.party_cnt` (Many-to-One)

#### `Batch_Renewal_Job_Branches`
**Domain: Policy**  **Owner: Operations**  **Refresh: Admin**

Lists the branches (sources) included in a batch renewal job. Restricts renewal processing to policies belonging to the specified branches.

*Keywords*: batch renewal, branches, source filter, renewal selection, branch restriction

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `batch_renewal_job_id` | `int` |  | Foreign key to Batch_Renewal_Job. |  |
| `source_id` | `int` |  | Foreign key to Source — the branch included in this renewal job filter. |  |

*Foreign Keys*:
- `Batch_Renewal_Job_Branches.batch_renewal_job_id` â†’ `Batch_Renewal_Job.batch_renewal_job_id` (Many-to-One)
- `Batch_Renewal_Job_Branches.source_id` â†’ `Source.source_id` (Many-to-One)

#### `Batch_Renewal_Job_Products`
**Domain: Policy**  **Owner: Operations**  **Refresh: Admin**

Lists the products included in a batch renewal job. Restricts renewal processing to policies of the specified products.

*Keywords*: batch renewal, products, product filter, renewal selection, product restriction

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `batch_renewal_job_id` | `int` |  | Foreign key to Batch_Renewal_Job. |  |
| `product_id` | `int` |  | Foreign key to Product — the product included in this renewal job filter. |  |

*Foreign Keys*:
- `Batch_Renewal_Job_Products.batch_renewal_job_id` â†’ `Batch_Renewal_Job.batch_renewal_job_id` (Many-to-One)
- `Batch_Renewal_Job_Products.product_id` â†’ `Product.product_id` (Many-to-One)

#### `Batch_Renewal_Job_Run_Insurance_Folder`
**Domain: Policy**  **Owner: Operations**  **Refresh: Real-time**

Records the processing result for each individual policy folder processed during a batch renewal job run. Holds the old and new insurance file references, processing status, and any error message. Main audit trail for batch renewal processing.

*Keywords*: batch renewal run, policy folder, renewal result, processing status, old policy, new policy, renewal audit, batch_id, insurance_folder_cnt

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `batch_id` | `int` |  | Foreign key to Batch — the financial batch this run belongs to. |  |
| `insurance_folder_cnt` | `int` |  | Foreign key to Insurance_Folder — the policy folder being processed. |  |
| `batch_renewal_job_id` | `int` |  | Foreign key to Batch_Renewal_Job — the job that triggered this run. |  |
| `batch_renewal_job_run_status_id` | `int` |  | Foreign key to Batch_Renewal_Job_Run_Status — processing status for this folder. |  |
| `old_insurance_file_cnt` | `int` |  | Foreign key to Insurance_File — the expiring policy version. |  |
| `new_insurance_file_cnt` | `int` |  | Foreign key to Insurance_File — the newly created renewal policy version. |  |
| `recalculate_commission` | `bit` |  | Whether commission was recalculated during this renewal. | 0,1 |
| `recalculate_fees` | `bit` |  | Whether fees were recalculated during this renewal. | 0,1 |
| `recalculate_taxes` | `bit` |  | Whether taxes were recalculated during this renewal. | 0,1 |
| `message` | `varchar` |  | Error or informational message recorded during processing of this folder. |  |

*Foreign Keys*:
- `Batch_Renewal_Job_Run_Insurance_Folder.batch_id` â†’ `Batch.batch_id` (Many-to-One)
- `Batch_Renewal_Job_Run_Insurance_Folder.insurance_folder_cnt` â†’ `Insurance_Folder.insurance_folder_cnt` (Many-to-One)
- `Batch_Renewal_Job_Run_Insurance_Folder.batch_renewal_job_id` â†’ `Batch_Renewal_Job.batch_renewal_job_id` (Many-to-One)
- `Batch_Renewal_Job_Run_Insurance_Folder.batch_renewal_job_run_status_id` â†’ `Batch_Renewal_Job_Run_Status.batch_renewal_job_run_status_id` (Many-to-One)

#### `Batch_Renewal_Job_Run_Risk`
**Domain: Policy**  **Owner: Operations**  **Refresh: Real-time**

Records risk-level processing instructions for a policy folder within a batch renewal run. Flags whether to re-rate the risk, recalculate reinsurance, fees and taxes during the renewal.

*Keywords*: batch renewal run, risk, re-rate, reinsurance, fees, taxes, renewal risk processing

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `batch_id` | `int` |  | Foreign key to Batch. |  |
| `insurance_folder_cnt` | `int` |  | Foreign key to Insurance_Folder. |  |
| `risk_folder_cnt` | `int` |  | Foreign key to Risk_Folder — the risk being processed in this renewal run. |  |
| `rerate` | `bit` |  | Whether to re-rate this risk during renewal processing (1=yes). | 0,1 |
| `recalculate_reinsurance` | `bit` |  | Whether to recalculate RI for this risk during renewal. | 0,1 |
| `recalculate_fees` | `bit` |  | Whether to recalculate fees for this risk during renewal. | 0,1 |
| `recalculate_taxes` | `bit` |  | Whether to recalculate taxes for this risk during renewal. | 0,1 |

*Foreign Keys*:
- `Batch_Renewal_Job_Run_Risk.risk_folder_cnt` â†’ `Risk_Folder.risk_folder_cnt` (Many-to-One)

#### `Batch_Renewal_Job_Run_Status`
**Domain: Policy**  **Owner: Operations**  **Refresh: Static**

Lookup defining the processing status of a policy within a batch renewal job run: e.g. Ready for Processing (R), Processing in Progress (P), Completed (C), Failed (F).

*Keywords*: batch renewal status, run status, ready, processing, completed, failed, lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `batch_renewal_job_run_status_id` | `int` | âœ“ | Primary key for the run status. |  |
| `code` | `char` |  | Short code for the status (e.g. R=Ready, P=Processing, C=Completed, F=Failed). | R,P,C,F |
| `description` | `varchar` |  | Description of the run status. | Ready for processing,Processing in progress,Completed,Failed |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `bit` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this status is effective. |  |

#### `Batch_Renewal_Job_Runs`
**Domain: Policy**  **Owner: Operations**  **Refresh: Real-time**

Header record for each execution (run) of a batch renewal job. Records the run date, the insurance file it was based on, success/failure status, failure reason, and a GUID linking all policies processed in that run.

*Keywords*: batch renewal run, job execution, run date, failure, GUID, renewal batch run, batch_renewal_job_runs_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `batch_renewal_job_runs_id` | `int` | âœ“ | Primary key for the job run record. |  |
| `batch_renewal_job_id` | `int` |  | Foreign key to Batch_Renewal_Job — the job that was executed. |  |
| `insurance_file_cnt` | `int` |  | Foreign key to Insurance_File — the insurance file context for this run. |  |
| `run_date` | `datetime` |  | Date and time the job run was executed. |  |
| `is_failed` | `tinyint` |  | Whether this run failed overall (1=failed). | 0,1 |
| `failure_reason` | `varchar` |  | Reason for failure if the run did not complete successfully. |  |
| `document_printed` | `varchar` |  | Indicator of whether documents were printed/generated during this run. |  |
| `GUID` | `varchar` |  | GUID linking all policy folders processed in the same batch run execution. |  |

*Foreign Keys*:
- `Batch_Renewal_Job_Runs.batch_renewal_job_id` â†’ `Batch_Renewal_Job.batch_renewal_job_id` (Many-to-One)
- `Batch_Renewal_Job_Runs.insurance_file_cnt` â†’ `Insurance_File.insurance_file_cnt` (Many-to-One)

#### `Batch_Renewal_Job_Type`
**Domain: Policy**  **Owner: Operations**  **Refresh: Static**

Lookup classifying the type of batch renewal job: Selection (SEL) — selects policies due for renewal; Invitation (INV) — generates and sends renewal invite documents.

*Keywords*: batch renewal job type, selection, invitation, SEL, INV, renewal type, lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `batch_renewal_job_type_id` | `int` | âœ“ | Primary key for the job type. |  |
| `code` | `varchar` |  | Short code: SEL=Selection, INV=Invitation. | SEL,INV |
| `description` | `varchar` |  | Description of the job type (Selection or Invitation). | Selection,Invitation |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this type is effective. |  |

#### `Renewal_Status_Type`
**Domain: Policy**  **Owner: Operations**  **Refresh: Static**

Lookup defining the possible renewal processing statuses for a policy: Awaiting Manual Review, Awaiting Renewal Notice Print, Awaiting Manual Rating Due To Failure, Policy Details Changed, etc. Tracks where a policy sits in the renewal workflow.

*Keywords*: renewal status type, awaiting review, renewal notice, manual rating, policy changed, renewal workflow, renewal_status_type_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `renewal_status_type_id` | `int` | âœ“ | Primary key for the renewal status type. |  |
| `code` | `char` |  | Short code for the renewal status (e.g. ManReview, AutoReview, AutoFailed, PolicyChan). | ManReview,AutoReview,AutoFailed,PolicyChan |
| `description` | `varchar` |  | Description of the renewal status (e.g. Awaiting Manual Review, Awaiting Renewal Notice Print). | Awaiting Manual Review,Awaiting Renewal Notice Print,Awaiting Manual Rating Due To Failure |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this status type is effective. |  |

---

## Party Management

**Tables in this module**: 46

### Tables

#### `Business_Type`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: business type codes used on party records

*Keywords*: Business Type, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `business_type_id` | `smallint` |  | Unique business type identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code |  |
| `description` | `varchar` |  | Description |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

#### `Department`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: internal department codes (used on handler parties)

*Keywords*: Department, Internal, Handler, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `department_id` | `int` |  | Unique department identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code |  |
| `description` | `varchar` |  | Description of the department |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

#### `FSA_Agent_Status`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: FCA/FSA regulatory status codes for agents

*Keywords*: FSA, FCA, Regulatory, Agent Status, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `fsa_agent_status_id` | `int` |  | Unique FSA status identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `varchar` |  | Short code |  |
| `description` | `varchar` |  | Description of the FSA status |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

#### `Insurer_Locking_Type`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: locking type classification for insurer parties

*Keywords*: Insurer, Locking, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `Insurer_locking_type_id` | `int` |  | Unique locking type identifier (PK) |  |
| `Caption_id` | `int` |  | FK to PMCaption |  |
| `Code` | `varchar` |  | Short code |  |
| `Description` | `varchar` |  | Description |  |
| `Is_deleted` | `int` |  | Soft delete flag | 0,1 |
| `Effective_date` | `datetime` |  | Date record became effective |  |

#### `Insurer_Type`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: insurer type codes (Lloyd's, company, etc.)

*Keywords*: Insurer Type, Lloyds, Company, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `insurer_Type_id` | `int` |  | Unique insurer type identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code (e.g. LLY, COM) |  |
| `description` | `varchar` |  | Description (e.g. Lloyd's, Company) |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

#### `License_type`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: vehicle or driver licence type codes

*Keywords*: License, Driver, Vehicle, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `license_type_id` | `int` |  | Unique license type identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code |  |
| `description` | `varchar` |  | Description |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

#### `Loyalty_Scheme`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: loyalty scheme definitions

*Keywords*: Loyalty, Scheme, Membership, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `loyalty_scheme_id` | `int` |  | Unique scheme identifier (PK) |  |
| `code` | `varchar` |  | Short code |  |
| `description` | `varchar` |  | Description of the loyalty scheme |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `party_type_id` | `smallint` |  | FK to party type this scheme applies to |  |

#### `Nationality`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: nationality codes

*Keywords*: Nationality, Country, ISO, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `Nationality_id` | `int` |  | Unique nationality identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code (ISO 3166) |  |
| `description` | `varchar` |  | Nationality description |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

#### `Party_Agent`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Agent/broker party supplemental data: FCA/FSA registration number, commission structure, credit limit, overdraft limit, binder authority, and trading details.

*Keywords*: agent, broker, FCA, FSA, registration, commission, credit limit, binder, delegated authority, agent details

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | FK to party (party_cnt) |  |
| `party_agent_type_id` | `int` |  | FK to party_agent_type |  |
| `party_agent_origin_id` | `int` |  | FK to Party_Agent_Origin |  |
| `agency_agreement_date` | `datetime` |  | Date agency agreement was signed |  |
| `agency_next_review_date` | `datetime` |  | Date of next agency review |  |
| `agency_account_number` | `varchar` |  | Agent account number |  |
| `is_branch` | `tinyint` |  | Flag: this party is a branch | 0,1 |
| `is_head_office` | `tinyint` |  | Flag: this party is a head office | 0,1 |
| `default_commission_percent` | `numeric` |  | Default commission percentage |  |
| `trading_name` | `varchar` |  | Trading name of the agent |  |
| `binder_indicator` | `int` |  | Indicates binder holding status |  |
| `report_indicator` | `int` |  | Report access indicator |  |
| `linked_account_executive_id` | `int` |  | FK to linked account executive party |  |
| `linked_account_group` | `int` |  | Linked account group identifier |  |
| `payment_method` | `int` |  | Payment method code |  |
| `payment_frequency` | `int` |  | Payment frequency code |  |
| `address_on_notice` | `int` |  | Address to use on notices |  |
| `type_of_statement` | `int` |  | Type of statement to produce |  |
| `source` | `int` |  | Acquisition source code |  |
| `title` | `varchar` |  | Agent title/salutation |  |
| `multipac` | `tinyint` |  | Flag: multi-PAC enabled | 0,1 |
| `contact_person` | `varchar` |  | Main contact person name |  |
| `first_name` | `varchar` |  | First name of agent contact |  |
| `bank_account` | `varchar` |  | Bank account reference for agent |  |
| `date_cancelled` | `datetime` |  | Date agent relationship was cancelled |  |
| `agent_status_id` | `int` |  | FK to FSA_Agent_Status |  |
| `fsa_registration_number` | `varchar` |  | FCA/FSA registration number |  |
| `broker_abi_id` | `varchar` |  | ABI broker identifier |  |
| `expense_account_id` | `int` |  | FK to expense account |  |
| `is_in_transfer_mode` | `tinyint` |  | Flag: agent is being transferred | 0,1 |
| `transfer_to_business_type_id` | `smallint` |  | Target business type during transfer |  |
| `transfer_to_party_cnt` | `int` |  | FK to target party during transfer |  |
| `allow_consolidated_commission` | `tinyint` |  | Flag: allow consolidated commission statements | 0,1 |
| `use_override_commission_rate` | `tinyint` |  | Flag: use override commission rate | 0,1 |
| `domiciled_for_tax` | `tinyint` |  | Flag: agent is domiciled for tax purposes | 0,1 |
| `can_make_live_invoice` | `tinyint` |  | Flag: can generate live invoices | 0,1 |
| `can_make_live_instalments` | `tinyint` |  | Flag: can generate live instalment plans | 0,1 |
| `can_make_live_paynow` | `tinyint` |  | Flag: can use Pay Now facility | 0,1 |
| `is_standard_account` | `tinyint` |  | Flag: standard account type | 0,1 |
| `is_float_balance_account` | `tinyint` |  | Flag: float balance account | 0,1 |
| `is_overdraft_account` | `tinyint` |  | Flag: overdraft account | 0,1 |
| `is_prepayment_account` | `tinyint` |  | Flag: prepayment account | 0,1 |
| `expected_daily_premium` | `money` |  | Expected daily premium income |  |
| `days_allowed` | `smallint` |  | Credit days allowed |  |
| `float_balance_limit` | `money` |  | Float balance credit limit |  |
| `overdraft_limit` | `money` |  | Overdraft credit limit |  |
| `overdraft_expiry` | `datetime` |  | Date overdraft facility expires |  |
| `alternate_reference_mandatory` | `tinyint` |  | Flag: alternate reference is mandatory | 0,1 |
| `alternate_reference_for_each_transaction` | `tinyint` |  | Flag: alternate ref required on each txn | 0,1 |
| `commission_posting_type_id` | `int` |  | FK to commission posting type |  |
| `is_viewable_only` | `tinyint` |  | Flag: agent is read-only viewable | 0,1 |
| `is_ssp_subagent` | `bit` |  | Flag: SSP sub-agent | 0,1 |
| `is_single_instalment_plan` | `tinyint` |  | Flag: single instalment plan only | 0,1 |
| `common_renewal_date` | `datetime` |  | Common renewal date for agent portfolio |  |
| `produce_agent_renewal_list` | `tinyint` |  | Flag: produce renewal list for agent | 0,1 |
| `can_make_live_BankGuarantee` | `tinyint` |  | Flag: can use bank guarantee facility | 0,1 |
| `can_make_live_cashdeposit` | `tinyint` |  | Flag: can use cash deposit facility | 0,1 |
| `commission_level_id` | `int` |  | FK to Commission_level |  |
| `use_override_commission_renewal` | `int` |  | Override commission renewal flag/code |  |
| `is_gross_agent` | `tinyint` |  | Flag: agent works on gross basis | 0,1 |
| `bankaccount_id` | `int` |  | FK to BankAccount |  |
| `Receives_Client_Correspondence` | `tinyint` |  | Flag: agent receives client correspondence | 0,1 |
| `UserId` | `int` |  | Last modified by user (FK to PMUser) |  |
| `UniqueId` | `varchar` |  | Unique sync identifier |  |
| `ScreenHierarchy` | `varchar` |  | UI screen hierarchy path |  |

*Foreign Keys*:
- `Party_Agent.party_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `Party_Agent.party_agent_type_id` â†’ `party_agent_type (party_agent_type_id)` (Many-to-One)
- `Party_Agent.party_agent_origin_id` â†’ `Party_Agent_Origin (party_agent_origin_id)` (Many-to-One)
- `Party_Agent.agent_status_id` â†’ `FSA_Agent_Status (fsa_agent_status_id)` (Many-to-One)
- `Party_Agent.commission_level_id` â†’ `Commission_level (commission_level_id)` (Many-to-One)
- `Party_Agent.bankaccount_id` â†’ `BankAccount (bankaccount_id)` (Many-to-One)
- `Party_Agent.transfer_to_party_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `Party_Agent.linked_account_executive_id` â†’ `party (party_cnt)` (Many-to-One)

#### `Party_Agent_Branch`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Agent branch party supplemental data

*Keywords*: Agent, Branch

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | FK to party (party_cnt) |  |
| `source_id` | `int` |  | FK to source/branch origin |  |
| `party_agent_branch_id` | `int` |  | Unique agent branch record (PK) |  |
| `UserId` | `int` |  | Last modified by user |  |
| `UniqueId` | `varchar` |  | Unique sync identifier |  |
| `ScreenHierarchy` | `varchar` |  | UI screen hierarchy path |  |

*Foreign Keys*:
- `Party_Agent_Branch.party_cnt` â†’ `party (party_cnt)` (Many-to-One)

#### `Party_Agent_Group`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Flags a party as belonging to an agent group

*Keywords*: Agent Group, Active

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | FK to party (party_cnt) |  |
| `Active` | `tinyint` |  | Flag: group is active | 0,1 |

*Foreign Keys*:
- `Party_Agent_Group.party_cnt` â†’ `party (party_cnt)` (Many-to-One)

#### `Party_Agent_Origin`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: origin/source codes for agent parties

*Keywords*: Agent Origin, Source, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_agent_origin_id` | `int` |  | Unique agent origin identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code |  |
| `description` | `varchar` |  | Description |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

#### `Party_Agent_Product`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Products that a party (agent/broker) is authorised to sell or transact. Controls which insurance products an agent can place business on.

*Keywords*: agent product, authorised product, broker product, product access, agent authorisation

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `Party_Cnt` | `int` |  | FK to party (party_cnt) |  |
| `Product_Id` | `int` |  | FK to product |  |
| `party_agent_product_id` | `int` |  | Unique product authorisation record (PK) |  |
| `UserId` | `int` |  | Last modified by user (FK to PMUser) |  |
| `UniqueId` | `varchar` |  | Unique sync identifier |  |
| `ScreenHierarchy` | `varchar` |  | UI screen hierarchy path |  |

*Foreign Keys*:
- `Party_Agent_Product.Party_Cnt` â†’ `party (party_cnt)` (Many-to-One)
- `Party_Agent_Product.Product_Id` â†’ `product (product_id)` (Many-to-One)

#### `Party_Bank`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Bank account details for a party (client, agent, or insurer). Stores account number, sort code, bank name, IBAN/BIC for EFT, and credit card details for direct debit.

*Keywords*: party bank, bank account, sort code, IBAN, BIC, account number, EFT, direct debit, credit card, bank details

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_bank_id` | `int` |  | Unique party bank record identifier (PK) | 1,2,3 |
| `account_id` | `int` |  | Linked account (FK to Account) | 3797,3867 |
| `is_bank` | `tinyint` |  | Flag: record is for a bank account (vs card) | 0,1 |
| `bank_payment_type_id` | `int` |  | Payment type for this bank record | 2 |
| `account_holder_name` | `varchar` |  | Name of the account holder |  |
| `account_number` | `varchar` |  | Bank account number |  |
| `Bank_Name` | `varchar` |  | Free-text bank name |  |
| `bank_branch` | `varchar` |  | Bank branch name |  |
| `bank_branch_code` | `varchar` |  | Bank branch sort code |  |
| `cc_num` | `varchar` |  | Credit card number (masked) |  |
| `cc_expiry_date` | `varchar` |  | Credit card expiry date |  |
| `business_identifier_code` | `varchar` |  | BIC/SWIFT code |  |
| `international_bank_account_number` | `varchar` |  | IBAN |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `is_default` | `tinyint` |  | Flag: this is the default bank record for the party | 0,1 |

*Foreign Keys*:
- `Party_Bank.account_id` â†’ `account (account_id)` (Many-to-One)

#### `Party_Bank_History`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Audit history of changes to a party bank account record

*Keywords*: Bank, Payment, History, Audit, Card

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_bank_history_id` | `int` |  | Unique bank history record (PK) |  |
| `party_bank_id` | `int` |  | FK to Party_Bank |  |
| `action_code` | `varchar` |  | Type of change action made |  |
| `account_id` | `int` |  | FK to account |  |
| `bank_payment_type_id` | `int` |  | FK to Bank_Payment_Type |  |
| `account_holder_name` | `varchar` |  | Account holder name at time of change |  |
| `account_number` | `varchar` |  | Bank account number at time of change |  |
| `bank_name_id` | `int` |  | Bank name reference |  |
| `bank_branch` | `varchar` |  | Bank branch name |  |
| `bank_branch_code` | `varchar` |  | Bank sort code |  |
| `bank_add1` | `varchar` |  | Bank address line 1 |  |
| `bank_add2` | `varchar` |  | Bank address line 2 |  |
| `bank_add3` | `varchar` |  | Bank address line 3 |  |
| `bank_town` | `varchar` |  | Bank town |  |
| `bank_pcode` | `varchar` |  | Bank postcode |  |
| `bank_region` | `varchar` |  | Bank region |  |
| `bank_country` | `varchar` |  | Bank country |  |
| `cc_num` | `varchar` |  | Credit card number (masked) |  |
| `cc_start_date` | `varchar` |  | Credit card start date |  |
| `cc_expiry_date` | `varchar` |  | Credit card expiry date |  |
| `cc_issue_num` | `varchar` |  | Credit card issue number |  |
| `cc_pin` | `varchar` |  | Credit card PIN (should not be stored) |  |
| `is_registered` | `tinyint` |  | Flag: payment method is registered | 0,1 |
| `cc_add1` | `varchar` |  | Card billing address line 1 |  |
| `cc_add2` | `varchar` |  | Card billing address line 2 |  |
| `cc_add3` | `varchar` |  | Card billing address line 3 |  |
| `cc_town` | `varchar` |  | Card billing town |  |
| `cc_pcode` | `varchar` |  | Card billing postcode |  |
| `cc_country` | `varchar` |  | Card billing country |  |
| `user_id` | `int` |  | User who made the change |  |
| `date_modified` | `datetime` |  | Date the record was changed |  |
| `name_on_card` | `varchar` |  | Name as it appears on the card |  |
| `manual_auth_number` | `varchar` |  | Manual authorisation number |  |
| `account_type` | `varchar` |  | Type of bank account |  |
| `cc_tracking_number` | `varchar` |  | Card tracking/reference number |  |
| `business_identifier_code` | `varchar` |  | BIC/SWIFT code |  |
| `international_bank_account_number` | `varchar` |  | IBAN |  |
| `Bank_Name` | `varchar` |  | Bank name free text |  |
| `is_default` | `tinyint` |  | Flag: this is the default payment method | 0,1 |

*Foreign Keys*:
- `Party_Bank_History.party_bank_id` â†’ `Party_Bank (party_bank_id)` (Many-to-One)
- `Party_Bank_History.account_id` â†’ `account (account_id)` (Many-to-One)
- `Party_Bank_History.bank_payment_type_id` â†’ `Bank_Payment_Type (bank_payment_type_id)` (Many-to-One)

#### `Party_Corporate_Client`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Corporate client specific details for a party: company registration number, trade type, SIC code, number of employees, turnover, and incorporation date.

*Keywords*: corporate client, company, company registration, SIC code, employees, turnover, limited company, corporate

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | FK to party (party_cnt) |  |
| `company_reg` | `varchar` |  | Companies House registration number |  |
| `trading_since_date` | `datetime` |  | Date company started trading |  |
| `party_business_id` | `varchar` |  | Business type identifier |  |
| `location` | `int` |  | Location/region code |  |
| `no_of_offices` | `int` |  | Number of offices |  |
| `no_of_employees` | `int` |  | Number of employees |  |
| `financial_year` | `datetime` |  | Financial year end date |  |
| `trade_code` | `varchar` |  | Trade code for the business |  |
| `wage_roll` | `numeric` |  | Annual wage roll / payroll amount |  |
| `turnover` | `numeric` |  | Annual business turnover |  |
| `SIC_code_id` | `int` |  | FK to SIC_code (Standard Industrial Classification) |  |
| `salutation` | `varchar` |  | Salutation for correspondence |  |
| `trade_id` | `int` |  | Trade identifier |  |
| `source` | `varchar` |  | Acquisition source |  |
| `tpsind` | `tinyint` |  | Telephone Preference Service indicator | 0,1 |
| `empsind` | `tinyint` |  | Email Preference Service indicator | 0,1 |
| `tp_password` | `varchar` |  | Third-party portal password |  |
| `mailshot` | `tinyint` |  | Flag: party accepts marketing mailshots | 0,1 |
| `is_fee_client` | `bit` |  | Flag: client is fee-based (not commission) | 0,1 |

*Foreign Keys*:
- `Party_Corporate_Client.party_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `Party_Corporate_Client.SIC_code_id` â†’ `SIC_code (SIC_code_id)` (Many-to-One)

#### `Party_Extra`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Extra agent flags (fee charge, risk transfer, delegated auth)

*Keywords*: Agent, Fee, Delegated Authority, Risk Transfer

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | FK to party (party_cnt) |  |
| `agency_number` | `varchar` |  | Agency number |  |
| `fee_charge` | `tinyint` |  | Flag: party can charge fees | 0,1 |
| `risk_transfer_agreement` | `bit` |  | Flag: risk transfer agreement in place | 0,1 |
| `delegated_authority` | `bit` |  | Flag: party has delegated authority | 0,1 |
| `fsa_product_id` | `int` |  | FK to FSA_Product |  |

*Foreign Keys*:
- `Party_Extra.party_cnt` â†’ `party (party_cnt)` (Many-to-One)

#### `Party_Group_Type`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: party group type codes

*Keywords*: Party Group, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_group_type_id` | `int` |  | Unique group type identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption (display text) |  |
| `code` | `char` |  | Short code for this group type |  |
| `description` | `varchar` |  | Description of the group type |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date this record became effective |  |

#### `Party_Handler`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Handler (staff member) party supplemental data: department, job title, and system access configuration.

*Keywords*: handler, staff, employee, department, job title, underwriter, claims handler, account handler

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | FK to party (party_cnt) |  |
| `forename` | `varchar` |  | First name of the handler |  |
| `initials` | `varchar` |  | Middle initials of the handler |  |
| `department_id` | `int` |  | FK to Department lookup |  |
| `party_title_code` | `varchar` |  | Title code (Mr, Mrs, Dr etc.) |  |
| `commission_cnt` | `int` |  | FK to commission party record |  |

*Foreign Keys*:
- `Party_Handler.party_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `Party_Handler.department_id` â†’ `Department (department_id)` (Many-to-One)

#### `Party_Loyalty_Scheme`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Loyalty scheme memberships linked to a party

*Keywords*: Loyalty, Scheme, Membership

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_loyalty_scheme_id` | `int` |  | Unique loyalty scheme membership (PK) |  |
| `party_cnt` | `int` |  | FK to party (party_cnt) |  |
| `loyalty_scheme_id` | `int` |  | FK to Loyalty_Scheme lookup |  |
| `membership_number` | `varchar` |  | Membership number for this scheme |  |
| `other_reference` | `varchar` |  | Additional reference |  |
| `start_date` | `datetime` |  | Membership start date |  |
| `end_date` | `datetime` |  | Membership end date |  |
| `main_membership_number` | `varchar` |  | Main/primary membership number |  |
| `is_active` | `tinyint` |  | Flag: membership is active | 0,1 |

*Foreign Keys*:
- `Party_Loyalty_Scheme.party_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `Party_Loyalty_Scheme.loyalty_scheme_id` â†’ `Loyalty_Scheme (loyalty_scheme_id)` (Many-to-One)

#### `Party_Net_Data`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Online portal / self-service credentials and security data for a party. Stores username, password hash, and portal access flags for web/online access.

*Keywords*: online portal, web access, credentials, username, password, portal, self-service, client portal

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | FK to party (party_cnt) |  |
| `password` | `varchar` |  | Online portal password (hashed - never expose) |  |
| `mothers_maiden_name` | `varchar` |  | Security question answer - mothers maiden name |  |
| `tp_introducer_code` | `char` |  | Third-party introducer code |  |
| `tp_user_code` | `char` |  | Third-party user code |  |
| `memorable_date` | `datetime` |  | Security verification memorable date |  |
| `a_question` | `varchar` |  | Security question text |  |
| `the_answer` | `varchar` |  | Security question answer |  |
| `userid` | `varchar` |  | Online portal login username |  |
| `current_ins_renewal_date` | `datetime` |  | Current insurance renewal date visible online |  |
| `online_status` | `bit` |  | Flag: party has active online access | 0,1 |
| `online_status_updated` | `datetime` |  | Date online status was last changed |  |
| `contact_cnt` | `int` |  | FK to contact record |  |

*Foreign Keys*:
- `Party_Net_Data.party_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `Party_Net_Data.contact_cnt` â†’ `contact (contact_cnt)` (Many-to-One)

#### `Party_Personal_Client`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Personal client details for a party: title, forename, surname, date of birth, nationality, marital status.

*Keywords*: personal client, name, forename, surname, DOB, date of birth, nationality, marital status, individual

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | FK to party (party_cnt) |  |
| `party_title_code` | `varchar` |  | Title code (Mr, Mrs, Dr etc.) | Mr,Mrs |
| `forename` | `varchar` |  | First name of the personal client |  |
| `initials` | `varchar` |  | Middle initials |  |
| `employment_status_code` | `varchar` |  | Employment status code (employed, self-emp etc) |  |
| `employer_cnt` | `int` |  | FK to employer party |  |
| `employer_business` | `varchar` |  | Employer business name |  |
| `secondary_employer_business` | `varchar` |  | Secondary employer business name |  |
| `secondary_employment_status_co` | `varchar` |  | Secondary employment status code |  |
| `marital_status_code` | `varchar` |  | Marital status code |  |
| `number_of_children` | `int` |  | Number of dependent children |  |
| `Nationality_id` | `int` |  | FK to Nationality lookup |  |
| `country_of_origin_code` | `varchar` |  | Country of origin code |  |
| `mailshot` | `tinyint` |  | Flag: accepts marketing mailshots | 0,1 |
| `is_pet_owner` | `tinyint` |  | Flag: party is a pet owner | 0,1 |
| `accommodation_type_code` | `varchar` |  | Accommodation type code |  |
| `salutation` | `varchar` |  | Salutation for correspondence |  |
| `source` | `varchar` |  | Acquisition source |  |
| `tpsind` | `tinyint` |  | Telephone Preference Service indicator | 0,1 |
| `empsind` | `tinyint` |  | Email Preference Service indicator | 0,1 |
| `tp_password` | `varchar` |  | Third-party portal password |  |
| `is_fee_client` | `bit` |  | Flag: fee-based client (not commission) | 0,1 |

*Foreign Keys*:
- `Party_Personal_Client.party_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `Party_Personal_Client.Nationality_id` â†’ `Nationality (Nationality_id)` (Many-to-One)

#### `Party_Public_Text`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Free-text notes and public comments associated with a party record. Used by handlers to record important notes about a client, agent, or insurer.

*Keywords*: party notes, public text, comment, client notes, handler notes, party text

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | FK to party (party_cnt) |  |
| `party_public_text_id` | `int` |  | Unique public text record identifier (PK) |  |
| `text_line` | `varchar` |  | A line of public text/note for the party |  |

*Foreign Keys*:
- `Party_Public_Text.party_cnt` â†’ `party (party_cnt)` (Many-to-One)

#### `Party_Relationship`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Records relationships between two parties, typed by the relationship role (e.g. Director of, Spouse of, Subsidiary of). Used to model corporate hierarchies and personal connections.

*Keywords*: party relationship, relationship, director, spouse, subsidiary, corporate hierarchy, connection, related party

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | FK to party (party_cnt) - the relationship owner |  |
| `relation_cnt` | `int` |  | FK to related party (party_cnt) |  |
| `relationship_type_id` | `smallint` |  | FK to Relationship_Type |  |
| `description` | `varchar` |  | Description of the relationship |  |
| `commission_transaction` | `bit` |  | Flag: relationship is for commission transactions | 0,1 |
| `UserId` | `int` |  | Last modified by user |  |
| `UniqueId` | `varchar` |  | Unique sync identifier |  |
| `ScreenHierarchy` | `varchar` |  | UI screen hierarchy path |  |

*Foreign Keys*:
- `Party_Relationship.party_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `Party_Relationship.relation_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `Party_Relationship.relationship_type_id` â†’ `Relationship_Type (relationship_type_id)` (Many-to-One)

#### `Party_Relationship_Group`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: groups of relationship types

*Keywords*: Relationship Group, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_relationship_group_id` | `int` |  | Unique relationship group (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code |  |
| `description` | `varchar` |  | Description |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

#### `Party_Structure`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: structural classification of a party

*Keywords*: Party Structure, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_structure_id` | `int` |  | Unique structure identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code |  |
| `description` | `varchar` |  | Description |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `interface_object_name` | `char` |  | Interface object name |  |
| `interface_class_name` | `char` |  | Interface class name |  |

#### `Relationship_Type`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: types of relationships between parties

*Keywords*: Relationship, Party, Type, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `relationship_type_id` | `smallint` |  | Unique relationship type identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code |  |
| `description` | `varchar` |  | Description |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `complementary_type_id` | `smallint` |  | FK to complementary (inverse) relationship type |  |
| `party_relationship_group_id` | `int` |  | FK to Party_Relationship_Group |  |

*Foreign Keys*:
- `Relationship_Type.complementary_type_id` â†’ `Relationship_Type (relationship_type_id)` (Many-to-One)
- `Relationship_Type.party_relationship_group_id` â†’ `Party_Relationship_Group (party_relationship_group_id)` (Many-to-One)

#### `SIC_code`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: Standard Industrial Classification codes

*Keywords*: SIC, Industry, Trade, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `SIC_code_id` | `int` |  | Unique SIC code identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Standard Industrial Classification code |  |
| `description` | `varchar` |  | Industry description |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

#### `Strength_code`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: prospect strength codes (lead quality)

*Keywords*: Prospect, Lead Strength, Quality, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `Strength_code_id` | `int` |  | Unique strength code identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code |  |
| `description` | `varchar` |  | Description of the strength level |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

#### `address`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Postal and geographic address records. Stores house number/name, street, town, county, postcode, country. Used for party correspondence addresses and risk location addresses.

*Keywords*: address, postcode, street, town, county, country, location, correspondence address, risk address, property address, home address

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `address_cnt` | `int` |  | Address Unique Identifier |  |
| `source_id` | `int` |  | Linked Branch |  |
| `address1` | `varchar` |  | Address Line 1 |  |
| `address2` | `varchar` |  | Address Line 2 |  |
| `address3` | `varchar` |  | Address Line 3 |  |
| `address4` | `varchar` |  | Address Line 4 |  |
| `postal_code` | `varchar` |  | Post Code |  |
| `country_id` | `smallint` |  | Country |  |
| `address5` | `varchar` |  | Address Line 5 |  |
| `address6` | `varchar` |  | Address Line 6 |  |
| `address7` | `varchar` |  | Address Line 7 |  |
| `address8` | `varchar` |  | Address Line 8 |  |
| `address9` | `varchar` |  | Address Line 9 |  |
| `address10` | `varchar` |  | Address Line 10 |  |

#### `contact`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Contact record storing communication details: phone numbers, email addresses, mobile numbers, fax. Associated with parties for correspondence and communication.

*Keywords*: contact, phone, email, mobile, fax, telephone, communication, party contact, email address, phone number

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `contact_cnt` | `int` |  | Contact Unique Key |  |
| `contact_type_id` | `smallint` |  | Contact Type Link |  |
| `source_id` | `int` |  | Branch Link |  |
| `country_id` | `smallint` |  | Country |  |
| `description` | `varchar` |  | Description |  |
| `area_code` | `char` |  | Area Code |  |
| `number` | `varchar` |  | Number |  |
| `extension` | `char` |  | Extension |  |

#### `contact_type`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup defining the type of a contact record: Phone, Mobile, Email, Fax, Website.

*Keywords*: contact type, phone, mobile, email, fax, website, lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `contact_type_id` | `smallint` |  | Contact Type |  |
| `code` | `char` |  | Code |  |
| `description` | `varchar` |  | Description |  |
| `effective_date` | `datetime` |  | Effective Date |  |

#### `party`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Central entity representing any person or organisation in the system: clients (personal or corporate), agents/brokers, insurers, reinsurers, handlers (staff), TPAs, and other parties. Joined to sub-type tables (party_insurer, party_agent, Party_Personal_Client etc.) for type-specific attributes. party_cnt is the primary key.

*Keywords*: party, client, agent, broker, insurer, reinsurer, handler, staff, TPA, person, organisation, policyholder, party_cnt, party name

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | Unique party identifier |  |
| `party_type_id` | `int` |  | Type of Party (Personal Client, Corporate Client, Agent |  |
| `shortname` | `varchar` |  | short name/code of Party |  |
| `resolved_name` | `varchar` |  | Full Name of Party |  |
| `name` | `varchar` |  | Full Name of Party |  |

*Foreign Keys*:
- `party.party_type_id` â†’ `party_type (party_type_id)` (Many-to-One)

#### `party_address_usage`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Links an address to a party for a specific usage type (e.g. Home, Correspondence, Business, Risk Location). A party may have multiple addresses of different types.

*Keywords*: party address, address link, home address, correspondence address, business address, risk location, party, address usage

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | Party linked |  |
| `address_cnt` | `int` |  | Address linked |  |
| `description` | `varchar` |  | Description |  |
| `address_usage_type_id` | `int` |  | Address Type |  |

#### `party_agent_type`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: agent type classification codes

*Keywords*: Agent Type, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_agent_type_id` | `int` |  | Unique agent type identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code |  |
| `description` | `varchar` |  | Description |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `is_visible` | `int` |  | Flag: type is visible in UI | 0,1 |

#### `party_business`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: business type codes for parties

*Keywords*: Business Type, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_business_id` | `int` |  | Unique business type identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code |  |
| `description` | `varchar` |  | Description |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

#### `party_category`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: party category codes

*Keywords*: Party Category, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_category_id` | `int` |  | Unique category identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code |  |
| `description` | `varchar` |  | Description |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

#### `party_contact_usage`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Links a contact record (phone/email/mobile) to a party for a specific contact type. A party may have multiple contacts.

*Keywords*: party contact, phone, email, mobile, fax, contact link, contact usage, party communication

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | Party |  |
| `contact_cnt` | `int` |  | Contact |  |
| `description` | `varchar` |  | Description |  |

#### `party_conviction`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Motor conviction records attached to a party

*Keywords*: Conviction, Motor, Offence, Penalty Points

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | FK to party (party_cnt) |  |
| `party_conviction_id` | `int` |  | Unique conviction record (PK) |  |
| `code` | `varchar` |  | Conviction code |  |
| `conviction_date` | `varchar` |  | Date of conviction |  |
| `description` | `varchar` |  | Description of the conviction |  |
| `fine_amt` | `numeric` |  | Fine amount imposed |  |
| `sentence_code` | `varchar` |  | Sentence type code |  |
| `sentence_description` | `varchar` |  | Sentence description |  |
| `sentence_duration` | `decimal` |  | Duration of sentence |  |
| `sentence_duration_qualifier` | `varchar` |  | Unit qualifier for sentence duration |  |
| `sentence_effective_date` | `varchar` |  | Date sentence took effect |  |
| `status_code` | `varchar` |  | Status of the conviction record |  |
| `alcohol_level` | `decimal` |  | Alcohol level at time of offence |  |
| `alcohol_measurement_method` | `varchar` |  | Method used to measure alcohol level |  |
| `driving_licence_penalty_pts` | `decimal` |  | Penalty points on driving licence |  |

*Foreign Keys*:
- `party_conviction.party_cnt` â†’ `party (party_cnt)` (Many-to-One)

#### `party_handler_branch`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Links a handler party to a source/branch

*Keywords*: Handler, Branch, Source

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | FK to party (party_cnt) |  |
| `source_id` | `int` |  | FK to source/branch assignment |  |

*Foreign Keys*:
- `party_handler_branch.party_cnt` â†’ `party (party_cnt)` (Many-to-One)

#### `party_insurer`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Insurer party details: RI type, credit ratings, payment terms, commission arrangements, and whether they act as a coinsurer or lead insurer.

*Keywords*: insurer, reinsurer, RI, credit rating, payment terms, commission, coinsurer, lead insurer, insurer details

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | FK to party (party_cnt) |  |
| `agency_number` | `varchar` |  | Agency reference number |  |
| `binder_indicator` | `int` |  | Binder holding indicator |  |
| `report_indicator` | `int` |  | Report access indicator |  |
| `is_reinsurer` | `tinyint` |  | Flag: party is a reinsurer | 0,1 |
| `reinsurance_type` | `int` |  | Type of reinsurance arrangement |  |
| `is_reinsurance_debit_credit_no` | `tinyint` |  | Flag: RI uses debit/credit note accounting | 0,1 |
| `default_comm_rate` | `float` |  | Default commission rate |  |
| `method` | `varchar` |  | Billing/settlement method |  |
| `icr` | `decimal` |  | Insurer commission rate |  |
| `polaris_insurer_no` | `int` |  | Insurer number in Polaris system |  |
| `abi_1_edi_directory` | `varchar` |  | ABI EDI directory code |  |
| `payment_method` | `int` |  | Payment method code |  |
| `payment_frequency` | `int` |  | Payment frequency code |  |
| `bank_account` | `varchar` |  | Bank account reference for insurer |  |
| `fsa_registration_number` | `varchar` |  | FCA/FSA registration number |  |
| `fsa_insurercreditrating_id` | `int` |  | Insurer credit rating from FSA/FCA |  |
| `fsa_insurerstatus_id` | `int` |  | Insurer status from FSA/FCA |  |
| `is_retained` | `tinyint` |  | Flag: insurer is retained/captive | 0,1 |
| `tax_group_id` | `int` |  | FK to tax group |  |
| `claims_rating_agency_id` | `int` |  | FK to Claims_Rating_Agency |  |
| `claims_rating_grading` | `varchar` |  | Rating grade from the claims rating agency |  |
| `claims_rating_date` | `datetime` |  | Date of the claims rating assessment |  |
| `claims_rating_description` | `varchar` |  | Description of the claims rating |  |
| `terms_of_payment_id` | `int` |  | FK to payment terms |  |
| `domiciled_for_tax` | `tinyint` |  | Flag: domiciled for tax purposes | 0,1 |
| `risk_transfer_agreement` | `bit` |  | Flag: risk transfer agreement in place | 0,1 |
| `Brokerlink_Subaccount` | `int` |  | Brokerlink sub-account number |  |
| `Brokerlink_UW_ID` | `varchar` |  | Brokerlink underwriter ID |  |
| `is_ri_broker` | `tinyint` |  | Flag: party is an RI broker | 0,1 |
| `Insurer_locking_type_id` | `int` |  | FK to Insurer_Locking_Type |  |
| `risk_transfer_editable` | `bit` |  | Flag: risk transfer flag is editable | 0,1 |
| `insurer_type_id` | `int` |  | FK to Insurer_Type |  |
| `BureauAccountParty` | `int` |  | Bureau account party link |  |
| `UserId` | `int` |  | Last modified by user |  |
| `UniqueId` | `varchar` |  | Unique sync identifier |  |
| `ScreenHierarchy` | `varchar` |  | UI screen hierarchy path |  |

*Foreign Keys*:
- `party_insurer.party_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `party_insurer.claims_rating_agency_id` â†’ `Claims_Rating_Agency (Claims_Rating_Agency_id)` (Many-to-One)
- `party_insurer.Insurer_locking_type_id` â†’ `Insurer_Locking_Type (Insurer_locking_type_id)` (Many-to-One)
- `party_insurer.insurer_type_id` â†’ `Insurer_Type (insurer_Type_id)` (Many-to-One)

#### `party_lifestyle`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lifestyle details for a personal client party: date of birth, gender, occupation, smoker status. Used in risk assessment and underwriting.

*Keywords*: lifestyle, date of birth, DOB, gender, occupation, smoker, personal details, underwriting, client lifestyle

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | FK to party (party_cnt) |  |
| `party_lifestyle_id` | `int` |  | Unique lifestyle record identifier (PK) |  |
| `name` | `varchar` |  | Name of the lifestyle member/dependent |  |
| `category` | `int` |  | Category code for the lifestyle record |  |
| `date_of_birth` | `datetime` |  | Date of birth of lifestyle member |  |
| `gender_code` | `varchar` |  | Gender code | M,F |
| `occupation_code` | `varchar` |  | Occupation code |  |
| `secondary_occupation_code` | `varchar` |  | Secondary occupation code |  |
| `is_smoker` | `tinyint` |  | Flag: is smoker | 0,1 |

*Foreign Keys*:
- `party_lifestyle.party_cnt` â†’ `party (party_cnt)` (Many-to-One)

#### `party_other`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Generic other-party details (license, gender, TPA flags)

*Keywords*: Other, TPA, License, Vehicle

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | FK to party (party_cnt) |  |
| `license_type_id` | `int` |  | FK to License_type lookup |  |
| `license_number` | `varchar` |  | License/registration number |  |
| `date_of_birth` | `datetime` |  | Date of birth |  |
| `gender` | `varchar` |  | Gender code | M,F |
| `party_status` | `int` |  | Status of the party |  |
| `reference_number` | `varchar` |  | External reference number |  |
| `external_id` | `int` |  | External system identifier |  |
| `reg_number` | `varchar` |  | Registration number |  |
| `date_passed_test` | `datetime` |  | Date passed relevant test |  |
| `contact_name` | `varchar` |  | Contact name |  |
| `contact_telephone_number` | `varchar` |  | Contact telephone number |  |
| `insurer_name` | `varchar` |  | Previous/associated insurer name |  |
| `insurer_address1` | `varchar` |  | Insurer address line 1 |  |
| `insurer_address2` | `varchar` |  | Insurer address line 2 |  |
| `insurer_address3` | `varchar` |  | Insurer address line 3 |  |
| `insurer_address4` | `varchar` |  | Insurer address line 4 |  |
| `insurer_postcode` | `varchar` |  | Insurer postcode |  |
| `insurer_telephone_number` | `varchar` |  | Insurer telephone number |  |
| `insurer_fax_number` | `varchar` |  | Insurer fax number |  |
| `insurer_contact_name` | `varchar` |  | Insurer contact person name |  |
| `insurer_email` | `varchar` |  | Insurer email address |  |
| `insurer_notes` | `varchar` |  | Notes about the insurer |  |
| `company_notes` | `varchar` |  | General company notes |  |
| `active_indicator` | `bit` |  | Flag: party is active | 0,1 |
| `after_hours_indicator` | `bit` |  | Flag: party handles after-hours | 0,1 |
| `Priority_indicator` | `tinyint` |  | Priority level indicator | 0,1 |
| `is_TPA_settle_directly` | `tinyint` |  | Flag: TPA settles claims directly | 0,1 |

*Foreign Keys*:
- `party_other.party_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `party_other.license_type_id` â†’ `License_type (license_type_id)` (Many-to-One)

#### `party_prospect`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Prospect-specific data for a party: pipeline status, source, lead strength, and conversion details. Used for CRM and pipeline management.

*Keywords*: prospect, lead, pipeline, CRM, status, lead strength, conversion, sales pipeline, prospect status

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_cnt` | `int` |  | FK to party (party_cnt) |  |
| `agent_reference` | `varchar` |  | Agent-assigned reference for the prospect |  |
| `current_intermediary` | `int` |  | FK to current intermediary party |  |
| `prospect_status_id` | `int` |  | FK to prospect_status (pipeline stage) |  |
| `Strength_code_id` | `int` |  | FK to Strength_code (lead quality) |  |
| `previous_insurer_cnt` | `int` |  | FK to previous insurer party |  |
| `previous_broker_cnt` | `int` |  | FK to previous broker party |  |

*Foreign Keys*:
- `party_prospect.party_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `party_prospect.prospect_status_id` â†’ `prospect_status (prospect_status_id)` (Many-to-One)
- `party_prospect.Strength_code_id` â†’ `Strength_code (Strength_code_id)` (Many-to-One)
- `party_prospect.current_intermediary` â†’ `party (party_cnt)` (Many-to-One)

#### `party_type`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup defining the type/role of a party: Client, Agent/Broker, Insurer, Reinsurer, Handler (staff), Other.

*Keywords*: party type, client, agent, broker, insurer, handler, other, party classification, lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `party_type_id` | `int` |  | Party Type Id |  |
| `code` | `char` |  | Party Type Code | PC, GC, AG, CC |
| `description` | `varchar` |  | Party Type Description | Personal Client, Group Client, Agent, Corporate Client |

#### `prospect_status`
**Domain: Customer**  **Owner: Customer Service**  **Refresh: Real-time**

Lookup: prospect pipeline status codes

*Keywords*: Prospect, Pipeline, Status, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `prospect_status_id` | `int` |  | Unique status identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code |  |
| `description` | `varchar` |  | Description of the status |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

---

## GIS

**Tables in this module**: 2

### Query Rules

**Sum Insured and Premium by Rating Section** â€” `rating_section.risk_cnt = risk.risk_cnt`  
*The rating_section table holds the breakdown of sum insured and calculated premium by individual cover section within a risk. Join risk → rating_section to get section-level exposure and premium details.*

**GIS (Risk) Property Values** â€” `GIS_Property.risk_cnt = risk.risk_cnt AND GIS_Property.gis_data_model_id = GIS_Data_Model.gis_data_model_id AND GIS_Data_Model.property_name = @property_name`  
*Risk attributes (e.g. postcode, building type, alarm type) are stored as GIS properties linked to risks. To get a specific property value, join risk → GIS_Data_Model (to find property definition) and filter on property_name. Values are stored in GIS_Property.*

### Tables

#### `GIS_User_Def_Detail`
**Domain: Underwriting**  **Owner: IT Department**  **Refresh: On Change**

Stores the individual valid values (items) within each user-defined GIS lookup list defined by GIS_User_Def_Header. e.g. for Licence_Type the items would be Full, Provisional, International. Users select from these values when entering risk data.

*Keywords*: GIS, user-defined, lookup, list item, risk attribute, valid value

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `gis_user_def_detail_id` | `int` | âœ“ | PK — unique identifier for one item within a user-defined GIS lookup list. | 1, 2, 1000 |
| `gis_user_def_header_id` | `int` |  | FK to GIS_User_Def_Header — the list type this item belongs to. | 1, 2, 78 |
| `caption_id` | `int` |  | FK to PMCaption — display label for this lookup value. | 1432, 1433 |
| `code` | `char(10)` |  | Short code for this lookup item. Used as the stored value in GIS risk properties. | Full, Provisiona, COMP |
| `description` | `varchar(255)` |  | Human-readable description of this lookup value shown to users. | Full, Provisional, Comprehensive |
| `is_deleted` | `tinyint` |  | 0 = active, 1 = soft-deleted. | 0, 1 |
| `effective_date` | `datetime` |  | Date from which this value is effective. | 2006-02-22 |
| `Parent` | `int` |  | Parent item id for hierarchical sub-lists. -1 = top-level. | -1, NULL |
| `GIS_user_def_header_inds_id` | `int` |  | FK to GIS_User_Def_Header_Inds — indicator flags associated with this value. |  |
| `system_generated` | `int` |  | 1 = system-created, 0 = user-created. | 0, 1 |
| `UserId` | `int` |  | FK to PMUser — last modifier. |  |
| `UniqueId` | `varchar(50)` |  | System GUID for upgrade/sync. |  |
| `ScreenHierarchy` | `varchar(500)` |  | Navigation hierarchy path for the admin UI. |  |

*Foreign Keys*:
- `GIS_User_Def_Detail.gis_user_def_header_id` â†’ `GIS_User_Def_Header` (Many-to-One)

#### `GIS_User_Def_Header`
**Domain: Underwriting**  **Owner: IT Department**  **Refresh: On Change**

Defines the header/type record for each user-defined GIS lookup list (e.g. Licence_Type, Body_Type, Build_Type). Each header represents one configurable drop-down list used on risk entry screens. The detail items are stored in GIS_User_Def_Detail.

*Keywords*: GIS, user-defined, lookup, list, header, risk attribute, dropdown

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `gis_user_def_header_id` | `int` | âœ“ | PK — unique identifier for a user-defined GIS lookup list type. | 1, 2, 78 |
| `caption_id` | `int` |  | FK to PMCaption — the display label for this lookup list. | 1431, 1434 |
| `code` | `char(10)` |  | Short code identifying this list type. e.g. S4ILicence, S4IBODY. | S4ILicence, S4IBODY |
| `description` | `varchar(255)` |  | Human-readable description of the lookup list. e.g. S4I_Licence_Type, S4I_Body_Type. | S4I_Licence_Type, S4I_Body_Type |
| `is_deleted` | `tinyint` |  | 0 = active, 1 = soft-deleted (hidden from UI but retained for history). | 0, 1 |
| `effective_date` | `datetime` |  | Date from which this lookup list definition is effective. | 2006-02-22 |
| `Parent` | `int` |  | Optional parent list id — supports hierarchical lookup lists. -1 = top-level (no parent). | -1, NULL |
| `system_generated` | `int` |  | 1 = created by the system/product configuration (not user-defined). 0 = user-created. | 0, 1 |
| `UserId` | `int` |  | FK to PMUser — the user who last modified this record. |  |
| `UniqueId` | `varchar(50)` |  | System unique identifier (GUID-style) used for upgrade and synchronisation. |  |
| `ScreenHierarchy` | `varchar(500)` |  | Navigation hierarchy string used by the UI to position this list in the admin screens. |  |

---

## Document Management

**Tables in this module**: 19

### Tables

#### `DME_Migration_Status`
**Domain: Document Management**  **Owner: IT**  **Refresh: Static**

Lookup table for DME (Document Management Engine) migration status codes, indicating which phase of migration a document is in.

*Keywords*: DME, migration, status, document

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `DME_Migration_Status_id` | `int` | âœ“ | Primary key identifier for migration status. | 1,2,3 |
| `code` | `varchar` |  | Short code for the migration status. | NOT_MIGRATED,MIGRATED |
| `description` | `varchar` |  | Human-readable description of the migration status. | Not Migrated,Migrated |
| `caption_id` | `int` |  | Foreign key to caption/label resource. |  |
| `effective_date` | `datetime` |  | Date from which this status became effective. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag (1 = deleted). | 0,1 |

#### `DOC_annotation`
**Domain: Document Management**  **Owner: IT**  **Refresh: Real-time**

Stores text annotations attached to documents, recording who created the annotation and when.

*Keywords*: annotation, document, note, comment

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `annotation_id` | `int` | âœ“ | Primary key for the annotation record. |  |
| `doc_num` | `int` |  | Foreign key to DOC_document identifying the annotated document. |  |
| `ann_text` | `varchar` |  | The text content of the annotation. |  |
| `user_name` | `varchar` |  | Username of the person who created the annotation. |  |
| `create_date` | `datetime` |  | Date and time the annotation was created. |  |

*Foreign Keys*:
- `DOC_annotation.doc_num` â†’ `DOC_document.doc_num` (Many-to-One)

#### `DOC_device`
**Domain: Document Management**  **Owner: IT**  **Refresh: Static**

Defines physical or virtual storage devices used by the document management system, with network share details.

*Keywords*: device, storage, network share, DME

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `device_id` | `int` | âœ“ | Primary key for the storage device. |  |
| `device_name` | `varchar` |  | Friendly name of the storage device. |  |
| `server_unc` | `varchar` |  | UNC server path for the device. |  |
| `share_name` | `varchar` |  | Network share name on the server. |  |
| `drive` | `varchar` |  | Drive letter mapping for the shared volume. |  |

#### `DOC_doc_info`
**Domain: Document Management**  **Owner: IT**  **Refresh: Real-time**

Supplementary metadata for documents including expiry date, scan user, and last modification details.

*Keywords*: document, metadata, expiry, scan, audit

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `doc_num` | `int` | âœ“ | Primary key / foreign key to DOC_document. |  |
| `expiry_date` | `datetime` |  | Date on which the document expires / should be purged. |  |
| `scan_user` | `varchar` |  | Username of the person who scanned the document. |  |
| `doc_date` | `datetime` |  | Date assigned to the document content. |  |
| `last_user` | `varchar` |  | Username who last modified the document. |  |
| `last_date` | `datetime` |  | Date and time of the last modification. |  |

*Foreign Keys*:
- `DOC_doc_info.doc_num` â†’ `DOC_document.doc_num` (One-to-One)

#### `DOC_doc_keyword`
**Domain: Document Management**  **Owner: IT**  **Refresh: Real-time**

Junction table linking documents to their keyword tags for search and categorisation.

*Keywords*: document, keyword, tag, search, link

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `doc_keyword_id` | `int` | âœ“ | Primary key for the document-keyword link. |  |
| `doc_num` | `int` |  | Foreign key to DOC_document. |  |
| `keyword_id` | `int` |  | Foreign key to DOC_keyword. |  |
| `user_name` | `varchar` |  | Username who applied the keyword. |  |
| `create_date` | `datetime` |  | Date and time the keyword was applied. |  |

*Foreign Keys*:
- `DOC_doc_keyword.doc_num` â†’ `DOC_document.doc_num` (Many-to-One)
- `DOC_doc_keyword.keyword_id` â†’ `DOC_keyword.keyword_id` (Many-to-One)

#### `DOC_doc_name`
**Domain: Document Management**  **Owner: IT**  **Refresh: Static**

Lookup of document name templates/categories used during document creation.

*Keywords*: document name, template, category

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `doc_name_id` | `int` | âœ“ | Primary key for the document name entry. |  |
| `doc_name` | `varchar` |  | Name or title template for a document type. |  |

#### `DOC_doc_user`
**Domain: Document Management**  **Owner: IT**  **Refresh: Admin**

Defines document management system users with their access level and home folder assignment.

*Keywords*: user, access level, home folder, DME

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `user_id` | `smallint` | âœ“ | Primary key / user identifier in the DME system. |  |
| `access_level` | `tinyint` |  | Access level granted to the user in DME (higher = more access). |  |
| `user_name` | `varchar` |  | Login username for the DME user. |  |
| `home_folder_num` | `int` |  | Foreign key to DOC_folder — user s default home folder. |  |
| `retired` | `char` |  | Flag indicating whether the user account is retired (Y/N). | Y,N |

#### `DOC_document`
**Domain: Document Management**  **Owner: IT**  **Refresh: Real-time**

Core document records in the DME system, holding folder, name, type, access level, and migration status for each document.

*Keywords*: document, DME, folder, type, access, migration

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `doc_num` | `int` | âœ“ | Primary key — unique document number within DME. |  |
| `folder_num` | `int` |  | Foreign key to DOC_folder indicating the document s folder. |  |
| `doc_name` | `varchar` |  | Name or title of the document. |  |
| `ex_code` | `varchar` |  | External reference code linking the document to a business entity. |  |
| `doc_type` | `char` |  | Document type code (e.g. T=text, I=image, P=PDF). | T,I,P |
| `access_level` | `tinyint` |  | Access level controlling who can view or modify the document. |  |
| `create_date` | `datetime` |  | Date and time the document was created in DME. |  |
| `zipped` | `char` |  | Indicates whether the document file is compressed (Y/N). | Y,N |
| `link` | `int` |  | Link indicator — 0 = standalone, non-zero = linked/copy. | 0,1 |
| `document_template_id` | `int` |  | References a document template used to generate this document. |  |
| `batch_id` | `int` |  | Batch identifier for documents generated as part of a bulk process. |  |
| `visible_from_web` | `bit` |  | Indicates whether the document is accessible via web portal. | 0,1 |
| `DME_Migration_Status_id` | `int` |  | Foreign key to DME_Migration_Status — migration state of this document. |  |
| `migration_id` | `int` |  | Migration batch identifier used during DME data migration. |  |

*Foreign Keys*:
- `DOC_document.folder_num` â†’ `DOC_folder.folder_num` (Many-to-One)
- `DOC_document.DME_Migration_Status_id` â†’ `DME_Migration_Status.DME_Migration_Status_id` (Many-to-One)

#### `DOC_folder`
**Domain: Document Management**  **Owner: IT**  **Refresh: Real-time**

Represents the hierarchical folder structure in the DME document management system with access controls.

*Keywords*: folder, hierarchy, document, access, DME

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `folder_num` | `int` | âœ“ | Primary key — unique folder number in DME. |  |
| `folder_name` | `varchar` |  | Display name of the folder. |  |
| `parent_num` | `int` |  | Self-referencing FK to parent folder (0 = root). |  |
| `ex_code` | `varchar` |  | External code linking the folder to a business entity (e.g. policy ref). |  |
| `folder_level` | `tinyint` |  | Depth level of the folder in the hierarchy. | 0,1,2,3 |
| `access_level` | `tinyint` |  | Access level required to view this folder. |  |
| `password` | `varchar` |  | Optional password protecting the folder. |  |
| `create_date` | `datetime` |  | Date and time the folder was created. |  |

#### `DOC_history`
**Domain: Document Management**  **Owner: IT**  **Refresh: Real-time**

Audit history log of DME document events — cabinet/drawer/folder movements, retrievals, and status changes.

*Keywords*: history, audit, document, cabinet, drawer, folder, event

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `history_id` | `int` | âœ“ | Primary key for the history log entry. |  |
| `task` | `smallint` |  | Task code indicating the type of operation performed. |  |
| `cabinetcode` | `char` |  | Cabinet code from legacy DME structure. |  |
| `cabinetname` | `char` |  | Cabinet name from legacy DME structure. |  |
| `drawercode` | `char` |  | Drawer code within the cabinet. |  |
| `drawername` | `char` |  | Drawer name within the cabinet. |  |
| `foldercode` | `char` |  | Folder code within the drawer. |  |
| `foldername` | `char` |  | Folder name within the drawer. |  |
| `docref` | `char` |  | Document reference identifier. |  |
| `eventtype` | `char` |  | Single-character event type code (e.g. R=retrieve, D=delete). | R,D,U |
| `description` | `char` |  | Short description of the history event. |  |
| `volume` | `char` |  | Storage volume identifier at time of event. |  |
| `create_date` | `datetime` |  | Date and time the history record was written. |  |
| `processed` | `char` |  | Flag indicating whether this history entry has been processed (Y/N). | Y,N |

#### `DOC_keyword`
**Domain: Document Management**  **Owner: IT**  **Refresh: Static**

Master keyword list used for tagging and searching documents in the DME system.

*Keywords*: keyword, tag, search, document

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `keyword_id` | `int` | âœ“ | Primary key for the keyword. |  |
| `keyword` | `varchar` |  | The keyword text used for document tagging and search. |  |
| `deleted` | `char` |  | Soft-delete flag (Y = deleted). | Y,N |

#### `DOC_options`
**Domain: Document Management**  **Owner: IT**  **Refresh: Static**

System-level configuration options for the DME document management module stored as name/value pairs.

*Keywords*: options, configuration, DME, system

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `option_id` | `int` | âœ“ | Primary key for the system option. |  |
| `option_name` | `varchar` |  | Name of the DME system option. |  |
| `option_value` | `varchar` |  | Value configured for the option. |  |

#### `DOC_page`
**Domain: Document Management**  **Owner: IT**  **Refresh: Real-time**

Individual page records for multi-page documents, tracking page number, type, size, and storage volume.

*Keywords*: page, document, volume, storage, DME

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `page_name` | `varchar` | âœ“ | File name of the page stored on disk. |  |
| `doc_num` | `int` |  | Foreign key to DOC_document. |  |
| `page_num` | `int` |  | Sequential page number within the document. | 1,2,3 |
| `page_type` | `varchar` |  | MIME type or format identifier for the page (e.g. image/tiff). | image/tiff,application/pdf |
| `create_date` | `datetime` |  | Date and time the page was stored. |  |
| `page_size` | `int` |  | Size of the page file in bytes. |  |
| `volume_id` | `int` |  | Foreign key to DOC_volume — the storage volume holding this page. |  |

*Foreign Keys*:
- `DOC_page.doc_num` â†’ `DOC_document.doc_num` (Many-to-One)
- `DOC_page.volume_id` â†’ `DOC_volume.volume_id` (Many-to-One)

#### `DOC_system`
**Domain: Document Management**  **Owner: IT**  **Refresh: Static**

Global system settings for the DME document management engine including retention periods and permission levels.

*Keywords*: system, configuration, DME, retention, permissions

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `system_id` | `int` | âœ“ | Primary key — single row configuration record. | 1 |
| `doc_date` | `tinyint` |  | Number of days ahead to set default document date. |  |
| `expiry_date` | `smallint` |  | Default expiry period (days) for new documents. |  |
| `admin_level` | `tinyint` |  | Minimum access level required for admin operations. |  |
| `update_history` | `char` |  | Whether to write history records for updates (Y/N). | Y,N |
| `File_Delete_Level` | `tinyint` |  | Access level required to delete a file. |  |
| `File_Move_Level` | `tinyint` |  | Access level required to move a file. |  |
| `Folder_Delete_Level` | `tinyint` |  | Access level required to delete a folder. |  |

#### `DOC_volume`
**Domain: Document Management**  **Owner: IT**  **Refresh: Static**

Storage volumes available to the DME system, referencing the physical device they reside on.

*Keywords*: volume, storage, device, DME

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `volume_id` | `int` | âœ“ | Primary key for the storage volume. |  |
| `volume_name` | `varchar` |  | Friendly name of the storage volume. |  |
| `directory` | `varchar` |  | Directory path on the device for this volume. |  |
| `device_id` | `tinyint` |  | Foreign key to DOC_device — the physical device hosting this volume. |  |

*Foreign Keys*:
- `DOC_volume.device_id` â†’ `DOC_device.device_id` (Many-to-One)

#### `Document_Spooler`
**Domain: Operations**  **Owner: IT Department**  **Refresh: Real-time**

A print/archive queue holding pending documents yet to be printed or archived. Documents are spooled here when generated and can be linked to a party, insurance folder, policy version, or claim. Tracks how many times a document has been printed or archived, whether it is deletable/editable, and which document template was used.

*Keywords*: document, spool, print, queue, archive, policy, claim, correspondence

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `document_spooler_id` | `int` | âœ“ | PK — unique id for this spooled document record. |  |
| `document_type_id` | `int` |  | FK to Document_Type — the type of document (e.g. renewal notice, debit note). |  |
| `party_cnt` | `int` |  | FK to Party — the party (client or broker) this document is for, if applicable. |  |
| `insurance_folder_cnt` | `int` |  | FK to Insurance_Folder — the client folder this document relates to, if applicable. |  |
| `insurance_file_cnt` | `int` |  | FK to Insurance_File — the specific policy version this document relates to, if applicable. |  |
| `claim_cnt` | `int` |  | FK to Claim — the claim this document relates to, if applicable. |  |
| `description` | `varchar(255)` |  | A short label describing this spooled document (e.g. the document reference or report name). | RNC10, Agent List |
| `is_deletable` | `tinyint` |  | 1 = this document can be deleted from the spool by a user. 0 = protected. | 0, 1 |
| `is_editable` | `tinyint` |  | 1 = this document can be edited before printing. 0 = read-only. | 0, 1 |
| `created_by_id` | `smallint` |  | FK to PMUser — the user who generated/created this document. |  |
| `date_created` | `datetime` |  | Date and time this document was spooled/created. |  |
| `modified_by_id` | `smallint` |  | FK to PMUser — the user who last modified this spool record. |  |
| `date_modified` | `datetime` |  | Date and time this spool record was last modified. |  |
| `times_printed` | `int` |  | Number of times this document has been printed from the spool. 0 = not yet printed. | 0, 1, 2 |
| `times_archived` | `int` |  | Number of times this document has been archived. 0 = not yet archived. | 0, 1 |
| `printer` | `varchar(255)` |  | Name of the printer this document was sent to, if applicable. |  |
| `spool_level_ind` | `tinyint` |  | Indicates the audience level for this document — office copy, client copy, agent copy. |  |
| `source_id` | `int` |  | FK to Source — the business source/branch context for this document. |  |
| `document_template_id` | `int` |  | FK to Document_Template — the template used to generate this document. |  |
| `is_client` | `tinyint` |  | 1 = this is a client-facing copy of the document. | 0, 1 |
| `is_agent` | `tinyint` |  | 1 = this is a broker/agent copy of the document. | 0, 1 |
| `is_office` | `tinyint` |  | 1 = this is an office (internal) copy of the document. | 0, 1 |
| `production_order` | `tinyint` |  | Sequence/order in which documents within a batch are produced. | 1, 2, 3 |

*Foreign Keys*:
- `Document_Spooler.document_type_id` â†’ `Document_Type` (Many-to-One)
- `Document_Spooler.party_cnt` â†’ `Party` (Many-to-One)
- `Document_Spooler.insurance_folder_cnt` â†’ `Insurance_Folder` (Many-to-One)
- `Document_Spooler.insurance_file_cnt` â†’ `Insurance_File` (Many-to-One)
- `Document_Spooler.created_by_id` â†’ `PMUser` (Many-to-One)
- `Document_Spooler.modified_by_id` â†’ `PMUser` (Many-to-One)

#### `Document_Template`
**Domain: Operations**  **Owner: Operations Department**  **Refresh: Real-time**

Document template definitions (letters, notices, policy documents)

*Keywords*: Document, Template, Letter, Notice, Report

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `document_template_id` | `int` |  | Unique document template identifier (PK) |  |
| `code` | `char` |  | Short code for the template |  |
| `description` | `varchar` |  | Template description |  |
| `source_id` | `int` |  | FK to source/branch scope |  |
| `document_type_id` | `int` |  | FK to document type |  |
| `created_by_id` | `smallint` |  | FK to PMUser who created |  |
| `date_created` | `datetime` |  | Date template was created |  |
| `modified_by_id` | `smallint` |  | FK to PMUser who last modified |  |
| `last_modified` | `datetime` |  | Date template was last modified |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `is_editable_after_merging` | `tinyint` |  | Flag: can be edited after merge | 0,1 |
| `document_template_group_id` | `int` |  | FK to Document_Template_Group |  |
| `document_template_sub_group_id` | `int` |  | FK to Document_Template_Sub_Group |  |
| `effective_date` | `datetime` |  | Date template became effective |  |
| `archive_as_text` | `bit` |  | Flag: archive as plain text | 0,1 |
| `spool_document` | `tinyint` |  | Flag: spool document for batch printing | 0,1 |

#### `Document_Type`
**Domain: Document Management**  **Owner: IT**  **Refresh: Static**

Lookup defining types of document templates available in the system: e.g. Standard Letter, Debit Note, Credit Control, Policy Schedule, Client Text File Template, Policy Text File Template. Controls which template types can be created and whether they are editable after merging.

*Keywords*: document type, template type, letter, debit note, credit control, policy schedule, lookup, document_type_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `document_type_id` | `int` | âœ“ | Primary key for the document type. |  |
| `code` | `char` |  | Short code for the document type (e.g. LETTER, DEBIT, CCONTROL). | LETTER,DEBIT,CCONTROL,PTEXT,CTEXT |
| `description` | `varchar` |  | Description of the document type (e.g. Standard Letter, Debit Note, Credit Control). | Standard Letter,Debit Note,Credit Control,Policy Schedule |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this document type is effective. |  |
| `is_editable_after_merging` | `tinyint` |  | Whether the generated document can be edited by a user after merge (1=yes). | 0,1 |

#### `wp_fields`
**Domain: Document Management**  **Owner: IT**  **Refresh: Static**

Registry of all merge fields available for use in document templates (letters, policy schedules, renewal notices, claim correspondence, etc.). Each row defines a single merge field with its internal name, display name, the SQL stored procedure that retrieves it, the source column, data model, product family, main group (e.g. Policy, Party, Claim, Risk, Finance), and loop context for repeating sections. Used by the document template engine to populate dynamic variables in generated documents.

*Keywords*: merge field, document template, wp field, mail merge, letter, policy schedule, renewal notice, claim letter, field name, template variable, document generation, field_name, main_group, sql, display_name

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `field_name` | `varchar` | âœ“ | The unique merge field token name used in document templates (e.g. «PolicyNumber», «ClientName»). | PolicyNumber,ClientName,PremiumAmount |
| `sql` | `varchar` |  | Name of the stored procedure or SQL function that retrieves the value for this merge field at document generation time. | spu_wp_partyall,spg_wp_policy |
| `column_name` | `varchar` |  | The column or output alias returned by the SQL procedure that contains this field value. |  |
| `column_type` | `int` |  | Data type code for the field value (e.g. 0=string, 13=boolean, 18=number). | 0,13,18 |
| `main_group` | `varchar` |  | High-level grouping of the merge field for UI categorisation. Examples: Policy, Party, Claim, Risk, Finance, Instalment, Renewals, Reinsurance, System. | Policy,Party,Claim,Risk,Finance,Instalment,Renewals,Reinsurance,System |
| `sub_group` | `varchar` |  | Sub-category within the main group (e.g. within Policy: Cover, Premium, Agent; within Party: All, Agent, Address Contact). | Cover,Premium,Agent,All,Address Contact |
| `display_name` | `varchar` |  | Human-readable label shown in the template editor when selecting a merge field. | Policy Number,Client Name,Total Premium |
| `is_displayed` | `tinyint` |  | Whether this field is visible in the merge field picker UI (1=displayed, 0=hidden). | 0,1 |
| `loop1` | `varchar` |  | First loop context name — identifies the repeating block this field must appear inside (e.g. PartyAddress, RiskCovers). | PartyAddress,RiskCovers,Instalments |
| `loop2` | `varchar` |  | Second (nested) loop context for fields that appear inside a loop within a loop. | ContactAddress |
| `loop3` | `varchar` |  | Third (deeply nested) loop context. |  |
| `loop4` | `varchar` |  | Fourth loop context for deeply nested repeating sections. |  |
| `product_family` | `int` |  | Product family identifier controlling which products this merge field is available for. | 9 |
| `data_model` | `varchar` |  | GIS data model code this field is associated with (for GIS-sourced fields, e.g. ACCESS, MOTOR). | ACCESS,MOTOR |
| `property_id` | `int` |  | Foreign key to GIS_Property — the GIS property this merge field maps to (for GIS-driven fields). |  |
| `sub_group2` | `varchar` |  | Additional sub-category level 2 for further field grouping in the template editor. |  |
| `sub_group3` | `varchar` |  | Additional sub-category level 3. |  |
| `sub_group4` | `varchar` |  | Additional sub-category level 4. |  |
| `hidden_option_number` | `int` |  | Hidden system option number that must be set for this field to be available (feature-flag control). |  |
| `required_option_value` | `varchar` |  | The value the hidden option must have for this field to be enabled. |  |
| `specials_type` | `int` |  | Special handling type code for fields requiring non-standard rendering (e.g. formatted text, images, tables). | 0 |
| `Table_Name` | `varchar` |  | The underlying database table or view name from which this field value originates. | CorePartyAll,CorePartyAgent |
| `DataStructure_Name` | `varchar` |  | The data structure/object name in the document engine data model for this field. |  |

*Foreign Keys*:
- `wp_fields.property_id` â†’ `GIS_Property.gis_property_id` (Many-to-One)

---

## Tax

**Tables in this module**: 10

### Tables

#### `Tax_Band`
**Domain: Finance**  **Owner: Finance**  **Refresh: Static**

Named tax band grouping related tax rates within a tax type. Determines which rates apply based on country, currency, class of business and transaction type (NB/MTA/REN/CANC). See also Tax_Band (same table).

*Keywords*: tax band, IPT band, tax rate, tax type, calculation, premium tax, insurance premium tax, tax_band_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `tax_band_id` | `int` | âœ“ | Primary key for the tax band. |  |
| `code` | `char` |  | Short code identifying the tax band. | IPT,VAT_STD |
| `caption_id` | `int` |  | Foreign key to caption/label resource for UI display. |  |
| `description` | `varchar` |  | Human-readable description of the tax band. | Insurance Premium Tax,Standard Rate VAT |
| `effective_date` | `datetime` |  | Date from which this tax band is effective. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag (1 = deleted). | 0,1 |
| `tax_type_id` | `int` |  | Foreign key to Tax_Type — the tax type this band belongs to. |  |

*Foreign Keys*:
- `Tax_Band.tax_type_id` â†’ `Tax_Type.tax_type_id` (Many-to-One)

#### `Tax_Calculation`
**Domain: Finance**  **Owner: Finance**  **Refresh: Real-time**

Tax calculation record for a financial transaction. Stores the calculated tax amount, premium basis, percentage applied, band, group and transaction type (NB/MTA/REN/CANC). Linked to risk, insurance file, or claim. See also Tax_Calculation (same table).

*Keywords*: tax calculation, IPT, VAT, premium tax, tax amount, NB, MTA, renewal, cancellation, claim tax, tax band, tax group

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `tax_calculation_cnt` | `int` | âœ“ | Primary key — unique identifier for the tax calculation record. |  |
| `risk_cnt` | `int` |  | Foreign key to Risk — the risk this tax was calculated against. |  |
| `insurance_file_cnt` | `int` |  | Foreign key to the insurance file this calculation belongs to. |  |
| `tax_band_id` | `int` |  | Foreign key to Tax_Band — the band applied for this calculation. |  |
| `tax_group_id` | `int` |  | Foreign key to Tax_Group. |  |
| `premium` | `money` |  | Premium amount used as the basis for tax calculation. |  |
| `percentage` | `float` |  | Tax rate percentage applied. | 0.12,0.20 |
| `value` | `money` |  | Calculated tax amount. |  |
| `is_value` | `tinyint` |  | Whether the rate was a fixed value rather than percentage. | 0,1 |
| `transtype` | `varchar` |  | Transaction type code (e.g. NB, MTA, CANC) for which tax was calculated. | NB,AMTA,RMTA,REN,CANC |
| `currency_id` | `smallint` |  | Foreign key to Currency for the tax amount. |  |
| `country_id` | `smallint` |  | Foreign key to Country. |  |
| `claim_payment_id` | `int` |  | Foreign key to Claim_Payment if the tax relates to a claim payment. |  |
| `claim_receipt_id` | `int` |  | Foreign key to Claim_Receipt if the tax relates to a claim receipt. |  |
| `is_not_applied_to_client` | `tinyint` |  | Indicates the tax is not charged directly to the client. | 0,1 |
| `include_tax_in_instalments` | `tinyint` |  | Whether this tax is included in instalment calculations. | 0,1 |
| `is_suspended` | `tinyint` |  | Whether this tax calculation is suspended from posting. | 0,1 |

*Foreign Keys*:
- `Tax_Calculation.tax_band_id` â†’ `Tax_Band.tax_band_id` (Many-to-One)
- `Tax_Calculation.tax_group_id` â†’ `Tax_Group.tax_group_id` (Many-to-One)
- `Tax_Calculation.risk_cnt` â†’ `Risk.risk_cnt` (Many-to-One)
- `Tax_Calculation.currency_id` â†’ `Currency.currency_id` (Many-to-One)
- `Tax_Calculation.country_id` â†’ `Country.country_id` (Many-to-One)
- `Tax_Calculation.claim_payment_id` â†’ `Claim_Payment.claim_payment_id` (Many-to-One)
- `Tax_Calculation.claim_receipt_id` â†’ `Claim_Receipt.claim_receipt_id` (Many-to-One)

#### `Tax_Exempt_Postcodes`
**Domain: Finance**  **Owner: Finance**  **Refresh: Static**

Stores postcode ranges exempt from a given tax group, used to suppress tax calculations for specific geographical locations.

*Keywords*: tax exempt, postcode, tax group, geography

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `Tax_group_id` | `int` |  | Foreign key to Tax_Group — the tax group from which the postcode is exempt. |  |
| `Postcode` | `varchar` |  | Postcode or postcode prefix that is exempt from the tax group. | BT,IM,GY |

*Foreign Keys*:
- `Tax_Exempt_Postcodes.Tax_group_id` â†’ `Tax_Group.tax_group_id` (Many-to-One)

#### `Tax_Group`
**Domain: Finance**  **Owner: Finance**  **Refresh: Static**

Groups one or more tax bands into a logical tax group applied to a product or risk type. Examples: UK IPT, VAT, Stamp Duty Reserve Tax (SDRT). Supports withholding tax and advanced scripting. See also Tax_Group (same table).

*Keywords*: tax group, IPT, VAT, stamp duty, SDRT, withholding tax, tax rule, product tax, tax_group_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `tax_group_id` | `int` | âœ“ | Primary key for the tax group. |  |
| `code` | `varchar` |  | Short code identifying the tax group. | UK_IPT,SDRT |
| `description` | `varchar` |  | Description of the tax group. | UK Insurance Premium Tax |
| `effective_date` | `datetime` |  | Date from which this tax group is effective. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag (1 = deleted). | 0,1 |
| `is_withholding_tax` | `tinyint` |  | Indicates this group represents a withholding tax. | 0,1 |
| `Rule_Type` | `int` |  | Foreign key to risk_type_rule_set_type — business rule type controlling tax application. |  |
| `advanced_tax_script` | `varchar` |  | Name of an advanced script used for complex tax calculation logic. |  |
| `is_coinsurer_multiple_tax_group` | `tinyint` |  | Whether this group supports multiple tax bands in a coinsurance structure. | 0,1 |
| `is_tax_amount_editable` | `tinyint` |  | Whether a user can override the calculated tax amount. | 0,1 |

*Foreign Keys*:
- `Tax_Group.Rule_Type` â†’ `risk_type_rule_set_type.risk_type_rule_set_type_id` (Many-to-One)

#### `Tax_Rates`
**Domain: Finance**  **Owner: Finance**  **Refresh: Static**

Simple lookup of tax rates by country with effective date and soft-delete support, used for straightforward percentage-based tax calculations.

*Keywords*: tax rate, country, rate, lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `tax_rates_id` | `smallint` | âœ“ | Primary key for the tax rate entry. |  |
| `code` | `char` |  | Short code for the rate. |  |
| `description` | `varchar` |  | Description of the rate. |  |
| `rate` | `numeric` |  | The tax rate as a decimal percentage. | 0.12,0.20 |
| `country_id` | `smallint` |  | Foreign key to Country — the country this rate applies to. |  |
| `effective_date` | `datetime` |  | Date from which this rate is effective. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |

*Foreign Keys*:
- `Tax_Rates.country_id` â†’ `Country.country_id` (Many-to-One)

#### `Tax_Type`
**Domain: Finance**  **Owner: Finance**  **Refresh: Static**

Lookup defining types of tax (e.g. IPT, VAT, stamp duty) with basis, instalment inclusion rules, and age analysis exclusion flags.

*Keywords*: tax type, IPT, VAT, stamp duty, basis, instalment

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `tax_type_id` | `int` | âœ“ | Primary key for the tax type. |  |
| `code` | `char` |  | Short code for the tax type. | IPT,VAT,SDT |
| `description` | `varchar` |  | Description of the tax type. | Insurance Premium Tax,VAT |
| `tax_basis` | `tinyint` |  | Foreign key to Tax_Type_Basis defining how the tax is calculated. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this tax type is effective. |  |
| `is_not_applied_to_client` | `tinyint` |  | Whether this tax type is hidden from the client (applied internally). | 0,1 |
| `is_excluded_from_age_analysis` | `tinyint` |  | Whether this tax is excluded from debtor age analysis reports. | 0,1 |
| `is_include_tax_in_instalments` | `tinyint` |  | Default setting for including this tax in instalment schedules. | 0,1 |
| `is_spread_tax_across_instalments` | `tinyint` |  | Whether the tax is spread evenly across instalments. | 0,1 |

*Foreign Keys*:
- `Tax_Type.tax_basis` â†’ `Tax_Type_Basis.Tax_Type_Basis_ID` (Many-to-One)

#### `Tax_Type_Band`
**Domain: Finance**  **Owner: Finance**  **Refresh: Static**

Links tax types to specific band rates with a numeric rate and description, providing an alternative simpler banding structure to tax_band_rate.

*Keywords*: tax type, tax band, rate, banding

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `tax_type_band_id` | `int` | âœ“ | Primary key for the tax type band entry. |  |
| `tax_type_id` | `int` |  | Foreign key to Tax_Type. |  |
| `tax_band` | `tinyint` |  | Band number/code within the type. |  |
| `is_value` | `tinyint` |  | Whether the rate is a fixed value (1) vs percentage (0). | 0,1 |
| `rate` | `numeric` |  | The rate applied for this band. | 0.12 |
| `description` | `varchar` |  | Description of the band rate. |  |

*Foreign Keys*:
- `Tax_Type_Band.tax_type_id` â†’ `Tax_Type.tax_type_id` (Many-to-One)

#### `Tax_Type_Basis`
**Domain: Finance**  **Owner: Finance**  **Refresh: Static**

Reference lookup defining the basis on which a tax type is calculated (e.g. on premium, on sum insured).

*Keywords*: tax basis, tax type, calculation method

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `Tax_Type_Basis_ID` | `tinyint` | âœ“ | Primary key for the tax type basis. |  |
| `code` | `char` |  | Short code for the basis. | NP,GP,SI |
| `Description` | `varchar` |  | Description of the calculation basis (e.g. Net Premium, Gross Premium). | Net Premium,Sum Insured |
| `Caption_ID` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this basis is effective. |  |

#### `Tax_group_tax_band`
**Domain: Finance**  **Owner: Finance**  **Refresh: Static**

Junction table linking tax groups to tax bands with sequence and allocation rules controlling the order and method of tax application.

*Keywords*: tax group, tax band, sequence, allocation, link

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `tax_group_id` | `int` |  | Foreign key to Tax_Group. |  |
| `tax_band_id` | `int` |  | Foreign key to Tax_Band. |  |
| `sequence` | `smallint` |  | Order in which the tax band is applied within the group. | 1,2,3 |
| `allocation_sequence` | `smallint` |  | Sequence used for allocating tax amounts across bands. | 1,2 |
| `allocation_rule` | `smallint` |  | Rule code governing how tax amounts are allocated. |  |

*Foreign Keys*:
- `Tax_group_tax_band.tax_group_id` â†’ `Tax_Group.tax_group_id` (Many-to-One)
- `Tax_group_tax_band.tax_band_id` â†’ `Tax_Band.tax_band_id` (Many-to-One)

#### `tax_band_rate`
**Domain: Finance**  **Owner: Finance**  **Refresh: Static**

Detailed rate records for each tax band, specifying percentage or value rates along with applicable transaction types (NB, MTA, renewal, cancellation) and filters by country, currency, class of business.

*Keywords*: tax rate, tax band, NB, MTA, renewal, country, currency, premium, calculation

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `tax_band_rate_id` | `int` | âœ“ | Primary key for the tax band rate record. |  |
| `tax_band_id` | `int` |  | Foreign key to Tax_Band. |  |
| `code` | `char` |  | Short code for this rate entry. |  |
| `description` | `varchar` |  | Description of this rate entry. |  |
| `rate` | `float` |  | Tax rate as a percentage or absolute value. | 0.12,0.20 |
| `is_value` | `tinyint` |  | Whether the rate is a fixed value (1) rather than a percentage (0). | 0,1 |
| `Calc_Basis` | `int` |  | Calculation basis code (e.g. on net premium, gross premium). |  |
| `Basis_Value` | `money` |  | Monetary basis value when calc basis requires a fixed amount threshold. |  |
| `NB` | `tinyint` |  | Apply this rate to New Business transactions (1=yes). | 0,1 |
| `AMTA` | `tinyint` |  | Apply this rate to Additional MTA (mid-term adjustment) transactions. | 0,1 |
| `RMTA` | `tinyint` |  | Apply this rate to Return MTA transactions. | 0,1 |
| `CANC` | `tinyint` |  | Apply this rate to Cancellation transactions. | 0,1 |
| `REN` | `tinyint` |  | Apply this rate to Renewal transactions. | 0,1 |
| `currency_id` | `smallint` |  | Foreign key to Currency — rate applies only for this currency. |  |
| `country_id` | `smallint` |  | Foreign key to Country — rate applies only for this country. |  |
| `class_of_business_id` | `int` |  | Foreign key to Class_Of_Business — rate applies only for this COB. |  |
| `effective_date` | `datetime` |  | Date from which this rate is effective. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `is_suspended` | `tinyint` |  | Indicates the rate is suspended and should not be applied. | 0,1 |
| `Is_passed_to_insurer` | `tinyint` |  | Indicates whether this tax is passed on to the insurer. | 0,1 |

*Foreign Keys*:
- `tax_band_rate.tax_band_id` â†’ `Tax_Band.tax_band_id` (Many-to-One)
- `tax_band_rate.currency_id` â†’ `Currency.currency_id` (Many-to-One)
- `tax_band_rate.country_id` â†’ `Country.country_id` (Many-to-One)
- `tax_band_rate.class_of_business_id` â†’ `Class_Of_Business.class_of_business_id` (Many-to-One)

---

## Reporting

**Tables in this module**: 4

### Tables

#### `Report_Group`
**Domain: Admin**  **Owner: IT Department**  **Refresh: Admin**

Lookup grouping reports into categories (e.g. All, Underwriting, Claims, Statistics, Finance). Used in the report scheduler and report UI to organise and filter available reports by functional area.

*Keywords*: report group, report category, underwriting reports, claims reports, statistics, report_group_id, UND, CLM, STATS

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `report_group_id` | `int` | âœ“ | Primary key for the report group. |  |
| `code` | `char` |  | Short code for the group (e.g. All, UND, CLM, STATS). | All,UND,CLM,STATS |
| `description` | `varchar` |  | Description of the report group (e.g. All, Underwriting, Claims, Statistics). | All,Underwriting,Claims,Statistics |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this group is effective. |  |

#### `Report_Group_User_Groups`
**Domain: Admin**  **Owner: IT Department**  **Refresh: On Change**

Access control link table — defines which PMUser user groups have permission to access which report groups. If a user group is not listed for a report group, members of that group cannot see or run the reports in that group.

*Keywords*: report group, user group, access control, permissions, security

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `report_group_id` | `int` |  | FK to Report_Group — the report group that a user group has access to. |  |
| `pmuser_group_id` | `int` |  | FK to PMUser_Group — the user group that can access this report group. |  |

*Foreign Keys*:
- `Report_Group_User_Groups.report_group_id` â†’ `Report_Group` (Many-to-One)
- `Report_Group_User_Groups.pmuser_group_id` â†’ `PMUser_Group` (Many-to-One)

#### `stats_detail`
**Domain: Underwriting**  **Owner: Underwriting Department**  **Refresh: Real-time**

Line-level breakdown of each stats_folder transaction. Contains premium, reinsurance, sum insured, commission and tax amounts broken down by risk, peril and class of business.

*Keywords*: Stats Detail, Premium Breakdown, Reinsurance, Sum Insured, Commission, Tax, Peril, Risk, Class of Business

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `stats_folder_cnt` | `int` |  | Links to parent stats_folder record (composite PK) | 0,1,2 |
| `stats_detail_id` | `int` |  | Unique line identifier within the stats folder (composite PK) | 1,2,3 |
| `stats_detail_type` | `char` |  | Type code for the detail line e.g. TAG | TAG |
| `risk_id` | `int` |  | Risk identifier from the risk table | 92849 |
| `risk_type_id` | `int` |  | Risk type identifier | 42 |
| `risk_type_code` | `char` |  | Risk type code e.g. SME | SME |
| `peril_id` | `int` |  | Peril identifier |  |
| `peril_description` | `varchar` |  | Description of the peril e.g. SME - Property All Risks | SME - Property All Risks |
| `peril_type_id` | `int` |  | Peril type identifier | 138 |
| `peril_type_code` | `char` |  | Peril type code e.g. SMEPAR | SMEPAR |
| `policy_section_type_id` | `int` |  | Policy section type identifier |  |
| `policy_section_type_code` | `char` |  | Policy section type code |  |
| `class_of_business_id` | `int` |  | Class of business identifier | 73 |
| `class_of_business_code` | `char` |  | Class of business code e.g. SME | SME |
| `tax_type_id` | `int` |  | Tax type identifier |  |
| `tax_type_code` | `char` |  | Tax type code |  |
| `tax_value` | `numeric` |  | Tax amount value |  |
| `ri_party_cnt` | `int` |  | Reinsurance party identifier |  |
| `ri_shortname` | `varchar` |  | Reinsurance party short name | NOTAIPTIN |
| `ri_party_type` | `char` |  | Reinsurance party type code | 0 |
| `ri_share_percent` | `float` |  | Percentage share held by the reinsurer | 0.0 |
| `ri_agreement_code` | `varchar` |  | Reinsurance agreement reference code |  |
| `annual_premium` | `numeric` |  | Annualised premium amount in original currency | 5263069864.46 |
| `currency_code` | `char` |  | Currency of the premium amounts | ZAR,GBP,USD |
| `currency_rate` | `numeric` |  | Exchange rate to home currency | 0.0480 |
| `this_premium_original` | `numeric` |  | Premium for this transaction in original currency |  |
| `this_premium_home` | `numeric` |  | Premium for this transaction converted to home currency |  |
| `commission_percent` | `float` |  | Commission percentage applied |  |
| `lead_commission_value_home` | `numeric` |  | Lead broker commission value in home currency |  |
| `sub_commission_value_home` | `numeric` |  | Sub broker commission value in home currency |  |
| `sum_insured_home` | `numeric` |  | Sum insured converted to home currency |  |
| `sum_insured_currency_code` | `char` |  | Currency code of the sum insured |  |
| `sum_insured_change` | `numeric` |  | Change in sum insured for this transaction |  |
| `transaction_ledger_id` | `char` |  | Ledger identifier for the transaction line |  |
| `transaction_account_id` | `int` |  | Account identifier for the transaction line |  |
| `account_type_code` | `char` |  | Account type code |  |
| `ceded_ref` | `char` |  | Ceded reinsurance reference |  |
| `cover_share_percent` | `numeric` |  | Cover share percentage |  |
| `sum_insured_total` | `numeric` |  | Total sum insured for the line | 1000000.00 |
| `charges_total` | `numeric` |  | Total charges on the line |  |
| `taxes_total` | `numeric` |  | Total taxes on the line |  |
| `recoveries_total` | `numeric` |  | Total recoveries on the line |  |
| `commission_excluded` | `numeric` |  | Commission excluded from calculation |  |
| `withholding_tax_excluded` | `numeric` |  | Withholding tax excluded from calculation |  |
| `purchase_order_no` | `varchar` |  | Purchase order number |  |
| `purchase_invoice_no` | `varchar` |  | Purchase invoice number |  |
| `stats_version` | `int` |  | Version number of the stats record | 13 |
| `this_premium_system` | `money` |  | Premium for this transaction in system currency |  |
| `lead_commission_value_system` | `money` |  | Lead broker commission value in system currency |  |
| `sub_commission_value_system` | `money` |  | Sub broker commission value in system currency |  |
| `sum_insured_system` | `money` |  | Sum insured in system currency |  |
| `is_commission_modified` | `tinyint` |  | Flag indicating if commission was manually modified | 0,1 |
| `original_flag` | `tinyint` |  | Flag indicating original record | 0,1 |
| `cover_to_date` | `datetime` |  | Cover to date for the detail line |  |
| `Claim_RI_Only_Amendment` | `int` |  | Flag for claim RI only amendment |  |
| `Earning_Pattern_id` | `int` |  | Earning pattern identifier (FK to Earning_Pattern) | 1 |
| `ri_arrangement_line_Id` | `int` |  | Reinsurance arrangement line identifier |  |

*Foreign Keys*:
- `stats_detail.stats_folder_cnt` â†’ `stats_folder (stats_folder_cnt)` (Many-to-One)
- `stats_detail.Earning_Pattern_id` â†’ `Earning_Pattern (Earning_Pattern_id)` (Many-to-One)
- `stats_detail.risk_id` â†’ `risk (risk_cnt)` (Many-to-One)
- `stats_detail.risk_type_id` â†’ `risk_type (risk_type_id)` (Many-to-One)
- `stats_detail.peril_id` â†’ `peril (peril_id)` (Many-to-One)
- `stats_detail.peril_type_id` â†’ `peril_type (peril_type_id)` (Many-to-One)
- `stats_detail.policy_section_type_id` â†’ `policy_section_type (policy_section_type_id)` (Many-to-One)
- `stats_detail.class_of_business_id` â†’ `class_of_business (class_of_business_id)` (Many-to-One)
- `stats_detail.tax_type_id` â†’ `tax_type (tax_type_id)` (Many-to-One)
- `stats_detail.ri_party_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `stats_detail.transaction_account_id` â†’ `account (account_id)` (Many-to-One)
- `stats_detail.ri_arrangement_line_Id` â†’ `ri_arrangement_line (ri_arrangement_line_id)` (Many-to-One)

#### `stats_folder`
**Domain: Underwriting**  **Owner: Underwriting Department**  **Refresh: Real-time**

Pre-aggregated reporting header table for policy financial transactions. Denormalized snapshot combining policy, party, transaction and organisational data to support analytics without deep joins.

*Keywords*: Stats, Reporting, Policy Transaction, Premium, New Business, MTA, Renewal, Cancellation, Agent, Branch, Currency

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `stats_folder_cnt` | `int` |  | Unique stats folder identifier (PK) | 1,2,3,4 |
| `product_id` | `int` |  | Product identifier | 12 |
| `source_id` | `int` |  | Policy branch/source identifier | 1,2,3 |
| `debit_credit` | `char` |  | Indicates debit (D) or credit (C) transaction | D,C |
| `document_ref` | `varchar` |  | Document reference number | SND00000001 |
| `document_comment` | `varchar` |  | Optional comment on the document |  |
| `document_date` | `datetime` |  | Date the document was created |  |
| `accounting_date` | `datetime` |  | Accounting date for the transaction |  |
| `posting_period_year` | `int` |  | Financial year of the posting period | 2011,2012 |
| `posting_period_number` | `smallint` |  | Week/period number within the posting year | 34,35 |
| `premium_total` | `numeric` |  | Total premium amount for the transaction | 1000.00,600.00 |
| `transaction_type_id` | `int` |  | Transaction type identifier | 4 |
| `transaction_type_code` | `char` |  | Transaction type code e.g. NB, MTA, REN, CAN | NB,MTA,REN,CAN |
| `transaction_date` | `datetime` |  | Date of the transaction |  |
| `insurance_file_cnt` | `int` |  | Links to the policy version (insurance_file) | 1,2,3 |
| `insurance_ref` | `varchar` |  | Policy or quote reference number | MRULPOL0528 |
| `effective_date` | `datetime` |  | Effective date of the transaction on the policy |  |
| `cover_start_date` | `datetime` |  | Cover start date of the policy version |  |
| `expiry_date` | `datetime` |  | Expiry date of the policy version |  |
| `insurance_holder_cnt` | `int` |  | Client party identifier | 1155,1157 |
| `insurance_holder_shortname` | `varchar` |  | Client short name | AMITAA |
| `insurance_holder_name` | `varchar` |  | Client full name | AMIT |
| `product_code` | `char` |  | Product code | AAA2011 |
| `business_type_id` | `smallint` |  | Business type identifier (Direct, Agency etc) | 1,7 |
| `business_type_code` | `char` |  | Business type code e.g. DIRECT, AGENCY | DIRECT,AGENCY |
| `account_handler_cnt` | `int` |  | Account handler party identifier |  |
| `account_handler_shortname` | `varchar` |  | Account handler short name |  |
| `branch_id` | `smallint` |  | Branch identifier | 1 |
| `branch_code` | `char` |  | Branch code e.g. HQ | HQ |
| `currency_code` | `char` |  | Currency of the transaction | GBP,USD,ZAR |
| `agent_cnt` | `int` |  | Broker/Agent party identifier | 237 |
| `agent_shortname` | `varchar` |  | Broker/Agent short name | 1stBROKERS |
| `loss_id` | `int` |  | Claim/loss identifier if transaction relates to a claim |  |
| `loss_code` | `varchar` |  | Claim/loss reference code |  |
| `loss_date` | `datetime` |  | Date of the loss/claim |  |
| `created_by_user_id` | `smallint` |  | User ID who created the record | 1,23 |
| `created_by_username` | `varchar` |  | Username who created the record | sirius,ursula |
| `stats_version` | `int` |  | Version number of the stats record | 13 |
| `underwriting_year_id` | `int` |  | Underwriting year identifier (FK to Underwriting_Year) |  |
| `payment_id` | `int` |  | Payment reference identifier |  |
| `Receipt_Id` | `int` |  | Receipt reference identifier |  |

*Foreign Keys*:
- `stats_folder.underwriting_year_id` â†’ `Underwriting_Year (underwriting_year_id)` (Many-to-One)
- `stats_folder.source_id` â†’ `source (source_id)` (Many-to-One)
- `stats_folder.product_id` â†’ `product (product_id)` (Many-to-One)
- `stats_folder.insurance_file_cnt` â†’ `insurance_file (insurance_file_cnt)` (Many-to-One)
- `stats_folder.insurance_holder_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `stats_folder.account_handler_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `stats_folder.agent_cnt` â†’ `party (party_cnt)` (Many-to-One)
- `stats_folder.loss_id` â†’ `claim (claim_id)` (Many-to-One)
- `stats_folder.transaction_type_id` â†’ `transaction_type (transaction_type_id)` (Many-to-One)
- `stats_folder.business_type_id` â†’ `business_type (business_type_id)` (Many-to-One)
- `stats_folder.branch_id` â†’ `branch (branch_id)` (Many-to-One)

---

## User Management

**Tables in this module**: 10

### Tables

#### `PMUser`
**Domain: Admin**  **Owner: IT Department**  **Refresh: Real-time**

System users (staff). Stores login credentials, user details, role, active dates and audit flags.

*Keywords*: User, Staff, Login, PMUser, Account Handler

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `user_id` | `smallint` |  | Unique user identifier (PK) | 1,2,3 |
| `username` | `varchar` |  | Login username | sirius,ursula |
| `full_name` | `varchar` |  | Full name of the user |  |
| `initials` | `varchar` |  | User initials |  |
| `sirius_user` | `bit` |  | Flag: user has Sirius system access | 0,1 |
| `date_deleted` | `datetime` |  | Date the user was soft-deleted |  |
| `is_locked` | `tinyint` |  | Flag: account is locked due to failed attempts | 0,1 |
| `is_temp_password` | `bit` |  | Flag: user must change temporary password | 0,1 |
| `secure_password` | `varchar` |  | Hashed/encrypted password (never expose) |  |
| `alternative_identifier` | `varchar` |  | Alternative user identifier for SSO |  |
| `sso_preferred_username` | `varchar` |  | Preferred SSO username |  |
| `job_title_id` | `int` |  | Job title reference |  |
| `telephone_number` | `varchar` |  | Office telephone number |  |
| `mobile_number` | `varchar` |  | Mobile telephone number |  |

#### `PMUser_Authority_Level`
**Domain: Admin**  **Owner: IT Department**  **Refresh: Admin**

Assigns an authority level to a specific user for a specific product. Combined with PMUser_Authority_Rule_Set_Link, this controls the maximum policy/transaction values a user can authorise (underwriting authority).

*Keywords*: user authority, underwriting authority, authority level, product access, user_id, product_id, authority_level_type_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `user_id` | `smallint` | âœ“ | Foreign key to PMUser — the user whose authority is being defined. |  |
| `product_id` | `int` | âœ“ | Foreign key to Product — the product this authority applies to. |  |
| `authority_level_type_id` | `int` | âœ“ | Foreign key to Authority_Level_Type — the authority level assigned. |  |

*Foreign Keys*:
- `PMUser_Authority_Level.user_id` â†’ `PMUser.user_id` (Many-to-One)
- `PMUser_Authority_Level.authority_level_type_id` â†’ `Authority_Level_Type.authority_level_type_id` (Many-to-One)

#### `PMUser_Authority_Rule_Set_Link`
**Domain: Admin**  **Owner: IT Department**  **Refresh: Admin**

Defines which rule set applies for a given combination of authority level type, underwriter flag, product and transaction type. Drives underwriting authority checks when policies are submitted or modified.

*Keywords*: authority rule set, underwriting rules, authority level, product, transaction type, rule_set_id, is_underwriter

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `authority_level_type_id` | `int` | âœ“ | Foreign key to Authority_Level_Type — the authority level this rule applies to. |  |
| `is_underwriter` | `tinyint` | âœ“ | Whether this rule applies when the user is acting as an underwriter (1=yes). | 0,1 |
| `product_id` | `int` | âœ“ | Foreign key to Product — the product this rule applies to. |  |
| `transaction_type_id` | `int` | âœ“ | Foreign key to Transaction_Type — the transaction type this rule applies to. |  |
| `rule_set_id` | `int` | âœ“ | Foreign key to the rule set defining the authority limits for this combination. |  |

#### `PMUser_Group`
**Domain: Admin**  **Owner: IT Department**  **Refresh: Admin**

User groups used for access control and workflow assignment. Groups can be marked as system admin groups and are linked to activity task groups, branches, and debtor management workflows. Users are members of one or more groups via PMUser_Group_User.

*Keywords*: user group, access control, system admin, workflow, activity group, pmuser_group_id, SYSADMIN, broker group

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pmuser_group_id` | `int` |  | Unique user group identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code for the group |  |
| `description` | `varchar` |  | Description of the user group |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `is_sys_admin_group` | `tinyint` |  | Flag: this is the system admin group | 0,1 |
| `pmuser_group_id` | `int` | âœ“ | Primary key for the user group. |  |
| `code` | `char` |  | Short code for the group (e.g. SYSADMIN, NOMLDGR, SFORB). | SYSADMIN,NOMLDGR,SFORB |
| `description` | `varchar` |  | Full name of the user group (e.g. System Administrators, Nominal Ledger Team). | System Administrators,Nominal Ledger Team |
| `is_sys_admin_group` | `tinyint` |  | Whether this group has system administrator privileges (1=yes). | 0,1 |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this group is effective. |  |

#### `PMUser_Group_Activity`
**Domain: Admin**  **Owner: IT Department**  **Refresh: Admin**

Links user groups to work task groups (activity queues). Determines which workflow activity queues a user group is responsible for processing.

*Keywords*: user group activity, workflow queue, task group assignment, pmuser_group_id, pmwrk_task_group_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pmuser_group_id` | `int` | âœ“ | Foreign key to PMUser_Group. |  |
| `pmwrk_task_group_id` | `int` | âœ“ | Foreign key to PMWrk_Task_Group — the workflow activity queue assigned to this group. |  |
| `display_sequence_num` | `int` |  | Display order of the activity within the group. | 0,1,2 |

*Foreign Keys*:
- `PMUser_Group_Activity.pmuser_group_id` â†’ `PMUser_Group.pmuser_group_id` (Many-to-One)
- `PMUser_Group_Activity.pmwrk_task_group_id` â†’ `PMWrk_Task_Group.pmwrk_task_group_id` (Many-to-One)

#### `PMUser_Group_Group`
**Domain: Admin**  **Owner: IT Department**  **Refresh: Admin**

Supports group nesting — allows a user group to contain other user groups as members. A group (pmuser_group_id) has member groups (pmuser_member_group_id), enabling hierarchical group structures.

*Keywords*: group of groups, nested groups, group hierarchy, pmuser_group_id, pmuser_member_group_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pmuser_group_id` | `int` | âœ“ | Foreign key to PMUser_Group — the parent group that contains the member group. |  |
| `pmuser_member_group_id` | `int` | âœ“ | Foreign key to PMUser_Group — the child/member group nested within the parent. |  |
| `display_sequence_num` | `int` |  | Display order of the member group within the parent. | 0,1,2 |

*Foreign Keys*:
- `PMUser_Group_Group.pmuser_group_id` â†’ `PMUser_Group.pmuser_group_id` (Many-to-One)
- `PMUser_Group_Group.pmuser_member_group_id` â†’ `PMUser_Group.pmuser_group_id` (Many-to-One)

#### `PMUser_Group_User`
**Domain: Admin**  **Owner: IT Department**  **Refresh: Admin**

Many-to-many link assigning users to user groups. Also records whether the user is a supervisor of that group and their display sequence within the group.

*Keywords*: user group membership, group user, supervisor, display sequence, pmuser_group_id, user_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pmuser_group_id` | `int` | âœ“ | Foreign key to PMUser_Group — the group the user belongs to. |  |
| `user_id` | `smallint` | âœ“ | Foreign key to PMUser — the user assigned to the group. |  |
| `pmuser_group_user_id` | `int` |  | Surrogate key for the group-user link. |  |
| `is_supervisor` | `tinyint` |  | Whether this user is a supervisor of the group (1=yes). | 0,1 |
| `display_sequence_num` | `int` |  | Display order of the user within the group listing. | 0,1,2 |
| `UniqueId` | `varchar` |  | Unique identifier for synchronisation purposes. |  |
| `ScreenHierarchy` | `varchar` |  | Screen navigation context. |  |

*Foreign Keys*:
- `PMUser_Group_User.pmuser_group_id` â†’ `PMUser_Group.pmuser_group_id` (Many-to-One)
- `PMUser_Group_User.user_id` â†’ `PMUser.user_id` (Many-to-One)

#### `PMUser_Password_History`
**Domain: Admin**  **Owner: IT Department**  **Refresh: Real-time**

Stores hashed historical passwords for each user to enforce password reuse policy. Prevents users from re-using recent passwords when changing their credentials.

*Keywords*: password history, password reuse, hashed password, user_id, historic_password, security, bcrypt

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `user_id` | `smallint` |  | Foreign key to PMUser — the user this password history belongs to. |  |
| `historic_password` | `varchar` |  | Bcrypt-hashed historical password. Never expose this value. |  |
| `date_added` | `datetime` |  | Date and time this password was set/recorded in the history. |  |

*Foreign Keys*:
- `PMUser_Password_History.user_id` â†’ `PMUser.user_id` (Many-to-One)

#### `PMUser_Source`
**Domain: Admin**  **Owner: IT Department**  **Refresh: Admin**

Maps which branches (sources) each user has access to. Used to enforce data segregation — a user can only see and process business from their assigned branches.

*Keywords*: user branch access, source access, branch restriction, user_id, source_id, data segregation

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `user_id` | `smallint` | âœ“ | Foreign key to PMUser — the user being granted branch access. |  |
| `source_id` | `int` | âœ“ | Foreign key to Source — the branch the user is permitted to access. |  |
| `pmuser_source_id` | `int` |  | Surrogate key for the user-source link record. |  |
| `UniqueId` | `varchar` |  | Unique identifier for synchronisation purposes. |  |
| `ScreenHierarchy` | `varchar` |  | Screen navigation context. |  |

*Foreign Keys*:
- `PMUser_Source.user_id` â†’ `PMUser.user_id` (Many-to-One)
- `PMUser_Source.source_id` â†’ `Source.source_id` (Many-to-One)

#### `User_Authorities`
**Domain: Admin**  **Owner: IT Department**  **Refresh: Admin**

Stores each user's financial and functional authority limits. One row per user. Covers write-off limits, payment amounts, refund/transfer authority, policy/claim edit rights, commission editing, instalment management, reinsurance visibility, debug access and many other per-user permission flags. The core user permissions table beyond group-level access.

*Keywords*: user authorities, write-off limit, payment authority, refund authority, policy edit, claim edit, commission edit, instalment, reinsurance, debug, user_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `user_id` | `smallint` | âœ“ | Primary key and foreign key to PMUser — one row per user. |  |
| `has_write_off_authority` | `tinyint` |  | Whether the user can write off outstanding balances (1=yes). | 0,1 |
| `write_off_amount` | `numeric` |  | Maximum amount this user can write off in a single transaction. |  |
| `has_unrestricted_enquiry` | `tinyint` |  | Whether the user can view all records without branch restriction. | 0,1 |
| `has_unrestricted_update` | `tinyint` |  | Whether the user can edit all records without branch restriction. | 0,1 |
| `has_payments_authority` | `tinyint` |  | Whether the user can process payments. | 0,1 |
| `payments_amount` | `numeric` |  | Maximum payment amount this user can process. |  |
| `has_claim_payments_authority` | `tinyint` |  | Whether the user can authorise claim payments. | 0,1 |
| `claim_payments_amount` | `numeric` |  | Maximum claim payment amount this user can authorise. |  |
| `has_refund_authority` | `tinyint` |  | Whether the user can process refunds. | 0,1 |
| `has_transfer_authority` | `tinyint` |  | Whether the user can transfer funds between accounts. | 0,1 |
| `is_edit_policy` | `smallint` |  | Whether the user can edit policies (1=yes). | 0,1 |
| `is_edit_claim` | `smallint` |  | Whether the user can edit claims (1=yes). | 0,1 |
| `is_raise_debit` | `smallint` |  | Whether the user can raise debit transactions. | 0,1 |
| `is_raise_credit` | `smallint` |  | Whether the user can raise credit transactions. | 0,1 |
| `is_reverse_transactions` | `smallint` |  | Whether the user can reverse transactions. | 0,1 |
| `can_override_posting_period` | `tinyint` |  | Whether the user can post to a closed accounting period. | 0,1 |
| `display_reinsurance` | `tinyint` |  | Whether the user can see reinsurance data. | 0,1 |
| `Edit_Default_Commission` | `tinyint` |  | Whether the user can edit the default commission rate on a policy. | 0,1 |
| `fee_discount` | `numeric` |  | Maximum % discount the user can apply to fees. | 0.00 |
| `can_user_debug_dynamic_logic_scripts` | `tinyint` |  | Whether the user can debug dynamic logic scripts (developer access). | 0,1 |

*Foreign Keys*:
- `User_Authorities.user_id` â†’ `PMUser.user_id` (One-to-One)
- `User_Authorities.write_off_currency_id` â†’ `Currency.currency_id` (Many-to-One)
- `User_Authorities.payments_currency_id` â†’ `Currency.currency_id` (Many-to-One)
- `User_Authorities.claims_payments_currency_id` â†’ `Currency.currency_id` (Many-to-One)
- `User_Authorities.paynow_bankaccount_id` â†’ `BankAccount.bankaccount_id` (Many-to-One)

---

## System Administration

**Tables in this module**: 26

### Query Rules

**Active Chase Cycle Items** â€” `Chase_cycle_item WHERE completed_date IS NULL  -- open items awaiting action`  
*Chase_cycle_item holds individual follow-up items generated for a policy as part of a chase cycle workflow (e.g. chasing outstanding premium). Items that have not been completed are the active workload. Filter on Chase_cycle_item where there is no completion date or the status is open.*

**Background Jobs Pending or Failed** â€” `Background_Job WHERE is_active = 1  -- or filter on last_run_result indicating failure`  
*Background_Job table holds scheduled background processing jobs. Jobs that are pending, scheduled, or have recorded a failure need attention. Filter on the job status / is_active fields to find jobs that have not completed successfully.*

### Tables

#### `Background_Job`
**Domain: Admin**  **Owner: IT Department**  **Refresh: Real-time**

Queue of asynchronous background jobs submitted by the application. Each job contains an XML payload (job_xml) describing the work to perform (e.g. document generation/archiving — jobtype=DOCUPACK), a status (C=Completed, Q=Queued, P=Processing, F=Failed), the submitting user, and timestamps for created/started/completed/expiry. Also stores the client code, policy reference and claim number the job relates to for traceability.

*Keywords*: background job, async job, document generation, archive, DOCUPACK, job queue, job status, job_xml, job_service, job_user_id, background_job_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `background_job_id` | `int` | âœ“ | Primary key for the background job. |  |
| `description` | `nvarchar` |  | Human-readable description of the job (e.g. Archive documents, Generate Schedule). | Archive documents,Generate Schedule |
| `job_status` | `nvarchar` |  | Current status of the job: Q=Queued, P=Processing, C=Completed, F=Failed. | Q,P,C,F |
| `job_service` | `nvarchar` |  | The server/service instance that processed this job. | DEMOWIN2012 |
| `job_xml` | `nvarchar` |  | XML payload describing the job parameters (job type, document templates, policy/party references, output format etc.). |  |
| `job_created` | `datetime` |  | Date and time the job was submitted to the queue. |  |
| `job_when_to_start` | `datetime` |  | Earliest date/time the job should be picked up and processed. |  |
| `job_started` | `datetime` |  | Date and time the job was picked up and started processing. |  |
| `job_completed` | `datetime` |  | Date and time the job finished processing. |  |
| `job_expiry` | `datetime` |  | Date after which a completed/failed job can be purged from the queue. |  |
| `job_user_id` | `smallint` |  | Foreign key to PMUser — the user who submitted the job. |  |
| `failure_description` | `nvarchar` |  | Error message or description of failure if the job status is F (Failed). |  |
| `job_retry_count` | `int` |  | Number of times the job has been retried after failure. | 0,1,2 |
| `last_job_retry_time` | `datetime` |  | Timestamp of the last retry attempt. |  |
| `party_code` | `varchar` |  | Client/party code the job relates to (for traceability). | BFI1594,BFIPINDERL |
| `insurance_ref` | `varchar` |  | Policy reference number the job relates to (e.g. BFICOMC00553). | BFICOMC00553,BFIPOL1295 |
| `claim_number` | `varchar` |  | Claim number the job relates to (if applicable). |  |

*Foreign Keys*:
- `Background_Job.job_user_id` â†’ `PMUser.user_id` (Many-to-One)

#### `Event_Public_Text`
**Domain: Operations**  **Owner: Operations Department**  **Refresh: Real-time**

Public text lines associated with an event log entry

*Keywords*: Event, Public Text, Diary, Note

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `event_cnt` | `int` |  | FK to event_log |  |
| `event_public_text_id` | `int` |  | Unique record identifier (PK) |  |
| `text_line` | `varchar` |  | A single line of public text for the event |  |

*Foreign Keys*:
- `Event_Public_Text.event_cnt` â†’ `event_log (event_cnt)` (Many-to-One)

#### `Hidden_options`
**Domain: System Administration**  **Owner: IT**  **Refresh: Admin**

Stores hidden (internal/advanced) configuration flags per branch and option number, used to control underwriting and accounting behaviour that is not exposed through the standard UI.

*Keywords*: hidden options, configuration, branch, underwriting, accounting, flag

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `branch_id` | `smallint` |  | Branch identifier for the hidden option (0 = global). | 0,1 |
| `option_number` | `int` |  | Numeric code identifying the specific hidden option. | 1,12,32,60 |
| `Value` | `varchar` |  | Value for the hidden option flag or setting. | 0,1,U,Y,N |
| `UW_type` | `varchar` |  | Underwriting type qualifier for the option (e.g. U = underwriting mode). | U, |
| `Acc_Type` | `varchar` |  | Accounting type qualifier for the option. |  |

#### `PMUser_Group`
**Domain: Operations**  **Owner: IT Department**  **Refresh: Real-time**

User group definitions for role-based access control

*Keywords*: User Group, Permissions, Role, Security

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pmuser_group_id` | `int` |  | Unique user group identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code for the group |  |
| `description` | `varchar` |  | Description of the user group |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `is_sys_admin_group` | `tinyint` |  | Flag: this is the system admin group | 0,1 |
| `pmuser_group_id` | `int` | âœ“ | Primary key for the user group. |  |
| `code` | `char` |  | Short code for the group (e.g. SYSADMIN, NOMLDGR, SFORB). | SYSADMIN,NOMLDGR,SFORB |
| `description` | `varchar` |  | Full name of the user group (e.g. System Administrators, Nominal Ledger Team). | System Administrators,Nominal Ledger Team |
| `is_sys_admin_group` | `tinyint` |  | Whether this group has system administrator privileges (1=yes). | 0,1 |
| `caption_id` | `int` |  | Foreign key to caption resource. |  |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |
| `effective_date` | `datetime` |  | Date from which this group is effective. |  |

#### `PMWrk_Task`
**Domain: Operations**  **Owner: Operations Department**  **Refresh: Real-time**

Work manager task definitions (workflow tasks assigned to users/groups)

*Keywords*: Work Manager, Task, Workflow, Assignment

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pmwrk_task_id` | `int` |  | Unique task identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code for the task |  |
| `description` | `varchar` |  | Description of the task |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `is_system_task` | `tinyint` |  | Flag: system-defined task | 0,1 |
| `type_of_task` | `tinyint` |  | Task type code |  |
| `auto_delete_after_num_days` | `int` |  | Auto-delete after N days of completion |  |
| `is_view_only_task` | `tinyint` |  | Flag: read-only task | 0,1 |
| `pmwrk_task_category_id` | `int` |  | FK to PMWrk_Task_Category |  |

*Foreign Keys*:
- `PMWrk_Task.pmwrk_task_category_id` â†’ `PMWrk_Task_Category (pmwrk_task_category_id)` (Many-to-One)

#### `PMWrk_Task_Action_Type`
**Domain: Operations**  **Owner: IT Department**  **Refresh: Real-time**

Lookup: action types that can be recorded against a work manager task instance

*Keywords*: Work Manager, Action Type, Outcome, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pmwrk_task_action_type_id` | `int` |  | Unique action type identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code for the action type |  |
| `description` | `varchar` |  | Description of the action type |  |
| `Is_Deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `Effective_Date` | `datetime` |  | Date record became effective |  |
| `Due_Days` | `int` |  | Number of days until the action is due |  |
| `Document_Template_code` | `char` |  | Code of the document template to use for this action |  |
| `Outcome_not_editable` | `tinyint` |  | Flag: outcome cannot be changed once set | 0,1 |

#### `PMWrk_Task_Category`
**Domain: Operations**  **Owner: IT Department**  **Refresh: Real-time**

Lookup: work manager task category definitions grouping related task types

*Keywords*: Work Manager, Task Category, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pmwrk_task_category_id` | `int` |  | Unique category identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code for the category |  |
| `description` | `varchar` |  | Description of the category |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `licence_limit` | `int` |  | Maximum number of licensed tasks for this category |  |
| `licence_key` | `varchar` |  | Licence key for this category |  |
| `is_block_above_licence_limit` | `tinyint` |  | Flag: block creation above licence limit | 0,1 |
| `is_warn_above_licence_limit` | `tinyint` |  | Flag: warn when above licence limit | 0,1 |
| `warns_since_licence_upgrade` | `int` |  | Number of warnings issued since last licence upgrade |  |

#### `PMWrk_Task_Group`
**Domain: Operations**  **Owner: IT Department**  **Refresh: Real-time**

Work manager task group definitions — logical groupings of tasks shown in the Work Manager UI

*Keywords*: Work Manager, Task Group, Lookup

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pmwrk_task_group_id` | `int` |  | Unique task group identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code for the task group |  |
| `description` | `varchar` |  | Description of the task group |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `display_icon` | `int` |  | Icon identifier for display in the UI |  |

#### `PMWrk_Task_Group_Task`
**Domain: Operations**  **Owner: IT Department**  **Refresh: Real-time**

Links tasks to the task groups they belong to with display sequence

*Keywords*: Work Manager, Task Group, Task Link

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pmwrk_task_group_id` | `int` |  | FK to PMWrk_Task_Group |  |
| `pmwrk_task_id` | `int` |  | FK to PMWrk_Task |  |
| `display_sequence_num` | `int` |  | Display order within the group |  |

*Foreign Keys*:
- `PMWrk_Task_Group_Task.pmwrk_task_group_id` â†’ `PMWrk_Task_Group (pmwrk_task_group_id)` (Many-to-One)
- `PMWrk_Task_Group_Task.pmwrk_task_id` â†’ `PMWrk_Task (pmwrk_task_id)` (Many-to-One)

#### `PMWrk_Task_Inst_Key`
**Domain: Operations**  **Owner: IT Department**  **Refresh: Real-time**

Key-value pairs linked to a work manager task instance for contextual navigation

*Keywords*: Work Manager, Task Instance, Key, Navigation

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pmwrk_task_instance_cnt` | `int` |  | FK to PMWrk_Task_Instance |  |
| `pmnav_key_id` | `int` |  | FK to PMNav_Key (navigation key definition) |  |
| `key_value` | `varchar` |  | Value of the navigation key for this instance |  |

*Foreign Keys*:
- `PMWrk_Task_Inst_Key.pmwrk_task_instance_cnt` â†’ `PMWrk_Task_Instance (pmwrk_task_instance_cnt)` (Many-to-One)

#### `PMWrk_Task_Inst_Log`
**Domain: Operations**  **Owner: IT Department**  **Refresh: Real-time**

Audit log entries against a specific work manager task instance

*Keywords*: Work Manager, Task Instance, Log, Audit

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pmwrk_task_instance_cnt` | `int` |  | FK to PMWrk_Task_Instance |  |
| `date_created` | `datetime` |  | Date this log entry was created |  |
| `text` | `varchar` |  | Log entry text |  |
| `created_by_id` | `smallint` |  | FK to PMUser who created the log entry |  |

*Foreign Keys*:
- `PMWrk_Task_Inst_Log.pmwrk_task_instance_cnt` â†’ `PMWrk_Task_Instance (pmwrk_task_instance_cnt)` (Many-to-One)
- `PMWrk_Task_Inst_Log.created_by_id` â†’ `PMUser (user_id)` (Many-to-One)

#### `PMWrk_Task_Instance`
**Domain: Operations**  **Owner: IT Department**  **Refresh: Real-time**

An instance of a work manager task assigned to a user or group with status and due date

*Keywords*: Work Manager, Task Instance, Assignment, Due Date, Status, Workflow

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pmwrk_task_instance_cnt` | `int` |  | Unique task instance identifier (PK) |  |
| `pmwrk_task_id` | `int` |  | FK to PMWrk_Task (task definition) |  |
| `pmwrk_task_group_id` | `int` |  | FK to PMWrk_Task_Group |  |
| `description` | `varchar` |  | Description of this specific task instance |  |
| `customer` | `varchar` |  | Customer/party name associated with this task |  |
| `task_due_date` | `datetime` |  | Date the task is due for completion |  |
| `pmuser_group_id` | `int` |  | FK to PMUser_Group (assigned group) |  |
| `user_id` | `smallint` |  | FK to PMUser (assigned user) |  |
| `task_status` | `tinyint` |  | Status code of this task instance (e.g. open, complete) |  |
| `is_urgent` | `tinyint` |  | Flag: task is marked urgent | 0,1 |
| `date_created` | `datetime` |  | Date the task instance was created |  |
| `created_by_id` | `smallint` |  | FK to PMUser who created the task |  |
| `last_modified` | `datetime` |  | Date the task was last modified |  |
| `modified_by_id` | `smallint` |  | FK to PMUser who last modified the task |  |
| `is_visible` | `tinyint` |  | Flag: task is visible in the UI | 0,1 |
| `workflow_information` | `varchar` |  | Serialised workflow context for the task |  |
| `source_id` | `int` |  | FK to source/branch this task is associated with |  |
| `pmwrk_task_action_type_id` | `int` |  | FK to PMWrk_Task_Action_Type (action recorded) |  |
| `task_outcome_date` | `datetime` |  | Date the task outcome was recorded |  |
| `task_outcome_id` | `int` |  | FK to task outcome lookup |  |
| `Is_task_review` | `tinyint` |  | Flag: this is a review task | 0,1 |
| `Original_pmuser_group_id` | `int` |  | FK to PMUser_Group that originally owned the task |  |
| `party_cnt` | `int` |  | FK to Party associated with this task |  |
| `PMWrk_task_parent_instance_cnt` | `int` |  | FK to parent PMWrk_Task_Instance (for sub-tasks) |  |
| `external_Workflow_id` | `uniqueidentifier` |  | External workflow system GUID |  |
| `Is_External_WorkItem` | `int` |  | Flag: task is an external work item | 0,1 |
| `ExternalTask_Category_Id` | `int` |  | Category of the external task |  |

*Foreign Keys*:
- `PMWrk_Task_Instance.pmwrk_task_id` â†’ `PMWrk_Task (pmwrk_task_id)` (Many-to-One)
- `PMWrk_Task_Instance.pmwrk_task_group_id` â†’ `PMWrk_Task_Group (pmwrk_task_group_id)` (Many-to-One)
- `PMWrk_Task_Instance.pmuser_group_id` â†’ `PMUser_Group (pmuser_group_id)` (Many-to-One)
- `PMWrk_Task_Instance.Original_pmuser_group_id` â†’ `PMUser_Group (pmuser_group_id)` (Many-to-One)
- `PMWrk_Task_Instance.user_id` â†’ `PMUser (user_id)` (Many-to-One)
- `PMWrk_Task_Instance.created_by_id` â†’ `PMUser (user_id)` (Many-to-One)
- `PMWrk_Task_Instance.modified_by_id` â†’ `PMUser (user_id)` (Many-to-One)
- `PMWrk_Task_Instance.party_cnt` â†’ `Party (party_cnt)` (Many-to-One)
- `PMWrk_Task_Instance.PMWrk_task_parent_instance_cnt` â†’ `PMWrk_Task_Instance (pmwrk_task_instance_cnt)` (Many-to-One)

#### `PMWrk_Task_Instance_Temp`
**Domain: Operations**  **Owner: IT Department**  **Refresh: Real-time**

Temporary work manager task instances used during batch processing before promotion to live

*Keywords*: Work Manager, Task Instance, Temp, Batch

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pmwrk_task_instance_temp_cnt` | `int` |  | Unique temp instance identifier (PK) |  |
| `pmwrk_task_id` | `int` |  | FK to PMWrk_Task |  |
| `pmwrk_task_group_id` | `int` |  | FK to PMWrk_Task_Group |  |
| `description` | `varchar` |  | Description of this temp task instance |  |
| `customer` | `varchar` |  | Customer name associated with temp task |  |
| `task_due_date` | `datetime` |  | Due date for this temp task |  |
| `pmuser_group_id` | `int` |  | Assigned user group |  |
| `user_id` | `smallint` |  | Assigned user |  |
| `task_status` | `tinyint` |  | Status code |  |
| `is_urgent` | `tinyint` |  | Flag: urgent task | 0,1 |
| `date_created` | `datetime` |  | Date created |  |
| `created_by_id` | `smallint` |  | FK to PMUser who created |  |
| `last_modified` | `datetime` |  | Date last modified |  |
| `modified_by_id` | `smallint` |  | FK to PMUser who last modified |  |
| `is_visible` | `tinyint` |  | Flag: visible in UI | 0,1 |
| `workflow_information` | `varchar` |  | Serialised workflow context |  |

#### `PMWrk_User_Quick_Start`
**Domain: Operations**  **Owner: IT Department**  **Refresh: Real-time**

User-specific quick-start shortcuts to work manager task groups and tasks

*Keywords*: Work Manager, Quick Start, User, Shortcut

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pmwrk_task_group_id` | `int` |  | FK to PMWrk_Task_Group |  |
| `pmwrk_task_id` | `int` |  | FK to PMWrk_Task |  |
| `user_id` | `smallint` |  | FK to PMUser (owner of this quick start) |  |
| `display_sequence_num` | `int` |  | Display order for this quick start entry |  |

*Foreign Keys*:
- `PMWrk_User_Quick_Start.pmwrk_task_group_id` â†’ `PMWrk_Task_Group (pmwrk_task_group_id)` (Many-to-One)
- `PMWrk_User_Quick_Start.pmwrk_task_id` â†’ `PMWrk_Task (pmwrk_task_id)` (Many-to-One)
- `PMWrk_User_Quick_Start.user_id` â†’ `PMUser (user_id)` (Many-to-One)

#### `PMWrk_websites`
**Domain: Operations**  **Owner: IT Department**  **Refresh: Real-time**

External website links accessible from the Work Manager UI

*Keywords*: Work Manager, Website, URL, Link

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `pmwrk_websites_id` | `int` |  | Unique website link identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `varchar` |  | Short code for the website link |  |
| `description` | `varchar` |  | Description of the website link |  |
| `website_url` | `varchar` |  | URL of the website |  |
| `button_tooltip` | `varchar` |  | Tooltip text shown on the button in the UI |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `Work_Manager_Icon_Id` | `int` |  | FK to Work_Manager_Icon |  |

*Foreign Keys*:
- `PMWrk_websites.Work_Manager_Icon_Id` â†’ `Work_Manager_Icon (Work_Manager_Icon_Id)` (Many-to-One)

#### `Report`
**Domain: Operations**  **Owner: Operations Department**  **Refresh: Real-time**

Report definitions available in the system

*Keywords*: Report, Scheduler, Output

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `report_id` | `int` |  | Unique report identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code for the report |  |
| `description` | `varchar` |  | Description of the report |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `report_name` | `varchar` |  | Report file/template name |  |

#### `System_Options`
**Domain: System Administration**  **Owner: IT**  **Refresh: Admin**

Stores system-wide and branch-level configuration options as numbered key/value pairs, controlling behaviour across all modules (tax settings, renewal sequences, audit reports, etc.). Each row is keyed by branch and option number.

*Keywords*: system options, configuration, branch, settings, option number, value

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `branch_id` | `smallint` |  | Branch identifier — options can be configured per branch (0 = global). | 0,1,2 |
| `option_number` | `int` |  | Numeric code identifying the specific system option. | 1,2,3 |
| `value` | `varchar` |  | Configured value for the option (stored as string regardless of type). | 0,1,Y,N |
| `description` | `varchar` |  | Human-readable label describing what the option number controls. | IPT and other taxes:,Renewal list sequence: |
| `UserId` | `int` |  | User ID of the last person who modified this option, if tracked. |  |
| `UniqueId` | `varchar` |  | Unique identifier for this option row, used for synchronisation or import/export. |  |

#### `class_of_business`
**Domain: Underwriting**  **Owner: Underwriting Department**  **Refresh: Real-time**

Lookup defining the class of business (COB) for a rating section. Examples: Motor Private, Property Commercial, Employers Liability, Marine Hull. Used in tax calculations, RI model selection, and regulatory/FCA reporting.

*Keywords*: class of business, COB, motor, property, liability, marine, employers liability, reporting, tax, regulatory, FCA, class_of_business_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `class_of_business_id` | `int` |  | Class of Business Unique ID |  |
| `code` | `char` |  | Class_Of_Business Code |  |
| `description` | `varchar` |  | Class_Of_Business Description |  |

#### `commission_band`
**Domain: Underwriting**  **Owner: Underwriting Department**  **Refresh: Real-time**

Defines commission rate bands/tiers for product/agent combinations. Sets the applicable commission percentage tiers for a given product for an agent or group.

*Keywords*: commission band, commission rate, tier, product, agent, percentage, commission structure, slab

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `commission_band_id` | `int` |  | Commission Band Unique identifier |  |
| `code` | `char` |  | Commission Band Code |  |
| `description` | `varchar` |  | Commission Band Description |  |

#### `currency`
**Domain: Underwriting**  **Owner: Underwriting Department**  **Refresh: Real-time**

Lookup of currencies supported by the system. Each has an ISO code, description and symbol. Used across premiums, claims, payments, RI and foreign exchange calculations.

*Keywords*: currency, GBP, USD, EUR, ISO currency code, multi-currency, foreign currency, exchange rate, currency_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `currency_id` | `int` |  | Currency Unique ID |  |
| `code` | `char` |  | Currency Code |  |
| `description` | `varchar` |  | Currency Description |  |

#### `event_log`
**Domain: Operations**  **Owner: Operations Department**  **Refresh: Real-time**

Event log of all events raised in the system (correspondence, diary notes, documents, transaction events)

*Keywords*: Event, Audit, Diary, Correspondence, Log, Document, Policy, Claim

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `event_cnt` | `int` |  | Unique event log identifier (PK) |  |
| `party_cnt` | `int` |  | FK to Party — party the event relates to |  |
| `insurance_folder_cnt` | `int` |  | FK to Insurance_Folder |  |
| `insurance_file_cnt` | `int` |  | FK to Insurance_File (policy) |  |
| `claim_cnt` | `int` |  | FK to Claim |  |
| `document_cnt` | `int` |  | FK to Document generated by this event |  |
| `document_type_id` | `int` |  | FK to Document_Type |  |
| `event_type_id` | `int` |  | FK to event_type (classification) |  |
| `event_log_subject_id` | `int` |  | FK to event_log_subject (subject code) |  |
| `user_id` | `smallint` |  | FK to PMUser who raised the event |  |
| `event_date` | `datetime` |  | Date and time the event was raised |  |
| `description` | `varchar` |  | Full description/body text of the event |  |
| `short_description` | `varchar` |  | Short summary description of the event |  |
| `Is_Completed` | `smallint` |  | Flag: diary event has been completed | 0,1 |
| `Priority_Code` | `varchar` |  | Priority code for diary/task events |  |
| `report_type_id` | `int` |  | FK to Report type used in this event |  |
| `account_key` | `int` |  | FK to account (financial posting reference) |  |
| `transaction_export_folder_cnt` | `int` |  | FK to Transaction_Export_Folder |  |
| `campaign_id` | `int` |  | FK to campaign (if event is tied to a campaign) |  |
| `case_id` | `int` |  | FK to Case |  |
| `batch_id` | `int` |  | FK to Batch |  |
| `peril_id` | `int` |  | FK to peril (if event relates to a specific peril) |  |
| `fsa_complaint_folder_cnt` | `int` |  | FK to FSA_complaint_folder |  |
| `new_address_cnt` | `int` |  | FK to Address (new address for address-change events) |  |
| `old_address_cnt` | `int` |  | FK to Address (old address for address-change events) |  |
| `rtf_text` | `text` |  | Rich text body of the event (RTF format) |  |
| `is_manual_description` | `tinyint` |  | Flag: description was manually entered (not system-generated) | 0,1 |
| `document_library_reference` | `varchar` |  | External document library reference |  |
| `Document_Path` | `varchar` |  | File system path to the associated document |  |

*Foreign Keys*:
- `event_log.event_type_id` â†’ `event_type (event_type_id)` (Many-to-One)
- `event_log.event_log_subject_id` â†’ `event_log_subject (event_log_subject_id)` (Many-to-One)
- `event_log.insurance_file_cnt` â†’ `Insurance_File (insurance_file_cnt)` (Many-to-One)
- `event_log.insurance_folder_cnt` â†’ `Insurance_Folder (insurance_folder_cnt)` (Many-to-One)
- `event_log.claim_cnt` â†’ `Claim (Claim_id)` (Many-to-One)
- `event_log.batch_id` â†’ `Batch (batch_id)` (Many-to-One)
- `event_log.case_id` â†’ `Case (case_id)` (Many-to-One)
- `event_log.new_address_cnt` â†’ `Address (address_cnt)` (Many-to-One)
- `event_log.fsa_complaint_folder_cnt` â†’ `FSA_complaint_folder (FSA_complaint_folder_cnt)` (Many-to-One)

#### `event_log_subject`
**Domain: Operations**  **Owner: Operations Department**  **Refresh: Real-time**

Lookup: subject codes categorising the subject of an event log entry

*Keywords*: Event Log Subject, Lookup, Code

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `event_log_subject_id` | `int` |  | Unique subject identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code for the subject |  |
| `description` | `varchar` |  | Description of the subject |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |

#### `event_type`
**Domain: Operations**  **Owner: Operations Department**  **Refresh: Real-time**

Lookup: event type codes grouping different categories of system events

*Keywords*: Event Type, Lookup, Code

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `event_type_id` | `int` |  | Unique event type identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code for the event type |  |
| `description` | `varchar` |  | Description of the event type |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `event_type_group_id` | `int` |  | FK to event_type_group |  |

*Foreign Keys*:
- `event_type.event_type_group_id` â†’ `event_type_group (event_type_group_id)` (Many-to-One)

#### `event_type_group`
**Domain: Operations**  **Owner: Operations Department**  **Refresh: Real-time**

Lookup: groupings of event types (e.g. correspondence, diary, transaction)

*Keywords*: Event Type Group, Lookup, Classification

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `event_type_group_id` | `int` |  | Unique group identifier (PK) |  |
| `caption_id` | `int` |  | FK to PMCaption |  |
| `code` | `char` |  | Short code for the group |  |
| `description` | `varchar` |  | Description of the event type group |  |
| `is_deleted` | `tinyint` |  | Soft delete flag | 0,1 |
| `effective_date` | `datetime` |  | Date record became effective |  |
| `exclusion_level` | `smallint` |  | Exclusion level controlling visibility |  |

#### `numbering_scheme`
**Domain: Admin**  **Owner: IT Department**  **Refresh: Admin**

Defines automatic number generation schemes for policies, clients, quotes and other entities. Specifies the mask/format (e.g. PPPYYX99999999), next number, step, whether numbers can be reused, and the type of entity being numbered. Drives the policy/client/quote reference number generation.

*Keywords*: numbering scheme, auto number, policy number, client code, quote number, mask, next number, numbering_scheme_id, PCCODE, CCCODE

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `numbering_scheme_id` | `int` | âœ“ | Primary key for the numbering scheme. |  |
| `code` | `char` |  | Short code for the scheme (e.g. PCCODE=Personal Client Code, CCCODE=Corporate Client Code). | PCCODE,CCCODE,Quote |
| `description` | `varchar` |  | Description of what this numbering scheme generates. | Personal Client Code,Corporate Client Code,Quote Number |
| `numbering_scheme_type_id` | `int` |  | Foreign key to numbering_scheme_type — the category of entity being numbered. |  |
| `numbering_scheme` | `tinyint` |  | Numbering scheme subtype (1=Personal, 2=Corporate, 3=Group). | 1,2,3 |
| `mask_code` | `varchar` |  | Format mask for the generated number (e.g. PPPYYX99999999 where P=prefix, Y=year, 9=digit). | PPPYYX99999999,LLLLLLLLLLIII9999 |
| `fixed_code` | `varchar` |  | Fixed prefix prepended to the generated number (e.g. Q for quotes). | Q |
| `next_number` | `int` |  | The next number to be generated by this scheme. |  |
| `highest_number` | `int` |  | The maximum number allowed for this scheme before wrapping. |  |
| `step` | `int` |  | Increment step between generated numbers (usually 1). | 0,1 |
| `is_generated` | `tinyint` |  | Whether numbers are auto-generated (1) or manually entered (0). | 0,1 |
| `is_reuse_abandoned` | `tinyint` |  | Whether abandoned numbers can be reused. | 0,1 |
| `is_read_only` | `tinyint` |  | Whether users can manually override generated numbers. | 0,1 |
| `party_type_id` | `smallint` |  | Foreign key to Party_Type — the party type this scheme applies to (for client codes). |  |
| `date_last_generated` | `datetime` |  | Date the scheme last generated a number. |  |
| `is_reset_daily` | `smallint` |  | Whether the counter resets daily. | 0,1 |
| `Is_Reset_Number` | `tinyint` |  | Whether the number has been manually reset. | 0,1 |
| `is_deleted` | `tinyint` |  | Soft-delete flag. | 0,1 |

*Foreign Keys*:
- `numbering_scheme.numbering_scheme_type_id` â†’ `numbering_scheme_type.numbering_scheme_type_id` (Many-to-One)
- `numbering_scheme.party_type_id` â†’ `Party_Type.party_type_id` (Many-to-One)

#### `source`
**Domain: Underwriting**  **Owner: Underwriting Department**  **Refresh: Real-time**

Branch/source definition — a trading branch or office of the insurance company. Policies, users and financial transactions are associated with a source for business segmentation and reporting.

*Keywords*: source, branch, office, trading branch, company branch, branch code, division, cost centre, source_id

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `source_id` | `int` |  | Branch Unique ID |  |
| `code` | `char` |  | Branch Code |  |
| `description` | `varchar` |  | Branch Description |  |
| `base_currency_id` | `int` |  | Branch Base Currency |  |

---

## Core

**Tables in this module**: 3

### Tables

#### `Country`
**Domain: Reference Data**  **Owner: IT Department**  **Refresh: Static**

Reference data for all countries supported in the system. Stores country code, ISO code, telephone dialling code, the default currency for that country, and address format configuration (caption labels for address lines, whether the country uses state lookups, and postcode visibility rules). Used on party and risk address records.

*Keywords*: country, reference, address, currency, ISO, telephone, geography

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `country_id` | `smallint` | âœ“ | PK — unique identifier for a country. | 1 (UK), 2 (NI) |
| `code` | `char(10)` |  | Short code for the country. e.g. GBR, USA, IRL. | GBR, NI, USA |
| `description` | `varchar(255)` |  | Full country name. e.g. United Kingdom, Northern Ireland. | United Kingdom, Northern Ireland |
| `currency_id` | `smallint` |  | FK to Currency — the default currency for this country (e.g. GBP for UK). | 26 (GBP) |
| `iso_code` | `char(4)` |  | ISO 3166 alpha-3 country code. e.g. GBR, USA. | GBR, USA, IRL |
| `caption_id` | `int` |  | FK to PMCaption — multilingual display label for this country. |  |
| `telephone_code` | `varchar(10)` |  | International dialling code prefix. e.g. 44 for UK. | 44, 1, 353 |
| `is_deleted` | `tinyint` |  | 0 = active, 1 = soft-deleted. | 0, 1 |
| `effective_date` | `datetime` |  | Date from which this country record is effective. |  |
| `is_state_lookup` | `tinyint` |  | 1 = addresses for this country should use the State lookup table for county/state. 0 = free-text county entry. | 0, 1 |
| `postcode_visibility_id` | `smallint` |  | FK to Postcode_visibility — controls whether/how postcode is shown for this country. |  |

*Foreign Keys*:
- `Country.currency_id` â†’ `Currency` (Many-to-One)
- `Country.postcode_visibility_id` â†’ `Postcode_visibility` (Many-to-One)

#### `PMCaption`
**Domain: System Configuration**  **Owner: IT Department**  **Refresh: Static**

Multilingual caption/label lookup — maps caption_id to its text string in a given language. Used system-wide: every user-facing description, lookup code display label, and screen caption in the system references this table. 93,000+ rows covering all installed languages.

*Keywords*: caption, label, language, multilingual, text, display

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `caption_id` | `int` | âœ“ | Unique identifier for a caption/label. Referenced by almost every lookup table in the system via caption_id FK. | 1, 2, 5, 100 |
| `language_id` | `smallint` |  | FK to Language — the language this caption text is in. Allows captions to be stored in multiple languages. | 1 (English British), 2 (American English) |
| `caption` | `varchar(255)` |  | The display text for this caption in the related language. Used as the visible label throughout the UI. | English (British), American English, United Kingdom |

*Foreign Keys*:
- `PMCaption.language_id` â†’ `Language` (Many-to-One)

#### `State`
**Domain: Reference Data**  **Owner: IT Department**  **Refresh: Static**

State, county or region sub-divisions within a country. Linked to Country. Used on address records where the country has is_state_lookup=1 (e.g. US states, UK counties). Supports ISO codes for states.

*Keywords*: state, county, region, address, geography, sub-division

| Column | Type | PK | Description | Sample |
|---|---|---|---|---|
| `State_id` | `smallint` | âœ“ | PK — unique identifier for a state/county/region. | 1, 2, 3 |
| `code` | `varchar(10)` |  | Short code for this state/region. e.g. UK (West Midlands), NI (Greater London). | UK, NI, ENG |
| `description` | `varchar(50)` |  | Full name of the state, county or region. e.g. West Midlands, Greater London. | West Midlands, Greater London |
| `iso_code` | `varchar(4)` |  | ISO code for this state/region where applicable. |  |
| `caption_id` | `int` |  | FK to PMCaption — multilingual label for this state/region. |  |
| `country_id` | `smallint` |  | FK to Country — the country this state/region belongs to. | 1 (UK) |
| `is_deleted` | `tinyint` |  | 0 = active, 1 = soft-deleted. | 0, 1 |
| `effective_date` | `datetime` |  | Date from which this state record is effective. |  |

*Foreign Keys*:
- `State.country_id` â†’ `Country` (Many-to-One)

---

## Business Terms Glossary

**Total terms**: 101

### Underwriting

**Account Handler**
: The internal staff member (underwriter or account manager) responsible for managing a client or portfolio of policies. Linked to policies via handler table.
  *Related tables*: `handler, insurance_file, PMUser`
  *Formula/Condition*: `handler_id on insurance_file identifies the responsible underwriter`

**Batch Renewal**
: An automated process that selects policies due for renewal and generates renewal documentation in bulk. Configured via Batch_Renewal_Job (Selection or Invitation type) and tracked via Batch_Renewal_Job_Runs.
  *Related tables*: `Batch_Renewal_Job, Batch_Renewal_Job_Runs, Batch_Renewal_Job_Run_Insurance_Folder, Batch_Renewal_Job_Type`
  *Formula/Condition*: `Run records in Batch_Renewal_Job_Runs; per-policy results in Batch_Renewal_Job_Run_Insurance_Folder`

**Broker / Agent**
: An intermediary who arranges insurance on behalf of clients. Stored in Party with party_type = Broker/Agent. Commission is paid to brokers via the agent_commission and Fee_Amounts tables.
  *Related tables*: `party, Party_Type, agent_commission, Fee_Amounts`
  *Formula/Condition*: `Party_Type.code = 'BROKER' or 'AGENT'`

**Broker of Record**
: The current appointed broker authorised to act on behalf of the policyholder. Recorded as lead_agent_cnt on insurance_file.
  *Related tables*: `insurance_file, party, Party_Type`
  *Formula/Condition*: `insurance_file.lead_agent_cnt = party_cnt of the appointed broker`

**Coinsurance**
: An arrangement where two or more insurers share the risk of a single policy, each taking a proportion of the premium and liability. Controlled by Coinsurance_Treatment lookup.
  *Related tables*: `Coinsurance_Treatment, insurance_file`
  *Formula/Condition*: `Each coinsurer holds a proportion of the risk and premium`

**Commission**
: Amount paid to Agent for selling the Policy
  *Related tables*: `agent_commission, insurance_file`
  *Formula/Condition*: `All the Data from Agent Commisson table for the Broker`

**Cover Note**
: A temporary insurance document issued before the formal policy schedule is produced. Cover notes are tracked via Cover_Note_Sheet and Cover_Note_Book.
  *Related tables*: `Cover_Note_Sheet, Cover_Note_Book, insurance_file`
  *Formula/Condition*: `Cover_Note_Sheet links to insurance_file_cnt`

**Direct Policy**
: A policy placed directly by the insurer without a broker intermediary. Identified by the absence of a lead_agent_cnt or a direct-business flag. Controlled by include_direct_policies on Batch_Renewal_Job.
  *Related tables*: `insurance_file, party, Batch_Renewal_Job`
  *Formula/Condition*: `insurance_file WHERE lead_agent_cnt IS NULL or direct flag set`

**Endorsement**
: A change to the terms, conditions or coverage of a policy added via a policy amendment (MTA). Endorsements modify the original policy document. Represented as MTA transactions in the system.
  *Related tables*: `insurance_file, insurance_file_type, Transaction_Type`
  *Formula/Condition*: `insurance_file_type.code = 'MTA PERM'`

**Excess / Deductible**
: The amount a policyholder must pay themselves before the insurer pays a claim. The insurer pays (loss − excess). Stored as a GIS property on the risk.
  *Related tables*: `risk, GIS_Property, Reserve_type`
  *Formula/Condition*: `GIS property value for excess/deductible; Reserve_type Is_Excess = 1`

**Expiry Date**
: The date on which a policy ends. After this date the policy is no longer in force unless renewed. Stored as expiry_date on insurance_file.
  *Related tables*: `insurance_file`
  *Formula/Condition*: `insurance_file.expiry_date`

**GIS (Generic Information System)**
: The flexible data model framework used to define and store all risk-specific data (e.g. vehicle details, property details, driver details). GIS properties are the individual fields; GIS data models group them by risk section.
  *Related tables*: `GIS_Data_Model, GIS_Property, GIS_Screen, GIS_Object`
  *Formula/Condition*: `GIS data stored in GIS_Object or UDL_ tables; structure defined in GIS_Data_Model/GIS_Property`

**Inception Date**
: The date on which a policy starts. Also called the risk commencement date. Stored as inception_date on insurance_file. For renewals, the original inception is stored as inception_date_tpi.
  *Related tables*: `insurance_file`
  *Formula/Condition*: `insurance_file.inception_date`

**Insurance File**
: A single version of a policy at a point in time — NB, MTA, renewal etc. Each transaction creates a new insurance_file row. The live version is the current contract. PK: insurance_file_cnt.
  *Related tables*: `insurance_file, Insurance_File`
  *Formula/Condition*: `insurance_file_cnt identifies a specific policy version`

**Insurance Folder**
: The parent container for a client-policy relationship. An insurance folder holds all versions (NB, MTA, renewal) of a policy for one client and product. Also called a policy folder. PK: insurance_folder_cnt.
  *Related tables*: `insurance_folder, insurance_file, Insurance_Folder`
  *Formula/Condition*: `insurance_folder_cnt groups all insurance_file versions`

**Limit of Indemnity**
: The maximum amount payable by the insurer for any one claim. May differ from sum insured — e.g. liability policies have a limit of indemnity rather than a sum insured.
  *Related tables*: `risk, GIS_Property`
  *Formula/Condition*: `GIS property value for limit of indemnity on the risk`

**Live Policy**
: Once Quotaion is Live, it becomes a Policy which is a Contract between Insurer and Insured specifying terms
  *Related tables*: `insurance_file, insurance_file_type`
  *Formula/Condition*: `All the insurance file type = POLICY`

**MTA**
: Any changes requested in the terms or premium of the Policy
  *Related tables*: `insurance_file, insurance_file_type`
  *Formula/Condition*: `All the insurance file type = MTA PERM `

**Mid-Term Adjustment (MTA)**
: A permanent change to a policy during its term — e.g. adding a driver, changing an address, increasing a sum insured. Creates a new insurance_file version. Transaction type code = MTA PERM.
  *Related tables*: `insurance_file, insurance_file_type, Transaction_Type`
  *Formula/Condition*: `insurance_file_type.code = 'MTA PERM'`

**Mid-Term Cancellation (MTC)**
: Cancellation of a policy before its expiry date. Creates a new insurance_file version. Transaction type code = MTC.
  *Related tables*: `insurance_file, insurance_file_type, Transaction_Type`
  *Formula/Condition*: `insurance_file_type.code = 'MTA CANC'`

**New Business (NB)**
: The first inception of a policy for a client — a brand new policy, not a renewal. Transaction type code = NB.
  *Related tables*: `insurance_file, insurance_file_type, Transaction_Type`
  *Formula/Condition*: `Transaction_Type.code = NB`

**Party**
: Any person or organisation recorded in the system: client (personal or corporate), broker/agent, insurer, other party. The party table is the central entity for all counterparties. PK: party_cnt.
  *Related tables*: `party, Party, Party_Type`
  *Formula/Condition*: `party_cnt identifies any counterparty in the system`

**Peril**
: A specific insured event or risk type (e.g. fire, theft, accidental damage, death). Perils belong to peril groups and are rated in rating sections. PK: peril_type_id.
  *Related tables*: `Peril_Type, Peril_Group, Peril_Type_Usage, rating_section`
  *Formula/Condition*: `peril_type_id identifies a specific insured event`

**Persistency**
: The percentage of policies that remain in force over a given period — a measure of customer retention. High persistency means fewer lapses and cancellations.
  *Related tables*: `insurance_file`
  *Formula/Condition*: `(Policies still live at end of period / Policies live at start of period) × 100`

**Premium**
: The amount paid for an insurance policy coverage
  *Related tables*: `insurance_file, rating_section, peril`
  *Formula/Condition*: `Sum of all this_premium components`

**Product**
: Defines the type of insurance being sold (e.g. Motor, Property, Commercial). The Product table configures which risk types, perils, fees and commission structures are available.
  *Related tables*: `Product, product_id, insurance_file`
  *Formula/Condition*: `product_id on insurance_file identifies the product`

**Quotation**
: A Quotation for between Insurer and Insured specifying terms
  *Related tables*: `insurance_file, insurance_file_type`
  *Formula/Condition*: `All the insurance file type = QUOTE`

**Quote Conversion**
: The process of converting a quotation to a live policy (binding). The insurance_file_type changes from QUOTE to POLICY when the quote is accepted and bound.
  *Related tables*: `insurance_file, insurance_file_type`
  *Formula/Condition*: `insurance_file_type.code changes from 'QUOTE' to 'POLICY'`

**Rating Section**
: A section of risk data used to calculate a premium component. Each rating section maps to a GIS data model and holds the rates applied to the risk attributes. Multiple rating sections can exist per risk.
  *Related tables*: `rating_section, peril, risk`
  *Formula/Condition*: `Sum of rating_section.this_premium gives total risk premium`

**Reinstatement (MTR)**
: Reinstating a cancelled policy to live status — reversing a mid-term cancellation. Creates a new insurance_file version with transaction type MTR.
  *Related tables*: `insurance_file, insurance_file_type, Transaction_Type`
  *Formula/Condition*: `Transaction_Type.code = 'MTR'`

**Reinsurance**
: insurer transfers part of their policy risks to another company, the reinsurer
  *Related tables*: `ri_arrangement, ri_arrangement_line, risk, insurance_file`
  *Formula/Condition*: `All the Data from ri_arrangement_line for the Broker`

**Renewal**
: Policy is agained continued for another term
  *Related tables*: `insurance_file, insurance_file_type`
  *Formula/Condition*: `All the insurance file type = POLICY and Inception_date<>Inception_date_tpi`

**Risk**
: The individual insured object or subject within a policy (e.g. a vehicle, a building, a person). A policy can have multiple risks. Contains the GIS user-defined fields holding the risk details. PK: risk_cnt.
  *Related tables*: `risk, Risk, risk_folder, insurance_file`
  *Formula/Condition*: `risk_cnt identifies an individual risk on a policy`

**Risk Folder**
: The parent container for a risk across all policy versions. Groups all versions of the same risk together. PK: risk_folder_cnt.
  *Related tables*: `risk_folder, Risk_Folder, risk`
  *Formula/Condition*: `risk_folder_cnt groups all risk versions`

**Source / Branch**
: A business branch or office through which policies are written. The Source table identifies each branch. Users and policies are linked to branches for data segregation (PMUser_Source).
  *Related tables*: `Source, PMUser_Source, insurance_file`
  *Formula/Condition*: `source_id links policies to branches`

**Sum Insured**
: The maximum amount the insurer will pay on a claim — the face value of cover. Set on the risk via GIS properties. Different from limit of indemnity (which may be lower).
  *Related tables*: `risk, GIS_Property, GIS_Data_Model`
  *Formula/Condition*: `GIS property value for sum_insured on the relevant risk section`

**UDL (User Defined Logic)**
: User-defined data tables (prefixed UDL_) that store the actual risk attribute values for each risk version under a specific GIS data model. Each product/risk section has its own UDL table.
  *Related tables*: `GIS_Object, risk, GIS_Data_Model`
  *Formula/Condition*: `UDL_ tables joined via risk_cnt/insurance_file_cnt`

**Wording**
: Legal policy wording text — the terms and conditions of coverage. Wordings are linked to products (Wording_Product_Link) and risk types (Wording_Risk_Type_Link).
  *Related tables*: `Wording_Product_Link, Wording_Risk_Type_Link`
  *Formula/Condition*: `wording_id links wording to products and risk types`

### Finance

**Bordereaux**
: A detailed listing (schedule) of risks, premiums or claims produced for reinsurers or managing agents. Typically a periodic report showing individual policy or claim details for treaty accounting.
  *Related tables*: `insurance_file, transdetail, ri_arrangement_line, claim`
  *Formula/Condition*: `Detailed listing of policies or claims within a treaty period`

**Chase Cycle**
: An automated credit control workflow that chases a policyholder or broker for outstanding information or payments. Defined by Chase_cycle_rule with steps (Chase_Cycle_Step) and active items (Chase_cycle_item). Can auto-cancel policies if unresolved.
  *Related tables*: `Chase_cycle_rule, Chase_Cycle_Step, Chase_cycle_item`
  *Formula/Condition*: `Active chases in Chase_cycle_item WHERE is_deleted = 0`

**Claims Bordereau**
: A periodic report listing all claims under a reinsurance treaty — showing claim details, payments, reserves and RI recoveries. Sent to reinsurers for claims accounting.
  *Related tables*: `claim, ri_arrangement_line, transdetail`
  *Formula/Condition*: `List of claims with RI recovery per claim`

**Creditor**
: A party to whom the insurer owes money — e.g. a reinsurer entitled to a ceded premium or a policyholder due a refund.
  *Related tables*: `account, party, ri_arrangement_line`
  *Formula/Condition*: `Outstanding credit balance owed by the insurer`

**Debtor**
: A party that owes money to the insurer — typically a broker who has collected premium but not yet remitted it. Outstanding debtor balances are tracked via broker accounts.
  *Related tables*: `account, party, Party_Type, Debtor_User_Groups`
  *Formula/Condition*: `Outstanding debit balance on broker/client account`

**Earned Premium**
: The portion of written premium that has been "earned" — i.e. relates to the period that has already elapsed. Earned Premium = Written Premium − UPR.
  *Related tables*: `insurance_file, transdetail`
  *Formula/Condition*: `Gross Premium × (Days elapsed / Total policy days)`

**Earning Pattern**
: A non-linear schedule defining how premium is earned over the policy term — e.g. a 12-month policy might earn 25% in month 1, then equal amounts thereafter. Used where risk is not uniform over time. Configured in Earning_Pattern and Earning_Pattern_Usage.
  *Related tables*: `Earning_Pattern, Earning_Pattern_Usage, insurance_file`
  *Formula/Condition*: `Applied to gross premium to derive earned amount at any date`

**Manual Journal**
: A direct accounting entry created manually by a user to correct or adjust account balances — not generated by a policy transaction. Controlled by has_ManualJournal_authority in User_Authorities. Stored in ManualJournal/ManualJournalDetail.
  *Related tables*: `ManualJournal, ManualJournalDetail, User_Authorities`
  *Formula/Condition*: `Manual debit/credit entry to nominal accounts`

**Posting Period**
: The accounting period (month/year) to which a financial transaction is allocated. Controls which accounting period is affected by a posting. Can sometimes be overridden by users with the right authority (can_override_posting_period).
  *Related tables*: `document, transdetail, User_Authorities`
  *Formula/Condition*: `document.posting_date determines the period; controlled by User_Authorities.can_override_posting_period`

**Premium Bordereau**
: A periodic report listing all policies ceded under a reinsurance treaty — showing risk details, premium amounts and the reinsurer's share. Sent to reinsurers for accounting purposes.
  *Related tables*: `insurance_file, transdetail, ri_arrangement_line`
  *Formula/Condition*: `List of ceded policies with RI premium per line`

**Pro Rata Cancellation**
: A cancellation where the return premium is calculated proportionally to the unexpired period of the policy — the policyholder receives back the unearned premium.
  *Related tables*: `insurance_file, policy_fee_u`
  *Formula/Condition*: `Return premium = Gross Premium × (days remaining / total policy days)`

**Return Premium**
: A refund of premium to a policyholder — typically arising from a mid-term cancellation or reduction in risk. Generates a credit transaction. Also called "return" or "refund of premium".
  *Related tables*: `insurance_file, document, transdetail, Transaction_Type`
  *Formula/Condition*: `Transaction_Type.code in ('MTC','MTA CANC') generating a credit transdetail`

**Short Rate Cancellation**
: A cancellation where the return premium is less than the pro-rata amount — the insurer retains a penalty for early cancellation. Often used for policyholder-initiated cancellations.
  *Related tables*: `insurance_file`
  *Formula/Condition*: `Return premium < pro-rata amount; penalty retained by insurer`

**Unearned Premium Reserve (UPR)**
: The portion of written premium that relates to the unexpired period of a policy. The insurer has an obligation to provide cover for this period and must hold the UPR as a liability. Calculated pro-rata by default unless an earning pattern is defined.
  *Related tables*: `insurance_file, Earning_Pattern, Earning_Pattern_Usage`
  *Formula/Condition*: `(Days remaining / Total policy days) × Gross Premium`

### Financial

**Account Currency**
: Currency in which an Account needs to be operated
  *Related tables*: `currency, transdetail`
  *Formula/Condition*: `account_currency_id configured for a transaction`

**Account Type**
: What is the nature of an account. Whether it's Income, Expense, Asset, Liability
  *Related tables*: `accounttype, account`
  *Formula/Condition*: `accounttype_id configured for an account`

**Allocation**
: The matching (allocation) of a received cash payment against an outstanding debit document. Tracks which receipts settle which invoices. Stored in allocation/cashlistitem tables.
  *Related tables*: `cashlist, cashlistitem, account, document`
  *Formula/Condition*: `Allocated amount reduces outstanding balance on account`

**Base Currency**
: Main Currency used for a Branch
  *Related tables*: `currency, transdetail`
  *Formula/Condition*: `currency_id configured for a transaction`

**Cashlist**
: A batch of incoming cash receipts entered together for processing. A cashlist groups multiple cashlistitem payments and must be confirmed/approved before posting.
  *Related tables*: `cashlist, cashlistitem`
  *Formula/Condition*: `Sum of cashlistitem.amount per cashlist`

**Credit Note**
: A financial document issuing a refund or reduction — e.g. for a cancellation or return premium. Stored in the document (financial) table.
  *Related tables*: `document, transdetail, Document_Type`
  *Formula/Condition*: `document type = 'Credit Note'`

**Debit Note**
: A financial document raising a charge to a client or broker — typically for a premium, fee or adjustment. The document table (financial transactions) stores these with document_type = Debit.
  *Related tables*: `document, transdetail, Document_Type`
  *Formula/Condition*: `document type = 'Debit Note'`

**Exchange Rate**
: The rate used to convert a foreign currency amount into the base (reporting) currency. Stored in CurrencyRate with an effective_from date.
  *Related tables*: `CurrencyRate, Currency`
  *Formula/Condition*: `CurrencyRate.rate_against_base for a given currency_id and effective_from`

**Gross Premium**
: The total premium before deduction of commission or reinsurance. In transdetail, the GROSS detail type line holds the gross premium amount.
  *Related tables*: `transdetail, Transdetail_Type, rating_section`
  *Formula/Condition*: `transdetail WHERE transdetail_type = 'GROSS'`

**Instalment**
: A scheduled part-payment of a premium. Policies can be paid in instalments rather than a single payment. Controlled by instalment plan configuration and tracked via the instalment tables.
  *Related tables*: `insurance_file, Fee_Amounts, policy_fee_u`
  *Formula/Condition*: `include_fee_in_instalments / spread_fee_across_instalments flags on Fee_Amounts`

**Ledger**
: What kind of Account it is, Client,Agent, Commission, Nominal etc
  *Related tables*: `ledger, account`
  *Formula/Condition*: `ledger_id configured for an account`

**Loss Ratio**
: The ratio of incurred losses to earned premiums, expressed as percentage
  *Related tables*: `transdetail, document, account, insurance_file, party`
  *Formula/Condition*: `(total claims paid/total premiums earned)*100`

**Net Premium**
: The premium net of commission — i.e. the amount retained by the insurer. Calculated as Gross Premium minus Commission.
  *Related tables*: `transdetail, Transdetail_Type`
  *Formula/Condition*: `Gross Premium - Commission`

**Nominal Ledger**
: The general ledger recording all accounting entries (income, expense, asset, liability). Postings are made from policy transactions to nominal accounts via the transdetail/account structure.
  *Related tables*: `account, ledger, transdetail, LedgerType`
  *Formula/Condition*: `Account balances aggregated by ledger and account type`

**System Currency**
: Main Currency used for a System
  *Related tables*: `currency, transdetail`
  *Formula/Condition*: `system_currency_id configured for a transaction`

**Transaction**
: A financial posting on the system — debit, credit, fee, tax, commission etc. Each transaction has a type (NB, REN, MTA, MTC), amount, currency and links to a policy version. Stored in the document (financial) and transdetail tables.
  *Related tables*: `document, transdetail, Transaction_Type, Transdetail_Type`
  *Formula/Condition*: `Sum of transdetail amounts by type`

**Transaction Currency**
: Currency used for a Transaction
  *Related tables*: `currency, transdetail`
  *Formula/Condition*: `amount_currency_id configured for a transaction`

**Write-Off**
: The cancellation of an irrecoverable outstanding balance — e.g. a small unpaid amount. Controlled by user authority limits in User_Authorities (has_write_off_authority, write_off_amount).
  *Related tables*: `User_Authorities, account, document`
  *Formula/Condition*: `Controlled by User_Authorities.write_off_amount per user`

### Claims

**Catastrophe Code**
: A code assigned to group claims arising from the same catastrophe event (e.g. a storm, flood). Allows aggregate loss reporting by event. Stored in Catastrophe_Code table.
  *Related tables*: `Catastrophe_Code, claim`
  *Formula/Condition*: `claim.catastrophe_code_id groups claims by event`

**Claim Peril**
: The specific peril (event type) under which a claim is made — e.g. theft, fire, accidental damage. Each claim can have multiple perils with separate reserves and payments.
  *Related tables*: `claim, peril_type_reserve_type, Reserve_type, Peril_Type`
  *Formula/Condition*: `Join claim to peril_type through claim detail tables`

**FNOL (First Notification of Loss)**
: The first report of a loss event to the insurer — the point at which a claim is opened. The date_opened on the claim table records FNOL.
  *Related tables*: `claim`
  *Formula/Condition*: `claim.date_opened = FNOL date`

**IBNR (Incurred But Not Reported)**
: Estimated reserves for claims that have occurred but have not yet been reported to the insurer. An actuarial estimate added on top of known outstanding claims.
  *Related tables*: `Reserve_type, claim`
  *Formula/Condition*: `Actuarial estimate — not directly calculated from claim data alone`

**Loss Adjuster**
: An independent specialist appointed to investigate, assess and quantify a large or complex claim on behalf of the insurer. Appointed via the claims process.
  *Related tables*: `claim, Claim_Expert_Service`
  *Formula/Condition*: `Claim_Expert_Service records specialist appointments on claims`

**Recovery**
: Money recovered after paying a claim — e.g. via subrogation, salvage or third-party recovery. Categorised by Recovery_type (Salvage, Third Party, Subrogation etc.).
  *Related tables*: `Recovery_type, claim`
  *Formula/Condition*: `Sum of recovery amounts by Recovery_type for a claim`

**Reserve**
: An amount set aside to cover the estimated future cost of a claim. Reserves are categorised by Reserve_type (Excess, Indemnity, Fees, Medical etc.) and are recorded against a claim peril.
  *Related tables*: `Reserve_type, peril_type_reserve_type, claim`
  *Formula/Condition*: `Sum of reserve amounts by Reserve_type for a claim`

**Run-Off**
: Policies that are no longer being written (closed to new business) but still have outstanding claims or liabilities being managed. The insurer continues to administer existing claims.
  *Related tables*: `claim, insurance_file`
  *Formula/Condition*: `insurance_file WHERE expiry_date < today AND open claims exist`

**Salvage**
: The residual value of insured property recovered after a total loss claim. Reduces the net claims cost. Classified in Recovery_type as is_salvage = 1.
  *Related tables*: `Recovery_type, claim`
  *Formula/Condition*: `Recovery_type.is_salvage = 1`

**Subrogation**
: The right of the insurer to pursue a third party responsible for a loss after paying the claim. A type of recovery. Classified in Recovery_type as code=SUB.
  *Related tables*: `Recovery_type, claim`
  *Formula/Condition*: `Recovery_type.code = 'SUB'`

### Claim

**Claim**
: A request for payment for the Loss Suffered under a Policy
  *Related tables*: `claim`
  *Formula/Condition*: `All the Data from Claim table`

### Reinsurance

**Accumulation / CAT Exposure**
: The total insured value or potential loss concentrated in a geographic area or risk category. Used for catastrophe management. Tracked via the Accumulation and Accumulation_Values tables.
  *Related tables*: `Accumulation, Accumulation_Values, risk, Catastrophe_Code`
  *Formula/Condition*: `SUM(sum_insured) grouped by area/postcode/catastrophe zone`

**Cedant**
: The insurance company that cedes (transfers) risk to a reinsurer. In this system, the insurer (the company running this system) is the cedant.
  *Related tables*: `ri_arrangement`
  *Formula/Condition*: `The primary insurer in all ri_arrangement records`

**Facultative Reinsurance**
: Reinsurance arranged on a case-by-case basis for individual risks — the reinsurer can accept or decline each risk. Configured in ri_arrangement with type=Facultative.
  *Related tables*: `ri_arrangement, ri_arrangement_line`
  *Formula/Condition*: `ri_arrangement.type = 'Facultative'`

**Proportional RI**
: Reinsurance where the insurer cedes a fixed proportion of premium and losses to the reinsurer (quota share or surplus treaty). The reinsurer shares both premium income and claim costs proportionally.
  *Related tables*: `ri_arrangement, ri_arrangement_line, risk`
  *Formula/Condition*: `RI premium = cession % × gross premium; RI recovery = cession % × loss`

**Quota Share**
: A proportional reinsurance treaty where the cedant cedes a fixed percentage of all premiums and losses. e.g. 30% quota share means RI receives 30% of premium and pays 30% of any loss.
  *Related tables*: `ri_arrangement, ri_arrangement_line`
  *Formula/Condition*: `RI premium = cession% × gross premium; RI loss = cession% × loss`

**Stop Loss**
: A non-proportional reinsurance cover that protects the cedant against aggregate losses exceeding a defined percentage of premium income. Provides overall loss ratio protection.
  *Related tables*: `ri_arrangement`
  *Formula/Condition*: `RI pays: aggregate losses - retention% × premium, up to limit`

**Surplus Treaty**
: A proportional reinsurance arrangement where the cedant retains a fixed amount (retention line) and cedes the surplus above that, up to a maximum number of lines.
  *Related tables*: `ri_arrangement, ri_arrangement_line`
  *Formula/Condition*: `Cession = (Sum Insured − retention) / Sum Insured × premium`

**TX / XOL Arrangement**
: Excess of Loss reinsurance arrangement. The insurer retains losses up to a retention limit; the reinsurer covers losses above that up to a maximum. Claim_XOL_Arrangement tracks XOL recoveries at the claim level.
  *Related tables*: `ri_arrangement, ri_arrangement_line, Claim_XOL_Arrangement`
  *Formula/Condition*: `Reinsurer pays: min(loss, limit) - retention when loss > retention`

**Treaty Reinsurance**
: A standing reinsurance agreement covering all policies meeting defined criteria (product, class, territory). The reinsurer automatically accepts a share. Configured in ri_arrangement with type=Treaty.
  *Related tables*: `ri_arrangement, ri_arrangement_line`
  *Formula/Condition*: `ri_arrangement.type = 'Treaty'`

**Underwriting Year**
: The year in which a risk was underwritten, used for Lloyd's/London market and reinsurance aggregate reporting. Distinct from the policy inception year — policies can span multiple calendar years.
  *Related tables*: `Underwriting_Year, insurance_file`
  *Formula/Condition*: `underwriting_year_id on insurance_file or risk records`

### Operations

**Case**
: A general purpose container for grouping related activities, tasks or communications on a policy or client. The Case table tracks open and closed operational cases.
  *Related tables*: `Case, Case_Progress`
  *Formula/Condition*: `case_id groups related activities`

**Diary / Activity**
: A workflow task or reminder assigned to a user or user group for follow-up on a policy, claim or client. Managed through the PMWrk_Task and PMWrk_Task_Group workflow tables.
  *Related tables*: `PMWrk_Task_Group, PMUser_Group_Activity`
  *Formula/Condition*: `Tasks assigned via pmwrk_task_group_id to user groups`

**Document Template**
: A template used to generate policy documents (schedules, debit notes, renewal invites etc.). Defined in Document_Template with group and sub-group classification. Used by Background_Job for document generation.
  *Related tables*: `Document_Template, Document_Template_Sub_Group, Document_Template_Group, Background_Job`
  *Formula/Condition*: `document_template_id links templates to generation jobs`

### Admin

**Audit Trail**
: A log of all changes made to policies, financial transactions and system configuration. Provides a complete history for compliance and investigation purposes. Stored in audit_trail and related tables.
  *Related tables*: `Audit_trail_custom_fields, Audit_Trail_Modules, configuration_audit_details, AuditSet`
  *Formula/Condition*: `All change events logged with user, timestamp and before/after values`

**Authority Level**
: The maximum policy value or transaction amount a user is permitted to approve or underwrite. Assigned per user per product via PMUser_Authority_Level and controlled by rule sets (PMUser_Authority_Rule_Set_Link).
  *Related tables*: `PMUser_Authority_Level, PMUser_Authority_Rule_Set_Link, Authority_Level_Type, User_Authorities`
  *Formula/Condition*: `authority_level_type_id defines the tier; rule_set_id defines the limits`

**Background Job**
: An asynchronous task queued for processing by the application server — typically document generation (DOCUPACK jobs). Status: Q=Queued, P=Processing, C=Completed, F=Failed. Links to party/policy for traceability.
  *Related tables*: `Background_Job`
  *Formula/Condition*: `Background_Job WHERE job_status = 'Q' for pending jobs`

**Numbering Scheme**
: The configuration that controls automatic generation of policy numbers, client codes, quote numbers and other system references. Defines the format mask, prefix, next number and step. Stored in numbering_scheme.
  *Related tables*: `numbering_scheme, numbering_scheme_type, numbering_scheme_history`
  *Formula/Condition*: `numbering_scheme.mask_code + next_number generates the reference`

**PMCaption**
: The multilingual label/caption system. Every screen label, dropdown value and description in the system is stored as a caption with a caption_id. PMCaption holds the text for each caption_id.
  *Related tables*: `PMCaption, PMCaptionIDGen`
  *Formula/Condition*: `caption_id referenced throughout lookup tables to support multilingual UI`

### Compliance

**Complaint**
: A formal complaint raised by a policyholder. Classified by FSA_complaint_category and FSA_complaint_method. Tracked with resolution and FCA reporting requirements.
  *Related tables*: `FSA_complaint_category, FSA_complaint_method, FSA_complaint_actiontype`
  *Formula/Condition*: `Complaints data required for FCA Gabriel return`

**FSA / FCA Regulatory Reporting**
: Regulatory reporting required by the Financial Conduct Authority (FCA) — formerly FSA. Includes complaints data (FSA_complaint_category), insurer credit ratings, type of sale classification (FSA_Type_Of_Sale).
  *Related tables*: `FSA_complaint_category, FSA_Type_Of_Sale, FSA_InsurerStatus, FSA_InsurerCreditRating`
  *Formula/Condition*: `Aggregate counts and values as required by FCA returns`

---

## Business Metrics & KPIs

**Total metrics**: 70

### Underwriting

| Metric | Type | Formula | Tables | Freq | Threshold |
|---|---|---|---|---|---|
| **Average Premium per Policy** | Underwriting KPI | GWP / COUNT(live policies) | `insurance_file, transdetail` | Monthly | â€” |
| **Batch Renewal Run Success Rate** | Operational KPI | (COUNT succeeded / COUNT total in Batch_Renewal_Job_Run_Insurance_Folder for a râ€¦ | `Batch_Renewal_Job_Runs, Batch_Renewal_Job_Run_Insuranceâ€¦` | Per Run | â€” |
| **Cover Note Register** | Operational Report | SELECT cover_note ref, issue date, status, policy ref, client, broker FROM Coverâ€¦ | `Cover_Note_Sheet, Cover_Note_Book, Cover_Note_Sheet_Staâ€¦` | Monthly | â€” |
| **Lapse / Cancellation Rate** | Retention KPI | (COUNT cancelled policies / COUNT live policies at start of period) * 100 | `insurance_file, insurance_file_type` | Monthly | 10 |
| **New Business Count per Agent** | Broker KPI | COUNT(DISTINCT insurance_folder_cnt) per lead_agent_cnt WHERE NB in period | `insurance_file, party` | Monthly | â€” |
| **Persistency Rate** | Retention KPI | (Policies live at end of 12 months / Policies live at start) × 100 | `insurance_file` | Monthly | 80 |
| **Policies Due for Renewal (Next 30 Days)** | Pipeline KPI | COUNT(DISTINCT insurance_folder_cnt) WHERE expiry_date BETWEEN GETDATE() AND DATâ€¦ | `insurance_file` | Daily | â€” |
| **Policy Count by Branch** | Mix KPI | COUNT(DISTINCT insurance_folder_cnt) GROUP BY source_id (joined to Source.descriâ€¦ | `insurance_file, Source` | Monthly | â€” |
| **Policy Count — Live (In-Force)** | Volume KPI | COUNT(DISTINCT insurance_folder_cnt) WHERE insurance_file has max version per foâ€¦ | `insurance_file, insurance_file_type` | Daily | â€” |
| **Policy Count — New Business** | Volume KPI | COUNT(DISTINCT insurance_folder_cnt) WHERE insurance_file_type.code = 'POLICY' Aâ€¦ | `insurance_file, insurance_file_type` | Monthly | â€” |
| **Policy Count — Renewals** | Volume KPI | COUNT(DISTINCT insurance_folder_cnt) WHERE insurance_file_type.code = 'POLICY' Aâ€¦ | `insurance_file, insurance_file_type` | Monthly | â€” |
| **Policy Diary / Renewal Diary** | Operational Report | SELECT policy ref, client, product, expiry_date, premium, broker WHERE expiry_daâ€¦ | `insurance_file, party, Product, Source` | Daily | â€” |
| **Premium by Product** | Mix KPI | SUM(GWP) GROUP BY product_id (joined to Product.description) | `transdetail, insurance_file, Product` | Monthly | â€” |
| **Premium per Active Broker** | Broker KPI | GWP in period / COUNT(distinct lead_agent_cnt with ≥1 policy) | `insurance_file, transdetail, party` | Monthly | â€” |
| **Quote Pipeline Value** | Pipeline KPI | SUM(insurance_file rated premium) WHERE insurance_file_type = 'QUOTE' AND expiryâ€¦ | `insurance_file, insurance_file_type` | Daily | â€” |
| **Quote to Bind Conversion Rate** | Conversion KPI | (COUNT policies WHERE insurance_file_type changed from QUOTE to POLICY) / COUNT(â€¦ | `insurance_file, insurance_file_type` | Monthly | 40 |
| **Rate Change on Renewal** | Underwriting KPI | AVG((Renewal premium − Prior year premium) / Prior year premium × 100) for policâ€¦ | `insurance_file` | Monthly | â€” |
| **Renewal Invitations Sent** | Operational KPI | COUNT(Batch_Renewal_Job_Runs) WHERE job type = INV AND run_date IN period AND isâ€¦ | `Batch_Renewal_Job_Runs, Batch_Renewal_Job, Batch_Renewaâ€¦` | Monthly | â€” |
| **Renewal Rate** | Retention KPI | (Renewed policies in period / Policies due for renewal in period) * 100 | `insurance_file, insurance_file_type` | Monthly | 80 |

**Average Premium per Policy**
: The average gross written premium per in-force policy. Useful for benchmarking and rate monitoring.
  *Formula*: GWP / COUNT(live policies)
  *Tables*: `insurance_file, transdetail`

**Batch Renewal Run Success Rate**
: Percentage of policies processed by a batch renewal job run that completed without failure.
  *Formula*: (COUNT succeeded / COUNT total in Batch_Renewal_Job_Run_Insurance_Folder for a run) * 100
  *Tables*: `Batch_Renewal_Job_Runs, Batch_Renewal_Job_Run_Insurance_Folder`

**Cover Note Register**
: A register of all cover notes issued in a period, with their status (current, cancelled, converted to policy). Required for regulatory and audit purposes.
  *Formula*: SELECT cover_note ref, issue date, status, policy ref, client, broker FROM Cover_Note_Sheet JOIN Cover_Note_Book
  *Tables*: `Cover_Note_Sheet, Cover_Note_Book, Cover_Note_Sheet_Status`

**Lapse / Cancellation Rate**
: Percentage of policies that lapsed or were cancelled during the period.
  *Formula*: (COUNT cancelled policies / COUNT live policies at start of period) * 100
  *Tables*: `insurance_file, insurance_file_type`

**New Business Count per Agent**
: Number of new policies placed through each broker/agent in the period. Broker productivity ranking.
  *Formula*: COUNT(DISTINCT insurance_folder_cnt) per lead_agent_cnt WHERE NB in period
  *Tables*: `insurance_file, party`

**Persistency Rate**
: Percentage of policies that remain in force over a 12-month period. Key retention metric.
  *Formula*: (Policies live at end of 12 months / Policies live at start) × 100
  *Tables*: `insurance_file`

**Policies Due for Renewal (Next 30 Days)**
: Count of live policies with expiry date within the next 30 days — the renewal pipeline.
  *Formula*: COUNT(DISTINCT insurance_folder_cnt) WHERE expiry_date BETWEEN GETDATE() AND DATEADD(day, 30, GETDATE()) AND latest version status = 'LIVE'
  *Tables*: `insurance_file`

**Policy Count by Branch**
: Number of live policies broken down by branch/source. Shows regional or channel distribution of the book.
  *Formula*: COUNT(DISTINCT insurance_folder_cnt) GROUP BY source_id (joined to Source.description)
  *Tables*: `insurance_file, Source`

**Policy Count — Live (In-Force)**
: Number of currently in-force policies — highest version per folder where the status is live/active and expiry_date >= today.
  *Formula*: COUNT(DISTINCT insurance_folder_cnt) WHERE insurance_file has max version per folder AND insurance_file_status = 'LIVE' AND expiry_date >= GETDATE()
  *Tables*: `insurance_file, insurance_file_type`

**Policy Count — New Business**
: Number of new policies incepted in a period. Excludes renewals and MTAs.
  *Formula*: COUNT(DISTINCT insurance_folder_cnt) WHERE insurance_file_type.code = 'POLICY' AND inception_date = inception_date_tpi AND inception_date BETWEEN @start AND @end
  *Tables*: `insurance_file, insurance_file_type`

**Policy Count — Renewals**
: Number of policies renewed in a period. Renewals are live policies where inception_date differs from original inception date (inception_date_tpi).
  *Formula*: COUNT(DISTINCT insurance_folder_cnt) WHERE insurance_file_type.code = 'POLICY' AND inception_date <> inception_date_tpi AND inception_date BETWEEN @start AND @end
  *Tables*: `insurance_file, insurance_file_type`

**Policy Diary / Renewal Diary**
: List of all policies expiring in a future period (typically 30/60/90 days), used by underwriters to manage the renewal workbook.
  *Formula*: SELECT policy ref, client, product, expiry_date, premium, broker WHERE expiry_date BETWEEN today AND today+N days AND current status = 'LIVE'
  *Tables*: `insurance_file, party, Product, Source`

**Premium by Product**
: Gross written premium broken down by product — shows the business mix across insurance products.
  *Formula*: SUM(GWP) GROUP BY product_id (joined to Product.description)
  *Tables*: `transdetail, insurance_file, Product`

**Premium per Active Broker**
: Average GWP generated per active broker in the period. Measures broker portfolio profitability.
  *Formula*: GWP in period / COUNT(distinct lead_agent_cnt with ≥1 policy)
  *Tables*: `insurance_file, transdetail, party`

**Quote Pipeline Value**
: Total gross premium value of all open quotations not yet bound. Measures the potential new business in the pipeline.
  *Formula*: SUM(insurance_file rated premium) WHERE insurance_file_type = 'QUOTE' AND expiry_date >= today
  *Tables*: `insurance_file, insurance_file_type`

**Quote to Bind Conversion Rate**
: Percentage of quotations that are converted to live policies. Key new business effectiveness metric.
  *Formula*: (COUNT policies WHERE insurance_file_type changed from QUOTE to POLICY) / COUNT(all quotes issued) × 100 in period
  *Tables*: `insurance_file, insurance_file_type`

**Rate Change on Renewal**
: The average percentage change in premium at renewal versus the prior year. Positive = rate increase, negative = rate reduction.
  *Formula*: AVG((Renewal premium − Prior year premium) / Prior year premium × 100) for policies renewed in period
  *Tables*: `insurance_file`

**Renewal Invitations Sent**
: Number of renewal invitation documents generated and sent to policyholders in the period via batch renewal jobs.
  *Formula*: COUNT(Batch_Renewal_Job_Runs) WHERE job type = INV AND run_date IN period AND is_failed = 0
  *Tables*: `Batch_Renewal_Job_Runs, Batch_Renewal_Job, Batch_Renewal_Job_Type`

**Renewal Rate**
: Percentage of expiring policies that were successfully renewed. Key retention metric.
  *Formula*: (Renewed policies in period / Policies due for renewal in period) * 100
  *Tables*: `insurance_file, insurance_file_type`

### Finance

| Metric | Type | Formula | Tables | Freq | Threshold |
|---|---|---|---|---|---|
| **Account Balance** | Financial KPI | Sum of all outstanding_amount for Account | `transdetail, document, account` | Daily | 70 |
| **Account Balance on Given Date** | Financial KPI | Sum of all outstanding_amount for Account Where Accounting date<=Given Date | `transdetail, document, account` | Daily | 70 |
| **Active Chase Items** | Credit Control KPI | COUNT(*) FROM Chase_cycle_item WHERE is_deleted = 0 | `Chase_cycle_item` | Daily | â€” |
| **Average Broker Commission Rate** | Financial KPI | AVG(agent_commission.commission_value / gross_premium * 100) for the period | `agent_commission, insurance_file, party` | Monthly | â€” |
| **Broker Outstanding Balance** | Credit Control KPI | SUM(outstanding_amount) on broker accounts WHERE account type = broker/agent | `account, party, party_type, transdetail` | Daily | â€” |
| **Broker Statement Balance** | Financial KPI | SUM(debit transactions) − SUM(credit transactions) − SUM(allocated receipts) forâ€¦ | `account, transdetail, cashlistitem` | Daily | â€” |
| **Cancelled Policy** | Financial KPI | get the max(insurance_file_cnt) after grouping with Insurance_folder_cnt and theâ€¦ | `insurance_file, insurance_file_status` | Daily | 70 |
| **Cash Received in Period** | Financial KPI | SUM(cashlistitem.amount) WHERE cashlist confirmed in period | `cashlist, cashlistitem` | Daily | â€” |
| **Combined Ratio** | Financial KPI | (Claims Incurred + Operating Expenses) / Earned Premium × 100 | `transdetail, claim, insurance_file` | Monthly | 100 |
| **Debtors Over 90 Days** | Credit Control KPI | SUM(outstanding_amount) on broker accounts WHERE accounting_date < DATEADD(day,-â€¦ | `account, transdetail, party` | Daily | â€” |
| **Earned Premium** | Financial KPI | GWP in period + Opening UPR − Closing UPR | `insurance_file, transdetail` | Monthly | â€” |
| **Expense Ratio** | Financial KPI | Operating Expenses / Earned Premium × 100 | `transdetail, account, insurance_file` | Monthly | 30 |
| **Fee Income** | Financial KPI | SUM(policy_fee_u.base_fee_amount) for transactions posted in period | `policy_fee_u, Fee_Amounts, insurance_file` | Monthly | â€” |
| **Foreign Currency Exposure** | Risk KPI | SUM(transdetail.amount) WHERE currency_id <> base_currency_id, grouped by currenâ€¦ | `transdetail, Currency, CurrencyRate` | Monthly | â€” |
| **Gross Written Premium (GWP)** | Financial KPI | SUM(transdetail.amount) WHERE transdetail_type = 'GROSS' AND document posted in â€¦ | `transdetail, document, documenttype, Transdetail_Type` | Monthly | â€” |
| **Loss Ratio** | Financial KPI | (total claims paid/total premiums earned)*100 | `transdetail, document, account, insurance_file, party` | Monthly | 70 |
| **Net Written Premium (NWP)** | Financial KPI | GWP - SUM(ri_arrangement_line.ri_premium) for the period | `transdetail, ri_arrangement_line, document` | Monthly | â€” |
| **Overdue Chase Items** | Credit Control KPI | COUNT(*) FROM Chase_cycle_item WHERE is_deleted = 0 AND due_date < GETDATE() | `Chase_cycle_item` | Daily | â€” |
| **Policies at Risk of Auto-Cancellation** | Credit Control KPI | COUNT(*) FROM Chase_cycle_item WHERE will_auto_cancel = 1 AND is_deleted = 0 | `Chase_cycle_item` | Daily | â€” |
| **Policy Transactions** | Financial KPI | Get all the Policy Transactions from documenttype.code in ('SND','SEC','SED','SRâ€¦ | `transdetail, document, account, documenttype` | Daily | 70 |
| **Premium Bordereaux — Monthly** | Operational Report | SELECT per policy: policy ref, product, inception, expiry, sum insured, gross prâ€¦ | `insurance_file, transdetail, ri_arrangement_line, party` | Monthly | â€” |
| **Return Premium (Credit Transactions)** | Financial KPI | SUM(transdetail.amount) WHERE document generates credit and Transaction_Type IN â€¦ | `transdetail, document, Transaction_Type` | Monthly | â€” |
| **Total Outstaning Premium** | Financial KPI | Sum of all outstanding_amount for Client or Broker Account depending upon whetheâ€¦ | `transdetail, document, insurance_file, account` | Daily | 70 |
| **Unallocated Cash** | Credit Control KPI | SUM(cashlistitem.amount) WHERE allocation is incomplete | `cashlist, cashlistitem, account` | Daily | â€” |
| **Unearned Premium Reserve (UPR)** | Financial KPI | (Days remaining on policy / Total policy days) × Gross Premium, summed across alâ€¦ | `insurance_file, Earning_Pattern, Earning_Pattern_Usage` | Monthly | â€” |

**Account Balance**
: The Final Balance of any account
  *Formula*: Sum of all outstanding_amount for Account
  *Tables*: `transdetail, document, account`

**Account Balance on Given Date**
: The Final Balance of any account for a Given Date
  *Formula*: Sum of all outstanding_amount for Account Where Accounting date<=Given Date
  *Tables*: `transdetail, document, account`

**Active Chase Items**
: Number of policies currently in an active chase cycle (outstanding information or payment chases not yet resolved).
  *Formula*: COUNT(*) FROM Chase_cycle_item WHERE is_deleted = 0
  *Tables*: `Chase_cycle_item`

**Average Broker Commission Rate**
: Average commission percentage paid to brokers across policies in a period.
  *Formula*: AVG(agent_commission.commission_value / gross_premium * 100) for the period
  *Tables*: `agent_commission, insurance_file, party`

**Broker Outstanding Balance**
: Total amount owed by each broker — sum of unallocated debit transactions on broker accounts. Key credit control metric.
  *Formula*: SUM(outstanding_amount) on broker accounts WHERE account type = broker/agent
  *Tables*: `account, party, party_type, transdetail`

**Broker Statement Balance**
: Net balance on a specific broker account at a given date — sum of all unallocated debits minus unallocated credits.
  *Formula*: SUM(debit transactions) − SUM(credit transactions) − SUM(allocated receipts) for broker account
  *Tables*: `account, transdetail, cashlistitem`

**Cancelled Policy**
: The Latest Version of the Policy is cancelled or not
  *Formula*: get the max(insurance_file_cnt) after grouping with Insurance_folder_cnt and then checking if insurance_file_status='CAN'
  *Tables*: `insurance_file, insurance_file_status`

**Cash Received in Period**
: Total cash receipts posted to the system in the period — from cashlists and direct receipts.
  *Formula*: SUM(cashlistitem.amount) WHERE cashlist confirmed in period
  *Tables*: `cashlist, cashlistitem`

**Combined Ratio**
: The combined loss ratio and expense ratio — the key profitability metric for an insurer. A combined ratio below 100% means underwriting profit.
  *Formula*: (Claims Incurred + Operating Expenses) / Earned Premium × 100
  *Tables*: `transdetail, claim, insurance_file`

**Debtors Over 90 Days**
: Total outstanding premium owed by brokers/clients that is more than 90 days overdue. Key credit risk metric.
  *Formula*: SUM(outstanding_amount) on broker accounts WHERE accounting_date < DATEADD(day,-90,GETDATE())
  *Tables*: `account, transdetail, party`

**Earned Premium**
: Premium revenue recognised in the accounting period. Gross Written Premium minus movement in UPR.
  *Formula*: GWP in period + Opening UPR − Closing UPR
  *Tables*: `insurance_file, transdetail`

**Expense Ratio**
: Operating expenses (excluding claims) as a percentage of earned premium. Measures operational efficiency.
  *Formula*: Operating Expenses / Earned Premium × 100
  *Tables*: `transdetail, account, insurance_file`

**Fee Income**
: Total fee income collected in a period — sum of all policy fees charged.
  *Formula*: SUM(policy_fee_u.base_fee_amount) for transactions posted in period
  *Tables*: `policy_fee_u, Fee_Amounts, insurance_file`

**Foreign Currency Exposure**
: Total premium or outstanding balance denominated in non-base currencies. Measures FX risk.
  *Formula*: SUM(transdetail.amount) WHERE currency_id <> base_currency_id, grouped by currency_id
  *Tables*: `transdetail, Currency, CurrencyRate`

**Gross Written Premium (GWP)**
: Total gross premium written in a period across all live policies. The top-line revenue metric for an insurer.
  *Formula*: SUM(transdetail.amount) WHERE transdetail_type = 'GROSS' AND document posted in period AND documenttype.code IN ('SND','SEC','SED','SRD','SID')
  *Tables*: `transdetail, document, documenttype, Transdetail_Type`

**Loss Ratio**
: The ratio of incurred losses to earned premiums, expressed as percentage
  *Formula*: (total claims paid/total premiums earned)*100
  *Tables*: `transdetail, document, account, insurance_file, party`

**Net Written Premium (NWP)**
: Gross written premium minus reinsurance ceded premium. Represents the premium retained by the insurer after RI.
  *Formula*: GWP - SUM(ri_arrangement_line.ri_premium) for the period
  *Tables*: `transdetail, ri_arrangement_line, document`

**Overdue Chase Items**
: Number of active chase items where the due_date has passed and the chase is still open — indicating unresolved credit control actions.
  *Formula*: COUNT(*) FROM Chase_cycle_item WHERE is_deleted = 0 AND due_date < GETDATE()
  *Tables*: `Chase_cycle_item`

**Policies at Risk of Auto-Cancellation**
: Number of policies in a chase cycle where auto-cancellation is flagged as imminent (will_auto_cancel = 1).
  *Formula*: COUNT(*) FROM Chase_cycle_item WHERE will_auto_cancel = 1 AND is_deleted = 0
  *Tables*: `Chase_cycle_item`

**Policy Transactions**
: Get All Policy Transactions
  *Formula*: Get all the Policy Transactions from documenttype.code in ('SND','SEC','SED','SRD','SID')
  *Tables*: `transdetail, document, account, documenttype`

**Premium Bordereaux — Monthly**
: Detailed listing of all policies written in the period with risk details, gross premium and RI share — sent to reinsurers for treaty accounting.
  *Formula*: SELECT per policy: policy ref, product, inception, expiry, sum insured, gross premium, RI%, RI premium
  *Tables*: `insurance_file, transdetail, ri_arrangement_line, party`

**Return Premium (Credit Transactions)**
: Total premium returned to policyholders in a period — from cancellations and risk reductions.
  *Formula*: SUM(transdetail.amount) WHERE document generates credit and Transaction_Type IN ('MTC','MTR')
  *Tables*: `transdetail, document, Transaction_Type`

**Total Outstaning Premium**
: The outstanding premium amount for an insurance policy coverage
  *Formula*: Sum of all outstanding_amount for Client or Broker Account depending upon whether Policy has lead_agent_cnt attached or not
  *Tables*: `transdetail, document, insurance_file, account`

**Unallocated Cash**
: Cash received but not yet matched (allocated) to outstanding debit documents. Represents suspense items requiring reconciliation.
  *Formula*: SUM(cashlistitem.amount) WHERE allocation is incomplete
  *Tables*: `cashlist, cashlistitem, account`

**Unearned Premium Reserve (UPR)**
: The liability held for unexpired policy cover. A balance-sheet metric required for statutory accounts.
  *Formula*: (Days remaining on policy / Total policy days) × Gross Premium, summed across all live policies
  *Tables*: `insurance_file, Earning_Pattern, Earning_Pattern_Usage`

### Financial

| Metric | Type | Formula | Tables | Freq | Threshold |
|---|---|---|---|---|---|
| **Age Analysis** | Financial KPI | Account Outstanding Balance due from 0-30 days, 30-90 days, 90-180 days, 180-270â€¦ | `transdetail, document, documenttype, account, party, paâ€¦` | Daily | 70 |
| **Agent Performance** | Financial KPI | Sum(amount) from transdetail for broker's account for documenttype.code in ('SNDâ€¦ | `transdetail, document, documenttype, account, party, paâ€¦` | Daily | 70 |
| **Commission Report** | Financial KPI | Sum(commission_value) from Agent_commission for each agent in a policy cycle forâ€¦ | `insurance_file, agent_commission, party` | Daily | 70 |

**Age Analysis**
: Categorize receivables or payables based on the length of time they have remained outstanding
  *Formula*: Account Outstanding Balance due from 0-30 days, 30-90 days, 90-180 days, 180-270 days, 270+ days
  *Tables*: `transdetail, document, documenttype, account, party, party_type`

**Agent Performance**
: Total Premium Collected by each Agent for Live and Cancelled Policies
  *Formula*: Sum(amount) from transdetail for broker's account for documenttype.code in ('SND','SEC','SED','SRD','SID')
  *Tables*: `transdetail, document, documenttype, account, party, party_type`

**Commission Report**
: Commisson Earned by each Agent in a period
  *Formula*: Sum(commission_value) from Agent_commission for each agent in a policy cycle for Live Policy
  *Tables*: `insurance_file, agent_commission, party`

### Claims

| Metric | Type | Formula | Tables | Freq | Threshold |
|---|---|---|---|---|---|
| **Average Claim Cost** | Financial KPI | SUM(claim payments) / COUNT(settled claims) for the period | `claim, transdetail` | Monthly | â€” |
| **Average Days to Settle Claim** | Operational KPI | AVG(date_closed − date_opened) WHERE claim closed in period | `claim` | Monthly | 30 |
| **Catastrophe Loss Aggregate** | Risk KPI | SUM(claim payments + reserves) GROUP BY catastrophe_code_id JOIN Catastrophe_Codâ€¦ | `claim, Catastrophe_Code, transdetail` | Per Event | â€” |
| **Claims Bordereaux — Monthly** | Operational Report | SELECT per claim: claim ref, policy ref, FNOL date, cause, payments, reserve, RIâ€¦ | `claim, transdetail, ri_arrangement_line, Catastrophe_Coâ€¦` | Monthly | â€” |
| **Claims Closed in Period** | Volume KPI | COUNT(*) FROM claim WHERE date_closed BETWEEN @start AND @end | `claim` | Monthly | â€” |
| **Claims Frequency** | Underwriting KPI | (Number of claims opened in period / Number of in-force policies) * 100 | `claim, insurance_file` | Monthly | â€” |
| **Claims Incurred** | Financial KPI | SUM(claim payments in period) + SUM(closing reserves) - SUM(opening reserves) | `claim, Reserve_type, transdetail` | Monthly | â€” |
| **Claims Recovery Rate** | Financial KPI | (SUM(recovery amounts) / SUM(claim payments)) * 100 | `Recovery_type, claim` | Monthly | â€” |
| **Claims Reported in Period** | Volume KPI | COUNT(*) FROM claim WHERE date_opened BETWEEN @start AND @end | `claim` | Monthly | â€” |
| **Large Losses (Attritional vs Large)** | Underwriting KPI | SUM claims below £X (attritional) vs SUM claims above £X (large) | `claim, transdetail` | Monthly | â€” |
| **Open Claims Count** | Volume KPI | COUNT(*) FROM claim WHERE claim_status = 'Open' | `claim, Claim_Status` | Daily | â€” |
| **Outstanding Claims Reserve** | Financial KPI | SUM(reserve amounts) WHERE claim status = open, grouped by Reserve_type | `Reserve_type, peril_type_reserve_type, claim` | Daily | â€” |

**Average Claim Cost**
: Average cost per claim settled in the period. Useful for pricing and reserving.
  *Formula*: SUM(claim payments) / COUNT(settled claims) for the period
  *Tables*: `claim, transdetail`

**Average Days to Settle Claim**
: Average number of days from claim opening (FNOL) to claim closure. Measures claims handling speed.
  *Formula*: AVG(date_closed − date_opened) WHERE claim closed in period
  *Tables*: `claim`

**Catastrophe Loss Aggregate**
: Total claims incurred grouped by catastrophe event code. Used for cat management and XOL RI recovery tracking.
  *Formula*: SUM(claim payments + reserves) GROUP BY catastrophe_code_id JOIN Catastrophe_Code.description
  *Tables*: `claim, Catastrophe_Code, transdetail`

**Claims Bordereaux — Monthly**
: Detailed listing of all claims activity in the period — FNOL date, payments, reserves, RI recovery — sent to reinsurers for claims accounting.
  *Formula*: SELECT per claim: claim ref, policy ref, FNOL date, cause, payments, reserve, RI recovery
  *Tables*: `claim, transdetail, ri_arrangement_line, Catastrophe_Code`

**Claims Closed in Period**
: Number of claims settled and closed in the period.
  *Formula*: COUNT(*) FROM claim WHERE date_closed BETWEEN @start AND @end
  *Tables*: `claim`

**Claims Frequency**
: Number of claims per policy (or per 100 policies). Measures how often insured events occur.
  *Formula*: (Number of claims opened in period / Number of in-force policies) * 100
  *Tables*: `claim, insurance_file`

**Claims Incurred**
: Total value of claims incurred in a period — payments made plus movement in reserves. Primary claims cost metric.
  *Formula*: SUM(claim payments in period) + SUM(closing reserves) - SUM(opening reserves)
  *Tables*: `claim, Reserve_type, transdetail`

**Claims Recovery Rate**
: Percentage of claim costs recovered through salvage, subrogation or third-party recovery.
  *Formula*: (SUM(recovery amounts) / SUM(claim payments)) * 100
  *Tables*: `Recovery_type, claim`

**Claims Reported in Period**
: Number of new claims opened (FNOL) in the period.
  *Formula*: COUNT(*) FROM claim WHERE date_opened BETWEEN @start AND @end
  *Tables*: `claim`

**Large Losses (Attritional vs Large)**
: Split of total claims incurred between attritional (below threshold) and large claims (above threshold). Used for pricing and RI analysis.
  *Formula*: SUM claims below £X (attritional) vs SUM claims above £X (large)
  *Tables*: `claim, transdetail`

**Open Claims Count**
: Number of claims currently open (not yet settled or closed). Measures pipeline/workload.
  *Formula*: COUNT(*) FROM claim WHERE claim_status = 'Open'
  *Tables*: `claim, Claim_Status`

**Outstanding Claims Reserve**
: Total reserve held for open claims not yet settled. A balance sheet liability metric.
  *Formula*: SUM(reserve amounts) WHERE claim status = open, grouped by Reserve_type
  *Tables*: `Reserve_type, peril_type_reserve_type, claim`

### Reinsurance

| Metric | Type | Formula | Tables | Freq | Threshold |
|---|---|---|---|---|---|
| **Accumulation Exposure by Area** | Risk KPI | SUM(sum_insured GIS property) GROUP BY area/postcode | `Accumulation, Accumulation_Values, risk, GIS_Property` | Monthly | â€” |
| **Maximum Probable Loss (MPL) by Product** | Risk KPI | Based on Accumulation data and loss scenarios per product_id | `Accumulation, Accumulation_Values, Product` | Quarterly | â€” |
| **Premium by Underwriting Year** | Financial KPI | SUM(GWP) GROUP BY underwriting_year_id (joined to Underwriting_Year.description) | `transdetail, insurance_file, Underwriting_Year` | Monthly | â€” |
| **RI Cession Ratio** | Financial KPI | (SUM(ri_arrangement_line.ri_premium) / GWP) * 100 for the period | `ri_arrangement_line, transdetail` | Monthly | â€” |
| **RI Recovery Pending** | Financial KPI | SUM(ri_arrangement_line.ri_recovery_due − ri_recovery_received) WHERE recovery oâ€¦ | `ri_arrangement_line, claim` | Monthly | â€” |
| **RI Recovery Ratio** | Financial KPI | (SUM(RI claim recoveries) / SUM(gross claims incurred)) * 100 | `ri_arrangement_line, claim, transdetail` | Monthly | â€” |

**Accumulation Exposure by Area**
: Total sum insured concentrated in each geographic area or postcode band. Used for catastrophe scenario modeling.
  *Formula*: SUM(sum_insured GIS property) GROUP BY area/postcode
  *Tables*: `Accumulation, Accumulation_Values, risk, GIS_Property`

**Maximum Probable Loss (MPL) by Product**
: Estimated maximum loss for worst-case scenarios per product line, for RI purchasing decisions.
  *Formula*: Based on Accumulation data and loss scenarios per product_id
  *Tables*: `Accumulation, Accumulation_Values, Product`

**Premium by Underwriting Year**
: GWP allocated to each underwriting year — used for Lloyd's/London market year-of-account reporting.
  *Formula*: SUM(GWP) GROUP BY underwriting_year_id (joined to Underwriting_Year.description)
  *Tables*: `transdetail, insurance_file, Underwriting_Year`

**RI Cession Ratio**
: Percentage of gross premium ceded to reinsurers. Measures how much risk is passed to the RI market.
  *Formula*: (SUM(ri_arrangement_line.ri_premium) / GWP) * 100 for the period
  *Tables*: `ri_arrangement_line, transdetail`

**RI Recovery Pending**
: Total RI recoveries due but not yet received from reinsurers. A balance-sheet receivable.
  *Formula*: SUM(ri_arrangement_line.ri_recovery_due − ri_recovery_received) WHERE recovery outstanding
  *Tables*: `ri_arrangement_line, claim`

**RI Recovery Ratio**
: Proportion of gross claims recovered from reinsurers. Measures RI effectiveness.
  *Formula*: (SUM(RI claim recoveries) / SUM(gross claims incurred)) * 100
  *Tables*: `ri_arrangement_line, claim, transdetail`

### Compliance

| Metric | Type | Formula | Tables | Freq | Threshold |
|---|---|---|---|---|---|
| **Complaints Received in Period** | Regulatory KPI | COUNT(*) FROM complaint/claim WHERE complaint flag = true AND date_opened IN perâ€¦ | `FSA_complaint_category, FSA_complaint_method` | Monthly | â€” |
| **Complaints Resolved Within 8 Weeks** | Regulatory KPI | (COUNT resolved within 56 days / COUNT total closed complaints) × 100 | `FSA_complaint_category, FSA_complaint_actiontype` | Monthly | 95 |

**Complaints Received in Period**
: Number of formal FCA complaints opened in the period. Required for FCA Gabriel regulatory return.
  *Formula*: COUNT(*) FROM complaint/claim WHERE complaint flag = true AND date_opened IN period
  *Tables*: `FSA_complaint_category, FSA_complaint_method`

**Complaints Resolved Within 8 Weeks**
: Percentage of complaints resolved within the FCA-mandated 8-week timeframe.
  *Formula*: (COUNT resolved within 56 days / COUNT total closed complaints) × 100
  *Tables*: `FSA_complaint_category, FSA_complaint_actiontype`

### Admin

| Metric | Type | Formula | Tables | Freq | Threshold |
|---|---|---|---|---|---|
| **Audit Trail Report** | Compliance Report | SELECT change_date, user, table_changed, field_changed, old_value, new_value FROâ€¦ | `Audit_trail_custom_fields, configuration_audit_details,â€¦` | On Demand | â€” |
| **Background Job Failure Rate** | Operational KPI | (COUNT(job_status='F') / COUNT(*)) * 100 FROM Background_Job WHERE job_created Iâ€¦ | `Background_Job` | Daily | 5 |
| **Background Jobs Pending** | Operational KPI | COUNT(*) FROM Background_Job WHERE job_status = 'Q' | `Background_Job` | Real-time | 100 |

**Audit Trail Report**
: Full change history for a policy, claim or account — who changed what, when and from what value. Required for compliance investigations.
  *Formula*: SELECT change_date, user, table_changed, field_changed, old_value, new_value FROM audit trail tables for given entity
  *Tables*: `Audit_trail_custom_fields, configuration_audit_details, AuditSet`

**Background Job Failure Rate**
: Percentage of background jobs that failed in a period. Operational health metric.
  *Formula*: (COUNT(job_status='F') / COUNT(*)) * 100 FROM Background_Job WHERE job_created IN period
  *Tables*: `Background_Job`

**Background Jobs Pending**
: Number of background jobs currently queued and waiting to be processed. Indicates processing backlog.
  *Formula*: COUNT(*) FROM Background_Job WHERE job_status = 'Q'
  *Tables*: `Background_Job`

---

## SQL Query Templates

Verified SQL templates tested against the database.

### Policy Summary
**Category**: Basic Reporting  |  **Domain**: Underwriting  |  **Complexity**: Simple
**Example question**: *"show me all active policies"*

Get Basic policy information with client details

```sql
select p.shortname,p.name,insurance_ref,cover_start_date,expiry_date from insurance_file ifi join
party p on ifi.insured_cnt=p.party_cnt
WHERE insurance_file_status_id is null
```

### Policy Transactions
**Category**: Finance Reporting  |  **Domain**: Finance  |  **Complexity**: Medium
**Example question**: *"show me all policy transactions"*

Get All Policy Transactions

```sql
SELECT ifi.insurance_ref, d.document_ref,a.account_name,t.amount, t.outstanding_amount 
from transdetail t join document d on 
t.document_id = d.document_id 
join insurance_file ifi on ifi.insurance_file_cnt=d.insurance_file_cnt
join account a on t.account_id=a.account_id
```

### Policy Specific Version
**Category**: Basic Reporting  |  **Domain**: Underwriting  |  **Complexity**: Medium
**Example question**: *"show me all policy version for a single policy"*

Get All Policy Versions for a Policy POL12345

```sql
select p.shortname,p.name,insurance_ref,cover_start_date,expiry_date from insurance_file ifi join
party p on ifi.insured_cnt=p.party_cnt
WHERE Insurance_ref='POL12345'
```

### Policy latest version
**Category**: Basic Reporting  |  **Domain**: Underwriting  |  **Complexity**: Medium
**Example question**: *"show me latest version for a single policy"*

Get Latest Policy Version for Policy POL12345

```sql
select p.shortname,p.name,insurance_ref,cover_start_date,expiry_date from insurance_file ifi join
party p on ifi.insured_cnt=p.party_cnt
WHERE insurance_file_cnt = (select max(insurance_file)cnt) from insurance_file where Insurance_ref='POL12345')
```

### Policy Client Transactions
**Category**: Finance Reporting  |  **Domain**: Finance  |  **Complexity**: Medium
**Example question**: *"show me all client transactions for policies"*

Get All Policy Transactions for Client

```sql
SELECT ifi.insurance_ref, d.document_ref,a.account_name,t.amount, t.outstanding_amount 
from transdetail t join document d on 
t.document_id = d.document_id 
join insurance_file ifi on ifi.insurance_file_cnt=d.insurance_file_cnt
join account a on t.account_id=a.account_id
join insurance_file ifi on i.insured_cnt=a.account_key
```

### Policy Agent Transactions
**Category**: Finance Reporting  |  **Domain**: Finance  |  **Complexity**: Medium
**Example question**: *"show me all broker transactions for policies"*

Get All Policy Transactions for Agent/Broker

```sql
SELECT ifi.insurance_ref, d.document_ref,a.account_name,t.amount, t.outstanding_amount 
from transdetail t join document d on 
t.document_id = d.document_id 
join insurance_file ifi on ifi.insurance_file_cnt=d.insurance_file_cnt
join account a on t.account_id=a.account_id
join insurance_file ifi on i.lead_agent_cnt=a.account_key
```
