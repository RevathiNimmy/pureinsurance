-- *******************************************************************************************
-- Party Cleardown Stored Procedure

-- v1.0	    D. Davis 2008-07-18  First Version for S4I 1.11
-- *******************************************************************************************/

EXEC DDLDropProcedure 'spu_SIR_Party_Cleardown'
GO

CREATE PROCEDURE spu_SIR_Party_Cleardown
	@party_cnt INT
AS

SET NOCOUNT ON

DECLARE @account_id INT

PRINT ''
PRINT '**********  STARTED  **********'
PRINT ''

--Report if there are any personal/corporate/group clients incorrectly linked to BankAccounts.
--If there are then do NOT start the cleardown until they are manually fixed in the system.
IF (SELECT count(*) FROM Account 
    WHERE Account_key IN (SELECT Party_cnt from Party WHERE party_cnt=@party_cnt		
                          AND Account_id IN (SELECT account_id FROM BankAccount)))>0
BEGIN
   PRINT 'CLEARDOWN HAS NOT BEEN RUN SINCE THE PARTY IS LINKED TO A BANKACCOUNT'
   PRINT ' ' 
   SELECT * FROM Account 
    WHERE Account_key IN (SELECT Party_cnt from Party WHERE Party_type_id in (1, 2, 4))		
                          AND Account_id IN (SELECT account_id FROM BankAccount)

   RETURN
END

-- *******************************************************************************************/
-- BUILD LIST OF KEYS TO BE USED FOR CLEARING DOWN
-- *******************************************************************************************/
PRINT 'BUILDING KEYS FOR CLEARING DOWN PARTY'

SELECT @account_id = account_id FROM Account WHERE account_key=@party_cnt
CREATE TABLE #Risks(
	insurance_folder_cnt INT,
	insurance_file_cnt INT,
	risk_cnt INT)

INSERT INTO #Risks
	SELECT 	IFL.insurance_folder_cnt, IFRL.insurance_file_cnt, IFRL.risk_cnt
	FROM	Insurance_File_Risk_Link IFRL
	INNER JOIN Insurance_File IFL ON IFL.insurance_file_cnt = IFRL.insurance_file_cnt
	WHERE IFL.insured_cnt=@party_cnt

CREATE TABLE #Claims(
	claim_id INT,
	claim_payment_id INT,
	claim_receipt_id INT)

INSERT INTO #Claims
	SELECT DISTINCT C.claim_id, NULL, NULL
	FROM	Claim C
	INNER JOIN #Risks R ON R.insurance_file_cnt=C.Policy_id

INSERT INTO #Claims
	SELECT DISTINCT CP.claim_id, CP.claim_payment_id, NULL
	FROM	Claim_Payment CP
	INNER JOIN #Claims C ON C.claim_id=CP.claim_id

INSERT INTO #Claims
	SELECT DISTINCT CR.claim_id, NULL, CR.claim_receipt_id
	FROM	Claim_Receipt CR
	INNER JOIN #Claims C ON C.claim_id=CR.claim_id

CREATE TABLE #PolicyLinks(
	gis_policy_link_id INT)

INSERT INTO #PolicyLinks
	SELECT DISTINCT gis_policy_link_id
	FROM	GIS_Policy_Link
	WHERE	party_cnt=@party_cnt

INSERT INTO #PolicyLinks
	SELECT DISTINCT GPL.gis_policy_link_id
	FROM	GIS_Policy_Link GPL
	INNER JOIN #Risks R ON R.risk_cnt=GPL.risk_id

INSERT INTO #PolicyLinks
	SELECT DISTINCT GPL.gis_policy_link_id
	FROM	GIS_Policy_Link GPL
	INNER JOIN #Claims C ON C.claim_id = GPL.claim_id

CREATE TABLE #Documents(
	document_id INT)

INSERT INTO #Documents
	SELECT DISTINCT document_id
	FROM TransDetail
	WHERE account_id=@account_id

CREATE TABLE #CashList(
	cashlist_id INT)

