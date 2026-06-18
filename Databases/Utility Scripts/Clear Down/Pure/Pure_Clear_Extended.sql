-- *******************************************************************************************
-- Pure Cleardown Script
-- *******************************************************************************************/

-- Uncomment line below to get Transaction for testing (see last line of this file)
-- BEGIN TRANSACTION

SET NOCOUNT ON

PRINT '---START---'

-- *******************************************************************************************/
-- PERFORM CHECKS BEFORE RUNNING THE ACTUAL CLEARDOWN SCRIPT
-- *******************************************************************************************/

--Report if there are any personal/corporate/group clients incorrectly linked to PFSchemes.
--If there are then do NOT start the cleardown until they are manually fixed in the system.
IF (SELECT count(*) FROM Account 
    WHERE Account_key IN (SELECT Party_cnt from Party WHERE Party_type_id in (1, 2, 4))		
                          AND Account_id IN (SELECT suspense_account_id FROM PFScheme)) > 0 
BEGIN
   PRINT 'CLEARDOWN HAS NOT BEEN RUN SINCE INVALID ACCOUNT ENTRIES EXIST WHICH NEED TO BE MANUALLY FIXED.'
   PRINT 'THE FOLLOWING ACCOUNTS (WITH A PARTY TYPE OF EITHER Personal/Corporate/Group) ARE INCORRECTLY LINKED TO THE PFSCHEME TABLE'
   PRINT ' ' 
   SELECT * FROM Account 
    WHERE Account_key IN (SELECT Party_cnt from Party WHERE Party_type_id in (1, 2, 4))		
                          AND Account_id IN (SELECT suspense_account_id FROM PFScheme)

   RETURN
END


--Report if there are any personal/corporate/group clients incorrectly linked to BankAccounts.
--If there are then do NOT start the cleardown until they are manually fixed in the system.
IF (SELECT count(*) FROM Account 
    WHERE Account_key IN (SELECT Party_cnt from Party WHERE Party_type_id in (1, 2, 4))		
                          AND Account_id IN (SELECT account_id FROM BankAccount)) > 0 
BEGIN
   PRINT 'CLEARDOWN HAS NOT BEEN RUN SINCE INVALID ACCOUNT ENTRIES EXIST WHICH NEED TO BE MANUALLY FIXED.'
   PRINT 'THE FOLLOWING ACCOUNTS (WITH A PARTY TYPE OF EITHER Personal/Corporate/Group) ARE INCORRECTLY LINKED TO THE BANKACCOUNT TABLE'
   PRINT ' ' 
   SELECT * FROM Account 
    WHERE Account_key IN (SELECT Party_cnt from Party WHERE Party_type_id in (1, 2, 4))		
                          AND Account_id IN (SELECT account_id FROM BankAccount)

   RETURN
END

-- *******************************************************************************************/
-- DELETE ALL DATA FROM USER-DEFINED PRODUCTBUILDER TABLES
-- *******************************************************************************************/

PRINT 'DATAMODEL TABLES'
DECLARE @GisObjectID INT,
    @ParentObjectID INT,
    @TableName VARCHAR(70),
    @DeleteString VARCHAR(50),
    @GisDataModel VARCHAR(70)


DECLARE C_DataModel CURSOR FAST_FORWARD FOR
    SELECT code
    FROM GIS_data_model
    ORDER BY code DESC

OPEN C_DataModel
FETCH NEXT FROM C_DataModel INTO @GisDataModel
WHILE (@@FETCH_STATUS = 0)
BEGIN
IF EXISTS (SELECT * FROM sysobjects where name = RTRIM(@GisDataModel) + '_standard_wording')
    BEGIN 
        SET @DeleteString = 'DELETE ' + RTRIM(@GisDataModel) + '_standard_wording'
        Print '    > ' + RTRIM(@GisDataModel) + '_standard_wording'
        EXEC (@DeleteString)
    END

    IF EXISTS (SELECT * FROM sysobjects where name = RTRIM(@GisDataModel) + '_sum_insured')
    BEGIN 
        SET @DeleteString = 'DELETE ' + RTRIM(@GisDataModel) + '_sum_insured'
        Print '    > ' + RTRIM(@GisDataModel) + '_sum_insured'
        EXEC (@DeleteString)
    END

    FETCH NEXT FROM C_DataModel INTO @GisDataModel