INSERT INTO #CashList
	SELECT DISTINCT CLI.cashlist_id
	FROM CashListItem CLI
	WHERE CLI.account_id=@account_id

CREATE TABLE #Allocation(
	allocation_id INT)

INSERT INTO #Allocation
	SELECT DISTINCT AD.allocation_id
	FROM AllocationDetail AD
	INNER JOIN TransDetail TD ON TD.transdetail_id = AD.transdetail_id
	WHERE TD.account_id=@account_id

INSERT INTO #Allocation
	SELECT DISTINCT A.allocation_id
	FROM Allocation A
	WHERE A.account_id=@account_id

CREATE TABLE #MatchGroup(
	match_id INT)

INSERT INTO #MatchGroup
	SELECT DISTINCT TM.match_id
	FROM TransMatch TM
	INNER JOIN TransDetail TD ON TD.transdetail_id = TM.transdetail_id
	WHERE TD.account_id=@account_id

CREATE TABLE #PFPLans(
	pfprem_finance_cnt INT,
	pfprem_finance_version INT)

INSERT INTO #PFPLans
	SELECT DISTINCT PF.pfprem_finance_cnt, PF.pfprem_finance_version
	FROM PFPremiumFinance PF
	WHERE PF.ClientId=@party_cnt

CREATE TABLE #Element(
	element_id INT)

INSERT INTO #Element
	SELECT DISTINCT element_id
	FROM StructureTree
	WHERE account_id=@account_id

CREATE TABLE #Addresses(
	address_cnt INT,
	contact_cnt INT)

INSERT INTO #Addresses
	SELECT NULL, Contact_Cnt FROM Party_Contact_Usage WHERE party_cnt = @party_cnt

INSERT INTO #Addresses
	SELECT Address_Cnt, NULL FROM Party_Address_Usage WHERE party_cnt = @party_cnt

-- *******************************************************************************************/
-- DELETE CLAIM ADDRESS DATE FROM USER-DEFINED CLAIMSBUILDER TABLES
-- *******************************************************************************************/
DECLARE @TableName VARCHAR(70),
	@GISObjectName VARCHAR(70),
	@GISColumnName VARCHAR(70),
    @DeleteString VARCHAR(2000),
    @GISDataModel VARCHAR(70)

DECLARE C_DataModel CURSOR FAST_FORWARD FOR
	SELECT RTRIM(GDM.code), GOB.table_name, GP.column_name
	FROM GIS_Property GP
	INNER JOIN GIS_Object GOB ON GOB.gis_object_id=GP.gis_object_id
	INNER JOIN GIS_Data_Model GDM ON GDM.gis_data_model_id=GOB.gis_data_model_id
	WHERE GP.column_name like '%address_cnt%'

OPEN C_DataModel
FETCH NEXT FROM C_DataModel INTO @GISDataModel, @GISObjectName, @GISColumnName
WHILE (@@FETCH_STATUS = 0)
BEGIN
	SET @DeleteString='DELETE Claim_Address FROM Claim_Address CA INNER JOIN '+
		@GISObjectName+' TAB ON CA.address_cnt=CAST(TAB.'+@GISColumnName+' AS INTEGER)'
		--' INNER JOIN '+@GISDataModel + '_Policy_Binder PB ON PB.'+@GISDataModel+'_Policy_binder_id=TAB.'+@GISDataModel+'_Policy_binder_id '+
		--'INNER JOIN GIS_Policy_Link GPL ON GPL.gis_policy_link_id=PB.gis_policy_link_id'

	EXEC (@DeleteString)
	FETCH NEXT FROM C_DataModel INTO @GISDataModel, @GISObjectName, @GISColumnName
END
CLOSE C_DataModel
DEALLOCATE C_DataModel

-- *******************************************************************************************/
-- DELETE ALL DATA FROM USER-DEFINED PRODUCTBUILDER TABLES
-- *******************************************************************************************/
PRINT 'DATAMODEL TABLES'