END
CLOSE C_DataModel
DEALLOCATE C_DataModel


DECLARE C_Objects CURSOR FAST_FORWARD FOR
    SELECT gis_object_id, parent_object_id, table_name 
    FROM GIS_Object
    ORDER BY parent_object_id DESC


OPEN C_Objects
FETCH NEXT FROM C_Objects INTO @GisObjectID, @ParentObjectID, @TableName
WHILE (@@FETCH_STATUS = 0)
BEGIN
    SET @DeleteString = 'DELETE ' + @TableName    
    Print '    > ' + @TableName
    EXEC (@DeleteString)

    FETCH NEXT FROM C_Objects INTO @GisObjectID, @ParentObjectID, @TableName
END
CLOSE C_Objects
DEALLOCATE C_Objects


-- *******************************************************************************************/
-- GIS SCHEME/POLICY Tables
-- *******************************************************************************************/

PRINT 'GIS SCHEME/POLICY'

IF EXISTS (SELECT * FROM sysobjects where name = 'PVD_Results')
BEGIN
	DELETE PVD_Results
END

DELETE GIS_Scheme_audit
DELETE GIS_Policy_Schemes_Sel
DELETE GIS_Policy_Link


-- *******************************************************************************************/
-- EVENTS
-- Delete all tables which are prefixed with Event_ except for Event_Type, Event_Type_Group
-- RG v1.5 - Also keep Event_Log_Subject
-- *******************************************************************************************/


PRINT 'EVENT'

DECLARE C_Events CURSOR FAST_FORWARD FOR
    SELECT name 
    FROM sysobjects 
    WHERE name LIKE 'Event_%'
    AND xtype = 'U'
    AND name not like 'Event_Type%'
    AND name not like 'Event_Log_Subject'	--v1.5

    
OPEN C_Events
FETCH NEXT FROM C_Events INTO @TableName
WHILE (@@FETCH_STATUS = 0)
BEGIN
    SET @DeleteString = 'DELETE ' + @TableName
    Print '    > ' + RTRIM(@TableName) 
    EXEC (@DeleteString)

    FETCH NEXT FROM C_Events INTO @TableName
END
CLOSE C_Events
DEALLOCATE C_Events

DELETE  Sub_Event
DELETE  Export_Party
DELETE  Farm
DELETE  ins_file_extra_value
DELETE  Ins_file_tax_band
DELETE  Ins_file_tax_value
DELETE	Credit_Control_Item
DELETE	Accumulation
DELETE  Background_Job
DELETE  Lead_Commission
DELETE  Main_Event
DELETE  Marine
DELETE  MTA
DELETE  MTA_Text
DELETE  Offices
DELETE  personal_accident
DELETE  policy_numbers
DELETE  Renewal_Stats
DELETE  Last_Print_Run
DELETE  Renewal_Status
DELETE  Risk_Extra_Value

DELETE  private_motor
DELETE  private_public_hire
DELETE  property_owners

DELETE  household_buildings
DELETE  household_contents

DELETE  report_audit_debit_table1
DELETE  report_audit_debit_table2
DELETE  report_audit_debit_table3
DELETE  report_audit_debit_table6

DELETE  ReportPartyList
DELETE  ReportPolicyList

PRINT 'CLAIMS REINSURANCE'
DELETE claim_ri_arrangement_line
DELETE claim_ri_arrangement
DELETE Claim_XOL_Arrangement

PRINT 'TAX'
DELETE  Tax_Calculation

PRINT 'RISK/PERIL'
DELETE  Peril_party
DELETE  Peril
DELETE  Rating_section

DELETE  RI_Arrangement_Line
DELETE  RI_Arrangement
DELETE  insurance_file_risk_link
DELETE  insurance_file_persistent_risk_link
DELETE	Insurance_File_Payment_Details
DELETE  Risk_History
DELETE  Risk_Locator
DELETE  Risk_Private_Text
DELETE  Risk_Public_Text
DELETE  Risk_Tax_Band
DELETE  Third_Party_Interest
DELETE  user_defined_risk_data

DELETE  Accumulation_class
DELETE  Accumulation_limit
DELETE  Accumulation_values
DELETE  Risk
DELETE  Risk_folder
DELETE  Party_Address_Risk_Link

PRINT 'STATS & TRANS'
DELETE Stats_Detail
DELETE Stats_folder
DELETE Transaction_Export_Detail
DELETE Transaction_Export_Folder
DELETE Transaction_Export_Complete

PRINT 'ACCOUNTS'

DELETE Payment_Approval
DELETE Cheque
DELETE TransMatch
DELETE TransDetailEx
DELETE TransDetail

DELETE Claim_Payment_Item
DELETE Claim_Payment
DELETE Claim_Receipt_Item
DELETE Claim_Receipt

DELETE Document
DELETE AllocationDetail
DELETE Allocation
DELETE CashListItem_Instalments 
DELETE CashListItem
DELETE CashList

-- Delete all accounts (and structures) which join to parties of type 1,2,4
UPDATE Insurance_File SET intermediary_agent_account_id=NULL
DELETE Party_Bank_History 
DELETE Party_Bank 

DELETE StructureTree where account_id IN (
	SELECT Account_id FROM Account JOIN Party on Party.party_cnt = Account.account_key
	WHERE Party.Party_type_id in (1, 2, 3, 4, 6, 14, 15, 16, 17, 19, 20, 22, 23, 24) )
DELETE ElementExtras where element_id NOT IN (SELECT Element_id FROM StructureTree where Element_id is not null)
DELETE Element where element_id NOT IN (SELECT Element_id FROM StructureTree where Element_id is not null)
DELETE Account WHERE Account_key in (SELECT Party_cnt from Party WHERE Party_type_id in (1, 2, 3, 4, 6, 14, 15, 16, 17, 19, 20, 22, 23, 24))
DELETE ACTUnique_Document_Number
DELETE ACTNumber

-- Additional Accounts tables
DELETE ACTNumber_Pool
DELETE AuditSet
DELETE InsurerPayment
DELETE invoice
DELETE invoice_item
DELETE MatchGroup
DELETE Statement
DELETE StatementDetail
DELETE Transaction_Report_Detail
DELETE Transaction_Report_Document
DELETE Suspended_Accounts_Transactions

DELETE  Policy_relationship
DELETE  policy_sections
DELETE  prospect_policy
DELETE  Shop
DELETE  Sub_Commission_Band
DELETE  Sub_Commission_Party
DELETE  Sub_Commission_Value
DELETE  Travel
DELETE  Vehicles
-- RG end

PRINT 'POLICY'
DELETE  Renewal_Control
DELETE  Renewal_Task_Log
DELETE  Policy_shared_premiums
DELETE  policy_coinsurers_section
DELETE  Policy_Narrative
DELETE  Policy_Agents
DELETE  Policy_Standard_Wording
DELETE  Policy_Coinsurers
DELETE  Agent_Commission
DELETE  Policy_Fee
DELETE  Policy_Fee_u
DELETE  Insurance_File_System
DELETE  insurance_file_agent
DELETE  Document_Spooler
DELETE  Text_file
DELETE  Event_Insurance_COB_Section
DELETE  Event_Insurance_File
DELETE  Event_Insurance_File_System
DELETE  Event_Insurance_Folder
DELETE  Insurance_COB_Section

DELETE  Ins_File_Private_Text
DELETE  Ins_File_Public_Text