CREATE TABLE #PolicyBinders(
	policy_binder_id INT)

DECLARE @GisObjectID INT,
    @ParentObjectID INT

DECLARE C_DataModel CURSOR FAST_FORWARD FOR
    SELECT RTRIM(code)
    FROM GIS_data_model
    ORDER BY code DESC

OPEN C_DataModel
FETCH NEXT FROM C_DataModel INTO @GisDataModel
WHILE (@@FETCH_STATUS = 0)
BEGIN
IF EXISTS (SELECT * FROM sysobjects where name = @GisDataModel + '_Policy_Binder')
BEGIN
	SELECT @DeleteString = 'INSERT INTO #PolicyBinders SELECT ' + @GisDataModel + '_Policy_binder_id FROM '
		+ @GisDataModel + '_Policy_Binder PB INNER JOIN #PolicyLinks ON PB.gis_policy_link_id=#PolicyLinks.gis_policy_link_id'
	EXEC (@DeleteString)
END

IF EXISTS (SELECT * FROM sysobjects where name = @GisDataModel + '_standard_wording')
    BEGIN 
        SET @DeleteString = 'DELETE ' + @GisDataModel + '_standard_wording FROM ' + @GisDataModel + '_standard_wording PB2 ' + 
		'INNER JOIN #PolicyBinders PB ON PB.policy_binder_id=PB2.' + @GisDataModel + '_policy_binder_id'
        Print '    > ' + @GisDataModel + '_standard_wording'
        EXEC (@DeleteString)
    END

    IF EXISTS (SELECT * FROM sysobjects where name = @GisDataModel + '_sum_insured')
    BEGIN 
        SET @DeleteString = 'DELETE ' + @GisDataModel + '_sum_insured FROM ' + @GisDataModel + '_sum_insured PB2 ' +
		'INNER JOIN #PolicyBinders PB ON PB.policy_binder_id=PB2.' + @GisDataModel + '_policy_binder_id'
        Print '    > ' + @GisDataModel + '_sum_insured'
        EXEC (@DeleteString)
    END

    FETCH NEXT FROM C_DataModel INTO @GisDataModel
END
CLOSE C_DataModel
DEALLOCATE C_DataModel


DECLARE C_Objects CURSOR FAST_FORWARD FOR
    SELECT GOB.gis_object_id, GOB.parent_object_id, GOB.table_name, RTRIM(GDM.code)
    FROM GIS_Object GOB
    INNER JOIN GIS_Data_Model GDM ON GDM.gis_data_model_id=GOB.gis_data_model_id
    ORDER BY parent_object_id DESC


OPEN C_Objects
FETCH NEXT FROM C_Objects INTO @GisObjectID, @ParentObjectID, @TableName, @GisDataModel
WHILE (@@FETCH_STATUS = 0)
BEGIN
	IF RIGHT(@TableName,12)='_Claim_Peril' OR RIGHT(@TableName,6)='_Claim' BEGIN
		SET @DeleteString = 'DELETE ' + @TableName + ' FROM ' + @TableName +
			' INNER JOIN #Claims ON ' + @TableName + '.claim_id=#Claims.claim_id'
	END
	ELSE BEGIN
		SET @DeleteString = 'DELETE ' + @TableName + ' FROM ' + @TableName +
			' INNER JOIN #PolicyBinders ON ' + @TableName + '.' + @GisDataModel +'_policy_binder_id=#PolicyBinders.policy_binder_id'
	END
    Print '    > ' + @TableName
    EXEC (@DeleteString)

    FETCH NEXT FROM C_Objects INTO @GisObjectID, @ParentObjectID, @TableName, @GisDataModel
END
CLOSE C_Objects 
DEALLOCATE C_Objects


-- *******************************************************************************************/
-- GIS POLICY LINK
-- *******************************************************************************************/

PRINT 'GIS POLICY LINK'

DELETE GIS_Policy_Link
FROM	GIS_Policy_Link TAB
	INNER JOIN #PolicyLinks ON #PolicyLinks.gis_policy_link_id=TAB.gis_policy_link_id