DELETE  Cover_Note_Sheet
DELETE  Cover_Note_Book_Products
DELETE  Cover_Note_Book
DELETE  MTA_Insurance_File_Link
DELETE	Renewal_report
DELETE  Insurance_File

DELETE  Insurance_Folder
DELETE  Transaction_Export_Complete
DELETE  Transaction_Export_Detail
DELETE  Transaction_Export_Folder

-- RG additional v0.4 tables
DELETE  Client_Product_Link
DELETE  Party_locator
DELETE  Party_marketing_data

DELETE  prospect_campaign
DELETE  Accumulation_values
DELETE  aviation                 
DELETE  Coi_compulsory_value

DELETE  Coi_value
DELETE  Coi_arrangement
DELETE  Combined_liability
DELETE  Combined_motor
DELETE  Commercial_combined

DELETE  Historic_Payment_Reference_Only
DELETE  Historic_Receipt_Reference_Only

PRINT 'INSTALMENTS'
DELETE  PFInstalments_History
DELETE  PFInstalments
DELETE  PFMediaTypeHistory
DELETE  PFPremiumFinance
DELETE  PFTransaction_ID 
DELETE  PF_Accounts_Transactions

PRINT 'CLIENT'
-- Delete Personal, Group and Corporate clients only
DELETE Document_Spooler
DELETE Party_Conviction WHERE Party_cnt IN (SELECT Party_Cnt FROM Party WHERE party_type_id in (1, 2, 3, 4, 6, 14, 15, 16, 17, 19, 20, 22, 23, 24))
DELETE Party_Private_Text WHERE Party_cnt IN (SELECT Party_Cnt FROM Party WHERE party_type_id in (1, 2, 3, 4, 6, 14, 15, 16, 17, 19, 20, 22, 23, 24))
DELETE Party_Prospect WHERE Party_cnt IN (SELECT Party_Cnt FROM Party WHERE party_type_id in (1, 2, 3, 4, 6, 14, 15, 16, 17, 19, 20, 22, 23, 24))
DELETE Prospect_Policy WHERE Party_cnt IN (SELECT Party_Cnt FROM Party WHERE party_type_id in (1, 2, 3, 4, 6, 14, 15, 16, 17, 19, 20, 22, 23, 24))
DELETE Party_Other WHERE Party_cnt IN (SELECT Party_Cnt FROM Party WHERE party_type_id in (1, 2, 3, 4, 6, 14, 15, 16, 17, 19, 20, 22, 23, 24))
DELETE previous_accidents WHERE Party_cnt IN (SELECT Party_Cnt FROM Party WHERE party_type_id in (1, 2, 3, 4, 6, 14, 15, 16, 17, 19, 20, 22, 23, 24))
DELETE Party_Supplier_Business WHERE Party_cnt IN (SELECT Party_Cnt FROM Party WHERE party_type_id in (1, 2, 3, 4, 6, 14, 15, 16, 17, 19, 20, 22, 23, 24))
DELETE BG_Branch_Link
DELETE BG_Product_Link
DELETE Bank_Guarantee
DELETE CashDeposit_Branch_Link
DELETE CashDeposit_Policy_Link
DELETE CashDeposit_Product_Link
DELETE CashDepositNumber
DELETE CashDeposit
DELETE Party WHERE party_type_id in (1, 2, 3, 4, 6, 14, 15, 16, 17, 19, 20, 22, 23, 24)

DELETE Party_Corporate_Client
DELETE Party_Personal_Client
DELETE Party_Group_Client
DELETE Party_Net_Data
DELETE Party_Address_Usage WHERE Party_Cnt NOT IN (SELECT Party_Cnt FROM Party)
DELETE Party_Public_Text WHERE Party_Cnt NOT IN (SELECT Party_Cnt FROM Party)
DELETE Party_Relationship WHERE Party_Cnt NOT IN (SELECT Party_Cnt FROM Party)
DELETE party_lifestyle WHERE Party_Cnt NOT IN (SELECT Party_Cnt FROM Party)
DELETE Contact_Address_Usage WHERE Address_Cnt NOT IN (SELECT Address_Cnt FROM Party_Address_Usage)
DELETE Address WHERE Address_Cnt NOT IN (SELECT Address_Cnt FROM Party_Address_Usage)
DELETE Party_Contact_Usage WHERE Party_Cnt NOT IN (SELECT Party_Cnt FROM Party)
DELETE Contact WHERE Contact_Cnt NOT IN (SELECT Contact_Cnt FROM Party_Contact_Usage)

PRINT 'AGENTS'
DELETE Commission_Arrangement
DELETE Agent_Docs
DELETE Agent_Section_Rate
DELETE Party_Agent_Group

PRINT 'CLAIMS'
DELETE User_defined_peril_data
DELETE Reserve WHERE Claim_Peril_id In (SELECT Claim_Peril_Id FROM Claim_Peril)
DELETE Claim_User_Defined_Risk_Data WHERE Claim_id IN (SELECT Claim_Id FROM Claim)
DELETE Recovery

DELETE  Claim_address
DELETE  Claim_conviction
DELETE  Claim_party_claim
DELETE  Claim_private_text
DELETE  Claim_public_text
DELETE  Claim_user_defined_risk_data
DELETE  Party_claim
DELETE  Risk_Claim
DELETE  Summary_Stats_Agent
DELETE  Summary_Stats_Holder
DELETE  Summary_Stats_Premium
DELETE  Summary_Stats_Day
DELETE  Summary_Stats_Month

DELETE Claim_Party_Link
DELETE Claim_Party
DELETE Claim_Risk
DELETE Claim_Expert_Service
DELETE Claim_Peril
DELETE Claim_Comments
DELETE Claim_CoInsurers
DELETE Claim_Link
DELETE Claim
DELETE Claim_Folder
DELETE [Case]
DELETE Batch

PRINT 'WORKMANAGER'
DELETE PMWrk_Task_Inst_Key
DELETE PMWrk_Task_Inst_Log
DELETE PMWrk_Task_Instance
DELETE PMCategory_Message
DELETE PMMessage
DELETE PMCategory
DELETE PMNumber

DELETE DTLinks

DECLARE @iResult AS INT
EXEC @iResult=DDLExistsTable 'DOC_Folder' 
IF @iResult=1
BEGIN
	PRINT 'DOCUMASTER'
	DELETE DOC_annotation
	DELETE DOC_doc_info
	DELETE DOC_doc_keyword
	DELETE DOC_history
	DELETE DOC_page
	DELETE DOC_document
	DELETE DOC_folder
END

PRINT 'USERS'
DELETE FROM pmuser_group_user WHERE user_id not in (1,2)
DELETE FROM PMUser_Authority_Level WHERE user_id not in (1,2)
DELETE FROM PMWrk_User_Quick_Start WHERE user_id not in (1,2)
DELETE FROM PMNav_Batch_Key_Value WHERE pmnav_batch_set_id in 
(
SELECT pmnav_batch_set_id FROM PMNav_Batch_Set WHERE created_by_id not in (1,2)
)
DELETE FROM PMNav_Batch_Record WHERE pmnav_batch_set_id in 
(
SELECT pmnav_batch_set_id FROM PMNav_Batch_Set WHERE created_by_id not in (1,2)
)
DELETE FROM PMNav_Batch_Set WHERE created_by_id not in (1,2)
UPDATE document_template SET created_by_id =1,modified_by_id =1 
DELETE FROM PMLOCK WHERE locked_by_id not in (1,2) 
DELETE FROM pmuser WHERE user_id not in (1,2)

--- END OF DELETE SCRIPTS

PRINT 'RESET UNIQUE NUMBER TABLE'

DECLARE	@source_id int,
	@next_id int,
	@table_name varchar(30),
	@gone_in smallint,
	@entityid int