-- *******************************************************************************************/
-- DOCUMASTER FOLDERS
-- *******************************************************************************************/

DECLARE @iResult AS INT
EXEC @iResult=DDLExistsTable 'DOC_Folder' 
IF @iResult=1
BEGIN
	PRINT 'DOCUMASTER FOLDERS'
	
	-- Remove the history first
	DELETE DOC_History FROM DOC_History DH
	WHERE DH.drawercode=@party_cnt

	-- Remove the child folders
	DELETE DOC_Folder FROM DOC_Folder
	INNER JOIN DOC_Folder DF2 ON DF2.folder_num = DOC_Folder.parent_num
	WHERE DF2.ex_code=@party_cnt

	-- Remove the child folders
	DELETE DOC_Folder FROM DOC_Folder DF
	WHERE DF.ex_code=@party_cnt
END

-- *******************************************************************************************/
-- EVENTS
-- *******************************************************************************************/

PRINT 'EVENT'

DELETE Event_Insurance_COB_Section FROM Event_Insurance_COB_Section TAB
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt

DELETE Event_Insurance_File FROM Event_Insurance_File TAB
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt

DELETE Event_Insurance_File_System FROM Event_Insurance_File_System TAB
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt

DELETE Event_Insurance_Folder FROM Event_Insurance_Folder TAB
	INNER JOIN #Risks ON #Risks.insurance_folder_cnt=TAB.insurance_folder_cnt

DELETE Event_Log WHERE party_cnt=@party_cnt
DELETE Event_Log FROM Event_Log TAB
	INNER JOIN #Risks ON #Risks.insurance_folder_cnt=TAB.insurance_folder_cnt WHERE party_cnt=@party_cnt

-- *******************************************************************************************/
-- CREDIT CONTROL
-- *******************************************************************************************/
PRINT 'CREDIT CONTROL'

DELETE	Credit_Control_Item 
	FROM Credit_Control_Item TAB INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt
DELETE  Last_Print_Run
DELETE  Renewal_Status 
	FROM Renewal_Status TAB INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt

-- *******************************************************************************************/
-- TEMPORARY REPORT DATA
-- *******************************************************************************************/
DELETE  report_audit_debit_table1
DELETE  report_audit_debit_table2
DELETE  report_audit_debit_table3
DELETE  report_audit_debit_table6
DELETE  ReportPartyList
DELETE  ReportPolicyList

-- *******************************************************************************************/
-- TAX
-- *******************************************************************************************/
PRINT 'TAX'
DELETE  Tax_Calculation FROM Tax_Calculation TAB
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt

-- *******************************************************************************************/
-- ADDRESS INFORMATION
-- *******************************************************************************************/
PRINT 'ADDRESS AND CONTACT INFORMATION'

DELETE Contact_Address_Usage FROM Contact_Address_Usage TAB
	INNER JOIN #Addresses A ON A.contact_cnt=TAB.contact_cnt

DELETE Party_Contact_Usage WHERE Party_cnt = @party_cnt
DELETE Party_Address_Usage WHERE Party_cnt = @party_cnt

DELETE Claim_address FROM Claim_address TAB
	INNER JOIN claim c on c.client_address=TAB.address_cnt 
	INNER JOIN insurance_file ifl on ifl.insurance_file_cnt=c.policy_id where ifl.insured_cnt=@party_cnt


DELETE Address FROM Address TAB
	INNER JOIN #Addresses A ON A.address_cnt=TAB.address_cnt

DELETE Contact FROM Contact TAB
	INNER JOIN #Addresses A ON A.contact_cnt=TAB.contact_cnt

-- *******************************************************************************************/
-- CLAIMS
-- *******************************************************************************************/
PRINT 'CLAIMS REINSURANCE'