DECLARE	address_cursor CURSOR FOR
	SELECT	DISTINCT source_id
	FROM	address

OPEN	address_cursor

FETCH NEXT FROM address_cursor INTO @source_id

SET @gone_in = 0

WHILE @@FETCH_STATUS = 0
BEGIN

	SET @gone_in = 1

	SELECT	@next_id = MAX(address_id)
	FROM	address
	WHERE	source_id = @source_id

	IF @next_id IS NULL
		SELECT	@next_id = 0

	SELECT	@next_id = @next_id + 1

	SELECT	@table_name = 'Address_' + convert(varchar(10), @source_id)
	
	DELETE
	FROM	unique_number
	WHERE	table_name = @table_name

        INSERT INTO Unique_Number
        ( table_name, next_number )
        VALUES
        ( @table_name, @next_id )

	FETCH NEXT FROM address_cursor INTO @source_id

END

IF @gone_in = 0
BEGIN
	UPDATE Unique_Number 
	SET next_number=1
	WHERE table_name like 'Address_%'
END

CLOSE address_cursor
DEALLOCATE address_cursor

DECLARE	accumulation_cursor CURSOR FOR
	SELECT	DISTINCT accumulation_id
	FROM	accumulation

OPEN	accumulation_cursor

FETCH NEXT FROM accumulation_cursor INTO @source_id

SET @gone_in = 0

WHILE @@FETCH_STATUS = 0
BEGIN

	SET @gone_in = 1

	SELECT	@next_id = MAX(accumulation_id)
	FROM	accumulation

	IF @next_id IS NULL
		SELECT	@next_id = 0

	SELECT	@next_id = @next_id + 1

	SELECT	@table_name = 'Accumulation'
	
	DELETE
	FROM	unique_number
	WHERE	table_name = @table_name

        INSERT INTO Unique_Number
        ( table_name, next_number )
        VALUES
        ( @table_name, @next_id )

	FETCH NEXT FROM accumulation_cursor INTO @source_id

END

IF @gone_in = 0
BEGIN
	UPDATE Unique_Number 
	SET next_number=1
	WHERE table_name = 'Accumulation'
END

CLOSE accumulation_cursor
DEALLOCATE accumulation_cursor

DECLARE	contact_cursor CURSOR FOR
	SELECT	DISTINCT source_id
	FROM	contact

OPEN	contact_cursor

FETCH NEXT FROM contact_cursor INTO @source_id

SET @gone_in = 0

WHILE @@FETCH_STATUS = 0
BEGIN

	SET @gone_in = 1

	SELECT	@next_id = MAX(contact_id)
	FROM	contact
	WHERE	source_id = @source_id

	IF @next_id IS NULL
		SELECT	@next_id = 0

	SELECT	@next_id = @next_id + 1

	SELECT	@table_name = 'Contact_' + convert(varchar(10), @source_id)
	
	DELETE
	FROM	unique_number
	WHERE	table_name = @table_name

        INSERT INTO Unique_Number
        ( table_name, next_number )
        VALUES
        ( @table_name, @next_id )

	FETCH NEXT FROM contact_cursor INTO @source_id

END

IF @gone_in = 0
BEGIN
	UPDATE Unique_Number 
	SET next_number=1
	WHERE table_name like 'Contact_%'
END

CLOSE contact_cursor
DEALLOCATE contact_cursor

DECLARE	party_cursor CURSOR FOR
	SELECT	DISTINCT source_id
	FROM	party

OPEN	party_cursor

FETCH NEXT FROM party_cursor INTO @source_id

SET @gone_in = 0

WHILE @@FETCH_STATUS = 0
BEGIN

	SET @gone_in = 1

	SELECT	@next_id = MAX(party_id)
	FROM	party
	WHERE	source_id = @source_id

	IF @next_id IS NULL
		SELECT	@next_id = 0

	SELECT	@next_id = @next_id + 1

	SELECT	@table_name = 'Party_' + convert(varchar(10), @source_id)
	
	DELETE
	FROM	unique_number
	WHERE	table_name = @table_name

        INSERT INTO Unique_Number
        ( table_name, next_number )
        VALUES
        ( @table_name, @next_id )

	FETCH NEXT FROM party_cursor INTO @source_id

END

IF @gone_in = 0
BEGIN
	UPDATE Unique_Number 
	SET next_number=1
	WHERE table_name like 'Party_%'
END

CLOSE party_cursor
DEALLOCATE party_cursor

DECLARE	insurance_folder_cursor CURSOR FOR
	SELECT	DISTINCT source_id
	FROM	insurance_folder

OPEN	insurance_folder_cursor

FETCH NEXT FROM insurance_folder_cursor INTO @source_id

SET @gone_in = 0

WHILE @@FETCH_STATUS = 0
BEGIN

	SET @gone_in = 1

	SELECT	@next_id = MAX(insurance_folder_id)
	FROM	insurance_folder
	WHERE	source_id = @source_id

	IF @next_id IS NULL
		SELECT	@next_id = 0

	SELECT	@next_id = @next_id + 1

	SELECT	@table_name = 'Insurance_Folder_' + convert(varchar(10), @source_id)
	
	DELETE
	FROM	unique_number
	WHERE	table_name = @table_name

        INSERT INTO Unique_Number
        ( table_name, next_number )
        VALUES
        ( @table_name, @next_id )

	FETCH NEXT FROM insurance_folder_cursor INTO @source_id

END

IF @gone_in = 0
BEGIN
	UPDATE Unique_Number 
	SET next_number=1
	WHERE table_name like 'Insurance_Folder_%'
END

CLOSE insurance_folder_cursor
DEALLOCATE insurance_folder_cursor

DECLARE	insurance_file_cursor CURSOR FOR
	SELECT	DISTINCT source_id
	FROM	insurance_file

OPEN	insurance_file_cursor

FETCH NEXT FROM insurance_file_cursor INTO @source_id

SET @gone_in = 0

WHILE @@FETCH_STATUS = 0
BEGIN

	SET @gone_in = 1

	SELECT	@next_id = MAX(insurance_file_id)
	FROM	insurance_file
	WHERE	source_id = @source_id

	IF @next_id IS NULL
		SELECT	@next_id = 0

	SELECT	@next_id = @next_id + 1

	SELECT	@table_name = 'Insurance_File_' + convert(varchar(10), @source_id)
	
	DELETE
	FROM	unique_number
	WHERE	table_name = @table_name

        INSERT INTO Unique_Number
        ( table_name, next_number )
        VALUES
        ( @table_name, @next_id )

	FETCH NEXT FROM insurance_file_cursor INTO @source_id

END

IF @gone_in = 0
BEGIN
	UPDATE Unique_Number 
	SET next_number=1
	WHERE table_name like 'Insurance_File_%'
END

CLOSE insurance_file_cursor
DEALLOCATE insurance_file_cursor

-- Alix - 23/06/2003

DECLARE	risk_folder_cursor CURSOR FOR
	SELECT	DISTINCT source_id
	FROM	risk_folder

OPEN	risk_folder_cursor

FETCH NEXT FROM risk_folder_cursor INTO @source_id

SET @gone_in = 0

WHILE @@FETCH_STATUS = 0
BEGIN

	SET @gone_in = 1

	SELECT	@next_id = MAX(risk_folder_cnt)
	FROM	risk_folder
	WHERE	source_id = @source_id

	IF @next_id IS NULL
		SELECT	@next_id = 0

	SELECT	@next_id = @next_id + 1

	SELECT	@table_name = 'Risk_Folder_' + convert(varchar(10), @source_id)
	
	DELETE
	FROM	unique_number
	WHERE	table_name = @table_name

        INSERT INTO Unique_Number
        ( table_name, next_number )
        VALUES
        ( @table_name, @next_id )

	FETCH NEXT FROM risk_folder_cursor INTO @source_id