DELETE DTLinks FROM DTLinks TAB INNER JOIN #Claims ON #Claims.claim_id=TAB.siriuskey
DELETE claim_ri_arrangement_line FROM claim_ri_arrangement_line TAB INNER JOIN #Claims ON #Claims.claim_id=TAB.claim_id
DELETE Claim_XOL_Arrangement FROM Claim_XOL_Arrangement TAB INNER JOIN #Claims ON #Claims.claim_id=TAB.claim_id
DELETE claim_ri_arrangement FROM claim_ri_arrangement TAB INNER JOIN #Claims ON #Claims.claim_id=TAB.claim_id

PRINT 'CLAIMS'
DELETE Claim_Receipt_Item FROM Claim_Receipt_Item TAB
	INNER JOIN #Claims ON #Claims.claim_receipt_id=TAB.claim_receipt_id

DELETE Claim_Receipt FROM Claim_Receipt TAB
	INNER JOIN #Claims ON #Claims.claim_receipt_id=TAB.claim_receipt_id

DELETE User_defined_peril_data FROM User_defined_peril_data TAB
	INNER JOIN #Claims ON #Claims.claim_id=TAB.claim_id

DELETE Claim_Payment_Item FROM Claim_Payment_Item TAB
	INNER JOIN #Claims ON #Claims.claim_payment_id=TAB.claim_payment_id
DELETE Claim_Payment FROM Claim_Payment TAB
	INNER JOIN #Claims ON #Claims.claim_payment_id=TAB.claim_payment_id

DELETE Reserve FROM Reserve R
	INNER JOIN Claim_Peril CP ON CP.claim_peril_id = R.claim_peril_id
	INNER JOIN #Claims ON #Claims.claim_id=CP.claim_id

DELETE Claim_User_Defined_Risk_Data FROM Claim_User_Defined_Risk_Data TAB
	INNER JOIN #Claims ON #Claims.claim_id=TAB.claim_id

DELETE Recovery FROM Recovery R
	INNER JOIN Claim_Peril CP ON CP.claim_peril_id = R.claim_peril_id
	INNER JOIN #Claims ON #Claims.claim_id=CP.claim_id

DELETE  Claim_party_claim FROM Claim_party_claim TAB
	INNER JOIN #Claims ON #Claims.claim_id=TAB.claim_id

DELETE  Claim_user_defined_risk_data FROM Claim_user_defined_risk_data TAB
	INNER JOIN #Claims ON #Claims.claim_id=TAB.claim_id

DELETE Claim_Party_Link FROM Claim_Party_Link TAB
	INNER JOIN #Claims ON #Claims.claim_id=TAB.claim_id

DELETE Claim_Party FROM Claim_Party TAB
	INNER JOIN #Claims ON #Claims.claim_id=TAB.claim_id

DELETE Claim_Risk FROM Claim_Risk TAB
	INNER JOIN #Claims ON #Claims.claim_id=TAB.claim_id

DELETE Claim_Peril FROM Claim_Peril TAB
	INNER JOIN #Claims ON #Claims.claim_id=TAB.claim_id

DELETE Claim_Comments FROM Claim_Comments TAB
	INNER JOIN #Claims ON #Claims.claim_id=TAB.claim_id

DELETE Claim_CoInsurers FROM Claim_CoInsurers TAB
	INNER JOIN #Claims ON #Claims.claim_id=TAB.claim_id

DELETE Claim_Link FROM Claim_Link TAB
	INNER JOIN #Claims ON #Claims.claim_id=TAB.claim_id

DELETE [Case] FROM [Case] TAB
	INNER JOIN Claim ON Claim.base_case_id = TAB.base_case_id
	INNER JOIN #Claims ON #Claims.claim_id=Claim.claim_id

DELETE Claim FROM Claim TAB
	INNER JOIN #Claims ON #Claims.claim_id=TAB.claim_id

DELETE Claim_Folder FROM Claim_Folder TAB
	INNER JOIN #Risks ON #Risks.risk_cnt=TAB.risk_cnt

-- *******************************************************************************************/
-- RISK/PERILS
-- *******************************************************************************************/