END

IF @gone_in = 0
BEGIN
	UPDATE Unique_Number 
	SET next_number=1
	WHERE table_name like 'Risk_Folder_%'
END

CLOSE risk_folder_cursor
DEALLOCATE risk_folder_cursor

DECLARE	Transaction_Export_Folder_cursor CURSOR FOR
	SELECT	DISTINCT source_id
	FROM	Transaction_Export_Folder

OPEN	Transaction_Export_Folder_cursor

FETCH NEXT FROM Transaction_Export_Folder_cursor INTO @source_id

SET @gone_in = 0

WHILE @@FETCH_STATUS = 0
BEGIN

	SET @gone_in = 1

	SELECT	@next_id = MAX(Transaction_Export_Folder_cnt)
	FROM	Transaction_Export_Folder
	WHERE	source_id = @source_id

	IF @next_id IS NULL
		SELECT	@next_id = 0

	SELECT	@next_id = @next_id + 1

	SELECT	@table_name = 'Transaction_Export_Folder_' + convert(varchar(10), @source_id)
	
	DELETE
	FROM	unique_number
	WHERE	table_name = @table_name

        INSERT INTO Unique_Number
        ( table_name, next_number )
        VALUES
        ( @table_name, @next_id )

	FETCH NEXT FROM Transaction_Export_Folder_cursor INTO @source_id

END

IF @gone_in = 0
BEGIN
	UPDATE Unique_Number 
	SET next_number=1
	WHERE table_name like 'Transaction_Export_Folder_%'
END

CLOSE Transaction_Export_Folder_cursor
DEALLOCATE Transaction_Export_Folder_cursor


PRINT 'RESET TEXT FILE TABLE'

declare entity_cursor cursor 
for	select entity_type_id
	from text_file_number

open entity_cursor

fetch next from entity_cursor
into @entityid

while @@fetch_status = 0
begin
	update text_file_number
	set next_file_number = 
	(	select isnull(max(file_number), 1)
		from text_file
		where entity_type_id = @entityid
	)
	where entity_type_id = @entityid

	fetch next from entity_cursor
	into @entityid
end

close entity_cursor
deallocate entity_cursor

declare entity_cursor cursor 
for	select entity_type_id
	from text_file_number

open entity_cursor

fetch next from entity_cursor
into @entityid

while @@fetch_status = 0
begin
	update text_file_number
	set next_file_number = 
	(	select isnull(max(file_number), 1)
		from text_file
		where entity_type_id = @entityid
	)
	where entity_type_id = @entityid

	fetch next from entity_cursor
	into @entityid
end

close entity_cursor
deallocate entity_cursor


-- Re-seeds all identity values

PRINT 'RESEED ALL TABLES'
PRINT ''

EXEC DDLReSeedIdentities

-- DN v0.9
dbcc checkident('GIS_Policy_link', reseed, 1000)
     
EXEC sp_MSforeachtable @command1="print '?' DBCC DBREINDEX ('?', ' ', 80)"
GO

PRINT ''
PRINT '**********  FINISHED (COMMIT)  **********'
PRINT ''
PRINT ''

DECLARE @DME_Folder VARCHAR(50)

select @DME_Folder = dd.server_unc + dd.share_name + dv.directory from doc_device dd, doc_volume dv
PRINT '*** REMEMBER TO DELETE THE DME DATA FILES FROM: ' + @DME_Folder
PRINT ''

Cleardown_End:
PRINT '---END---'

-- Uncomment to get a table record count at the end of the process for verification
/*

select Tablename = t.name, Records = i.rows
from sysobjects t, sysindexes i 
where t.xtype = 'U'
and i.id = t.id
and i.indid in (0,1)
order by t.name
*/

GO 

-- Uncomment to get Transactional Rollback for testing
-- ROLLBACK TRANSACTION