PRINT 'RISK/PERIL'
DELETE  Peril_party FROM Peril_party TAB INNER JOIN #Claims ON #Claims.claim_id=TAB.claim_id
DELETE  Peril FROM Peril TAB  INNER JOIN #Risks ON #Risks.risk_cnt=TAB.risk_cnt
DELETE  Rating_section FROM Rating_Section TAB INNER JOIN #Risks ON #Risks.risk_cnt=TAB.risk_cnt

DELETE  RI_Arrangement_Line FROM RI_Arrangement_Line RIL
	INNER JOIN RI_Arrangement TAB ON RIL.ri_arrangement_id=TAB.ri_arrangement_id
	INNER JOIN #Risks ON #Risks.risk_cnt=TAB.risk_cnt

DELETE  RI_Arrangement 
	FROM RI_Arrangement TAB INNER JOIN #Risks ON #Risks.risk_cnt=TAB.risk_cnt
DELETE  insurance_file_risk_link 
	FROM insurance_file_risk_link TAB INNER JOIN #Risks ON #Risks.risk_cnt=TAB.risk_cnt
DELETE  insurance_file_persistent_risk_link 
	FROM insurance_file_persistent_risk_link TAB INNER JOIN #Risks ON #Risks.risk_cnt=TAB.risk_cnt

DELETE  Accumulation_values FROM Accumulation_values TAB INNER JOIN #Risks ON #Risks.risk_cnt=TAB.risk_cnt
DELETE  Risk FROM Risk TAB INNER JOIN #Risks ON #Risks.risk_cnt=TAB.risk_cnt
DELETE  Risk_folder FROM Risk_Folder TAB INNER JOIN #Risks ON #Risks.insurance_folder_cnt=TAB.insurance_folder_cnt

-- *******************************************************************************************/
-- STATS
-- *******************************************************************************************/

PRINT 'STATS & TRANS'
DELETE Stats_Detail FROM Stats_Detail SD
	INNER JOIN Stats_Folder TAB ON TAB.stats_folder_cnt = SD.stats_folder_cnt
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt
DELETE Stats_folder FROM Stats_Folder TAB
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt

DELETE Transaction_Export_Detail FROM Transaction_Export_Detail TD
	INNER JOIN Transaction_Export_Folder TAB ON TAB.transaction_export_folder_cnt = TD.transaction_export_folder_cnt
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt
DELETE Transaction_Export_Complete FROM Transaction_Export_Complete TD
	INNER JOIN Transaction_Export_Folder TAB ON TAB.transaction_export_folder_cnt = TD.transaction_export_folder_cnt
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt
DELETE Transaction_Export_Folder FROM Transaction_Export_Folder TAB
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt

-- *******************************************************************************************/
-- ACCOUNTS
-- *******************************************************************************************/
PRINT 'APPROVAL/CHEQUE LOGS'

DELETE Payment_Approval FROM Payment_Approval PA
	INNER JOIN CashListItem TAB ON TAB.cashlistitem_id=PA.payment_cnt
	WHERE TAB.account_id=@account_id

DELETE Cheque FROM Cheque C
	INNER JOIN TransDetail TAB ON TAB.transdetail_id=C.transdetail_id
	WHERE TAB.account_id=@account_id

PRINT 'ALLOCATIONS'

DELETE TransMatch FROM TransMatch TM
	INNER JOIN #MatchGroup TAB ON TAB.match_id = TM.match_id
		
DELETE MatchGroup FROM MatchGroup MG
	INNER JOIN #MatchGroup TAB ON TAB.match_id = MG.match_id

DELETE AllocationDetail FROM AllocationDetail AD
	INNER JOIN #Allocation TAB ON TAB.allocation_id=AD.allocation_id

DELETE Allocation FROM Allocation A
	INNER JOIN #Allocation TAB ON TAB.allocation_id=A.allocation_id

DELETE AuditSet FROM AuditSet AUS
	INNER JOIN #Documents TAB ON TAB.document_id=AUS.document_id

PRINT 'RECEIPTS/PAYMENTS'

DELETE CashListItem_Instalments FROM CashListItem_Instalments TAB 
	INNER JOIN CashListItem C ON C.cashlistitem_id=TAB.cashlistitem_id
	INNER JOIN #CashList ON #CashList.cashlist_id = C.cashlist_id

DELETE CashListItem FROM CashListItem C
	INNER JOIN #CashList ON #CashList.cashlist_id = C.cashlist_id

DELETE CashList FROM CashList C
	INNER JOIN #CashList ON #CashList.cashlist_id = C.cashlist_id

DELETE Suspended_Accounts_Transactions FROM Suspended_Accounts_Transactions SAT
	INNER JOIN TransDetail TD ON TD.transdetail_id=SAT.suspended_transdetail_id
	INNER JOIN #Documents TAB ON TAB.document_id=TD.document_id

DELETE Released_Accounts_Transactions FROM Released_Accounts_Transactions RAT
	INNER JOIN TransDetail TD ON TD.transdetail_id=RAT.destination_transdetail_id
	INNER JOIN #Documents TAB ON TAB.document_id=TD.document_id

PRINT 'TRANSACTIONS'

DELETE TransDetail
FROM TransDetail TD
	INNER JOIN #Documents TAB ON TAB.document_id=TD.document_id

DELETE Document
FROM Document D
	INNER JOIN #Documents TAB ON TAB.document_id=D.document_id

PRINT 'INVOICES'

-- Additional Accounts tables
DELETE InsurerPayment WHERE account_id=@account_id
DELETE Invoice_item
FROM Invoice_Item II
INNER JOIN Invoice I ON I.invoice_id=II.invoice_id
WHERE I.account_id=@account_id

DELETE Invoice
FROM Invoice I
WHERE I.account_id=@account_id

PRINT 'STRUCTURE TREE'

DELETE StructureTree where account_id=@account_id
DELETE ElementExtras FROM ElementExtras TAB
	INNER JOIN #Element ON #Element.element_id=TAB.element_id
DELETE Element FROM Element TAB
	INNER JOIN #Element ON #Element.element_id=TAB.element_id
DELETE Account WHERE account_id=@account_id

-- *******************************************************************************************/
-- INSTALMENTS
-- *******************************************************************************************/
PRINT 'INSTALMENTS'

DELETE PFInstalments_History FROM PFInstalments_History PF
	INNER JOIN PFInstalments PFI ON PFI.pfinstalments_id = PF.pfinstalments_id
	INNER JOIN #PFPlans TAB ON TAB.pfprem_finance_cnt=PFI.pfprem_finance_cnt 
	AND TAB.pfprem_finance_version=PFI.pfprem_finance_version

DELETE PFInstalments FROM PFInstalments PF
	INNER JOIN #PFPlans TAB ON TAB.pfprem_finance_cnt=PF.pfprem_finance_cnt 
	AND TAB.pfprem_finance_version=PF.pfprem_finance_version

DELETE PFMediaTypeHistory FROM PFMediaTypeHistory PF
	INNER JOIN #PFPlans TAB ON TAB.pfprem_finance_cnt=PF.pfprem_finance_cnt 
	AND TAB.pfprem_finance_version=PF.pfprem_finance_version

DELETE PFPremiumFinance FROM PF_Accounts_Transactions PF
	INNER JOIN #PFPlans TAB ON TAB.pfprem_finance_cnt=PF.pfprem_finance_cnt 
	AND TAB.pfprem_finance_version=PF.pfprem_finance_version

DELETE PFTransaction_ID FROM PFTransaction_ID PF
	INNER JOIN #PFPlans TAB ON TAB.pfprem_finance_cnt=PF.pfprem_finance_cnt 
	AND TAB.pfprem_finance_version=PF.pfprem_finance_version
 
DELETE PF_Accounts_Transactions FROM PF_Accounts_Transactions PF
	INNER JOIN #PFPlans TAB ON TAB.pfprem_finance_cnt=PF.pfprem_finance_cnt 
	AND TAB.pfprem_finance_version=PF.pfprem_finance_version

DELETE PFPremiumFinance FROM PFPremiumFinance PF
	INNER JOIN #PFPlans TAB ON TAB.pfprem_finance_cnt=PF.pfprem_finance_cnt 
	AND TAB.pfprem_finance_version=PF.pfprem_finance_version

-- *******************************************************************************************/
-- POLICY
-- *******************************************************************************************/
PRINT 'POLICY'

DELETE  Policy_Standard_Wording FROM Policy_Standard_Wording TAB
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt
DELETE  Policy_Coinsurers FROM Policy_Standard_Wording TAB
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt
DELETE  Agent_Commission FROM Agent_Commission TAB
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt
DELETE  Policy_Fee_u FROM Policy_Fee_u TAB
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt
DELETE  Insurance_File_System FROM Insurance_File_System TAB
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt
DELETE  Document_Spooler FROM Document_Spooler TAB
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt
DELETE  Accumulation_values FROM Accumulation_values TAB
	INNER JOIN #Risks ON #Risks.risk_cnt=TAB.risk_cnt

UPDATE Cover_Note_Sheet 
SET insurance_file_cnt = NULL
FROM Cover_Note_Sheet TAB
INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt

DELETE  MTA_Insurance_File_Link FROM Policy_Standard_Wording TAB
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt
DELETE  Insurance_File FROM Insurance_File TAB
	INNER JOIN #Risks ON #Risks.insurance_folder_cnt=TAB.insurance_folder_cnt
DELETE  Insurance_Folder FROM Insurance_Folder TAB
	INNER JOIN #Risks ON #Risks.insurance_folder_cnt=TAB.insurance_folder_cnt

DELETE  Client_Product_Link WHERE party_cnt=@party_cnt
DELETE  Party_locator WHERE party_cnt=@party_cnt
DELETE  Party_marketing_data WHERE party_cnt=@party_cnt

DELETE  prospect_campaign

DELETE  Coi_compulsory_value FROM Coi_compulsory_value TAB
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt
DELETE  Coi_value FROM Coi_value TAB
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt
DELETE  Coi_arrangement FROM Coi_arrangement TAB
	INNER JOIN #Risks ON #Risks.insurance_file_cnt=TAB.insurance_file_cnt

-- *******************************************************************************************/
-- PARTY
-- *******************************************************************************************/
PRINT 'PARTY'

DELETE Party_Conviction WHERE Party_cnt = @party_cnt
DELETE Party_Private_Text WHERE Party_cnt = @party_cnt
DELETE Party_Prospect WHERE Party_cnt = @party_cnt
DELETE Prospect_Policy WHERE Party_cnt = @party_cnt
DELETE Party_Other WHERE Party_cnt = @party_cnt
DELETE previous_accidents WHERE Party_cnt = @party_cnt
DELETE Party_Supplier_Business WHERE Party_cnt = @party_cnt

DELETE Party_Corporate_Client WHERE Party_cnt = @party_cnt
DELETE Party_Personal_Client WHERE Party_cnt = @party_cnt
DELETE Party_Group_Client WHERE Party_cnt = @party_cnt
DELETE Party_Net_Data WHERE Party_cnt = @party_cnt

DELETE Party_Public_Text WHERE Party_cnt = @party_cnt
DELETE Party_Relationship WHERE Party_cnt = @party_cnt
DELETE party_lifestyle WHERE Party_cnt = @party_cnt

DELETE Party WHERE party_cnt = @party_cnt

--- END OF DELETE SCRIPTS

DROP TABLE #Risks
DROP TABLE #Claims
DROP TABLE #PolicyLinks
DROP TABLE #Documents
DROP TABLE #Allocation
DROP TABLE #MatchGroup
DROP TABLE #PolicyBinders
DROP TABLE #PFPlans
DROP TABLE #Element
DROP TABLE #Addresses

PRINT ''
PRINT '**********  FINISHED  **********'
PRINT ''

GO 