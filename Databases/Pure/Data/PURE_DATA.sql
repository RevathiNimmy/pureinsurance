
-- Pure 2.0 Database Upgrade file
-- Upgrade file starts

set quoted_identifier on set ansi_nulls on
go

-- *****************************************************************************
-- Header Template

-- *****************************************************************************
-- * <ISSxxx / PSxxx>: <Enter description here>
-- * Author:   <Your name>
-- * Date:     <Date added>
-- *****************************************************************************

-- *****************************************************************************
-- * Author:  Richard Taylor
-- * Date:    20/11/2007       
-- * Purpose: Add additional Task Entries.
-- *****************************************************************************
declare @pmwrk_task_id int
declare @caption_id int
if not exists(select null from pmwrk_task where code = 'SAMAMTAQTE')
Begin
    EXECUTE spu_pm_caption_id_return 1, 'SamAddMTAQuote', @caption_id OUTPUT
    insert into pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values(@caption_id, 'SAMAMTAQTE', 'SamAddMTAQuote',0,getdate(),0,1,null,null,null,null,1,0,null,null,null,0,2,null)
End
go

-- *****************************************************************************
-- * Author:  Vijay Bhushan
-- * Date:    20/11/2007       
-- * Purpose: PN39365 - Duplicate entries deleted from PmWrk_task.
-- *****************************************************************************
DECLARE  @code VARCHAR(10)
DECLARE  @pmwrk_task_id INT
                        
DECLARE c_cursor SCROLL CURSOR  FOR
SELECT   MIN(pmwrk_task_id),
         code
FROM     pmwrk_task
WHERE    code IN (SELECT   code
                  FROM     pmwrk_task
                  GROUP BY code
                  HAVING   COUNT(* ) > 1)
GROUP BY code

OPEN c_cursor

SELECT DISTINCT pmwrk_task_group_id
INTO   #temp
FROM   pmwrk_task_group_task
WHERE  pmwrk_task_id IN (SELECT pmwrk_task_id
                         FROM   pmwrk_task
                         WHERE  code IN (SELECT   code
                                         FROM     pmwrk_task
                                         GROUP BY code
                                         HAVING   COUNT(* ) > 1))
                        
FETCH FIRST FROM c_cursor
INTO @pmwrk_task_id,
     @code
     
WHILE @@FETCH_STATUS = 0
  BEGIN
   DELETE FROM pmwrk_task_group_task
   WHERE       pmwrk_task_id IN (SELECT pmwrk_task_id
                                  FROM   pmwrk_task
                                  WHERE  code IN (SELECT   code
                                                  FROM     pmwrk_task
                                                  GROUP BY code
                                                  HAVING   COUNT(* ) > 1))
												 
-- Update the PMWrk_User_Quick_start pmwrk_task_id before deleting from pmwrk_task
   Update PMWrk_User_Quick_start Set pmwrk_task_id = @pmwrk_task_id
   Where pmwrk_task_id IN (SELECT pmwrk_task_id from pmwrk_task where code = @Code ) 

   Update pmwrk_task_instance Set pmwrk_task_id = @pmwrk_task_id
   Where pmwrk_task_id IN (SELECT pmwrk_task_id from pmwrk_task where code = @Code )

   Update Credit_Control_Step Set pmwrk_task_id = @pmwrk_task_id
   Where pmwrk_task_id IN (SELECT pmwrk_task_id from pmwrk_task where code = @Code )

    DELETE FROM pmwrk_task
    WHERE       pmwrk_task_id <> @pmwrk_task_id
                AND code = @code
                           
    INSERT INTO pmwrk_task_group_task
               (pmwrk_task_group_id,
                pmwrk_task_id,
                display_sequence_num)
    SELECT pmwrk_task_group_id,
           @pmwrk_task_id,
           0
    FROM   #temp
            
    FETCH NEXT FROM c_cursor
    INTO @pmwrk_task_id,
         @code
          
  END

DROP TABLE #temp

CLOSE c_cursor

DEALLOCATE c_cursor


GO

-- *****************************************************************************
-- * Author:  Andrew Robinson
-- * Date:    29/11/2007       
-- * Purpose: Add GetDocumentList SAM Task
-- *****************************************************************************
declare @pmwrk_task_id int
declare @caption_id int
if not exists(select null from pmwrk_task where code = 'SAMGDocLst')
Begin
    EXECUTE spu_pm_caption_id_return 1, 'SamGetDocumentList', @caption_id OUTPUT
    insert into pmwrk_task
    ( caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values( @caption_id, 'SAMGDocLst', 'SamGetDocumentList',0,getdate(),0,1,null,null,null,null,1,0,null,null,null,0,2,null)
End
GO

-- *****************************************************************************
-- * Author:  Daniel Morey
-- * Date:    30/11/2007       
-- * Purpose: Issue 39678 - Update commission side of agent adjustment with correct transdetail_type
-- *****************************************************************************
UPDATE td
SET td.transdetail_type_id = 
    (
        SELECT
            transdetail_type_id
        FROM transdetail_type
        WHERE code = 'BROK ADJ'
    )
FROM transdetail td
JOIN transdetail_type tt
    ON tt.transdetail_type_id = td.transdetail_type_id
JOIN document d
    ON d.document_id = td.document_id
JOIN account a 
    ON a.account_id = td.account_id
JOIN ledger l
    ON l.ledger_id = a.ledger_id
WHERE l.ledger_short_name = 'CO'
AND tt.code = 'AGENTADJ'
AND td.spare = 'BROK ADJ'

GO

-- *****************************************************************************
-- * Author:  Andrew Robinson
-- * Date:    07/12/2007       
-- * Purpose: Add WM Task SAM Task
-- *****************************************************************************
declare @pmwrk_task_id int
declare @caption_id int
if not exists(select null from pmwrk_task where code = 'SAMWmTask')
Begin
    EXECUTE spu_pm_caption_id_return 1, 'SamAddWmTask', @caption_id OUTPUT
    insert into pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values(@caption_id, 'SAMWmTask', 'SamAddWmTask',0,getdate(),0,1,null,null,null,null,1,0,null,null,null,0,2,null)
End
GO

-- *****************************************************************************
-- * Author:   Daniel Morey
-- * Date:     12-12-07
-- * Purpose:  Issue 39557 - Add main contact field for group clients
-- *****************************************************************************
IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'GroupMainContact')
BEGIN
    INSERT INTO wp_fields(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
    VALUES('GroupMainContact', 'spu_wp_partyall', 'contact', 0, 'Party', 'Group', 'Main Contact', 1, NULL, 9)
END

GO



-- *****************************************************************************
-- * Issue:  Nexus MTA - add SSP sub agent
-- * Author: Andrew Robinson
-- * Date:   18/12/2007
-- *****************************************************************************

IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN

	IF NOT EXISTS(SELECT NULL FROM party_agent WHERE is_ssp_subagent=1)
	BEGIN

	DECLARE @branch_id int
	DECLARE @user_id int
	DECLARE @country_id int
	DECLARE @address_cnt int
	DECLARE @party_cnt int
	DECLARE @party_type_id int
	DECLARE @currency_id int
	DECLARE @language_id int
	DECLARE @party_structure_id int
	DECLARE @now datetime

	SELECT @now=getdate()

	SELECT TOP 1 @branch_id=source_id, @currency_id=base_currency_id FROM Source ORDER BY source_id ASC
	SELECT TOP 1 @user_id=user_id, @language_id=language_id FROM PMUser ORDER BY user_id ASC

	SELECT @country_id=country_id FROM Country WHERE code='GBR'
	SELECT @party_type_id=party_type_id FROM Party_Type WHERE code='AG'
	SELECT @party_structure_id=party_structure_id FROM Party_Structure WHERE code='PMB'

	exec spe_Address_add 
		@address_cnt = @address_cnt OUTPUT ,
		@source_id = @branch_id ,
		@address_id = 0 ,
		@address1 = 'Fearnley Mill' ,
		@address2 = 'Dean Clough' ,
		@address3 = 'Halifax' ,
		@address4 = 'West Yorkshire' ,
		@postal_code = 'HX3 5AX' ,
		@country_id = @country_id ,
		@created_by_id = @user_id ,
		@date_created = @now ,
		@modified_by_id = NULL ,
		@last_modified = @now,
		@ExternalId = NULL


	exec spe_Party_add
		@party_cnt = @party_cnt output,
		@party_type_id = @party_type_id,
		@is_also_agent = 0,
		@party_structure_id = @party_structure_id,
		@source_id = @branch_id,
		@party_id = 0,
		@shortname = 'SSPSUBAGENT',
		@name = 'SSP Sub Agent',
		@resolved_name = 'SSP Sub Agent',
		@currency_id = @currency_id,
		@language_id = @language_id,
		@collect_type_id = NULL,
		@accum_treatment_type_id = NULL,
		@stats_treatment_type_id = NULL,
		@party_category_id = NULL,
		@agent_cnt = NULL,
		@consultant_cnt = NULL,
		@created_by_id = @user_id,
		@date_created = @now,
		@last_modified = @now,
		@modified_by_id = @user_id,
		@payment_method_code = 'Cash',
		@payment_term_code = 0,
		@credit_card_code = '',
		@file_code = '',
		@abc_count = NULL,
		@statements = 0,
		@reminder_type_id = NULL,
		@renewals = 0,
		@status = '',
		@last_action_type = '',
		@is_travel_agent = 0,
		@is_prospect = 0,
		@is_deleted = 0,
		@abi_code_on_406 = '',
		@abi_code_on_81 = '',
		@abi_codelist = '',
		@area_id = NULL,
		@service_level_id = 0,
		@invariant_key = 0,
		@record_status = '',
		@CCJs = 0,
		@user_defined_data_id = NULL,
		@seasonal_gift_id = NULL,
		@correspondence_type_id = NULL,
		@renewal_stop_code_id = NULL,
		@swift_party_id = NULL,
		@loyalty_number = NULL,
		@alternative_identifier = NULL,
		@marketing_segment_ind = NULL,
		@trading_name = NULL,
		@sub_branch_id = NULL,
		@tob_letter = NULL

	DECLARE @party_agent_type_id int
	DECLARE @origin_id int
	DECLARE @address_type_id int
	DECLARE @commission_posting_type_id int

	SELECT @party_agent_type_id=party_agent_type_id FROM party_agent_type WHERE code='Sub-Agent'
	SELECT @origin_id=party_agent_origin_id FROM party_agent_origin WHERE code='OTHER'
	SELECT @address_type_id=address_usage_type_id FROM address_usage_type WHERE code='3131 XCO'
	SELECT @commission_posting_type_id=commission_posting_type_id FROM commission_posting_type WHERE code='INVOICE'

	exec spe_Party_Agent_add @party_cnt = @party_cnt, @party_agent_type_id = @party_agent_type_id, @party_agent_origin_id = @origin_id, @agency_agreement_date = 'Dec 30 1899 12:00:00:000AM', @agency_next_review_date = 'Dec 29 1899 12:00:00:000AM', @agency_account_number = '', @is_branch = 0, @is_head_office = 0, @default_commission_percent = 0.0, @trading_name = 'SSP Sub Agent', @binder_indicator = NULL, @report_indicator = NULL, @linked_account_executive_id = 0, @linked_account_group = 0, @payment_method = NULL, @payment_frequency = NULL, @address_on_notice = @address_type_id, @type_of_statement = NULL, @source = NULL, @title = '', @multipac = 0, @contact_person = '', @first_name = '', @bank_account = '', @allow_consolidated_commission = 0, @date_cancelled = 'Dec 29 1899 12:00:00:000AM', @agent_status_id = NULL, @fsa_registration_number = '', @broker_abi_id = '', @expense_account_id = NULL, @is_in_transfer_mode = NULL, @transfer_to_business_type_id = NULL, @transfer_to_party_cnt = NULL, @use_override_commission_rate = 1, @domiciled_for_tax = 0, @can_make_live_invoice = 0, @can_make_live_instalments = 0, @can_make_live_paynow = 0, @is_standard_account = 1, @is_float_balance_account = 0, @is_overdraft_account = 0, @is_prepayment_account = 0, @expected_daily_premium = 0.0, @days_allowed = 0, @float_balance_limit = 0.0, @overdraft_limit = 0.0, @overdraft_expiry = NULL, @alternate_reference_mandatory = 0, @alternate_reference_for_each_transaction = 0, @commission_posting_type = @commission_posting_type_id
	UPDATE party_agent SET is_ssp_subagent=1 WHERE party_cnt=@party_cnt

	INSERT INTO party_address_usage (address_cnt, party_cnt, address_usage_type_id, risk_group_id) VALUES (@address_cnt, @party_cnt, @address_type_id, 0)

	DECLARE @this_branch_id int

	DECLARE BRANCH_CURSOR CURSOR FAST_FORWARD FOR
	SELECT source_id FROM Source WHERE ISNULL(is_deleted,0)=0

	OPEN BRANCH_CURSOR

	FETCH NEXT FROM BRANCH_CURSOR INTO @this_branch_id

	WHILE @@FETCH_STATUS=0
	BEGIN
		INSERT INTO
			Party_Agent_Branch (party_cnt, source_id)
		VALUES
			(@party_cnt, @this_branch_id)

		FETCH NEXT FROM BRANCH_CURSOR INTO @this_branch_id
	END

	CLOSE BRANCH_CURSOR
	DEALLOCATE BRANCH_CURSOR

	DECLARE @parent_node_id int
	DECLARE @sub_branch_id int
	DECLARE @multi_tree varchar(20)
	DECLARE @account_id int
	DECLARE @element_id int
	DECLARE @ledger_id int
	DECLARE @node_id int

	SELECT @sub_branch_id=sub_branch_id FROM party WHERE party_cnt=@party_cnt
	SELECT @multi_tree=value FROM hidden_options WHERE option_number=16

	IF ISNULL(@multi_tree,'0')='0'
		SELECT @sub_branch_id=1

	SELECT @parent_node_id = node_id, @ledger_id = ledger_id
	FROM   StructureTree s
	JOIN   ledger l ON l.mapping_id = s.mapping_id
	WHERE  l.ledger_short_name = 'UB'
	AND    l.sub_branch_id = @sub_branch_id

	exec spu_ACT_add_Account @Account_id = @account_id output, @company_id = @branch_id, @purgefrequency_id = 1, @accounttype_id = 4, @paymenttype_id = NULL, @currency_id = @currency_id, @ledger_id = @ledger_id, @account_name = 'SSP Sub Agent', @short_code = 'SSPSUBAGENT', @restrict_enquiry = 0, @restrict_update = 0, @delete_at_purge = 0, @contact_name = '', @address1 = 'Fearnley Mill', @address2 = 'Dean Clough', @address3 = 'Halifax', @address4 = 'West Yorkshire', @postal_code = 'HX3 5AX', @address_country = @country_id, @phone_area_code = '', @phone_number = '', @phone_extension = '', @fax_area_code = '', @fax_number = '', @fax_extension = '', @payment_name = '', @payment_account_code = '', @payment_branch_code = '', @payment_expiry_date = @now, @payment_reference1 = '', @payment_reference2 = '', @prooflist_report_id = NULL, @bordereau_report_id = NULL, @credit_limit = 0.0, @discount_percentage = 0.0, @settlement_period = 0, @bank_name = '', @bank_address1 = '', @bank_address2 = '', @bank_address3 = '', @bank_address4 = '', @bank_postal_code = '', @bank_country = NULL, @bank_phone_area_code = '', @bank_phone_number = '', @bank_phone_extension = '', @bank_fax_area_code = '', @bank_fax_number = '', @bank_fax_extension = '', @comments = '', @account_key = @party_cnt, @nominal_account_id = 0, @accountstatus_id = 1, @sub_branch_id = 0, @allow_electronic_payment = 0, @client_money_calc_account_type = 0, @client_bank_account_type = NULL
	exec spu_ACT_add_Element @element_id = @element_id output, @element_name = 'SSPSUBAGENT', @parent_id = NULL
	exec spe_ElementExtras_add @element_id = @element_id, @totalling_id = NULL, @description = NULL, @report_map_id = NULL, @account_map_id = NULL, @is_Deletable = 1
	exec spu_ACT_add_StructureTree @node_id = @node_id output, @company_id = @branch_id, @mapping_id = NULL, @account_id = @account_id, @element_id = @element_id, @parent_node_id = @parent_node_id

	IF EXISTS(SELECT NULL FROM system_options WHERE branch_id=1 AND option_number=10 AND value='1')
	BEGIN
		DECLARE @parent_folder_num int
		DECLARE @party_folder_num int
		DECLARE @general_folder_num int
		DECLARE @history_id int

		DECLARE @branch_c varchar(20)
		DECLARE @party_c varchar(20)

		SELECT @branch_c=CONVERT(varchar(20), @branch_id)
		SELECT @party_c=CONVERT(varchar(20), @party_cnt)

		IF NOT EXISTS(SELECT folder_num FROM DOC_folder WHERE ex_code=CONVERT(varchar(20),@branch_id) AND folder_level = 0 AND parent_num = 0) 
		BEGIN
			DECLARE @Branch_Name varchar(255)
			SELECT @Branch_Name = description from Source where source_id = @branch_id
			exec spu_DOC_add_folder @folder_name = @Branch_Name, @parent_num = 0, @ex_code = @branch_c, @folder_level = 0, @access_level = 9, @password = '', @create_date = @now, @Folder_Num = @party_folder_num output
			exec spu_DOC_add_history @task = 1, @cabinetcode = @branch_c, @cabinetname = @Branch_Name, @drawercode = '', @drawername = '', @foldercode = '', @foldername = '', @docref = '', @request_date = '', @request_time = '', @eventtype = '', @description = '', @volume = '', @pagefile = '', @doctype = '', @filler = '', @hderror = 'N', @create_date = @now, @processed = 'N', @History_id = @history_id output		
		END

		SELECT @parent_folder_num=folder_num FROM DOC_folder WHERE ex_code=CONVERT(varchar(20),@branch_id) AND folder_level = 0 AND parent_num = 0

		exec spu_DOC_add_folder @folder_name = 'SSPSUBAGENT', @parent_num = @parent_folder_num, @ex_code = @party_c, @folder_level = 1, @access_level = 9, @password = '', @create_date = @now, @Folder_Num = @party_folder_num output

		exec spu_DOC_add_history @task = 4, @cabinetcode = @branch_c, @cabinetname = '', @drawercode = @party_c, @drawername = 'SSPSUBAGENT', @foldercode = '', @foldername = '', @docref = '', @request_date = '', @request_time = '', @eventtype = '', @description = '', @volume = '', @pagefile = '', @doctype = '', @filler = '', @hderror = 'N', @create_date = @now, @processed = 'N', @History_id = @history_id output

		exec spu_DOC_add_folder @folder_name = 'GENERAL', @parent_num = @party_folder_num, @ex_code = 'GENERAL', @folder_level = 2, @access_level = 9, @password = '', @create_date = @now, @Folder_Num = @general_folder_num output

		exec spu_DOC_add_history @task = 7, @cabinetcode = @branch_c, @cabinetname = '', @drawercode = @party_c, @drawername = '', @foldercode = 'GENERAL', @foldername = 'GENERAL', @docref = '', @request_date = '', @request_time = '', @eventtype = '', @description = '', @volume = '', @pagefile = '', @doctype = '', @filler = '', @hderror = 'N', @create_date = @now, @processed = 'N', @History_id = @history_id output
	END

	END

END

GO

-- *****************************************************************************
-- * Issue:  PN40087 (Duplicate 'INSI' entries removed from batch_type table.)
-- * Author: Vijay Bhushan
-- * Date:   20/12/2007
-- *****************************************************************************
DECLARE @count int 

SELECT @count = ISNULL(Count(*),0)  FROM batch_type where code = 'INSI'

IF @count > 1
BEGIN

UPDATE batch 
SET batch_type_id = (
	SELECT MIN(batch_type_id)
	FROM batch_Type 
	WHERE code ='INSI')
WHERE batch_type_id in (Select batch_type_id from batch_type where code = 'INSI')

DELETE FROM batch_type
WHERE       batch_type_id <> (
	SELECT MIN(batch_type_id)
	FROM   batch_type
	WHERE  code = 'INSI')
AND code = 'INSI'

END
GO

-- *****************************************************************************
-- * Issue:  PN40287 Add LookupValue Instalment Deposit to cashlistitem_receipt_type
-- * Author: Amit Kumar 
-- * Date:   09/01/2008
-- *****************************************************************************
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN

    DECLARE @cashListItem_receipt_type_id INT
    DECLARE @lCaptionID INT
 
    -- Instalment Deposit
    IF NOT EXISTS(select code FROM CashListItem_Receipt_Type WHERE code = 'INSTDEPT')
    BEGIN

        -- Add LookupValue
        SELECT @cashListItem_receipt_type_id = MAX(cashListItem_receipt_type_id)+1 FROM CashListItem_Receipt_Type
        EXECUTE spu_pm_caption_id_return 1, 'Instalment Deposit', @lCaptionID OUTPUT
        INSERT INTO CashListItem_Receipt_Type(cashListItem_receipt_type_id,code, description, caption_id, effective_date,is_deleted, is_Instalment )
    	VALUES(@cashListItem_receipt_type_id,'INSTDEPT','Instalment Deposit', @lCaptionID, GETDATE(), 0,0 )

    END	    
END
GO

-- *****************************************************************************
-- * Author:  Richard Taylor
-- * Date:    10/01/2008
-- * Purpose: Add additional Task Entries.
-- *****************************************************************************
declare @pmwrk_task_id int
declare @caption_id int
if not exists(select null from pmwrk_task where code = 'SAMGOS')
Begin
    EXECUTE spu_pm_caption_id_return 1, 'SamGetOptionSetting', @caption_id OUTPUT
    insert into pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values(@caption_id, 'SAMGOS', 'SamGetOptionSetting',0,getdate(),0,1,null,null,null,null,1,0,null,null,null,0,2,null)
End
go


-- *****************************************************************************
-- * Author:        Pankaj Kaushik
-- * Date:          04-01-2008
-- * Purpose:       1810 Sr8 Unattended Renewals
-- *****************************************************************************
    -- Check for new renewal exception type
    IF NOT EXISTS (SELECT * FROM Renewal_exception_reason) BEGIN

	DECLARE @caption_id int

        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'Product Flagged for policy number change - manual acceptance required', @caption_id OUTPUT

        -- Add new new renewal exception type
        INSERT Renewal_exception_reason(Renewal_exception_reason_id, caption_id, is_deleted, effective_date, description, code)
            VALUES(1, @caption_id, 0, getdate(), 'Product Flagged for policy number change - manual acceptance required','POLNUM')

        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'Insufficient Instalment Details', @caption_id OUTPUT

        -- Add new new renewal exception type
        INSERT Renewal_exception_reason(Renewal_exception_reason_id, caption_id, is_deleted, effective_date, description, code)
            VALUES(2, @caption_id, 0, getdate(), 'Insufficient Instalment Details','INSTAL')

        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'Prepayment Required', @caption_id OUTPUT

        -- Add new new renewal exception type
        INSERT Renewal_exception_reason(Renewal_exception_reason_id, caption_id, is_deleted, effective_date, description, code)
            VALUES(3, @caption_id, 0, getdate(), 'Prepayment Required','PREPAY')

        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'No Renewal Document Template configured', @caption_id OUTPUT

        -- Add new new renewal exception type
        INSERT Renewal_exception_reason(Renewal_exception_reason_id, caption_id, is_deleted, effective_date, description, code)
            VALUES(4, @caption_id, 0, getdate(), 'No Renewal Document Template configured','TEMPLATE')

        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'Other Exception occured', @caption_id OUTPUT

        -- Add new new renewal exception type
        INSERT Renewal_exception_reason(Renewal_exception_reason_id, caption_id, is_deleted, effective_date, description, code)
            VALUES(5, @caption_id, 0, getdate(), 'Other Exception occured','OTHER')


    END
    GO

-- *****************************************************************************  
-- * Author:       Dan Morey
-- * Date:         01-02-2008
-- * Purpose:      Risk Transfer, RMAR & Contract Certainty Changes.
-- *****************************************************************************

/*Add records into new table for all insurer risks*/
IF EXISTS (SELECT NULL FROM Hidden_Options WHERE option_number = 1 AND VALUE = 'A')
BEGIN
INSERT INTO party_insurer_risk
(
    party_cnt,
    risk_code_id,
    risk_transfer_agreement,
    delegated_authority
)
SELECT
    pi.party_cnt,
    rc.risk_code_id, 
    ISNULL(risk_transfer_agreement, 1),
    ISNULL(rc.is_delegated_authority, 0)
FROM risk_code rc
CROSS JOIN party_insurer pi
WHERE rc.is_deleted = 0
AND NOT EXISTS
    (
        SELECT 
            NULL
        FROM party_insurer_risk
        WHERE party_cnt = pi.party_cnt
        AND risk_code_id = rc.risk_code_id
    )

/*Update extra lines on transdetail so that they don't require risk transfer processing*/
UPDATE td 
SET td.risk_transfer = 0 
FROM transdetail td    
JOIN account a
    ON a.account_id = td.account_id
JOIN party p
    ON p.party_cnt = a.account_key
JOIN party_type pt
    ON pt.party_type_id = p.party_type_id
WHERE pt.code = 'EX'
AND td.risk_transfer IS NULL

/*Default the new FSA fields on the extra table*/
UPDATE pe
SET pe.risk_transfer_agreement = 1,
    pe.delegated_authority = 0,
    pe.fsa_product_id = 0
FROM party_extra pe
JOIN party p
    ON p.party_cnt = pe.party_cnt
JOIN party_type pt
    ON pt.party_type_id = p.party_type_id
WHERE pt.code = 'EX'
AND NOT EXISTS
    (
        SELECT 
            NULL 
        FROM party_extra 
        WHERE risk_transfer_agreement IS NOT NULL
        OR delegated_authority IS NOT NULL
        OR fsa_product_id IS NOT NULL
    )

END
GO

-- *****************************************************************************
-- * Author:  Matthew Keough-West
-- * Date:    07/02/2008
-- * Purpose: Add additional Task Entries.
-- *****************************************************************************
declare @pmwrk_task_id int
declare @caption_id int
if not exists(select null from pmwrk_task where code = 'SAMAMtaQte')
Begin
    EXECUTE spu_pm_caption_id_return 1, 'SAMAddMtaQuote', @caption_id OUTPUT
    insert into pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values(@caption_id, 'SAMAMtaQte', 'SAMAddMtaQuote',0,getdate(),0,1,null,null,null,null,1,0,null,null,null,0,2,null)
End
go

-- *****************************************************************************
-- * Author:  Rajesh Jawane
-- * Date:    07/02/2008
-- * Purpose: Added Event User Merge Code
-- *****************************************************************************
IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='EventUserName')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('EventUserName','spu_wp_PolicyEvent','EventUserName',0,'Policy','Event','Event User Name',1,'PolicyEvent',9)
END
GO

-- *****************************************************************************
-- * Author:  Brajesh 
-- * Date:    08/02/2008
-- * Purpose: Added Merge Code For Loyalty Membership Screen
-- *****************************************************************************
IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyLoyaltyScheme')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyLoyaltyScheme','spu_wp_PartyLoyalityScheme','Code',0,'Party','Loyalty','Loyality Code',1,'PartyLoyalityScheme',9)

END

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyLoyaltySchemeDescription')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyLoyaltySchemeDescription','spu_wp_PartyLoyalityScheme','description',0,'Party','Loyalty','Loyalty Description',1,'PartyLoyalityScheme',9)

END

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyLoyaltySchemeMembershipNumber')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyLoyaltySchemeMembershipNumber','spu_wp_PartyLoyalityScheme','Membership_number',0,'Party','Loyalty','Loyalty Membership Number',1,'PartyLoyalityScheme',9)

END

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyLoyaltySchemeStartDate')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyLoyaltySchemeStartDate','spu_wp_PartyLoyalityScheme','Start_date',0,'Party','Loyalty','Scheme Start Date',1,'PartyLoyalityScheme',9)

END

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyLoyaltySchemeEndDate')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyLoyaltySchemeEndDate','spu_wp_PartyLoyalityScheme','End_Date',0,'Party','Loyalty','Scheme End Date',1,'PartyLoyalityScheme',9)

END

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyLoyaltySchemeother_reference')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyLoyaltySchemeother_reference','spu_wp_PartyLoyalityScheme','other_reference',0,'Party','Loyalty','Other References',1,'PartyLoyalityScheme',9)
END

GO
-- *****************************************************************************
-- * Author:  Vivek 
-- * Date:    03/04/2008
-- * Purpose: Added Merge Code For Total Agent Commission
-- * PN:      38111	
-- *****************************************************************************
IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='TotalAgentCommission')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,product_family)
    VALUES ('TotalAgentCommission','spu_wp_insurancefileall','total_agent_commission',11,'Policy','General','Total Agent Commission',1,9)

END

GO

-- *****************************************************************************
-- * Issue No: PN41263
-- * Author  : Ashutosh Bhardwaj
-- * Date    : 11/02/2008       
-- * Purpose : These reports are added back         
-- *****************************************************************************

IF EXISTS (SELECT NULL FROM Hidden_Options WHERE option_number = 1 AND VALUE = 'A')
BEGIN
  
    DECLARE  @report_id INT
    DECLARE  @group_id INT
                       
    --Undelete Business Summary by Insurer Risk Report
    UPDATE report SET is_deleted = 0 WHERE code = 'Bsum_In_Rk'
                  
    SELECT @report_id = report_id FROM report WHERE code = 'Bsum_In_Rk'
    SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'ALL'
        
    IF (ISNULL(@group_id,0) <> 0 AND ISNULL(@report_id,0) <> 0)
    BEGIN
        -- Link report to group           
        IF NOT EXISTS (SELECT NULL FROM Report_Group_Contents WHERE  report_group_id = @group_id AND report_id = @report_id)
            INSERT INTO Report_Group_Contents (report_group_id,report_id) VALUES (@group_id, @report_id)
    END          

    --Undelete Business Summary by Riskcode Report
    UPDATE report SET is_deleted = 0 WHERE code = 'Bsum_Rc'
                  
    SELECT @report_id = report_id FROM report WHERE code = 'Bsum_Rc'
    SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'ALL'
        
    IF (ISNULL(@group_id,0) <> 0 AND ISNULL(@report_id,0) <> 0)
    BEGIN
        -- Link report to group           
        IF NOT EXISTS (SELECT NULL FROM Report_Group_Contents WHERE  report_group_id = @group_id AND report_id = @report_id)
            INSERT INTO Report_Group_Contents (report_group_id,report_id) VALUES (@group_id, @report_id)
    END  


    --Undelete Business Summary by Transaction Type Report
    UPDATE report SET is_deleted = 0 WHERE code = 'BSum_Tr_Ty'
                  
    SELECT @report_id = report_id FROM report WHERE code = 'BSum_Tr_Ty'
    SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'ALL'
        
    IF (ISNULL(@group_id,0) <> 0 AND ISNULL(@report_id,0) <> 0)
    BEGIN
        -- Link report to group           
        IF NOT EXISTS (SELECT NULL FROM Report_Group_Contents WHERE  report_group_id = @group_id AND report_id = @report_id)
            INSERT INTO Report_Group_Contents (report_group_id,report_id) VALUES (@group_id, @report_id)
    END 


    --Undelete Insurer Business Summary Type Report
    UPDATE report SET is_deleted = 0 WHERE code = 'Ins_Bs_Sum'
                  
    SELECT @report_id = report_id FROM report WHERE code = 'Ins_Bs_Sum'
    SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'ALL'
        
    IF (ISNULL(@group_id,0) <> 0 AND ISNULL(@report_id,0) <> 0)
    BEGIN
        -- Link report to group           
        IF NOT EXISTS (SELECT NULL FROM Report_Group_Contents WHERE  report_group_id = @group_id AND report_id = @report_id)
            INSERT INTO Report_Group_Contents (report_group_id,report_id) VALUES (@group_id, @report_id)
    END 

END

GO

-- *****************************************************************************
-- * Issue:  Renewals Back Office Changes - 111SR2
-- * Author: Amit Kumar
-- * Date:   13/02/2008
-- *****************************************************************************

    -- Underwriting upgrade code goes here
IF EXISTS (SELECT NULL FROM pmproduct_lookup WHERE lookup_table_name = 'contact_type')
   BEGIN

	DECLARE @caption_id int
	IF NOT EXISTS(SELECT NULL from contact_type WHERE code = 'MEMAIL') 
	  BEGIN
		DECLARE @contact_type_id int

		SELECT @contact_type_id = ISNULL(MAX(contact_type_id),0)+1 from contact_type
       -- Get caption
        EXEC spu_pm_caption_id_return 1, 'Main Email Contact', @caption_id OUTPUT
 
        -- Add new new contact type
        INSERT INTO contact_type(contact_type_id, caption_id, code, description, is_deleted, effective_date, is_contact_type, is_correspondence_type )
            VALUES(@contact_type_id, @caption_id, 'MEMAIL','Main Email Contact', 0, getdate(),1,1 )
	  END


	IF NOT EXISTS(SELECT NULL from Document_Type WHERE code = 'EMAIL') 
	  BEGIN
		DECLARE @document_type_id int

		SELECT @document_type_id = ISNULL(MAX(document_type_id),0)+1 from document_type
       -- Get caption
        EXEC spu_pm_caption_id_return 1, 'Email', @caption_id OUTPUT
 
        -- Add new new documet type
        INSERT INTO document_type(document_type_id, caption_id, code, description, is_deleted, effective_date, is_editable_after_merging )
            VALUES(@document_type_id, @caption_id, 'EMAIL','Email', 0, getdate(),0 )
	  END
      IF NOT EXISTS(SELECT NULL from contact_type WHERE code = 'HOMEPHONE') 
	  BEGIN
		SELECT @contact_type_id = ISNULL(MAX(contact_type_id),0)+1 from contact_type
       -- Get caption
        EXEC spu_pm_caption_id_return 1, 'HOME PHONE', @caption_id OUTPUT

        -- Add new new contact type
        INSERT INTO contact_type(contact_type_id, caption_id, code, description, is_deleted, effective_date, is_contact_type, is_correspondence_type )
            VALUES(@contact_type_id, @caption_id, 'HOMEPHONE','Home Phone', 0, getdate(),1,1 )
END 
END 
GO

-- *****************************************************************************  
-- * Author:       Deepak Mittal
-- * Date:         14-02-2008
-- * Purpose:      PVY London Market Development
-- *****************************************************************************
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN
   	IF NOT EXISTS (SELECT NULL FROM pmproduct_lookup WHERE lookup_table_name='Insurer_Type')
	BEGIN
		DECLARE @pmproduct_id INT
		DECLARE @caption_id INT
		SELECT @pmproduct_id = pmproduct_id FROM pmproduct WHERE code='SirSol'
		EXEC spu_pm_caption_id_return 1, 'Insurer_Type', @caption_id OUTPUT

		INSERT INTO pmproduct_lookup(pmproduct_id,lookup_table_name,edit_privilege_level,linked_caption_id,linked_class_name,linked_object_name,is_generic_maintenance,interface_component,interface_control)
		VALUES(@pmproduct_id,'Insurer_Type',3,@caption_id,NULL,NULL,0,NULL,NULL)
	END
END
GO

IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN
	IF NOT EXISTS (SELECT NULL FROM party WHERE shortname = 'MULTILM')
	BEGIN
		DECLARE @party_id int
		SELECT @party_id = MAX(party_id) + 1 FROM party
		INSERT INTO party(party_type_id,party_structure_id,is_also_agent,source_id,party_id,shortname,name,resolved_name,
		currency_id,language_id,collect_type_id,accum_treatment_type_id,stats_treatment_type_id,agent_cnt,created_by_id,
		party_category_id,date_created,last_modified,consultant_cnt,modified_by_id,payment_method_code,payment_term_code,
		credit_card_code,file_code,abc_count,statements,reminder_type_id,renewals,status,last_action_type,is_travel_agent,
		is_prospect,is_deleted,abi_code_on_406,abi_code_on_81,abi_codelist,area_id,service_level_id,invariant_key,
		record_status,CCJs,user_defined_data_id,seasonal_gift_id,correspondence_type_id,renewal_stop_code_id,
		swift_party_id,loyalty_number,alternative_identifier,marketing_segment_ind,trading_name,sub_branch_id,tob_letter,
		tax_number,domiciled_for_tax,tax_exempt,tax_percentage,blacklist_reason_id)
		VALUES
		(7,1,0,1,@party_id,'MULTILM','Multi London Market','Multi London Market',
		26,1,NULL,NULL,NULL,NULL,1,
		NULL,'2007-01-01',NULL,NULL,NULL,'Cash',NULL,
		NULL,NULL,NULL,0,NULL,0,NULL,NULL,0,
		0,0,NULL,999,NULL,NULL,0,0,
		NULL,0,NULL,NULL,NULL,NULL,
		NULL,NULL,NULL,NULL,NULL,1,'1899-12-30',
		NULL,1,0,NULL,NULL)
	END
END
GO

IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN
	IF NOT EXISTS (SELECT NULL FROM party_insurer WHERE party_cnt IN(SELECT party_cnt FROM party WHERE shortname = 'MULTILM'))
	BEGIN
		DECLARE @party_cnt int
		SELECT @party_cnt = party_cnt FROM party where shortname = 'MULTILM'
		INSERT INTO party_insurer(party_cnt,agency_number,binder_indicator,report_indicator,
		is_reinsurer,reinsurance_type,is_reinsurance_debit_credit_no,default_comm_rate,
		method,icr,polaris_insurer_no,abi_1_edi_directory,payment_method,payment_frequency,
		bank_account,fsa_registration_number,fsa_insurercreditrating_id,fsa_insurerstatus_id,
		is_retained,tax_group_id,claims_rating_agency_id,claims_rating_grading,claims_rating_date,
		claims_rating_description,terms_of_payment_id,domiciled_for_tax,risk_transfer_agreement,
		Brokerlink_Subaccount,Brokerlink_UW_ID,Insurer_locking_type_id,BureauAccountParty,
		risk_transfer_editable,insurer_type_id)
		VALUES
		(@party_cnt,99999,0,0,
		0,0,0,0.0,
		NULL,NULL,NULL,NULL,NULL,NULL,
		NULL,NULL,0,0,
		0,NULL,NULL,NULL,NULL,
		NULL,0,1,1,
		NULL,NULL,1,NULL,NULL,3
		)
	END
END
GO

IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'A')
	BEGIN
	IF NOT EXISTS (SELECT NULL FROM party_address_usage WHERE party_cnt IN(SELECT party_cnt FROM party WHERE shortname = 'MULTILM'))
	BEGIN
	DECLARE @party_cnt int
	SELECT @party_cnt = party_cnt FROM party WHERE shortname = 'MULTILM'

	INSERT INTO 
	party_address_usage
	(party_cnt,address_cnt,description,address_usage_type_id,risk_group_id)
	VALUES
	(@party_cnt,1,NULL,4,0)
	END
END
GO

IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN
	IF NOT EXISTS (SELECT NULL FROM account WHERE account_key IN(SELECT party_cnt FROM party WHERE shortname = 'MULTILM'))
	BEGIN
	DECLARE @party_cnt int
	SELECT @party_cnt = party_cnt FROM party WHERE shortname = 'MULTILM'
	INSERT INTO account
	(purgefrequency_id,accountstatus_id,company_id,sub_branch_id,currency_id,accounttype_id,paymenttype_id,ledger_id,
	account_name,short_code,restrict_enquiry,restrict_update,delete_at_purge,contact_name,address1,address2,address3,
	address4,postal_code,address_country,phone_area_code,phone_number,phone_extension,fax_area_code,fax_number,
	fax_extension,payment_name,payment_account_code,payment_branch_code,payment_expiry_date,payment_reference1,
	payment_reference2,credit_limit,discount_percentage,settlement_period,bank_name,bank_address1,bank_address2,
	bank_address3,bank_address4,bank_postal_code,bank_country,bank_phone_area_code,bank_phone_number,bank_phone_extension,
	bank_fax_area_code,bank_fax_number,bank_fax_extension,comments,account_key,nominal_account_id,prooflist_report_id,
	bordereau_report_id,allow_electronic_payment,client_money_calc_account_type,client_bank_account_type)
	VALUES
	(1,1,1,1,26,4,NULL,4,
	'Multi London Market','MULTILM',0,0,0,NULL,'Internal Use Only',NULL,NULL,
	NULL,'X9 9XX',1,NULL,NULL,NULL,NULL,NULL,
	NULL,NULL,NULL,NULL,NULL,NULL
	,NULL,0.0000,0.0,0,NULL,NULL,NULL
	,NULL,NULL,NULL,NULL,NULL,NULL,NULL
	,NULL,NULL,NULL,NULL,@party_cnt,0,NULL
	,NULL,0,0,NULL)
	END
END
GO



-- *****************************************************************************  
-- * Author:       Deepak Mittal
-- * Date:         14-02-2008
-- * Purpose:      PVY London Market Development Merge Fields
-- *****************************************************************************
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN
	-- For Section 6.1 of Tech Spec
	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='TotalWrittenPercentage')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('TotalWrittenPercentage','spu_wp_LinePercentage','total_written_percentage',16,'Debits','London Market','Line Detail','Total Written Percentage',1,NULL,9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='TotalSignedPercentage')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('TotalSignedPercentage','spu_wp_LinePercentage','total_signed_percentage',16,'Debits','London Market','Line Detail','Total Signed Percentage',1,NULL,9)
	END

	-- 6.1 END

	-- For Section 6.2 of Tech Spec

	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='LineCoinsurerName')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('LineCoinsurerName','spu_wp_debitCoinsurer','CoinsurerName',0,'Debits','London Market','Line Detail Breakdown','Insurer',1,'DebitCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='LineBureauName')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('LineBureauName','spu_wp_debitCoinsurer','Bureau',0,'Debits','London Market','Line Detail Breakdown','Bureau',1,'DebitCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='WrittenLinePercentage')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('WrittenLinePercentage','spu_wp_debitCoinsurer','WrittenLinePerc',16,'Debits','London Market','Line Detail Breakdown','Written Line Percentage',1,'DebitCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='LineStand')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('LineStand','spu_wp_debitCoinsurer','LineStands',0,'Debits','London Market','Line Detail Breakdown','Line Stands',1,'DebitCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='SignedLinePercentage')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('SignedLinePercentage','spu_wp_debitCoinsurer','SignedLinePerc',16,'Debits','London Market','Line Detail Breakdown','Signed Line Percentage',1,'DebitCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='SignedLinePremiumIncTax')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('SignedLinePremiumIncTax','spu_wp_debitCoinsurer','CoinsurerPremiumIncTax',11,'Debits','London Market','Line Detail Breakdown','Signed Line Premium Including Tax',1,'DebitCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='SignedLinePremiumTax')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('SignedLinePremiumTax','spu_wp_debitCoinsurer','CoinsurerPremiumTax',11,'Debits','London Market','Line Detail Breakdown','Signed Line Tax Value',1,'DebitCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='SignedLinePremiumExcTax')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('SignedLinePremiumExcTax','spu_wp_debitCoinsurer','CoinsurerPremiumExcTax',11,'Debits','London Market','Line Detail Breakdown','Signed Line Premium Excluding Tax',1,'DebitCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='SignedLineCommValue')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('SignedLineCommValue','spu_wp_debitCoinsurer','SignedLineComm',11,'Debits','London Market','Line Detail Breakdown','Signed Line Commission Value',1,'DebitCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='ISLeadUnderwriter')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('ISLeadUnderwriter','spu_wp_debitCoinsurer','LeadUnderwriter',0,'Debits','London Market','Line Detail Breakdown','Lead Underwriter',1,'DebitCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicyNumber')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('PolicyNumber','spu_wp_debitCoinsurer','CoinsurerPolicyNumber',0,'Debits','London Market','Line Detail Breakdown','Policy Number',1,'DebitCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='SignedLineCommissionCharge')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('SignedLineCommissionCharge','spu_wp_debitCoinsurer','SignedLineCommCharge',11,'Debits','London Market','Line Detail Breakdown','Line Detail Commission Charge',1,'DebitCoinsurer',9)
	END

	-- 6.2 END

	-- For Section 6.3 of Tech Spec

	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='LineDetailSectionTaxGroup')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,product_family)
		VALUES('LineDetailSectionTaxGroup','spu_wp_debitSectionCoinsurer','SectionCoinsurerPremiumTaxGroup',0,'Debits','London Market','Line Detail Breakdown','Section Detail','Line Detail Section Tax Group',1,'DebitSectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='LineDetailSectionTaxRate')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,product_family)
		VALUES('LineDetailSectionTaxRate','spu_wp_debitSectionCoinsurer','SectionTaxRate',16,'Debits','London Market','Line Detail Breakdown','Section Detail','Line Detail Section Tax Rate',1,'DebitSectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='LineDetailSectionName')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,product_family)
		VALUES('LineDetailSectionName','spu_wp_debitSectionCoinsurer','SectionName',0,'Debits','London Market','Line Detail Breakdown','Section Detail','Line Detail Section Name',1,'DebitSectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='LineDetailSectionApplied')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,product_family)
		VALUES('LineDetailSectionApplied','spu_wp_debitSectionCoinsurer','SectionApplied',0,'Debits','London Market','Line Detail Breakdown','Section Detail','Line Detail Section Applied',1,'DebitSectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='LineDetailSectionCommPerc')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,product_family)
		VALUES('LineDetailSectionCommPerc','spu_wp_debitSectionCoinsurer','SectionCoinsurerCommissionPerc',16,'Debits','London Market','Line Detail Breakdown','Section Detail','Line Detail Section Commission Percentage',1,'DebitSectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='LineDetailSectionCommCharge')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,product_family)
		VALUES('LineDetailSectionCommCharge','spu_wp_debitSectionCoinsurer','SectionCoinsurerCommissionCharge',11,'Debits','London Market','Line Detail Breakdown','Section Detail','Line Detail Section Commission Charge',1,'DebitSectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='SignedLineSectionPremiumIncTax')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,product_family)
		VALUES('SignedLineSectionPremiumIncTax','spu_wp_debitSectionCoinsurer','SectionCoinsurerPremiumIncTax',11,'Debits','London Market','Line Detail Breakdown','Section Detail','Signed Line Section Premium Including Tax',1,'DebitSectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='SignedLineSectionPremiumExcTax')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,product_family)
		VALUES('SignedLineSectionPremiumExcTax','spu_wp_debitSectionCoinsurer','SectionCoinsurerPremiumExcTax',11,'Debits','London Market','Line Detail Breakdown','Section Detail','Signed Line Section Premium Excluding Tax',1,'DebitSectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='SignedLineSectionTaxValue')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,product_family)
		VALUES('SignedLineSectionTaxValue','spu_wp_debitSectionCoinsurer','SectionCoinsurerPremiumTax',11,'Debits','London Market','Line Detail Breakdown','Section Detail','Signed Line Section Tax Value',1,'DebitSectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='WrittenLineSectionPercentage')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,product_family)
		VALUES('WrittenLineSectionPercentage','spu_wp_debitSectionCoinsurer','WrittenLinePerc',16,'Debits','London Market','Line Detail Breakdown','Section Detail','Written Line Section Percentage',1,'DebitSectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='SignedLineSectionPercentage')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,product_family)
		VALUES('SignedLineSectionPercentage','spu_wp_debitSectionCoinsurer','WrittenLinePerc',16,'Debits','London Market','Line Detail Breakdown','Section Detail','Signed Line Section Percentage',1,'DebitSectionCoinsurer',9)
	END


	-- 6.3 END

	-- For Section 6.4 of Tech Spec

	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='TotalPolicyWrittenPercentage')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('TotalPolicyWrittenPercentage','spu_wp_PolicyLinePercentage','policy_total_written_percentage',16,'Policy','London Market','Line Detail','Total Written Percentage',1,NULL,9)
	END

	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='TotalPolicySignedPercentage')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('TotalPolicySignedPercentage','spu_wp_PolicyLinePercentage','policy_total_signed_percentage',16,'Policy','London Market','Line Detail','Total Signed Percentage',1,NULL,9)
	END


	-- 6.4 END

	-- For Section 6.5 of Tech Spec

	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicyLineCoinsurerName')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('PolicyLineCoinsurerName','spu_wp_coinsurer','insurer_name',0,'Policy','London Market','Line Detail Breakdown','Insurer',1,'coinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicyLineBureauName')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('PolicyLineBureauName','spu_wp_coinsurer','bureau_name',0,'Policy','London Market','Line Detail Breakdown','Bureau',1,'coinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicyWrittenLinePercentage')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('PolicyWrittenLinePercentage','spu_wp_coinsurer','written_line_percentage',16,'Policy','London Market','Line Detail Breakdown','Written Line Percentage',1,'coinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicyLineStand')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('PolicyLineStand','spu_wp_coinsurer','line_stand',0,'Policy','London Market','Line Detail Breakdown','Line Stands',1,'coinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicySignedLinePercentage')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('PolicySignedLinePercentage','spu_wp_coinsurer','signed_line_percentage',16,'Policy','London Market','Line Detail Breakdown','Signed Line Percentage',1,'coinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicySignedLinePremiumIncTax')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('PolicySignedLinePremiumIncTax','spu_wp_coinsurer','coinsurer_value_incl',11,'Policy','London Market','Line Detail Breakdown','Signed Line Premium Including Tax',1,'coinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicySignedLinePremiumTax')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('PolicySignedLinePremiumTax','spu_wp_coinsurer','coinsurer_ipt_amount',11,'Policy','London Market','Line Detail Breakdown','Signed Line Tax Value',1,'coinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicySignedLinePremiumExcTax')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('PolicySignedLinePremiumExcTax','spu_wp_coinsurer','coinsurer_value',11,'Policy','London Market','Line Detail Breakdown','Signed Line Premium Excluding Tax',1,'coinsurer',9)
	END



	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicySignedLineCommValue')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('PolicySignedLineCommValue','spu_wp_coinsurer','coinsurer_commission_amount',11,'Policy','London Market','Line Detail Breakdown','Signed Line Commission Value',1,'coinsurer',9)
	END



	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicyISLeadUnderwriter')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('PolicyISLeadUnderwriter','spu_wp_coinsurer','is_lead_underwriter',0,'Policy','London Market','Line Detail Breakdown','Lead Underwriter',1,'coinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicyRefNumber')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('PolicyRefNumber','spu_wp_coinsurer','policy_number',0,'Policy','London Market','Line Detail Breakdown','Policy Number',1,'coinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicySignedLineCommissionCharge')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,display_name,is_displayed,loop1,product_family)
		VALUES('PolicySignedLineCommissionCharge','spu_wp_coinsurer','commission_charge',11,'Policy','London Market','Line Detail Breakdown','Line Detail Commission Charge',1,'coinsurer',9)
	END


	-- 6.5 END

	-- For Section 6.6 of Tech Spec

	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicyLineDetailSectionTaxGroup')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,loop2,product_family)
		VALUES('PolicyLineDetailSectionTaxGroup','spu_wp_policysectionCoinsurer','TaxGroup',0,'Policy','London Market','Line Detail Breakdown','Section Detail','Line Detail Section Tax Group',1,'PolicySection','policysectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicyLineDetailSectionTaxRate')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,loop2,product_family)
		VALUES('PolicyLineDetailSectionTaxRate','spu_wp_policysectionCoinsurer','SectionTaxRate',16,'Policy','London Market','Line Detail Breakdown','Section Detail','Line Detail Section Tax Rate',1,'PolicySection','policysectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicyLineDetailSectionName')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,loop2,product_family)
		VALUES('PolicyLineDetailSectionName','spu_wp_policysectionCoinsurer','SectionName',0,'Policy','London Market','Line Detail Breakdown','Section Detail','Line Detail Section Name',1,'PolicySection','policysectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicyLineDetailSectionApplied')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,loop2,product_family)
		VALUES('PolicyLineDetailSectionApplied','spu_wp_policysectionCoinsurer','IsApplied',0,'Policy','London Market','Line Detail Breakdown','Section Detail','Line Detail Section Applied',1,'PolicySection','policysectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicyLineDetailSectionCommPerc')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,loop2,product_family)
		VALUES('PolicyLineDetailSectionCommPerc','spu_wp_policysectionCoinsurer','CommissionPercent',16,'Policy','London Market','Line Detail Breakdown','Section Detail','Line Detail Section Commission Percentage',1,'PolicySection','policysectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicyLineDetailSectionCommCharge')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,loop2,product_family)
		VALUES('PolicyLineDetailSectionCommCharge','spu_wp_policysectionCoinsurer','CommissionCharge',11,'Policy','London Market','Line Detail Breakdown','Section Detail','Line Detail Section Commission Charge',1,'PolicySection','policysectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicySignedLineSectionPremiumIncTax')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,loop2,product_family)
		VALUES('PolicySignedLineSectionPremiumIncTax','spu_wp_policysectionCoinsurer','PremiumIncTax',11,'Policy','London Market','Line Detail Breakdown','Section Detail','Signed Line Section Premium Including Tax',1,'PolicySection','policysectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicySignedLineSectionPremiumExcTax')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,loop2,product_family)
		VALUES('PolicySignedLineSectionPremiumExcTax','spu_wp_policysectionCoinsurer','PremiumExcTax',11,'Policy','London Market','Line Detail Breakdown','Section Detail','Signed Line Section Premium Excluding Tax',1,'PolicySection','policysectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicySignedLineSectionTaxValue')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,loop2,product_family)
		VALUES('PolicySignedLineSectionTaxValue','spu_wp_policysectionCoinsurer','TaxValue',11,'Policy','London Market','Line Detail Breakdown','Section Detail','Signed Line Section Tax Value',1,'PolicySection','policysectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicyWrittenLineSectionPercentage')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,loop2,product_family)
		VALUES('PolicyWrittenLineSectionPercentage','spu_wp_policysectionCoinsurer','WrittenLinePerc',16,'Policy','London Market','Line Detail Breakdown','Section Detail','Written Line Section Percentage',1,'PolicySection','policysectionCoinsurer',9)
	END


	IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PolicySignedLineSectionPercentage')
	BEGIN
		INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2,sub_group3,display_name,is_displayed,loop1,loop2,product_family)
		VALUES('PolicySignedLineSectionPercentage','spu_wp_policysectionCoinsurer','SignedLinePerc',16,'Policy','London Market','Line Detail Breakdown','Section Detail','Signed Line Section Percentage',1,'PolicySection','policysectionCoinsurer',9)
	END


	-- 6.6 END

END		

-- *****************************************************************************  
-- * Date:         28-05-2019
-- * Purpose:      RI address line tags
-- *****************************************************************************
	IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_ReinsuranceFAC' AND Field_name='FACAddress1')
	BEGIN 
		INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family, Table_Name )
		VALUES ('FACAddress1','spu_wp_ReinsuranceFAC','fac_address1','0','Reinsurance','Fac','FAC Address1','1','9','CoreReinsuranceFAC')
	END
	GO

	IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_ReinsuranceFAC' AND Field_name='FACAddress2')
	BEGIN 
		INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family, Table_Name )
		VALUES ('FACAddress2','spu_wp_ReinsuranceFAC','fac_address2','0','Reinsurance','Fac','FAC Address2','1','9','CoreReinsuranceFAC')
	END
	GO

	IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_ReinsuranceFAC' AND Field_name='FACAddress3')
	BEGIN 
		INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family, Table_Name )
		VALUES ('FACAddress3','spu_wp_ReinsuranceFAC','fac_address3','0','Reinsurance','Fac','FAC Address3','1','9','CoreReinsuranceFAC')
	END
	GO

	IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_ReinsuranceFAC' AND Field_name='FACAddress4')
	BEGIN 
		INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family, Table_Name )
		VALUES ('FACAddress4','spu_wp_ReinsuranceFAC','fac_address4','0','Reinsurance','Fac','FAC Address4','1','9','CoreReinsuranceFAC')
	END
	GO

	IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_ReinsuranceFAC' AND Field_name='FACPostalCode')
	BEGIN 
		INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family, Table_Name )
		VALUES ('FACPostalCode','spu_wp_ReinsuranceFAC','fac_postal_code','0','Reinsurance','Fac','FAC postal code','1','9','CoreReinsuranceFAC')
	END
	GO	

-- *****************************************************************************  
-- * Author:       Deepak Mittal
-- * Date:         14-02-2008
-- * Purpose:      PVY London Market Development default value for system option
-- *****************************************************************************
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN
	
	UPDATE 
		system_options 
		SET value ='1' 
		WHERE option_number = 6002 
		AND value IS NULL
END
GO

-- *****************************************************************************
-- * Author:   Shankh Dhar Dubey	 
-- * Date:     15/02/2008
-- * Purpose:  PN41178 - default values for claim_party entries
-- *****************************************************************************
IF EXISTS (SELECT null FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN
	IF NOT EXISTS(SELECT null FROM claim_party_link WHERE peril_type_id IS NOT NULL)
	BEGIN
		DECLARE @claim_id int
		DECLARE @peril_type_id int

		DECLARE CLAIM_PARTY_CURSOR CURSOR FORWARD_ONLY FOR SELECT claim_id FROM claim_party_link

		OPEN CLAIM_PARTY_CURSOR

		FETCH NEXT FROM CLAIM_PARTY_CURSOR INTO @claim_id

		WHILE @@FETCH_STATUS=0
		BEGIN
			SELECT TOP 1 @peril_type_id=peril_type_id FROM claim_peril WHERE claim_id=@claim_id
			UPDATE claim_party_link SET peril_type_id=@peril_type_id WHERE CURRENT OF CLAIM_PARTY_CURSOR
			FETCH NEXT FROM CLAIM_PARTY_CURSOR INTO @claim_id
		END

		CLOSE CLAIM_PARTY_CURSOR
		DEALLOCATE CLAIM_PARTY_CURSOR
	END
END
GO

-- *****************************************************************************
-- * Author:   Shankh Dhar Dubey	 
-- * Date:     15/02/2008
-- * Purpose:  PN41178 - Claim Peril merge fields
-- *****************************************************************************

IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPDatePassedTest')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPDatePassedTest','spu_wp_clmperilotherparty','Other_Party_Date_Passed_Test',4,'Claim','Perils','Date Passed Test',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)
	
IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPContactName')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPContactName','spu_wp_clmperilotherparty','Other_Party_Contact_Name',0,'Claim','Perils','Contact Name',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)
	
IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPContactTelNo')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPContactTelNo','spu_wp_clmperilotherparty','Other_Party_Contact_Telephone_Number',0,'Claim','Perils','Contact Telephone No',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)
	
IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPInsurerName')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPInsurerName','spu_wp_clmperilotherparty','Other_Party_Insurer_Name',0,'Claim','Perils','Insurer Name',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)
	
IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPInsurerAddress1')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPInsurerAddress1','spu_wp_clmperilotherparty','Other_Party_Insurer_Address_1',0,'Claim','Perils','Insurer Address 1',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)
	
IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPInsurerAddress2')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPInsurerAddress2','spu_wp_clmperilotherparty','Other_Party_Insurer_Address_2',0,'Claim','Perils','Insurer Address 2',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)
	
IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPInsurerAddress3')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPInsurerAddress3','spu_wp_clmperilotherparty','Other_Party_Insurer_Address_3',0,'Claim','Perils','Insurer Address 3',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)
	
IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPInsurerAddress4')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPInsurerAddress4','spu_wp_clmperilotherparty','Other_Party_Insurer_Address_4',0,'Claim','Perils','Insurer Address 4',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)
	
IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPInsurerPostCode')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPInsurerPostCode','spu_wp_clmperilotherparty','Other_Party_Insurer_PostCode',0,'Claim','Perils','Insurer PostCode',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)
	
IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPInsurerTelNo')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPInsurerTelNo','spu_wp_clmperilotherparty','Other_Party_Insurer_Telephone_Number',0,'Claim','Perils','Insurer Telephone No',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)
	
IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPInsurerFaxNo')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPInsurerFaxNo','spu_wp_clmperilotherparty','Other_Party_Insurer_Fax_Number',0,'Claim','Perils','Insurer Fax No',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)
	
IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPInsurerContactName')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPInsurerContactName','spu_wp_clmperilotherparty','Other_Party_Insurer_Contact_Name',0,'Claim','Perils','Insurer Contact Name',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)
	
IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPInsurerEmail')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPInsurerEmail','spu_wp_clmperilotherparty','Other_Party_Insurer_Email',0,'Claim','Perils','Insurer Email',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)

IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPInsurerNotes')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPInsurerNotes','spu_wp_clmperilotherparty','Other_Party_Insurer_Notes',0,'Claim','Perils','Insurer Notes',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)

IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPCompanyNotes')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPCompanyNotes','spu_wp_clmperilotherparty','Other_Party_Company_Notes',0,'Claim','Perils','Company Notes',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)

IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPReferenceNumber')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPReferenceNumber','spu_wp_clmperilotherparty','Other_Party_Reference_Number',0,'Claim','Perils','Reference',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)

IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPPartyType')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPPartyType','spu_wp_clmperilotherparty','Other_Party_Party_Type',0,'Claim','Perils','Party Type',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)

IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPAddress1')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPAddress1','spu_wp_clmperilotherparty','Other_Party_Address_1',0,'Claim','Perils','Address 1',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)
	
IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPAddress2')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPAddress2','spu_wp_clmperilotherparty','Other_Party_Address_2',0,'Claim','Perils','Address 2',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)
	
IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPAddress3')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPAddress3','spu_wp_clmperilotherparty','Other_Party_Address_3',0,'Claim','Perils','Address 3',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)
	
IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPAddress4')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPAddress4','spu_wp_clmperilotherparty','Other_Party_Address_4',0,'Claim','Perils','Address 4',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)
	
IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPPostCode')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPPostCode','spu_wp_clmperilotherparty','Other_Party_PostCode',0,'Claim','Perils','PostCode',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)

IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPDateOfBirth')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPDateOfBirth','spu_wp_clmperilotherparty','Other_Party_Date_Of_Birth',4,'Claim','Perils','Date of Birth',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)

IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPName')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPName','spu_wp_clmperilotherparty','Other_Party_Name',0,'Claim','Perils','Name',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)

IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPRegNo')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPRegNo','spu_wp_clmperilotherparty','Other_Party_Reg_Number',0,'Claim','Perils','Registration Number',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)

IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPPhoneNumber')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPPhoneNumber','spu_wp_clmperilotherparty','Other_Party_Phone_Number',0,'Claim','Perils','Phone Number',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)

IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name='ClmPerilOPSex')
	INSERT INTO wp_fields
	(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4)
	VALUES
	('ClmPerilOPSex','spu_wp_clmperilotherparty','Other_Party_Sex',0,'Claim','Perils','Sex',1,'clmperilotherparty',NULL,NULL,9,NULL,NULL,'Other Parties',NULL,NULL,NULL,NULL,NULL)
GO

-- *****************************************************************************
-- * Author  : Daniel Morey
-- * Date    : 19/02/2008       
-- * Purpose : Move the end date of pf schemes back.
-- *****************************************************************************
UPDATE pfrf
SET enddate = '2020-01-01'
WHERE enddate = '2008-01-01'

UPDATE pfscheme
SET enddate = '2020-01-01'
WHERE enddate = '2008-01-01'

GO

-- *****************************************************************************
-- * Author  : Deepak arora
-- * Date    : 28/02/2008       
-- * Purpose : LOAWR22i - SMS Support
-- *****************************************************************************
IF Not Exists (Select 1 From Event_Type_Group Where Code = 'MESSAGE')
BEGIN
    Declare @lCaptionID integer
    Declare @Event_type_GroupId Integer

    Execute spu_pm_caption_id_return 1, 'MESSAGE', @lCaptionID output
    
    Select @Event_type_GroupId = Max(Event_type_Group_Id)+1 From Event_type_Group

    Insert into Event_Type_Group 
    (
        event_type_group_id,
        caption_id,
        code,
        description,
        is_deleted,
        effective_date
    )
    Values
    (
        @Event_type_GroupId,
        @lCaptionID,
        'MESSAGE',
        'Message',
        0,
        GetDate()
    )

    Declare @Event_type_Id Integer
    Execute spu_pm_caption_id_return 1, 'SMS', @lCaptionID output
    
    Select @Event_type_Id = Max(Event_type_Id)+1 From Event_type
    
    Insert into Event_Type
    (
        event_type_id,
        caption_id,
        code,
        description,
        is_deleted,
        effective_date,
        Event_type_Group_Id
    )
    Values
    (
        @Event_type_Id,
        @lCaptionID,
        'SMS',
        'SMS Message',
        0,
        GetDate(),
        @Event_type_GroupId
    )
    END

GO


IF Not Exists (Select 1 From Batch_Type Where Code = 'MSGX')
BEGIN
    Declare @lCaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'MSGX', @lCaptionID output
    
    Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type
    
    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @lCaptionID,
        0,
        GetDate(),
        'Message Export',
        'MSGX'
    )
END
GO
	
-- *****************************************************************************
-- * Author:   Deepak Arora	 
-- * Date:     10-03-2008
-- * Purpose:  Claims Payment Enhancement
-- *****************************************************************************	
DECLARE @run_authorisation_scripts tinyint 

IF EXISTS (SELECT 1 FROM Hidden_options Where Option_Number=51)
BEGIN
     Select @run_authorisation_scripts= ISNULL(Value,0) FROM Hidden_options Where Option_Number=51
     Update Product Set run_authorisation_scripts_claim_payments = @run_authorisation_scripts 
     WHERE run_authorisation_scripts_claim_Payments IS NULL
     
     Delete From Hidden_options Where Option_Number=51
END


-- *****************************************************************************
-- * Author:   Aaron Rhodes	 
-- * Date:     28-11-07
-- * Purpose:  Adds PCLPSG as a new premium finance connectivity scheme
-- *****************************************************************************
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN

    DECLARE @PartyCnt INT
    DECLARE @PartyID INT
    DECLARE @AccountID INT
    DECLARE @ParentMappingID INT
    DECLARE @ParentNodeID INT
    DECLARE @ElementID INT
    DECLARE @SchemeNo INT
    DECLARE @SchemeVersion INT
    DECLARE @MediaTypeId INT
    DECLARE @SchemeTypeID INT
    DECLARE @SchemePrintTypeID INT
    DECLARE @FrequencyID INT
    DECLARE @PFEDIID int
    DECLARE @AddressCnt as int
    DECLARE @AddressId as int
    DECLARE @lCaptionID integer
    
    ---SQL Script for adding in Premium Credi Personal
    IF NOT EXISTS(SELECT NULL FROM pfscheme WHERE xsl_code = 'PCLPSG')
        BEGIN
    
            BEGIN TRANSACTION
    		
            SELECT	@PartyCnt = party_cnt
            FROM	party
            WHERE	shortname = 'PCLSG'
    
            SELECT	@AccountID = account_id
            FROM	account
            WHERE	short_code = 'PCLSG'
    
            --find mediatype_id for Direct Debit Mandate.
            SET @MediaTypeId = (SELECT MediaType_id FROM MediaType WHERE code = 'DD')
    
            --find the scheme type ID for third party schemes
            SET @SchemeTypeID = (SELECT pfscheme_type_id FROM pfscheme_type WHERE code = 'TPSG')
    
            --find the print type ID for no documents
            SET @SchemePrintTypeID = (SELECT pfscheme_printtype_id FROM pfscheme_printtype WHERE code = 'ND')
    
            --find next available scheme no and version no
            SET @SchemeNo = (isnull((SELECT MAX(SchemeNo) FROM PFScheme WHERE CompanyNo = 1),0) + 1)
            SET @SchemeVersion = (isnull((SELECT MAX(SchemeVersion) FROM PFScheme WHERE CompanyNo = 1),0) + 1)
    		
            --create PFScheme
            INSERT INTO PFScheme
            (companyno,schemeno,schemeversion,party_cnt,startdate,enddate,schemename,schemedescription,quoteableind,InsrMailBoxNo,
            EDImessagecount,immediatebankdetails,mediatype_id,pfscheme_type_id,pfscheme_printtype_id,spread_commission,
            currency_id,bank_name_mandatory,bank_address_mandatory,branch_name_mandatory,branch_code_mandatory,spread_taxes,
            spread_ri,deposit_as_instalment,deposit_on_other_media_type,agent_ref_mandatory, xsl_code, limittransactions, transactionlimit,
            pf_message, business_code_mandatory, receipt_difference_option, provider_website)
            VALUES
            (1,@SchemeNo,@SchemeVersion,@PartyCnt,'2002-01-01','2008-01-01','PCLPSG','PCLPSG','Y','',
            0,0,@MediaTypeID,@SchemeTypeID,@SchemePrintTypeID,0,26,1,1,1,1,0,0,0,0,0, 'PCLPSG', 1, 1, '', 0, 0, 'https://www.pclpls.com/services/xmltransferengine.asmx')
    
            --Now add in the PFRF records....
    
            --Find the frequency_id for a monthly scheme
            SET @FrequencyID = (SELECT pffrequency_id FROM pffrequency WHERE code = 'M')
    
            --The first is a Stargate dummy rate (live rates are retrieved remotely)
            INSERT INTO PFRF
            (CompanyNo,SchemeNo,SchemeVersion,StartDate,ProductFamily,ArrangementFee,Mnemonic,EndDate,Protect,
            DaysDelay,DepositReq,DepositPC,AllowProtection,ProtectRate,MinInterest,Min1,Max1,Rate1,R1Com,Min2,
            Max2,Rate2,R2Com,Min3,Max3,Rate3,R3Com,Min4,Max4,Rate4,R4Com,Min5,Max5,Rate5,R5Com,AlLowOveride,MinMTA,
            MinMTAInstalments,pffrequency_id,tax_charged_to,fee_type,fee_charged_to,protection_type,
            protection_charged_to,deposit_type,deposit_charged_to,backdated_rollup_to,align_to,start_limit,recollect_on_next,
            recollect_days,retry_limit,mta_on_next_instalment,advance_instalments,remainder_amount_threshhold,
            remainder_amount_at_end,maximum_instalments,existing_days_delay)
            VALUES
            (1,@SchemeNo,@SchemeVersion,'2002-01-01','SG',0,'SG','2008-01-01','N',14,'N',
            0,'N',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,'N',0,0,
            @FrequencyID,0,0,0,0,0,1,0,0,1,17,0,0,0,0,0,0,0,0,0)
    	
    	    --Now insert message headers for PCLP
    	    INSERT INTO pfmessageheader (xsl_code, header_name, header_value, request_type) VALUES ('PCLPSG', 'SOAPAction', 'www.PCL.co.uk/XMLTransferService/XMLTransferEngine/GetQuote', 0)
	    INSERT INTO pfmessageheader (xsl_code, header_name, header_value, request_type) VALUES ('PCLPSG', 'SOAPAction', 'www.PCL.co.uk/XMLTransferService/XMLTransferEngine/CreateNewBusiness', 1)
	    INSERT INTO pfmessageheader (xsl_code, header_name, header_value, request_type) VALUES ('PCLPSG', 'SOAPAction', 'www.PCL.co.uk/XMLTransferService/XMLTransferEngine/CreateMidTermAdjustment', 2)
	    INSERT INTO pfmessageheader (xsl_code, header_name, header_value, request_type) VALUES ('PCLPSG', 'SOAPAction', 'www.PCL.co.uk/XMLTransferService/XMLTransferEngine/CreateCancellation', 6)
	    INSERT INTO pfmessageheader (xsl_code, header_name, header_value, request_type) VALUES ('PCLPSG', 'SOAPAction', 'www.PCL.co.uk/XMLTransferService/XMLTransferEngine/CreateChangeOfAddress', 7)
	    INSERT INTO pfmessageheader (xsl_code, header_name, header_value, request_type) VALUES ('PCLPSG', 'SOAPAction', 'www.PCL.co.uk/XMLTransferService/XMLTransferEngine/CreateChangeOfBankDetails', 8)
	    INSERT INTO pfmessageheader (xsl_code, header_name, header_value, request_type) VALUES ('PCLPSG', 'SOAPAction', 'www.PCL.co.uk/XMLTransferService/XMLTransferEngine/CreateChangeOfCollectionDate', 9)
	    INSERT INTO pfmessageheader (xsl_code, header_name, header_value, request_type) VALUES ('PCLPSG', 'SOAPAction', 'www.PCL.co.uk/XMLTransferService/XMLTransferEngine/CreateRenewal', 10)
    	
            COMMIT TRANSACTION
    END
END
GO

-- *****************************************************************************
-- * Author:   Gurucharan Gulati	 
-- * Date:     10/03/2008
-- * Purpose:  Maintain Party Numbering Scheme
-- *****************************************************************************

UPDATE Party_Type SET is_on_numbering_scheme = 1 
	WHERE code IN ('PC','GC','CC','IN','AH','CO','HC','AG','AGG','OTHERPARTY')

IF NOT EXISTS(SELECT NULL FROM numbering_scheme_type WHERE code = 'PARTY')
BEGIN
    DECLARE @lCaptionID INT
    DECLARE @numbering_scheme_type_id INT
    EXECUTE spu_pm_caption_id_return 1, 'Party Code', @lCaptionID OUTPUT
    SELECT  @numbering_scheme_type_id = MAX(numbering_scheme_type_id)+ 1 FROM numbering_scheme_type

    INSERT INTO numbering_scheme_type
    (numbering_scheme_type_id,caption_id,code,description,is_deleted,effective_date)
    VALUES
    (@numbering_scheme_type_id,@lCaptionID,'PARTY','Party Code',0,getdate()) 
END
GO


-- *****************************************************************************
-- * Author:        Pankaj Kaushik
-- * Date:          06-4-03-2008
-- * Purpose:       Renewal Printing
-- *****************************************************************************
 
   --Deleting Existing Process Types which are for S4B only
    IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
    BEGIN
        DELETE FROM pmb_doc_link where Process_Type_Id in (
	SELECT Process_type_id FROM process_type WHERE code in('MTA_Q','PRERENSEL','RENSEL','REN_Q')
	)
	DELETE FROM process_type where code in ('MTA_Q','PRERENSEL','RENSEL','REN_Q')
    END

    IF NOT EXISTS (SELECT * FROM Process_Type) BEGIN

	DECLARE @caption_id int
 
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'NB', @caption_id OUTPUT

        -- Add new New Business Description
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging)
            VALUES(1, @caption_id, 'NB','New Business',0, getdate(),0)

        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'AP', @caption_id OUTPUT

        -- Add new New Business Description
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging)
            VALUES(2, @caption_id, 'AP','MTA Additional Premium',0, getdate(),0)

        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'RP', @caption_id OUTPUT

        -- Add new New Business Description
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging)
            VALUES(3, @caption_id, 'RP','MTA Return Premium',0, getdate(),0)

        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'ZP', @caption_id OUTPUT

        -- Add new New Business Description
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging)
            VALUES(4, @caption_id, 'ZP','MTA Zero Premium',0, getdate(),0)

        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'RI', @caption_id OUTPUT

        -- Add new New Business Description
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging)
            VALUES(5, @caption_id, 'RI','MTA Reinstatement',0, getdate(),0)

        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'RN', @caption_id OUTPUT

        -- Add new New Business Description
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging)
            VALUES(6, @caption_id, 'RN','Renewal',0, getdate(),0)
    END
    GO

--**********************************************************************************************  
-- Author : Pankaj Kaushik
--   
-- History: 12/03/2008    
--
-- Task : Renewal Printing
--**********************************************************************************************  

DECLARE @COUNT INT
SELECT @COUNT = COUNT(*) FROM PMB_Doc_Link
EXEC DDLDropTable 'tmpDocument_Template'
IF ISNULL(@COUNT,0) = 0
BEGIN
		DECLARE @bExists TINYINT
	EXECUTE @bExists = DDLExistsTable 'temp_PMBDocLink'
	IF @bExists = 0 BEGIN
		EXEC DDLDropTable temp_PMBDocLink
	END 
	
	--Create a temp table and set all relevant records (where source_id = 0) of table doucment_template 
	--for all source
	CREATE TABLE tmpDocument_Template(
		Document_Type_Id INT,  
		Document_Template_Id INT,  
		source_id INT,
		code varchar(40))  

	INSERT INTO tmpDocument_Template SELECT Document_Type_Id,Document_Template_Id,source_id,code 
		FROM document_template where source_id <> 0 AND is_deleted = 0
	
	Declare @tmp_source_id INT
	DECLARE	c_cursor SCROLL CURSOR FOR SELECT source_id FROM source WHERE is_deleted = 0
	OPEN c_cursor
	FETCH NEXT FROM c_cursor INTO @tmp_source_id
	WHILE @@FETCH_STATUS = 0
	BEGIN
	    INSERT INTO tmpDocument_Template SELECT Document_Type_Id,Document_Template_Id,@tmp_source_id,code 
		FROM document_template where source_id = 0 AND is_deleted = 0
	
		FETCH NEXT FROM c_cursor INTO @tmp_source_id
	END
	
	CLOSE 		c_cursor
	DEALLOCATE	c_cursor
	
	-- End table tmpDocument_Template
	
	
	EXEC DDLDropTable 'temp_PMBDocLink'
	
	CREATE TABLE temp_PMBDocLink(
		GIS_Scheme_Id INT,
		Process_Type_Id INT,  
		Document_Type_Id INT,  
		Document_Template_Id INT,  
		Agent_Cnt INT,
		spool_document TINYINT,  
		Auto_Archive_Document TINYINT,
		process_types_docs_id INT,  
		functional_area TINYINT,  
		product_id INT,  
		source_id INT,  
		is_client TINYINT,  
		is_agent TINYINT,  
		is_office TINYINT,  
		production_order TINYINT,
		report_pointer INT,
		document_pointer VARCHAR(20))
	
		DECLARE @PMB_Doc_Link_Id INT,
		@GIS_Scheme_Id INT,
		@Process_Type_Id INT,  
		@Document_Type_Id INT,  
		@Document_Template_Id INT,  
		@Agent_Cnt INT,
		@spool_document TINYINT,  
		@Auto_Archive_Document TINYINT,
		@process_types_docs_id INT,  
		@functional_area TINYINT,  
		@product_id INT,  
		@source_id INT,  
		@is_client TINYINT,  
		@is_agent TINYINT,  
		@is_office TINYINT,  
		@production_order TINYINT,
		@report_pointer INT,
	    @document_pointer VARCHAR(20)
		
	DECLARE	c_cursor SCROLL CURSOR FOR SELECT product_id,isnull(report_pointer,0) FROM product
	OPEN c_cursor
	FETCH NEXT FROM c_cursor INTO @product_id,@report_pointer
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	    INSERT into temp_PMBDocLink
	    SELECT 
	    	
	    1,  --GIS_Scheme_ID
	    CASE isnumeric(Left(dt.code,1)) --Process_Type_id
	            WHEN 1 THEN 
			(SELECT process_type_id FROM process_type where code = SUBSTRING(dt.code,5,2))
	  	    ELSE
			(SELECT process_type_id FROM process_type where code = SUBSTRING(dt.code,4,2))
		    END,
	    dt.document_type_id, --Document_Type_Id
	    dt.document_template_id, --Document_Template_ID
      	    NULL, --Agent_Cnt
	    1, --Spool_Document
	    0, --Auto_Archive_Document
	    CASE isnumeric(Left(dt.code,1)) --Process_Types_Docs_Id
	            WHEN 1 THEN 
			(SELECT process_types_docs_id FROM process_types_docs where code = SUBSTRING(dt.code,2,3))
	            ELSE
	                (SELECT process_types_docs_id FROM process_types_docs where code = SUBSTRING(dt.code,1,3))
	            END,
	    1, --Functional_Area
	    @product_id, --Product_Id
	    dt.source_id, -- Source_Id
	    CASE LEFT(dt.code,1)  --for is_client if first character is 1
	            WHEN '1' THEN 
					1
	            ELSE
	                0
		    END,
	    CASE LEFT(dt.code,1)  --for is_agent if first character is 2
	            WHEN '2' THEN 
					1
	            ELSE
	                0
		    END,
	    CASE LEFT(dt.code,1)  --for is_office if first character is 3
	            WHEN '3' THEN 
					1
	            ELSE
	                0
		    END,
	    1,  -- Production_Order
	    @report_pointer, --report_pointer as configured in product table
	    CASE ISNUMERIC(Left(dt.code,1))
	            WHEN 1 THEN 
				(select REPLACE(dt.code, substring(dt.code,1,6),''))
		    ELSE
				(select REPLACE(dt.code, substring(dt.code,1,5),''))
		    END
	
		from tmpDocument_Template AS dt
		FETCH NEXT FROM c_cursor INTO @product_id,@report_pointer
	END
	
	CLOSE 		c_cursor
	DEALLOCATE	c_cursor
	
	DELETE FROM temp_PMBDocLink WHERE process_type_id is NULL
	DELETE FROM temp_PMBDocLink WHERE process_types_docs_id is NULL
	DELETE FROM temp_PMBDocLink WHERE source_id = 0
	DELETE FROM temp_PMBDocLink WHERE document_pointer <> convert(varchar(10),report_pointer)
	
	DECLARE	c_cursor SCROLL CURSOR FOR SELECT * FROM temp_PMBDocLink
	OPEN c_cursor
	FETCH NEXT FROM c_cursor INTO 
		 @GIS_Scheme_Id,
		 @Process_Type_Id,  
		 @Document_Type_Id,  
		 @Document_Template_Id,  
		 @Agent_Cnt,
		 @spool_document,  
		 @Auto_Archive_Document,
		 @process_types_docs_id,  
		 @functional_area,  
		 @product_id,  
		 @source_id,  
		 @is_client,  
		 @is_agent,  
		 @is_office,  
		 @production_order,
		 @report_pointer,
	     @document_pointer

	
	WHILE	@@FETCH_STATUS = 0
	BEGIN
		
		INSERT INTO PMB_Doc_Link(
		     GIS_Scheme_Id,
		     Process_Type_Id,  
		     Document_Type_Id,  
		     Document_Template_Id,  
		     Agent_Cnt,
		     spool_document,  
		     Auto_Archive_Document,
		     process_types_docs_id,  
		     functional_area,  
		     product_id,  
		     source_id,  
		     is_client,  
		     is_agent,  
		     is_office,  
		     production_order)
		VALUES(
		     @GIS_Scheme_Id,
		     @Process_Type_Id,  
		     @Document_Type_Id,  
		     @Document_Template_Id,  
		     @Agent_Cnt,
		     @spool_document,  
		     @Auto_Archive_Document,
		     @process_types_docs_id,  
		     @functional_area,  
		     @product_id,  
		     @source_id,  
		     @is_client,  
		     @is_agent,  
		     @is_office,  
		     @production_order)
		FETCH NEXT FROM c_cursor INTO 
		     @GIS_Scheme_Id,
		     @Process_Type_Id,  
		     @Document_Type_Id,  
		     @Document_Template_Id,  
		     @Agent_Cnt,
		     @spool_document,  
		     @Auto_Archive_Document,
		     @process_types_docs_id,  
		     @functional_area,  
		     @product_id,  
		     @source_id,  
		     @is_client,  
		     @is_agent,  
		     @is_office,  
		     @production_order,
			 @report_pointer,
	    	 @document_pointer
	END
	CLOSE 		c_cursor
	DEALLOCATE	c_cursor
	EXEC DDLDropTable temp_PMBDocLink
	EXEC DDLDropTable tmpDocument_Template
END
GO


-- *****************************************************************************
-- * Author:  Rajesh Jawane
-- * Date:    14/03/2008
-- * Purpose: Added Policy Add On Account Name
-- *****************************************************************************
IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='AddOnName')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('AddOnName','spu_wp_PolicyAddOn','AddOnName',0,'Policy','AddOn','Add On Account Name',1,'PolicyAddOn',9)
END
GO

-- *****************************************************************************
-- * Author:       Gurucharan Gulati
-- * Date:         02/04/2008  
-- * Development : Payment Maintenance
-- *****************************************************************************
-- * Purpose:To add a new task for Payment Maintenance

IF  (Select Count(*) FROM  PMWrk_Task WHERE code = 'PMTMAINT' ) >1 
BEGIN
    DELETE FROM PMWrk_Task_Group_Task WHERE pmwrk_task_id IN
    (SELECT pmwrk_task_id FROM PMWrk_Task WHERE code = 'PMTMAINT' )

    DELETE FROM PMWrk_Task WHERE code = 'PMTMAINT' 
    AND PMWrk_Task_ID >(SELECT MIN(PMWrk_Task_ID)FROM PMWrk_Task WHERE code = 'PMTMAINT')
END
 
DECLARE @lCaptionID  INT
DECLARE @TaskGroupId INT
DECLARE @TaskId      INT
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task WHERE code = 'PMTMAINT')
    BEGIN
               EXECUTE spu_pm_caption_id_return 1, 'Payment Maintenance', @lCaptionID OUTPUT

               INSERT INTO PMWrk_Task 
                        (caption_id, code, description, is_deleted, effective_date, 
                        is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, 
                        auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, 
                        linked_caption_id, is_available_task, pmwrk_task_category_id) 

               Values (@lCaptionID, 'PMTMAINT', 'Payment Maintenance', 0, 
                          GETDATE(), 0, 1, NULL, 'iACTPaymentMaintenance', 'NavigatorV3', 0, 42, 0, NULL, NULL, NULL, 1, 2)
               
    END

    -- Create link to group for this new task
    SELECT @TaskId = ( SELECT MAX(pmwrk_task_id) FROM pmwrk_task WHERE code = 'PMTMAINT')

    -- Add task to correct group
    SELECT @TaskGroupId = ( SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'SLACS')

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task_Group_Task WHERE pmwrk_task_group_id = @TaskGroupId AND pmwrk_task_id  = @TaskId)
    BEGIN
       INSERT INTO pmwrk_task_group_task
       (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num)
       VALUES
       (@TaskGroupId, @TaskId, 0)
    END
END

GO

-- * Purpose: Added Report Cancelled_Payment_Report.rpt       
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U') 
BEGIN
    DECLARE @report_id INT
    DECLARE @lCaptionID INT
    DECLARE @group_id INT

    -- Cancel Payment report
    SELECT @report_id = report_id FROM report WHERE code = 'Cancel_Pmt'

    IF(ISNULL(@report_id,0) = 0)
    BEGIN    
        -- Add report
        SELECT @report_id = MAX(report_id)+1 FROM report
        EXECUTE spu_pm_caption_id_return 1, 'Audit Report For Cancelled Payments by Media Type', @lCaptionID OUTPUT
        INSERT INTO report(report_id, caption_id, code, description, is_deleted, effective_date, report_name) 
               VALUES(@report_id, @lCaptionID, 'Cancel_Pmt', 'Audit Report For Cancelled Payments by Media Type', 0, GETDATE(), 'Cancelled_Payment_Report.rpt')

        -- Link report to group
        SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'UND'
        IF(ISNULL(@group_id,0)<>0)
            INSERT INTO Report_Group_Contents (report_group_id,report_id) VALUES(@group_id,@report_id)
    END
END
GO

-- * Purpose: Update to show cashlistitem_reverse_reason table in lookup maintenance  
UPDATE pmproduct_lookup SET is_generic_maintenance=1 WHERE lookup_table_name='cashlistitem_reverse_reason'
AND is_generic_maintenance <> 1
     
-- * Purpose : Payment Maintenance -Script to add new event in event_type table
IF Not Exists (Select 1 From Event_Type_Group Where Code = 'CNCLPMT')
BEGIN
    Declare @lCaptionID integer
    Declare @Event_type_GroupId Integer

    Execute spu_pm_caption_id_return 1, 'CNCLPMT', @lCaptionID output
    
    Select @Event_type_GroupId = Max(Event_type_Group_Id)+1 From Event_type_Group

    Insert into Event_Type_Group 
    (
        event_type_group_id,
        caption_id,
        code,
        description,
        is_deleted,
        effective_date
    )
    Values
    (
        @Event_type_GroupId,
        @lCaptionID,
        'CNCLPMT',
        'Cancel Payment',
        0,
        GetDate()
    )

    Declare @Event_type_Id Integer
    Execute spu_pm_caption_id_return 1, 'CNCLPMT', @lCaptionID output
    
    Select @Event_type_Id = Max(Event_type_Id)+1 From Event_type
    
    Insert into Event_Type
    (
        event_type_id,
        caption_id,
        code,
        description,
        is_deleted,
        effective_date,
        Event_type_Group_Id
    )
    Values
    (
        @Event_type_Id,
        @lCaptionID,
        'CNCLPMT',
        'Cancel Payment',
        0,
        GetDate(),
        @Event_type_GroupId
    )
    END

GO


-- *****************************************************************************
-- * Author:    Sumit Bhardwaj
-- * Date:      02/04/2008
-- * Purpose:   Updating wp_fields table field is_displayed=1
                
-- *****************************************************************************
BEGIN

    UPDATE wp_fields
    SET is_displayed=1
    WHERE field_name='businessType'

END
GO


-- *****************************************************************************
-- * Author:  Sumit Bhardwaj
-- * Date:    03/04/2008
-- * Purpose: Added Merge Code For Party Conviction Screen
-- *****************************************************************************
IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyConviction')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyConviction','spu_wp_PartyConviction','Code',0,'Party','Conviction','Conviction Code',1,'PartyConviction',9)

END

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyConvictionDate')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyConvictionDate','spu_wp_PartyConviction','conviction_date',0,'Party','Conviction','Conviction Date',1,'PartyConviction',9)

END

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyConvictionDescription')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyConvictionDescription','spu_wp_PartyConviction','description',0,'Party','Conviction','Conviction Description',1,'PartyConviction',9)

END


IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyConvictionFineAmount')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyConvictionFineAmount','spu_wp_PartyConviction','fine_amt',11,'Party','Conviction','Conviction Fine Amount',1,'PartyConviction',9)

END

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyConvictionSentenceCode')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyConvictionSentenceCode','spu_wp_PartyConviction','sentence_code',0,'Party','Conviction','Sentence Code',1,'PartyConviction',9)

END


IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyConvictionSentenceDescription')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyConvictionSentenceDescription','spu_wp_PartyConviction','sentence_description',0,'Party','Conviction','Sentence Description',1,'PartyConviction',9)

END


IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyConvictionSentenceDuration')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyConvictionSentenceDuration','spu_wp_PartyConviction','sentence_duration',11,'Party','Conviction','Sentence Duration',1,'PartyConviction',9)

END


IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyConvictionSentenceDurationQualifier')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyConvictionSentenceDurationQualifier','spu_wp_PartyConviction','sentence_duration_qualifier',0,'Party','Conviction','Sentence Duration Qualifier',1,'PartyConviction',9)

END


IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyConvictionSentenceEffectiveDate')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyConvictionSentenceEffectiveDate','spu_wp_PartyConviction','sentence_effective_date',0,'Party','Conviction','Sentence Effective Date',1,'PartyConviction',9)

END


IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyConvictionStatusCode')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyConvictionStatusCode','spu_wp_PartyConviction','status_code',0,'Party','Conviction','Status Code',1,'PartyConviction',9)

END

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyConvictionAlcoholLevel')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyConvictionAlcoholLevel','spu_wp_PartyConviction','alcohol_level',11,'Party','Conviction','Alcohol Level',1,'PartyConviction',9)

END


IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyConvictionAlcoholMeasurementMethod')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyConvictionAlcoholMeasurementMethod','spu_wp_PartyConviction','alcohol_measurement_method',11,'Party','Conviction','Alcohol Measurement Method',1,'PartyConviction',9)

END

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyConvictionDrivingLicencePenaltyPoints')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyConvictionDrivingLicencePenaltyPoints','spu_wp_PartyConviction','driving_licence_penalty_pts',11,'Party','Conviction','Driving Licence Penalty Points',1,'PartyConviction',9)

END


GO
-- *****************************************************************************
-- * Author: 	Lipcy Joseph
-- * Date: 	07/04/2008
-- * Purpose:	Work Manager Tool Kit - Add a new task for Navigator Editor
-- *****************************************************************************
    -- Create new task for Navigator Editor
    DECLARE @lCaptionID int
    DECLARE @TaskGroupId int
    DECLARE @TaskId AS INTEGER
    
    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task WHERE code = 'PMNAVXMED')
    BEGIN
        EXECUTE spu_pm_caption_id_return 1, 'Navigator Editor', @lCaptionID output
        
        INSERT INTO PMWrk_Task (caption_id, code, description, is_deleted, effective_date, 
				is_system_task, type_of_task, pmnav_process_id, component_object_name, 
				component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, 
				linked_object_name, linked_class_name, linked_caption_id, is_available_task,
				pmwrk_task_category_id) 
        SELECT  @lCaptionID, 'PMNAVXMED', 'Navigator Editor', 0, 
                    GETDATE()-1, 0, 1, NULL, 'PmNavXmEditor', 'NavigatorV3', 0, 1, 0, NULL, NULL, NULL, 1, 2
        FROM PMWrk_Task
    END


-- *****************************************************************************
-- * Author: 	Lipcy Joseph
-- * Date: 	07/04/2008
-- * Purpose:	Work Manager Tool Kit - Add steps to Navigator Editor
-- *****************************************************************************
    IF NOT EXISTS (SELECT NULL FROM pmnavxm_available_step WHERE available_step_code = 'SCREENTEXT')
    BEGIN        
        INSERT INTO pmnavxm_available_step (pmnavxm_available_step_id, available_step_code,available_step_description, 
						available_step_icon,effective_date,is_deleted,is_core,attribute_description,
						attribute_component,attribute_type,Attribute_cancelaction,
						attribute_okaction,attribute_oksteps,attribute_cancelsteps) 
        SELECT  ISNULL(MAX(pmnavxm_available_step_id), 0) + 1,'SCREENTEXT','TEXT','EditText',GETDATE()-1,0,0,
				  'SCREENTEXT','iPMNavXmStepWrapper.ScreenText','XMST','AP','F1',0,0
        FROM pmnavxm_available_step
    END

    -- Inserting Steps entry to pmnav_key table

    IF NOT EXISTS (SELECT NULL FROM pmnav_key WHERE name = 'TEXT')
    BEGIN        
        INSERT INTO pmnav_key (pmnav_key_id,Name,Description,data_type,is_deleted,effective_date ) 
        SELECT  ISNULL(MAX(pmnav_key_id), 0) + 1,'TEXT','TEXT',0,0,GETDATE()-1
        FROM pmnav_key
    END
   
    IF NOT EXISTS (SELECT NULL FROM pmnav_key WHERE name = 'DESCRIPTION')
    BEGIN        
        INSERT INTO pmnav_key (pmnav_key_id,Name,Description,data_type,is_deleted,effective_date ) 
        SELECT  ISNULL(MAX(pmnav_key_id), 0) + 1,'DESCRIPTION','DESCRIPTION',0,0,GETDATE()-1
        FROM pmnav_key
    END

    IF NOT EXISTS (SELECT NULL FROM pmnav_key WHERE name = 'DOC_TEMPLATE_MODE')
    BEGIN        
        INSERT INTO pmnav_key (pmnav_key_id,Name,Description,data_type,is_deleted,effective_date ) 
        SELECT  ISNULL(MAX(pmnav_key_id), 0) + 1,'DOC_TEMPLATE_MODE','DOC_TEMPLATE_MODE',0,0,GETDATE()-1
        FROM pmnav_key
    END

    IF NOT EXISTS (SELECT NULL FROM pmnav_key WHERE name = 'DiaryUserGroupId')
    BEGIN        
        INSERT INTO pmnav_key (pmnav_key_id,Name,Description,data_type,is_deleted,effective_date ) 
        SELECT  ISNULL(MAX(pmnav_key_id), 0) + 1,'DiaryUserGroupId','Diary User Group Id',0,0,GETDATE()-1
        FROM pmnav_key
    END

    IF NOT EXISTS (SELECT NULL FROM pmnav_key WHERE name = 'DiaryUserId')
    BEGIN        
        INSERT INTO pmnav_key (pmnav_key_id,Name,Description,data_type,is_deleted,effective_date ) 
        SELECT  ISNULL(MAX(pmnav_key_id), 0) + 1,'DiaryUserId','Diary User Id',0,0,GETDATE()-1
        FROM pmnav_key
    END

    IF NOT EXISTS (SELECT NULL FROM pmnav_key WHERE name = 'DiaryTaskDays')
    BEGIN        
        INSERT INTO pmnav_key (pmnav_key_id,Name,Description,data_type,is_deleted,effective_date ) 
        SELECT  ISNULL(MAX(pmnav_key_id), 0) + 1,'DiaryTaskDays','Diary Task Days',0,0,GETDATE()-1
        FROM pmnav_key
    END

    IF NOT EXISTS (SELECT NULL FROM pmnav_key WHERE name = 'DiaryTaskGroupId')
    BEGIN        
        INSERT INTO pmnav_key (pmnav_key_id,Name,Description,data_type,is_deleted,effective_date ) 
        SELECT  ISNULL(MAX(pmnav_key_id), 0) + 1,'DiaryTaskGroupId','Diary Task Group Id',0,0,GETDATE()-1
        FROM pmnav_key
    END

    IF NOT EXISTS (SELECT NULL FROM pmnav_key WHERE name = 'DiaryTaskId')
    BEGIN        
        INSERT INTO pmnav_key (pmnav_key_id,Name,Description,data_type,is_deleted,effective_date ) 
        SELECT  ISNULL(MAX(pmnav_key_id), 0) + 1,'DiaryTaskId','Diary Task Id',0,0,GETDATE()-1
        FROM pmnav_key
    END

    DECLARE @pmnavxm_available_step_id int
    DECLARE @pmnav_key_id int	

    --SCREENTEXT STEP
    SELECT @pmnavxm_available_step_id = pmnavxm_available_step_id 
    FROM pmnavxm_available_step 
    WHERE available_step_code ='SCREENTEXT'
 
    SELECT @pmnav_key_id = pmnav_key_id 
    FROM pmnav_key
    WHERE name ='TEXT'

    IF NOT EXISTS (SELECT NULL FROM pmnavxm_step_default_key WHERE pmnav_key_id = @pmnav_key_id)
    BEGIN        
        INSERT INTO pmnavxm_step_default_key (pmnavxm_step_default_key_id,pmnavxm_available_step_id,pmnav_key_id) 
        SELECT  ISNULL(MAX(pmnavxm_step_default_key_id), 0) + 1,@pmnavxm_available_step_id,@pmnav_key_id
        FROM pmnavxm_step_default_key
    END

    SELECT @pmnav_key_id=0
    SELECT @pmnav_key_id = pmnav_key_id 
    FROM pmnav_key
    WHERE name ='DESCRIPTION'

    IF NOT EXISTS (SELECT NULL FROM pmnavxm_step_default_key WHERE pmnav_key_id = @pmnav_key_id)
    BEGIN        
        INSERT INTO pmnavxm_step_default_key (pmnavxm_step_default_key_id,pmnavxm_available_step_id,pmnav_key_id) 
        SELECT  ISNULL(MAX(pmnavxm_step_default_key_id), 0) + 1,@pmnavxm_available_step_id,@pmnav_key_id
        FROM pmnavxm_step_default_key
    END

    -- STANDARDLETTER STEP
    SELECT @pmnavxm_available_step_id=0
    SELECT @pmnav_key_id =0

    SELECT @pmnavxm_available_step_id = pmnavxm_available_step_id 
    FROM pmnavxm_available_step 
    WHERE available_step_code ='STANDARDLETTER'
 
    SELECT @pmnav_key_id = pmnav_key_id 
    FROM pmnav_key
    WHERE name ='DOC_TEMPLATE_MODE'

    IF NOT EXISTS (SELECT NULL FROM pmnavxm_step_default_key WHERE pmnav_key_id = @pmnav_key_id)
    BEGIN        
        INSERT INTO pmnavxm_step_default_key (pmnavxm_step_default_key_id,pmnavxm_available_step_id,pmnav_key_id) 
        SELECT  ISNULL(MAX(pmnavxm_step_default_key_id), 0) + 1,@pmnavxm_available_step_id,@pmnav_key_id
        FROM pmnavxm_step_default_key
    END

    --DIARY STEP
    SELECT @pmnavxm_available_step_id=0
    SELECT @pmnav_key_id =0

    SELECT @pmnavxm_available_step_id = pmnavxm_available_step_id 
    FROM pmnavxm_available_step 
    WHERE available_step_code ='DIARY'
 
    SELECT @pmnav_key_id = pmnav_key_id 
    FROM pmnav_key
    WHERE name ='DiaryUserGroupId'

    IF NOT EXISTS (SELECT NULL FROM pmnavxm_step_default_key WHERE pmnav_key_id = @pmnav_key_id)
    BEGIN        
        INSERT INTO pmnavxm_step_default_key (pmnavxm_step_default_key_id,pmnavxm_available_step_id,pmnav_key_id) 
        SELECT  ISNULL(MAX(pmnavxm_step_default_key_id), 0) + 1,@pmnavxm_available_step_id,@pmnav_key_id
        FROM pmnavxm_step_default_key
    END

    SELECT @pmnav_key_id =0
    
    SELECT @pmnav_key_id = pmnav_key_id 
    FROM pmnav_key
    WHERE name ='DiaryUserId'

    IF NOT EXISTS (SELECT NULL FROM pmnavxm_step_default_key WHERE pmnav_key_id = @pmnav_key_id)
    BEGIN        
        INSERT INTO pmnavxm_step_default_key (pmnavxm_step_default_key_id,pmnavxm_available_step_id,pmnav_key_id) 
        SELECT  ISNULL(MAX(pmnavxm_step_default_key_id), 0) + 1,@pmnavxm_available_step_id,@pmnav_key_id
        FROM pmnavxm_step_default_key
    END

    SELECT @pmnav_key_id =0

    SELECT @pmnav_key_id = pmnav_key_id 
    FROM pmnav_key
    WHERE name ='DiaryTaskDays'

    IF NOT EXISTS (SELECT NULL FROM pmnavxm_step_default_key WHERE pmnav_key_id = @pmnav_key_id)
    BEGIN        
        INSERT INTO pmnavxm_step_default_key (pmnavxm_step_default_key_id,pmnavxm_available_step_id,pmnav_key_id) 
        SELECT  ISNULL(MAX(pmnavxm_step_default_key_id), 0) + 1,@pmnavxm_available_step_id,@pmnav_key_id
        FROM pmnavxm_step_default_key
    END

    SELECT @pmnav_key_id =0

    SELECT @pmnav_key_id = pmnav_key_id 
    FROM pmnav_key
    WHERE name ='DiaryTaskGroupId'

    IF NOT EXISTS (SELECT NULL FROM pmnavxm_step_default_key WHERE pmnav_key_id = @pmnav_key_id)
    BEGIN        
        INSERT INTO pmnavxm_step_default_key (pmnavxm_step_default_key_id,pmnavxm_available_step_id,pmnav_key_id) 
        SELECT  ISNULL(MAX(pmnavxm_step_default_key_id), 0) + 1,@pmnavxm_available_step_id,@pmnav_key_id
        FROM pmnavxm_step_default_key
    END

    SELECT @pmnav_key_id =0

    SELECT @pmnav_key_id = pmnav_key_id 
    FROM pmnav_key
    WHERE name ='DiaryTaskId'

    IF NOT EXISTS (SELECT NULL FROM pmnavxm_step_default_key WHERE pmnav_key_id = @pmnav_key_id)
    BEGIN        
        INSERT INTO pmnavxm_step_default_key (pmnavxm_step_default_key_id,pmnavxm_available_step_id,pmnav_key_id) 
        SELECT  ISNULL(MAX(pmnavxm_step_default_key_id), 0) + 1,@pmnavxm_available_step_id,@pmnav_key_id
        FROM pmnavxm_step_default_key
    END
GO



-- *****************************************************************************
-- * Purpose:Add a new task for Banking
-- * Author: Vijay Bhushan
-- * Date: 02/03/2008
-- *****************************************************************************
DECLARE @lCaptionID  INT
DECLARE @TaskGroupId INT
DECLARE @TaskId      INT

IF EXISTS (SELECT NULL FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN
    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task WHERE code = 'BANKING')
    BEGIN
	   EXECUTE spu_pm_caption_id_return 1, 'Banking', @lCaptionID OUTPUT

	   INSERT INTO PMWrk_Task 
		(caption_id, code, description, is_deleted, effective_date, 
		is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, 
		auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, 
		linked_caption_id, is_available_task, pmwrk_task_category_id) 

	   SELECT @lCaptionID, 'BANKING', 'Banking', 0, 
		  GETDATE(), 0, 1, NULL, 'iACTBanking', 'NavigatorV3', 0, 1, 0, NULL, NULL, NULL, 1, 2
	   FROM PMWrk_Task
    END

    -- Create link to group for this new task
    SELECT @TaskId = ( SELECT pmwrk_task_id FROM pmwrk_task WHERE code = 'BANKING')

    -- Add task to correct group
    SELECT @TaskGroupId = ( SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'SLACS')

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task_Group_Task WHERE pmwrk_task_group_id = @TaskGroupId AND pmwrk_task_id  = @TaskId)
    BEGIN
       INSERT INTO pmwrk_task_group_task
       (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num)
       VALUES
       (@TaskGroupId, @TaskId, 0)
    END
END
GO

DECLARE @lCaptionID INT

IF NOT EXISTS(SELECT NULL FROM Bank_Account_Type WHERE code = 'BANKACC')
BEGIN
	EXEC spu_pm_caption_id_return 1, 'Bank Account', @lCaptionID OUTPUT
	INSERT INTO Bank_Account_Type(Bank_account_type_id,code,description,caption_id,effective_date,is_deleted)
	VALUES (1,'BANKACC','Bank Account',@lCaptionID,GETDATE(),0)
END

IF NOT EXISTS(SELECT NULL FROM Bank_Account_Type WHERE code = 'BANKSUSP')
BEGIN
	EXEC spu_pm_caption_id_return 1, 'Bank Suspence Account', @lCaptionID OUTPUT
	INSERT INTO Bank_Account_Type(Bank_account_type_id,code,description,caption_id,effective_date,is_deleted)
	VALUES (2,'BANKSUSP','Bank Suspence Account',@lCaptionID,GETDATE(),0)
END

IF NOT EXISTS (SELECT NULL FROM PMProduct_Lookup WHERE lookup_table_name = 'Bank_Account_Type')
BEGIN
	INSERT INTO PMProduct_Lookup
             	(pmproduct_id, lookup_table_name, edit_privilege_level, is_generic_maintenance)
	VALUES  (3, 'Bank_Account_Type', 3, 1)

END
GO

-- *****************************************************************************
-- * Author: Vijay Bhushan
-- * Date: 02/03/2008
-- * Purpose : Add a suspense account in account explorer
-- *****************************************************************************
IF EXISTS (SELECT NULL FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN

    DECLARE @AccountID INT
    DECLARE @ParentMappingID INT
    DECLARE @ElementId INT
    DECLARE @ParentNodeID INT
    DECLARE @Mapping_id INT


    IF NOT EXISTS (SELECT Short_Code FROM Account WHERE Short_Code = 'BANKSUSP')
    BEGIN
    
        INSERT INTO Account
        (
        purgefrequency_id, accountstatus_id,company_id,sub_branch_id,currency_id,accounttype_id,paymenttype_id,ledger_id,
        account_name, short_code,restrict_enquiry, restrict_update,delete_at_purge,contact_name, address1,address2,address3,
        Address4,postal_code,address_country,credit_limit,discount_percentage,settlement_period,account_key,nominal_account_id,
        allow_electronic_payment)
        VALUES
        (1,1,1,1,26,3,1,1,'Bank Suspense Account','BANKSUSP',0,0,0,'','','','','','',1,0,0,0,0,0,0)

        SET @AccountID = @@Identity

    	--create Element
    	INSERT INTO Element  
    	    (element_name)
    	VALUES
    	    ('BANKSUSP')
    	
        --store the element_id
       	SELECT @ElementId = @@Identity
     
        --Add Folder in the account explorer
	EXEC spu_ACT_setup_add_folder 'Bank Suspense','Current Assets',1,1

        SET @ParentNodeID = (SELECT st.node_id FROM structuretree st
                        JOIN element e ON  e.element_id = st.element_id
                        WHERE e.element_name = 'Bank Suspense'
                        AND st.company_id = 1)  

	INSERT INTO ElementExtras
            (element_id,account_map_id,totalling_id,spare_number, is_deletable)
        VALUES
            (@ElementID,@AccountID,2,0,0)

        --create structure tree record
    	INSERT INTO StructureTree
            (company_id,mapping_id, account_id, element_id, parent_node_id, core_node)
        VALUES
            (1,@mapping_id,@AccountID,@ElementID,@ParentNodeID,1)
            
    END
END
GO

DECLARE @captionid AS INTEGER,
        @description AS VARCHAR(50),
	@bankid AS INT,
        @AccountID AS INT,
	@DefaultBankAccountID AS INT

SET IDENTITY_INSERT Bank ON 
IF NOT EXISTS (SELECT NULL FROM Account WHERE Short_Code = 'BANKSUSPAC')
    BEGIN
        SELECT @bankid = MAX(bank_id) + 1 FROM bank 

	SELECT @bankid =  ISNULL(@bankid,1)
	SET @Description = 'Bank Suspense Account'
	
	INSERT INTO bank 
	(bank_id,code,bank_name,head_office,bank_country,comments, bank_account_type_id)
	VALUES
	(@bankid,'BANKSUSPAC',@Description,@bankid,1,'Bank',2)

        INSERT INTO Account
        (
        purgefrequency_id, accountstatus_id,company_id,sub_branch_id,currency_id,accounttype_id,paymenttype_id,ledger_id,
        account_name, short_code,restrict_enquiry, restrict_update,delete_at_purge,contact_name, address1,address2,address3,
        Address4,postal_code,address_country,credit_limit,discount_percentage,settlement_period,account_key,nominal_account_id,
        allow_electronic_payment
        )
        VALUES
        (1,1,1,1,26,3,1,1,@Description,'BANKSUSPAC',0,0,0,'','','','','','',1,0,0,0,0,0,0)

        SET @AccountID = @@Identity

	EXECUTE spu_pm_caption_id_return 1, @Description, @CaptionId OUTPUT
	SELECT @DefaultBankAccountID=Account_ID FROM Account WHERE Short_Code = 'BANKSUSP' 
	
	INSERT INTO bankaccount 
	(
	currency_id,
	company_id,
	sub_branch_id,
	account_id,
	bank_id,
	code,
	bank_account_no,
	bank_account_name,
	description,
	caption_id,
	effective_date,
	is_deleted,
	default_bank_account_id )
	VALUES (26,1,1, @AccountID,@bankid,'','',@Description,@Description, @captionid,'2000-01-01',0,@DefaultBankAccountID)

END
GO
IF EXISTS (SELECT NULL FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN

  IF NOT EXISTS ( SELECT NULL FROM CashListItem_Receipt_Type WHERE code='BANK')
  BEGIN
    DECLARE @caption_id INT
    DECLARE @cashlistitem_receipt_type_id INT

    SELECT @cashlistitem_receipt_type_id = MAX(ISNULL(cashlistitem_receipt_type_id,0))+1 
    FROM CashListItem_Receipt_Type

    EXECUTE spu_pm_caption_id_return 1, 'Banking', @caption_id OUTPUT

    INSERT INTO CashListItem_Receipt_Type
    (cashlistitem_receipt_type_id,code,description,caption_id,effective_date,is_deleted)
    VALUES
    (@cashlistitem_receipt_type_id,'BANK','Banking',@caption_id,GETDATE(),0)
  END

  IF NOT EXISTS ( SELECT NULL FROM MediaType WHERE code='BANK')
  BEGIN

    DECLARE @media_type_id INT

    SELECT @media_type_id= MAX(ISNULL(mediatype_id ,0))+ 1 
    FROM MediaType 

    EXECUTE spu_pm_caption_id_return 1, 'Banking', @caption_id OUTPUT
    INSERT INTO mediatype
    (
        mediatype_id,caption_id,is_deleted,effective_date,description,
        code,mediatype_validation_id,is_rounding_enabled,is_validation_enabled,
        is_banking,is_stoppable,is_receipt,is_payment,is_manual_payment,
        is_via_third_party,is_media_reference_mandatory,is_receipt_printed_automatically
    )
    VALUES
       (@media_type_id,@caption_id,0,getdate(),'Banking',
        'BANK',4,0,0,0,0,1,0,0,0,0,0)

   END
END
GO

IF EXISTS (SELECT NULL FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN

    DECLARE @report_id INT
    DECLARE @lCaptionID INT
    DECLARE @group_id INT

    IF NOT EXISTS (SELECT NULL FROM report WHERE code = 'BANKING')
    BEGIN
        -- Add report
        SELECT @report_id = MAX(report_id)+1 FROM report
        EXECUTE spu_pm_caption_id_return 1, 'Manual Banking', @lCaptionID OUTPUT
        INSERT INTO report(report_id, caption_id, code, description, is_deleted, effective_date, report_name)
        VALUES(@report_id, @lCaptionID, 'BANKING', 'Manual Banking', 0, GETDATE(), 'Manual_Banking.rpt')

        -- Link report to group
        SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'ALL'
        IF(ISNULL(@group_id,0)<>0)
            INSERT INTO Report_Group_Contents (report_group_id,report_id) VALUES(@group_id,@report_id)
    END
END
GO

GO

-- *****************************************************************************
-- * Purpose:Add a new task for Banking
-- * Author: Vijay Bhushan
-- * Date: 02/03/2008
-- *****************************************************************************
DECLARE @lCaptionID  INT
DECLARE @TaskGroupId INT
DECLARE @TaskId      INT

IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN
    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task WHERE code = 'BANKING')
    BEGIN
	   EXECUTE spu_pm_caption_id_return 1, 'Banking', @lCaptionID OUTPUT

	   INSERT INTO PMWrk_Task 
		(caption_id, code, description, is_deleted, effective_date, 
		is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, 
		auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, 
		linked_caption_id, is_available_task, pmwrk_task_category_id) 

	   SELECT @lCaptionID, 'BANKING', 'Banking', 0, 
		  GETDATE(), 0, 1, NULL, 'iACTBanking', 'NavigatorV3', 0, 1, 0, NULL, NULL, NULL, 1, 2
	   FROM PMWrk_Task
    END

    -- Create link to group for this new task
    SELECT @TaskId = ( SELECT pmwrk_task_id FROM pmwrk_task WHERE code = 'BANKING')

    -- Add task to correct group
    SELECT @TaskGroupId = ( SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'SLACS')

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task_Group_Task WHERE pmwrk_task_group_id = @TaskGroupId AND pmwrk_task_id  = @TaskId)
    BEGIN
       INSERT INTO pmwrk_task_group_task
       (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num)
       VALUES
       (@TaskGroupId, @TaskId, 0)
    END
END
GO

DECLARE @lCaptionID INT

IF NOT EXISTS(SELECT NULL FROM Bank_Account_Type WHERE code = 'BANKACC')
BEGIN
	EXEC spu_pm_caption_id_return 1, 'Bank Account', @lCaptionID OUTPUT
	INSERT INTO Bank_Account_Type(Bank_account_type_id,code,description,caption_id,effective_date,is_deleted)
	VALUES (1,'BANKACC','Bank Account',@lCaptionID,GETDATE(),0)
END

IF NOT EXISTS(SELECT NULL FROM Bank_Account_Type WHERE code = 'BANKSUSP')
BEGIN
	EXEC spu_pm_caption_id_return 1, 'Bank Suspence Account', @lCaptionID OUTPUT
	INSERT INTO Bank_Account_Type(Bank_account_type_id,code,description,caption_id,effective_date,is_deleted)
	VALUES (2,'BANKSUSP','Bank Suspence Account',@lCaptionID,GETDATE(),0)
END

IF NOT EXISTS (SELECT NULL FROM PMProduct_Lookup WHERE lookup_table_name = 'Bank_Account_Type')
BEGIN
	INSERT INTO PMProduct_Lookup
             	(pmproduct_id, lookup_table_name, edit_privilege_level, is_generic_maintenance)
	VALUES  (3, 'Bank_Account_Type', 3, 1)

END
GO

-- *****************************************************************************
-- * Author: Vijay Bhushan
-- * Date: 02/03/2008
-- * Purpose : Add a suspense account in account explorer
-- *****************************************************************************
IF EXISTS (SELECT NULL FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN

    DECLARE @AccountID INT
    DECLARE @ParentMappingID INT
    DECLARE @ElementId INT
    DECLARE @ParentNodeID INT
    DECLARE @Mapping_id INT


    IF NOT EXISTS (SELECT Short_Code FROM Account WHERE Short_Code = 'BANKSUSP')
    BEGIN
    
        INSERT INTO Account
        (
        purgefrequency_id, accountstatus_id,company_id,sub_branch_id,currency_id,accounttype_id,paymenttype_id,ledger_id,
        account_name, short_code,restrict_enquiry, restrict_update,delete_at_purge,contact_name, address1,address2,address3,
        Address4,postal_code,address_country,credit_limit,discount_percentage,settlement_period,account_key,nominal_account_id,
        allow_electronic_payment)
        VALUES
        (1,1,1,1,26,3,1,1,'Bank Suspense Account','BANKSUSP',0,0,0,'','','','','','',1,0,0,0,0,0,0)

        SET @AccountID = @@Identity

    	--create Element
    	INSERT INTO Element  
    	    (element_name)
    	VALUES
    	    ('BANKSUSP')
    	
        --store the element_id
       	SELECT @ElementId = @@Identity
     
        --Add Folder in the account explorer
	EXEC spu_ACT_setup_add_folder 'Bank Suspense','Current Assets',1,1

        SET @ParentNodeID = (SELECT st.node_id FROM structuretree st
                        JOIN element e ON  e.element_id = st.element_id
                        WHERE e.element_name = 'Bank Suspense'
                        AND st.company_id = 1)  

	INSERT INTO ElementExtras
            (element_id,account_map_id,totalling_id,spare_number, is_deletable)
        VALUES
            (@ElementID,@AccountID,2,0,0)

        --create structure tree record
    	INSERT INTO StructureTree
            (company_id,mapping_id, account_id, element_id, parent_node_id, core_node)
        VALUES
            (1,@mapping_id,@AccountID,@ElementID,@ParentNodeID,1)
            
    END
END
GO

DECLARE @captionid AS INTEGER,
        @description AS VARCHAR(50),
	@bankid AS INT,
        @AccountID AS INT,
	@DefaultBankAccountID AS INT


    IF NOT EXISTS (SELECT NULL FROM Account WHERE Short_Code = 'BANKSUSPAC')
    BEGIN
        SELECT @bankid = MAX(bank_id) + 1 FROM bank 

	SELECT @bankid =  ISNULL(@bankid,1)
	SET @Description = 'Bank Suspense Account'
	
	INSERT INTO bank 
	(bank_id,code,bank_name,head_office,bank_country,comments, bank_account_type_id)
	VALUES
	(@bankid,'BANKSUSPAC',@Description,@bankid,1,'Bank',2)

        INSERT INTO Account
        (
        purgefrequency_id, accountstatus_id,company_id,sub_branch_id,currency_id,accounttype_id,paymenttype_id,ledger_id,
        account_name, short_code,restrict_enquiry, restrict_update,delete_at_purge,contact_name, address1,address2,address3,
        Address4,postal_code,address_country,credit_limit,discount_percentage,settlement_period,account_key,nominal_account_id,
        allow_electronic_payment
        )
        VALUES
        (1,1,1,1,26,3,1,1,@Description,'BANKSUSPAC',0,0,0,'','','','','','',1,0,0,0,0,0,0)

        SET @AccountID = @@Identity

	EXECUTE spu_pm_caption_id_return 1, @Description, @CaptionId OUTPUT
	SELECT @DefaultBankAccountID=Account_ID FROM Account WHERE Short_Code = 'BANKSUSP' 
	
	INSERT INTO bankaccount 
	(
	currency_id,
	company_id,
	sub_branch_id,
	account_id,
	bank_id,
	code,
	bank_account_no,
	bank_account_name,
	description,
	caption_id,
	effective_date,
	is_deleted,
	default_bank_account_id )
	VALUES (26,1,1, @AccountID,@bankid,'','',@Description,@Description, @captionid,'2000-01-01',0,@DefaultBankAccountID)

END
GO

IF NOT EXISTS ( SELECT NULL FROM CashListItem_Receipt_Type WHERE code='BANK')
BEGIN
    DECLARE @caption_id INT
    DECLARE @cashlistitem_receipt_type_id INT

    SELECT @cashlistitem_receipt_type_id = MAX(ISNULL(cashlistitem_receipt_type_id,0))+1 
    FROM CashListItem_Receipt_Type

    EXECUTE spu_pm_caption_id_return 1, 'Banking', @caption_id OUTPUT

    INSERT INTO CashListItem_Receipt_Type
    (cashlistitem_receipt_type_id,code,description,caption_id,effective_date,is_deleted)
    VALUES
    (@cashlistitem_receipt_type_id,'BANK','Banking',@caption_id,GETDATE(),0)
END
GO

IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN

    DECLARE @report_id INT
    DECLARE @lCaptionID INT
    DECLARE @group_id INT

    IF NOT EXISTS (SELECT NULL FROM report WHERE code = 'BANKING')
    BEGIN
        -- Add report
        SELECT @report_id = MAX(report_id)+1 FROM report
        EXECUTE spu_pm_caption_id_return 1, 'Manual Banking', @lCaptionID OUTPUT
        INSERT INTO report(report_id, caption_id, code, description, is_deleted, effective_date, report_name)
        VALUES(@report_id, @lCaptionID, 'BANKING', 'Manual Banking', 0, GETDATE(), 'Manual_Banking.rpt')

        -- Link report to group
        SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'ALL'
        IF(ISNULL(@group_id,0)<>0)
            INSERT INTO Report_Group_Contents (report_group_id,report_id) VALUES(@group_id,@report_id)
    END
END
GO

DELETE FROM system_option_configuration WHERE option_number =6003
INSERT INTO system_option_configuration 
(option_number, system_option_configuration_group_id, control_type, control_top, control_height, control_left, 
control_width, control_caption, command, mandatory_or_optional, tab_index, command_parameters) 
VALUES (6003, 21, 'CheckBox', 6600, 255, 240, 3015, 'Allow Centralised Banking', NULL, NULL, 32, NULL)

UPDATE system_options 
SET value = 1
WHERE
option_number=6003
AND value IS NULL
GO


-- *****************************************************************************
-- * Author  : Vijay Bhushan
-- * Date    :   12/03/2008
-- * Purpose : New PmNav_Process added for Banking
-- *****************************************************************************
IF EXISTS (SELECT NULL FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN

    IF  NOT EXISTS(SELECT NULL FROM PMNav_Map WHERE code= 'BANKING')
    BEGIN
    
    DECLARE
        @pmnav_component_id int,
        @pmnav_process_id int,
        @pmnav_map_id int,
        @pmnav_step_id int,
        @pmproduct_id int,
        @caption_id int,
        @pmnav_key_id int,
        @pmnav_batch_id int 
        

    /*GETTING CAPTION FOR THIS MAP...*/
    EXECUTE spu_pm_caption_id_return 1, 'Banking', @caption_id OUTPUT

    /*GETTING SOURCE MAP DETAILS...*/

    /*INSERTING DESTINATION MAP DETAILS...*/
    SELECT @pmnav_map_id = ISNULL(MAX(pmnav_map_id),0) + 1 FROM PMNav_Map
    INSERT INTO PMNav_Map
    (pmnav_map_id, code, caption_id, description, is_deleted, effective_date, is_start_map )
    VALUES( @pmnav_map_id, 'BANKING', @caption_id, 'Banking', 0, '02/25/2003', 1)


    SELECT @pmnav_process_id = MAX(pmnav_process_id) + 1 FROM PMNav_process
    INSERT INTO PMNav_process
    (pmnav_process_id, pmproduct_id, code,caption_id,description, transaction_type_id, process_mode, 
    start_nav_map_id, is_logged, is_user_driven,is_deleted,effective_date )
    VALUES(@pmnav_process_id,3,'BANKING',@caption_id,'Banking', NULL,0,@pmnav_map_id,0,0,0,'10/04/2004')    

    /*COPYING MAP SETKEYS...*/
    
    --Account_id
    /*LOOKING UP SOURCE FIELD: PMNAV_KEY_ID */
    SELECT @pmnav_key_id = NULL
    SELECT @pmnav_key_id  = pmnav_key_id FROM PMNav_Key WHERE [name] = 'account_id' AND data_type = 2

    IF @pmnav_key_id IS NULL
    BEGIN
        SELECT @pmnav_key_id = ISNULL(MAX(pmnav_key_id),0) + 1 FROM PMNav_Key
        INSERT INTO PMNav_Key(pmnav_key_id, name, description, data_type, is_deleted, effective_date) 
        VALUES(@pmnav_key_id, 'account_id', 'Account ID', 2, 0, '10/04/2004')
    END

    /*INSERT RECORD */
    INSERT INTO PMNav_Set_Map_Key(pmnav_map_id, pmnav_key_id, sequence_number, description, initial_key_value) 
    VALUES (@pmnav_map_id, @pmnav_key_id, 1, 'Account ID',NULL )

    INSERT INTO PMNav_Set_Process_Key(pmnav_process_id, pmnav_key_id, sequence_number, description, initial_key_value) 
    VALUES (@pmnav_process_id, @pmnav_key_id, 1, 'Account ID',NULL )	

    -- batch_set_id
    /*LOOKING UP SOURCE FIELD: PMNAV_KEY_ID */
    SELECT @pmnav_key_id = NULL
    SELECT @pmnav_key_id  = pmnav_key_id  FROM PMNav_Key WHERE [name] = 'batch_set_id' AND data_type = 2
    IF @pmnav_key_id IS NULL
    BEGIN
    SELECT @pmnav_key_id = ISNULL(MAX(pmnav_key_id),0) + 1 FROM PMNav_Key
    INSERT INTO PMNav_Key(pmnav_key_id, name, description, data_type, is_deleted, effective_date) 
    VALUES(@pmnav_key_id, 'batch_set_id', 'Batch Set ID', 2, 0, '10/04/2004')
    END

    /*INSERT RECORD */
    INSERT INTO PMNav_Set_Map_Key(pmnav_map_id, pmnav_key_id, sequence_number, description, initial_key_value) 
    VALUES (@pmnav_map_id, @pmnav_key_id, 2, 'Batch Set ID',NULL ) 
    
    INSERT INTO PMNav_Set_Process_Key(pmnav_process_id, pmnav_key_id, sequence_number, description, initial_key_value) 
    VALUES (@pmnav_process_id, @pmnav_key_id, 2, 'Batch Set ID',NULL )	

    --banking_amount
    /*LOOKING UP SOURCE FIELD: PMNAV_KEY_ID */
    SELECT @pmnav_key_id = NULL
    SELECT @pmnav_key_id  = pmnav_key_id FROM PMNav_Key WHERE name = 'banking_amount' AND data_type = 0

    IF @pmnav_key_id IS NULL
    BEGIN
    SELECT @pmnav_key_id = ISNULL(MAX(pmnav_key_id),0) + 1 FROM PMNav_Key
    INSERT INTO PMNav_Key(pmnav_key_id, name, description, data_type, is_deleted, effective_date) 
    VALUES(@pmnav_key_id, 'banking_amount', 'Banking Amount', 0, 0, '10/04/2004')
    END

    /*INSERT RECORD */
    INSERT INTO PMNav_Set_Map_Key(pmnav_map_id, pmnav_key_id, sequence_number, description, initial_key_value) 
    VALUES (@pmnav_map_id, @pmnav_key_id,3, 'banking_amount',NULL )

    INSERT INTO PMNav_Set_Process_Key(pmnav_process_id, pmnav_key_id, sequence_number, description, initial_key_value) 
    VALUES (@pmnav_process_id, @pmnav_key_id, 3, 'banking_amount',NULL )	

    --currency_id
    /*LOOKING UP SOURCE FIELD: PMNAV_KEY_ID */
    SELECT @pmnav_key_id = NULL
    SELECT @pmnav_key_id  = pmnav_key_id FROM PMNav_Key WHERE name = 'currency_id' AND data_type = 0

    IF @pmnav_key_id IS NULL
    BEGIN
    SELECT @pmnav_key_id = ISNULL(MAX(pmnav_key_id),0) + 1 FROM PMNav_Key
    INSERT INTO PMNav_Key(pmnav_key_id, name, description, data_type, is_deleted, effective_date) 
    VALUES(@pmnav_key_id, 'currency_id', 'Currency ID', 2, 0, '10/04/2004')
    END

    /*INSERT RECORD */
    INSERT INTO PMNav_Set_Map_Key(pmnav_map_id, pmnav_key_id, sequence_number, description, initial_key_value) 
    VALUES (@pmnav_map_id, @pmnav_key_id,4, 'Currency ID',NULL )

    INSERT INTO PMNav_Set_Process_Key(pmnav_process_id, pmnav_key_id, sequence_number, description, initial_key_value) 
    VALUES (@pmnav_process_id, @pmnav_key_id, 4,'Currency ID',NULL )	

    --ledger_code
    /*LOOKING UP SOURCE FIELD: PMNAV_KEY_ID */
    SELECT @pmnav_key_id = NULL
    SELECT @pmnav_key_id  = pmnav_key_id FROM PMNav_Key WHERE name = 'ledger_code' AND data_type = 0

    IF @pmnav_key_id IS NULL
    BEGIN
    SELECT @pmnav_key_id = ISNULL(MAX(pmnav_key_id),0) + 1 FROM PMNav_Key
    INSERT INTO PMNav_Key(pmnav_key_id, name, description, data_type, is_deleted, effective_date) 
    VALUES(@pmnav_key_id, 'ledger_code', 'Ledger Code', 0, 0, '10/04/2004')
    END

    /*INSERT RECORD*/
    INSERT INTO PMNav_Set_Map_Key(pmnav_map_id, pmnav_key_id, sequence_number, description, initial_key_value) 
    VALUES (@pmnav_map_id, @pmnav_key_id, 5, 'Ledger Code',NULL )

    INSERT INTO PMNav_Set_Process_Key(pmnav_process_id, pmnav_key_id, sequence_number, description, initial_key_value) 
    VALUES (@pmnav_process_id, @pmnav_key_id, 5,'Ledger Code',NULL )	

   --pmnav_get_process_key
   --cash_list_id
   /*LOOKING UP SOURCE FIELD: PMNAV_KEY_ID */
    SELECT @pmnav_key_id = NULL
    SELECT @pmnav_key_id  = pmnav_key_id FROM PMNav_Key WHERE name = 'cashlist_id' AND data_type = 2

    IF @pmnav_key_id IS NULL
    BEGIN
        SELECT @pmnav_key_id = ISNULL(MAX(pmnav_key_id),0) + 1 FROM PMNav_Key
        INSERT INTO PMNav_Key(pmnav_key_id, name, description, data_type, is_deleted, effective_date) 
        VALUES(@pmnav_key_id, 'cashlist_id', 'Cash List ID', 2, 0, '10/04/2004')
    END

    /*INSERT RECORD */
    INSERT INTO pmnav_get_process_key(pmnav_process_id, pmnav_key_id, sequence_number, description) 
    VALUES (@pmnav_process_id, @pmnav_key_id, 1, 'Cash List ID')

    --cash_list_item_id
    SELECT @pmnav_key_id = NULL
    SELECT @pmnav_key_id  = pmnav_key_id FROM PMNav_Key WHERE name = 'cashlistitem_id' AND data_type = 2

    IF @pmnav_key_id IS NULL
    BEGIN
        SELECT @pmnav_key_id = ISNULL(MAX(pmnav_key_id),0) + 1 FROM PMNav_Key
        INSERT INTO PMNav_Key(pmnav_key_id, name, description, data_type, is_deleted, effective_date) 
        VALUES(@pmnav_key_id, 'cashlistitem_id', 'Cash List Item ID', 2, 0, '10/04/2004')
    END
   
    /*INSERT RECORD */
    INSERT INTO pmnav_get_process_key(pmnav_process_id, pmnav_key_id, sequence_number, description) 
    VALUES (@pmnav_process_id, @pmnav_key_id, 2, 'Cash List Item ID')

    --cash_list_type_id
    SELECT @pmnav_key_id = NULL
    SELECT @pmnav_key_id  = pmnav_key_id FROM PMNav_Key WHERE name = 'cashlisttype_id' AND data_type = 2

    IF @pmnav_key_id IS NULL
    BEGIN
        SELECT @pmnav_key_id = ISNULL(MAX(pmnav_key_id),0) + 1 FROM PMNav_Key
        INSERT INTO PMNav_Key(pmnav_key_id, name, description, data_type, is_deleted, effective_date) 
        VALUES(@pmnav_key_id, 'cashlisttype_id', 'Cash List Type ID', 2, 0, '10/04/2004')
    END

     /*INSERT RECORD */
    INSERT INTO pmnav_get_process_key(pmnav_process_id, pmnav_key_id, sequence_number, description) 
    VALUES (@pmnav_process_id, @pmnav_key_id, 3, 'Cash List Type ID')
    

    /*COPYING MAP STEPS... */
    INSERT INTO PMNav_step(
       pmnav_map_id, pmnav_step_id, sub_nav_map_id, pmnav_component_id,
       task,ok_action,cancel_action,ok_no_of_steps,
       cancel_no_of_steps, ok_nav_process_id,cancel_nav_process_id,navigate_status,
       is_hidden,is_logged,caption_id,description)
   SELECT 
      @pmnav_map_id, ns.pmnav_step_id, ns.sub_nav_map_id, ns.pmnav_component_id,  
      ns.task, ns.ok_action, ns.cancel_action, ns.ok_no_of_steps,  
      ns.cancel_no_of_steps, ns.ok_nav_process_id, ns.cancel_nav_process_id, ns.navigate_status,  
      ns.is_hidden,ns.is_logged,pmc.caption_id,pmc.caption   
   FROM 
   --Start Modification for SQL Server 2005 Compatibility
        pmnav_step ns
        LEFT OUTER JOIN pmnav_component nc
            ON ns.pmnav_component_id = nc.pmnav_component_id
        LEFT OUTER JOIN pmcaption pmc  
            ON ns.caption_id = pmc.caption_id  
            AND pmc.language_id = 1 
   WHERE 
       ns.pmnav_map_id = (SELECT MAX(pmnav_Map_id) FROM pmnav_map WHERE code ='ACTIPAYM1')
   AND ns.pmnav_component_id NOT IN 
       ( 
        SELECT pmnav_component_id 
        FROM pmnav_component 
        WHERE object_name IN ('bACTCashListPost', 'bACTAllocationPost')
       )
   --End Modification for SQL Server 2005 Compatibility
   ORDER BY pmnav_step_id ASC  
  

   EXECUTE spu_pm_caption_id_return 1, 'Print Banking Advice', @caption_id OUTPUT
   
   UPDATE pmnav_step
   SET caption_id = @caption_id,
   description = 'Print Banking Advice'
   WHERE pmnav_Map_id = 373
   AND pmnav_component_id = (SELECT pmnav_component_id FROM pmnav_component 
   WHERE object_name = 'iPMBReportPrint' AND class_name='NavigatorV3')

   --pmnav_set_step_key

   INSERT INTO pmnav_set_step_key 
     (pmnav_map_id,
      pmnav_step_id,
      pmnav_key_id,
      sequence_number,
      description,
      initial_key_value)
   SELECT  
      @pmnav_map_id,
      ssk.pmnav_step_id,
      CASE (select name from pmnav_key where pmnav_key_id =ssk.pmnav_key_id) 
      WHEN 'insurer_payment' THEN (SELECT pmnav_key_id FROM pmnav_key WHERE name ='banking_amount')
      ELSE ssk.pmnav_key_id
      END,
      ssk.sequence_number,
      CASE (select name from pmnav_key where pmnav_key_id =ssk.pmnav_key_id) 
      WHEN 'insurer_payment' THEN (SELECT description FROM pmnav_key WHERE name ='banking_amount')
      ELSE ssk.description
      END,
      ssk.initial_key_value  
   FROM pmnav_set_step_key ssk
     
   WHERE ssk.pmnav_map_id = (SELECT MAX(pmnav_Map_id) FROM pmnav_map WHERE code ='ACTIPAYM1') 
   AND PMNav_step_id NOT IN 
   (
    SELECT PMNav_step_id 
    FROM PMNav_step 
    WHERE pmnav_map_id = (SELECT MAX(pmnav_Map_id) FROM pmnav_map WHERE code ='ACTIPAYM1')
    AND pmnav_component_id IN (SELECT pmnav_component_id 
      			      FROM pmnav_component 
                              WHERE object_name IN ('bACTCashListPost', 'bACTAllocationPost')
                              )
   )


  UPDATE pmnav_set_step_key
  SET initial_key_value = 'RECEIPT'
  WHERE pmnav_map_id = @pmnav_map_id
  AND initial_key_value = 'PAYMENT'

  UPDATE pmnav_set_step_key
  SET initial_key_value='ACTRCTV2'
  WHERE pmnav_map_id = @pmnav_map_id
  AND initial_key_value = 'ACTINSPAY2'

  UPDATE pmnav_set_step_key
  SET initial_key_value = 'Banking'
  WHERE pmnav_map_id = @pmnav_map_id
  AND initial_key_value='RemittanceAdvice'

  INSERT INTO pmnav_get_step_key
    (
    pmnav_map_id,
    pmnav_step_id,
    pmnav_key_id,
    sequence_number,
    description)
  SELECT 
    @pmnav_map_id,
    pmnav_step_id,
    pmnav_key_id,
    sequence_number,
    description 
  FROM pmnav_get_step_key
  WHERE pmnav_map_id = (SELECT MAX(pmnav_map_id) FROM pmnav_map WHERE code ='ACTIPAYM1')

   SELECT @pmnav_batch_id=MAX(pmnav_batch_id)+1 FROM PMNav_Batch
   SELECT @PMNav_key_id = PMNav_key_id FROM PMNav_key WHERE name ='batch_set_id'

   INSERT INTO PMNav_Batch
   (pmnav_batch_id,code,description,is_deleted, effective_date)
   VALUES (@pmnav_batch_id, 'BANKING', 'Banking',0,getdate() )
  
   INSERT INTO PMNav_Batch_Key (pmnav_batch_id, pmnav_key_id,sequence_number)
   VALUES (@pmnav_batch_id,@PMNav_key_id,1)
   END
END
GO

-- *****************************************************************************
-- * Author:  Sumit Bhardwaj
-- * Date:    16/04/2008
-- * Purpose: Added Merge Code For branch address
-- *****************************************************************************

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='Address1')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,product_family)
    VALUES ('Address1','spu_wp_Partyall','branch_address1',0,'Party','All','Branch Address 1',1,9)
END
GO


IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='Address2')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,product_family)
    VALUES ('Address2','spu_wp_Partyall','branch_address2',0,'Party','All','Branch Address 2',1,9)
END
GO


IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='Address3')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,product_family)
    VALUES ('Address3','spu_wp_Partyall','branch_address3',0,'Party','All','Branch Address 3',1,9)
END
GO

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='Address4')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,product_family)
    VALUES ('Address4','spu_wp_Partyall','branch_address4',0,'Party','All','Branch Address 4',1,9)
END
GO

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PostalCode')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,product_family)
    VALUES ('PostalCode','spu_wp_Partyall','branch_postal_code',0,'Party','All','Branch Postal Code',1,9)
END
GO

-- *****************************************************************************
-- * Author  : Daniel Morey
-- * Date    : 18/04/2008       
-- * Purpose : Issue 43433
-- *****************************************************************************

UPDATE pfscheme 
SET receipt_difference_option = 0
WHERE receipt_difference_option IS NULL

GO

-- *****************************************************************************  
-- * Author:       Deepak Mittal
-- * Date:         23-03-2008
-- * Purpose:      PVY London Market Development Changes
-- *****************************************************************************
UPDATE wp_fields 
SET display_name = 'Total Written Line Percentage' 
WHERE display_name = 'Total Written Percentage'

GO

UPDATE wp_fields 
SET display_name = 'Total Signed Line Percentage' 
WHERE display_name = 'Total Signed Percentage'

GO

-- *****************************************************************************
-- * Author:  Nitin Suri
-- * Date:    15/05/2008       
-- * Purpose: PN 43734 Updated Payment Method of Debit Note to  
-- * fetch details from spu_wp_debitheader         
-- *****************************************************************************

IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 AND value = 'A')
  BEGIN
    UPDATE wp_fields SET sql = 'spu_wp_debitheader'
    WHERE field_name = 'DNPaymentMethod'
    AND sub_group='Debit Note'
    AND sql <> 'spu_wp_debitheader'
  END

GO

-- *****************************************************************************
-- * Author:        Pankaj Kaushik
-- * Date:          19-05-2008
-- * Purpose:       Renewal Printing
-- *****************************************************************************
    IF NOT EXISTS (SELECT * FROM Process_Type Where code = 'RNI') BEGIN
	DECLARE @caption_id int
	DECLARE @Process_type_id int

        EXEC spu_pm_caption_id_return 1, 'RNI', @caption_id OUTPUT

		SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type 
        -- Add new New Business Description
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging)
            VALUES(@Process_type_id, @caption_id, 'RNI','Renewal Invite',0, getdate(),0)
    END
	
    UPDATE Process_type SET description = 'Renewal Accept' WHERE code = 'RN'
		
    GO
    -- *****************************************************************************
    -- * Author:  Brajesh Kumar
    -- * Date:    19/05/2008
    -- * Purpose: Delete  Report Renewal_List.rpt in report table
    -- *****************************************************************************
    IF EXISTS(SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'A')
    BEGIN
    
    	DELETE FROM  Report_Group_Contents  WHERE report_id in 
    		(SELECT report_id FROM Report WHERE Report_name = 'Renewal_List.rpt')
    	DELETE FROM Report WHERE Report_name = 'Renewal_List.rpt'
    	DELETE FROM PMCaption WHERE caption ='Renewal List'
    END
GO

-- *****************************************************************************
-- * Author:  Shankh Dhar Dubey
-- * Date:    20/05/2008
-- * Purpose: PN43964 Change the display name
-- *****************************************************************************

UPDATE    wp_fields
SET              display_name = 'Period Cover Start Date'
WHERE     (field_name='coverstartdate')

-- *****************************************************************************
-- * Author: 		Pankaj Kaushik
-- * Date: 		    20/05/2008
-- * Purpose:		Account Function & CCY Cash Allocation
-- *****************************************************************************
DECLARE @COUNT INT
SELECT @COUNT = COUNT(*) FROM BankAccount_Source
IF ISNULL(@COUNT,0) = 0
BEGIN
	INSERT INTO BankAccount_Source (bankaccount_id,source_id) SELECT b.bankaccount_id,s.source_id from bankaccount b,source s
    WHERE b.is_deleted = 0 AND s.is_deleted = 0
END

-- *****************************************************************************
-- * Author:  Shankh Dhar Dubey
-- * Date:    26/05/2008       
-- * Purpose: PN 43758
-- *****************************************************************************

IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 AND value = 'A')
  BEGIN
	UPDATE wp_fields 
	SET loop1 = 'debitfeedetail' 
	WHERE sql = 'spu_wp_debitfeedetail'
  END
  
GO

-- *****************************************************************************
-- * Author:  Rajesh Jawane
-- * Date:    22/05/2008
-- * Purpose: Delete Insurer Statement Currency Report
-- *****************************************************************************
IF EXISTS(SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN
	DELETE FROM Report_Group_Contents WHERE report_id in 
		(SELECT report_id FROM Report WHERE report_name = 'Insurer_Statement_Currency.rpt')
	DELETE FROM Report WHERE report_name = 'Insurer_Statement_Currency.rpt'
END
GO
-- *****************************************************************************
-- * Author:        Deepak Arora
-- * Date:          20/05/2008
-- * Purpose:       WR5 - Claims WorkFlow
-- *****************************************************************************
IF EXISTS (Select * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
    BEGIN
	-- check one of the options for existence at system level
	IF NOT EXISTS (Select 1 From Product_Claims_Workflow)
	Begin
	    Update Product Set
		Claim_Value_For_Large_Loss_Advice = (Select TOP 1 cast(value as Money) From system_options Where option_number = 1014 And branch_id = 1),
		Inclusion_of_CoInsurers_On_Claims = (Select TOP 1 value From system_options Where option_number = 1015 And branch_id = 1),
		Allow_Negative_Reserve = (Select TOP 1 value From system_options Where option_number = 1016 And branch_id = 1),
		Ext_Clm_Handler_Acknowledged_Task_Allowed_Time = (Select TOP 1 value From system_options Where option_number = 1017 And branch_id = 1),
		Ext_Clm_Handler_Supply_Pre_Report_Task_Allowed_Time = (Select TOP 1 value From system_options Where option_number = 1018 And branch_id = 1),
		Valid_Policy_Version_At_Loss_Date = (Select TOP 1 value From system_options Where option_number = 1025 And branch_id = 1),
		Is_Gross_Claim_Payment_Amount = (Select TOP 1 value From system_options Where option_number = 5018 And branch_id = 1),
		Claim_Task_Group = (Select TOP 1 value From system_options Where option_number = 1019 And branch_id = 1),
		Claim_User_Group = (Select TOP 1 value From system_options Where option_number = 1020 And branch_id = 1),
		Claims_UDT_A = (Select TOP 1 value From system_options Where option_number = 2003 And branch_id = 1),
		Claims_UDT_B = (Select TOP 1 value From system_options Where option_number = 2004 And branch_id = 1),
		Claims_UDT_C = (Select TOP 1 value From system_options Where option_number = 2005 And branch_id = 1),
		Claims_UDT_D = (Select TOP 1 value From system_options Where option_number = 2006 And branch_id = 1),
		Claims_UDT_E = (Select TOP 1 value From system_options Where option_number = 2007 And branch_id = 1),
		Is_Duplicate_Claim_Check_Enabled = (Select TOP 1 value From system_options Where option_number = 5002 And branch_id = 1),
		Is_Advanced_Tax_Script_Enabled = (Select TOP 1 value From system_options Where option_number = 5007 And branch_id = 1),
		Is_Payment_Ref_Check_Enabled = (Select TOP 1 value From system_options Where option_number = 5040 And branch_id = 1)
	
				-- Open Claim Workflow
		    INSERT INTO Product_Claims_Workflow 
			(product_id, claim_process_type_id, check_unpaid_status, reinsurance_recovery,
			salvage_recovery, third_party_recovery, external_claim_handling, description_for_change_in_reserve,
			claim_notification_doc_message, generate_claim_notification_doc, claim_payment_process, 
			check_deferred_reinsurance, fast_track_claims, reinsurance_payment, 
			description_for_change_in_payment, cash_payment_process, claim_payment_doc_message, 
			generate_claim_payment_doc, make_further_payments)
		    SELECT product_id, 1, 1, 1, 1, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 
			FROM Product 
	
		    -- Maintain Claim Workflow
		    INSERT INTO Product_Claims_Workflow 
			(product_id, claim_process_type_id, check_unpaid_status, reinsurance_recovery,
			salvage_recovery, third_party_recovery, external_claim_handling, description_for_change_in_reserve,
			claim_notification_doc_message, generate_claim_notification_doc, claim_payment_process, 
			check_deferred_reinsurance, fast_track_claims, reinsurance_payment, 
			description_for_change_in_payment, cash_payment_process, claim_payment_doc_message, 
			generate_claim_payment_doc, make_further_payments)
		    SELECT product_id, 2, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 
			FROM Product
	
		    -- Pay Claim Workflow
		    INSERT INTO Product_Claims_Workflow 
			(product_id, claim_process_type_id, check_unpaid_status, reinsurance_recovery,
			salvage_recovery, third_party_recovery, external_claim_handling, description_for_change_in_reserve,
			claim_notification_doc_message, generate_claim_notification_doc, claim_payment_process, 
			check_deferred_reinsurance, fast_track_claims, reinsurance_payment, 
			description_for_change_in_payment, cash_payment_process, claim_payment_doc_message, 
			generate_claim_payment_doc, make_further_payments)
		    SELECT product_id, 3, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 1, 1 
			FROM Product 
			

			DECLARE @value int
			SELECT @value= ISNULL(value,0) FROM System_options Where option_number = 5017 And branch_id = 1
			
			Update Product_Claims_Workflow 
				Set cash_payment_process= @value WHERE  claim_process_type_id = 3
    END
	END
	GO

	Delete From system_options Where option_number IN
		(1014, 1015, 1016, 1017, 1018, 1019, 1020, 1025, 5017, 5018, 
		2002, 2003, 2004, 2005, 2006, 2007, 2008, 2014, 5002, 5007, 5040)
GO

-- *****************************************************************************
-- * Author:    Sumit Bhardwaj
-- * Date:      03/06/2008
-- * Purpose:   Updating wp_fields table field column_type=0
                
-- *****************************************************************************

BEGIN

    UPDATE wp_fields 
    SET column_type=0
    WHERE column_name='alcohol_measurement_method' AND main_group='party' AND sub_group='Conviction' AND field_name='PartyConvictionAlcoholMeasurementMethod'

END
GO

-- *****************************************************************************
-- * Author:    Sumit Bhardwaj
-- * Date:      03/06/2008
-- * Purpose:   Added Merge Code For County Court Judgement 
                
-- *****************************************************************************

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyCountyCourtJudgements')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('PartyCountyCourtJudgements','spu_wp_PartyConviction','ccjs',0,'Party','Conviction','County Court Judgements',1,'PartyConviction',9)

END

GO

-- *****************************************************************************
-- * Author:  Brajesh Kumar
-- * Date:    04/06/2008
-- * Purpose: Delete  Audit Report for Direct to Insurer Reversals From report Report List
-- *****************************************************************************
IF EXISTS(SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN

	DELETE FROM  Report_Group_Contents  WHERE report_id in 
		(SELECT report_id FROM Report WHERE Report_name = 'Audit_Report_For_Direct_to_Insurer_Reversals.rpt')
	DELETE FROM Report WHERE Report_name = 'Audit_Report_For_Direct_to_Insurer_Reversals.rpt'
	DELETE FROM PMCaption WHERE caption ='Audit Report for Direct to Insurer Reversals'
END
GO

-- *****************************************************************************
-- * Author:  Brajesh Kumar
-- * Date:    10/06/2008
-- * Purpose: For PN-44850
-- *****************************************************************************
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 AND value = 'A')
  BEGIN
	UPDATE DOCUMENT SET payment_due_date = cover_start_date 
	FROM  Document INNER JOIN
	Insurance_File ON Document.insurance_file_cnt = Insurance_File.insurance_file_cnt
	WHERE  ISNULL(Document.payment_due_date,'') = '' or Document.payment_due_date ='1899-12-29 00:00:00.000'
  END
 
GO 


-- *****************************************************************************  
-- * Author:      Deepak
-- * Date:        14-06-2008
-- * Purpose:     Claims WorkFlow
-- *****************************************************************************


IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN

    DELETE FROM pmb_doc_link where Process_Type_Id in (
	SELECT Process_type_id FROM process_type WHERE code in('MTA_Q','PRERENSEL','RENSEL','REN_Q'))

    DELETE FROM process_type where code in ('MTA_Q','PRERENSEL','RENSEL','REN_Q')
    
    DECLARE @Process_Type_id INT
    DECLARE @caption_id      INT

    IF NOT EXISTS (SELECT 1 FROM Process_Type WHERE CODE='NB') 
    BEGIN
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'NB', @caption_id OUTPUT
        --Function Area  : 1
        SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type 
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging)
            VALUES(@Process_type_id, @caption_id, 'NB','New Business',0, getdate(),0)    
    END

    IF NOT EXISTS (SELECT 1 FROM Process_Type WHERE CODE='AP') 
    BEGIN
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'AP', @caption_id OUTPUT
        SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type 
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging)
            VALUES(@Process_type_id, @caption_id, 'AP','MTA Additional Premium',0, getdate(),0)    
    END

    IF NOT EXISTS (SELECT 1 FROM Process_Type WHERE CODE='RP') 
    BEGIN
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'RP', @caption_id OUTPUT
        SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type 
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging)
            VALUES(@Process_type_id, @caption_id, 'RP','MTA Return Premium',0, getdate(),0)    
    END


    IF NOT EXISTS (SELECT 1 FROM Process_Type WHERE CODE='ZP') 
    BEGIN
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'ZP', @caption_id OUTPUT
        SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type 
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging)
            VALUES(@Process_type_id, @caption_id, 'ZP','MTA Zero Premium',0, getdate(),0)    
    END

    IF NOT EXISTS (SELECT 1 FROM Process_Type WHERE CODE='RI') 
    BEGIN
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'RI', @caption_id OUTPUT
        SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type 
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging)
            VALUES(@Process_type_id, @caption_id, 'RI','MTA Reinstatement',0, getdate(),0)    
    END

    IF NOT EXISTS (SELECT 1 FROM Process_Type WHERE CODE='RN') 
    BEGIN
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'RN', @caption_id OUTPUT
        SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type 
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging)
            VALUES(@Process_type_id, @caption_id, 'RN','Renewal',0, getdate(),0)    
    END

    UPDATE Process_Type SET Functional_area =1 WHERE Functional_Area IS NULL
 
    IF NOT EXISTS (SELECT 1 FROM Process_Type WHERE CODE='LA') 
    BEGIN
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'LA', @caption_id OUTPUT
         -- Add new New Business Description
        --Function Area  : Open/Main Claim = 2
        SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type 
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging,Functional_Area)
            VALUES(@Process_type_id, @caption_id, 'LA','Loss Advice',0, getdate(),0,2)    
    END

    IF NOT EXISTS (SELECT 1 FROM Process_Type WHERE CODE='LL') 
    BEGIN
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'LL', @caption_id OUTPUT
         -- Add new New Business Description
        --Function Area  : Open/Main Claim = 2
        SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type         
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging,Functional_Area)
            VALUES(@Process_type_id, @caption_id, 'LL','Large Loss Advice',0, getdate(),0,2)    
    END

    IF NOT EXISTS (SELECT 1 FROM Process_Type WHERE CODE='CJ') 
    BEGIN
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'CJ', @caption_id OUTPUT
         -- Add new New Business Description
        --Function Area  : Open/Main Claim = 2
        SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type 
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging,Functional_Area)
            VALUES(@Process_type_id, @caption_id, 'CJ','Claim Jacket',0, getdate(),0,2)    
    END

    IF NOT EXISTS (SELECT 1 FROM Process_Type WHERE CODE='CC') 
    BEGIN
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'CC', @caption_id OUTPUT
         -- Add new New Business Description
        --Function Area  : Open/Main Claim = 2
        SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type 
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging,Functional_Area)
            VALUES(@Process_type_id, @caption_id, 'CC','Claim notification to Client',0, getdate(),0,2)    
    END

    IF NOT EXISTS (SELECT 1 FROM Process_Type WHERE CODE='CA') 
    BEGIN
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'CA', @caption_id OUTPUT
         -- Add new New Business Description
        --Function Area  : Open/Main Claim = 2
        SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type 
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging,Functional_Area)
            VALUES(@Process_type_id, @caption_id, 'CA','Claim notification to Agent',0, getdate(),0,2)    
    END

    IF NOT EXISTS (SELECT 1 FROM Process_Type WHERE CODE='CI') 
    BEGIN
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'CI', @caption_id OUTPUT
         -- Add new New Business Description
        --Function Area  : Open/Main Claim = 2
        SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type 
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging,Functional_Area)
            VALUES(@Process_type_id, @caption_id, 'CI','Claim notification to Insurer',0, getdate(),0,2)    
    END

    IF NOT EXISTS (SELECT 1 FROM Process_Type WHERE CODE='EH') 
    BEGIN
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'EH', @caption_id OUTPUT
         -- Add new New Business Description
        --Function Area  : Open/Main Claim = 2
        SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type 
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging,Functional_Area)
            VALUES(@Process_type_id, @caption_id, 'EH','External Handler Notification',0, getdate(),0,2)    
    END

    --Docs For claim Payment , functional Area :=4
    IF NOT EXISTS (SELECT 1 FROM Process_Type WHERE CODE='AS') 
    BEGIN
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'AS', @caption_id OUTPUT
         -- Add new New Business Description
        --Function Area  : Open/Main Claim = 2
        SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type 
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging,Functional_Area)
            VALUES(@Process_type_id, @caption_id, 'AS','Claim Advice to Agent',0, getdate(),0,4)    
    END

    IF NOT EXISTS (SELECT 1 FROM Process_Type WHERE CODE='CQ') 
    BEGIN
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'CQ', @caption_id OUTPUT
         -- Add new New Business Description
        --Function Area  : Open/Main Claim = 2
        SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type 
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging,Functional_Area)
            VALUES(@Process_type_id, @caption_id, 'CQ','Cheque Requisition',0, getdate(),0,4)    
    END

    IF NOT EXISTS (SELECT 1 FROM Process_Type WHERE CODE='AF') 
    BEGIN
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'AF', @caption_id OUTPUT
         -- Add new New Business Description
        --Function Area  : Open/Main Claim = 2
        SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type 
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging,Functional_Area)
            VALUES(@Process_type_id, @caption_id, 'AF','Claim Acceptance Form',0, getdate(),0,4)    
    END

    IF NOT EXISTS (SELECT 1 FROM Process_Type WHERE CODE='NT') 
    BEGIN
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'NT', @caption_id OUTPUT
         -- Add new New Business Description
        --Function Area  : Open/Main Claim = 2
        SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type 
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging,Functional_Area)
            VALUES(@Process_type_id, @caption_id, 'NT','Advice to Reinsurer',0, getdate(),0,4)    
    END

    IF NOT EXISTS (SELECT 1 FROM Process_Type WHERE CODE='CP') 
    BEGIN
        -- Get caption
        EXEC spu_pm_caption_id_return 1, 'CP', @caption_id OUTPUT
         -- Add new New Business Description
        --Function Area  : Open/Main Claim = 2
        SELECT @Process_type_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type 
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging,Functional_Area)
            VALUES(@Process_type_id, @caption_id, 'CP','Claim Payment Advice',0, getdate(),0,4)    
    END
END



GO

--**********************************************************************************************  
-- Author : Deepak Arora
--   
-- History: 14/06/2008    
--
-- Task : Claims WorkFlow
--**********************************************************************************************  

DECLARE @COUNT INT
SELECT @COUNT = COUNT(*) FROM PMB_Doc_Link WHERE functional_Area = 2
IF ISNULL(@COUNT,0) = 0
BEGIN
	--Create a temp table and set all relevant records (where source_id = 0) of table doucment_template 
	--for all source
	CREATE TABLE tmpDocument_Template(
		Document_Type_Id INT,  
		Document_Template_Id INT,  
		source_id INT,
		code varchar(40))  
	    INSERT INTO tmpDocument_Template SELECT Document_Type_Id,Document_Template_Id,source_id,code 
		FROM document_template where source_id <> 0 AND is_deleted = 0
	
	Declare @tmp_source_id INT
	DECLARE	c_cursor SCROLL CURSOR FOR SELECT source_id FROM source WHERE is_deleted = 0
	OPEN c_cursor
	FETCH NEXT FROM c_cursor INTO @tmp_source_id
	WHILE @@FETCH_STATUS = 0
	BEGIN
	    INSERT INTO tmpDocument_Template SELECT Document_Type_Id,Document_Template_Id,@tmp_source_id,code 
		FROM document_template where source_id = 0 AND is_deleted = 0
	
		FETCH NEXT FROM c_cursor INTO @tmp_source_id
	END
	
	CLOSE 		c_cursor
	DEALLOCATE	c_cursor
	
	CREATE TABLE temp_PMBDocLink(
		GIS_Scheme_Id INT,
		Process_Type_Id INT,  
		Document_Type_Id INT,  
		Document_Template_Id INT,  
		Agent_Cnt INT,
		spool_document TINYINT,  
		Auto_Archive_Document TINYINT,
		process_types_docs_id INT,  
		functional_area TINYINT,  
		product_id INT,  
		source_id INT,  
		is_client TINYINT,  
		is_agent TINYINT,  
		is_office TINYINT,  
		production_order TINYINT,
		report_pointer INT,
		document_pointer VARCHAR(20))
	
		DECLARE @PMB_Doc_Link_Id INT,
		@GIS_Scheme_Id INT,
		@Process_Type_Id INT,  
		@Document_Type_Id INT,  
		@Document_Template_Id INT,  
		@Agent_Cnt INT,
		@spool_document TINYINT,  
		@Auto_Archive_Document TINYINT,
		@process_types_docs_id INT,  
		@functional_area TINYINT,  
		@product_id INT,  
		@source_id INT,  
		@is_client TINYINT,  
		@is_agent TINYINT,  
		@is_office TINYINT,  
		@production_order TINYINT,
		@report_pointer INT,
	    @document_pointer VARCHAR(20)
		
	DECLARE	c_cursor SCROLL CURSOR FOR SELECT product_id,isnull(report_pointer,0) FROM product
	OPEN c_cursor
	FETCH NEXT FROM c_cursor INTO @product_id,@report_pointer
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	    INSERT INTO temp_PMBDocLink
	    SELECT 
	    	
	    1,  --GIS_Scheme_ID
	    CASE isnumeric(Left(dt.code,1)) --Process_Type_id
	        WHEN 1 THEN 
			(SELECT process_type_id FROM process_type where code = SUBSTRING(dt.code,5,2))
	  	    ELSE
			(SELECT process_type_id FROM process_type where code = SUBSTRING(dt.code,4,2))
		    END,
	    dt.document_type_id, --Document_Type_Id
	    dt.document_template_id, --Document_Template_ID
      	    NULL, --Agent_Cnt
	    1, --Spool_Document
	    0, --Auto_Archive_Document
        CASE isnumeric(Left(dt.code,1)) --Process_Types_Docs_Id
            WHEN 1 THEN 
                (SELECT process_types_docs_id FROM process_types_docs where code = SUBSTRING(dt.code,2,3))
            ELSE
                (SELECT process_types_docs_id FROM process_types_docs where code = SUBSTRING(dt.code,1,3))
            END,
	    1, --Functional_Area

	    @product_id, --Product_Id
	    dt.source_id, -- Source_Id
        CASE LEFT(dt.code,1)  --for is_client if first character is 1
            WHEN '1' THEN 
                1
            ELSE
                0
            END,
        CASE LEFT(dt.code,1)  --for is_agent if first character is 2
            WHEN '2' THEN 
                1
            ELSE
                0
            END,
        CASE LEFT(dt.code,1)  --for is_office if first character is 3
            WHEN '3' THEN 
                1
            ELSE
                0
            END,
	    1,  -- Production_Order
	    @report_pointer, --report_pointer as configured in product table
        CASE ISNUMERIC(Left(dt.code,1))
            WHEN 1 THEN 
				(select REPLACE(dt.code, substring(dt.code,1,6),''))
            ELSE
				(select REPLACE(dt.code, substring(dt.code,1,5),''))
            END
	
		FROM tmpDocument_Template AS dt
        
		FETCH NEXT FROM c_cursor INTO @product_id,@report_pointer
	END
	
	CLOSE 		c_cursor
	DEALLOCATE	c_cursor
	
	DELETE FROM temp_PMBDocLink WHERE process_type_id is NULL
	DELETE FROM temp_PMBDocLink WHERE process_types_docs_id is NULL
	DELETE FROM temp_PMBDocLink WHERE source_id = 0
	DELETE FROM temp_PMBDocLink WHERE document_pointer <> convert(varchar(20),report_pointer)
	
    UPDATE temp_PMBDocLink SET FUNCTIONAL_Area =(SELECT Functional_area FROM Process_Type 
    WHERE temp_PMBDocLink.Process_Type_ID = Process_Type.Process_Type_ID)

    DELETE FROM temp_PMBDocLink WHERE Functional_Area <> 2 

	DECLARE	c_cursor SCROLL CURSOR FOR SELECT * FROM temp_PMBDocLink
	OPEN c_cursor
	FETCH NEXT FROM c_cursor INTO 
		 @GIS_Scheme_Id,
		 @Process_Type_Id,  
		 @Document_Type_Id,  
		 @Document_Template_Id,  
		 @Agent_Cnt,
		 @spool_document,  
		 @Auto_Archive_Document,
		 @process_types_docs_id,  
		 @functional_area,  
		 @product_id,  
		 @source_id,  
		 @is_client,  
		 @is_agent,  
		 @is_office,  
		 @production_order,
		 @report_pointer,
	     @document_pointer

	
	WHILE	@@FETCH_STATUS = 0
	BEGIN
		
		INSERT INTO PMB_Doc_Link(
		     GIS_Scheme_Id,
		     Process_Type_Id,  
		     Document_Type_Id,  
		     Document_Template_Id,  
		     Agent_Cnt,
		     spool_document,  
		     Auto_Archive_Document,
		     process_types_docs_id,  
		     functional_area,  
		     product_id,  
		     source_id,  
		     is_client,  
		     is_agent,  
		     is_office,  
		     production_order)
		VALUES(
		     @GIS_Scheme_Id,
		     @Process_Type_Id,  
		     @Document_Type_Id,  
		     @Document_Template_Id,  
		     @Agent_Cnt,
		     @spool_document,  
		     @Auto_Archive_Document,
		     @process_types_docs_id,  
		     @functional_area,  
		     @product_id,  
		     @source_id,  
		     @is_client,  
		     @is_agent,  
		     @is_office,  
		     @production_order)
		FETCH NEXT FROM c_cursor INTO 
		     @GIS_Scheme_Id,
		     @Process_Type_Id,  
		     @Document_Type_Id,  
		     @Document_Template_Id,  
		     @Agent_Cnt,
		     @spool_document,  
		     @Auto_Archive_Document,
		     @process_types_docs_id,  
		     @functional_area,  
		     @product_id,  
		     @source_id,  
		     @is_client,  
		     @is_agent,  
		     @is_office,  
		     @production_order,
			 @report_pointer,
	    	 @document_pointer
	END
	CLOSE 		c_cursor
	DEALLOCATE	c_cursor
	DROP TABLE temp_PMBDocLink
	DROP TABLE tmpDocument_Template

END
GO

-- *****************************************************************************
-- * Author:        Samrendu Bhushan
-- * Date:          26-05-2008
-- * Purpose:       Agent Payments
-- *****************************************************************************
IF NOT EXISTS (SELECT * FROM Party_Category Where code = 'NONB') BEGIN
    DECLARE @caption_id int
    EXEC spu_pm_caption_id_return 1, 'No New Business', @caption_id OUTPUT

    -- Add new New Business Description
    INSERT Party_Category(party_category_id, caption_id,code,description,is_deleted, effective_date)
         VALUES(4, @caption_id, 'NONB','No New Business',0, getdate())
    END
		
GO

-- *****************************************************************************
-- * Author:        Gurucharan Gulati
-- * Date:          19-06-2008
-- * Purpose:       Batch Renewals-Multi-Threaded Controller
-- *****************************************************************************
IF NOT EXISTS (SELECT lookup_table_name FROM PMProduct_Lookup WHERE lookup_table_name = 'Batch_Renewal_Job_Type')
BEGIN
        INSERT INTO PMProduct_Lookup
                     (pmproduct_id, lookup_table_name, edit_privilege_level, is_generic_maintenance, linked_data_mandatory)
        VALUES       (2, 'Batch_Renewal_Job_Type', 3, 0, 0)
END
GO

-- * Purpose:To add default data in Lookup Table-Batch_Renewal_Job_Type
DECLARE @lCaptionID INT
IF NOT EXISTS(SELECT NULL FROM Batch_Renewal_Job_Type WHERE code = 'SEL')
BEGIN
	EXEC spu_pm_caption_id_return 1, 'Selection', @lCaptionID OUTPUT
	INSERT INTO Batch_Renewal_Job_Type(batch_renewal_job_type_id,code,description,caption_id,effective_date,is_deleted)
	VALUES (1,'SEL','Selection',@lCaptionID,GETDATE(),0)
END

IF NOT EXISTS(SELECT NULL FROM Batch_Renewal_Job_Type WHERE code = 'INV')
BEGIN
	EXEC spu_pm_caption_id_return 1, 'Invitation', @lCaptionID OUTPUT
	INSERT INTO Batch_Renewal_Job_Type(batch_renewal_job_type_id,code,description,caption_id,effective_date,is_deleted)
	VALUES (2,'INV','Invitation',@lCaptionID,GETDATE(),0)
END

IF NOT EXISTS(SELECT NULL FROM Batch_Renewal_Job_Type WHERE code = 'ACC')
BEGIN
	EXEC spu_pm_caption_id_return 1, 'Acceptance', @lCaptionID OUTPUT
	INSERT INTO Batch_Renewal_Job_Type(batch_renewal_job_type_id,code,description,caption_id,effective_date,is_deleted)
	VALUES (3,'ACC','Acceptance',@lCaptionID,GETDATE(),0)
END
GO

-- * Purpose:To add a new task for Batch Renewal Jobs Interface
DECLARE @lCaptionID  INT
DECLARE @TaskGroupId INT
DECLARE @TaskId      INT
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task WHERE code = 'BATCHREN')
    BEGIN
               EXECUTE spu_pm_caption_id_return 1, 'Batch Renewal Configuration', @lCaptionID OUTPUT

               INSERT INTO PMWrk_Task 
                        (caption_id, code, description, is_deleted, effective_date, 
                        is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, 
                        auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, 
                        linked_caption_id, is_available_task, pmwrk_task_category_id) 
               VALUES( @lCaptionID, 'BATCHREN', 'Batch Renewal Configuration', 0,GETDATE(), 
			0, 1, NULL, 'iPMUBatchRenewalJobs', 'NavigatorV3', 
			0, 6, 0, NULL, NULL, 
			NULL, 1, 2)
    END

    -- Create link to group for this new task
    SELECT @TaskId = (SELECT pmwrk_task_id FROM pmwrk_task WHERE code = 'BATCHREN')

    -- Add task to correct group
    SELECT @TaskGroupId = (SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'UNDER')

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task_Group_Task WHERE pmwrk_task_group_id = @TaskGroupId AND pmwrk_task_id  = @TaskId)
    BEGIN
       INSERT INTO pmwrk_task_group_task
       (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num)
       VALUES
       (@TaskGroupId, @TaskId, 0)
    END
END

GO

-- *****************************************************************************
-- * Author:  Brajesh Kumar
-- * Date:    20/06/2008
-- * Purpose: Change Sp of DebitFeeTax Merge Code
-- *****************************************************************************
IF EXISTS(SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN
	UPDATE wp_fields SET sql= 'spu_wp_debitfeedetail'
 	WHERE field_name ='DebitFeeTax'
		OR field_name ='DebitFeeTaxGroup'
		OR field_name ='DebitFeeTaxGroupCode'
	
END
GO
-- *****************************************************************************
-- * Author:  Brajesh Kumar
-- * Date:    20/06/2008
-- * Purpose: Update commissionPercentage SQL field
-- *****************************************************************************
IF EXISTS(SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN

	UPDATE wp_fields SET sql ='Spu_wp_insurancefileTotalCommissionPercentage' 
	WHERE field_name= 'CommissionPercentage'
END
GO


-- *****************************************************************************
-- * Author:  Sumit Bhardwaj
-- * Date:    20/06/2008
-- * Purpose: Update gis_screen_detail fields 
-- *****************************************************************************
IF EXISTS(SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'A')
BEGIN

	UPDATE gis_screen_detail
	
	SET pre_quote_requirement = 1
	
	WHERE ISNULL(pre_quote_requirement, 0)=0
	
	AND gis_property_id IS NOT NULL
	
		
	UPDATE gis_screen_detail
	
	SET  post_quote_requirement=1
	
	WHERE ISNULL(post_quote_requirement, 0)=0
	
	AND gis_property_id IS NOT NULL
	
		
	UPDATE gis_screen_detail
	
	SET  purchase_requirement=1
	
	WHERE ISNULL(purchase_requirement, 0)=0
	
	AND gis_property_id IS NOT NULL
	

END
GO
-- *****************************************************************************
-- * Author:  Deepak Arora
-- * Date:    23/06/2008
-- * Purpose: Include Agent as Part of Instalment Plan Maintenance
-- *****************************************************************************

DECLARE @keyid INT,    
        @pmnav_map_id INT,
        @pmnav_step_id INT 
IF EXISTS(SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
IF NOT EXISTS(SELECT 1 FROM pmnav_key WHERE Name like 'include_agent')
BEGIN
    
    Select @keyid = ISNULL(MAX(pmnav_key_id),0)+1 FROM pmnav_key
    Select @pmnav_map_id =pmnav_map_id From pmnav_map where code='PFPLNMAINT'
    Select @pmnav_step_id= min(pmnav_step_id) From pmNav_step Where pmnav_map_id=@pmnav_map_id

    INSERT INTO pmnav_key(pmnav_key_id,name,description,data_type,is_deleted,effective_date)
    Values
    (@keyid, 'include_agent','include_agent',0,0,getdate()) 
    INSERT INTO  pmnav_set_step_key (pmnav_map_id,pmnav_step_id,pmnav_key_id,sequence_number,description,initial_key_value)
    values
    (@pmnav_map_id,@pmnav_step_id,@keyid,1,'include_agent','1')
END
END

GO
--**********************************************************************************************  
-- Author : Roopaly Rastogi
-- History: 23/06/2008    
-- Task : PN 45639
--**********************************************************************************************  

DECLARE @keyid INT,    
        @pmnav_map_id INT,
        @pmnav_step_id INT 
IF EXISTS(SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
    Select @keyid = ISNULL(MAX(pmnav_key_id),0)+1 FROM pmnav_key
    Select @pmnav_map_id =pmnav_map_id From pmnav_map where code='PFPLNMAINT'
    Select @pmnav_step_id= pmnav_step_id From pmNav_step Where pmnav_map_id=@pmnav_map_id and description ='Finance Plan Maintenance'
    
    Update pmNav_step set task = 0 where pmnav_map_id=@pmnav_map_id and pmnav_step_id = @pmnav_step_id	
END
GO
-- *****************************************************************************
-- * Author: 		Samrendu Bhushan
-- * Date: 		25/05/2008
-- * Purpose:		Agent Payments
-- *****************************************************************************
IF NOT EXISTS (SELECT NULL FROM PMProduct_Lookup WHERE lookup_table_name = 'PFPremiumFinance_Cancel_Reason')
BEGIN
	INSERT INTO PMProduct_Lookup
             	(pmproduct_id, lookup_table_name, edit_privilege_level, is_generic_maintenance,linked_data_mandatory)
	VALUES  (2,'PFPremiumFinance_Cancel_Reason', 3, 1, 0)

END
GO

-- *****************************************************************************
-- * Author: 		Deepak Arora
-- * Date: 		26/06/2008
-- * Purpose:		Agent Payments
-- *****************************************************************************
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partybankdetails' AND Column_name='Account_Number')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family,loop1)
    VALUES ('AccountNumber', 'spu_wp_partybankdetails', 'Account_Number', 0, 'Party', 'BankDetails', 'Account Number', 1, 9,'partybankdetails')
END
GO


IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partybankdetails' AND Column_name='Account_holder_name')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family,loop1)
    VALUES ('AccountHolderName', 'spu_wp_partybankdetails', 'Account_holder_name', 0, 'Party', 'BankDetails', 'Account Holder Name', 1, 9,'partybankdetails')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partybankdetails' AND Column_name='Bank_Branch')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family,loop1)
    VALUES ('BankBranch', 'spu_wp_partybankdetails', 'Bank_Branch', 0, 'Party', 'BankDetails', 'Bank Branch', 1, 9,'partybankdetails')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partybankdetails' AND Column_name='Bank_Branch_code')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family,loop1)
    VALUES ('BankBranchCode', 'spu_wp_partybankdetails', 'Bank_Branch_code', 0, 'Party', 'BankDetails', 'Bank Branch Code', 1, 9,'partybankdetails')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partybankdetails' AND Column_name='PBAddress1')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family,loop1)
    VALUES ('PBAddress1', 'spu_wp_partybankdetails', 'PBAddress1', 0, 'Party', 'BankDetails', 'Address1', 1, 9,'partybankdetails')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partybankdetails' AND Column_name='PBAddress2')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family,loop1)
    VALUES ('PBAddress2', 'spu_wp_partybankdetails', 'PBAddress2', 0, 'Party', 'BankDetails', 'Address2', 1, 9,'partybankdetails')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partybankdetails' AND Column_name='PBAddress3')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family,loop1)
    VALUES ('PBAddress3', 'spu_wp_partybankdetails', 'PBAddress3', 0, 'Party', 'BankDetails', 'Address3', 1, 9,'partybankdetails')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partybankdetails' AND Column_name='PBTown')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family,loop1)
    VALUES ('PBTown', 'spu_wp_partybankdetails', 'PBTown', 0, 'Party', 'BankDetails', 'Town', 1, 9,'partybankdetails')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partybankdetails' AND Column_name='Pincode')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family,loop1)
    VALUES ('Pincode', 'spu_wp_partybankdetails', 'Pincode', 0, 'Party', 'BankDetails', 'Pincode', 1, 9,'partybankdetails')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partybankdetails' AND Column_name='Country')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family,loop1)
    VALUES ('Country', 'spu_wp_partybankdetails', 'Country', 0, 'Party', 'BankDetails', 'Country', 1, 9,'partybankdetails')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partybankdetails' AND Column_name='Name_on_card')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family,loop1)
    VALUES ('NameOnCard', 'spu_wp_partybankdetails', 'Name_on_card', 0, 'Party', 'BankDetails', 'Name ON Card', 1, 9,'partybankdetails')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partybankdetails' AND field_name='CCStartDate')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family,loop1)
    VALUES ('CCStartDate', 'spu_wp_partybankdetails', 'CC_StartDate', 0, 'Party', 'BankDetails', 'CC StartDate', 1, 9,'partybankdetails')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partybankdetails' AND field_name='CCExpiryDate')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family,loop1)
    VALUES ('CCExpiryDate', 'spu_wp_partybankdetails', 'CC_ExpiryDate', 0, 'Party', 'BankDetails', 'CC ExpiryDate', 1, 9,'partybankdetails')
END
GO
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partybankdetails' AND Column_name='manual_auth_number')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family,loop1)
    VALUES ('ManualAuthNumber', 'spu_wp_partybankdetails', 'manual_auth_number', 0, 'Party', 'BankDetails', 'Manual Auth Number', 1, 9,'partybankdetails')
END
GO

-- *****************************************************************************
-- * Author: 		Roopaly Rastogi
-- * Date: 		26/06/2008
-- * Purpose:		1.12 PLICO 45
-- *****************************************************************************

UPDATE PRODUCT SET out_of_sequence_mta_dates = 1 WHERE 
out_of_sequence_mta_dates is NULL
GO

UPDATE PRODUCT SET out_of_sequence_mta_allocation = 0 WHERE 
out_of_sequence_mta_allocation is NULL
GO

UPDATE user_authorities SET out_of_sequence_mta_authority = 3
WHERE out_of_sequence_mta_authority is NULL
GO

-- *****************************************************************************
-- * Author:  Gurucharan Gulati
-- * Date:    27/06/2008
-- * Purpose: Added Report Batch_Renewal_Summary_Audit_Report.rpt       
-- *****************************************************************************
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U') 
BEGIN
    DECLARE @report_id INT
    DECLARE @lCaptionID INT
    DECLARE @group_id INT

    -- Batch Renewal Job Summary Report
    SELECT @report_id = report_id FROM report WHERE code = 'Batch_Sum'

    IF(ISNULL(@report_id,0) = 0)
    BEGIN    
        -- Add report
        SELECT @report_id = MAX(report_id)+1 FROM report
        EXECUTE spu_pm_caption_id_return 1, 'Batch Renewal Summary Audit Report', @lCaptionID OUTPUT
        INSERT INTO report(report_id, caption_id, code, description, is_deleted, effective_date, report_name) 
		VALUES(@report_id, @lCaptionID, 'Batch_Sum', 'Batch Renewal Summary Audit Report', 0, GETDATE(), 'Batch_Renewal_Summary_Audit_Report.rpt')

        -- Link report to group
        SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'ALL'
        IF(ISNULL(@group_id,0)<>0)
		INSERT INTO Report_Group_Contents (report_group_id,report_id) VALUES(@group_id,@report_id)
    END
END
GO
-- *****************************************************************************
-- * Author:  Gurucharan Gulati
-- * Date:    27/06/2008
-- * Purpose: Added Report Batch_Renewal_Detailed_Audit_Report.rpt       
-- *****************************************************************************
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U') 
BEGIN
    DECLARE @report_id INT
    DECLARE @lCaptionID INT
    DECLARE @group_id INT

    -- Batch Renewal Job Summary Report
    SELECT @report_id = report_id FROM report WHERE code = 'Batch_Det'

    IF(ISNULL(@report_id,0) = 0)
    BEGIN    
        -- Add report
        SELECT @report_id = MAX(report_id)+1 FROM report
        EXECUTE spu_pm_caption_id_return 1, 'Batch Renewal Detailed Audit Report', @lCaptionID OUTPUT
        INSERT INTO report(report_id, caption_id, code, description, is_deleted, effective_date, report_name) 
		VALUES(@report_id, @lCaptionID, 'Batch_Det', 'Batch Renewal Detailed Audit Report', 0, GETDATE(), 'Batch_Renewal_Detailed_Audit_Report.rpt')

        -- Link report to group
        SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'ALL'
        IF(ISNULL(@group_id,0)<>0)
		INSERT INTO Report_Group_Contents (report_group_id,report_id) VALUES(@group_id,@report_id)
    END
END
GO

-- *****************************************************************************
-- * Author:  Krishan Kr Gaurav
-- * Date:    27/06/2008
-- * Purpose: Added Report Installment_Plan_Statement.rpt       
-- *****************************************************************************
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U') 
BEGIN
    DECLARE @report_id INT
    DECLARE @lCaptionID INT
    DECLARE @group_id INT

    -- Batch Renewal Job Summary Report
    SELECT @report_id = report_id FROM report WHERE code = 'Inst_Stat'

    IF(ISNULL(@report_id,0) = 0)
    BEGIN    
        -- Add report
        SELECT @report_id = MAX(report_id)+1 FROM report
        EXECUTE spu_pm_caption_id_return 1, 'Installment Plan Statement', @lCaptionID OUTPUT
        INSERT INTO report(report_id, caption_id, code, description, is_deleted, effective_date, report_name) 
		VALUES(@report_id, @lCaptionID, 'Inst_Stat', 'Installment Plan Statement', 0, GETDATE(), 'Installment_Plan_Statement.rpt')

        -- Link report to group
        SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'ALL'
        IF(ISNULL(@group_id,0)<>0)
		INSERT INTO Report_Group_Contents (report_group_id,report_id) VALUES(@group_id,@report_id)
    END
END
GO

-- *****************************************************************************
-- * Author:  Deepak Arora
-- * Date:    27/06/2008
-- * Purpose: Script to copt Existign finance plan data to party Bank table
-- *****************************************************************************

IF NOT EXISTS (Select 1 FROM Party_Bank WHERE account_holder_name IN (Select MAX(BankAccountName) From PFPremiumFinance ))
BEGIN
DECLARE @Bank_payment_Type_id INT 
    Select @Bank_payment_Type_id = bank_payment_type_id FROM Bank_payment_Type Where CODE='INS'

INSERT INTO Party_Bank
( 
is_bank,
account_id,
bank_payment_type_id,
account_holder_name, account_number,bank_branch_code,bank_add1,bank_add2,
bank_add3,bank_town,bank_pcode,bank_region,bank_country,
cc_add1,cc_add2,cc_add3,cc_pcode,
is_deleted,
name_on_card
)

SELECT DISTINCT 1,
(SELECT account_id from account A where A.account_key = p.clientid),
@Bank_payment_Type_id,
ISNULL(BankAccountName,''),
BankAccountNo,
BankSortCode,    
BankAddr1,BankAddr2,BankAddr3,BankTown,BankPcode,BankRegion,Country_id,
cardholder_address1,
cardholder_address2,
cardholder_address3,
cardholder_postcode,0,
cardholder_name 
FROM pfPremiumFinance p  
LEFT JOIN Country ON Country.Description= BankCountry 
where BankAccountName <> '' and BankAccountNo <> '' and BankSortCode <> '' 

Update Party_bank SET is_bank = 0 WHERE Account_number is NULL or RTRIM(Account_number)='' and name_on_card<>'' 
END
GO

--**********************************************************************************************  
-- Author : Gautam Poddar
-- History: 08/07/2008    
-- Task : PN 46065
--**********************************************************************************************  
Update Currency Set round_to_places =2 where round_to_places = 0
GO
--**********************************************************************************************  
-- Author : Deepak Arora
-- History: 08/23/2008    
-- Task : PN 47395
--**********************************************************************************************  

DECLARE @lCaptionID bigint
DECLARE @cashlistitem_payment_status_id INT
IF EXISTS(SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
    IF NOT EXISTS(Select 1 FROM cashlistitem_payment_status WHERE CODE ='UNISS')
    BEGIN
        Select  @Cashlistitem_Payment_Status_id =  ISNULL(MAX(cashlistitem_payment_status_id),0) + 1 
        FROM Cashlistitem_payment_status
     
        EXECUTE spu_pm_caption_id_return 1, 'UnIssued', @lCaptionID OUTPUT
        INSERT INTO cashlistitem_payment_status(caption_id, is_deleted,effective_date,Description,code,cashlistitem_payment_status_id)
        Values (@lCaptionID,0,getdate(),'UnIssued','UNISS',@Cashlistitem_Payment_Status_id) 
    END
END
GO

--**********************************************************************************************  
-- Author : Deepak Arora
-- History: 08/23/2008    
-- Task : PN 47444
--**********************************************************************************************  

DECLARE @Party_Type_id INT
DECLARE @Client_Num_Sch_type_id INT 
DECLARE @Party_Num_Sch_type_id INT 
DECLARE @Numbering_Scheme  int 


SELECT @Party_Num_Sch_type_id = Numbering_scheme_Type_id FROM Numbering_scheme_type WHERE Code='PARTY'

SELECT @party_type_id = party_type_id FROM Party_Type WHERE Code= 'PC'
SELECT @Numbering_Scheme = Numbering_Scheme FROM Numbering_scheme WHERE CODE ='PCCODE'

IF EXISTS(SELECT 1 FROM numbering_scheme WHERE CODE ='PCCODE'  AND Party_type_id is NULL) 
BEGIN
IF NOT EXISTS ( SELECT 1 FROM numbering_scheme 
                    WHERE party_type_id= @party_type_id AND is_deleted = 0)
    BEGIN
        IF NOT EXISTS ( Select 1 FROM Numbering_scheme WHERE NUMBERING_SCHEME_TYPE_ID = @Party_Num_Sch_type_id 
                        AND Numbering_scheme = @Numbering_scheme )
        Update numbering_scheme SET party_type_id= @Party_type_id , 
        Numbering_scheme_type_id = @Party_Num_Sch_type_id WHERE CODE ='PCCODE' 
                    
    END 
END
 
SELECT @party_type_id = party_type_id FROM Party_Type WHERE Code= 'CC'
SELECT @Numbering_Scheme = Numbering_Scheme FROM Numbering_scheme WHERE CODE ='CCCODE'

IF EXISTS(SELECT 1 FROM numbering_scheme WHERE CODE ='CCCODE'  AND Party_type_id is NULL) 
BEGIN
    IF NOT EXISTS ( SELECT 1 FROM numbering_scheme 
                    WHERE party_type_id= @party_type_id AND is_deleted = 0)
    BEGIN 
        IF NOT EXISTS ( Select 1 FROM Numbering_scheme WHERE NUMBERING_SCHEME_TYPE_ID = @Party_Num_Sch_type_id 
                AND Numbering_scheme = @Numbering_scheme )
        Update numbering_scheme SET party_type_id= @Party_type_id , 
        Numbering_scheme_type_id = @Party_Num_Sch_type_id WHERE CODE ='CCCODE' 
    END 
END

SELECT @party_type_id = party_type_id FROM Party_Type WHERE Code= 'GC'
SELECT @Numbering_Scheme = Numbering_Scheme FROM Numbering_scheme WHERE CODE ='GCCODE'

IF EXISTS(SELECT 1 FROM numbering_scheme WHERE CODE ='GCCODE'  AND Party_type_id is NULL) 
BEGIN
    IF NOT EXISTS ( SELECT 1 FROM numbering_scheme 
                    WHERE party_type_id= @party_type_id AND is_deleted = 0)
    BEGIN
        IF NOT EXISTS ( Select 1 FROM Numbering_scheme WHERE NUMBERING_SCHEME_TYPE_ID = @Party_Num_Sch_type_id 
                AND Numbering_scheme = @Numbering_scheme )
        Update numbering_scheme SET party_type_id= @Party_type_id , 
        Numbering_scheme_type_id = @Party_Num_Sch_type_id WHERE CODE ='GCCODE' 
    END 
END
GO

-- *****************************************************************************
-- * Author:  Pankaj Kaushik
-- * Date:    18/08/2008       
-- * Purpose: WR9 Batch Renewals
-- *****************************************************************************
declare @pmwrk_task_id int
declare @caption_id int
if not exists(select null from pmwrk_task where code = 'SAMGHRKey')
Begin
    EXECUTE spu_pm_caption_id_return 1, 'SamBatchRenewal', @caption_id OUTPUT
    insert into pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values(@caption_id, 'SAMGHRKey', 'SamBatchRenewal',0,getdate(),0,1,null,null,null,null,1,0,null,null,null,0,2,null)
End
go

--**********************************************************************************************  
-- Author: Richard Clarke
-- History: 18/08/2008    
-- Task: PN 48278
--********************************************************************************************** 

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_GetCommissionBreakdown' AND Column_name='agent_commission_percentage')
BEGIN
    INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
    VALUES ('AgentCommissionPercentage', 'spu_wp_GetCommissionBreakdown', 'agent_commission_percentage', 16, 'Policy', 'Agent', 'Commission Percentage', 1,  'GetCommissionBreakdown', 9)
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_GetCommissionBreakdown' AND Column_name='commission_type')
BEGIN
    INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
    VALUES ('CommissionType', 'spu_wp_GetCommissionBreakdown', 'commission_type', 0, 'Policy', 'Agent', 'Commission Type', 1, 'GetCommissionBreakdown', 9) 
END
GO
--**********************************************************************************************  
-- Author: Deepak Arora
-- History: 20/08/2008    
-- Task: PN 48844
--********************************************************************************************** 

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partyall' AND Column_name='Title')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family)
    VALUES ('Title', 'spu_wp_partyall', 'Title', 0, 'Party', 'Agent', 'Contact Title', 1, 9)
END
GO

Delete from wp_fields WHERE SQL='spu_wp_partyall' AND Column_name='Contact_Person'
GO 
Delete from wp_fields WHERE SQL='spu_wp_partyall' AND Column_name='First_Name'
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partyall' AND Column_name='ContactPerson')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family)
    VALUES ('ContactPerson', 'spu_wp_partyall', 'ContactPerson', 0, 'Party', 'Agent', 'Contact Person', 1, 9)
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partyall' AND Column_name='FirstName')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family)
    VALUES ('FirstName', 'spu_wp_partyall', 'FirstName', 0, 'Party', 'Agent', 'Contact FirstName', 1, 9)
END
GO


-- *****************************************************************************
-- * Author:   Gaurav Arora
-- * Date:     10-11-2008
-- * Purpose:  QBENZ CR005 Client Portfolio Transfer(6.1.1)
-- *****************************************************************************
--Start(Saurabh Agrawal) Tech Spec QBE CR005 Client Portfolio Transfer
IF NOT EXISTS(SELECT TOP 1 * FROM event_type WHERE description='Policy Transfer')
BEGIN
	declare @caption_id int
	declare @Event_type_id int
	EXECUTE spu_pm_caption_id_return 1, 'Policy Transfer', @caption_id OUTPUT
	select @Event_type_id = Max(event_type_id) from event_type
	insert into event_type
	(event_type_id,caption_id, code, description, is_deleted, effective_date,event_type_group_id)
	values(@Event_type_id + 1,@caption_id, 'POLTRANS', 'Policy Transfer',0,getdate(),2)
END
Go



-- Create new if not existing, do not add navxm_process, should be picked up automatically
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
    DECLARE
        @id int,
        @captionid int

    Select @id = pmnavxm_process_id
        From PMNavXM_Process
        Where file_name = 'PORTTRANS.XML'


    IF (@id IS NULL)
    BEGIN
        select @id = max(pmnavxm_process_id) + 1 from pmnavxm_process

        Insert Into PMNavXM_Process(
            pmnavxm_process_id, parent_id, file_name, file_version_number, file_timestamp, is_custom, is_core, xml_definition)
        Values
           (@id, null, 'PORTTRANS.XML', 1, getdate(), 0, 1, '')
    END
    ELSE
    BEGIN
        Update PMNavXM_Process
            Set file_name = 'PORTTRANS.XML' where pmnavxm_Process_id = @id
    END

    -- always update caption
    Execute spu_pm_caption_id_return 1, 'Client Portfolio Transfer', @captionid output

    If Exists (Select * From PMWrk_Task Where Code = 'PROTTRANS')
	       UPDATE PMWrk_Task SET code = 'PORTTRANS'
	              WHERE code = 'PROTTRANS'

    If Not Exists (Select * From PMWrk_Task Where Code = 'PORTTRANS')
    Begin
        Insert Into PMWrk_Task(
             caption_id, code, description, is_deleted,
            effective_date, is_system_task, type_of_task, pmnav_process_id,
            component_object_name, component_class_name, auto_delete_after_num_days, display_icon,
            is_view_only_task, linked_object_name, linked_class_name, linked_caption_id,
            is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
      Values
            ( @captionid, 'PORTTRANS', 'Client Portfolio Transfer', 0,
            '2002.10.14', 0, 2, NULL,
            '', '', 0, 23,
            0, NULL, NULL, NULL,
            1, 2, @id)
         -- Create link to group for this new task
	DECLARE @TaskGroupId INT
	DECLARE @TaskId      INT

    SELECT @TaskId = ( SELECT pmwrk_task_id FROM pmwrk_task WHERE code = 'PORTTRANS')

    -- Add task to correct group
    SELECT @TaskGroupId = ( SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'SYSADMIN')

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task_Group_Task WHERE pmwrk_task_group_id = @TaskGroupId AND pmwrk_task_id  = @TaskId)
    BEGIN
       INSERT INTO pmwrk_task_group_task
       (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num)
       VALUES
       (@TaskGroupId, @TaskId, 0)
    END

    End
    Else
    Begin
        Update PMWrk_Task
            Set caption_id = @captionid,
                description = 'Client Portfolio Transfer'
            Where code = 'PORTTRANS'
    End
END
GO
--End(Saurabh Agrawal) Tech Spec QBE CR005 Client Portfolio Transfer


--**********************************************************************************************
-- Author : Gaurav Arora
--
-- History: 21/05/2008
--
-- Task : UIIC Phase 1 Implementation - Adding Tasks for SAM
--**********************************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCHKUPPR', 'SAMCheckUnpaidPremium'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGTCPSUM', 'GetClaimPerilSummary'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGRCOINS', 'SAMGetRecoveryCoinsurance'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGHRKey', 'GetHeaderAndRiskFeesByKey'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGHPKey', 'GetHeaderAndPolicyFeesByKey'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGHRKEY', 'SAMGetHeaderAndRisks'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGRREINS', 'SAMGetRecoveryReinsurance'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGETCOIN', 'SAMGetClaimCoinsurer'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGHRKey', 'SAMAgentCommission'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMURatSec', 'SAMURatingSection'
GO

--**********************************************************************************************
-- Author : Rahul Jaiswal
--
-- History: 08/07/2008
--
-- Task : UIIC Phase 2 Implementation - Adding wp fields
--**********************************************************************************************


IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='RPCCollectionDate')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,product_family)
    VALUES('RPCCollectionDate','spu_wp_debitcash','collection_date',4,'Receipts/Payments','Cash','Collection Date',1,9)
END

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='RPCComments')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,product_family)
    Values('RPCComments','spu_wp_debitcash','comments',0,'Receipts/Payments','Cash','Comments',1,9)
END


--**********************************************************************************************
-- Author : Gaurav Arora
--
-- History: 09/07/2008
--
-- Task : UIIC Phase 2 Implementation
--**********************************************************************************************

EXECUTE spu_SAM_PMWrk_Task_Group_add 1, 'SAMADMIN', ' SAM Administrator Tasks'
GO


EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMACCTDET', 'SAM Account Detail'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAUG', 'AddUserGroup'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMAUG'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMATG', 'AddTaskGroup'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMATG'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGCVs', 'GetCoinsuranceValues'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGCD', 'GetCoinsuranceDefaults'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUCIV', 'UpdateCoinsuranceValues'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAUTGS', 'UpdateTaskGroups'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMAUTGS'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAUG', 'DeleteUndeleteUserGroup'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMAUG'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_add 1, 'SAMADMIN', ' All SAM Tasks '
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAUG', 'UpdateUserGroup'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMAUG'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGUG', 'GetTaskGroups'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGUG'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMMAWMKey', 'ReAssignMultipleWmTasks'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGWMTKey', 'SAMGetWmTask'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGWTLKey', 'SAMGetWmTaskLog'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAWTLKey', 'SAMAddWmTaskLog'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCWMKey', 'SAMCreateWorkManager'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGUG', 'GetTaskGroupTasks'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGUG'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMEVTLOG', 'SAM Event Log'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMEVTLOG', 'SAM Event Log'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMEVTLOG', 'SAM Event Log'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAQuot', 'GetSubAgents'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAQuot', 'UpdateSubAgents'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAQuot', 'GetStandardPolicyWordings'
GO


EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAQuot', 'UpdateStandardPolicyWordings'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAQuot', 'FindDocumentTemplates'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMEVTLOG', 'GetEventNote'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAUTUG', 'UpdateUserGroupUsers'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMAUTUG'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAUTUG', 'UpdateTaskGroupTasks'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMAUTUG'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGRIA', 'GetRiskReinsuranceArrangements'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMDWMKey', 'SAMDeleteWmTask'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUWMKey', 'SAMUpdateWmTask'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMANWMKey', 'SAMAssignWmTask'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCMWMKey', 'SAMCompleteWmTask'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMICWMKey', 'SAMInCompleteWmTask'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMRWMKey', 'SAMRunWmTask'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_add 1, 'SAMADMIN', 'SAMUserAccess'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGUG', 'GetUserGroupUsers'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGUG'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAWMTKey', 'GetWorkManagerScheduledTasks'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_add 1, 'SAMADMIN', 'SAMUserAccess'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGUG', 'GetUserGroups'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGUG'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCWMKey', 'SAMAddWmTask'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGUGTG', 'GetUserGroupTaskGroups'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMGUGTG'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SLACS', 'SHOWCLITEM'
GO


-- *****************************************************************************
-- * Issue: Lookup Entry for BG Status for Bank Guarantee Development
-- * Author: Gaurav Arora
-- * Date:
-- *****************************************************************************
IF NOT EXISTS (SELECT * FROM pmproduct_lookup WHERE lookup_table_name = 'BG_Status')
BEGIN
    INSERT INTO pmproduct_lookup
    (pmproduct_id,lookup_table_name,edit_privilege_level,is_generic_maintenance)

    VALUES
    (2,'BG_Status',3,0)
END
GO

-- *****************************************************************************
-- * Author:      Gaurav Arora
-- * Date:
-- * Purpose:     WR06 - Bank Guarantee
-- *****************************************************************************

IF NOT EXISTS (Select * from BG_Status)
BEGIN
    Declare @Caption_id int
    EXEC spu_pm_caption_id_return 1,'Active',@Caption_id Output
    --Start Installation error fix
    INSERT INTO BG_Status ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'Active','Active',0,Getdate())

    EXEC spu_pm_caption_id_return 1,'Issued',@Caption_id Output
    INSERT INTO BG_Status ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'Issued','Issued',0,Getdate())

    EXEC spu_pm_caption_id_return 1,'Invoked',@Caption_id Output
    INSERT INTO BG_Status ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'Invoked','Invoked',0,Getdate())

    EXEC spu_pm_caption_id_return 1,'Deleted',@Caption_id Output
    INSERT INTO BG_Status ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'Deleted','Deleted',0,Getdate())

    EXEC spu_pm_caption_id_return 1,'Expired',@Caption_id Output
    INSERT INTO BG_Status ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'Expired','Expired',0,Getdate())
	--End Installation error fix

END
GO
-- *****************************************************************************
-- * Add a new task for Bank Guarantee
-- * Author: Gaurav Arora
-- * Date:
-- *****************************************************************************
DECLARE @lCaptionID  INT
DECLARE @TaskGroupId INT
DECLARE @TaskId      INT
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
    IF NOT EXISTS (SELECT * FROM PMWrk_Task WHERE code = 'BGUAMGMT')
    BEGIN
	   EXECUTE spu_pm_caption_id_return 1, 'Bank Guarantee Maintenance', @lCaptionID OUTPUT

	   INSERT INTO PMWrk_Task
		(caption_id, code, description, is_deleted, effective_date,
		is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name,
		auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name,
		linked_caption_id, is_available_task, pmwrk_task_category_id)

	   SELECT @lCaptionID, 'BGUAMGMT', 'Bank Guarantee Maintenance', 0,
		  GETDATE(), 0, 1, NULL, 'iSIRFindBankGuarantee', 'NavigatorV3', 0, 42, 0, NULL, NULL, NULL, 1, 2
	   
    END

    -- Create link to group for this new task
    SELECT @TaskId = ( SELECT  pmwrk_task_id FROM pmwrk_task WHERE code = 'BGUAMGMT')

    -- Add task to correct group
    SELECT @TaskGroupId = ( SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'SYSADMIN')

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task_Group_Task WHERE pmwrk_task_group_id = @TaskGroupId AND pmwrk_task_id  = @TaskId)
    BEGIN
       INSERT INTO pmwrk_task_group_task
       (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num)
       VALUES
       (@TaskGroupId, @TaskId, 0)
    END
END
GO


-- *****************************************************************************
-- * Author:        Rahul Jaiswal
-- * Date:          12-08-2008
-- * Purpose:       New Tasks and Groups created for Batch 3
-- *****************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMPBGGET', 'GetPolicyBankGuarantee'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCRCLI', 'CreateReceiptCashListItem'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMBGADD', 'AddBankGuarantee'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGBGET', 'GetBankGuarantee'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMFIBG', 'FindBankGuarantee'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMFIB', 'FindBank'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMBGUPD', 'UpdateBankGuarantee'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMRBGGET', 'GetPoliciesOnBankGuaranteeForReceipt'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMBGPol', 'GetPoliciesOnBankGuaranteeByKey'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMBGConl', 'UpdateBankGuaranteeConditionally'
GO


EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCHKUPPR', 'SAMCheckUnpaidPremium'
GO

--Start (Prakash C Varghese) - (Tech Spec - UIIC WR25 - MaintainClaim - RiskRelatedDetails - Financials.doc) - (6.1.1)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGTCPSUM', 'GetClaimPerilSummary'
GO
--End (Prakash C Varghese) - (Tech Spec - UIIC WR25 - MaintainClaim - RiskRelatedDetails - Financials.doc) - (6.1.1)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGRCOINS', 'SAMGetRecoveryCoinsurance'
GO

-- Start (PraveenGora) - (Tech Spec - UIIC WR50 - List Risks - Risk Fees.doc) - (7.1.4.6)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGHRKey', 'GetHeaderAndRiskFeesByKey'
GO

-- End (PraveenGora) - (Tech Spec - UIIC WR50 - List Risks - Risk Fees.doc) - (7.1.4.6)

-- Start (PraveenGora) - (Tech Spec - UIIC WR50 - List Risks - Policy Fees.doc) - (7.1.4.6)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGHPKey', 'GetHeaderAndPolicyFeesByKey'
GO

-- End (PraveenGora) - (Tech Spec - UIIC WR50 - List Risks - Policy Fees.doc) - (7.1.4.6)

-- Start (PraveenGora) - (Tech Spec - WR42A_SAM_Party_Enquiry-GetAllocationDetails.doc) - (7.1.3.8)
 
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMACCTDET', 'SAM Account Detail' 
GO 
 
-- End (PraveenGora) - (Tech Spec - WR42A_SAM_Party_Enquiry-GetAllocationDetails.doc) - (7.1.3.8)

-- Start (PraveenGora) - (Tech Spec - UIIC WR33 - Work Manager -  ReAssign Multiple Tasks.doc) - (7.1.4.7)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMMAWMKey', 'ReAssignMultipleWmTasks' 
GO 

-- End (PraveenGora) - (Tech Spec - UIIC WR33 - Work Manager -  ReAssign Multiple Tasks.doc) - (7.1.4.7)

--Start (Sankar)-(Tech Spec - UIICWR50 - MTC - List Risks - Risks.doc)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGHRKEY', 'SAMGetHeaderAndRisks'
GO

--End (Sankar)-(Tech Spec - UIICWR50 - MTC - List Risks - Risks.doc)

--Start (Sankar) - (Tech Spec - UIIC WR24 - OpenClaim - Reserve Process - Reinsurance - Recoveries.doc) - (6.1.1)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGRREINS', 'SAMGetRecoveryReinsurance'
GO

--End (Sankar) - (Tech Spec - UIIC WR24 - OpenClaim - Reserve Process - Reinsurance - Recoveries.doc) - (6.1.1)


--Start (Prakash C Varghese) - (Tech Spec - UIIC WR24 - OpenClaim - Reserve Process - Coinsurer Breakdown_new.doc) - (6.1.1)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGETCOIN', 'SAMGetClaimCoinsurer'
GO

--End (Prakash C Varghese) - (Tech Spec - UIIC WR24 - OpenClaim - Reserve Process - Coinsurer Breakdown_new.doc) - (6.1.1)


--Start (Vijayakumar Ramasamy) - (Tech Spec - UIICWR50 - MTC - List Risks - Agent Commission.doc) - (7.1.3.8)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGHRKey', 'SAMAgentCommission'
GO
--End (Vijayakumar Ramasamy) - (Tech Spec - UIICWR50 - MTC - List Risks - Agent Commission.doc) - (7.1.3.8)


--Start (Vijayakumar Ramasamy) - (Tech Spec - UIICWR50 - MTC - Risk Premium Details - Update Rating Sections.doc) - (7.1.4.6)


EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMURatSec', 'SAMURatingSection'
GO

--End (Vijayakumar Ramasamy) - (Tech Spec - UIICWR50 - MTC - Risk Premium Details - Update Rating Sections.doc) - (7.1.4.6)


--Start (Arul Stephen) - (Tech Spec - UIIC WR26 View Claims  Claim Versions.doc) - (6.1.1)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGETCLVR', 'SAMGetClaimsVersions'
GO

--End (Arul Stephen) - (Tech Spec - UIIC WR26 View Claims  Claim Versions.doc) - (6.1.1)



--Start (Arul Stephen) - (Tech Spec - UIICWR27 - MTA - Risk Premium - Get Risk Taxes.doc) - (6.0.0)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGHRKey', 'GetHeaderAndRiskTaxByKey'
GO

--End (Arul Stephen) - (Tech Spec - UIICWR27 - MTA - Risk Premium - Get Risk Taxes.doc) - (6.0.0)



--Start (Arul Stephen) - (Tech Spec - UIICWR50 - MTC - List Risks - Policy Tax.doc) - (6.0.0)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGHRKey', 'GetHeaderAndPolicyTaxByKey'
GO

--End (Arul Stephen) - (Tech Spec - UIICWR50 - MTC - List Risks - Policy Tax.doc) - (6.0.0)

--Start (Prakash C Varghese) - (Tech Spec - UIIC WR01 - User Access - Add User Group.doc) - (7.1.5.6)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAUG', 'AddUserGroup'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMAUG'
GO
--End (Prakash C Varghese) - (Tech Spec - UIIC WR01 - User Access - Add User Group.doc) - (7.1.5.6)

--Start (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR01 - User Access - Add Task Group.doc)-(7.1.5.6)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMATG', 'AddTaskGroup'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMATG'
GO
--End (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR01 - User Access - Add Task Group.doc)-(7.1.5.6)


--Start (Vivek Athalye) - (Tech Spec - UIIC WR22 - Capture Quote Details - Co-Insurance.doc) - (7.1.5.6) 
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGCVs', 'GetCoinsuranceValues'
GO

--End (Vivek Athalye) - (Tech Spec - UIIC WR22 - Capture Quote Details - Co-Insurance.doc) - (7.1.5.6) 

--Start (Vivek Athalye) - (Tech Spec - UIIC WR22 - Capture Quote Details - Co-Insurance.doc) - (7.2.5.4) 
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGCD', 'GetCoinsuranceDefaults'
GO

--End (Vivek Athalye) - (Tech Spec - UIIC WR22 - Capture Quote Details - Co-Insurance.doc) - (7.2.5.4) 

--Start (Vivek Athalye) - (Tech Spec - UIIC WR22 - Capture Quote Details - Co-Insurance.doc) - (7.3.5.6) 
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUCIV', 'UpdateCoinsuranceValues'
GO

--End (Vivek Athalye) - (Tech Spec - UIIC WR22 - Capture Quote Details - Co-Insurance.doc) - (7.3.5.6) 
--Start (PraveenGora) - (Tech Spec - UIIC WR01 - User Access - Update Task Groups.doc) - (7.1.5.6) 
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAUTGS', 'UpdateTaskGroups'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMAUTGS'
GO
--End (PraveenGora) - (Tech Spec - UIIC WR01 - User Access - Update Task Groups.doc) - (7.1.5.6) 
--Start (PraveenGora) - (Tech Spec - UIIC WR01 - User Access - Delete Undelete User Group.doc) - (7.1.5.6) 
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAUG', 'DeleteUndeleteUserGroup'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMAUG'
GO
--End (PraveenGora) - (Tech Spec - UIIC WR01 - User Access - Delete Undelete User Group.doc) - (7.1.5.6) 

--Start (Sriram P) - (Tech Spec - UIIC WR01 - User Access - Update User Group.doc) - (7.1.5.6)
EXECUTE spu_SAM_PMWrk_Task_Group_add 1, 'SAMADMIN', ' All SAM Tasks ' 
GO 
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAUG', 'UpdateUserGroup'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMAUG'
GO
--End (Sriram P) - (Tech Spec - UIIC WR01 - User Access - Update User Group.doc) - (7.1.5.6)

--Start (Sriram P) - (Tech Spec - UIIC WR01 - User Access - Get Task Groups.doc) - (7.1.5.6)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGUG', 'GetTaskGroups'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGUG'
GO
--End (Sriram P) - (Tech Spec - UIIC WR01 - User Access - Get Task Groups.doc) - (7.1.5.6)


--Start (Sriram P) - (Tech Spec - UIIC WR33 - Work Manager - View Task.doc) - (6.1)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGWMTKey', 'SAMGetWmTask' 
GO 

--End  (Sriram P) - Tech Spec - UIIC WR33 - Work Manager - View Task.doc) - (6.1)

--Start (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR33 - Work Manager - Task  Log.doc)-(6.1)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGWTLKey', 'SAMGetWmTaskLog' 
GO 

EXECUTE spu_SAM_PMWrk_Task_add 1, SAMAWTLKey, 'SAMAddWmTaskLog' 
GO 
 
--End (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR33 - Work Manager - Task  Log.doc)-(6.1)

--Start (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR33 - Work Manager - Add Task.doc)-(6.1)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCWMKey', 'SAMCreateWorkManager' 
GO 

--End (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR33 - Work Manager - Add Task.doc)-(6.1)
--Start (Sriram P) - (Tech Spec - UIIC WR01 - User Access - Get Task Group Tasks.doc) - (7.1.5.6)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGUG', 'GetTaskGroupTasks'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGUG'
GO

--End (Sriram P) - (Tech Spec - UIIC WR01 - User Access - Get Task Group Tasks.doc) - (7.1.5.6)
--Start (Sriram P) - (Tech Specs - WR42A_SAM_Party_Enquiry - AddEvent.doc) - (7.1.5.6)
 
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMEVTLOG', 'SAM Event Log' 
GO 
 
--End  (Sriram P) (Tech Specs - WR42A_SAM_Party_Enquiry - AddEvent.doc) - (7.1.5.6)

--Start (Sriram P) - (Tech Specs - WR42A_SAM_Party_Enquiry - AddEventNote.doc) - (7.1.5.6)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMEVTLOG', 'SAM Event Log' 
GO 

--End  (Sriram P)  (Tech Tech Specs - WR42A_SAM_Party_Enquiry - AddEventNote.doc) - (7.1.5.6)


--Start (Sriram P) - Tech Specs - WR42A_SAM_Party_Enquiry - GetEventDetails.doc) - (7.1.5.6)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMEVTLOG', 'SAM Event Log' 
GO 

--End  (Sriram P) ( Tech Specs - WR42A_SAM_Party_Enquiry - GetEventDetails.doc) - (7.1.5.6)

--Start (Girija chokkalingam) - (Tech Spec - UIIC WR22  Capture Quote Details  Sub Agents.doc) - (7.1.5.6)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAQuot', 'GetSubAgents'
GO

--End (Girija chokkalingam) - (Tech Spec - UIIC WR22  Capture Quote Details  Sub Agents.doc) - (7.1.5.6)

--Start (Girija chokkalingam) - (Tech Spec - UIIC WR22  Capture Quote Details  Sub Agents.doc) - (7.2.5.6)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAQuot', 'UpdateSubAgents'
GO

--End (Girija chokkalingam) - (Tech Spec - UIIC WR22  Capture Quote Details  Sub Agents.doc) - (7.2.5.6)

--Start (Girija chokkalingam) - (Tech Spec - UIIC WR22  Capture Quote Details  Policy Standard Wordings.doc) - (7.1.5.6)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAQuot', 'GetStandardPolicyWordings'
GO

--End (Girija chokkalingam) - (Tech Spec - UIIC WR22  Capture Quote Details  Policy Standard Wordings.doc) - (7.1.5.6)

--Start (Girija chokkalingam) - (Tech Spec - UIIC WR22  Capture Quote Details  Policy Standard Wordings.doc) - (7.2.5.6)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAQuot', 'UpdateStandardPolicyWordings'
GO

--End (Girija chokkalingam) - (Tech Spec - UIIC WR22  Capture Quote Details  Policy Standard Wordings.doc) - (7.2.5.6)

--Start (Girija chokkalingam) - (Tech Spec - UIIC WR22  Capture Quote Details  Policy Standard Wordings.doc) - (7.3.5.6)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAQuot', 'FindDocumentTemplates'
GO

--End (Girija chokkalingam) - (Tech Spec - UIIC WR22  Capture Quote Details  Policy Standard Wordings.doc) - (7.3.5.6)

--Start (Girija chokkalingam) - (Tech Spec - UIIC WR42A_SAM_Party_Enquiry_GetEventNote.doc) - (7.1.4.6)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMEVTLOG', 'GetEventNote'
GO

--End (Girija chokkalingam) - (Tech Spec - UIIC WR42A_SAM_Party_Enquiry_GetEventNote.doc) - (7.1.4.6)

--Start (Girija chokkalingam) - (Tech Spec - UIIC WR01  User Access  Update User Group Users.doc) - (7.1.5.6)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAUTUG', 'UpdateUserGroupUsers'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMAUTUG'
GO
--End (Girija chokkalingam) - (Tech Spec - UIIC WR01  User Access  Update User Group Users.doc) - (7.1.5.6)

--Start (Girija chokkalingam) - (Tech Spec - UIIC WR01  User Access  Update User Group Users.doc) - (7.1.5.6)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAUTUG', 'UpdateTaskGroupTasks'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMAUTUG'
GO
--End (Girija chokkalingam) - (Tech Spec - UIIC WR01  User Access  Update User Group Users.doc) - (7.1.5.6)

-- Start (Sankar) - (Tech Spec - UIIC WR22 - Capture Quote Details - Reinsurance Details.doc) 
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGRIA', 'GetRiskReinsuranceArrangements'
GO

-- End (Sankar) - (Tech Spec - UIIC WR22 - Capture Quote Details - Reinsurance Details.doc) 

-- Start (Sankar) - (Tech Spec - UIIC WR33 - Work Manager - Delete Task.doc)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMDWMKey', 'SAMDeleteWmTask' 
GO 

-- End (Sankar) - (Tech Spec - UIIC WR33 - Work Manager - Delete Task.doc)

-- Start (Sankar) - (Tech Spec - UIIC WR33 - Work Manager - Update Task.doc) - 6.1

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUWMKey', 'SAMUpdateWmTask'
GO 

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMANWMKey', 'SAMAssignWmTask' 
GO 

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCMWMKey', 'SAMCompleteWmTask' 
GO 

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMICWMKey', 'SAMInCompleteWmTask' 
GO 


EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMRWMKey', 'SAMRunWmTask' 
GO 

-- End (Sankar) - (Tech Spec - UIIC WR33 - Work Manager - Update Task.doc) - 6.1

-- Start (Sankar) - (Tech Spec - UIIC WR01 - User Access - Get User Group Users.doc)
EXECUTE spu_SAM_PMWrk_Task_Group_add 1, 'SAMADMIN', 'SAMUserAccess' 
GO 
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGUG', 'GetUserGroupUsers' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGUG'
GO 
-- End (Sankar) - (Tech Spec - UIIC WR01 - User Access - Get User Group Users.doc)
--Start (Ravikumar P) - (Tech Spec - UIIC WR33 - Work Manager - Get Task.doc) - (6.1)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAWMTKey', 'GetWorkManagerScheduledTasks' 
GO 

--End  (Ravikumar P) - Tech Spec - UIIC WR33 - Work Manager - View Task.doc) - (6.1)

-- Start (Ravikumar P) - (Tech Spec - UIIC WR01 - User Access - Get User Group.doc)
EXECUTE spu_SAM_PMWrk_Task_Group_add 1, 'SAMADMIN', 'SAMUserAccess' 
GO 
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGUG', 'GetUserGroups' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGUG'
GO 
-- End (Ravikumar P) - (Tech Spec - UIIC WR01 - User Access - Get User Group.doc)

--Start (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR33 - Work Manager - Create Task.doc)-(6.1)

EXECUTE spu_SAM_PMWrk_Task_add 1, SAMCWMKey, 'SAMAddWmTask' 
GO 

--End (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR33 - Work Manager - Create Task.doc)-(6.1)

--Start (Girija chokkalingam) - (Tech Spec - UIIC WR6 Bank Guarantee  Find Bank Guarantee.doc) - (7.1.4.6)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMFBGKey', 'FindBankGuarantee'
GO

--End (Girija chokkalingam) - (Tech Spec - UIIC WR6 Bank Guarantee  Find Bank Guarantee.doc) - (7.1.4.6)
 


--Start (Arul Stephen A) - (Tech Spec - UIIC WR6 - Get Bank Guarantee.doc) 
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGBGKey', 'GetBankGuarantee'
GO

---End (Arul Stephen A) - (Tech Spec - UIIC WR6 - Get Bank Guarantee.doc) 
--Start (Ravikumar Pasupuleti)-(Tech Spec - UIICWR6 - Find Bank.doc)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMFBKey', 'FindBank'
GO

--End (Ravikumar Pasupuleti)-(Tech Spec - UIICWR6 - Find Bank.doc)
--Start (Sriram P) - (Tech Spec - UIIC WR28 - User Access - Get User Group Task Groups.doc) - (7.1.5.8)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGUGTG', 'GetUserGroupTaskGroups'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMGUGTG'
GO

--End (Sriram P) - (Tech Spec - UIIC WR28 - User Access - Get User Group Task Groups.doc) - (7.1.5.8)
--Start (Sriram P) - (Tech Spec - UIIC WR53 - Cover Note Maintenance - Add Cover Note Book.doc) - (6.1)

EXECUTE spu_SAM_PMWrk_Task_Group_Add 1, 'SAMAdmin', 'SAM Admin' 
GO 
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMACNBKey', 'SAMAddCoverNoteBook' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMAdmin', 'SAMACNBKey'
GO 
--End (Sriram P) - (Tech Spec - UIIC WR53 - Cover Note Maintenance - Add Cover Note Book.doc) - (6.1)

--Start (PraveenGora) - (Tech Spec - UIIC WR53 - Cover Note Maintenance - Update Cover Note Book.doc) - (6.1)
EXECUTE spu_SAM_PMWrk_Task_Group_Add 1, 'SAMAdmin', 'SAM Admin' 
GO 
EXECUTE spu_SAM_PMWrk_Task_add 1, SAMUCNBKey, 'SAMUpdateCoverNoteBook' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMAdmin', SAMUCNBKey
GO  
--End (PraveenGora) - (Tech Spec - UIIC WR53 - Cover Note Maintenance - Update Cover Note Book.doc) - (6.1)

--Start (PraveenGora) - (Tech Spec - UIIC WR53 - Cover Note Maintenance - Update Cover Note Sheet.doc) - (6.1)
EXECUTE spu_SAM_PMWrk_Task_add 1, SAMUCNSKey, 'SAMUpdateCoverNoteSheet'  
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMAdmin', SAMUCNSKey
GO
--End (PraveenGora) - (Tech Spec - UIIC WR53 - Cover Note Maintenance - Update Cover Note Sheet.doc) - (6.1)

EXECUTE spu_SAM_PMWrk_Task_Group_Add 1, 'SAMAdmin', 'SAM Admin' 
GO 
EXECUTE spu_SAM_PMWrk_Task_add 1, SAMUCNBKey, 'SAMAddCoverNoteBook' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMAdmin', SAMUCNBKey
GO  


--Start (Vijayakumar Ramasamy) - (Tech Spec - UIIC WR53 - Cover Note Maintenance - Get Cover Note Book Details.doc) - (6.1)
EXECUTE spu_SAM_PMWrk_Task_add 1, SAMGCNBKey, 'SAMGetCoverNoteBook' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMAdmin', SAMGCNBKey
GO 
--End (Vijayakumar Ramasamy) - (Tech Spec - UIIC WR53 - Cover Note Maintenance - Get Cover Note Book Details.doc) - (6.1)
--Start (Ravikumar Pasupuleti) - (Tech Spec - UIIC WR53 - Cover Note Maintenance - Add Cover Note Sheet.doc) - (6.1)
EXECUTE spu_SAM_PMWrk_Task_add 1, SAMACNSKey, 'SAMGetCoverNoteBook' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMAdmin', SAMACNSKey
GO 
--End (Ravikumar Pasupuleti) - (Tech Spec - UIIC WR53 - Cover Note Maintenance - Add Cover Note Sheet.doc) - (6.1)
--Start (PraveenGora) - (Tech Spec - UIIC WR63a - Claim Payment Processing - Get Unallocated Claim Payments.doc) - (6.1)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMPClm', 'SAMPayClaim' 
GO 

--End (PraveenGora) - (Tech Spec - UIIC WR63a - Claim Payment Processing - Get Unallocated Claim Payments.doc) - (6.1)
--Start (PraveenGora) - (Tech Spec - UIIC WR63a - Claim Payment Processing - Find Account.doc) - (6.1)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMPClm', 'SAMPayClaim' 
GO 

--End (PraveenGora) - (Tech Spec - UIIC WR63a - Claim Payment Processing - Find Account.doc) - (6.1)
--Start (Girija chokkalingam) - (Tech Spec - WR53  Cover Note Maintenance  Delete Cover Note Sheet .doc) - (6.1)
EXECUTE spu_SAM_PMWrk_Task_add 1, SAMDCNSKey, 'SAMDeleteCoverNoteSheet' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMAdmin', SAMDCNSKey
GO 
--End (Girija chokkalingam) - (Tech Spec - WR53  Cover Note Maintenance  Delete Cover Note Sheet .doc) - (6.1)
--Start (Girija chokkalingam) - (Tech Spec - UIIC WR60  Authorise Payment  Approve Cash List Item .doc) - (not specified)
EXECUTE spu_SAM_PMWrk_Task_add 1, SAMCLIApr, 'ApproveCashListItem' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMAdmin', SAMCLIApr
GO 
--End (Girija chokkalingam) - (Tech Spec - UIIC WR60  Authorise Payment  Approve Cash List Item .doc) - (not specified)


--Start(Sriram P) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Payment Cash List Items.doc) - (7.1.4.6)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCASHPAY', 'SAM Get payment Cashlist'
GO 
 
--End (Sriram P) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Payment Cash List Items.doc) - (7.1.4.6)

--Start(Sriram P) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Receipt Cash List Details.doc) - (7.1.4.6)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCASHREC', 'SAM Get Receipt Cashlist' 
GO 
 
--End (Sriram P) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Receipt Cash List Details.doc) - (7.1.4.6)

--Start(Ravikumar Pasupuleti) - (Tech Spec - UIIC WR62 - Cash Cheque Payment - Get Transaction Details.doc) - (7.1.4.6)


EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGTRAKey', 'SAM Get Transaction Details' 
GO 
 
--End (Ravikumar Pasupuleti) - (Tech Spec - UIIC WR62 - Cash Cheque Payment - Get Transaction Details.doc) - (7.1.4.6)

--Start(Ravikumar Pasupuleti) - (Tech Spec - UIIC WR6 - Get Currency Conversion Rates.doc) - (7.1.4.6)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGCEXKey', 'SAM Get Currency ExchangeRates' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMAdmin', 'SAMGCEXKey'
GO 
--End (Ravikumar Pasupuleti) - (Tech Spec - UIIC WR6 - Get Currency Conversion Rates.doc) - (7.1.4.6)


--Start(Vijayakumar Ramasamy) - (Tech Spec - UIICWR6 - Get Policies on Bank Guarantee For Receipt.doc) - (7.1.5.5)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMFBGKey', 'GetPoliciesOnBankGuaranteeForReceipt' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMAdmin', 'SAMFBGKey'
GO 
--End (Vijayakumar Ramasamy) - (Tech Spec - UIICWR6 - Get Policies on Bank Guarantee For Receipt.doc) - (7.1.5.5)

--Start(Vijayakumar Ramasamy) - (Tech Spec - UIICWR6 - Get Policies on Bank Guarantee By Key.doc) - (7.1.5.5)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMBGPol', 'GetPoliciesOnBankGuaranteeByKey' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMAdmin', 'SAMBGPol'
GO 
--End (Vijayakumar Ramasamy) - (Tech Spec - UIICWR6 - Get Policies on Bank Guarantee By Key.doc) - (7.1.5.5)

--Start (Arul Stephen) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List .doc) -(7.1.4.6)

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCRCL', 'CreateReceiptCashListWithItems' 
GO 

--End (Arul Stephen) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List .doc) -(7.1.4.6)
--Start (Saurabh Agrawal) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Payment Cash List) - (7.1.4.5) 

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCPCL', 'CreatePaymentCashListWithItems' 
GO 

--End (Saurabh Agrawal) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Payment Cash List) - (7.1.4.5) 

--Start (Saurabh Agrawal) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Payment Cash List Item) - (7.1.4.5) 
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCPCLI', 'CreatePaymentCashListItem' 
GO 

--End (Saurabh Agrawal) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Payment Cash List) - (7.1.4.5)

--Start (Saurabh Agrawal) - (Tech Spec - UIIC WR6 -FindPolicy)-(7.1.4.6)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMFindPol', 'SamFindpolicy' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMAdmin', 'SAMFindPol'
--End (Saurabh Agrawal) - (Tech Spec - UIIC WR6 -FindPolicy)-(7.1.4.6)


--Start (UdayaBhaskar Kondapalli) - (Tech Spec - UIIC WR53 -Cover Note Maintenance-Get Cover Not Sheet)-(7.1.4.6) 
EXECUTE spu_SAM_PMWrk_Task_Group_add 1, 'SAMAdmin', 'SAM Admin' 
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGCNSKey', 'SAM Cover Note Sheet' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMAdmin', 'SAMGCNSKey'
GO
--End (UdayaBhaskar Kondapalli) - (Tech Spec - UIIC WR53 -Cover Note Maintenance-Get Cover Not Sheet)-(7.1.4.6)
--Start (UdayaBhaskar Kondapalli) - (Tech Spec - UIIC WR62 -Cash Cheque Receipt-Get Receipt Cash list Itemm details)-(7.1.4.6) 
EXECUTE spu_SAM_PMWrk_Task_Group_add 1, 'SAMAdmin', 'SAM Admin' 
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGRefP', 'SAM Get Receipt Cash list Itemm details' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMAdmin', 'SAMGRefP'
GO
--End (UdayaBhaskar Kondapalli) - (Tech Spec - UIIC WR62 -Cash Cheque Receipt-Get Receipt Cash list Itemm details)-(7.1.4.6)

--Start (UdayaBhaskar Kondapalli) - (Tech Spec - UIICWR6 - Update Bank Guarantee Conditionaly)-(7.1.4.6) 
EXECUTE spu_SAM_PMWrk_Task_Group_add 1, 'SAMAdmin', 'SAM Admin' 
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMFBGKey', 'SAM UpdateBankGuaranteeConditionaly' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMAdmin', 'SAMFBGKey'
GO
--End (UdayaBhaskar Kondapalli) - (Tech Spec - UIICWR6 - Update Bank Guarantee Conditionaly)-(7.1.4.6)  
--Start (PraveenGora) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Payment Cash List Details)-(7.1.4.6)
EXECUTE spu_SAM_PMWrk_Task_Group_add 1, 'SAMAdmin', 'SAM Admin' 
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCASHPAY', 'SAM Get Payment Cash list details' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMAdmin', 'SAMCASHPAY'
GO
--End (PraveenGora) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Payment Cash List Details)-(7.1.4.6) 
--Start (PraveenGora) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Payment Cash List Item Details)-(7.1.4.6) 
EXECUTE spu_SAM_PMWrk_Task_Group_add 1, 'SAMAdmin', 'SAM Admin' 
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCASHPAY', 'SAM Get Payment Cash list Item details' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMAdmin', 'SAMCASHPAY'
GO
--End (PraveenGora) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Payment Cash List Item Details)-(7.1.4.6) 
--Start (PraveenGora) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Receipt Cash List Items)-(7.1.4.6) 
EXECUTE spu_SAM_PMWrk_Task_Group_add 1, 'SAMAdmin', 'SAM Admin' 
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCASHREC', 'SAM Get Receipt Cash list Items' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMAdmin', 'SAMCASHREC'
GO
--End (PraveenGora) - (Tech Spec - UIIC WR62 - Cash Cheque Receipt - Get Receipt Cash List Items)-(7.1.4.6) 

--Start (Sankar) - (Tech Spec - UIIC WR53 - Cover Note Maintenance - Find Cover Note Book)-(6.1)


--End (Sankar) - (Tech Spec - UIIC WR53 - Cover Note Maintenance - Find Cover Note Book)-(6.1)

--Start (Sankar) - (Tech Specs - UIICWR61 Cash Cheque Payment - UpdateAllocation)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCPUA', 'SAMUpdateAllocation' 
GO

--End (Sankar) - (Tech Specs - UIICWR61 Cash Cheque Payment - UpdateAllocation)

--Start (Prakash C Varghese) - (Tech Spec - SAM - UIIC WR73 - Renewals.doc) - (7.5.1)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMPRSBP', 'RunRenewalSelectionByPolicy'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMPRSBP'
GO
--End (Prakash C Varghese) - (Tech Spec - SAM - UIIC WR73 - Renewals.doc) - (7.5.1)

--Start (Prakash C Varghese) - (Tech Spec - SAM - UIIC WR73 - Renewals.doc) - (7.5.2)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMPRDEL', 'DeleteRenewal'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMPRDEL'
GO
--End (Prakash C Varghese) - (Tech Spec - SAM - UIIC WR73 - Renewals.doc) - (7.5.2)

--Start (Prakash C Varghese) - (Tech Spec - SAM - UIIC WR73 - Renewals.doc) - (7.5.3)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMPRURS', 'UpdateRenewalStatus'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMPRURS'
GO
--End (Prakash C Varghese) - (Tech Spec - SAM - UIIC WR73 - Renewals.doc) - (7.5.3)


-- *****************************************************************************
-- * Issue:  Added LookupValue for Bank Guarantee Debt to cashlistitem_receipt_type
-- * Author: Gaurav Arora
-- * Date:   01/09/2008
-- *****************************************************************************
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN

    DECLARE @cashListItem_receipt_type_id INT
    DECLARE @lCaptionID INT

    -- Instalment Deposit
    IF NOT EXISTS(select code FROM CashListItem_Receipt_Type WHERE code = 'BGDEPT')
    BEGIN

        -- Add LookupValue
        SELECT @cashListItem_receipt_type_id = MAX(cashListItem_receipt_type_id)+1 FROM CashListItem_Receipt_Type
        EXECUTE spu_pm_caption_id_return 1, 'Bank Guarantee Debt', @lCaptionID OUTPUT
        INSERT INTO CashListItem_Receipt_Type(cashListItem_receipt_type_id,code, description, caption_id, effective_date,is_deleted, is_Instalment )
    	VALUES(@cashListItem_receipt_type_id,'BGDEPT','Bank Guarantee Debt', @lCaptionID, GETDATE(), 0,0 )

    END
END
GO

--*****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:
-- * Purpose:      Tech Spec WR34 - Claims Recovery Party Link

--*****************************************************************************

IF NOT EXISTS (Select * from Recovery_Party_Type)
BEGIN
    	Declare @Caption_id int

    	EXEC spu_pm_caption_id_return 1,'Agent',@Caption_id Output
    	Insert Into Recovery_Party_Type
		(code,[description],caption_id,effective_date,is_deleted,is_closed)
	values
		('AG','Agent',@Caption_id,Getdate(),0,0)


    	EXEC spu_pm_caption_id_return 1,'Client',@Caption_id Output
    	Insert Into Recovery_Party_Type
		(code,[description],caption_id,effective_date,is_deleted,is_closed)
	values
		('CL','Client',@Caption_id,Getdate(),0,0)

    	EXEC spu_pm_caption_id_return 1,'Insurer',@Caption_id Output
    	Insert Into Recovery_Party_Type
		(code,[description],caption_id,effective_date,is_deleted,is_closed)
	values
		('IN','Insurer',@Caption_id,Getdate(),0,0)

    	EXEC spu_pm_caption_id_return 1,'Other Party',@Caption_id Output
    	Insert Into Recovery_Party_Type
		(code,[description],caption_id,effective_date,is_deleted,is_closed)
	values
		('OT','Other Party',@Caption_id,Getdate(),0,0)

END


IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'RecoveryParty')
BEGIN
INSERT INTO wp_fields
           (field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,loop2,loop3,product_family,data_model,property_id,sub_group2,sub_group3,hidden_option_number,required_option_value,sub_group4,loop4,specials_type)
     VALUES
           ('RecoveryParty','spu_wp_recoveryreserve','resolved_name',0,'Claim','Third Party Recovery','Recovery Party',1,'claimperil','recoveryreserve',NULL,9,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL)
END
GO

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'SalvageRecoveryParty')
BEGIN
INSERT INTO wp_fields
           (field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,loop2,loop3,product_family,data_model,property_id,sub_group2,sub_group3,hidden_option_number,required_option_value,sub_group4,loop4,specials_type)
     VALUES
           ('SalvageRecoveryParty','spu_wp_salvagereserve','resolved_name',0,'Claim','Salvage','Recovery Party',1,'claimperil','salvagereserve',NULL,9,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL)
END
GO

--*****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:
-- * Purpose:      Tech Spec - VAL P14 Policy Numbering

--*****************************************************************************
--Start(Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering (6.3.1)
--Script to be executed
INSERT INTO numbering_scheme_history(
       scheme_valid_from,
       numbering_scheme_id,
       caption_id,
       code,
       description,
       is_deleted,
       effective_date,
       numbering_scheme_type_id,
       numbering_scheme,
       is_generated,
       mask_code,
       fixed_code,
       next_number,
       highest_number,
       step,
       is_reuse_abandoned,
       party_type_id,
       is_read_only)
SELECT
       DATEADD(yy,-1,GETDATE()),
       numbering_scheme_id,
       caption_id,
       code,
       description,
       is_deleted,
       effective_date,
       numbering_scheme_type_id,
       numbering_scheme,
       is_generated,
       mask_code,
       fixed_code,
       next_number,
       highest_number,
       step,
       is_reuse_abandoned,
       party_type_id,
       is_read_only
FROM [numbering_scheme]

--End(Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering (6.3.1)


--*****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:
-- * Purpose:      Tech Spec - US Localization

--*****************************************************************************

DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN PMProduct b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Orion'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Orion')
End
GO

DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN PMProduct b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Sirius Back-office'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Sirius Back-office')
End
GO

DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN PMProduct b
              ON a.caption_id = b.caption_id
              WHERE a.caption = 'Sirius Architecture'
              AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Sirius Architecture')
End
GO



DECLARE @captionID int

SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Cheque Production'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Check Printing')
End
GO

DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Cash / Cheque Receipt'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Cash / Check Receipt')
End
GO

DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Cash / Cheque Payment'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Cash / Check Payment')
End
GO

DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Account Explorer'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values (@captionid,2,'Chart of Accounts')
End
GO



DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Claims'
        AND a.language_id = 1

if not exists ( select * from PMCaption  where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	INSERT INTO PMCaption (caption_id,language_id,caption) VALUES (@captionid,2,'Claims')
End
GO

DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Claims Admin'
        AND a.language_id = 1


if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption)values(@captionid,2,'Claims Admin')
End
GO


DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Client Management'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption)values(@captionid,2,'Client Management')
End
GO


DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Common'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Common')
End
GO

DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Credit Control'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Credit Control')
End
GO


DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'DocuMaster'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'DocuMaster')
End
GO

DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Instalments'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Installment Billing')
End
GO

DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Links'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Links')
End
GO

DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Reports'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Reports')
End
GO

DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'SAM tasks that relate to new business only'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'SAM tasks that relate to new business only')
End
GO

DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'SAM tasks that relate to the creation and maintenance of claims but do not involve the payment or receipt of money'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'SAM tasks that relate to the creation and maintenance of claims but do not involve the payment or receipt of money')
End
GO


DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'SAM tasks that relate to the receipt or payment of money arising from a claim'
        AND a.language_id = 1


if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'SAM tasks that relate to the receipt or payment of money arising from a claim')
End
GO

DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'SAM tasks used for document management'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'SAM tasks used for document management')
End
GO


DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'SAM tasks used for the data transfer process'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'SAM tasks used for the data transfer process')
End
GO

DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'System Administration'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'System Administration')
End
GO

DECLARE @captionID int
        SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Underwriting Maintenance'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Underwriting Maintenance')
End
GO


DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Underwriting Renewals'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Underwriting Renewals')
End
GO

DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Accounts'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Accounting')
End
GO

DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Purchase Ledger Accounts'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Accounts Payable')
End
GO

DECLARE @captionID int
SELECT  @captionID=a.caption_id
        FROM PMCaption a
        inner JOIN pmwrk_task_group b
              ON a.caption_id = b.caption_id
        WHERE a.caption = 'Nominal Ledger Accounts'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption)values(@captionid,2,'General Ledger')
End
GO

--*****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:
-- * Purpose:      Tech Spec WR16 - Cover Note Functionality

--*****************************************************************************

DECLARE @Caption_id int
DECLARE @document_type_id int

EXEC spu_pm_caption_id_return 1,'Cover Note',@Caption_id OUTPUT

IF NOT EXISTS(SELECT caption_id FROM Document_Type
			where caption_id=@Caption_id)
BEGIN

	SELECT @document_type_id= MAX(document_type_id)
			FROM Document_Type
	PRINT @document_type_id
	INSERT INTO Document_Type
		(
			document_type_id,
			caption_id,
			code,
			description,
			is_deleted,
			effective_date,
			is_editable_after_merging
		)
	VALUES
		(
			@document_type_id+1,
			@Caption_id,
			'COVERNOTE',
			'Cover Note',
			0,
			Getdate(),
			1
		)


END
GO


DECLARE @Caption_id int
DECLARE @numbering_scheme_type_id int

EXEC spu_pm_caption_id_return 1,'Cover Note',@Caption_id OUTPUT

IF NOT EXISTS(SELECT caption_id FROM numbering_scheme_type
			where caption_id=@Caption_id)
BEGIN

	SELECT @numbering_scheme_type_id= MAX(numbering_scheme_type_id)
			FROM numbering_scheme_type
	Insert Into numbering_scheme_type
	(
		numbering_scheme_type_id,
		caption_id,
		code,
		description,
		is_deleted,
		effective_date
	)
	values
	(
		@numbering_scheme_type_id+1,
		@Caption_id,
		'COVER NOTE',
		'Cover Note',
		0,
		Getdate()
	)

END
GO

--RiskCoverNoteLink
If Not Exists (SELECT [sql] FROM wp_fields
WHERE         (main_group = 'Risk')
AND     (sub_group = 'Risk')
AND     (field_name = 'RiskCoverNoteLink') )

BEGIN

	INSERT INTO wp_fields
	(
		field_name,
		sql,
		column_name,
		column_type,
		main_group,
		sub_group,
		display_name,
		is_displayed,
		loop1,
		loop2,
		loop3,
		product_family,
		data_model,
		property_id,
		sub_group2,
		sub_group3,
		hidden_option_number,
		required_option_value,
		sub_group4,
		loop4,
		specials_type
	)

	VALUES
	(
		'RiskCoverNoteLink',
		'spu_wp_ris',
		'Risk_Cover_Note_Link_Id',
		11,
		'Risk',
		'Risk',
		'Risk Cover Note Link',
		1,
		NULL,
		NULL,
		NULL,
		9,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL
	)
END

GO



--CoverNoteNumber
If Not Exists (SELECT [sql] FROM wp_fields
                      WHERE (main_group = 'Risk')
                      AND     (sub_group = 'Risk')
                      AND     (field_name = 'CoverNoteRefNumber') )

BEGIN

	INSERT INTO wp_fields
	(
		field_name,
		sql,
		column_name,
		column_type,
		main_group,
		sub_group,
		display_name,
		is_displayed,
		loop1,
		loop2,
		loop3,
		product_family,
		data_model,
		property_id,
		sub_group2,
		sub_group3,
		hidden_option_number,
		required_option_value,
		sub_group4,
		loop4,
		specials_type
	)

	VALUES
	(
		'CoverNoteRefNumber',
		'spu_wp_risk',
		'Cover_Note_Ref',
		0,
		'Risk',
		'Risk',
		'Cover Note Number',
		1,
		NULL,
		NULL,
		NULL,
		9,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL
	)
END

Go



--CoverNoteStartDateTime
If Not Exists (SELECT [sql] FROM wp_fields
WHERE         (main_group = 'Risk')
AND     (sub_group = 'Risk')
AND     (field_name = 'CoverNoteStartDateTime') )

BEGIN

	INSERT INTO wp_fields
	(
		field_name,
		sql,
		column_name,
		column_type,
		main_group,
		sub_group,
		display_name,
		is_displayed,
		loop1,
		loop2,
		loop3,
		product_family,
		data_model,
		property_id,
		sub_group2,
		sub_group3,
		hidden_option_number,
		required_option_value,
		sub_group4,
		loop4,
		specials_type
	)

	VALUES
	(
		'CoverNoteStartDateTime',
		'spu_wp_risk',
		'Cover_Note_From',
		4,
		'Risk',
		'Risk',
		'Cover Note Start Date & Time',
		1,
		NULL,
		NULL,
		NULL,
		9,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL

	)
END
GO


--CoverNoteEndDateTime
If Not Exists (SELECT [sql] FROM wp_fields
WHERE         (main_group = 'Risk')
AND     (sub_group = 'Risk')
AND     (field_name = 'CoverNoteEndDateTime') )
BEGIN
	INSERT INTO wp_fields
	(
		field_name,
		sql,
		column_name,
		column_type,
		main_group,
		sub_group,
		display_name,
		is_displayed,
		loop1,
		loop2,
		loop3,
		product_family,
		data_model,
		property_id,
		sub_group2,
		sub_group3,
		hidden_option_number,
		required_option_value,
		sub_group4,
		loop4,
		specials_type
	)
	VALUES
	(
		'CoverNoteEndDateTime',
		'spu_wp_risk',
		'Cover_Note_To',
		4,
		'Risk',
		'Risk',
		'Cover Note End Date & Time',
		1,
		NULL,
		NULL,
		NULL,
		9,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL
	)
END
GO

--*****************************************************************************
-- * Author:       Rahul Jaiswal
-- * Date:
-- * Purpose:      QBENZCR003 - Back Dated MTAs

--*****************************************************************************

declare @RI2007 int
select @RI2007=value from hidden_options where option_number=88 and value=1
if @RI2007 = 1
Begin
update product set allow_backdated_mtas=1
End


-- *****************************************************************************
-- * Issue No: PN52227 - PLICO INT02 section 4.2.3
-- * Author  : Amit kumar
-- * Date    : 09/12/08       
-- * Purpose : Insurance_File_Cnt needs to be added to the document template field manager         
-- *****************************************************************************

IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN

    IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='InsuranceFileCnt')
        INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
        VALUES('InsuranceFileCnt','spu_wp_insurancefileall','Insurance_File_Cnt', 0,'Policy','General','Insurance File Cnt',1,null,9)
END
GO


--*****************************************************************************
-- * Author:       Rahul Jaiswal
-- * Date:
-- * Purpose:      PGR0026

--*****************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMFP', 'GetFinancePlans' 
GO 

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMFP' 
GO

--*****************************************************************************

-- * Author: Rahul Jaiswal
-- * Date:   23/12/2008
-- *****************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCRsk', 'CopyRisk'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMCRsk'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGPROV', 'ProductRiskOptions'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGPROV'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGROV', 'RiskTypeOptions'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGROV'
GO


EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGUAV', 'Get User Authorities'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGUAV'
GO


EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGAGSETT', 'Get Agent Settings'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGAGSETT'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGPIR', 'Get Policies in Renewal'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGPIR'
GO
--*****************************************************************************


--*****************************************************************************

-- * Author: Gaurav Arora
-- * Date:   29/12/2008
-- *****************************************************************************

--Start (Prakash C Varghese) - (Gap Fixing As told by Gaurav)
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGPCWFO', 'SAMGetProductClaimsWorkflowOptions'
GO

--END (Prakash C Varghese) - (Gap Fixing As told by Gaurav)


--*****************************************************************************

-- * Author: Gaurav Arora
-- * Date:   11/01/2009
-- *****************************************************************************
--Start (Girija) - Tech Spec - PGR028 - SAM MTA Change Credit Card Details.doc

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUpdFP', 'UpdateFinancePlanDetails'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMUpdFP'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMFUKey', 'Find Users'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMFUKey'
GO


--End (Girija) - Tech Spec - PGR028 - SAM MTA Change Credit Card Details.doc


--*****************************************************************************

-- * Author: Gaurav Arora
-- * Date:   11/01/2009
-- *****************************************************************************
--Start (Girija) - Tech Spec - PGR022 - Financial Interfaces.doc

DECLARE @caption_id INT
DECLARE @batch_type_id INT
IF NOT EXISTS (SELECT * from Batch_Type where code = 'RCPTX')
BEGIN
	EXECUTE spu_pm_caption_id_return 1, 'Receipt Export', @caption_id OUTPUT
	SELECT @batch_type_id=max(batch_type_id)+1 from batch_type
	insert into batch_type (batch_type_id,caption_id,is_deleted,effective_date,description,code)
	 values(@batch_type_id,@caption_id,0,getdate(),'Receipt Export','RCPTX')
END

--End (Girija) - Tech Spec - PGR022 - Financial Interfaces.doc


--*****************************************************************************

-- * Author:    Gaurav Arora
-- * Date:      11/01/2009
-- * Purpose:   PN - 54061

-- *****************************************************************************

--Start (Girija) - PN 54061

DECLARE @captionID int

SELECT  @captionID=a.caption_id
        FROM PMCaption a
             inner JOIN pmwrk_task_group b
             ON a.caption_id = b.caption_id
        WHERE a.caption = 'Instalments'
        AND a.language_id = 1


if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Installment Billing')
End


SELECT  @captionID=a.caption_id
        FROM PMCaption a
             inner JOIN pmwrk_task b
             ON a.caption_id = b.caption_id
        WHERE a.caption = 'Instalment Scheme Maintenance'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption)values(@captionid,2,'Installment Scheme Maintenance')
End


SELECT  @captionID=a.caption_id
        FROM PMCaption a
             inner JOIN pmwrk_task b
             ON a.caption_id = b.caption_id
        WHERE a.caption = 'Instalments Export Formats'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption)values(@captionid,2,'Installments Export Formats')
End


SELECT  @captionID=a.caption_id
        FROM PMCaption a
             inner JOIN pmwrk_task b
             ON a.caption_id = b.caption_id
        WHERE a.caption = 'Instalments Export'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption)values(@captionid,2,'Installments Export')
End


SELECT  @captionID=a.caption_id
        FROM PMCaption a
             inner JOIN pmwrk_task b
             ON a.caption_id = b.caption_id
        WHERE a.caption = 'Instalment Plan Maintenance'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Installment Plan Maintenance')
End

SELECT  @captionID=a.caption_id
        FROM PMCaption a
             inner JOIN pmwrk_task b
             ON a.caption_id = b.caption_id
        WHERE a.caption = 'Instalments Post'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Installments Post')
End

SELECT  @captionID=a.caption_id
        FROM PMCaption a
             inner JOIN pmwrk_task b
             ON a.caption_id = b.caption_id
        WHERE a.caption = 'Instalments Recall'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'Installments Recall')
End


SELECT  @captionID=a.caption_id
        FROM PMCaption a
             inner JOIN pmwrk_task b
             ON a.caption_id = b.caption_id
        WHERE a.caption = 'New Instalment Plan'
        AND a.language_id = 1

if not exists ( select * from PMCaption where caption_id=@captionid and language_id=2) AND NOT @captionid IS NULL
Begin
	insert into pmcaption (caption_id,language_id,caption) values(@captionid,2,'New Installment Plan')
End

--*****************************************************************************
-- * Author: Rahul Jaiswal
-- * Date:   14/01/2009
-- *****************************************************************************


UPDATE wp_fields SET column_type=0,sql='spu_wp_recoveryreserve' WHERE field_name='RecoveryParty' AND loop1='claimperil' AND loop2='recoveryreserve'
GO
UPDATE wp_fields SET column_type=0 WHERE field_name='SalvageRecoveryParty' AND sql='spu_wp_salvagereserve'
GO

--*****************************************************************************
-- * Author: Rahul Jaiswal
-- * Date:   14/01/2009
-- *****************************************************************************

IF NOT EXISTS (SELECT NULL FROM wp_fields WHERE field_name = 'PolicyBranchCode')
BEGIN
    INSERT wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value)
    VALUES ('PolicyBranchCode', 'spu_wp_InsuranceFileBranch', 'Branch_code', 0, 'Policy', 'Branch', 'Branch Code', 1, NULL, NULL, NULL, 9, NULL,  NULL, NULL, NULL, NULL, NULL)
END
GO

--**********************************************************************************************  
-- Author: Ramesh Kumar
-- History: 16/01/2009    
-- Task: PN 54178
--********************************************************************************************** 

    DECLARE @report_id INT      
    SELECT @report_id = report_id FROM report WHERE code = 'Inst_Stat'
    IF(ISNULL(@report_id,0) <> 0)
    BEGIN
	UPDATE Report SET Description='Instalment Plan Statement' WHERE report_id=@report_id
    END
GO

--*****************************************************************************

-- * Author:    Gaurav Arora
-- * Date:      11/01/2009
-- * Purpose:   PN - 54429

-- *****************************************************************************


DECLARE @CaptionID INT
 
SELECT  @CaptionID=a.caption_id
        FROM PMCaption a
             inner JOIN pmwrk_task_group b
             ON a.caption_id = b.caption_id
        WHERE a.caption = 'Instalments'
        AND a.language_id = 1

IF @CaptionID > 0
	IF NOT EXISTS ( SELECT * FROM PMCaption WHERE caption_id=@CaptionID and language_id=2) AND NOT @captionid IS NULL
		INSERT INTO pmcaption (caption_id,language_id,caption) VALUES(@CaptionID,2,'Installment Billing')
	Else
		UPDATE PMCaption SET caption='Installment Billing' where caption_id=@CaptionID AND language_id=2
GO

DECLARE @CaptionID INT

SELECT  @CaptionID=PMCaption.caption_id
        FROM PMCaption PMCaption
             inner JOIN pmwrk_task b
             ON PMCaption.caption_id = b.caption_id
        WHERE PMCaption.caption = 'Instalment Scheme Maintenance'
        AND PMCaption.language_id = 1

IF @CaptionID > 0
	IF NOT EXISTS ( SELECT * FROM PMCaption WHERE caption_id=@CaptionID and language_id=2) AND NOT @captionid IS NULL
		INSERT INTO pmcaption (caption_id,language_id,caption) VALUES(@CaptionID,2,'Installment Scheme Maintenance')
	Else
		UPDATE PMCaption SET caption='Installment Scheme Maintenance' WHERE caption_id=@CaptionID AND language_id=2
GO

DECLARE @CaptionID INT

SELECT  @CaptionID=PMCaption.caption_id
        FROM PMCaption PMCaption
             inner JOIN pmwrk_task b
             ON PMCaption.caption_id = b.caption_id
        WHERE PMCaption.caption = 'Instalments Export Formats'
        AND PMCaption.language_id = 1

IF @CaptionID > 0
	if NOT EXISTS ( SELECT * FROM PMCaption WHERE caption_id=@CaptionID and language_id=2) AND NOT @captionid IS NULL
		INSERT INTO pmcaption (caption_id,language_id,caption) VALUES(@CaptionID,2,'Installments Export Formats')
	Else
		UPDATE PMCaption SET caption='Installments Export Formats' WHERE caption_id=@CaptionID AND language_id=2
GO

DECLARE @CaptionID INT

SELECT  @CaptionID=PMCaption.caption_id
        FROM PMCaption PMCaption
             inner JOIN pmwrk_task b
             ON PMCaption.caption_id = b.caption_id
        WHERE PMCaption.caption = 'Instalments Export'
        AND PMCaption.language_id = 1

IF @CaptionID > 0
	IF NOT EXISTS ( SELECT * FROM PMCaption WHERE caption_id=@CaptionID and language_id=2) AND NOT @captionid IS NULL
		INSERT INTO pmcaption (caption_id,language_id,caption) VALUES(@CaptionID,2,'Installments Export')
	Else
		UPDATE PMCaption SET caption='Installments Export' WHERE caption_id=@CaptionID AND language_id=2

GO

DECLARE @CaptionID INT

SELECT  @CaptionID=PMCaption.caption_id
        FROM PMCaption PMCaption
             inner JOIN pmwrk_task b
             ON PMCaption.caption_id = b.caption_id
        WHERE PMCaption.caption = 'Instalment Plan Maintenance'
        AND PMCaption.language_id = 1

IF @CaptionID > 0
	IF NOT EXISTS ( SELECT * FROM PMCaption WHERE caption_id=@CaptionID and language_id=2) AND NOT @captionid IS NULL
		INSERT INTO pmcaption (caption_id,language_id,caption) VALUES(@CaptionID,2,'Installment Plan Maintenance')
	Else
		UPDATE PMCaption SET caption='Installment Plan Maintenance' WHERE caption_id=@CaptionID AND language_id=2

GO

DECLARE @CaptionID INT

SELECT  @CaptionID=PMCaption.caption_id
        FROM PMCaption PMCaption
             inner JOIN pmwrk_task b
             ON PMCaption.caption_id = b.caption_id
        WHERE PMCaption.caption = 'Instalments Post'
        AND PMCaption.language_id = 1

IF @CaptionID > 0
	IF NOT EXISTS ( SELECT * FROM PMCaption WHERE caption_id=@CaptionID and language_id=2) AND NOT @captionid IS NULL
		INSERT INTO pmcaption (caption_id,language_id,caption) VALUES(@CaptionID,2,'Installments Post')
	Else
		UPDATE PMCaption SET caption='Installments Post' WHERE caption_id=@CaptionID AND language_id=2

GO

DECLARE @CaptionID INT

SELECT  @CaptionID=PMCaption.caption_id
        FROM PMCaption PMCaption
             inner JOIN pmwrk_task b
             ON PMCaption.caption_id = b.caption_id
        WHERE PMCaption.caption = 'Instalments Recall'
        AND PMCaption.language_id = 1

IF @CaptionID > 0
	IF NOT EXISTS ( SELECT * FROM PMCaption WHERE caption_id=@CaptionID and language_id=2) AND NOT @captionid IS NULL
		INSERT INTO pmcaption (caption_id,language_id,caption) VALUES(@CaptionID,2,'Installments Recall')
	Else
		UPDATE PMCaption SET caption='Installments Recall' WHERE caption_id=@CaptionID AND language_id=2

GO

DECLARE @CaptionID INT

SELECT  @CaptionID=PMCaption.caption_id
        FROM PMCaption PMCaption
             inner JOIN pmwrk_task b
             ON PMCaption.caption_id = b.caption_id
        WHERE PMCaption.caption = 'New Instalment Plan'
        AND PMCaption.language_id = 1

IF @CaptionID > 0
	IF NOT EXISTS ( SELECT * FROM PMCaption WHERE caption_id=@CaptionID and language_id=2) AND NOT @captionid IS NULL
		INSERT INTO pmcaption (caption_id,language_id,caption) VALUES(@CaptionID,2,'New Installment Plan')
	Else
		UPDATE PMCaption SET caption='New Installment Plan' WHERE caption_id=@CaptionID AND language_id=2

GO



IF NOT EXISTS ( SELECT field_name FROM wp_fields WHERE field_name='BGBankName')
BEGIN
	INSERT INTO wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed
	           			,loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3
	           			,hidden_option_number, required_option_value, sub_group4, loop4, specials_type)
	VALUES('BGBankName', 'spu_wp_BankGuarantee', 'Bank_Name', 11, 'Policy', 'Bank Guarantee', 'Bank Name'
	           ,1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
END
	
	
IF NOT EXISTS ( SELECT field_name FROM wp_fields WHERE field_name='BGBankBranch')
BEGIN
	INSERT INTO wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed
	           			,loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3
	           			,hidden_option_number, required_option_value, sub_group4, loop4, specials_type)
	VALUES('BGBankBranch', 'spu_wp_BankGuarantee', 'Bank_Branch', 11, 'Policy', 'Bank Guarantee', 'Bank Branch'
	           ,1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
END
	
	
IF NOT EXISTS ( SELECT field_name FROM wp_fields WHERE field_name='BGIssueDate')
BEGIN
	INSERT INTO wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed
	           			,loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3
	           			,hidden_option_number, required_option_value, sub_group4, loop4, specials_type)
	VALUES('BGIssueDate', 'spu_wp_BankGuarantee', 'Issue_date', 11, 'Policy', 'Bank Guarantee', 'BG Issue Date'
			,1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
END


IF NOT EXISTS ( SELECT field_name FROM wp_fields WHERE field_name='BGExpiryDate')
BEGIN
	INSERT INTO wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed
	           			,loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3
	           			,hidden_option_number, required_option_value, sub_group4, loop4, specials_type)
	VALUES('BGExpiryDate', 'spu_wp_BankGuarantee', 'Expiry_Date', 11, 'Policy', 'Bank Guarantee', 'BG Expiry Date'
			,1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
END


IF NOT EXISTS ( SELECT field_name FROM wp_fields WHERE field_name='BGNumber')
BEGIN
	INSERT INTO wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed
	           			,loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3
	           			,hidden_option_number, required_option_value, sub_group4, loop4, specials_type)
	VALUES('BGNumber', 'spu_wp_BankGuarantee', 'BG_ref', 11, 'Policy', 'Bank Guarantee', 'BG Number'
			,1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
END

-- *****************************************************************************
-- * Author:  Gurucharan
-- * Date:    05/03/2009
-- * Purpose: PN56603 : Parallel fix of PN56484
-- *****************************************************************************
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
	UPDATE wp_fields SET column_type = 4 WHERE Field_name = 'QuoteExpiryDate' AND sql = 'spu_wp_insurancefileall'
END
GO

-- *****************************************************************************
-- * Author:  Gaurav Arora
-- * Date:    24/03/2009
-- * Purpose: LOA008 Account Handlers
-- *****************************************************************************
--Start(saurabh Agrawal) Tech Spec LOA008 Account Handlers(6.2.1)

DECLARE @CountRow INT
SELECT 	@CountRow = count(*) from party_handler_branch

IF @CountRow = 0
Insert into  
	party_handler_branch(party_cnt,source_id) 
select 
	p.party_cnt , 
	s.source_id 
from 
	party p ,source s 
where 
	p.party_type_id = 6 
GO

--End(saurabh Agrawal) Tech Spec LOA008 Account Handlers(6.2.1)

-- *****************************************************************************
-- * Author:       Prabodh Mishra
-- * Date:         22 April 2009
-- * Purpose:      EFT Number Final
-- *****************************************************************************
if not exists(select code from numbering_scheme_type where code='MEDIAREF') 
   BEGIN 
       
        declare @NextCaptionId int 

        declare @Nextnumbering_scheme_type_id int 

        select @Nextnumbering_scheme_type_id=max(numbering_scheme_type_id)+1 from numbering_scheme_type         
       
        EXECUTE spu_pm_caption_id_return 1, 'Media Type Reference', @NextCaptionId OUTPUT

        insert into numbering_scheme_type(numbering_scheme_type_id,caption_id,code,description,is_deleted,effective_date) 
        values(@Nextnumbering_scheme_type_id,@NextCaptionId,'MEDIAREF','Media Type Reference',0,getdate()) 
        
   END
GO

-- *****************************************************************************
-- * Author:       Prakash
-- * Date:         22 April 2009
-- * Purpose:      EFT Number Final
-- *****************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGPBD', 'GetPartyBankDetails'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGPBD'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAPBD', 'AddPartyBankDetails'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMAPBD'
GO

-- *****************************************************************************
-- * Author:   Gurucharan Gulati
-- * Date:     20-07-2009
-- * Purpose:  PN59305 - Added Report Audit_Report_For_JournalsWithPayments.rpt
-- *****************************************************************************
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U') 
BEGIN
    DECLARE @report_id INT
    DECLARE @lCaptionID INT
    DECLARE @group_id INT

    -- Daily Audit Report Journals With Payments report
    SELECT @report_id = report_id FROM report WHERE code = 'DAY_AJP'

    IF(ISNULL(@report_id,0) = 0)
    BEGIN    
        -- Add report
        SELECT @report_id = MAX(report_id)+1 FROM report
        EXECUTE spu_pm_caption_id_return 1, 'Daily Audit Report Journals with Payments', @lCaptionID OUTPUT
        INSERT INTO report(report_id, caption_id, code, description, is_deleted, effective_date, report_name) 
               VALUES(@report_id, @lCaptionID, 'DAY_AJP', 'Daily Audit Report Journals with Payments', 0, GETDATE(), 'Audit_Report_For_JournalsWithPayments.rpt')

        -- Link report to group
        SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'UND'
        IF(ISNULL(@group_id,0)<>0)
            INSERT INTO Report_Group_Contents (report_group_id,report_id) VALUES(@group_id,@report_id)
    END
END
GO

/**********************************************************************************
Name	:	SQL Script to delete Claim payments with Zero ID
Date	:	21/07/2009
Author	:	Surender Singh
Product	:	SFI RoW - 1.13 SR2 B45
**********************************************************************************/

if EXISTS(SELECT * FROM claim_payment where claim_payment_id=0)
BEGIN
	DELETE FROM claim_payment_item WHERE claim_payment_id=0
	DELETE FROM claim_payment WHERE claim_payment_id=0
END
GO

-- *****************************************************************************
-- * Author:   Gaurav Arora
-- * Date:     29-Jul-09
-- * Purpose:  PGR
-- *****************************************************************************
IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'ManualPolicyEventDesc')
BEGIN
    INSERT INTO wp_fields(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
    VALUES('ManualPolicyEventDesc', 'spu_wp_PolicyEventDescription', 'ManualEventDesc', 0, 'Policy', 'Event', 'Manual Event Description', 1, NULL, 9)
END
GO


-- *****************************************************************************
-- * Author:       Krishan Kumar Gaurav
-- * Date:         04-Nov-2009
-- * Purpose:      Add Document in Documaster
-- *****************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMADDOC', 'SAMAddDocumentToDocumaster'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'Documaster', 'SAMADDOC'
GO

-- *****************************************************************************
-- * Author:       Krishan Kumar Gaurav
-- * Date:         04-Nov-2009
-- * Purpose:      Add New Batch as DOC for Document Export
-- *****************************************************************************
IF Not Exists (Select 1 From Batch_Type Where Code = 'DOC')
BEGIN
    Declare @lCaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'DOC', @lCaptionID output
    
    Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type
    
    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @lCaptionID,
        0,
        GetDate(),
        'Document Export',
        'DOC'
    )
END
GO

-- *****************************************************************************
-- * Author:       Rahul Jaiswal
-- * Date:         13-Nov-2009
-- * Purpose:      PGR 8.1 Batch Notification
-- *****************************************************************************

IF Not Exists (Select 1 From Batch_Type Where Code = 'BATNOT')
BEGIN
    Declare @lCaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'BATNOT', @lCaptionID output
    
    Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type
    
    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @lCaptionID,
        0,
        GetDate(),
        'Batch Notification',
        'BATNOT'
    )
END
GO

-- *****************************************************************************
-- * Author:       Gurucharan
-- * Date:         25-Nov-2009
-- * Purpose:      System Option wrong checkin
-- * Further:	   Under 1.14 System Option 'Cancel Instalment Plan on Policy Cancellation:' 
-- *			   checkin against option number - 5076 and system option 'Recalculate
-- *			   pro-rata reinsurance rates during MTA:' checkin against option number - 5070	
-- *****************************************************************************
DELETE FROM System_Options
WHERE [description]='Cancel Instalment Plan on Policy Cancellation:'
AND option_number<>5076

-- *****************************************************************************  
-- * Author:      Amit Kumar
-- * Date:        7 Sep 2009
-- * Purpose:     Tech Spec - WPR12 - Enhancement Quote Collection Process 
-- *****************************************************************************

DECLARE @lCaptionID  INT
DECLARE @TaskGroupId INT
DECLARE @TaskId      INT

IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
    IF NOT EXISTS (SELECT * FROM PMWrk_Task WHERE code = 'QUCOLLECTP')
    BEGIN
	   EXECUTE spu_pm_caption_id_return 1, 'Quote Collection Process', @lCaptionID OUTPUT

	   INSERT INTO PMWrk_Task
		(caption_id, code, description, is_deleted, effective_date,
		is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name,
		auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name,
		linked_caption_id, is_available_task, pmwrk_task_category_id)

	   SELECT @lCaptionID, 'QUCOLLECTP', 'Quote Collection Process', 0,
		  GETDATE(), 0, 1, NULL, 'iPMUQuoteCollectionProcess', 'NavigatorV3', 0, 42, 0, NULL, NULL, NULL, 1, 2
	   
    END

    -- Create link to group for this new task
    SELECT @TaskId = ( SELECT  pmwrk_task_id FROM pmwrk_task WHERE code = 'QUCOLLECTP')

    -- Add task to correct group
    SELECT @TaskGroupId = ( SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'SLACS')

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task_Group_Task WHERE pmwrk_task_group_id = @TaskGroupId AND pmwrk_task_id  = @TaskId)
    BEGIN
       INSERT INTO pmwrk_task_group_task
       (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num)
       VALUES
       (@TaskGroupId, @TaskId, 0)
    END
END
GO


IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
	DECLARE @pmproduct_id INT
	DECLARE @caption_id INT


   	IF NOT EXISTS (SELECT NULL FROM pmproduct_lookup WHERE lookup_table_name='ChequeType')
	BEGIN
		SELECT @pmproduct_id = pmproduct_id FROM pmproduct WHERE code='Orion'
		EXEC spu_pm_caption_id_return 1, 'Cheque Type', @caption_id OUTPUT

		INSERT INTO pmproduct_lookup(pmproduct_id,lookup_table_name,edit_privilege_level,linked_caption_id,linked_class_name,linked_object_name,is_generic_maintenance,interface_component,interface_control)
		VALUES(@pmproduct_id,'ChequeType',3,@caption_id,NULL,NULL,1,NULL,NULL)
	END

   	IF NOT EXISTS (SELECT NULL FROM pmproduct_lookup WHERE lookup_table_name='Cheque_Clearing_Type')
	BEGIN
		SELECT @pmproduct_id = pmproduct_id FROM pmproduct WHERE code='Orion'
		EXEC spu_pm_caption_id_return 1, 'Cheque Clearing Type', @caption_id OUTPUT

		INSERT INTO pmproduct_lookup(pmproduct_id,lookup_table_name,edit_privilege_level,linked_caption_id,linked_class_name,linked_object_name,is_generic_maintenance,interface_component,interface_control)
		VALUES(@pmproduct_id,'Cheque_Clearing_Type',3,@caption_id,NULL,NULL,1,NULL,NULL)
	END

   	IF NOT EXISTS (SELECT NULL FROM pmproduct_lookup WHERE lookup_table_name='Type_of_Card')
	BEGIN
		SELECT @pmproduct_id = pmproduct_id FROM pmproduct WHERE code='Orion'
		EXEC spu_pm_caption_id_return 1, 'Type of Card', @caption_id OUTPUT

		INSERT INTO pmproduct_lookup(pmproduct_id,lookup_table_name,edit_privilege_level,linked_caption_id,linked_class_name,linked_object_name,is_generic_maintenance,interface_component,interface_control)
		VALUES(@pmproduct_id,'Type_of_Card',3,@caption_id,NULL,NULL,1,NULL,NULL)
	END

END
GO

IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN

    IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='CashListRef')
        INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
        VALUES('CashListRef','spu_wp_QuoteCollectionCashListDetail','CashList_Ref', 0,'Policy','Collection','Cash List Ref',1,'QuoteCollectionCashListDetail',9)

    IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='MediaRef')
        INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
        VALUES('MediaRef','spu_wp_QuoteCollectionCashListDetail','media_ref', 0,'Policy','Collection','Media Ref',1,'QuoteCollectionCashListDetail',9)

    IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='MediaTypeID')
        INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
        VALUES('MediaTypeID','spu_wpQuoteCollectionCashListDetail','mediatype_id', 0,'Policy','Collection','Media Type ID',1,'QuoteCollectionCashListDetail',9)

    IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='DocumentReference')
        INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
        VALUES('DocumentReference','spu_wp_QuoteCollectionCashListDetail','DocumentRef', 0,'Policy','Collection','Document Reference',1,'QuoteCollectionCashListDetail',9)


END
GO

-- *****************************************************************************  
-- * Author:      Rahul Jaiswal
-- * Date:        31 Oct 2009
-- * Purpose:     PN 65545
-- *****************************************************************************

update pmproduct_lookup set PMProduct_id=3,linked_caption_id=null where lookup_table_name in ('ChequeType','Cheque_Clearing_Type','Type_of_Card')
GO

-- *****************************************************************************  
-- * Author:      Rahul Jaiswal
-- * Date:        31 Oct 2009
-- * Purpose:     PN 65534
-- *****************************************************************************

update wp_fields SET loop1='QuoteCollectionCashListDetail', sql = 'spu_wp_QuoteCollectionCashListDetail' where sql like 'spu_wp_QuoteCollection_CashList_Detail'
GO

If Not Exists (SELECT [sql] FROM wp_fields
WHERE         (field_name = 'bank_branch'))

BEGIN	
    INSERT INTO wp_fields
(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)

    VALUES('bank_branch', 'spu_wp_debitCash', 'bank_branch', 0, 'Receipts/Payments', 'Cash', 'Drawee Bank Branch', 1, NULL, 9)
END



If Not Exists (SELECT [sql] FROM wp_fields
WHERE         (field_name = 'BankLocation'))

BEGIN
    INSERT INTO wp_fields
(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)

    VALUES('BankLocation', 'spu_wp_debitCash', 'bank_location', 0, 'Receipts/Payments', 'Cash', 'Drawee Bank Location', 1, NULL, 9)
END

If Not Exists (SELECT [sql] FROM wp_fields
WHERE         (field_name = 'ChequeType'))

BEGIN
    INSERT INTO wp_fields
(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)

    VALUES('ChequeType', 'spu_wp_debitCash', 'ChequeType', 0, 'Receipts/Payments', 'Cash', 'Cheque Type', 1, NULL, 9)
END


If Not Exists (SELECT [sql] FROM wp_fields
WHERE         (field_name = 'ChequeClearingType'))

BEGIN
    INSERT INTO wp_fields
(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)

    VALUES('ChequeClearingType', 'spu_wp_debitCash', 'ChequeClearingType', 0, 'Receipts/Payments', 'Cash', 'Cheque Clearing Type', 1, NULL, 9)
END

If Not Exists (SELECT [sql] FROM wp_fields
WHERE         (field_name = 'CCBank'))

BEGIN
    INSERT INTO wp_fields
(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)

    VALUES('CCBank', 'spu_wp_debitCash', 'CCBank', 0, 'Receipts/Payments', 'Cash', 'Card Issuing Bank Name', 1, NULL, 9)
END


If Not Exists (SELECT [sql] FROM wp_fields
WHERE         (field_name = 'CardType'))

BEGIN
    INSERT INTO wp_fields
(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)

    VALUES('CardType', 'spu_wp_debitCash', 'CardType', 0, 'Receipts/Payments', 'Cash', 'Type of Card', 1, NULL, 9)
END

If Not Exists (SELECT [sql] FROM wp_fields
WHERE         (field_name = 'cc_trans_slip_no'))

BEGIN
    INSERT INTO wp_fields
(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)

    VALUES('cc_trans_slip_no', 'spu_wp_debitCash', 'cc_trans_slip_no', 0, 'Receipts/Payments', 'Cash', 'Transaction Slip Number', 1, NULL, 9)
END

If Not Exists (SELECT [sql] FROM wp_fields
WHERE         (field_name = 'PolicyNumber'))

        INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,sub_group2, display_name,is_displayed,loop1,product_family)
        VALUES('PolicyNumber','spu_wp_debitcashPolicy','insuranceref', 0,'Receipts/Payments','Policy Details','Cash','Policy Number',1,'debitcashPolicy',9)

GO

--*****************************************************************************
-- * Author:       Khalid Naseem
-- * Date:		   17/11/2009
-- * Purpose:      Tech Spec - 8.6 Premium Claims Analysis.doc

--*****************************************************************************

IF Not Exists (Select 1 From Batch_Type Where Code = 'POLBAT')
BEGIN
    Declare @lCaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'POLBAT', @lCaptionID output
    
    Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type
    
    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @lCaptionID,
        0,
        GetDate(),
        'Policy Batch Export',
        'POLBAT'
    )
END
GO

-- *****************************************************************************
-- * Author:       Rob Parker
-- * Date:         01-Dec-2009
-- * Purpose:      Update Quote Payment Method
-- *****************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUPAYM', 'SAMUpdateQuotePaymentMethod'
GO

-- *****************************************************************************
-- * Author:       Gurucharan
-- * Date:         03-Dec-2009
-- * Purpose:      PGR Development 8.5 - Automated Batch Cycle
-- *****************************************************************************

IF NOT EXISTS (Select * from Scheduled_Report_Frequency)
BEGIN
    	DECLARE @Caption_id INT

    	EXEC spu_pm_caption_id_return 1,'Daily',@Caption_id OUTPUT
    	INSERT INTO Scheduled_Report_Frequency
		(code,[description],caption_id,effective_date,is_deleted,is_closed)
	VALUES
		('DL','Daily',@Caption_id,Getdate(),0,0)

    	EXEC spu_pm_caption_id_return 1,'Monthly',@Caption_id OUTPUT
    	INSERT INTO Scheduled_Report_Frequency
		(code,[description],caption_id,effective_date,is_deleted,is_closed)
	VALUES
		('MN','Monthly',@Caption_id,Getdate(),0,0)

    	EXEC spu_pm_caption_id_return 1,'Annually',@Caption_id OUTPUT
    	INSERT INTO Scheduled_Report_Frequency
		(code,[description],caption_id,effective_date,is_deleted,is_closed)
	VALUES
		('AN','Annually',@Caption_id,Getdate(),0,0)
END

-- *****************************************************************************
-- * Author:       Gurucharan
-- * Date:         03-Dec-2009
-- * Purpose:      PGR Development 8.5 - Automated Batch Cycle
-- * Add a new task for Report Scheduler
-- *****************************************************************************

DECLARE @lCaptionID  INT
DECLARE @TaskGroupId INT
DECLARE @TaskId      INT
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
    IF NOT EXISTS (SELECT * FROM PMWrk_Task WHERE code = 'REPSCHD')
    BEGIN
	   EXECUTE spu_pm_caption_id_return 1, 'Report Scheduler', @lCaptionID OUTPUT

	   INSERT INTO PMWrk_Task
		(caption_id, code, description, is_deleted, effective_date,
		is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name,
		auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name,
		linked_caption_id, is_available_task, pmwrk_task_category_id)

	   SELECT @lCaptionID, 'REPSCHD', 'Report Scheduler', 0,
		  GETDATE(), 0, 1, NULL, 'iSIRReportScheduler', 'NavigatorV3', 0, 42, 0, NULL, NULL, NULL, 1, 2
	   
    END

    -- Create link to group for this new task
    SELECT @TaskId = ( SELECT  pmwrk_task_id FROM pmwrk_task WHERE code = 'REPSCHD')

    -- Add task to correct group
    SELECT @TaskGroupId = ( SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'REPORTS')

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task_Group_Task WHERE pmwrk_task_group_id = @TaskGroupId AND pmwrk_task_id  = @TaskId)
    BEGIN
       INSERT INTO pmwrk_task_group_task
       (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num)
       VALUES
       (@TaskGroupId, @TaskId, 0)
    END
END
GO

-- *****************************************************************************
-- * Author:       Rob Parker
-- * Date:         07-Dec-2009
-- * Purpose:      GetPoliciesForRenewalSelection Method
-- *****************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGPFR', 'SAMGetPoliciesForRenewalSelection'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN ', 'SAMGPFR'
GO
-- *****************************************************************************
-- * Author:       Rahul Jaiswal
-- * Date:         06-Dec-2009
-- * Purpose:      PGR 8.11
-- *****************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGDOCE', 'GenerateDocumentsForEvent'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGDOCE'
GO

-- *****************************************************************************
-- * Author:       Rahul Jaiswal
-- * Date:         06-Dec-2009
-- * Purpose:      WPR67
-- *****************************************************************************
DECLARE @caption_id INT

EXECUTE spu_pm_caption_id_return 1, 'Round Off', @caption_id output

if not exists(SELECT * FROM DocumentType where code='SRO')    
 
	INSERT INTO DocumentType(documenttype_id, caption_id, doctypegroup_id, is_deleted, effective_date,description, code, from_sirius) 
	VALUES(56, @caption_id, 2, 0, getdate(), 'Round Off', 'SRO', 1)


SELECT @caption_id = caption_id    FROM pmcaption WHERE caption = 'Document Reference'

if not exists(SELECT * FROM actnumber_group where code in(SELECT 'DOCREF' + CONVERT(VARCHAR(2),DT.documenttype_id)	FROM documenttype DT WHERE dt.code='SRO' )) 
Begin
if exists (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_group')
SET IDENTITY_INSERT actnumber_group ON
	INSERT INTO actnumber_group
 		(		
		actnumber_group_id,
     		code,
	        caption_id,
	        description,
        	is_reset_yearly,
	        is_deleted,
        	effective_date
	 	)
	SELECT
		56,
		'DOCREF' + CONVERT(VARCHAR(2),DT.documenttype_id),
        	@caption_id,
        	'Document Reference',
        	0,
        	0,
        	'2002-10-08'
		FROM documenttype DT
    		WHERE dt.code='SRO'
if exists (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_group')
SET IDENTITY_INSERT actnumber_group OFF
END

	if not exists(SELECT * FROM actnumber_range where code in(SELECT DT.code FROM documenttype DT WHERE dt.code='SRO' )) 
BEGIN
if exists (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_range')
SET IDENTITY_INSERT actnumber_range ON
	INSERT INTO actnumber_range
    		(	
			actnumber_range_id,
        		code,
        		actnumber_group_id,
        		description
    		)
	SELECT
		56,
		DT.code,
        	DT.documenttype_id,
        	DT.code
		FROM documenttype DT
    		WHERE dt.code='SRO'
if exists (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_range')
SET IDENTITY_INSERT actnumber_range OFF
END
GO

Update product set is_roundoff_to_zero = 0 where is_roundoff_to_zero is null
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGEAGCOM', 'SAMGetAgentCommission'
GO


EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUPAGCOM', 'SAMUpdateAgentCommission'
GO

-- *****************************************************************************
-- * Author:       Rob Parker
-- * Date:         01-Jan-2010
-- * Purpose:      Scheme Date Issue
-- *****************************************************************************

UPDATE 
	gis_scheme
SET
	expiry_date = '01 JAN 2099'
GO

-- *****************************************************************************
-- * Author:       Rahul Jaiswal
-- * Date:         19-Jan-2010
-- * Purpose:      PN 62312
-- *****************************************************************************


update wp_fields set column_name ='CC_Expiry_Date' where field_name like 'CCExpiryDate'
update wp_fields set column_name ='CC_Start_Date'  where field_name like 'CCStartDate'
GO


-- *****************************************************************************
-- * Author:       Rahul Jaiswal
-- * Date:         20-Jan-2010
-- * Purpose:      WPR VB64
-- *****************************************************************************


-- Start - Sankar - (WPRvb64 Media Type Status) - Paralleling

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMFINDCAS', 'SAMFindCashListReceipt'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMFINDCAS'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUPRPTMT', 'SAMUpdateReceiptMediaTypeStatus'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMUPRPTMT'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGETPSFM', 'SAMGetPolicyStatusMediaStatus'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGETPSFM'
GO

If Not Exists (SELECT [sql] FROM wp_fields
WHERE         (main_group = 'Receipts/Payments')
AND     (sub_group = 'Cash')
AND     (column_name = 'media_type_status_desc') )

BEGIN
    INSERT INTO wp_fields
(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)

    VALUES('MediaTypeStatus', 'spu_wp_debitCash', 'media_type_status_desc', 0, 'Receipts/Payments', 'Cash', 'Media Status', 1, NULL, 9)
END

GO

DECLARE @lCaptionID  INT
DECLARE @TaskGroupId INT
DECLARE @TaskId      INT
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
    IF NOT EXISTS (SELECT * FROM PMWrk_Task WHERE code = 'MNTMEDTPST')
    BEGIN
	   EXECUTE spu_pm_caption_id_return 1, 'Maintain Media Type Status', @lCaptionID OUTPUT

	   INSERT INTO PMWrk_Task
		(caption_id, code, description, is_deleted, effective_date,
		is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name,
		auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name,
		linked_caption_id, is_available_task, pmwrk_task_category_id)

	   SELECT @lCaptionID, 'MNTMEDTPST', 'Maintain Media Type Status', 0,
		  GETDATE(), 0, 1, NULL, 'iACTMaintainMediaTypeStatus', 'NavigatorV3', 0, 42, 0, NULL, NULL, NULL, 1, 2
	   
    END

    -- Create link to group for this new task
    SELECT @TaskId = ( SELECT  pmwrk_task_id FROM pmwrk_task WHERE code = 'MNTMEDTPST')

    -- Add task to correct group
    SELECT @TaskGroupId = ( SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'SLACS')

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task_Group_Task WHERE pmwrk_task_group_id = @TaskGroupId AND pmwrk_task_id  = @TaskId)
    BEGIN
       INSERT INTO pmwrk_task_group_task
       (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num)
       VALUES
       (@TaskGroupId, @TaskId, 0)
    END
END
GO 

--End - Sankar - (WPRvb64 Media Type Status) - Paralleling




-- *****************************************************************************
-- * Author:       Rahul Jaiswal
-- * Date:         20-Jan-2010
-- * Purpose:      WPR VB64
-- *****************************************************************************


--Start - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling

DECLARE @lCaptionID  INT
DECLARE @TaskGroupId INT
DECLARE @TaskId      INT

IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
    IF NOT EXISTS (SELECT * FROM PMWrk_Task WHERE code = 'CSHDEPMAIN')
    BEGIN
	   EXECUTE spu_pm_caption_id_return 1, 'Cash Deposit Maintenance', @lCaptionID OUTPUT

	   INSERT INTO PMWrk_Task
		(caption_id, code, description, is_deleted, effective_date,
		is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name,
		auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name,
		linked_caption_id, is_available_task, pmwrk_task_category_id)

	   SELECT @lCaptionID, 'CSHDEPMAIN', 'Cash Deposit Maintenance', 0,
		  GETDATE(), 0, 1, NULL, 'iSIRFindCashDeptMaintenance', 'NavigatorV3', 0, 42, 0, NULL, NULL, NULL, 1, 2
	   
    END

    -- Create link to group for this new task
    SELECT @TaskId = ( SELECT  pmwrk_task_id FROM pmwrk_task WHERE code = 'CSHDEPMAIN')

    -- Add task to correct group
    SELECT @TaskGroupId = ( SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'SYSADMIN')

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task_Group_Task WHERE pmwrk_task_group_id = @TaskGroupId AND pmwrk_task_id  = @TaskId)
    BEGIN
       INSERT INTO pmwrk_task_group_task
       (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num)
       VALUES
       (@TaskGroupId, @TaskId, 0)
    END
END
GO  


IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'CDNumber')
BEGIN
    INSERT INTO wp_fields(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
    VALUES('CDNumber', 'spu_wp_CashDeposit', 'CashDeposit_Ref', 0, 'Policy', 'Cash Deposit', 'CD Number', 1, NULL, 9)
END

GO

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'CDBalance')
BEGIN
    INSERT INTO wp_fields(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
    VALUES('CDBalance', 'spu_wp_CashDeposit', 'CDBalance', 0, 'Policy', 'Cash Deposit', 'CD Balance', 1, NULL, 9)
END

GO



--------------------- Account Explorer ---------------------------
DECLARE @ELEMENTID INT
DECLARE @MAPPINGID INT
DECLARE @MAPTYPEID INT
DECLARE @COMPANYID INT
DECLARE @PARENTNODEID INT

IF NOT EXISTS ( SELECT * FROM Element WHERE element_name='Cash Deposit Account')
BEGIN	
	INSERT INTO Element(element_name) VALUES('Cash Deposit Account')
	SET @ELEMENTID=@@IDENTITY
END

IF NOT EXISTS ( SELECT * FROM MAPPING WHERE DESCRIPTION ='Cash Deposit Account')
BEGIN	
	
	SELECT @MAPTYPEID = MAPTYPE_ID FROM MAPTYPE WHERE CODE = 'L'

	DECLARE COMPANY_CUR CURSOR FOR SELECT COMPANY_ID FROM COMPANY

	OPEN COMPANY_CUR;

	FETCH NEXT FROM COMPANY_CUR INTO @COMPANYID;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		INSERT INTO MAPPING(COMPANY_ID, MAPTYPE_ID, DESCRIPTION) VALUES (@COMPANYID, @MAPTYPEID,'Cash Deposit Account')
		SET @MAPPINGID = @@IDENTITY

		SELECT @PARENTNODEID = NODE_ID FROM STRUCTURETREE INNER JOIN ELEMENT ON STRUCTURETREE.ELEMENT_ID = ELEMENT.ELEMENT_ID
			WHERE ELEMENT.ELEMENT_NAME='Current Liabilities' AND STRUCTURETREE.COMPANY_ID = @COMPANYID	
	
		INSERT INTO STRUCTURETREE(COMPANY_ID, MAPPING_ID, ACCOUNT_ID, ELEMENT_ID, PARENT_NODE_ID, CORE_NODE)
			VALUES(@COMPANYID, @MAPPINGID, NULL, @ELEMENTID, @PARENTNODEID, NULL)

		FETCH NEXT FROM COMPANY_CUR INTO @COMPANYID
	END;

	CLOSE COMPANY_CUR;
	DEALLOCATE COMPANY_CUR;

END

GO

--End - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling


--Start - Renuka - (WPR85_Cash_Deposit_Process)
 IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'CDBankName')
BEGIN
    INSERT INTO wp_fields(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
    VALUES('CDBankName', 'spu_wp_GetCDBankName', 'Description', 0, 'Policy', 'Cash Deposit', 'Bank Name', 1, 'GetCDBankName', 9)
END

GO

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'CDBankBranch')
BEGIN
    INSERT INTO wp_fields(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
    VALUES('CDBankBranch', 'spu_wp_GetCDBankName', 'bank_branch', 0, 'Policy', 'Cash Deposit', 'Bank Branch', 1, 'GetCDBankName', 9)
END

GO
--End - Renuka - (WPR85_Cash_Deposit_Process)

-- *****************************************************************************
-- * Author:  Gurucharan
-- * Date:    01/02/2010
-- * Purpose: Add ChargeBack in pfInstalments_status.
-- *****************************************************************************
IF NOT EXISTS(SELECT NULL FROM pfInstalments_status WHERE code='B')
BEGIN
    DECLARE @lCaptionID INT    
    EXECUTE spu_pm_caption_id_return 1, 'Chargeback', @lCaptionID OUTPUT
	DECLARE @lMaxInstalment_Status_id INT

	SELECT @lMaxInstalment_Status_id = MAX(PFInstalments_Status_id) FROM pfInstalments_status
	SET @lMaxInstalment_Status_id = @lMaxInstalment_Status_id + 1

	INSERT INTO pfInstalments_status
	(PFInstalments_Status_id,caption_id, code, description, is_deleted, effective_date)
	VALUES(@lMaxInstalment_Status_id,@lCaptionID, 'B', 'Chargeback', 0, GETDATE())
END
GO


-- *****************************************************************************
-- * Author:   Prakash Varghese
-- * Date:     17/12/2009
-- * Purpose:  To facilitate agent commission tax override
-- *****************************************************************************
--Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.1.1)

-- Update is_tax_amended column of existing records to default value if the current column
-- value is null
UPDATE
	Agent_Commission
SET
	is_tax_amended=0
WHERE
	is_tax_amended IS NULL
--End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.1.1)


-- *****************************************************************************
-- * Author:  Rahul Jaiswal
-- * Date:    08/02/2010
-- * Purpose: 68446
-- *****************************************************************************

-- * Purpose: Added Receipt Media Type Status.rpt       
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U') 
BEGIN
    DECLARE @report_id INT
    DECLARE @lCaptionID INT
    DECLARE @group_id INT

    -- Cancel Payment report
    SELECT @report_id = report_id FROM report WHERE code = 'Rcpt_MTS'

    IF(ISNULL(@report_id,0) = 0)
    BEGIN    
        -- Add report
        SELECT @report_id = MAX(report_id)+1 FROM report
        EXECUTE spu_pm_caption_id_return 1, 'Receipt Media Type Status', @lCaptionID OUTPUT
        INSERT INTO report(report_id, caption_id, code, description, is_deleted, effective_date, report_name) 
               VALUES(@report_id, @lCaptionID, 'Rcpt_MTS', 'Receipt Media Type Status', 0, GETDATE(), 'Receipt_MediaType_Status.rpt')

        -- Link report to group
        SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'ALL'
        IF(ISNULL(@group_id,0)<>0)
            INSERT INTO Report_Group_Contents(report_group_id,report_id)  VALUES(@group_id,@report_id)
    END
END
GO

-- *****************************************************************************
-- * Author:  Amit Kumar
-- * Date:    30 Jun 2010       
-- * Purpose: Get Agent Commission Tax.
-- *****************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGAGCOTX', 'SAMGetAgentCommissionTax'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGAGCOTX'
GO

-- *****************************************************************************
-- * Author:  Ashish Sachdeva
-- * Date:    21 July 2010       
-- * Purpose: PN 73746.
-- *****************************************************************************
DECLARE @pmwrk_task_id int
DECLARE @caption_id int
if not exists(select null from pmwrk_task where code = 'SAMCDAdd')
Begin
    EXECUTE spu_pm_caption_id_return 1, 'SamAddCashDeposit', @caption_id OUTPUT
    insert into pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values(@caption_id, 'SAMCDAdd', 'SamAddCashDeposit',0,getdate(),0,1,null,null,null,null,1,0,null,null,null,0,2,null)
End
GO


DECLARE @pmwrk_task_id int
DECLARE @caption_id int
if not exists(select null from pmwrk_task where code = 'SAMCDUPD')
Begin
    EXECUTE spu_pm_caption_id_return 1, 'SamUpdateCashDeposit', @caption_id OUTPUT
    insert into pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values(@caption_id, 'SAMCDUPD', 'SamUpdateCashDeposit',0,getdate(),0,1,null,null,null,null,1,0,null,null,null,0,2,null)
End
GO

DECLARE @pmwrk_task_id int
DECLARE @caption_id int
if not exists(select null from pmwrk_task where code = 'SAMFCD')
Begin
    EXECUTE spu_pm_caption_id_return 1, 'SamFindCashDeposit', @caption_id OUTPUT
    insert into pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values(@caption_id, 'SAMFCD', 'SamFindCashDeposit',0,getdate(),0,1,null,null,null,null,1,0,null,null,null,0,2,null)
End
GO

DECLARE @pmwrk_task_id int
DECLARE @caption_id int
if not exists(select null from pmwrk_task where code = 'SAMCDGETLN')
Begin
    EXECUTE spu_pm_caption_id_return 1, 'SamGetLinkedCashDeposit', @caption_id OUTPUT
    insert into pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values(@caption_id, 'SAMCDGETLN', 'SamGetLinkedCashDeposit',0,getdate(),0,1,null,null,null,null,1,0,null,null,null,0,2,null)
End
GO

DECLARE @pmwrk_task_id int
DECLARE @caption_id int
if not exists(select null from pmwrk_task where code = 'SAMCDGETLN')
Begin
    EXECUTE spu_pm_caption_id_return 1, 'SamGetLinkedCashDeposit', @caption_id OUTPUT
    insert into pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values(@caption_id, 'SAMCDGETLN', 'SamGetLinkedCashDeposit',0,getdate(),0,1,null,null,null,null,1,0,null,null,null,0,2,null)
End
GO

DECLARE @pmwrk_task_id int
DECLARE @caption_id int
if not exists(select null from pmwrk_task where code = 'SAMCDGET')
Begin
    EXECUTE spu_pm_caption_id_return 1, 'SamGetCashDeposit', @caption_id OUTPUT
    insert into pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values(@caption_id, 'SAMCDGET', 'SamGetCashDeposit',0,getdate(),0,1,null,null,null,null,1,0,null,null,null,0,2,null)
End
GO

DECLARE @pmwrk_task_id int
DECLARE @caption_id int
if not exists(select null from pmwrk_task where code = 'SAMGETNTCD')
Begin
    EXECUTE spu_pm_caption_id_return 1, 'SamGetNextCashDepositRef', @caption_id OUTPUT
    insert into pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values(@caption_id, 'SAMGETNTCD', 'SamGetNextCashDepositRef',0,getdate(),0,1,null,null,null,null,1,0,null,null,null,0,2,null)
End
GO
-- *****************************************************************************  
-- * Author:      Sandeep Kumar
-- * Date:        28 Jul 2010
-- * Purpose:     Tech Spec - WPR85 
-- *****************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGETPCDS', 'GetCashDepositsForPolicy'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGETPCDS'
GO
-- *****************************************************************************  
-- * Author:      Ashish Sachdeva
-- * Date:        29 July 2010
-- * Purpose:     WPR96
-- *****************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMADJNL', 'Add Journal'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMADJNL'
GO

-- *****************************************************************************
-- * Author:   Sandee Kumar
-- * Date:     13/10/2010
-- * Purpose:  Sagicor_WPR13
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGETBS', 'SAM GetBrokerSummary'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGETBS'
GO

-- *****************************************************************************
-- * Author:   Sandee Kumar
-- * Date:     13/10/2010
-- * Purpose:  Sagicor Transfer Quote
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMTQUOT', 'SAM TransferQuote'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMTQUOT'
GO

-- *****************************************************************************
-- * Sharepoint Intergration
-- * Author: Danny Davis
-- * Date: 08/07/2011
-- * Purpose: Add tables for Sharepoint Integration
-- *****************************************************************************

IF NOT EXISTS (SELECT NULL FROM pmproduct_lookup WHERE lookup_table_name='Document_Template_Group')
BEGIN
	DECLARE @pmproduct_id INT
	DECLARE @caption_id INT
	SELECT @pmproduct_id = pmproduct_id FROM pmproduct WHERE code='SirSol'
	EXEC spu_pm_caption_id_return 1, 'Document_Template_Group', @caption_id OUTPUT

	INSERT INTO pmproduct_lookup(pmproduct_id,lookup_table_name,edit_privilege_level,linked_caption_id,linked_class_name,linked_object_name,is_generic_maintenance,interface_component,interface_control,Linked_data_mandatory)
	VALUES(@pmproduct_id,'Document_Template_Group',3,NULL,NULL,NULL,1,NULL,NULL,0)
END
GO

IF NOT EXISTS (SELECT NULL FROM pmproduct_lookup WHERE lookup_table_name='Document_Template_Sub_Group')
BEGIN
	DECLARE @pmproduct_id INT
	DECLARE @caption_id INT
	SELECT @pmproduct_id = pmproduct_id FROM pmproduct WHERE code='SirSol'
	EXEC spu_pm_caption_id_return 1, 'Document_Template_Sub_Group', @caption_id OUTPUT

	INSERT INTO pmproduct_lookup(pmproduct_id,lookup_table_name,edit_privilege_level,linked_caption_id,linked_class_name,linked_object_name,is_generic_maintenance,interface_component,interface_control,Linked_data_mandatory)
	VALUES(@pmproduct_id,'Document_Template_Sub_Group',3,NULL,NULL,NULL,1,NULL,NULL,0)
END
GO

-- *****************************************************************************
-- * Correct invalid space at end of SAMGenDoc 
-- * Author: Danny Davis
-- * Date: 25/07/2011
-- *****************************************************************************
UPDATE PMWrk_Task SET code='SAMGenDoc' WHERE code='SAMGenDoc '
GO

-- *****************************************************************************
-- * Author	: Ram Chandrabose
-- * Date	: 09/03/2011
-- * Purpose: Add a background job row into background_job table 
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMADBGJ', 'SAMCreateBackgroundJob'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMADBGJ'
GO


-- *****************************************************************************
-- * Author	: Danny Davis
-- * Date	: 11/08/2011
-- * Purpose: Correction for migrated databases 
-- *****************************************************************************
UPDATE PMNav_Component SET class_name='Interface_Renamed' WHERE object_name='iPMBFindParty'
AND class_name='Interface'
GO

UPDATE PMNav_Component SET class_name='Interface_Renamed' WHERE object_name like 'iPMBParty%'
AND class_name='Interface'
GO

-- *****************************************************************************
-- * Author:   SANDEEP KUMAR	
-- * Date:     03/08/2011
-- * Purpose:  WPR48
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUPDSW', 'SAM Update Standard Wording'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMUPDSW'

GO

IF NOT EXISTS (SELECT NULL FROM pmproduct_lookup WHERE lookup_table_name='Document_Template_Group')
BEGIN
	DECLARE @pmproduct_id INT
	DECLARE @caption_id INT
	SELECT @pmproduct_id = pmproduct_id FROM pmproduct WHERE code='SirSol'
	EXEC spu_pm_caption_id_return 1, 'Document_Template_Group', @caption_id OUTPUT

INSERT INTO pmproduct_lookup(pmproduct_id,lookup_table_name,edit_privilege_level,linked_caption_id,linked_class_name,linked_object_name,is_generic_maintenance,interface_component,interface_control,Linked_data_mandatory)
	VALUES(@pmproduct_id,'Document_Template_Group',3,NULL,NULL,NULL,1,NULL,NULL,0)
END
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGETSW', 'SAM Get Standard Wording'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGETSW'

GO

-- *****************************************************************************
-- * Author	: Richard Clarke
-- * Date	: 23/08/2011
-- * Purpose: Correction for migrated databases 
-- *****************************************************************************
UPDATE PMNav_Component SET class_name='Interface_Renamed' WHERE object_name='iACTCashListItem'
AND class_name='Interface'
GO

-- *****************************************************************************
-- * Author:	Yogesh Yadav
-- * Date:		08/09/2011
-- * Purpose:	Added UserSignature field
-- *****************************************************************************
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_GetUserValues' AND Field_name='USRSIGNATUREFILE')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('USRSIGNATUREFILE','spu_wp_GetUserValues','USRSIGNATUREFILE','0','System','Logic','User Signature','1',NULL,'9')
END
GO 

-- *****************************************************************************
-- * Correct case of object name for Instalment Scheme Maintenance
-- * Author: Richard Clarke
-- * Date: 15/09/2011
-- *****************************************************************************
UPDATE PMWrk_Task SET component_object_name = 'iGISListMaint' WHERE code = 'PMLISTMAIN'
GO
-- *****************************************************************************
-- * Author:   Azeej Usmani
-- * Date:     18/08/2011
-- * Purpose:  SAGICOR WPR 14 Commission_Level table added to PMProduct_lookup
-- *****************************************************************************

IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U') 
BEGIN
    IF NOT EXISTS (SELECT NULL FROM pmproduct_lookup WHERE lookup_table_name='Commission_Level')
BEGIN 
	INSERT INTO pmproduct_lookup
         	   (pmproduct_id,
          	   lookup_table_name,
          	   edit_privilege_level,
           	   linked_caption_id,
          	   linked_class_name,
           	   linked_object_name,
           	   is_generic_maintenance,
           	   interface_component,
            	   interface_control,
            	   linked_data_mandatory,
            	   linked_data_table_name)
             VALUES      (2,
         	    'Commission_Level',
         	    3,
         	    NULL,
         	    NULL,
          	    NULL,
          	    1,
          	    NULL,
          	    NULL,
            	    0,
            	    NULL)  
    END
END
GO 

-- *****************************************************************************
-- * Author:      Sandeep Kumar
-- * Date:        28/09/2011
-- * Purpose:  	  GetReport parallel from 1.15.2
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGETREPT', 'SAMGetReport'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGETREPT'
GO

-- *****************************************************************************
-- * Correct case of object name for Renewal Amendment
-- * Author: Richard Clarke
-- * Date: 30/08/2011
-- *****************************************************************************
UPDATE PMWrk_Task SET component_object_name = 'iPMURenewalLaunch' WHERE code = 'RENAMEND'
GO

-- *****************************************************************************
-- * Author:	Danny Davis
-- * Date:		26/09/2011
-- * Purpose:	Added Terms of Payment fields for Agent and Insurer
-- *****************************************************************************
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer' AND Field_name='InsurerPaymentTerms')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('InsurerPaymentTerms','spu_wp_insurer','terms_of_payment','0','Party','Insurer','Terms of Payment','1',NULL,'9')
END
GO 

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent' AND Field_name='AgentPaymentTerms')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('AgentPaymentTerms','spu_wp_agent','terms_of_payment','0','Party','Agent','Terms of Payment','1',NULL,'9')
END
GO 

--*****************************************************************************
-- * Author:      rahul jaiswal
-- * Date:        11/04/2012
-- * Purpose:  	  WPR08 ARCH
-- *****************************************************************************

DELETE FROM wp_fields WHERE SQL= 'spu_wp_claimdetails' AND Field_name='TPA'
GO
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_claimdetails' AND Field_name='TPA')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('TPA','spu_wp_claimdetails','claim_TPA','0','Claim','Claim Details','TPA','1',NULL,'9')
END
GO 

--*****************************************************************************
-- * Author:      rahul jaiswal
-- * Date:        11/04/2012
-- * Purpose:  	  Point Manual Journal to correct Component
-- *****************************************************************************

update PMWrk_Task set 
type_of_task= 1,
pmnav_process_id= null,
component_object_name='iACTTransaction',
component_class_name='NavigatorV3'
where code='JOURNAL'


--*****************************************************************************
-- * Author:      rahul jaiswal
-- * Date:        18/05/2012
-- * Purpose:  	  WPR01
-- *****************************************************************************


IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U') 
BEGIN
    DECLARE @report_id INT
    DECLARE @lCaptionID INT
    DECLARE @group_id INT


    SELECT @report_id = report_id FROM report WHERE code = 'AgtBDX'

    IF(ISNULL(@report_id,0) = 0)
    BEGIN    
        -- Add report
        SELECT @report_id = MAX(report_id)+1 FROM report
        EXECUTE spu_pm_caption_id_return 1, 'Bordereau Import Status Report', @lCaptionID OUTPUT
        INSERT INTO report(report_id, caption_id, code, description, is_deleted, effective_date, report_name) 
               VALUES(@report_id, @lCaptionID, 'AgtBDX', 'Bordereau Import Status Report', 0, GETDATE(), 'Bordereau_Import_Status_Report.rpt')

        -- Link report to group
        SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'UND'
        IF(ISNULL(@group_id,0)<>0)
            INSERT INTO Report_Group_Contents (report_group_id,report_id) VALUES(@group_id,@report_id)
    END
END
GO
--*****************************************************************************
-- * Author:      Dharmendra Kumar
-- * Date:        21/06/2012
-- * Purpose:  	  WPR15
-- *****************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGETTAX', 'SAMGetTax'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGETTAX'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUPDTAX', 'SAMUpdateTax'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMUPDTAX'
GO

--*****************************************************************************
-- * Author:     Priyanka Sehgal
-- * Date:        27/09/2012
-- * Purpose:  	  WPR43
-- *****************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGPROBR', 'Products for Branches'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGPROBR'
GO
-- *****************************************************************************
-- * Author:      Sumeet Singh
-- * Date:        10/01/2012
-- * Purpose:  	  Update date cancelled for agent which are wrongly set.
-- *****************************************************************************
IF EXISTS(Select 1 from party_agent where date_cancelled='1899-12-30 00:00:00.000')
BEGIN
update party_agent set date_cancelled='1899-12-29 00:00:00.000' where date_cancelled ='1899-12-30 00:00:00.000'
END
GO

-- *****************************************************************************
-- * Author:   Sahil Ansari
-- * Date:     31/08/2011
-- * Purpose:  SAGICOR WPR 63 
-- *****************************************************************************
 IF NOT EXISTS (SELECT * FROM Quote_Status WHERE code = 'PEND' ) 
	BEGIN
		INSERT INTO Quote_Status(Quote_Status_id,caption_id,code,description) values(1,'','PEND','Pending')	
	END

	
 IF NOT EXISTS (SELECT * FROM Quote_Status WHERE code = 'AGPEND' ) 
	BEGIN
		INSERT INTO Quote_Status(Quote_Status_id,caption_id,code,description) values(2,'','AGPEND','Agent Pending')	
	END

 IF NOT EXISTS (SELECT * FROM Quote_Status WHERE code = 'AGCOMP' ) 
	BEGIN
		INSERT INTO Quote_Status(Quote_Status_id,caption_id,code,description) values(3,'','AGCOMP','Agent Complete')	
	END
	
 IF NOT EXISTS (SELECT * FROM Quote_Status WHERE code = 'ISS' ) 
	BEGIN
		INSERT INTO Quote_Status(Quote_Status_id,caption_id,code,description) values(4,'','ISS','Issued')	
	END	
	
 IF NOT EXISTS (SELECT * FROM Quote_Status WHERE code = 'LIVE' ) 
	BEGIN
		INSERT INTO Quote_Status(Quote_Status_id,caption_id,code,description) values(5,'','LIVE','Live')	
	END	


 IF NOT EXISTS (SELECT * FROM Quote_Status WHERE code = 'DECLINED' ) 
	BEGIN
		INSERT INTO Quote_Status(Quote_Status_id,caption_id,code,description) values(6,'','DECLINED','Declined')	
	END	

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCOPYQTE', 'SAMCopyQuote'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMCOPYQTE'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUPDQST', 'SAMUpdateQuoteStatus'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMUPDQST'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMDELPOL', 'SAMDeletePolicy'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMDELPOL'
GO


IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='QuoteVersion')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,product_family)
    VALUES ('QuoteVersion','spu_wp_insurancefileall','QuoteVersion',11,'Policy','General','Quote Version Number',1,9)

END



IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U') 
BEGIN
    DECLARE @report_id INT
    DECLARE @lCaptionID INT
    DECLARE @group_id INT

    
    SELECT @report_id = report_id FROM report WHERE code = 'Del_Quote'

    IF(ISNULL(@report_id,0) = 0)
    BEGIN    
        -- Add report
        SELECT @report_id = MAX(report_id)+1 FROM report
        EXECUTE spu_pm_caption_id_return 1, 'Quote Version Deletion Report', @lCaptionID OUTPUT
        INSERT INTO report(report_id, caption_id, code, description, is_deleted, effective_date, report_name) 
               VALUES(@report_id, @lCaptionID, 'Del_Quote', 'Quote Version Deletion Report', 0, GETDATE(), 'Quote_Version_Deletion_Report.rpt')

        -- Link report to group
        SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'UND'
        IF(ISNULL(@group_id,0)<>0)
	BEGIN
		INSERT INTO Report_Group_Contents (report_group_id,report_id) VALUES(@group_id,@report_id)
		INSERT INTO Report_Group_Contents (report_group_id,report_id) VALUES(1,@report_id) -- This is a temp fix,
	END
    END

	
	set @report_id=null
	
    SELECT  @report_id = report_id FROM report WHERE code = 'Del_QuoteF'
    

    IF(ISNULL(@report_id,0) = 0)
    BEGIN
        -- Add report
        SELECT @report_id = MAX(report_id)+1 FROM report
        EXECUTE spu_pm_caption_id_return 1, 'Quote Version Deletion Failure Report', @lCaptionID OUTPUT
        INSERT INTO report(report_id, caption_id, code, description, is_deleted, effective_date, report_name) 
               VALUES(@report_id, @lCaptionID, 'Del_QuoteF', 'Quote Version Deletion Failure Report', 0, GETDATE(), 'Quote_Version_Deletion_Failure_Report.rpt')

        -- Link report to group
        SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'UND'
        IF(ISNULL(@group_id,0)<>0)
	BEGIN
            INSERT INTO Report_Group_Contents (report_group_id,report_id) VALUES(@group_id,@report_id)
	        INSERT INTO Report_Group_Contents (report_group_id,report_id) VALUES(1,@report_id) -- This is a temp fix,
	 END	
    END

END


-- *****************************************************************************
-- * Correct case of object name for Instalment Scheme Maintenance
-- * Author: Richard Clarke
-- * Date: 06/09/2011
-- *****************************************************************************
UPDATE PMWrk_Task SET component_object_name = 'iPMBPFSchemeMaint' WHERE code = 'PFMAINT'
GO



-- *****************************************************************************
-- * Author:	Sahil Ansari
-- * Date:	30/11/2011
-- * Purpose:	WPR 63
-- *****************************************************************************
DECLARE @lCaptionID  INT
DECLARE @TaskGroupId INT
DECLARE @TaskId      INT


    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task WHERE code = 'SUMCOV')
    BEGIN
	   EXECUTE spu_pm_caption_id_return 1, 'Summary Of Cover', @lCaptionID OUTPUT

	   INSERT INTO PMWrk_Task 
		(caption_id, code, description, is_deleted, effective_date, 
		is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, 
		auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, 
		linked_caption_id, is_available_task, pmwrk_task_category_id) 

	   VALUES( @lCaptionID, 'SUMCOV', 'Summary Of Cover', 0, 
		  GETDATE(), 0, 1, NULL, '', '', 0, 1, 0, NULL, NULL, NULL, 0, 2)
	   
    END
GO
-- *****************************************************************************
-- * Author:  sandeep kumar
-- * Date:    09/12/2011
-- * Purpose: wpr 48
-- *****************************************************************************      
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U') 
BEGIN
    DECLARE @report_id INT
    DECLARE @lCaptionID INT
    DECLARE @group_id INT

    -- Cancel Payment report
    SELECT @report_id = report_id FROM report WHERE code = 'Rcpt_DBD'

    IF(ISNULL(@report_id,0) = 0)
    BEGIN    
        -- Add report
        SELECT @report_id = MAX(report_id)+1 FROM report
        EXECUTE spu_pm_caption_id_return 1, 'Debiting Bordereau Detailed', @lCaptionID OUTPUT
        INSERT INTO report(report_id, caption_id, code, description, is_deleted, effective_date, report_name) 
               VALUES(@report_id, @lCaptionID, 'Rcpt_DBD', 'Debiting Bordereau Detailed', 0, GETDATE(), 'SubAgent_Debiting_Bordereau_Detailed.rpt')

        -- Link report to group
        SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'ALL'
        IF(ISNULL(@group_id,0)<>0)
            INSERT INTO Report_Group_Contents(report_group_id,report_id)  VALUES(@group_id,@report_id)
    END
END
GO
-- *****************************************************************************
-- * Author:  sandeep kumar
-- * Date:    09/12/2011
-- * Purpose: wpr 48
-- *****************************************************************************
     
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U') 
BEGIN
    DECLARE @report_id INT
    DECLARE @lCaptionID INT
    DECLARE @group_id INT

    -- Cancel Payment report
    SELECT @report_id = report_id FROM report WHERE code = 'Rcpt_PBD'

    IF(ISNULL(@report_id,0) = 0)
    BEGIN    
        -- Add report
        SELECT @report_id = MAX(report_id)+1 FROM report
        EXECUTE spu_pm_caption_id_return 1, 'Paid Bordereau Detailed', @lCaptionID OUTPUT
        INSERT INTO report(report_id, caption_id, code, description, is_deleted, effective_date, report_name) 
               VALUES(@report_id, @lCaptionID, 'Rcpt_PBD', 'Paid Bordereau Detailed', 0, GETDATE(), 'SubAgent_Paid_Bordereau_Detailed.rpt')

        -- Link report to group
        SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'ALL'
        IF(ISNULL(@group_id,0)<>0)
            INSERT INTO Report_Group_Contents(report_group_id,report_id)  VALUES(@group_id,@report_id)
    END
END
GO



IF EXISTS(SELECT * FROM sysobjects WHERE ID = OBJECT_ID('UF_StringToTable'))
 DROP FUNCTION UF_StringToTable
GO

CREATE FUNCTION UF_StringToTable
(
 @CSString VARCHAR(8000)
)
RETURNS @otTemp TABLE(sID VARCHAR(20))
AS
BEGIN
 DECLARE @sTemp VARCHAR(10)

 WHILE LEN(@CSString) > 0
 BEGIN
  SET @sTemp = LEFT(@CSString, ISNULL(NULLIF(CHARINDEX(',', @CSString) - 1, -1),
                    LEN(@CSString)))
  SET @CSString = SUBSTRING(@CSString,ISNULL(NULLIF(CHARINDEX(',', @CSString), 0),
                               LEN(@CSString)) + 1, LEN(@CSString))
  INSERT INTO @otTemp VALUES (@sTemp)
 END

RETURN
END
Go

-- *****************************************************************************
-- * Author:      Sumeet Singh
-- * Date:        28/02/2012
-- * Purpose:  	  Update pfFrequency with distinct values of payment_term_code of Party table.
-- *****************************************************************************
Declare @STID INT
Select @STID = system_type_id from sys.columns where name = 'payment_term_code' and object_id = (select object_id from sys.tables where name = 'party')

IF @STID <>  56
BEGIN
DECLARE @lCaptionID int, 
		@pffrequency_id INT,
		@payment_term_code VARCHAR(255), 
		@CODE VARCHAR(20),
		@iCount INT

SELECT @pffrequency_id = MAX(pffrequency_id) FROM PFFrequency
Select @iCount = 1
Declare payment_code Cursor Fast_Forward 
For  

Select distinct payment_term_code from party Where LEN(payment_term_code)>1 AND payment_term_code NOT IN(select description from PFFrequency)

OPEN payment_code

Fetch Next From payment_code Into @payment_term_code

While (@@Fetch_Status = 0)
BEGIN
	SET @pffrequency_id = @pffrequency_id + 1

	EXEC spu_pm_caption_id_return 1, @payment_term_code, @lCaptionID output
    
    SELECT @CODE = 'PTC' + Convert(Varchar,@iCount)
    
	INSERT INTO PFFrequency (pffrequency_id,code, description, caption_id,			effective_date,is_deleted,period,amount,is_available_on_client_screen,			is_available_on_instalment_screen)
	Values
	(@pffrequency_id,@CODE,@payment_term_code,@lCaptionID,GETDATE(),0,'m',12,1,0)
	
	Select @iCount=@iCount + 1
	
Fetch Next From payment_code Into @payment_term_code
END

CLOSE payment_code
DEALLOCATE payment_code

IF @iCount >1

BEGIN
Update party set payment_term_code = (select pffrequency_id from PFFrequency where description = party.payment_term_code)
END

END
GO

DDLAlterColumn party, payment_term_code, 'int null'

GO
DDLAlterColumn Invoice, Code, 'varchar(30)'

GO
DDLAlterColumn Invoice_Item, nominal_code, 'varchar(30)'

GO

declare @navKey int
declare @processID int
declare @mapID int
IF NOT EXISTS (SELECT NULL FROM pmnav_key WHERE name = 'MultiCurrencyFlag')
BEGIN
	select @navKey = ISNULL(MAX(pmnav_key_id), 0) + 1  from    PMNav_Key 
	select @processID = pmnav_process_id from PMNav_Process where code = 'ACTIPAY'
	select @mapID= pmnav_map_id from PMNav_Map where code = 'ACTIPAYM1'
	--select @navKey ,@processID
    
    INSERT INTO pmnav_key (pmnav_key_id,Name,Description,data_type,is_deleted,effective_date ) 
    values (@navKey,'MultiCurrencyFlag','MultiCurrency Flag',0,0,GETDATE()-1)    
    
    INSERT INTO PMNav_Set_Process_Key(pmnav_process_id, pmnav_key_id, sequence_number, description, initial_key_value) 
    VALUES (@processID, @navKey, 9, 'MultiCurrency Flag',' ' )	
    
    INSERT INTO PMNav_Set_Map_Key(pmnav_map_id, pmnav_key_id, sequence_number, description, initial_key_value) 
    VALUES (@mapID, @navKey, 9, 'MultiCurrency Flag',' ') 
    
    INSERT INTO pmnav_set_step_key (pmnav_map_id,pmnav_step_id,pmnav_key_id,sequence_number,description,initial_key_value)
    values (@mapID,1,@navKey,4,'MultiCurrencyFlag',' ')
    
END
GO

-- *****************************************************************************
-- * Author:	Archana (as Suggested by Roopali)
-- * Date:		02/01/2012
-- * Purpose:	To Remove update PMCaption Table all "Finance Plan Maint" Captions will be replaced by "Finance Plan Maintenance"
-- *****************************************************************************
update pmcaption set caption ='Finance Plan Maintenance'  where caption  ='Finance Plan Maint'
GO

declare @pmwrk_task_id int
declare @caption_id int
if not exists(select null from pmwrk_task where code = 'SAMPOLDATA')
Begin
    EXECUTE spu_pm_caption_id_return 1, 'SAMPOLDATA', @caption_id OUTPUT
    insert into pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values(@caption_id, 'SAMPOLDATA', 'SAMPOLDATA',0,getdate(),0,1,null,null,null,null,1,0,null,null,null,0,2,null)
End
GO
declare @pmwrk_task_id int
declare @caption_id int
if not exists(select null from pmwrk_task where code = 'DELCLIENT')
Begin

    EXECUTE spu_pm_caption_id_return 1, 'DELCLIENT ', @caption_id OUTPUT
    insert into pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values(@caption_id, 'DELCLIENT', 'Delete a client',0,getdate(),0,1,null,'iPMBDeleteClientWrapper',null,null,1,0,null,null,null,0,2,null)
End
go

-- *****************************************************************************
-- * Author:	Inder (as Suggested by Roopali)
-- * Date:		10/05/2012
-- * Purpose:	To set  is_deleted=1 in   Fee_Amounts Table where Party_Cnt=0
-- *****************************************************************************

DECLARE @count int
SELECT @count= COUNT(*) FROM Fee_Amounts WHERE party_cnt = 0 and is_deleted=0

if @count>0
  BEGIN
     update Fee_Amounts set is_deleted=1 where party_cnt=0
  END
  
GO 
-- *****************************************************************************
-- * Author:   Vidya Rangdale
-- * Date:     16/05/2010
-- * Purpose:  To fix issue 1769
-- *****************************************************************************
DECLARE @caption_id INT
select @caption_id=caption_id from CashListItem_Payment_Type where description like 'General Payment'
IF @caption_id >0 
BEGIN
UPDATE PMCaption SET CAPTION='General Payment' WHERE caption_id=@caption_id
END
GO

declare @pmwrk_task_id int
declare @caption_id int
if not exists(select null from pmwrk_task where code = 'DELCLIENT')
Begin

    EXECUTE spu_pm_caption_id_return 1, 'DELCLIENT ', @caption_id OUTPUT
    insert into pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values(@caption_id, 'DELCLIENT', 'Delete a client',0,getdate(),0,1,null,'iPMBDeleteClientWrapper',null,null,1,0,null,null,null,0,2,null)
End
go

-- *****************************************************************************
-- * Author:	Sumeet Singh
-- * Date:		17/10/2012
-- * Purpose:	Added AlternateReference field
-- *****************************************************************************
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurancefileall' AND Field_name='AlternateReference')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('AlternateReference','spu_wp_insurancefileall','alternate_reference','0','Policy','General','Alternate Reference','1',NULL,'9')
END
GO 

-- *****************************************************************************
-- * Author:	Sandeep Kumar
-- * Date:		19/10/2012
-- * Purpose:	Other Caption Id
-- *****************************************************************************

 
DECLARE @captionID int
SELECT @captionID=CAPTION_ID FROM MTA_Event_Description WHERE CODE='OTHER'
IF ISNULL(@captionID,'')<>''
BEGIN

IF NOT EXISTS ( SELECT * FROM PMCaption WHERE caption_id=@captionid AND language_id=1)
BEGIN
	INSERT INTO pmcaption (caption_id,language_id,caption) VALUES(@captionid,1,'Other')
END

END
GO

-- *****************************************************************************
-- * Author:      Tarun Diwan
-- * Date:        26 Jul 2012
-- * Purpose:  	  WPR29 DME
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMFINDDME', 'FindDMEDocuments'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMFINDDME'

GO 
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMDMEFOLD', 'GetDMEFolder'

GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMDMEFOLD'

GO 


  -- *****************************************************************************

-- *****************************************************************************
-- * Author:   Vijay Pal
-- * Date:     29/06/2012
-- * Purpose:  SMMI 1.31
--*******************************************************************************
IF EXISTS (SELECT 1 FROM pmwrk_task WHERE description='Party Account Settlement' AND CODE='ACTINSPAY')
	BEGIN
         DECLARE @lCaptionID integer
	 EXEC spu_pm_caption_id_return 1, 'Insurer Payment', @lCaptionID output
         UPDATE pmwrk_task SET description='Insurer Payment',caption_id=@lCaptionID 	 WHERE description='Party Account Settlement' AND CODE='ACTINSPAY'
    END
	GO
-- *****************************************************************************
-- * Author:   Vijay Pal
-- * Date:     29/06/2012
-- * Purpose:  SMMI 1.31
-- *****************************************************************************

DECLARE @lCaptionID INT
IF NOT EXISTS(SELECT NULL FROM TransDetail_Selection_Type WHERE code = 'COMM')
BEGIN
 EXEC spu_pm_caption_id_return 1, 'Commission Selection', @lCaptionID OUTPUT
 INSERT INTO TransDetail_Selection_Type(transdetail_selection_type_id,
 code,description,caption_id,effective_date,is_deleted)
 VALUES (1,'COMM','Commission Selection',@lCaptionID,GETDATE(),0)
END

IF NOT EXISTS(SELECT NULL FROM TransDetail_Selection_Type WHERE code = 'ACCOUNT')
BEGIN
 EXEC spu_pm_caption_id_return 1, 'Account Selection (for future)', @lCaptionID OUTPUT
 INSERT INTO TransDetail_Selection_Type(transdetail_selection_type_id,
 code,description,caption_id,effective_date,is_deleted)
 VALUES (2,'ACCOUNT','Account Selection (for future)',@lCaptionID,
 GETDATE(),0)
END

IF NOT EXISTS(SELECT NULL FROM TransDetail_Selection_Type WHERE code = 'OFFSET')
BEGIN
 EXEC spu_pm_caption_id_return 1, 'Offset Selection', @lCaptionID OUTPUT
 INSERT INTO TransDetail_Selection_Type(transdetail_selection_type_id,
 code,description,caption_id,effective_date,is_deleted)
 VALUES (3,'OFFSET','Offset Selection',@lCaptionID,GETDATE(),0)
END
GO
-- *****************************************************************************
-- * Author:   Vijay Pal
-- * Date:     29/06/2012
-- * Purpose:  SMMI 1.31
-- *****************************************************************************
IF NOT EXISTS (SELECT lookup_table_name 
FROM PMProduct_Lookup 
WHERE lookup_table_name = 'TransDetail_Selection_Type')
BEGIN
 INSERT INTO PMProduct_Lookup
 (pmproduct_id, lookup_table_name, edit_privilege_level, 
 is_generic_maintenance, linked_data_mandatory)
 VALUES       (2, 'TransDetail_Selection_Type', 3, 0, 0)
END
GO
-- *****************************************************************************
-- * Author:   Vijay Pal
-- * Date:     29/06/2012
-- * Purpose:  SMMI 1.31
-- *****************************************************************************
IF Not Exists (Select 1 From Batch_Type Where Code = 'CMS')
BEGIN
 Declare @lCaptionID integer
 Declare @Batch_Type_Id integer

 Execute spu_pm_caption_id_return 1, 'CMS', @lCaptionID output

 Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type

 Insert into Batch_type 
 (
 Batch_type_id,
 Caption_id,
 is_deleted,
 effective_date,
 Description,
 Code
 )
 Values
 (
 @Batch_Type_Id,
 @lCaptionID,
 0,
 GetDate(),
 'Commission Statement',
 'CMS'
 )
END
GO
-- *****************************************************************************
-- * Author:   Vijay Pal
-- * Date:     29/06/2012
-- * Purpose:  SMMI 1.31
-- *****************************************************************************
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U') 
BEGIN
    DECLARE @report_id INT
    DECLARE @lCaptionID INT
    DECLARE @group_id INT

    -- Batch Renewal Job Summary Report
    SELECT @report_id = report_id FROM report WHERE code = 'comm_Stmt'

    IF(ISNULL(@report_id,0) = 0)
    BEGIN    
        -- Add report
        SELECT @report_id = MAX(report_id)+1 FROM report
        EXECUTE spu_pm_caption_id_return 1, 'Commission Statement', @lCaptionID OUTPUT
        INSERT INTO report(report_id, caption_id, code, description, is_deleted, effective_date, report_name) 
		VALUES(@report_id, @lCaptionID, 'comm_Stmt', 'Commission Statement', 0, GETDATE(), 'Commission_Statement.rpt')

        -- Link report to group
        SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'ALL'
        IF(ISNULL(@group_id,0)<>0)
		INSERT INTO Report_Group_Contents (report_group_id,report_id) VALUES(@group_id,@report_id)
    END
END
GO


-- *****************************************************************************
-- * Author:   Vijay Pal
-- * Date:     29/06/2012
-- * Purpose:  SMMI 1.31	
-- *****************************************************************************
IF NOT EXISTS( SELECT 1 fROM pmnavxm_process WHERE file_name = 'COMMPAY.XML')
BEGIN
Declare @pmnavxm_process_id int
Select @pmnavxm_process_id = MAX(pmnavxm_process_id)+1 FROM pmnavxm_process

insert into pmnavxm_process ( pmnavxm_process_id,file_name,file_version_number,file_timestamp,is_custom,is_core,xml_definition)
values
(@pmnavxm_process_id, 'COMMPAY.XML',25,getdate(), 0,1,
'<?xml version="1.0"?>
<!DOCTYPE MAP SYSTEM "navigatorxmV2.dtd">
<MAP WMTaskCode="COMMPAY" WMTaskDescription="Commission Payments" ImageURL="http://www.siriusgroup.co.uk" TransactionType="NB" ProcessMode="0" AutoClose="True" NavigatorDriven="True" Title="NavigatorXM" RoadmapName="Commission Payments" Core="1" Version="25" ElementID="E1">
	<STEP Description="Find Agents" Component="iPMBFindWrapper.NavigatorV3" Type="DF" CancelAction="AP" OKAction="F1" 	OKSteps="0" CancelSteps="0" ComponentAction="1" ServerSide="False" CreateWMTask="True" Core="1" ResumeStep="-1" 	Submap="" OKNewRoadmap="" CancelNewRoadmap="" ElementID="E2">
		<KEY Name="OverrideFind" Value="1" ElementID="E21"/>
		<KEY Name="FindType" Value="0" ElementID="E22"/>
		<KEY Name="CF;party_party_type_id;R;0" Value="3" ElementID="E23"/>
		<KEY Name="DefaultFindScreen" Value="iPMBFindPartyWrapper.NavigatorV3" ElementID="E24"/>
		<KEY Name="special_party" Value="AG" ElementID="E25"/>
		<KEY Name="AllowMultiSelection" Value="True" ElementID="E26"/>
	</STEP>
	<STEP Description="Agent Summary" Component="iACTCommissionPayments.NavigatorV3" Type="FF" CancelAction="AP" 	OKAction="CP" OKSteps="0" CancelSteps="0" ComponentAction="2" ServerSide="False" CreateWMTask="True" Core="1" 	ResumeStep="-1" Submap="" OKNewRoadmap="" CancelNewRoadmap="" ElementID="E4">
	</STEP>
</MAP>')
END
GO

-- *****************************************************************************
-- * Author:   Vijay Pal
-- * Date:     29/06/2012
-- * Purpose:  SMMI 1.31	
-- *****************************************************************************
    -- Create new task for Navigator Editor
    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task WHERE code = 'COMMPAY')
    BEGIN
		DECLARE @lCaptionID int
		DECLARE @TaskGroupId int
		DECLARE @TaskId AS INTEGER
		DECLARE @pmnavxm_process_id int 
		Select @pmnavxm_process_id = pmnavxm_process_id FROM pmnavxm_process WHERE  file_name = 'COMMPAY.XML'
        EXECUTE spu_pm_caption_id_return 1, 'Commission Payments', @lCaptionID output
        
        INSERT INTO PMWrk_Task (caption_id, code, description, is_deleted, effective_date, 
				is_system_task, type_of_task, pmnav_process_id, component_object_name, 
				component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, 
				linked_object_name, linked_class_name, linked_caption_id, is_available_task,
				pmwrk_task_category_id,pmnavxm_process_id) 
        SELECT  @lCaptionID, 'COMMPAY', 'Commission Payments', 0, 
                    GETDATE()-1, 0, 2, NULL, '', '', 0, 38, 0, NULL, NULL, NULL, 1, 2, @pmnavxm_process_id


	-- Create link to group for this new task
	SELECT @TaskId = ( SELECT pmwrk_task_id FROM pmwrk_task WHERE code = 'COMMPAY')

	-- Add task to correct group
	SELECT @TaskGroupId = ( SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'SLACS')

	IF NOT EXISTS (SELECT NULL FROM PMWrk_Task_Group_Task WHERE pmwrk_task_group_id = @TaskGroupId AND 				pmwrk_task_id  = @TaskId)
	BEGIN
		INSERT INTO pmwrk_task_group_task
		   (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num)
		   VALUES
		   (@TaskGroupId, @TaskId, 0)
		END
	END
GO
-- *****************************************************************************
-- * Author:      Tarun Diwan
-- * Date:        26 Jul 2012
-- * Purpose:  	  WPR29 DME
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMFINDDME', 'FindDMEDocuments'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMFINDDME'

GO 
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMDMEFOLD', 'GetDMEFolder'

GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMDMEFOLD'

GO 
--- * Author: Vijay Pal
-- * Date: 21/08/2012
-- * Purpose:	WPR 1.31
-- *****************************************************************************
Delete from pmnavxm_process WHERE file_name IN('ACTIPAYINST.XML','ACTIPAY.XML')
GO
IF NOT EXISTS( SELECT 1 fROM pmnavxm_process WHERE file_name = 'ACTIPAY.XML')
BEGIN
Declare @pmnavxm_process_id int
Select @pmnavxm_process_id = MAX(pmnavxm_process_id)+1 FROM pmnavxm_process

insert into pmnavxm_process ( pmnavxm_process_id,file_name,file_version_number,file_timestamp,is_custom,is_core,xml_definition)
values
(@pmnavxm_process_id, 'ACTIPAY.XML',25,getdate(), 0,1,
'<?xml version="1.0"?>
<!-- edited with XML Spy v4.4 U (http://www.xmlspy.com) by Internal Use (Sirius Financial Solutions Plc) -->
<!-- pmnav_map_id = 18 -->
<!DOCTYPE MAP SYSTEM "navigatorxmv2.dtd">
<MAP WMTaskCode="ACTIPAY" WMTaskDescription="Insurer Payment" ImageURL="http://www.siriusgroup.co.uk" TransactionType="" ProcessMode="1" AutoClose="False" NavigatorDriven="True" Title="NavigatorXM" RoadmapName="Insurer Payment" Core="1" Version="28" ElementID="E1">
	<STEP Description="Cash List" Component="iACTCashList.NavigatorV3" Type="FF" CancelAction="AP" OKAction="F1" OKSteps="1" CancelSteps="0" ComponentAction="1" ServerSide="False" CreateWMTask="False" ResumeStep="" Core="1" Submap="" OKNewRoadmap="" CancelNewRoadmap="" ElementID="E2"/>
	<STEP Description="Cash List Item" Component="iACTCashListItem.Interface_Renamed" Type="DF" CancelAction="AP" OKAction="F1" OKSteps="0" CancelSteps="0" ComponentAction="1" ServerSide="False" CreateWMTask="False" Core="1" ResumeStep="" Submap="" OKNewRoadmap="" CancelNewRoadmap="" ElementID="E3">
	</STEP>
	<STEP Description="Posting Allocation" Component="bACTAllocationPost.NavigatorV3" Type="BO" CancelAction="AP" OKAction="F1" OKSteps="0" CancelSteps="0" ComponentAction="1" ServerSide="True" CreateWMTask="False" ResumeStep="" Core="1" Submap="" OKNewRoadmap="" CancelNewRoadmap="" ElementID="E4"/>
	<STEP Description="Cash List Post" Component="bACTCashListPost.NavigatorV3" Type="BO" CancelAction="AP" OKAction="F1" OKSteps="0" CancelSteps="0" ComponentAction="1" ServerSide="True" CreateWMTask="False" ResumeStep="" Core="1" Submap="" OKNewRoadmap="" CancelNewRoadmap="" ElementID="E5"/>
</MAP>')
END
GO

IF NOT EXISTS( SELECT 1 fROM pmnavxm_process WHERE file_name = 'ACTIPAYINST.XML')
BEGIN
Declare @pmnavxm_process_id int
Select @pmnavxm_process_id = MAX(pmnavxm_process_id)+1 FROM pmnavxm_process

insert into pmnavxm_process ( pmnavxm_process_id,file_name,file_version_number,file_timestamp,is_custom,is_core,xml_definition)
values
(@pmnavxm_process_id, 'ACTIPAYINST.XML',25,getdate(), 0,1,
'<?xml version="1.0"?>
<!-- edited with XML Spy v4.4 U (http://www.xmlspy.com) by Internal Use (Sirius Financial Solutions Plc) -->
<!-- pmnav_map_id = 18 -->
<!DOCTYPE MAP SYSTEM "navigatorxmv2.dtd">
<MAP WMTaskCode="ACTIPAYINST" WMTaskDescription="Insurer Payment" ImageURL="http://www.siriusgroup.co.uk" TransactionType="" ProcessMode="1" AutoClose="False" NavigatorDriven="True" Title="NavigatorXM" RoadmapName="Insurer Payment" Core="1" Version="28" ElementID="E1">
	<STEP Description="Cash List" Component="iACTCashList.NavigatorV3" Type="FF" CancelAction="AP" OKAction="F1" OKSteps="1" CancelSteps="0" ComponentAction="1" ServerSide="False" CreateWMTask="False" ResumeStep="" Core="1" Submap="" OKNewRoadmap="" CancelNewRoadmap="" ElementID="E2"/>
	<STEP Description="Cash List Item" Component="iACTCashListItem.Interface_Renamed" Type="DF" CancelAction="AP" OKAction="F1" OKSteps="0" CancelSteps="0" ComponentAction="1" ServerSide="False" CreateWMTask="False" Core="1" ResumeStep="" Submap="" OKNewRoadmap="" CancelNewRoadmap="" ElementID="E3">
	</STEP>
</MAP>')
END
GO


-- *****************************************************************************
-- * Author:   Ashish Sachdeva
-- * Date:     13/05/2010
-- * Purpose:  DRE Integration Section 6
-- *****************************************************************************
DECLARE @lCaptionID INT
IF NOT EXISTS (SELECT 1 FROM risk_type_rule_set_type WHERE code = 'SCRIPT')
BEGIN	
	EXECUTE spu_pm_caption_id_return 1, 'SCRIPT', @lCaptionID OUTPUT
	INSERT INTO risk_type_rule_set_type(caption_id, code, description,is_deleted, effective_date)
	VALUES (@lCaptionID, 'SCRIPT', '.Rul file script',0, '1950/01-01')
END
--IF NOT EXISTS (SELECT 1 FROM risk_type_rule_set_type WHERE code = 'DRE')
--BEGIN	
--	EXECUTE spu_pm_caption_id_return 1, 'DRE', @lCaptionID OUTPUT
--	INSERT INTO risk_type_rule_set_type(caption_id, code, description,is_deleted, effective_date)
--	VALUES (@lCaptionID, 'DRE', 'DRE Rating Engine',0, '1950/01-01')
--END
IF EXISTS(SELECT * FROM risk_type_rule_set WHERE risk_type_rule_set_type_id IS NULL)
BEGIN
	DECLARE @risk_type_rule_set_type_id INT
	SELECT @risk_type_rule_set_type_id = risk_type_rule_set_type_id 
	FROM risk_type_rule_set_type 
	WHERE code = 'SCRIPT'

	UPDATE risk_type_rule_set 
	SET risk_type_rule_set_type_id = @risk_type_rule_set_type_id 
	WHERE risk_type_rule_set_type_id IS NULL
END
GO

-- *****************************************************************************
-- * Author:	Sandeep Kumar
-- * Date:		19/10/2012
-- * Purpose:	Other Caption Id
-- *****************************************************************************

 
DECLARE @captionID int
SELECT @captionID=CAPTION_ID FROM MTA_Event_Description WHERE CODE='OTHER'
IF ISNULL(@captionID,'')<>''
BEGIN

IF NOT EXISTS ( SELECT * FROM PMCaption WHERE caption_id=@captionid AND language_id=1)
BEGIN
	INSERT INTO pmcaption VALUES(@captionid,1,'Other')
END

END
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMPolData', 'SAM Policy Import'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMPolData'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMDocData', 'Document Data Import'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMVALBANO', 'SAM Validate Bank Account Number'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMVALBANO'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUPBD', 'SAM Update Party Bank Details'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMUPBD'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMDPBD', 'SAM Delete Party Bank Details'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMDPBD'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMAVPBD', 'SAM Activate Party Bank Details'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMAVPBD'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMDocData'
GO
	INSERT INTO Earning_Pattern_Usage(rating_section_type_id,Earning_Pattern_id,effective_date,is_deleted)
	SELECT rst.rating_section_type_id, 1, '2000-01-01 00:00:00.000', 0
	FROM rating_section_type rst
		Left Join Earning_Pattern_Usage epu ON epu.rating_section_type_id = rst.rating_section_type_id
	WHERE epu.rating_section_type_id IS NULL

GO

-- *****************************************************************************
-- * Author: Kshma Goyal
-- * Date: 20/06/2011
-- * Purpose: PN78619-To add seven new fields to Claim/Payee/Alt Payee tab within the field selection window in document template maintenance.
-- *****************************************************************************

--Adding Address Line 1
IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'CLMAltPayeeAddress1')
BEGIN
    INSERT INTO wp_fields(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, sub_group2)
    VALUES('CLMAltPayeeAddress1', 'spu_wp_ClaimAltPayee', 'PayeeAddress1', 0, 'Claim', 'Payee', 'Address Line 1', 1, NULL, 9,'Alt. Payee')
END
GO
--Adding Address Line 2
IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'CLMAltPayeeAddress2')
BEGIN
    INSERT INTO wp_fields(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, sub_group2)
    VALUES('CLMAltPayeeAddress2', 'spu_wp_ClaimAltPayee', 'PayeeAddress2', 0, 'Claim', 'Payee', 'Address Line 2', 1, NULL, 9,'Alt. Payee')
END
GO
--Adding Address Line 3
IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'CLMAltPayeeAddress3')
BEGIN
    INSERT INTO wp_fields(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, sub_group2)
    VALUES('CLMAltPayeeAddress3', 'spu_wp_ClaimAltPayee', 'PayeeAddress3', 0, 'Claim', 'Payee', 'Address Line 3', 1, NULL, 9,'Alt. Payee')
END
GO
--Adding Address Line 4
IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'CLMAltPayeeAddress4')
BEGIN
    INSERT INTO wp_fields(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, sub_group2)
    VALUES('CLMAltPayeeAddress4', 'spu_wp_ClaimAltPayee', 'PayeeAddress4', 0, 'Claim', 'Payee', 'Address Line 4', 1, NULL, 9,'Alt. Payee')
END
GO
--Adding Postal Code
IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'CLMAltPayeePostalCode')
BEGIN
    INSERT INTO wp_fields(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, sub_group2)
    VALUES('CLMAltPayeePostalCode', 'spu_wp_ClaimAltPayee', 'PayeePostalCode', 0, 'Claim', 'Payee', 'Postal Code', 1, NULL, 9,'Alt. Payee')
END
GO
--Adding Media Ref
IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'CLMAltPayeeMediaRef')
BEGIN
    INSERT INTO wp_fields(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, sub_group2)
    VALUES('CLMAltPayeeMediaRef', 'spu_wp_ClaimAltPayee', 'PayeeMediaRef', 0, 'Claim', 'Payee', 'Media Ref', 1, NULL, 9,'Alt. Payee')
END
GO
--Adding Account Type
IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'CLMAltPayeeAccountType')
BEGIN
    INSERT INTO wp_fields(field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, sub_group2)
    VALUES('CLMAltPayeeAccountType', 'spu_wp_ClaimAltPayee', 'PayeeAccountType', 0, 'Claim', 'Payee', 'Account Type', 1, NULL, 9,'Alt. Payee')
END
GO

-- *****************************************************************************
-- * Author:	Ritu Sharma
-- * Date:		14/02/2014
-- * Purpose:	To Truncate Temporary Tables
-- *****************************************************************************
    
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'report_transaction'
IF @bExists = 1 BEGIN
  TRUNCATE TABLE Report_Transaction  
END
EXECUTE @bExists = DDLExistsTable 'Report_TreePathNames'
IF @bExists = 1 BEGIN
  TRUNCATE TABLE Report_TreePathNames  
END
GO

-- *****************************************************************************
-- * Author:	Vijay Pal
-- * Date:		06/03/2012
-- * Purpose:	Wpr05
-- *****************************************************************************
IF NOT EXISTS (SELECT NULL FROM PMWrk_Task_Group WHERE code = 'CCYCLE')
    BEGIN
     declare @lCaptionID integer
               EXECUTE spu_pm_caption_id_return 1, 'Chase Cycle', @lCaptionID OUTPUT

               INSERT INTO PMWrk_Task_Group 
                        (caption_id, code, description, is_deleted, effective_date, 
                       display_icon ) 

               Values (@lCaptionID, 'CCYCLE', 'Chase Cycle', 0, 
                          GETDATE(),5)
               
    END
    
IF NOT EXISTS (SELECT NULL FROM PMWrk_Task WHERE code = 'CHCYMAINT')
    BEGIN
   
               EXECUTE spu_pm_caption_id_return 1, 'Chase Cycle Maintenance', @lCaptionID OUTPUT

               INSERT INTO PMWrk_Task 
                        (caption_id, code, description, is_deleted, effective_date, 
                        is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, 
                        auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, 
                        linked_caption_id, is_available_task, pmwrk_task_category_id) 

               Values (@lCaptionID, 'CHCYMAINT', 'Chase Cycle Maintenance', 0, 
                          GETDATE(), 0, 1, NULL, 'iPMUChaseCycleMaint', 'NavigatorV3', 0, 42, 0, NULL, NULL, NULL, 1, 2)
               
    END



IF NOT EXISTS (SELECT NULL FROM PMWrk_Task WHERE code = 'CHCYPROC')
    BEGIN
     
               EXECUTE spu_pm_caption_id_return 1, 'Chase Cycle Processing', @lCaptionID OUTPUT

               INSERT INTO PMWrk_Task 
                        (caption_id, code, description, is_deleted, effective_date, 
                        is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, 
                        auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, 
                        linked_caption_id, is_available_task, pmwrk_task_category_id) 

               Values (@lCaptionID, 'CHCYPROC', 'Chase Cycle Processing', 0, 
                          GETDATE(), 0, 1, NULL, 'iPMUChaseCycleProcessing', 'NavigatorV3', 0, 42, 0, NULL, NULL, NULL, 1, 2)
               
    END
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'CCYCLE', 'CHCYMAINT'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'CCYCLE', 'CHCYPROC'
GO

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'CCYWillAutoCancel')
BEGIN
    INSERT INTO wp_fields(field_name, sql, column_name, column_type, main_group, sub_group, display_name,
     is_displayed, loop1, product_family)
    VALUES('CCYWillAutoCancel', 'spu_wp_ChaseCycleItem', 'will_auto_cancel', 0, 'Policy', 'Chase Cycle Item',
     'Will Auto Cancel', 1, 'chasecycleitem', 9)
END

GO
    
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMACMQ', 'SAM Cancel MTA Quote'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMACMQ'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGETPF', 'Get Header And Summaries PFPlanByKey'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGETPF'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUPDPF', 'SAM_Update_Premium_Finance_Instalment'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMUPDPF'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUPDIS', 'SAM_Update_Premium_Finance_Instalment'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMUPDIS'
GO

update Process_Types_Docs set allow_filtering=1 where process_types_docs_id in (3,8,9,10,11,12,13)

GO
    
-- *****************************************************************************
-- * Author:        Goldy Saini
-- * Date:          04/03/2014
-- * Purpose:       Update the default column (INS 07)
-- *****************************************************************************
	-- UPDATE Write_Off_Reason 
	-- SET Is_Only_Valid_for_Instalment = ISNULL(Is_Only_Valid_for_Instalment,0)

-- GO
-- *****************************************************************************
-- * Author:  Goldy Saini
-- * Date:    05 Mar 2014 
-- * Purpose: Tech Spec QBECALINS06
-- *****************************************************************************
DECLARE @caption_id INT
DECLARE @pfinstalments_result_id INT
IF NOT EXISTS(SELECT null FROM PFInstalments_Result WHERE code = '_REV')
BEGIN
    EXECUTE spu_pm_caption_id_return 1, 'Reversed', @caption_id OUTPUT

	SELECT @pfinstalments_result_id = ISNULL(MAX(pfinstalments_result_id),0)+1 FROM PFInstalments_Result

    INSERT INTO PFInstalments_Result
    (pfinstalments_result_id,code, description,caption_id , effective_date,is_deleted)
    values(@pfinstalments_result_id,'_REV', 'Reversed', @caption_id,getdate(),0)
END
GO
-- *****************************************************************************

-- *****************************************************************************  
-- * Author:      Ashish Sachdeva
-- * Date:        20 Dec 2013
-- * Purpose:     RND009 - ClaimPaymentProcessing SettleAll in POrtal
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMSETALL', 'Settle All Claim Payments'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMSETALL'
GO

 IF NOT EXISTS ( SELECT NULL FROM Insurance_File_Type WHERE code='MTAQCAN')
  BEGIN
    DECLARE @MTAQCANcaption_id INT
    DECLARE @Insurance_File_Type_id INT

    SELECT @Insurance_File_Type_id = MAX(ISNULL(Insurance_File_Type_id,0))+1 
    FROM Insurance_File_Type

    EXECUTE spu_pm_caption_id_return 1, 'MTA Quotation Cancellation', @MTAQCANcaption_id OUTPUT

    INSERT INTO Insurance_File_Type
    (Insurance_File_Type_id,caption_id,code,description,var_data_structure_id,is_deleted,effective_date)
    VALUES
    (@Insurance_File_Type_id,@MTAQCANcaption_id,'MTAQCAN','MTA Quotation Cancellation',NULL,0,GETDATE())
  END
  
  GO
  
  -- *****************************************************************************
-- * Author:        Vijay pal
-- * Date:          15/05/2014
-- * Purpose:       QBECALINS16-18 - Instalment and Credit Control Merge
-- *****************************************************************************
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BillingAddressLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressLine1','spu_wp_addresses','Billing_Address_Address1','0','Address - Party','Billing Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BillingAddressLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressLine2','spu_wp_addresses','Billing_Address_Address2','0','Address - Party','Billing Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BillingAddressLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressLine3','spu_wp_addresses','Billing_Address_Address3','0','Address - Party','Billing Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BillingAddressLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressLine4','spu_wp_addresses','Billing_Address_Address4','0','Address - Party','Billing Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BillingPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingPostcode','spu_wp_addresses','Billing_Address_Postcode','0','Address - Party','Billing Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BranchAddressCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressCountry','spu_wp_addresses','Branch_Address_Country','0','Address - Party','Branch Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BranchAddressLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressLine1','spu_wp_addresses','Branch_Address_Address1','0','Address - Party','Branch Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BranchAddressLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressLine2','spu_wp_addresses','Branch_Address_Address2','0','Address - Party','Branch Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BranchAddressLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressLine3','spu_wp_addresses','Branch_Address_Address3','0','Address - Party','Branch Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BranchAddressLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressLine4','spu_wp_addresses','Branch_Address_Address4','0','Address - Party','Branch Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BranchPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchPostcode','spu_wp_addresses','Branch_Address_Postcode','0','Address - Party','Branch Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BrokerAddressCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressCountry','spu_wp_addresses','Broker_Address_Country','0','Address - Party','Broker Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BrokerAddressLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressLine1','spu_wp_addresses','Broker_Address_Address1','0','Address - Party','Broker Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BrokerAddressLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressLine2','spu_wp_addresses','Broker_Address_Address2','0','Address - Party','Broker Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BrokerAddressLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressLine3','spu_wp_addresses','Broker_Address_Address3','0','Address - Party','Broker Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BrokerAddressLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressLine4','spu_wp_addresses','Broker_Address_Address4','0','Address - Party','Broker Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BrokerPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerPostcode','spu_wp_addresses','Broker_Address_Postcode','0','Address - Party','Broker Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BusinessAddressCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressCountry','spu_wp_addresses','Business_Address_Country','0','Address - Party','Business Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BusinessAddressLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressLine1','spu_wp_addresses','Business_Address_Address1','0','Address - Party','Business Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BusinessAddressLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressLine2','spu_wp_addresses','Business_Address_Address2','0','Address - Party','Business Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BusinessAddressLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressLine3','spu_wp_addresses','Business_Address_Address3','0','Address - Party','Business Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BusinessAddressLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressLine4','spu_wp_addresses','Business_Address_Address4','0','Address - Party','Business Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='BusinessPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessPostcode','spu_wp_addresses','Business_Address_Postcode','0','Address - Party','Business Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='EmailAddressCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressCountry','spu_wp_addresses','Email_Address_Country','0','Address - Party','Email Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='EmailAddressLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressLine1','spu_wp_addresses','Email_Address_Address1','0','Address - Party','Email Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='EmailAddressLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressLine2','spu_wp_addresses','Email_Address_Address2','0','Address - Party','Email Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='EmailAddressLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressLine3','spu_wp_addresses','Email_Address_Address3','0','Address - Party','Email Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='EmailAddressLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressLine4','spu_wp_addresses','Email_Address_Address4','0','Address - Party','Email Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='EmailPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailPostcode','spu_wp_addresses','Email_Address_Postcode','0','Address - Party','Email Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='HomeAddressCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressCountry','spu_wp_addresses','Home_Address_Country','0','Address - Party','Home Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='HomeAddressLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressLine1','spu_wp_addresses','Home_Address_Address1','0','Address - Party','Home Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='HomeAddressLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressLine2','spu_wp_addresses','Home_Address_Address2','0','Address - Party','Home Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='HomeAddressLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressLine3','spu_wp_addresses','Home_Address_Address3','0','Address - Party','Home Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='HomeAddressLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressLine4','spu_wp_addresses','Home_Address_Address4','0','Address - Party','Home Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='HomePostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomePostcode','spu_wp_addresses','Home_Address_Postcode','0','Address - Party','Home Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='OtherCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherCountry','spu_wp_addresses','Other_Country','0','Address - Party','Other','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='OtherLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherLine1','spu_wp_addresses','Other_Address1','0','Address - Party','Other','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='OtherLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherLine2','spu_wp_addresses','Other_Address2','0','Address - Party','Other','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='OtherLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherLine3','spu_wp_addresses','Other_Address3','0','Address - Party','Other','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='OtherLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherLine4','spu_wp_addresses','Other_Address4','0','Address - Party','Other','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='OtherPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherPostcode','spu_wp_addresses','Other_Postcode','0','Address - Party','Other','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='PreviousAddressCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressCountry','spu_wp_addresses','Previous_Address_Country','0','Address - Party','Previous Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='PreviousAddressLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressLine1','spu_wp_addresses','Previous_Address_Address1','0','Address - Party','Previous Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='PreviousAddressLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressLine2','spu_wp_addresses','Previous_Address_Address2','0','Address - Party','Previous Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='PreviousAddressLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressLine3','spu_wp_addresses','Previous_Address_Address3','0','Address - Party','Previous Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='PreviousAddressLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressLine4','spu_wp_addresses','Previous_Address_Address4','0','Address - Party','Previous Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='PreviousPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousPostcode','spu_wp_addresses','Previous_Address_Postcode','0','Address - Party','Previous Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='RegisteredAddressCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressCountry','spu_wp_addresses','Registered_Address_Country','0','Address - Party','Registered Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='RegisteredAddressLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressLine1','spu_wp_addresses','Registered_Address_Address1','0','Address - Party','Registered Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='RegisteredAddressLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressLine2','spu_wp_addresses','Registered_Address_Address2','0','Address - Party','Registered Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='RegisteredAddressLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressLine3','spu_wp_addresses','Registered_Address_Address3','0','Address - Party','Registered Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='RegisteredAddressLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressLine4','spu_wp_addresses','Registered_Address_Address4','0','Address - Party','Registered Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='RegisteredPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredPostcode','spu_wp_addresses','Registered_Address_Postcode','0','Address - Party','Registered Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='SiteAddressCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressCountry','spu_wp_addresses','Site_Address_Country','0','Address - Party','Site Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='SiteAddressLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressLine1','spu_wp_addresses','Site_Address_Address1','0','Address - Party','Site Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='SiteAddressLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressLine2','spu_wp_addresses','Site_Address_Address2','0','Address - Party','Site Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='SiteAddressLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressLine3','spu_wp_addresses','Site_Address_Address3','0','Address - Party','Site Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='SiteAddressLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressLine4','spu_wp_addresses','Site_Address_Address4','0','Address - Party','Site Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='SitePostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SitePostcode','spu_wp_addresses','Site_Address_Postcode','0','Address - Party','Site Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='SubAgentCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentCountry','spu_wp_addresses','Sub_Agent_Country','0','Address - Party','Sub Agent','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='SubAgentLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentLine1','spu_wp_addresses','Sub_Agent_Address1','0','Address - Party','Sub Agent','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='SubAgentLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentLine2','spu_wp_addresses','Sub_Agent_Address2','0','Address - Party','Sub Agent','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='SubAgentLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentLine3','spu_wp_addresses','Sub_Agent_Address3','0','Address - Party','Sub Agent','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='SubAgentLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentLine4','spu_wp_addresses','Sub_Agent_Address4','0','Address - Party','Sub Agent','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_addresses' AND Field_name='SubAgentPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentPostcode','spu_wp_addresses','Sub_Agent_Postcode','0','Address - Party','Sub Agent','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BillingAddressAgentCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressAgentCountry','spu_wp_agent_addresses','Billing_Address_Country','0','Address - Agent','Billing Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_Agent_addresses' AND Field_name='BillingAddressAgentLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressAgentLine1','spu_wp_Agent_addresses','Billing_Address_Address1','0','Address - Agent','Billing Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BillingAddressAgentLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressAgentLine2','spu_wp_agent_addresses','Billing_Address_Address2','0','Address - Agent','Billing Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BillingAddressAgentLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressAgentLine3','spu_wp_agent_addresses','Billing_Address_Address3','0','Address - Agent','Billing Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BillingAddressAgentLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressAgentLine4','spu_wp_agent_addresses','Billing_Address_Address4','0','Address - Agent','Billing Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BillingAgentPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAgentPostcode','spu_wp_agent_addresses','Billing_Address_Postcode','0','Address - Agent','Billing Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BranchAddressAgentCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressAgentCountry','spu_wp_agent_addresses','Branch_Address_Country','0','Address - Agent','Branch Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_Agent_addresses' AND Field_name='BranchAddressAgentLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressAgentLine1','spu_wp_Agent_addresses','Branch_Address_Address1','0','Address - Agent','Branch Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BranchAddressAgentLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressAgentLine2','spu_wp_agent_addresses','Branch_Address_Address2','0','Address - Agent','Branch Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BranchAddressAgentLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressAgentLine3','spu_wp_agent_addresses','Branch_Address_Address3','0','Address - Agent','Branch Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BranchAddressAgentLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressAgentLine4','spu_wp_agent_addresses','Branch_Address_Address4','0','Address - Agent','Branch Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BranchAgentPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAgentPostcode','spu_wp_agent_addresses','Branch_Address_Postcode','0','Address - Agent','Branch Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BrokerAddressAgentCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressAgentCountry','spu_wp_agent_addresses','Broker_Address_Country','0','Address - Agent','Broker Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_Agent_addresses' AND Field_name='BrokerAddressAgentLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressAgentLine1','spu_wp_Agent_addresses','Broker_Address_Address1','0','Address - Agent','Broker Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BrokerAddressAgentLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressAgentLine2','spu_wp_agent_addresses','Broker_Address_Address2','0','Address - Agent','Broker Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BrokerAddressAgentLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressAgentLine3','spu_wp_agent_addresses','Broker_Address_Address3','0','Address - Agent','Broker Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BrokerAddressAgentLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressAgentLine4','spu_wp_agent_addresses','Broker_Address_Address4','0','Address - Agent','Broker Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BrokerAgentPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAgentPostcode','spu_wp_agent_addresses','Broker_Address_Postcode','0','Address - Agent','Broker Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BusinessAddressAgentCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressAgentCountry','spu_wp_agent_addresses','Business_Address_Country','0','Address - Agent','Business Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_Agent_addresses' AND Field_name='BusinessAddressAgentLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressAgentLine1','spu_wp_Agent_addresses','Business_Address_Address1','0','Address - Agent','Business Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BusinessAddressAgentLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressAgentLine2','spu_wp_agent_addresses','Business_Address_Address2','0','Address - Agent','Business Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BusinessAddressAgentLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressAgentLine3','spu_wp_agent_addresses','Business_Address_Address3','0','Address - Agent','Business Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BusinessAddressAgentLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressAgentLine4','spu_wp_agent_addresses','Business_Address_Address4','0','Address - Agent','Business Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='BusinessAgentPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAgentPostcode','spu_wp_agent_addresses','Business_Address_Postcode','0','Address - Agent','Business Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='EmailAddressAgentCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressAgentCountry','spu_wp_agent_addresses','Email_Address_Country','0','Address - Agent','Email Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_Agent_addresses' AND Field_name='EmailAddressAgentLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressAgentLine1','spu_wp_Agent_addresses','Email_Address_Address1','0','Address - Agent','Email Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='EmailAddressAgentLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressAgentLine2','spu_wp_agent_addresses','Email_Address_Address2','0','Address - Agent','Email Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='EmailAddressAgentLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressAgentLine3','spu_wp_agent_addresses','Email_Address_Address3','0','Address - Agent','Email Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='EmailAddressAgentLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressAgentLine4','spu_wp_agent_addresses','Email_Address_Address4','0','Address - Agent','Email Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='EmailAgentPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAgentPostcode','spu_wp_agent_addresses','Email_Address_Postcode','0','Address - Agent','Email Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='HomeAddressAgentCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressAgentCountry','spu_wp_agent_addresses','Home_Address_Country','0','Address - Agent','Home Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_Agent_addresses' AND Field_name='HomeAddressAgentLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressAgentLine1','spu_wp_Agent_addresses','Home_Address_Address1','0','Address - Agent','Home Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='HomeAddressAgentLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressAgentLine2','spu_wp_agent_addresses','Home_Address_Address2','0','Address - Agent','Home Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='HomeAddressAgentLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressAgentLine3','spu_wp_agent_addresses','Home_Address_Address3','0','Address - Agent','Home Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='HomeAddressAgentLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressAgentLine4','spu_wp_agent_addresses','Home_Address_Address4','0','Address - Agent','Home Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='HomeAgentPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAgentPostcode','spu_wp_agent_addresses','Home_Address_Postcode','0','Address - Agent','Home Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='OtherAgentCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherAgentCountry','spu_wp_agent_addresses','Other_Country','0','Address - Agent','Other','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_Agent_addresses' AND Field_name='OtherAgentLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherAgentLine1','spu_wp_Agent_addresses','Other_Address1','0','Address - Agent','Other','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='OtherAgentLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherAgentLine2','spu_wp_agent_addresses','Other_Address2','0','Address - Agent','Other','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='OtherAgentLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherAgentLine3','spu_wp_agent_addresses','Other_Address3','0','Address - Agent','Other','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='OtherAgentLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherAgentLine4','spu_wp_agent_addresses','Other_Address4','0','Address - Agent','Other','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='OtherAgentPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherAgentPostcode','spu_wp_agent_addresses','Other_Postcode','0','Address - Agent','Other','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='PreviousAddressAgentCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressAgentCountry','spu_wp_agent_addresses','Previous_Address_Country','0','Address - Agent','Previous Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_Agent_addresses' AND Field_name='PreviousAddressAgentLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressAgentLine1','spu_wp_Agent_addresses','Previous_Address_Address1','0','Address - Agent','Previous Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='PreviousAddressAgentLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressAgentLine2','spu_wp_agent_addresses','Previous_Address_Address2','0','Address - Agent','Previous Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='PreviousAddressAgentLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressAgentLine3','spu_wp_agent_addresses','Previous_Address_Address3','0','Address - Agent','Previous Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='PreviousAddressAgentLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressAgentLine4','spu_wp_agent_addresses','Previous_Address_Address4','0','Address - Agent','Previous Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='PreviousAgentPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAgentPostcode','spu_wp_agent_addresses','Previous_Address_Postcode','0','Address - Agent','Previous Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='RegisteredAddressAgentCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressAgentCountry','spu_wp_agent_addresses','Registered_Address_Country','0','Address - Agent','Registered Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_Agent_addresses' AND Field_name='RegisteredAddressAgentLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressAgentLine1','spu_wp_Agent_addresses','Registered_Address_Address1','0','Address - Agent','Registered Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='RegisteredAddressAgentLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressAgentLine2','spu_wp_agent_addresses','Registered_Address_Address2','0','Address - Agent','Registered Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='RegisteredAddressAgentLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressAgentLine3','spu_wp_agent_addresses','Registered_Address_Address3','0','Address - Agent','Registered Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='RegisteredAddressAgentLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressAgentLine4','spu_wp_agent_addresses','Registered_Address_Address4','0','Address - Agent','Registered Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='RegisteredAgentPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAgentPostcode','spu_wp_agent_addresses','Registered_Address_Postcode','0','Address - Agent','Registered Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='SiteAddressAgentCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressAgentCountry','spu_wp_agent_addresses','Site_Address_Country','0','Address - Agent','Site Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_Agent_addresses' AND Field_name='SiteAddressAgentLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressAgentLine1','spu_wp_Agent_addresses','Site_Address_Address1','0','Address - Agent','Site Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='SiteAddressAgentLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressAgentLine2','spu_wp_agent_addresses','Site_Address_Address2','0','Address - Agent','Site Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='SiteAddressAgentLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressAgentLine3','spu_wp_agent_addresses','Site_Address_Address3','0','Address - Agent','Site Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='SiteAddressAgentLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressAgentLine4','spu_wp_agent_addresses','Site_Address_Address4','0','Address - Agent','Site Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='SiteAgentPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAgentPostcode','spu_wp_agent_addresses','Site_Address_Postcode','0','Address - Agent','Site Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='SubAgentAgentCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentAgentCountry','spu_wp_agent_addresses','Sub_Agent_Country','0','Address - Agent','Sub Agent','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_Agent_addresses' AND Field_name='SubAgentAgentLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentAgentLine1','spu_wp_Agent_addresses','Sub_Agent_Address1','0','Address - Agent','Sub Agent','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='SubAgentAgentLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentAgentLine2','spu_wp_agent_addresses','Sub_Agent_Address2','0','Address - Agent','Sub Agent','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='SubAgentAgentLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentAgentLine3','spu_wp_agent_addresses','Sub_Agent_Address3','0','Address - Agent','Sub Agent','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='SubAgentAgentLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentAgentLine4','spu_wp_agent_addresses','Sub_Agent_Address4','0','Address - Agent','Sub Agent','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_agent_addresses' AND Field_name='SubAgentAgentPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentAgentPostcode','spu_wp_agent_addresses','Sub_Agent_Postcode','0','Address - Agent','Sub Agent','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_GetUserValues' AND Field_name='USRSIGNATUREFILE')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('USRSIGNATUREFILE','spu_wp_GetUserValues','USRSIGNATUREFILE','0','System','Logic','User Signature','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BillingAddressInsCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressInsCountry','spu_wp_insurer_addresses','Billing_Address_Country','0','Address - Insurer','Billing Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BillingAddressInsLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressInsLine1','spu_wp_insurer_addresses','Billing_Address_Address1','0','Address - Insurer','Billing Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BillingAddressInsLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressInsLine2','spu_wp_insurer_addresses','Billing_Address_Address2','0','Address - Insurer','Billing Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BillingAddressInsLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressInsLine3','spu_wp_insurer_addresses','Billing_Address_Address3','0','Address - Insurer','Billing Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BillingAddressInsLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressInsLine4','spu_wp_insurer_addresses','Billing_Address_Address4','0','Address - Insurer','Billing Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BillingInsPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingInsPostcode','spu_wp_insurer_addresses','Billing_Address_Postcode','0','Address - Insurer','Billing Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BranchAddressInsCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressInsCountry','spu_wp_insurer_addresses','Branch_Address_Country','0','Address - Insurer','Branch Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BranchAddressInsLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressInsLine1','spu_wp_insurer_addresses','Branch_Address_Address1','0','Address - Insurer','Branch Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BranchAddressInsLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressInsLine2','spu_wp_insurer_addresses','Branch_Address_Address2','0','Address - Insurer','Branch Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BranchAddressInsLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressInsLine3','spu_wp_insurer_addresses','Branch_Address_Address3','0','Address - Insurer','Branch Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BranchAddressInsLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressInsLine4','spu_wp_insurer_addresses','Branch_Address_Address4','0','Address - Insurer','Branch Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BranchInsPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchInsPostcode','spu_wp_insurer_addresses','Branch_Address_Postcode','0','Address - Insurer','Branch Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BrokerAddressInsCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressInsCountry','spu_wp_insurer_addresses','Broker_Address_Country','0','Address - Insurer','Broker Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BrokerAddressInsLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressInsLine1','spu_wp_insurer_addresses','Broker_Address_Address1','0','Address - Insurer','Broker Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BrokerAddressInsLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressInsLine2','spu_wp_insurer_addresses','Broker_Address_Address2','0','Address - Insurer','Broker Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BrokerAddressInsLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressInsLine3','spu_wp_insurer_addresses','Broker_Address_Address3','0','Address - Insurer','Broker Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BrokerAddressInsLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressInsLine4','spu_wp_insurer_addresses','Broker_Address_Address4','0','Address - Insurer','Broker Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BrokerInsPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerInsPostcode','spu_wp_insurer_addresses','Broker_Address_Postcode','0','Address - Insurer','Broker Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BusinessAddressInsCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressInsCountry','spu_wp_insurer_addresses','Business_Address_Country','0','Address - Insurer','Business Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BusinessAddressInsLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressInsLine1','spu_wp_insurer_addresses','Business_Address_Address1','0','Address - Insurer','Business Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BusinessAddressInsLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressInsLine2','spu_wp_insurer_addresses','Business_Address_Address2','0','Address - Insurer','Business Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BusinessAddressInsLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressInsLine3','spu_wp_insurer_addresses','Business_Address_Address3','0','Address - Insurer','Business Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BusinessAddressInsLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressInsLine4','spu_wp_insurer_addresses','Business_Address_Address4','0','Address - Insurer','Business Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='BusinessInsPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessInsPostcode','spu_wp_insurer_addresses','Business_Address_Postcode','0','Address - Insurer','Business Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='EmailAddressInsCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressInsCountry','spu_wp_insurer_addresses','Email_Address_Country','0','Address - Insurer','Email Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='EmailAddressInsLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressInsLine1','spu_wp_insurer_addresses','Email_Address_Address1','0','Address - Insurer','Email Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='EmailAddressInsLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressInsLine2','spu_wp_insurer_addresses','Email_Address_Address2','0','Address - Insurer','Email Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='EmailAddressInsLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressInsLine3','spu_wp_insurer_addresses','Email_Address_Address3','0','Address - Insurer','Email Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='EmailAddressInsLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressInsLine4','spu_wp_insurer_addresses','Email_Address_Address4','0','Address - Insurer','Email Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='EmailInsPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailInsPostcode','spu_wp_insurer_addresses','Email_Address_Postcode','0','Address - Insurer','Email Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='HomeAddressInsCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressInsCountry','spu_wp_insurer_addresses','Home_Address_Country','0','Address - Insurer','Home Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='HomeAddressInsLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressInsLine1','spu_wp_insurer_addresses','Home_Address_Address1','0','Address - Insurer','Home Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='HomeAddressInsLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressInsLine2','spu_wp_insurer_addresses','Home_Address_Address2','0','Address - Insurer','Home Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='HomeAddressInsLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressInsLine3','spu_wp_insurer_addresses','Home_Address_Address3','0','Address - Insurer','Home Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='HomeAddressInsLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressInsLine4','spu_wp_insurer_addresses','Home_Address_Address4','0','Address - Insurer','Home Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='HomeInsPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeInsPostcode','spu_wp_insurer_addresses','Home_Address_Postcode','0','Address - Insurer','Home Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='OtherInsCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherInsCountry','spu_wp_insurer_addresses','Other_Country','0','Address - Insurer','Other','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='OtherInsLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherInsLine1','spu_wp_insurer_addresses','Other_Address1','0','Address - Insurer','Other','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='OtherInsLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherInsLine2','spu_wp_insurer_addresses','Other_Address2','0','Address - Insurer','Other','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='OtherInsLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherInsLine3','spu_wp_insurer_addresses','Other_Address3','0','Address - Insurer','Other','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='OtherInsLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherInsLine4','spu_wp_insurer_addresses','Other_Address4','0','Address - Insurer','Other','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='OtherInsPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherInsPostcode','spu_wp_insurer_addresses','Other_Postcode','0','Address - Insurer','Other','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='PreviousAddressInsCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressInsCountry','spu_wp_insurer_addresses','Previous_Address_Country','0','Address - Insurer','Previous Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='PreviousAddressInsLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressInsLine1','spu_wp_insurer_addresses','Previous_Address_Address1','0','Address - Insurer','Previous Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='PreviousAddressInsLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressInsLine2','spu_wp_insurer_addresses','Previous_Address_Address2','0','Address - Insurer','Previous Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='PreviousAddressInsLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressInsLine3','spu_wp_insurer_addresses','Previous_Address_Address3','0','Address - Insurer','Previous Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='PreviousAddressInsLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressInsLine4','spu_wp_insurer_addresses','Previous_Address_Address4','0','Address - Insurer','Previous Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='PreviousInsPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousInsPostcode','spu_wp_insurer_addresses','Previous_Address_Postcode','0','Address - Insurer','Previous Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='RegisteredAddressInsCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressInsCountry','spu_wp_insurer_addresses','Registered_Address_Country','0','Address - Insurer','Registered Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='RegisteredAddressInsLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressInsLine1','spu_wp_insurer_addresses','Registered_Address_Address1','0','Address - Insurer','Registered Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='RegisteredAddressInsLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressInsLine2','spu_wp_insurer_addresses','Registered_Address_Address2','0','Address - Insurer','Registered Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='RegisteredAddressInsLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressInsLine3','spu_wp_insurer_addresses','Registered_Address_Address3','0','Address - Insurer','Registered Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='RegisteredAddressInsLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressInsLine4','spu_wp_insurer_addresses','Registered_Address_Address4','0','Address - Insurer','Registered Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='RegisteredInsPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredInsPostcode','spu_wp_insurer_addresses','Registered_Address_Postcode','0','Address - Insurer','Registered Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='SiteAddressInsCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressInsCountry','spu_wp_insurer_addresses','Site_Address_Country','0','Address - Insurer','Site Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='SiteAddressInsLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressInsLine1','spu_wp_insurer_addresses','Site_Address_Address1','0','Address - Insurer','Site Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='SiteAddressInsLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressInsLine2','spu_wp_insurer_addresses','Site_Address_Address2','0','Address - Insurer','Site Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='SiteAddressInsLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressInsLine3','spu_wp_insurer_addresses','Site_Address_Address3','0','Address - Insurer','Site Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='SiteAddressInsLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressInsLine4','spu_wp_insurer_addresses','Site_Address_Address4','0','Address - Insurer','Site Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='SiteInsPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteInsPostcode','spu_wp_insurer_addresses','Site_Address_Postcode','0','Address - Insurer','Site Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='SubAgentInsCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentInsCountry','spu_wp_insurer_addresses','Sub_Agent_Country','0','Address - Insurer','Sub Agent','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='SubAgentInsLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentInsLine1','spu_wp_insurer_addresses','Sub_Agent_Address1','0','Address - Insurer','Sub Agent','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='SubAgentInsLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentInsLine2','spu_wp_insurer_addresses','Sub_Agent_Address2','0','Address - Insurer','Sub Agent','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='SubAgentInsLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentInsLine3','spu_wp_insurer_addresses','Sub_Agent_Address3','0','Address - Insurer','Sub Agent','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='SubAgentInsLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentInsLine4','spu_wp_insurer_addresses','Sub_Agent_Address4','0','Address - Insurer','Sub Agent','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_insurer_addresses' AND Field_name='SubAgentInsPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentInsPostcode','spu_wp_insurer_addresses','Sub_Agent_Postcode','0','Address - Insurer','Sub Agent','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_partybankdetails' AND Field_name='PartyBankName')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PartyBankName','spu_wp_partybankdetails','Bank_Name','0','Party','BankDetails','Bank Name','1','partybankdetails','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_PFPremiumFinancePolicies' AND Field_name='INSSchemeType')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('INSSchemeType','spu_wp_PFPremiumFinancePolicies','Scheme_Type','0','Instalment','Policies','Scheme Type','1','PFPremiumFinancePolicies','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BillingAddressUndCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressUndCountry','spu_wp_underwriter_addresses','Billing_Address_Country','0','Addresses -  FSA Underwriter','Billing Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BillingAddressUndLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressUndLine1','spu_wp_underwriter_addresses','Billing_Address_Address1','0','Addresses -  FSA Underwriter','Billing Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BillingAddressUndLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressUndLine2','spu_wp_underwriter_addresses','Billing_Address_Address2','0','Addresses -  FSA Underwriter','Billing Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BillingAddressUndLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressUndLine3','spu_wp_underwriter_addresses','Billing_Address_Address3','0','Addresses -  FSA Underwriter','Billing Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BillingAddressUndLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingAddressUndLine4','spu_wp_underwriter_addresses','Billing_Address_Address4','0','Addresses -  FSA Underwriter','Billing Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BillingUndPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BillingUndPostcode','spu_wp_underwriter_addresses','Billing_Address_Postcode','0','Addresses -  FSA Underwriter','Billing Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BranchAddressUndCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressUndCountry','spu_wp_underwriter_addresses','Branch_Address_Country','0','Addresses -  FSA Underwriter','Branch Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BranchAddressUndLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressUndLine1','spu_wp_underwriter_addresses','Branch_Address_Address1','0','Addresses -  FSA Underwriter','Branch Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BranchAddressUndLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressUndLine2','spu_wp_underwriter_addresses','Branch_Address_Address2','0','Addresses -  FSA Underwriter','Branch Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BranchAddressUndLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressUndLine3','spu_wp_underwriter_addresses','Branch_Address_Address3','0','Addresses -  FSA Underwriter','Branch Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BranchAddressUndLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchAddressUndLine4','spu_wp_underwriter_addresses','Branch_Address_Address4','0','Addresses -  FSA Underwriter','Branch Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BranchUndPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BranchUndPostcode','spu_wp_underwriter_addresses','Branch_Address_Postcode','0','Addresses -  FSA Underwriter','Branch Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BrokerAddressUndCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressUndCountry','spu_wp_underwriter_addresses','Broker_Address_Country','0','Addresses -  FSA Underwriter','Broker Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BrokerAddressUndLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressUndLine1','spu_wp_underwriter_addresses','Broker_Address_Address1','0','Addresses -  FSA Underwriter','Broker Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BrokerAddressUndLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressUndLine2','spu_wp_underwriter_addresses','Broker_Address_Address2','0','Addresses -  FSA Underwriter','Broker Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BrokerAddressUndLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressUndLine3','spu_wp_underwriter_addresses','Broker_Address_Address3','0','Addresses -  FSA Underwriter','Broker Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BrokerAddressUndLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerAddressUndLine4','spu_wp_underwriter_addresses','Broker_Address_Address4','0','Addresses -  FSA Underwriter','Broker Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BrokerUndPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BrokerUndPostcode','spu_wp_underwriter_addresses','Broker_Address_Postcode','0','Addresses -  FSA Underwriter','Broker Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BusinessAddressUndCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressUndCountry','spu_wp_underwriter_addresses','Business_Address_Country','0','Addresses -  FSA Underwriter','Business Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BusinessAddressUndLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressUndLine1','spu_wp_underwriter_addresses','Business_Address_Address1','0','Addresses -  FSA Underwriter','Business Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BusinessAddressUndLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressUndLine2','spu_wp_underwriter_addresses','Business_Address_Address2','0','Addresses -  FSA Underwriter','Business Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BusinessAddressUndLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressUndLine3','spu_wp_underwriter_addresses','Business_Address_Address3','0','Addresses -  FSA Underwriter','Business Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BusinessAddressUndLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessAddressUndLine4','spu_wp_underwriter_addresses','Business_Address_Address4','0','Addresses -  FSA Underwriter','Business Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='BusinessUndPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('BusinessUndPostcode','spu_wp_underwriter_addresses','Business_Address_Postcode','0','Addresses -  FSA Underwriter','Business Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='EmailAddressUndCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressUndCountry','spu_wp_underwriter_addresses','Email_Address_Country','0','Addresses -  FSA Underwriter','Email Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='EmailAddressUndLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressUndLine1','spu_wp_underwriter_addresses','Email_Address_Address1','0','Addresses -  FSA Underwriter','Email Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='EmailAddressUndLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressUndLine2','spu_wp_underwriter_addresses','Email_Address_Address2','0','Addresses -  FSA Underwriter','Email Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='EmailAddressUndLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressUndLine3','spu_wp_underwriter_addresses','Email_Address_Address3','0','Addresses -  FSA Underwriter','Email Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='EmailAddressUndLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailAddressUndLine4','spu_wp_underwriter_addresses','Email_Address_Address4','0','Addresses -  FSA Underwriter','Email Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='EmailUndPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('EmailUndPostcode','spu_wp_underwriter_addresses','Email_Address_Postcode','0','Addresses -  FSA Underwriter','Email Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='HomeAddressUndCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressUndCountry','spu_wp_underwriter_addresses','Home_Address_Country','0','Addresses -  FSA Underwriter','Home Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='HomeAddressUndLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressUndLine1','spu_wp_underwriter_addresses','Home_Address_Address1','0','Addresses -  FSA Underwriter','Home Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='HomeAddressUndLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressUndLine2','spu_wp_underwriter_addresses','Home_Address_Address2','0','Addresses -  FSA Underwriter','Home Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='HomeAddressUndLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressUndLine3','spu_wp_underwriter_addresses','Home_Address_Address3','0','Addresses -  FSA Underwriter','Home Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='HomeAddressUndLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeAddressUndLine4','spu_wp_underwriter_addresses','Home_Address_Address4','0','Addresses -  FSA Underwriter','Home Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='HomeUndPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('HomeUndPostcode','spu_wp_underwriter_addresses','Home_Address_Postcode','0','Addresses -  FSA Underwriter','Home Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='OtherUndCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherUndCountry','spu_wp_underwriter_addresses','Other_Country','0','Addresses -  FSA Underwriter','Other','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='OtherUndLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherUndLine1','spu_wp_underwriter_addresses','Other_Address1','0','Addresses -  FSA Underwriter','Other','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='OtherUndLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherUndLine2','spu_wp_underwriter_addresses','Other_Address2','0','Addresses -  FSA Underwriter','Other','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='OtherUndLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherUndLine3','spu_wp_underwriter_addresses','Other_Address3','0','Addresses -  FSA Underwriter','Other','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='OtherUndLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherUndLine4','spu_wp_underwriter_addresses','Other_Address4','0','Addresses -  FSA Underwriter','Other','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='OtherUndPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('OtherUndPostcode','spu_wp_underwriter_addresses','Other_Postcode','0','Addresses -  FSA Underwriter','Other','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='PreviousAddressUndCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressUndCountry','spu_wp_underwriter_addresses','Previous_Address_Country','0','Addresses -  FSA Underwriter','Previous Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='PreviousAddressUndLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressUndLine1','spu_wp_underwriter_addresses','Previous_Address_Address1','0','Addresses -  FSA Underwriter','Previous Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='PreviousAddressUndLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressUndLine2','spu_wp_underwriter_addresses','Previous_Address_Address2','0','Addresses -  FSA Underwriter','Previous Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='PreviousAddressUndLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressUndLine3','spu_wp_underwriter_addresses','Previous_Address_Address3','0','Addresses -  FSA Underwriter','Previous Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='PreviousAddressUndLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousAddressUndLine4','spu_wp_underwriter_addresses','Previous_Address_Address4','0','Addresses -  FSA Underwriter','Previous Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='PreviousUndPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('PreviousUndPostcode','spu_wp_underwriter_addresses','Previous_Address_Postcode','0','Addresses -  FSA Underwriter','Previous Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='RegisteredAddressUndCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressUndCountry','spu_wp_underwriter_addresses','Registered_Address_Country','0','Addresses -  FSA Underwriter','Registered Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='RegisteredAddressUndLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressUndLine1','spu_wp_underwriter_addresses','Registered_Address_Address1','0','Addresses -  FSA Underwriter','Registered Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='RegisteredAddressUndLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressUndLine2','spu_wp_underwriter_addresses','Registered_Address_Address2','0','Addresses -  FSA Underwriter','Registered Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='RegisteredAddressUndLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressUndLine3','spu_wp_underwriter_addresses','Registered_Address_Address3','0','Addresses -  FSA Underwriter','Registered Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='RegisteredAddressUndLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredAddressUndLine4','spu_wp_underwriter_addresses','Registered_Address_Address4','0','Addresses -  FSA Underwriter','Registered Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='RegisteredUndPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('RegisteredUndPostcode','spu_wp_underwriter_addresses','Registered_Address_Postcode','0','Addresses -  FSA Underwriter','Registered Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='SiteAddressUndCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressUndCountry','spu_wp_underwriter_addresses','Site_Address_Country','0','Addresses -  FSA Underwriter','Site Address','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='SiteAddressUndLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressUndLine1','spu_wp_underwriter_addresses','Site_Address_Address1','0','Addresses -  FSA Underwriter','Site Address','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='SiteAddressUndLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressUndLine2','spu_wp_underwriter_addresses','Site_Address_Address2','0','Addresses -  FSA Underwriter','Site Address','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='SiteAddressUndLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressUndLine3','spu_wp_underwriter_addresses','Site_Address_Address3','0','Addresses -  FSA Underwriter','Site Address','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='SiteAddressUndLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteAddressUndLine4','spu_wp_underwriter_addresses','Site_Address_Address4','0','Addresses -  FSA Underwriter','Site Address','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='SiteUndPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SiteUndPostcode','spu_wp_underwriter_addresses','Site_Address_Postcode','0','Addresses -  FSA Underwriter','Site Address','Postcode','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='SubAgentUndCountry')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentUndCountry','spu_wp_underwriter_addresses','Sub_Agent_Country','0','Addresses -  FSA Underwriter','Sub Agent','Country','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='SubAgentUndLine1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentUndLine1','spu_wp_underwriter_addresses','Sub_Agent_Address1','0','Addresses -  FSA Underwriter','Sub Agent','Address 1','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='SubAgentUndLine2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentUndLine2','spu_wp_underwriter_addresses','Sub_Agent_Address2','0','Addresses -  FSA Underwriter','Sub Agent','Address 2','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='SubAgentUndLine3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentUndLine3','spu_wp_underwriter_addresses','Sub_Agent_Address3','0','Addresses -  FSA Underwriter','Sub Agent','Address 3','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='SubAgentUndLine4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentUndLine4','spu_wp_underwriter_addresses','Sub_Agent_Address4','0','Addresses -  FSA Underwriter','Sub Agent','Address 4','1','NULL','9')
END
GO 
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_underwriter_addresses' AND Field_name='SubAgentUndPostcode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family)
 VALUES ('SubAgentUndPostcode','spu_wp_underwriter_addresses','Sub_Agent_Postcode','0','Addresses -  FSA Underwriter','Sub Agent','Postcode','1','NULL','9')
END
GO 


UPDATE 	Credit_Control_Step
SET	single_instalment_jump_to_next_step_broker=ISNULL(jump_to_next_step_broker,0),
	single_instalment_account_number_of_days=ISNULL(Broker_days,0),
	single_instalment_account_tollerance_amount=ISNULL(account_tolerance_amount,0),
	single_instalment_broker_letter_id=broker_letter_id
FROM  Credit_Control_Step JOIN credit_control_rule ON Credit_Control_Step.credit_control_rule_id = credit_control_rule.credit_control_rule_id
WHERE	business_type IN ( 'INSH', 'INSC','INS')
AND	single_instalment_jump_to_next_step_broker IS NULL
GO

-- *****************************************************************************  
-- * Author:      Samarjeet singh
-- * Date:        6/05/2014
-- * Purpose:     WPR6 Updatepolicy Associates And GetPolicyAssociates
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUPDPASO', 'SAM_Update Policy Associates'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMUPDPASO'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGETPASO', 'SAM_Get Policy Associates'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGETPASO'
GO


-- *****************************************************************************
-- * Author:   Samarjeet Singh
-- * Date:     26/05/2014
-- * Purpose:  Add Look up For Association Type
-- *****************************************************************************
IF NOT EXISTS (SELECT lookup_table_name 
FROM PMProduct_Lookup 
WHERE lookup_table_name = 'Association_Type')
BEGIN
 INSERT INTO PMProduct_Lookup
 (pmproduct_id, lookup_table_name, edit_privilege_level, 
 is_generic_maintenance, linked_data_mandatory)
 VALUES       (2, 'Association_Type', 3, 1, 0)
END
GO

-- *****************************************************************************  
-- * Author:      samarjeet Singh
-- * Date:        6/05/2014
-- * Purpose:     Add WP Feild For Policy Associates Document
-- *****************************************************************************

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyName')
	BEGIN
		INSERT INTO wp_fields(
               field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
		VALUES('PartyName','spu_wp_insurancefileassociates','Party_Name',0,'Policy','Associates','Party Name',1,'insurancefileassociates',9)
	END

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='AssociationTypeDesc')
	BEGIN
		INSERT INTO wp_fields(
               field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
		VALUES('AssociationTypeDesc','spu_wp_insurancefileassociates','Association_Type_Desc',0,'Policy','Associates','Type Of Association ',1,'insurancefileassociates',9)
	END

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='DateAttached')
	BEGIN
		INSERT INTO wp_fields(
               field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
		VALUES('DateAttached','spu_wp_insurancefileassociates','Date_Attached',4,'Policy','Associates','Date Attached',1,'insurancefileassociates',9)
	END


IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='DateRemoved')
	BEGIN
		INSERT INTO wp_fields(
               field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
		VALUES('DateRemoved','spu_wp_insurancefileassociates','Date_Removed',4,'Policy','Associates','Date Removed',1,'insurancefileassociates',9)
	END

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='AssociationDetail')
	BEGIN
		INSERT INTO wp_fields(
               field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
		VALUES('AssociationDetail','spu_wp_insurancefileassociates','Association_Detail',0,'Policy','Associates','Association Detail',1,'insurancefileassociates',9)
	END
	
Go
IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE loop1='insurancefileassociates')
	BEGIN
	             update wp_fields SET loop1='insurancefileassociates' WHERE sub_group='Associates'    
	END


-- *****************************************************************************  
-- * Author:      Sahil Ansari
-- * Date:        07 May 2014
-- * Purpose:     WPR5 CancelPremiumFinancePlan, CancelPfPolicies, ProcessPFPlan
-- *****************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCANPFPL', 'Cancel Premium Finance Plan'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMCANPFPL'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCANPFPO', 'Cancel PF Policies'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMCANPFPO'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMPROPFP', 'Process PF Plan'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMPROPFP'
GO
-- *****************************************************************************  
-- * Author:     SamarJeet
-- * Date:        28 Jul 2014
-- * Purpose:    WPR13

DECLARE @lCaptionID  INT
DECLARE @TaskGroupId INT
DECLARE @TaskId      INT

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task WHERE code = 'EXTWCONFIG')
    BEGIN
    EXECUTE spu_pm_caption_id_return 1, 'External Workflow Configuration', @lCaptionID OUTPUT

    INSERT INTO PMWrk_Task 
                          (caption_id, code, description, is_deleted, effective_date, 
         is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, 
         auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, 
         linked_caption_id, is_available_task, pmwrk_task_category_id
           ) 

       Values
      (@lCaptionID, 'EXTWCONFIG', 'External Workflow Configuration', 0, 
      GETDATE(), 0, 1, NULL, 'iSIRExternalWorkflowConfig','NavigatorV3', 0, 40, 0, NULL, NULL, NULL, 1, 2
    )
    
    END
 

    -- Create link to group for this new task
    SELECT @TaskId = ( SELECT pmwrk_task_id FROM pmwrk_task WHERE code = 'EXTWCONFIG')

    -- Add task to correct group
    SELECT @TaskGroupId = ( SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'COMMON')

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task_Group_Task WHERE pmwrk_task_group_id = @TaskGroupId AND pmwrk_task_id  = @TaskId)
    BEGIN
       INSERT INTO pmwrk_task_group_task
       (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num)
       VALUES
       (@TaskGroupId, @TaskId, 0)
    END


GO


-- *****************************************************************************
-- * Author:      Sahil Ansari
-- * Date:        29 July 2014
-- * Purpose:     WPR13 / E5
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGTOKey', 'GetTaskOnKeys'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGTOKey'
GO

Go


-- *****************************************************************************
-- * Author:     SamarJeet
-- * Date:        31 Jul 2014
-- * Purpose:    WPR13 to remove the CUD SP For UID Updations
-- *============================================================================
If exists(
          select 1 from GIS_Object where gis_object_id not in (
                                                                select GIS_Object_id from GIS_Property WHERE property_name ='UID') 
																and object_name not in ('work_claim','work_claim_peril')
		)

        BEGIN

				
				Declare @sprocName varchar(500)
				Declare curDropCUDSP cursor For 

				Select 'DDLDropProcedure ' +  Name As ProcName  from sys.procedures where name like '%_cud' 
				and name not like '%_Claim_cud' and name not like '%_Claim_Peril_cud'

		OPEN curDropCUDSP

				FETCH Next From  curDropCUDSP into @sprocName

				while @@fetch_status = 0

				begin

					Execute(@sprocName)

					FETCH NEXT FROM curDropCUDSP into @sprocName

				End

		Close curDropCUDSP

		Deallocate curDropCUDSP

       END

Go


-- *****************************************************************************
-- * Author:     SamarJeet
-- * Date:       31 Jul 2014
-- * Purpose:    WPR13 to remove the CUD SP For UID INSERTIN For OLD GISObjects
-- *============================================================================

DECLARE Curgis CURSOR FOR 
Select  GIS_Object_id,Table_name  from GIS_Object
Where  
object_name not in ('work_claim','work_claim_peril') And 
gis_object_id Not  in (
Select Distinct gis_object_id From GIS_Property where column_name='UID'
) 
										


Declare @nCount As Integer
Declare @nmaxcnt As Integer
Declare @GISTableName As Varchar(255)
Declare @GISTable_ID As Varchar(255)

OPEN Curgis


FETCH NEXT FROM Curgis  into  @GISTable_ID,@GISTableName


WHILE @@FETCH_STATUS = 0

BEGIN
BEGIN TRAN Trn

														  
Select @nmaxcnt=Max(ISNUll(gis_property_id,0))+1 from gis_property
															
																													
IF NOT EXISTS (SELECT NULL FROM GIS_Property WHERE gis_object_id = @GISTable_ID and column_name = 'UID')

Begin		
																													 
INSERT INTO dbo.GIS_Property ( gis_property_id, gis_object_id, property_name, column_name, data_type,
is_input_property, is_identifying_property, is_primary_key, polaris_property_id,
is_deleted, is_search_property, index_linking_id, Edit_Flags,
Specials_Type, Specials_Type_Reference, is_in_mis_export,
is_formatted_text, is_chase_cycle_property
)
VALUES  
(@nmaxcnt, @GISTable_ID, 'UID', 'UID', 5, 0, 0, 0, NULL, 0, NULL, NULL, 0, 0, '', NULL, NULL, NULL)

--Case Alter Table Name

EXEC DDLAddColumn @sTableName=@GISTableName, @sColumnName ='UID',@sColumnDefinition='varchar(255) NULL'

End

FETCH NEXT FROM Curgis  into  @GISTable_ID,@GISTableName

COMMIT TRAN Trn;


END


CLOSE Curgis

DEALLOCATE Curgis

go


IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='CCICreditControlReason')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('CCICreditControlReason','spu_wp_CreditControlItem','credit_control_reason',0,'Finance','Credit Control Item','Credit Control Reason',
    1,'creditcontrolitem',9)
END

GO
UPDATE 
		system_options 
		SET value ='1' 
		WHERE option_number = 5096 
		AND ISNULL(value,0) = 0

-- *****************************************************************************  
-- * Author:      Sahil Ansari
-- * Date:        10 Nov 2014
-- * Purpose:     RACTI JIRA SSP 138, Claim Locking
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGLD', 'GetLockDetails'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGLD'
GO
GO
-- *****************************************************************************
-- * Author:	Mohammed Shariq Iqbal
-- * Date:		13/06/2013
-- * Purpose:	PM027971
-- *****************************************************************************

DECLARE @Party_CNT INT

DECLARE Party_Agent SCROLL CURSOR
FOR
SELECT party_cnt FROM Party_Agent

OPEN Party_Agent
FETCH NEXT FROM Party_Agent
INTO @Party_CNT

WHILE @@FETCH_STATUS = 0

BEGIN
IF NOT EXISTS (Select party_cnt,process_type from agent_docs where  party_cnt=@Party_CNT)  
BEGIN
DECLARE @process_types_docs_id INT
Declare Process_Types_Docs SCROLL CURSOR
FOR
Select process_types_docs_id from Process_Types_Docs

Open Process_Types_Docs
FETCH NEXT FROM Process_Types_Docs
INTO @process_types_docs_id 

While @@FETCH_STATUS = 0

BEGIN
IF NOT EXISTS (Select party_cnt,process_type from agent_docs where  party_cnt=@Party_CNT and process_type=@process_types_docs_id)
insert into Agent_Docs (party_cnt,process_type) values (@Party_CNT,@process_types_docs_id)

FETCH NEXT FROM Process_Types_Docs
INTO @process_types_docs_id
END

CLOSE Process_Types_Docs

DEALLOCATE Process_Types_Docs
END


FETCH NEXT FROM Party_Agent
INTO @Party_CNT

END

CLOSE Party_Agent

DEALLOCATE Party_Agent

-- *****************************************************************************
-- * Author:	Ramesh Kumar
-- * Date:	04/10/2013
-- * Purpose:	PM031072
-- *****************************************************************************
UPDATE DocumentType SET from_sirius=1 WHERE documenttype_id=37

-- *****************************************************************************
-- * Author:	Ashish Sachdeva
-- * Date:		26/04/2013
-- * Purpose:	To avoide deadlocks
-- *****************************************************************************

EXEC DDLADDINDEX 'tax_calculation','ri_arrangement_line_id'
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMMLock', 'MaintainLock'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMMLock'
GO
EXEC DDLADDINDEX 'tax_calculation','policy_fee_u_id'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUPDFEE', 'SAM Update fee'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMUPDFEE'
GO

--**********************************************************************************************
-- Author : RAKESH BAROLIA
--
-- History: 08/07/2008
--
-- Task : PLUS ONE - CLOSE B  - Adding wp fields
--**********************************************************************************************


IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PF3FinanceProvider')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,product_family)
    VALUES('PF3FinanceProvider','spu_wp_PFPremiumFinanceThirdParty','FinanceProvider',0,'Premium Finance','All','Finance Provider',1,9)
END

-- *****************************************************************************
-- * Author:       Ashish Sachdeva
-- * Date:          26/06/2012
-- * Purpose:       MTA CANCELLATION QUOTE
-- *****************************************************************************

  IF NOT EXISTS ( SELECT NULL FROM Insurance_File_Type WHERE code='MTAQCAN')
  BEGIN
    DECLARE @MTAQCANcaption_id INT
    DECLARE @Insurance_File_Type_id INT

    SELECT @Insurance_File_Type_id = MAX(ISNULL(Insurance_File_Type_id,0))+1 
    FROM Insurance_File_Type

    EXECUTE spu_pm_caption_id_return 1, 'MTA Quotation Cancellation', @MTAQCANcaption_id OUTPUT

    INSERT INTO Insurance_File_Type
    (Insurance_File_Type_id,caption_id,code,description,var_data_structure_id,is_deleted,effective_date)
    VALUES
    (@Insurance_File_Type_id,@MTAQCANcaption_id,'MTAQCAN','MTA Quotation Cancellation',NULL,0,GETDATE())
  END
  
  GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMSETALL', 'Settle All Claim Payments'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMSETALL'
GO



--**********************************************************************************************
-- Author : MANISH ARORA
--
-- History: 24/07/2013
--
-- Task : WPR 13 - Enable Policy/Risk fee editing for Nexus (Paralleled from Enhancement # 6039 - Changeset # 13712)
--**********************************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUPDFEE', 'SAM Update fee'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMUPDFEE'
GO

GO
/**********************************************************************************
Name	:	SQL Script to Add a new Reinsurance Type as Catastrophe
Date	:	26/08/2010
Author	:	Shubhankar Singh
Product	:	SFI RoW - Core [Parallel from 1.13.4 Etana]
**********************************************************************************/

IF NOT EXISTS (SELECT * FROM Reinsurance_Type WHERE code='CAT')
BEGIN
    DECLARE @Caption_id INT
    DECLARE @MAX_id INT
    SELECT @MAX_id = MAX(Reinsurance_Type_id)+1 FROM Reinsurance_Type  
    EXEC spu_pm_caption_id_return 1,'Catastrophe',@Caption_id OUTPUT
    INSERT INTO Reinsurance_Type VALUES(@MAX_id,@Caption_id,'CAT','Catastrophe',0,Getdate())
END
GO

-- *****************************************************************************
-- * Author:      Gurucharan Gulati
-- * Date:        28 Mar 2011
-- * Purpose:	  E007
-- *****************************************************************************

-- To Add Cloned Reinsurance Batch Processing task
DECLARE @lCaptionID  INT
DECLARE @TaskGroupId INT
DECLARE @TaskId      INT
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task WHERE code = 'CLNRIBP')
    BEGIN
               EXECUTE spu_pm_caption_id_return 1, 'Cloned Reinsurance Batch Processing', @lCaptionID OUTPUT

               INSERT INTO PMWrk_Task 
                        (caption_id, code, description, is_deleted, effective_date, 
                        is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, 
                        auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, 
                        linked_caption_id, is_available_task, pmwrk_task_category_id) 
               VALUES( @lCaptionID, 'CLNRIBP', 'Cloned Reinsurance Batch Processing', 0,GETDATE(), 
			0, 1, NULL, 'iPMUCloneRIBatchProcess', 'NavigatorV3',
			0, 6, 0, NULL, NULL, 
			NULL, 1, 2)
    END

    -- Create link to group for this new task
    SELECT @TaskId = (SELECT pmwrk_task_id FROM pmwrk_task WHERE code = 'CLNRIBP')

    -- Add task to correct group
    SELECT @TaskGroupId = (SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'UNDER')

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task_Group_Task WHERE pmwrk_task_group_id = @TaskGroupId AND pmwrk_task_id  = @TaskId)
    BEGIN
       INSERT INTO pmwrk_task_group_task
       (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num)
       VALUES
       (@TaskGroupId, @TaskId, 0)
    END
END
GO

-- To Add Cloned Reinsurance Policy Amendment task
DECLARE @lCaptionID  INT
DECLARE @TaskGroupId INT
DECLARE @TaskId      INT
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task WHERE code = 'CLNRIPAM')
    BEGIN
               EXECUTE spu_pm_caption_id_return 1, 'Cloned Reinsurance Policy Amendment', @lCaptionID OUTPUT

               INSERT INTO PMWrk_Task 
                        (caption_id, code, description, is_deleted, effective_date, 
                        is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, 
                        auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, 
                        linked_caption_id, is_available_task, pmwrk_task_category_id) 
               VALUES( @lCaptionID, 'CLNRIPAM', 'Cloned Reinsurance Policy Amendment', 0,GETDATE(), 
			0, 1, NULL, 'iPMUClonedRIManual', 'NavigatorV3',
			0, 6, 0, NULL, NULL, 
			NULL, 1, 2)
    END

    -- Create link to group for this new task
    SELECT @TaskId = (SELECT pmwrk_task_id FROM pmwrk_task WHERE code = 'CLNRIPAM')

    -- Add task to correct group
    SELECT @TaskGroupId = (SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'UNDER')

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task_Group_Task WHERE pmwrk_task_group_id = @TaskGroupId AND pmwrk_task_id  = @TaskId)
    BEGIN
       INSERT INTO pmwrk_task_group_task
       (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num)
       VALUES
       (@TaskGroupId, @TaskId, 0)
    END
END
GO

-- Script to add new transaction code in Transaction_Type table for Cloned RI
IF EXISTS (SELECT NULL FROM SYSOBJECTS Where name = 'Transaction_Type' AND xtype='u')
   BEGIN

	DECLARE @caption_id int
	IF NOT EXISTS(SELECT NULL from Transaction_Type WHERE code = 'CRI') 
	  BEGIN
	  	DECLARE @transaction_type_id int
		SELECT @transaction_type_id = ISNULL(MAX(transaction_type_id),0)+1 from transaction_type
		-- Get caption
        EXEC spu_pm_caption_id_return 1, 'Cloned Reinsurance', @caption_id OUTPUT
 
        -- Add new new contact type
        INSERT INTO Transaction_Type(transaction_type_id, caption_id, code, description, transaction_type_basis, is_deleted, effective_date)
            VALUES(@transaction_type_id, @caption_id, 'CRI','Cloned Reinsurance', 'A', 0, getdate())
	  END
END
   
-- *****************************************************************************
-- * Author:      Kuljeet Kaur
-- * Date:        6 Apr 2011
-- * Purpose:	  E007
-- *********

-- To Add Portfolio Transfer Policy Amendment Task
DECLARE @lCaptionID  INT
DECLARE @TaskGroupId INT
DECLARE @TaskId      INT

IF EXISTS (SELECT 1 FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM PMWrk_Task WHERE code = 'PTAMEND')
    BEGIN
	   EXECUTE spu_pm_caption_id_return 1, 'Portfolio Transfer Amendment', @lCaptionID OUTPUT

	   INSERT INTO PMWrk_Task
		(caption_id, code, description, is_deleted, effective_date,
		is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name,
		auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name,
		linked_caption_id, is_available_task, pmwrk_task_category_id)

	   SELECT @lCaptionID, 'PTAMEND', 'Portfolio Transfer Amendment', 0,
		  GETDATE(), 0, 1, NULL, 'iPMURIManPortfolioTransfer', 'NavigatorV3', 0, 1, 0, NULL, NULL, NULL, 1, 2
	   
    END

    -- Create link to group for this new task
    SELECT @TaskId = ( SELECT  pmwrk_task_id FROM pmwrk_task WHERE code = 'PTAMEND')

    -- Add task to correct group
    SELECT @TaskGroupId = ( SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'UNDER')

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task_Group_Task WHERE pmwrk_task_group_id = @TaskGroupId AND pmwrk_task_id  = @TaskId)
    BEGIN
       INSERT INTO pmwrk_task_group_task
       (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num)
       VALUES
       (@TaskGroupId, @TaskId, 0)
    END
END
GO

--*****************************************************************************
-- * Author:        Kuljeet Kaur
-- * Date:          16/05/2011
-- * Purpose:       E007
-- *****************************************************************************

DECLARE @lCaptionID INT
EXECUTE spu_pm_caption_id_return 1, 'Accounting Year', @lCaptionID OUTPUT

UPDATE Proportional_RI_Calculation_Method SET caption_id = @lCaptionID 
WHERE Proportional_RI_Calculation_Method_id = 2
GO


-- *****************************************************************************
-- * Author:   Vijay Pal
-- * Date:     20/07/2013
-- * Purpose:  WPR53
-- *****************************************************************************

	-- Script to add column to GIS Output table
	DECLARE @code VARCHAR(20)
	DECLARE @gis_object_id INT
	DECLARE @gis_property_id INT
	DECLARE GIS_OUTPUT_Cursor CURSOR FAST_FORWARD FOR  
	SELECT RTRIM(code) FROM GIS_Data_Model
	WHERE gis_data_model_type_id = 1 -- ie Risk

	OPEN GIS_OUTPUT_Cursor  
	FETCH NEXT FROM GIS_OUTPUT_Cursor INTO @code    
	-- Start processing  
	WHILE (@@FETCH_STATUS = 0)  
	BEGIN
	SELECT @code = @code + '_OUTPUT'
	IF EXISTS(SELECT * FROM SYSOBJECTS where name = @code)
	BEGIN	
		SELECT @gis_object_id = gis_object_id 
		FROM GIS_Object WHERE table_name = @code
		IF NOT EXISTS (SELECT property_name FROM GIS_property WHERE property_name ='Disable_New_ProRata' AND gis_object_id=@gis_object_id)
		BEGIN
			SELECT @gis_property_id = MAX(gis_property_id) + 1 
					FROM gis_property 
						WHERE gis_object_id = @gis_object_id 
			EXEC spu_GIS_property_add
				@gis_property_id=@gis_property_id,  
				@gis_object_id=@gis_object_id,  
				@property_name='Disable_Original_ProRata',  
				@column_name='Disable_Original_ProRata',  
				@data_type=20,  
				@is_input_property=0,  
				@is_identifying_property=0,  
				@is_primary_key=0,  
				@polaris_property_id=NULL,  
				@is_deleted=0,  
				@is_search_property=0,  
				@index_linking_id=NULL,  
				@Edit_Flags=0,  
				@Specials_Type=0,  
				@Specials_Type_Reference=NULL,  
				@is_in_mis_export=0,
				@is_formatted_text=0 ,
                @is_chase_cycle_property=0,
				@is_claim360display=0
		END 
		IF NOT EXISTS (SELECT property_name FROM GIS_property WHERE property_name ='Disable_New_ProRata' AND gis_object_id=@gis_object_id)
		BEGIN
				SELECT @gis_property_id = MAX(gis_property_id) + 1 
					FROM gis_property 
						WHERE gis_object_id = @gis_object_id 
				EXEC spu_GIS_property_add
				@gis_property_id=@gis_property_id,  
				@gis_object_id=@gis_object_id,  
				@property_name='Disable_New_ProRata',  
				@column_name='Disable_New_ProRata',  
				@data_type=20,  
				@is_input_property=0,  
				@is_identifying_property=0,  
				@is_primary_key=0,  
				@polaris_property_id=NULL,  
				@is_deleted=0,  
				@is_search_property=0,  
				@index_linking_id=NULL,  
				@Edit_Flags=0,  
				@Specials_Type=0,  
				@Specials_Type_Reference=NULL,  
				@is_in_mis_export=0,
				@is_formatted_text=0 ,
                @is_chase_cycle_property=0,
					 @is_claim360display=0
		END				
	END		
	-- Get Next Record
	FETCH NEXT FROM GIS_OUTPUT_Cursor INTO @code  
	END

	-- Close the cursor
	Close GIS_OUTPUT_Cursor  
	Deallocate GIS_OUTPUT_Cursor



	EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMFCASE', 'SAM Find Case'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMFCASE'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGCASE', 'SAM GetCaseDetails'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGCASE'
GO
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMSCASE', 'SAM SaveCase'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMSCASE'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCCASEVER', 'SAM CreateCaseVersion'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMCCASEVER'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCCASE', 'SAM CloseCase'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMCCASE'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCASELU', 'SAM CaseLinkUnlink'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMCASELU'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGCCLINK', 'SAM GetCashClaimLink'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGCCLINK'
GO

--*****************************************************************
--Author:   Deepak Arora (Paralleled by Manish Arora on 06-Sep-2013)
-- * Date:     22/11/2010
-- * Purpose: PN 76147 - claim Document now govern by claim Links.
-- So, generate_claim_payment_doc at product should always be 1.
-- *****************************************************************************
Update Product_Claims_Workflow SET generate_claim_notification_doc = 1, 
generate_claim_payment_doc =1
Where ISNULL(generate_claim_payment_doc,0) = 0 OR ISNULL(generate_claim_notification_doc,0) = 0


GO

-- *****************************************************************************  
-- * Author:      Saumitra Bhatnagar
-- * Date:        08 August 2013
-- * Purpose:     WPR34 - Reverse Allocation - Parallel
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMREVALOC', 'SAM Reverse Allocation'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMREVALOC'
GO

-- *****************************************************************************  
-- * Author:      Rahul Jaiswal
-- * Date:        26 Nov 2010
-- * Purpose:     WPR05 - RI
-- *****************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMFINDRI', 'Find Reinsurer'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMFINDRI'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGRI07', 'Get RI Arrangement Line 2007'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMGRI07'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGRIMOD', 'Get RI Model and Line Details'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMGRIMOD'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMURI07', 'Update RI Arrangement Line 2007'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMURI07'
GO
-- *****************************************************************************  
-- * Author:      Prabodh Mishra
-- * Date:        29 Nov 2010
-- * Purpose:     WPR05 - RI
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGTtyPty', 'Get Treaty Party'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGTtyPty'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGCLRI07', 'Get Claim RI Line 2007'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGCLRI07'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUCLRI07', 'Update Claim RI Line 2007'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMUCLRI07'
GO

-- *****************************************************************************
-- * Author:   Richard Clarke
-- * Date:     10/10/2013
-- * Purpose:  Compiled rules development and DRE renaming to PRE
-- *****************************************************************************

--This will only run once on any db
IF (SELECT COUNT(CODE) FROM risk_type_rule_set_type WHERE CODE = 'PRE') > 1 BEGIN
DECLARE @nMinPREID AS INT 
 SELECT @nMinPREID = MIN(risk_type_rule_set_type_id) FROM risk_type_rule_set_type WHERE CODE = 'PRE'
PRINT(@nMinPREID)

UPDATE risk_type_rule_set SET risk_type_rule_set_type_id = @nMinPREID
FROM risk_type_rule_set RS
JOIN risk_type_rule_set_type RSTY ON RS.risk_type_rule_set_type_id = RSTY.risk_type_rule_set_type_id
WHERE RSTY.code = 'PRE'

DELETE FROM risk_type_rule_set_type WHERE CODE = 'PRE' AND risk_type_rule_set_type_id > @nMinPREID

END



DECLARE @lCaptionID int
--UPDATE THE DRE rule set type to PRE
IF EXISTS (SELECT 1 FROM risk_type_rule_set_type WHERE code = 'DRE')
	BEGIN	
	DECLARE @rtrst int
	
	SELECT @rtrst = risk_type_rule_set_type_id FROM risk_type_rule_set_type WHERE code = 'DRE'
	EXECUTE spu_pm_caption_id_return 1, 'PRE', @lCaptionID OUTPUT
	--INSERT INTO risk_type_rule_set_type(caption_id, code, description,is_deleted, effective_date)
	--VALUES (@lCaptionID, 'DRE', 'DRE Rating Engine',0, '1950/01-01')
	UPDATE risk_type_rule_set_type SET caption_id = @lCaptionID, code = 'PRE', description = 'Precision Rating Engine' where risk_type_rule_set_type_id = @rtrst
	END
ELSE BEGIN
	-- *****************************************************************************
	-- * Author:   Ashish Sachdeva
	-- * Date:     13/05/2010
	-- * Purpose:  DRE Integration Section 6
	-- *****************************************************************************
	
	IF NOT EXISTS (SELECT 1 FROM risk_type_rule_set_type WHERE code = 'SCRIPT')
	BEGIN	
		EXECUTE spu_pm_caption_id_return 1, 'SCRIPT', @lCaptionID OUTPUT
		INSERT INTO risk_type_rule_set_type(caption_id, code, description,is_deleted, effective_date)
		VALUES (@lCaptionID, 'SCRIPT', '.Rul file script',0, '1950/01-01')
	END
	IF NOT EXISTS (SELECT 1 FROM risk_type_rule_set_type WHERE code = 'PRE')
	BEGIN	
		EXECUTE spu_pm_caption_id_return 1, 'PRE', @lCaptionID OUTPUT
		INSERT INTO risk_type_rule_set_type(caption_id, code, description,is_deleted, effective_date)
		VALUES (@lCaptionID, 'PRE', 'Precision Rating Engine',0, '1950/01-01')
	END
END

IF EXISTS(SELECT * FROM risk_type_rule_set WHERE risk_type_rule_set_type_id IS NULL)
BEGIN
	DECLARE @risk_type_rule_set_type_id INT
	SELECT @risk_type_rule_set_type_id = risk_type_rule_set_type_id 
	FROM risk_type_rule_set_type 
	WHERE code = 'SCRIPT'

	UPDATE risk_type_rule_set 
	SET risk_type_rule_set_type_id = @risk_type_rule_set_type_id 
	WHERE risk_type_rule_set_type_id IS NULL
END
GO

DECLARE @lCaptionID int
IF NOT EXISTS (SELECT 1 FROM risk_type_rule_set_type WHERE code = 'COMPILED')
BEGIN	
	EXECUTE spu_pm_caption_id_return 1, 'COMPILED', @lCaptionID OUTPUT
	INSERT INTO risk_type_rule_set_type(caption_id, code, description,is_deleted, effective_date)
	VALUES (@lCaptionID, 'COMPILED', 'Compiled Rules',0, '1950/01-01')
END

GO
UPDATE system_options 
SET value = 0
WHERE
option_number=5099
AND value IS NULL
GO


-- *****************************************************************************  
-- * Author:      Ashish Sachdeva
-- * Date:        08 Nov 2013
-- * Purpose:     RND003 - DMEToSharePoint
-- *****************************************************************************
DECLARE @lCaptionID INT
IF NOT EXISTS(SELECT NULL FROM DME_Migration_Status WHERE code = 'WIP')
BEGIN
 EXEC spu_pm_caption_id_return 1, 'In Progress', @lCaptionID OUTPUT
 INSERT INTO DME_Migration_Status(
 code,description,caption_id,effective_date,is_deleted)
 VALUES ('WIP','In Progress',@lCaptionID,GETDATE(),0)
END
IF NOT EXISTS(SELECT NULL FROM DME_Migration_Status WHERE code = 'FAIL')
BEGIN
 EXEC spu_pm_caption_id_return 1, 'Failed', @lCaptionID OUTPUT
 INSERT INTO DME_Migration_Status(
 code,description,caption_id,effective_date,is_deleted)
 VALUES ('FAIL','Failed',@lCaptionID,GETDATE(),0)
END
IF NOT EXISTS(SELECT NULL FROM DME_Migration_Status WHERE code = 'COMPLETE')
BEGIN
 EXEC spu_pm_caption_id_return 1, 'Completed', @lCaptionID OUTPUT
 INSERT INTO DME_Migration_Status(
 code,description,caption_id,effective_date,is_deleted)
 VALUES ('COMPLETE','Completed',@lCaptionID,GETDATE(),0)
END
GO
-- *****************************************************************************  
-- * Author:      Ashish Sachdeva
-- * Date:        20 Dec 2013
-- * Purpose:     RND009 - ClaimPaymentProcessing SettleAll in POrtal
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMSETALL', 'Settle All Claim Payments'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMSETALL'
GO


--*****************************************************************************
-- * Author:     Sumeet Singh
-- * Date:       21/11/2013
-- * Purpose:  	 PM032307
-- *****************************************************************************
update PMProduct_Client_Install set is_client_auto_installable=0
GO
update PMProduct set [description]='Pure Back-office' where code='SirSol'
Go
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMMID', 'GetMIDFiles'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMMID'
GO
-- * Purpose: Added Report Incurred_Claims_Details.rpt       
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U') 
BEGIN
    DECLARE @report_id INT
    DECLARE @lCaptionID INT
    DECLARE @group_id INT

    -- Incurred Claims report
    SELECT @report_id = report_id FROM report WHERE code = 'Inc_ClmDet'

    IF(ISNULL(@report_id,0) = 0)
    BEGIN    
        -- Add report
        SELECT @report_id = MAX(report_id)+1 FROM report
        EXECUTE spu_pm_caption_id_return 1, 'Incurred Claims Details', @lCaptionID OUTPUT
        INSERT INTO report(report_id, caption_id, code, description, is_deleted, effective_date, report_name) 
               VALUES(@report_id, @lCaptionID, 'Inc_ClmDet', 'Incurred_Claims_Details', 0, GETDATE(), 'Incurred_Claims_Details.rpt')

        -- Link report to group
        SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'CLM'
        IF(ISNULL(@group_id,0)<>0)
            INSERT INTO Report_Group_Contents (report_group_id,report_id) VALUES(@group_id,@report_id)
    END
END
GO

-- *****************************************************************************  
-- * Author:       Sandeep Kumar
-- * Date:         09-07-2015
-- * Purpose:      Increased the size of doc_name column in doc_document tabke as per enhancement for PM037468
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'doc_document', 'doc_name', 'varchar(255) NOT NULL'
-- *****************************************************************************
-- * Author:       Vijay Pal
-- * Date:          26/06/2014
-- * Purpose:      WPR 43/76
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMFINDPMT', 'FindPaymentDetails'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMFINDPMT'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCANPMT', 'CancelPayment'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMCANPMT'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMFINDRCT', 'FindReceiptDetails'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMFINDRCT'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCANRCT', 'CancelReceipt'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMCANRCT'
GO

  IF NOT EXISTS ( SELECT NULL FROM CashListItem_Receipt_Status WHERE code='CAN')
  BEGIN
    DECLARE @CANcaption_id INT
    DECLARE @CashListItem_Receipt_Status_id INT

    SELECT @CashListItem_Receipt_Status_id = MAX(ISNULL(CashListItem_Receipt_Status_id,0))+1 
    FROM CashListItem_Receipt_Status

    EXECUTE spu_pm_caption_id_return 1, 'Cancelled', @CANcaption_id OUTPUT

    INSERT INTO CashListItem_Receipt_Status
    (cashlistitem_receipt_status_id,code,description,caption_id,effective_date,is_deleted)
    VALUES
    (@CashListItem_Receipt_Status_id,'CAN','Cancelled',@CANcaption_id,GETDATE(),0)
  END
  
GO

-- * Author:       John Durnall ( Sandeep Kumar)
-- * Date:          19/04/2012
-- * Purpose:      WPR85
-- *****************************************************************************
  IF NOT EXISTS ( SELECT NULL FROM CashListItem_Receipt_Type WHERE code='CLAIMRPT')
  BEGIN
    DECLARE @CashListItem_Receipt_Type_caption_id INT
    DECLARE @CashListItem_Receipt_Type_id INT

    SELECT @CashListItem_Receipt_Type_id = MAX(ISNULL(CashListItem_Receipt_Type_id,0))+1 
    FROM CashListItem_Receipt_Type

    EXECUTE spu_pm_caption_id_return 1, 'Claim Receipt', @CashListItem_Receipt_Type_caption_id OUTPUT

    INSERT INTO CashListItem_Receipt_Type
    (CashListItem_Receipt_Type_id,code,description,caption_id,effective_date,is_deleted)
    VALUES
    (@CashListItem_Receipt_Type_id,'CLAIMRPT','Claim Receipt',@CashListItem_Receipt_Type_caption_id,GETDATE(),0)
  END
  
GO

-- *****************************************************************************
-- * Author:       John Durnall ( Sandeep Kumar)
-- * Date:          02/05/2012
-- * Purpose:      WPR85
-- *****************************************************************************
 
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGENCALI', 'GenerateCashList' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGENCALI'
GO

-- *****************************************************************************
-- * Author:       Sandeep Kumar
-- * Date:          24/05/2012
-- * Purpose:      WPR85
-- *****************************************************************************
 
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMDEFBAWC', 'GetDefaultBankAccountWithCurrency' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMDEFBAWC'
GO
-- *****************************************************************************
-- * Author:       Rakesh Barolia
-- * Date:          28/08/2014
-- * Purpose:       WPR46
-- *****************************************************************************
UPDATE Product SET do_not_delete_renQuote_on_mta=0 WHERE do_not_delete_renQuote_on_mta IS NULL
GO
-- *****************************************************************************
-- * Author:      Prabodh Mishra
-- * Date:        28 Jun 2012
-- * Purpose:     Tech Spec - WPR81 & WPR105
-- *****************************************************************************
IF NOT EXISTS (SELECT NULL FROM Transdetail_Type WHERE code like 'COLLECTION')
BEGIN
	DECLARE @caption_id INT
	DECLARE @transdetailtype_id INT
	DECLARE @effective_date datetime
	
	SELECT @effective_date = effective_date  FROM Transdetail_Type WHERE code like 'GROSS'
	
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'COMMTAX',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'COMMTAX', 'COMMTAX', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'INSDEBT',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'INSDEBT', 'Instalment Debt', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'INSFEE',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'INSFEE', 'Instalment Fees', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'ISUSP',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'ISUSP', 'Instalment Suspense', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'COLLECTION',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'COLLECTION', 'Instalment Collection', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINPREM',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINPREM', 'RI Premium', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINCOMM',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINCOMM', 'RI Commission', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINTAX',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINTAX', 'RI Premium Tax', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINCOMMTAX',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINCOMMTAX', 'RI Commission Tax', 0, @effective_date)
						
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINPREMTQ',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINPREMTQ', 'Premium Treaty Quota Share', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINPREMFS',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINPREMFS', 'Premium First Surplus', 0, @effective_date)
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	
	EXEC spu_pm_caption_id_return 1,'REINPREMTX',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINPREMTX', 'Premium Treaty XOL', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINPREMFAC',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINPREMFAC', 'Premium FAC', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINPREMFX',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINPREMFX', 'Premium FAC XOL', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINPREMCAT',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINPREMCAT', 'Premium CAT XOL', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINCOMMTQ',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINCOMMTQ', 'Commission Treaty Quota Share', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINCOMMFS',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINCOMMFS', 'Commission First Surplus', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINCOMMTX',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINCOMMTX', 'Commission Treaty XOL', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINCOMMFAC',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINCOMMFAC', 'Commission FAC', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINCOMMFX',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINCOMMFX', 'Commission FAC XOL', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINCOMMCAT',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINCOMMCAT', 'Commission CAT XOL', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINTAXTQ',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINTAXTQ', 'Tax on Treaty Quota Share', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINTAXFS',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINTAXFS', 'Tax on First Surplus', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINTAXTX',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINTAXTX', 'Tax on Treaty XOL', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINTAXFAC',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINTAXFAC', 'Tax on FAC', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINTAXFX',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINTAXFX', 'Tax on FAC XOL', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINTAXCAT',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINTAXCAT', 'Tax on CAT XOL', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'GAINLOSS',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'GAINLOSS', 'Currency gain loss', 0, @effective_date)
	
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CASHPAY',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CASHPAY', 'Cash Payment', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CASHTAXPAY',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CASHTAXPAY', 'Tax on Cash payment', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CASHREC',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CASHREC', 'Cash Receipt', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLMGROSS',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLMGROSS', 'Claim Reserve Gross', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLPGROSS',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLPGROSS', 'Claim Payment Gross', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLRGROSS',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLRGROSS', 'Claim Receipt Gross', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLMRIRES',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLMRIRES', 'Claim Reserve', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLPRIPAY',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLPRIPAY', 'Claim Payment', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLRRIREC',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLRRIREC', 'Claim Receipt', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLMRITQ',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLMRITQ', 'Claim Reserve Treaty Quota Share', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLMRIFS',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLMRIFS', 'Claim Reserve First Surplus', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLMRITX',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLMRITX', 'Claim Reserve Treaty XOL', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLMRIFAC',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLMRIFAC', 'Claim Reserve FAC', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLMRIFX',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLMRIFX', 'Claim Reserve FAC XOL', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLMRICAT',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLMRICAT', 'Claim Reserve CAT XOL', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLPTAX',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLPTAX', 'Tax on Claim Payment', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLPRITQ',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLPRITQ', 'Claim Payment Treaty Quota Share', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLPRIFS',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLPRIFS', 'Claim Payment First Surplus', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLPRITX',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLPRITX', 'Claim Payment Treaty XOL', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLPRIFAC',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLPRIFAC', 'Claim Payment FAC', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLPRIFX',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLPRIFX', 'Claim Payment FAC XOL', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLPRICAT',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLPRICAT', 'Claim Payment CAT XOL', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLRTAX',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLRTAX', 'Tax on Claim Receipt', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLRRITQ',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLRRITQ', 'Claim Receipt Treaty Quota Share', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLRRIFS',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLRRIFS', 'Claim Receipt First Surplus', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLRRITX',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLRRITX', 'Claim Receipt Treaty XOL', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLRRIFAC',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLRRIFAC', 'Claim Receipt FAC', 0, @effective_date)
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLRRIFX',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLRRIFX', 'Claim Receipt FAC XOL', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLRRICAT',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLRRICAT', 'Claim Receipt CAT XOL', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLMRITR',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLMRIFX', 'Claim Reserve Treaty', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLPRITR',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLPRITR', 'Claim Payment Treaty', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLRRITR',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLRRITR', 'Claim Receipt Treaty', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLMRICOI',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLMRICOI', 'Claim Reserve CoInsurance', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLPRICOI',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLPRICOI', 'Claim Payment CoInsurance', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'CLRRICOI',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'CLRRICOI', 'Claim Receipt CoInsurance', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'COIPREM',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'COIPREM', 'Premium Coinsurance', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'COICOMM',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'COICOMM', 'Commission Coinsurance', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'COITAX',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'COITAX', 'Tax Coinsurance', 0, @effective_date)

	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINPREMTR',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINPREMTR', 'Premium Treaty', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINCOMMTR',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINCOMMTR', 'Commission Treaty', 0, @effective_date)
															
	SELECT @transdetailtype_id = MAX(transdetail_type_id) + 1 FROM Transdetail_Type
	EXEC spu_pm_caption_id_return 1,'REINTAXTR',@Caption_id Output
	INSERT INTO Transdetail_Type (transdetail_type_id, Caption_Id, Code, Description, Is_Deleted, Effective_Date)
															VALUES(@transdetailtype_id, @Caption_id, 'REINTAXTR', 'Tax on Treaty', 0, @effective_date)
															
	UPDATE Transdetail_Type SET is_extended = 1 WHERE Code IN ('GROSS', 'TAX', 'COMM', 'COMMTAX', 'FEE', 'FEETAX')
END
GO
IF EXISTS (SELECT * FROM AllocationDetail WHERE alloc_account_amount IS NULL)
BEGIN
	UPDATE [AllocationDetail]
		SET [alloc_account_amount] =  Case When  ISNULL([TM].[is_reversed], 0) = 1 Then 0  When  ISNULL([TM].[is_reversed], 0) = 0 Then [TM].[account_match_amount] else  [AD].[alloc_account_amount] end,
			[alloc_system_amount] = Case When  ISNULL([TM].[is_reversed], 0) = 1 Then 0 When  ISNULL([TM].[is_reversed], 0) = 0 Then [TM].[system_match_amount] else  [AD].[alloc_system_amount] end,
			[is_reversed] =  Case When  ISNULL([TM].[is_reversed], 0) = 1 Then 1 else [AD].[is_reversed]  end,
			[allocation_reversed_date] = Case When  ISNULL([TM].[is_reversed], 0) = 1 Then [TM].allocation_reversed_date else [AD].[allocation_reversed_date] end
		FROM [AllocationDetail] [AD]
		INNER JOIN [TransMatch] [TM] 
			ON [TM].[allocationdetail_id] = [AD].[allocationdetail_id] AND [TM].[transdetail_id] = [AD].[transdetail_id]
	WHERE [AD].[alloc_account_amount] IS NULL ;
END


GO

-- *****************************************************************************
-- * Author:      Prabodh Mishra
-- * Date:        25 Jun 2012
-- * Purpose:     Tech Spec - WPR81 - Transaction Type & Audit Trail
-- *****************************************************************************
EXEC DDLAddColumn 'AllocationBatch', 'AllocationID', 'INT'
GO
IF NOT EXISTS ( SELECT TOP 1 (allocationbatch_id) FROM AllocationBatch )
BEGIN


CREATE TABLE #tempPeriod
(
	[period_id] int,
	[period_start_date_exclusive] DateTime,
	[period_end_date] DateTime
	

)
 
 Insert Into #tempPeriod 
	SELECT
		[P].[period_id],
		MAX([PB].[period_end_date]) AS 'period_start_date_exclusive',
		MIN([P].[period_end_date]) AS 'period_end_date'
	FROM [dbo].[Period] AS [P] WITH (NOLOCK)
		LEFT OUTER JOIN [dbo].[Period] AS [PB] WITH (NOLOCK)
			ON [PB].[period_end_date] < [P].[period_end_date]
	GROUP BY [P].[period_id]







	INSERT [dbo].[AllocationBatch] (
		[AllocationID],
		[allocationbatch_date],
		[period_id]
		
	)
	SELECT
	[A].[allocation_id],
	[A].[allocation_date],
	[P].[period_id]
FROM [dbo].[Allocation] AS [A]
	INNER JOIN #tempPeriod AS [P]
		ON ([P].[period_start_date_exclusive] IS NULL
				OR [P].[period_start_date_exclusive] < [A].[allocation_date])
			AND [P].[period_end_date] >= [A].[allocation_date]
WHERE [A].allocationbatch_id IS NULL
	
	
UPDATE 
    [Allocation]
SET 
    Allocation.allocationbatch_id = [AllocationBatch].allocationbatch_id
FROM 
    [Allocation]
    INNER JOIN [AllocationBatch]  ON [Allocation].allocation_id=[AllocationBatch].AllocationID
WHERE 
    [AllocationBatch].AllocationID IS Not NULL
	
	Drop Table #tempPeriod
END;

EXEC DDLDropColumn 'AllocationBatch','AllocationID'


IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
	DECLARE @pmproduct_id INT
	DECLARE @caption_id INT


   	IF NOT EXISTS (SELECT NULL FROM pmproduct_lookup WHERE lookup_table_name='DOPaymentTerms')
	BEGIN
		SELECT @pmproduct_id = pmproduct_id FROM pmproduct WHERE code='SirSol'

		INSERT INTO pmproduct_lookup(pmproduct_id,lookup_table_name,edit_privilege_level,linked_caption_id,linked_class_name,linked_object_name,is_generic_maintenance,interface_component,interface_control)
		VALUES(@pmproduct_id,'DOPaymentTerms',3,NULL,NULL,NULL,0,NULL,NULL)
		
		EXEC spu_pm_caption_id_return 1,'Cash',@Caption_id Output
		INSERT INTO DOPaymentTerms ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'Cash','Cash',0,'2000-Jan-01')

		EXEC spu_pm_caption_id_return 1,'Debit Order',@Caption_id Output
		INSERT INTO DOPaymentTerms ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'DebitOrder','Debit Order',0,'2000-Jan-01')
	END

   	IF NOT EXISTS (SELECT NULL FROM pmproduct_lookup WHERE lookup_table_name='CollectionFrequency')
	BEGIN
		SELECT @pmproduct_id = pmproduct_id FROM pmproduct WHERE code='SirSol'

		INSERT INTO pmproduct_lookup(pmproduct_id,lookup_table_name,edit_privilege_level,linked_caption_id,linked_class_name,linked_object_name,is_generic_maintenance,interface_component,interface_control)
		VALUES(@pmproduct_id,'CollectionFrequency',3,NULL,NULL,NULL,0,NULL,NULL)
		
		EXEC spu_pm_caption_id_return 1,'Monthly',@Caption_id Output
		INSERT INTO CollectionFrequency ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'Monthly','Monthly',0,'2000-Jan-01')

		EXEC spu_pm_caption_id_return 1,'Quarterly',@Caption_id Output
		INSERT INTO CollectionFrequency ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'Quarterly','Quarterly',0,'2000-Jan-01')

		EXEC spu_pm_caption_id_return 1,'BI-Annually',@Caption_id Output
		INSERT INTO CollectionFrequency ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'BIAnnual','BI-Annually',0,'2000-Jan-01')

		EXEC spu_pm_caption_id_return 1,'Once Off',@Caption_id Output
		INSERT INTO CollectionFrequency ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'Once Off','Once Off',0,'2000-Jan-01')		
	END
END
GO

--****************
IF EXISTS(SELECT * FROM sysobjects WHERE ID = OBJECT_ID('GetDaysInYearProrata'))
 DROP FUNCTION GetDaysInYearProrata
GO

CREATE FUNCTION GetDaysInYearProrata
(
	@StartDate datetime,
	@EndDate   datetime
)
RETURNS int
AS
BEGIN

	DECLARE
		@DaysInYear int,
		@NextYearStart DATETIME = DATEADD(YEAR, 1, @StartDate)

	-- If date falls on 29th of feb then days in year should be 366 and we can exit
	IF MONTH(@StartDate) = 2 AND DAY(@StartDate) = 29
		RETURN 366

	SELECT @DaysInYear = DATEDIFF(DAY, @StartDate, @NextYearStart)

	-- If there are 366 days in the year then no need to check the rest
	IF @DaysInYear = 366
		BEGIN
			IF YEAR(@EndDate) < YEAR(@NextYearStart)
			BEGIN
				IF MONTH(@EndDate) > 2
					RETURN 366
				IF MONTH(@EndDate) = 2 AND DAY(@EndDate) = 29
					RETURN  366
			END
			ELSE
				RETURN 366
		END

	WHILE @NextYearStart < @EndDate
	BEGIN
		DECLARE @CurrentYear DATETIME = @NextYearStart

		SELECT @NextYearStart = DATEADD(YEAR, 1, @CurrentYear)
		SELECT @DaysInYear = DATEDIFF(DAY, @CurrentYear, @NextYearStart)
		IF @DaysInYear = 366
		BEGIN
			IF YEAR(@EndDate) <= YEAR(@NextYearStart)
				BEGIN
					IF MONTH(@EndDate) > 2
						RETURN 366
					IF MONTH(@EndDate) = 2 AND DAY(@EndDate) = 29
						RETURN  366
					ELSE
						RETURN 365
				END
				ELSE
					RETURN 366
		END
	END

	RETURN @DaysInYear
END
GO
-- *****************************************************************************  
-- * Author:      Prabodh Mishra
-- * Date:        25 Jun 2012
-- * Purpose:     Tech Spec - WPR105 - Debit Order Processing
-- *****************************************************************************
IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
	DECLARE @pmproduct_id INT
	DECLARE @caption_id INT


   	IF NOT EXISTS (SELECT NULL FROM pmproduct_lookup WHERE lookup_table_name='DOPaymentTerms')
	BEGIN
		SELECT @pmproduct_id = pmproduct_id FROM pmproduct WHERE code='SirSol'

		INSERT INTO pmproduct_lookup(pmproduct_id,lookup_table_name,edit_privilege_level,linked_caption_id,linked_class_name,linked_object_name,is_generic_maintenance,interface_component,interface_control)
		VALUES(@pmproduct_id,'DOPaymentTerms',3,NULL,NULL,NULL,0,NULL,NULL)
		
		EXEC spu_pm_caption_id_return 1,'Cash',@Caption_id Output
		INSERT INTO DOPaymentTerms ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'Cash','Cash',0,'2000-Jan-01')

		EXEC spu_pm_caption_id_return 1,'Debit Order',@Caption_id Output
		INSERT INTO DOPaymentTerms ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'DebitOrder','Debit Order',0,'2000-Jan-01')
	END

   	IF NOT EXISTS (SELECT NULL FROM pmproduct_lookup WHERE lookup_table_name='CollectionFrequency')
	BEGIN
		SELECT @pmproduct_id = pmproduct_id FROM pmproduct WHERE code='SirSol'

		INSERT INTO pmproduct_lookup(pmproduct_id,lookup_table_name,edit_privilege_level,linked_caption_id,linked_class_name,linked_object_name,is_generic_maintenance,interface_component,interface_control)
		VALUES(@pmproduct_id,'CollectionFrequency',3,NULL,NULL,NULL,0,NULL,NULL)
		
		EXEC spu_pm_caption_id_return 1,'Monthly',@Caption_id Output
		INSERT INTO CollectionFrequency ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'Monthly','Monthly',0,'2000-Jan-01')

		EXEC spu_pm_caption_id_return 1,'Quarterly',@Caption_id Output
		INSERT INTO CollectionFrequency ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'Quarterly','Quarterly',0,'2000-Jan-01')

		EXEC spu_pm_caption_id_return 1,'BI-Annually',@Caption_id Output
		INSERT INTO CollectionFrequency ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'BIAnnual','BI-Annually',0,'2000-Jan-01')

		EXEC spu_pm_caption_id_return 1,'Once Off',@Caption_id Output
		INSERT INTO CollectionFrequency ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'Once Off','Once Off',0,'2000-Jan-01')		
	END
END
GO
-- *****************************************************************************  
-- * Author:      Sandeep Kumar
-- * Date:        23/07/2012
-- * Purpose:     WPR106
-- *****************************************************************************
IF NOT EXISTS ( SELECT NULL FROM MakeLiveOptions WHERE code='INVOICE')
  BEGIN
    DECLARE @INVOICEcaption_id INT

    EXECUTE spu_pm_caption_id_return 1, 'Invoice', @INVOICEcaption_id OUTPUT

    INSERT INTO MakeLiveOptions
    (code,description,caption_id,effective_date,is_deleted)
    VALUES
    ('INVOICE','Invoice',@INVOICEcaption_id,GETDATE(),0)
  END
  GO
  IF NOT EXISTS ( SELECT NULL FROM MakeLiveOptions WHERE code='INST')
  BEGIN
    DECLARE @INSTcaption_id INT

    EXECUTE spu_pm_caption_id_return 1, 'Invoice', @INSTcaption_id OUTPUT

    INSERT INTO MakeLiveOptions
    (code,description,caption_id,effective_date,is_deleted)
    VALUES
    ('INST','Instalments',@INSTcaption_id,GETDATE(),0)
  END
  GO
  IF NOT EXISTS ( SELECT NULL FROM MakeLiveOptions WHERE code='BG')
  BEGIN
    DECLARE @BGcaption_id INT
    
    EXECUTE spu_pm_caption_id_return 1, 'Bank Guarantee', @BGcaption_id OUTPUT

    INSERT INTO MakeLiveOptions
    (code,description,caption_id,effective_date,is_deleted)
    VALUES
    ('BG','Bank Guarantee',@BGcaption_id,GETDATE(),0)
  END
  GO
  IF NOT EXISTS ( SELECT NULL FROM MakeLiveOptions WHERE code='PAYNOW')
  BEGIN
    DECLARE @PAYNOWcaption_id INT
    
    EXECUTE spu_pm_caption_id_return 1, 'Pay Now', @PAYNOWcaption_id OUTPUT

    INSERT INTO MakeLiveOptions
    (code,description,caption_id,effective_date,is_deleted)
    VALUES
    ('PAYNOW','Pay Now',@PAYNOWcaption_id,GETDATE(),0)
  END
  GO
  IF NOT EXISTS ( SELECT NULL FROM MakeLiveOptions WHERE code='CD')
  BEGIN
    DECLARE @CDcaption_id INT
    
    EXECUTE spu_pm_caption_id_return 1, 'Cash Deposit', @CDcaption_id OUTPUT

    INSERT INTO MakeLiveOptions
    (code,description,caption_id,effective_date,is_deleted)
    VALUES
    ('CD','Cash Deposit',@CDcaption_id,GETDATE(),0)
  END
  GO
  IF NOT EXISTS ( SELECT NULL FROM MakeLiveOptions WHERE code='MARKED')
  BEGIN
    DECLARE @MARKEDcaption_id INT
    
    EXECUTE spu_pm_caption_id_return 1, 'Mark for Collection', @MARKEDcaption_id OUTPUT

    INSERT INTO MakeLiveOptions
    (code,description,caption_id,effective_date,is_deleted)
    VALUES
    ('MARKED','Mark for Collection',@MARKEDcaption_id,GETDATE(),0)
  END
  GO
  
  IF NOT EXISTS (SELECT NULL FROM pmproduct_lookup WHERE lookup_table_name='MakeLiveOptions')
	BEGIN
		DECLARE @pmproduct_id INT
		DECLARE @caption_id INT
		SELECT @pmproduct_id = pmproduct_id FROM pmproduct WHERE code='SirSol'
		EXEC spu_pm_caption_id_return 1, 'MakeLiveOptions', @caption_id OUTPUT

		INSERT INTO pmproduct_lookup(pmproduct_id,lookup_table_name,edit_privilege_level,linked_caption_id,linked_class_name,linked_object_name,is_generic_maintenance,interface_component,interface_control)
		VALUES(@pmproduct_id,'MakeLiveOptions',3,@caption_id,NULL,NULL,0,NULL,NULL)
	END
  GO

-- *****************************************************************************  
-- * Author:      Vidya Rangdale (Etana)
-- * Date:        16/09/2014
-- * Purpose:     WPR113
-- *****************************************************************************

IF Not Exists (Select Null From Batch_Type Where Code = 'REN')
BEGIN
    Declare @lCaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'REN', @lCaptionID output
    
    Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type
    
    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @lCaptionID,
        0,
        GetDate(),
        'Batch Renewals',
        'REN'
    )
END
GO

IF Not Exists (Select Null From Batch_Renewal_Job_Run_Status Where Code = 'R')
BEGIN
    Declare @lCaptionID integer
    Execute spu_pm_caption_id_return 1, 'R', @lCaptionID output

    Declare @Status_Id integer
    Select @Status_Id = Max(Batch_Renewal_Job_Run_Status_Id) + 1 From Batch_Renewal_Job_Run_Status
            
	INSERT INTO  Batch_Renewal_Job_Run_Status
           (batch_renewal_job_run_status_id,
           caption_id,
           is_deleted,
           effective_date,
           description,
           code)
     VALUES
           (ISNull(@Status_Id, 1),
           @lCaptionID,
           0,
           GETDATE(),
           'Ready for processing',
           'R')

    Execute spu_pm_caption_id_return 1, 'P', @lCaptionID output
	Select @Status_Id = Max(Batch_Renewal_Job_Run_Status_Id) + 1 From Batch_Renewal_Job_Run_Status
	
	INSERT INTO  Batch_Renewal_Job_Run_Status
           (batch_renewal_job_run_status_id,
           caption_id,
           is_deleted,
           effective_date,
           description,
           code)
     VALUES
           (@Status_Id,
           @lCaptionID,
           0,
           GETDATE(),
           'Processing in progress',
           'P')
           
    Execute spu_pm_caption_id_return 1, 'CS', @lCaptionID output
	Select @Status_Id = Max(Batch_Renewal_Job_Run_Status_Id) + 1 From Batch_Renewal_Job_Run_Status
	        
	INSERT INTO  Batch_Renewal_Job_Run_Status
           (batch_renewal_job_run_status_id,
           caption_id,
           is_deleted,
           effective_date,
           description,
           code)
     VALUES
           (@Status_Id,
           @lCaptionID,
           0,
           GETDATE(),
           'Completed - Success',
           'CS')
           
    Execute spu_pm_caption_id_return 1, 'CF', @lCaptionID output
	Select @Status_Id = Max(Batch_Renewal_Job_Run_Status_Id) + 1 From Batch_Renewal_Job_Run_Status
	        
	INSERT INTO  Batch_Renewal_Job_Run_Status
           (batch_renewal_job_run_status_id,
           caption_id,
           is_deleted,
           effective_date,
           description,
           code)
     VALUES
           (@Status_Id,
           @lCaptionID,
           0,
           GETDATE(),
           'Completed - Failed',
           'CF')                                 
END
GO

-- *****************************************************************************
-- * Author:   Sravanti Pasumarti
-- * Date:     19/05/2026
-- * ADO #39411: Cross-Branch Client Search
-- * Purpose:  Add system option 5265 - Client Search independent to Branch Access
-- *****************************************************************************
INSERT INTO system_options (branch_id, option_number, value, description)
SELECT s.source_id, 5265, '0', 'Client Search independent to Branch Access'
FROM source s WITH(NOLOCK)
WHERE NOT EXISTS (
    SELECT 1 FROM system_options so WITH(NOLOCK)
    WHERE so.branch_id = s.source_id AND so.option_number = 5265
)
GO

-- *****************************************************************************


-- *****************************************************************************
-- * ADO #39336: Instalment for Claim Recovery - Transaction Type Mapping
-- * Purpose:  Add ICC/ICD transaction codes for claim recovery instalments
-- *****************************************************************************
DECLARE @icc_caption_id INT
DECLARE @icd_caption_id INT
DECLARE @icc_id INT
DECLARE @icd_id INT

IF NOT EXISTS (SELECT 1 FROM pfinstalments_transaction WHERE code = 'ICC')
BEGIN
    SELECT @icc_id = ISNULL(MAX(pfinstalments_transaction_id), 0) + 1 FROM pfinstalments_transaction
    EXECUTE spu_pm_caption_id_return 1, 'Instalment Claim Credit', @icc_caption_id OUTPUT
    INSERT INTO pfinstalments_transaction (pfinstalments_transaction_id, code, description, caption_id, effective_date, is_deleted)
    VALUES (@icc_id, 'ICC', 'Instalment Claim Credit', @icc_caption_id, GETDATE(), 0)
END

IF NOT EXISTS (SELECT 1 FROM pfinstalments_transaction WHERE code = 'ICD')
BEGIN
    SELECT @icd_id = ISNULL(MAX(pfinstalments_transaction_id), 0) + 1 FROM pfinstalments_transaction
    EXECUTE spu_pm_caption_id_return 1, 'Instalment Claim Debit', @icd_caption_id OUTPUT
    INSERT INTO pfinstalments_transaction (pfinstalments_transaction_id, code, description, caption_id, effective_date, is_deleted)
    VALUES (@icd_id, 'ICD', 'Instalment Claim Debit', @icd_caption_id, GETDATE(), 0)
END
GO

-- * End of File
-- *****************************************************************************


-- *****************************************************************************
-- * Author:   Saumitra Bhatnagar
-- * Date:     01/09/2014
-- * Purpose: WPR18 -Payement recommendation
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGCCLINK', 'SAM GetCashClaimLink'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGCCLINK'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCLMURS', 'SAM UpdateRecommendStatus'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMCLMURS'
GO
--
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMACCLINK', 'AddCashClaimLink'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMACCLINK '
GO

--
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMDOCLINK', 'SAM GetProductDocuments'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMDOCLINK'
GO 

IF Not Exists (Select 1 From Event_Type_Group Where Code = 'CLMRCOMD')
BEGIN
    Declare @nCaptionID integer
    Declare @nEvent_type_GroupId Integer

    Execute spu_pm_caption_id_return 1, 'CLMRCOMD', @nCaptionID output
    
    Select @nEvent_type_GroupId = Max(Event_type_Group_Id)+1 From Event_type_Group

    Insert into Event_Type_Group 
    (
        event_type_group_id,
        caption_id,
        code,
        description,
        is_deleted,
        effective_date
    )
    Values
    (
        @nEvent_type_GroupId,
        @nCaptionID,
        'CLMRCOMD',
        'Claim Recommend',
        0,
        GetDate()
    )

    Declare @nEvent_type_Id Integer
    Execute spu_pm_caption_id_return 1, 'CLMRCOMD', @nCaptionID output
    
    Select @nEvent_type_Id = Max(Event_type_Id)+1 From Event_type
    
    Insert into Event_Type
    (
        event_type_id,
        caption_id,
        code,
        description,
        is_deleted,
        effective_date,
        Event_type_Group_Id
    )
    Values
    (
        @nEvent_type_Id,
        @nCaptionID,
        'CLMRCOMD',
        'Claim Recommend',
        0,
        GetDate(),
        @nEvent_type_GroupId
    )
    END
GO

IF Not Exists (Select 1 From Event_Type_Group Where Code = 'CLMAUTHO')
BEGIN
    Declare @nCaptionID integer
    Declare @nEvent_type_GroupId Integer

    Execute spu_pm_caption_id_return 1, 'CLMAUTHO', @nCaptionID output
    
    Select @nEvent_type_GroupId = Max(Event_type_Group_Id)+1 From Event_type_Group

    Insert into Event_Type_Group 
    (
        event_type_group_id,
        caption_id,
        code,
        description,
        is_deleted,
        effective_date
    )
    Values
    (
        @nEvent_type_GroupId,
        @nCaptionID,
        'CLMAUTHO',
        'Claim Authorise',
        0,
        GetDate()
    )

    Declare @nEvent_type_Id Integer
    Execute spu_pm_caption_id_return 1, 'CLMAUTHO', @nCaptionID output
    
    Select @nEvent_type_Id = Max(Event_type_Id)+1 From Event_type
    
    Insert into Event_Type
    (
        event_type_id,
        caption_id,
        code,
        description,
        is_deleted,
        effective_date,
        Event_type_Group_Id
    )
    Values
    (
        @nEvent_type_Id,
        @nCaptionID,
        'CLMAUTHO',
        'Claim Authorise',
        0,
        GetDate(),
        @nEvent_type_GroupId
    )
    END
GO

IF Not Exists (Select 1 From Event_Type_Group Where Code = 'CLMDECLN')
BEGIN
    Declare @nCaptionID integer
    Declare @nEvent_type_GroupId Integer

    Execute spu_pm_caption_id_return 1, 'CLMDECLN', @nCaptionID output
    
    Select @nEvent_type_GroupId = Max(Event_type_Group_Id)+1 From Event_type_Group

    Insert into Event_Type_Group 
    (
        event_type_group_id,
        caption_id,
        code,
        description,
        is_deleted,
        effective_date
    )
    Values
    (
        @nEvent_type_GroupId,
        @nCaptionID,
        'CLMDECLN',
        'Claim Decline',
        0,
        GetDate()
    )

    Declare @nEvent_type_Id Integer
    Execute spu_pm_caption_id_return 1, 'CLMDECLN', @nCaptionID output

    Select @nEvent_type_Id = Max(Event_type_Id)+1 From Event_type

    Insert into Event_Type
    (
        event_type_id,
        caption_id,
        code,
        description,
        is_deleted,
        effective_date,
        Event_type_Group_Id
    )
    Values
    (
        @nEvent_type_Id,
        @nCaptionID,
        'CLMDECLN',
        'Claim Decline',
        0,
        GetDate(),
        @nEvent_type_GroupId
    )
    END
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGPCLDET', 'GetCashPaymentDetails'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SYSADMIN', 'SAMGPCLDET'
GO 


Update Debtor_User_Groups Set Is_Payment_Type_Claim_Payment = 1
	From Debtor_User_Groups DUG
		Inner Join Debtor_User_Groups_Type DUGT ON DUGT.Debtor_User_Groups_Type_Id = DUG.Debtor_User_Groups_Type_Id
			Where DUGT.code = 'RPG' AND Is_Payment_Type_Claim_Payment IS NULL
GO
-- *****************************************************************************
-- * Author:   Vijay Pal
-- * Date:     18/05/2015
-- * Purpose: Enhancement 10533
-- *****************************************************************************

IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
                DECLARE @pmproduct_id INT
                DECLARE @caption_id INT


                IF NOT EXISTS (SELECT NULL FROM pmproduct_lookup WHERE lookup_table_name='RI_Version_Type')
                BEGIN
                                SELECT @pmproduct_id = pmproduct_id FROM pmproduct WHERE code='SirSol'

                                INSERT INTO pmproduct_lookup(pmproduct_id,lookup_table_name,edit_privilege_level,linked_caption_id,linked_class_name,linked_object_name,is_generic_maintenance,interface_component,interface_control)
                                VALUES(@pmproduct_id,'RI_Version_Type',3,NULL,NULL,NULL,0,NULL,NULL)
                                
                                EXEC spu_pm_caption_id_return 1,'Underwriting',@Caption_id Output
                                INSERT INTO RI_Version_Type ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'Underwriting','Underwriting',0,'2000-Jan-01')

                                EXEC spu_pm_caption_id_return 1,'Portfolio Transfer',@Caption_id Output
                                INSERT INTO RI_Version_Type ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'PT','Portfolio Transfer',0,'2000-Jan-01')
                                                                
                END
END

UPDATE RI_Arrangement SET RI_Version_Type_id =1 Where RI_Version_Type_id  IS NULL

UPDATE RI_Arrangement SET Effective_Date =inception_date from risk 
                WHERE Effective_Date Is Null and risk.risk_cnt =RI_Arrangement.risk_cnt            

GO
-- *****************************************************************************
-- * Author:   Vidya Rangdale
-- * Date:     18/06/2015
-- * Purpose: WPR 46 Issue No 11827
-- *****************************************************************************
IF NOT EXISTS (SELECT * FROM Process_Type Where code = 'RNA') 
BEGIN
	DECLARE @caption_id int
	DECLARE @Process_types_id int
        EXEC spu_pm_caption_id_return 1, 'RNA', @caption_id OUTPUT
		SELECT @Process_types_id= ISNULL(MAX(process_type_id),0)+1 FROM Process_Type
        -- Add new New Business Description
        INSERT Process_Type(process_type_id, caption_id,code,description,is_deleted, effective_date,is_editable_after_merging,Functional_Area )
            VALUES(@Process_types_id, @caption_id, 'RNA','Renewal Amendment',0, getdate(),0,1)
END


-- *****************************************************************************
-- * Author:   Anurag Gupta
-- * Date:     18/06/2015
-- * Purpose: Pure 4.0 Issue No 11836
-- *****************************************************************************

UPDATE Product SET Unified_Renewal_Date_Is_Read_Only = 0  Where Unified_Renewal_Date_Is_Read_Only  IS NULL
GO
IF EXISTS(SELECT NULL FROM risk_type_ri_properties where risk_type_id Is NOT Null and risk_type_ri_limit_version_id  is null)
BEGIN

DECLARE @risk_type_id INT
DECLARE @risk_type_ri_limit_version_id INT

DECLARE UPD_RI_LIMITCURSOR CURSOR FAST_FORWARD FOR
SELECT DISTINCT risk_type_id  from risk_type_ri_properties where risk_type_ri_limit_version_id  is null

OPEN UPD_RI_LIMITCURSOR

FETCH NEXT FROM UPD_RI_LIMITCURSOR INTO @risk_type_id

WHILE @@FETCH_STATUS=0
BEGIN
	INSERT INTO [Risk_Type_RI_Limit_Version] (  
		risk_type_id ,  
		description  ,  
		ri_limit_start_date,
		ri_limit_end_date )  
	VALUES (  
		@risk_type_id,  
		'RILimit',  
		'2010-01-01',
		'2014-06-30')  

	SELECT @risk_type_ri_limit_version_id = @@IDENTITY

	UPDATE risk_type_ri_properties SET risk_type_ri_limit_version_id = @risk_type_ri_limit_version_id where risk_type_id =@risk_type_id
	
	UPDATE Risk_Type_RI_Values SET risk_type_ri_limit_version_id = @risk_type_ri_limit_version_id where risk_type_id =@risk_type_id
	
	FETCH NEXT FROM UPD_RI_LIMITCURSOR INTO @risk_type_id
END

CLOSE UPD_RI_LIMITCURSOR
DEALLOCATE UPD_RI_LIMITCURSOR

END
GO
-- *****************************************************************************  
-- * Author:      Azeej Usmani
-- * Date:        16/1/2013			
-- * Purpose:     wpr 100(a)
-- *****************************************************************************
IF NOT EXISTS(SELECT null FROM Date_for_Treaty_XOL_Calculation WHERE code='ACCOUNTYR')
BEGIN
	DECLARE @caption_id  INT
	EXECUTE spu_pm_caption_id_return 1, 'Accounting Year', @caption_id OUTPUT
    INSERT INTO Date_for_Treaty_XOL_Calculation (Date_for_Treaty_XOL_Calculation_id,code,Description,caption_id,effective_date,is_deleted)
    VALUES(3,'ACCOUNTYR','Accounting Year',@caption_id,GETDATE(),0)
END

GO

-- *****************************************************************************  
-- * Author:      Azeej Usmani
-- * Date:        16/2/2013			
-- * Purpose:     wpr 100(b)
-- *****************************************************************************
UPDATE ri_arrangement set version_id =1 where version_id is null

UPDATE ri_arrangement set pro_rata_rate=1 where pro_rata_rate is null

IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
	DECLARE @pmproduct_id INT
	DECLARE @caption_id INT


   	IF NOT EXISTS (SELECT NULL FROM pmproduct_lookup WHERE lookup_table_name='Risk_RI_Status')
	BEGIN
		SELECT @pmproduct_id = pmproduct_id FROM pmproduct WHERE code='SirSol'

		INSERT INTO pmproduct_lookup(pmproduct_id,lookup_table_name,edit_privilege_level,linked_caption_id,linked_class_name,linked_object_name,is_generic_maintenance,interface_component,interface_control)
		VALUES(@pmproduct_id,'Risk_RI_Status',3,NULL,NULL,NULL,0,NULL,NULL)
		
		EXEC spu_pm_caption_id_return 1,'Failed',@Caption_id Output
		INSERT INTO Risk_RI_Status ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'Failed','Failed',0,'2000-Jan-01')

		EXEC spu_pm_caption_id_return 1,'RI Calculated',@Caption_id Output
		INSERT INTO Risk_RI_Status ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'RICalculated','RI Calculated',0,'2000-Jan-01')
		
		EXEC spu_pm_caption_id_return 1,'Posted',@Caption_id Output
		INSERT INTO Risk_RI_Status ( Caption_Id, Code, Description, Is_Deleted,Effective_Date)VALUES(@Caption_id,'Posted','Posted',0,'2000-Jan-01')
	END
END

GO
-- *****************************************************************************
-- * Author:   Sahil Ansari
-- * Date:     21/07/2015
-- * Purpose:  WPR 100 b
-- *****************************************************************************
DECLARE @caption_id INT
DECLARE @MaxDocumentTypeId INT
EXECUTE spu_pm_caption_id_return 1, 'Clone Reversal', @caption_id output

IF NOT EXISTS(SELECT * FROM DocumentType WHERE code='SDR')    
BEGIN
 SELECT @MaxDocumentTypeId = MAX(documenttype_id ) + 1 FROM DocumentType
 
	INSERT INTO DocumentType(documenttype_id, caption_id, doctypegroup_id, is_deleted, effective_date,description, code, from_sirius) 
	VALUES(@MaxDocumentTypeId, @caption_id, 2, 0, getdate(), 'Clone Reversal', 'SDR', 1)


SELECT @caption_id = caption_id    FROM pmcaption WHERE caption = 'Document Reference'

IF NOT EXISTS (SELECT * FROM actnumber_group WHERE code in(SELECT 'DOCREF' + CONVERT(VARCHAR(2),DT.documenttype_id)	FROM documenttype DT WHERE dt.code='SDR' )) 
Begin
IF EXISTS (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_group')
SET IDENTITY_INSERT actnumber_group ON
	INSERT INTO actnumber_group
 		(		
		actnumber_group_id,
     		code,
	        caption_id,
	        description,
        	is_reset_yearly,
	        is_deleted,
        	effective_date
	 	)
	SELECT
		@MaxDocumentTypeId,
		'DOCREF' + CONVERT(VARCHAR(2),DT.documenttype_id),
        	@caption_id,
        	'Document Reference',
        	0,
        	0,
        	'2002-10-08'
		FROM documenttype DT
    		WHERE dt.code='SDR'
IF EXISTS (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_group')
SET IDENTITY_INSERT actnumber_group OFF
END
END

	IF NOT EXISTS(SELECT * FROM actnumber_range where code in(SELECT DT.code FROM documenttype DT WHERE dt.code='SDR' )) 
BEGIN
IF EXISTS (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_range')
SET IDENTITY_INSERT actnumber_range ON
	INSERT INTO actnumber_range
    		(	
			actnumber_range_id,
        		code,
        		actnumber_group_id,
        		description
    		)
	SELECT
		@MaxDocumentTypeId,
		DT.code,
        	DT.documenttype_id,
        	DT.code
		FROM documenttype DT
    		WHERE dt.code='SDR'
IF EXISTS (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_range')
SET IDENTITY_INSERT actnumber_range OFF
END
GO


DECLARE @caption_id INT
DECLARE @MaxDocumentTypeId INT
EXECUTE spu_pm_caption_id_return 1, 'Claim Clone Reversal', @caption_id output

IF NOT EXISTS(SELECT * FROM DocumentType WHERE code='CLC')    BEGIN
 SELECT @MaxDocumentTypeId = MAX(documenttype_id ) + 1 FROM DocumentType
 
	INSERT INTO DocumentType(documenttype_id, caption_id, doctypegroup_id, is_deleted, effective_date,description, code, from_sirius) 
	VALUES(@MaxDocumentTypeId, @caption_id, 2, 0, getdate(), 'Claim Clone Reversal', 'CLC', 1)


SELECT @caption_id = caption_id    FROM pmcaption WHERE caption = 'Document Reference'

IF NOT EXISTS (SELECT * FROM actnumber_group WHERE code in(SELECT 'DOCREF' + CONVERT(VARCHAR(2),DT.documenttype_id)	FROM documenttype DT WHERE dt.code='CLC' )) 
Begin
IF EXISTS (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_group')
SET IDENTITY_INSERT actnumber_group ON
	INSERT INTO actnumber_group
 		(		
		actnumber_group_id,
     		code,
	        caption_id,
	        description,
        	is_reset_yearly,
	        is_deleted,
        	effective_date
	 	)
	SELECT
		@MaxDocumentTypeId,
		'DOCREF' + CONVERT(VARCHAR(2),DT.documenttype_id),
        	@caption_id,
        	'Document Reference',
        	0,
        	0,
        	'2002-10-08'
		FROM documenttype DT
    		WHERE dt.code='CLC'
IF EXISTS (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_group')
SET IDENTITY_INSERT actnumber_group OFF
END

	IF NOT EXISTS(SELECT * FROM actnumber_range where code in(SELECT DT.code FROM documenttype DT WHERE dt.code='CLC' )) 
BEGIN
IF EXISTS (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_range')
SET IDENTITY_INSERT actnumber_range ON
	INSERT INTO actnumber_range
    		(	
			actnumber_range_id,
        		code,
        		actnumber_group_id,
        		description
    		)
	SELECT
		@MaxDocumentTypeId,
		DT.code,
        	DT.documenttype_id,
        	DT.code
		FROM documenttype DT
    		WHERE dt.code='CLC'
IF EXISTS (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_range')
SET IDENTITY_INSERT actnumber_range OFF
END
END
GO



DECLARE @caption_id INT
DECLARE @MaxDocumentTypeId INT
EXECUTE spu_pm_caption_id_return 1, 'Claim Clone Process', @caption_id output

IF NOT EXISTS(SELECT * FROM DocumentType WHERE code='CLD')    BEGIN
 SELECT @MaxDocumentTypeId = MAX(documenttype_id ) + 1 FROM DocumentType
 
	INSERT INTO DocumentType(documenttype_id, caption_id, doctypegroup_id, is_deleted, effective_date,description, code, from_sirius) 
	VALUES(@MaxDocumentTypeId, @caption_id, 2, 0, getdate(), 'Claim Clone Process', 'CLD', 1)


SELECT @caption_id = caption_id    FROM pmcaption WHERE caption = 'Document Reference'

IF NOT EXISTS (SELECT * FROM actnumber_group WHERE code in(SELECT 'DOCREF' + CONVERT(VARCHAR(2),DT.documenttype_id)	FROM documenttype DT WHERE dt.code='CLD' )) 
Begin
IF EXISTS (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_group')
SET IDENTITY_INSERT actnumber_group ON
	INSERT INTO actnumber_group
 		(		
		actnumber_group_id,
     		code,
	        caption_id,
	        description,
        	is_reset_yearly,
	        is_deleted,
        	effective_date
	 	)
	SELECT
		@MaxDocumentTypeId,
		'DOCREF' + CONVERT(VARCHAR(2),DT.documenttype_id),
        	@caption_id,
        	'Document Reference',
        	0,
        	0,
        	'2002-10-08'
		FROM documenttype DT
    		WHERE dt.code='CLD'
IF EXISTS (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_group')
SET IDENTITY_INSERT actnumber_group OFF
END

	IF NOT EXISTS(SELECT * FROM actnumber_range where code in(SELECT DT.code FROM documenttype DT WHERE dt.code='CLD' )) 
BEGIN
IF EXISTS (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_range')
SET IDENTITY_INSERT actnumber_range ON
	INSERT INTO actnumber_range
    		(	
			actnumber_range_id,
        		code,
        		actnumber_group_id,
        		description
    		)
	SELECT
		@MaxDocumentTypeId,
		DT.code,
        	DT.documenttype_id,
        	DT.code
		FROM documenttype DT
    		WHERE dt.code='CLD'
IF EXISTS (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_range')
SET IDENTITY_INSERT actnumber_range OFF
END
END
GO

DECLARE @caption_id INT
DECLARE @MaxDocumentTypeId INT
EXECUTE spu_pm_caption_id_return 1, 'Claim Portfolio Transfer', @caption_id output

IF NOT EXISTS(SELECT * FROM DocumentType WHERE code='CPA')    BEGIN
 SELECT @MaxDocumentTypeId = MAX(documenttype_id ) + 1 FROM DocumentType
 
	INSERT INTO DocumentType(documenttype_id, caption_id, doctypegroup_id, is_deleted, effective_date,description, code, from_sirius) 
	VALUES(@MaxDocumentTypeId, @caption_id, 2, 0, getdate(), 'Claim Portfolio Transfer', 'CPA', 1)


SELECT @caption_id = caption_id    FROM pmcaption WHERE caption = 'Document Reference'

IF NOT EXISTS (SELECT * FROM actnumber_group WHERE code in(SELECT 'DOCREF' + CONVERT(VARCHAR(2),DT.documenttype_id)	FROM documenttype DT WHERE dt.code='CPA' )) 
Begin
IF EXISTS (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_group')
SET IDENTITY_INSERT actnumber_group ON
	INSERT INTO actnumber_group
 		(		
		actnumber_group_id,
     		code,
	        caption_id,
	        description,
        	is_reset_yearly,
	        is_deleted,
        	effective_date
	 	)
	SELECT
		@MaxDocumentTypeId,
		'DOCREF' + CONVERT(VARCHAR(2),DT.documenttype_id),
        	@caption_id,
        	'Document Reference',
        	0,
        	0,
        	'2002-10-08'
		FROM documenttype DT
    		WHERE dt.code='CPA'
IF EXISTS (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_group')
SET IDENTITY_INSERT actnumber_group OFF
END

	IF NOT EXISTS(SELECT * FROM actnumber_range where code in(SELECT DT.code FROM documenttype DT WHERE dt.code='CPA' )) 
BEGIN
IF EXISTS (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_range')
SET IDENTITY_INSERT actnumber_range ON
	INSERT INTO actnumber_range
    		(	
			actnumber_range_id,
        		code,
        		actnumber_group_id,
        		description
    		)
	SELECT
		@MaxDocumentTypeId,
		DT.code,
        	DT.documenttype_id,
        	DT.code
		FROM documenttype DT
    		WHERE dt.code='CPA'
IF EXISTS (SELECT TABLE_NAME + '.' + COLUMN_NAME AS COLUMN_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND TABLE_NAME='ACTNumber_range')
SET IDENTITY_INSERT actnumber_range OFF
END
END
GO


IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U') 
BEGIN
    DECLARE @report_id INT
    DECLARE @lCaptionID INT
    DECLARE @group_id INT

    -- Daily Audit Report Journals With Payments report
    SELECT @report_id = report_id FROM report WHERE code = 'PT_TRANS'

    IF(ISNULL(@report_id,0) = 0)
    BEGIN    
        -- Add report
        SELECT @report_id = MAX(report_id)+1 FROM report
        EXECUTE spu_pm_caption_id_return 1, 'Portfolio Transfer Report', @lCaptionID OUTPUT
        INSERT INTO report(report_id, caption_id, code, description, is_deleted, effective_date, report_name) 
               VALUES(@report_id, @lCaptionID, 'PT_TRANS', 'Portfolio Transfer Report', 0, GETDATE(), 'Portfolio_Transfer_Report.rpt')

        -- Link report to group
        SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'ALL'
        IF(ISNULL(@group_id,0)<>0)
            INSERT INTO Report_Group_Contents(report_group_id,report_id) VALUES(@group_id,@report_id)
    END
END
GO

IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U') 
BEGIN
    DECLARE @report_id INT
    DECLARE @lCaptionID INT
    DECLARE @group_id INT

    -- Daily Audit Report Journals With Payments report
    SELECT @report_id = report_id FROM report WHERE code = 'CT_TRANS'

    IF(ISNULL(@report_id,0) = 0)
    BEGIN    
        -- Add report
        SELECT @report_id = MAX(report_id)+1 FROM report
        EXECUTE spu_pm_caption_id_return 1, 'Clone Transfer Report', @lCaptionID OUTPUT
        INSERT INTO report(report_id, caption_id, code, description, is_deleted, effective_date, report_name) 
               VALUES(@report_id, @lCaptionID, 'CT_TRANS', 'Clone Transfer Report', 0, GETDATE(), 'Clone_Transfer_Report.rpt')

        -- Link report to group
        SELECT @group_id = report_group_id FROM Report_Group WHERE code = 'ALL'
        IF(ISNULL(@group_id,0)<>0)
            INSERT INTO Report_Group_Contents(report_group_id,report_id) VALUES(@group_id,@report_id)
    END
END
GO

-- *****************************************************************************
-- * Author:	Sandeep Kumar
-- * Date:		17/08/2015
-- * Purpose:	WPR84-Paralleling 
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUPDCRP', 'SAM Find Policy Transaction Grouped' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMUPDCRP'
GO  
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMACCTDET', 'SAM Get Allocation Details' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMACCTDET'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMREVALOB', 'SAM ReverseAllocationBatch' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMREVALOB'
GO
-- *****************************************************************************


-- *****************************************************************************
-- * ADO #39336: Instalment for Claim Recovery - Transaction Type Mapping
-- * Purpose:  Add ICC/ICD transaction codes for claim recovery instalments
-- *****************************************************************************
DECLARE @icc_caption_id INT
DECLARE @icd_caption_id INT
DECLARE @icc_id INT
DECLARE @icd_id INT

IF NOT EXISTS (SELECT 1 FROM pfinstalments_transaction WHERE code = 'ICC')
BEGIN
    SELECT @icc_id = ISNULL(MAX(pfinstalments_transaction_id), 0) + 1 FROM pfinstalments_transaction
    EXECUTE spu_pm_caption_id_return 1, 'Instalment Claim Credit', @icc_caption_id OUTPUT
    INSERT INTO pfinstalments_transaction (pfinstalments_transaction_id, code, description, caption_id, effective_date, is_deleted)
    VALUES (@icc_id, 'ICC', 'Instalment Claim Credit', @icc_caption_id, GETDATE(), 0)
END

IF NOT EXISTS (SELECT 1 FROM pfinstalments_transaction WHERE code = 'ICD')
BEGIN
    SELECT @icd_id = ISNULL(MAX(pfinstalments_transaction_id), 0) + 1 FROM pfinstalments_transaction
    EXECUTE spu_pm_caption_id_return 1, 'Instalment Claim Debit', @icd_caption_id OUTPUT
    INSERT INTO pfinstalments_transaction (pfinstalments_transaction_id, code, description, caption_id, effective_date, is_deleted)
    VALUES (@icd_id, 'ICD', 'Instalment Claim Debit', @icd_caption_id, GETDATE(), 0)
END
GO

-- * End of File
-- *****************************************************************************

--*****************************************************************************
-- * Author:     Joginder Sharma
-- * Date:       24/04/2015
-- * Purpose:  	 RFC02-Log Claims Outside of Policy Period
-- *****************************************************************************
DECLARE @bExistsAttachClaimOutside TINYINT
EXECUTE @bExistsAttachClaimOutside = DDLExistsColumn 'Risk_Type', 'Attach_Claim_Outside_Of_Policy_Period'
IF @bExistsAttachClaimOutside = 1 
 BEGIN
 update risk_type set Attach_Claim_Outside_Of_Policy_Period =0 where Attach_Claim_Outside_Of_Policy_Period is null 
 END
 GO

 
-- *****************************************************************************  
-- * Author:     Anshul Jha
-- * Date:       14 Aug 2014
-- * Purpose:    WPR13 | Parallelling
-- *****************************************************************************  
DECLARE @lCaptionID  INT
DECLARE @TaskGroupId INT
DECLARE @TaskId      INT

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task WHERE code = 'EXTWCONFIG')
    BEGIN
    EXECUTE spu_pm_caption_id_return 1, 'External Workflow Configuration', @lCaptionID OUTPUT

    INSERT INTO PMWrk_Task 
                          (caption_id, code, description, is_deleted, effective_date, 
         is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, 
         auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, 
         linked_caption_id, is_available_task, pmwrk_task_category_id
           ) 

       Values
      (@lCaptionID, 'EXTWCONFIG', 'External Workflow Configuration', 0, 
      GETDATE(), 0, 1, NULL, 'iSIRExternalWorkflowConfig','NavigatorV3', 0, 40, 0, NULL, NULL, NULL, 1, 2
    )
    
    END
 

    -- Create link to group for this new task
    SELECT @TaskId = ( SELECT pmwrk_task_id FROM pmwrk_task WHERE code = 'EXTWCONFIG')

    -- Add task to correct group
    SELECT @TaskGroupId = ( SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'COMMON')

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task_Group_Task WHERE pmwrk_task_group_id = @TaskGroupId AND pmwrk_task_id  = @TaskId)
    BEGIN
       INSERT INTO pmwrk_task_group_task
       (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num)
       VALUES
       (@TaskGroupId, @TaskId, 0)
    END


GO


-- *****************************************************************************
-- * Author:     Anshul Jha
-- * Date:       14 Aug 2014
-- * Purpose:    WPR13 | Parallelling
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGTOKey', 'GetTaskOnKeys'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGTOKey'
GO

Go


-- *****************************************************************************
-- * Author:     Anshul Jha
-- * Date:       14 Aug 2014
-- * Purpose:    WPR13 | Parallelling to remove the CUD SP For UID Updations
-- *============================================================================
If exists(
          select 1 from GIS_Object where gis_object_id not in (
                                                                select GIS_Object_id from GIS_Property WHERE property_name ='UID') 
																and object_name not in ('work_claim','work_claim_peril')
		)

        BEGIN

				
				Declare @sprocName varchar(500)
				Declare curDropCUDSP cursor For 

				Select 'DDLDropProcedure ' +  Name As ProcName  from sys.procedures where name like '%_cud' 
				and name not like '%_Claim_cud' and name not like '%_Claim_Peril_cud'

		OPEN curDropCUDSP

				FETCH Next From  curDropCUDSP into @sprocName

				while @@fetch_status = 0

				begin

					Execute(@sprocName)

					FETCH NEXT FROM curDropCUDSP into @sprocName

				End

		Close curDropCUDSP

		Deallocate curDropCUDSP

       END

Go


-- *****************************************************************************  
-- * Author:     Anshul Jha
-- * Date:       14 Aug 2014
-- * Purpose:    WPR13 | Parallelling to remove the CUD SP For UID INSERTIN For OLD GISObjects
-- *============================================================================

DECLARE Curgis CURSOR FOR 
Select  GIS_Object_id,Table_name  from GIS_Object
Where  
object_name not in ('work_claim','work_claim_peril') And 
gis_object_id Not  in (
Select Distinct gis_object_id From GIS_Property where column_name='UID'
) 
										


Declare @nCount As Integer
Declare @nmaxcnt As Integer
Declare @GISTableName As Varchar(255)
Declare @GISTable_ID As Varchar(255)

OPEN Curgis


FETCH NEXT FROM Curgis  into  @GISTable_ID,@GISTableName


WHILE @@FETCH_STATUS = 0

BEGIN
BEGIN TRAN Trn

														  
Select @nmaxcnt=Max(ISNUll(gis_property_id,0))+1 from gis_property
															
																													
IF NOT EXISTS (SELECT NULL FROM GIS_Property WHERE gis_object_id = @GISTable_ID and column_name = 'UID')

Begin		
IF EXISTS (SELECT NULL FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @GISTableName) BEGIN																													 
	INSERT INTO dbo.GIS_Property ( gis_property_id, gis_object_id, property_name, column_name, data_type,
	is_input_property, is_identifying_property, is_primary_key, polaris_property_id,
	is_deleted, is_search_property, index_linking_id, Edit_Flags,
	Specials_Type, Specials_Type_Reference, is_in_mis_export,
	is_formatted_text, is_chase_cycle_property
	)
	VALUES  
	(@nmaxcnt, @GISTable_ID, 'UID', 'UID', 5, 0, 0, 0, NULL, 0, NULL, NULL, 0, 0, '', NULL, NULL, NULL)

	--Case Alter Table Name

	EXEC DDLAddColumn @sTableName=@GISTableName, @sColumnName ='UID',@sColumnDefinition='varchar(255) NULL'
END

End

FETCH NEXT FROM Curgis  into  @GISTable_ID,@GISTableName

COMMIT TRAN Trn;


END


CLOSE Curgis

DEALLOCATE Curgis

go

IF EXISTS(SELECT 1 FROM wp_fields WHERE sql='sp_wp_insurancefileall')
BEGIN
    UPDATE wp_fields SET sql='spu_wp_insurancefileall' WHERE sql='sp_wp_insurancefileall'
END

GO

-- *****************************************************************************  
-- * Author:     Sahil Ansari
-- * Date:       7 Feb 2016
-- * Purpose:    SEPA
-- *============================================================================

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partybankdetails' AND Column_name='business_identifier_code')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family,loop1)
    VALUES ('BIC', 'spu_wp_partybankdetails', 'business_identifier_code', 0, 'Party', 'BankDetails', 'BIC', 1, 9,'partybankdetails')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL='spu_wp_partybankdetails' AND Column_name='international_bank_account_number')
BEGIN
    INSERT INTO wp_fields (field_name, sql,column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family,loop1)
    VALUES ('IBAN', 'spu_wp_partybankdetails', 'international_bank_account_number', 0, 'Party', 'BankDetails', 'IBAN', 1, 9,'partybankdetails')
END
GO

--*****************************************************************************
-- * Author:    samarjeet Singh
-- * Date:       07/11/2015
-- * Purpose:  	 PM040450
-- * Comment:    this is being paralleled from Arch2.1
-- *****************************************************************************
IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'PartyBankName')
BEGIN
INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,product_family,loop1)
VALUES('PartyBankName','spu_wp_partyBankDetails','Bank_name', 0,'Party','BankDetails','Bank Name',1,9,'partybankdetails')
END
GO

--*****************************************************************************
-- * Author:     Joginder 
-- * Date:       24/05/2016
-- * Purpose:  	 TFS-15783
-- * Comment:    Adding tow new fields (“Agent Commission Tax Type” and “Agent commission Tax Value”) 
-- *****************************************************************************
IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'AgentCommissionTaxType')
BEGIN
INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)  
VALUES('AgentCommissionTaxType','spu_wp_GetCommissionBreakdown','Commission_Tax_Description', 0,'Policy','Agent','Agent Commission Tax type',1,'GetCommissionBreakdown',9)  
END

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name = 'AgentCommissionTaxValue')
BEGIN
INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)  
VALUES('AgentCommissionTaxValue','spu_wp_GetCommissionBreakdown','Agent_Commission_Tax_Value', 0,'Policy','Agent','Agent Commission Tax Value',1,'GetCommissionBreakdown',9)  
END
GO


-- *****************************************************************************  
-- * Author:     Anshul Jha
-- * Date:       06 Nov 2015
-- * Purpose:    E5 | Parallelling

DECLARE @lCaptionID  INT
DECLARE @TaskGroupId INT
DECLARE @TaskId      INT

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task WHERE code = 'EXTWCONFIG')
    BEGIN
    EXECUTE spu_pm_caption_id_return 1, 'External Workflow Configuration', @lCaptionID OUTPUT

    INSERT INTO PMWrk_Task 
                          (caption_id, code, description, is_deleted, effective_date, 
         is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, 
         auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, 
         linked_caption_id, is_available_task, pmwrk_task_category_id
           ) 

       Values
      (@lCaptionID, 'EXTWCONFIG', 'External Workflow Configuration', 0, 
      GETDATE(), 0, 1, NULL, 'iSIRExternalWorkflowConfig','NavigatorV3', 0, 40, 0, NULL, NULL, NULL, 1, 2
    )
    
    END
 

    -- Create link to group for this new task
    SELECT @TaskId = ( SELECT pmwrk_task_id FROM pmwrk_task WHERE code = 'EXTWCONFIG')

    -- Add task to correct group
    SELECT @TaskGroupId = ( SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code = 'COMMON')

    IF NOT EXISTS (SELECT NULL FROM PMWrk_Task_Group_Task WHERE pmwrk_task_group_id = @TaskGroupId AND pmwrk_task_id  = @TaskId)
    BEGIN
       INSERT INTO pmwrk_task_group_task
       (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num)
       VALUES
       (@TaskGroupId, @TaskId, 0)
    END


GO


-- *****************************************************************************
-- * Author:     Anshul Jha
-- * Date:       06 Nov 2015
-- * Purpose:    E5 | Parallelling
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGTOKey', 'GetTaskOnKeys'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGTOKey'
GO

Go


-- *****************************************************************************
-- * Author:     Anshul Jha
-- * Date:       06 Nov 2015
-- * Purpose:    E5 | Parallelling to remove the CUD SP For UID Updations
-- *============================================================================
If exists(
          select 1 from GIS_Object where gis_object_id not in (
                                                                select GIS_Object_id from GIS_Property WHERE property_name ='UID') 
																and object_name not in ('work_claim','work_claim_peril')
		)

        BEGIN

				
				Declare @sprocName varchar(500)
				Declare curDropCUDSP cursor For 

				Select 'DDLDropProcedure ' +  Name As ProcName  from sys.procedures where name like '%_cud' 
				and name not like '%_Claim_cud' and name not like '%_Claim_Peril_cud'

		OPEN curDropCUDSP

				FETCH Next From  curDropCUDSP into @sprocName

				while @@fetch_status = 0

				begin

					Execute(@sprocName)

					FETCH NEXT FROM curDropCUDSP into @sprocName

				End

		Close curDropCUDSP

		Deallocate curDropCUDSP

       END

Go


-- *****************************************************************************  
-- * Author:     Anshul Jha
-- * Date:       06 Nov 2015
-- * Purpose:    E5 | Parallelling to remove the CUD SP For UID INSERTIN For OLD GISObjects
-- *============================================================================

DECLARE Curgis CURSOR FOR 
Select  GIS_Object_id,Table_name  from GIS_Object
Where  
object_name not in ('work_claim','work_claim_peril') And 
gis_object_id Not  in (
Select Distinct gis_object_id From GIS_Property where column_name='UID'
) 
										


Declare @nCount As Integer
Declare @nmaxcnt As Integer
Declare @GISTableName As Varchar(255)
Declare @GISTable_ID As Varchar(255)

OPEN Curgis


FETCH NEXT FROM Curgis  into  @GISTable_ID,@GISTableName


WHILE @@FETCH_STATUS = 0

BEGIN
BEGIN TRAN Trn

														  
Select @nmaxcnt=Max(ISNUll(gis_property_id,0))+1 from gis_property
															
																												
IF NOT EXISTS (SELECT NULL FROM GIS_Property WHERE gis_object_id = @GISTable_ID and column_name = 'UID')

BEGIN		
	IF Exists (SELECT NULL from INFORMATION_SCHEMA.TABLES where TABLE_NAME = @GISTableName) 
	BEGIN
		INSERT INTO dbo.GIS_Property ( gis_property_id, gis_object_id, property_name, column_name, data_type,
		is_input_property, is_identifying_property, is_primary_key, polaris_property_id,
		is_deleted, is_search_property, index_linking_id, Edit_Flags,
		Specials_Type, Specials_Type_Reference, is_in_mis_export,
		is_formatted_text, is_chase_cycle_property
		)
		VALUES  
		(@nmaxcnt, @GISTable_ID, 'UID', 'UID', 5, 0, 0, 0, NULL, 0, NULL, NULL, 0, 0, '', NULL, NULL, NULL)

		--Case Alter Table Name
		EXEC DDLAddColumn @sTableName=@GISTableName, @sColumnName ='UID',@sColumnDefinition='varchar(255) NULL'
	END

END

FETCH NEXT FROM Curgis  into  @GISTable_ID,@GISTableName

COMMIT TRAN Trn;


END


CLOSE Curgis

DEALLOCATE Curgis

go

-- *****************************************************************************  
-- * Author:     Mohammad Tasdeeq
-- * Date:       26 Oct 2016
-- * Purpose:    IM1059124
-- * Comment:    EAGLE | IM1059124 Documaster Config screen, clicking apply results in an error message
-- *============================================================================

IF NOT EXISTS (SELECT NULL FROM doc_options WHERE option_name = 'ALLOW_COPY_PASTE')
INSERT INTO doc_options (option_name, option_value) VALUES ('ALLOW_COPY_PASTE','0')
GO

-- *****************************************************************************  
-- * Author:    Anurag Gupta
-- * Date:       14 Oct 2017
-- * Purpose:    MIPS CR003
-- * Comment:    
-- *============================================================================
declare @pmwrk_task_id int
declare @caption_id int
if not exists(select null from pmwrk_task where code = 'SAMGALPVA')
Begin
    EXECUTE spu_pm_caption_id_return 1, 'SamGetLiveVerisonAmount', @caption_id OUTPUT
    insert into pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values(@caption_id, 'SAMGALPVA', 'SamGetLiveVerisonAmount',0,getdate(),0,1,null,null,null,null,1,0,null,null,null,0,2,null)
End
go

-- *****************************************************************************  
-- * Author:    Samar Jeet
-- * Date:       14 Feb 2017
-- * Purpose:    To Ensure the Quote Versioning functionality remain OFF
-- * Comment:    
-- *============================================================================
   UPDATE System_Options set value=0  where option_number=5089

-- *****************************************************************************  
-- * Author:     Aditya Pratap Singh
-- * Date:       12 Dec 2016
-- * Purpose:    Pure4.1RnDWPR03
-- * Comment:    Add New Batch Status Failed in BatchStatus Table
-- *============================================================================

IF Not Exists (Select 1 From BatchStatus Where Code = 'F')
BEGIN
    Declare @CaptionID integer
    Declare @Batch_Status_Id integer
    
    Execute spu_pm_caption_id_return 1, 'Failed', @CaptionID output
    
    Select @Batch_Status_Id = ISNULL(Max(batchstatus_id),0)+1 From BatchStatus
    
	INSERT INTO BatchStatus (batchstatus_id, caption_id , is_deleted, effective_date, description, code)
	VALUES (@Batch_Status_Id, @CaptionID, 0, GETDATE(), 'Failed', 'F')
    
END

GO

-- *****************************************************************************  
-- * Author:     Aditya Pratap Singh
-- * Date:       12 Dec 2016
-- * Purpose:    Pure4.1RnDWPR03
-- * Comment:    Add New Batch Type Policy Export in Batch Type Table
-- *============================================================================

IF Not Exists (Select 1 From Batch_Type Where Code = 'POLX')
BEGIN
    Declare @CaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'Policy Export', @CaptionID output
    
    Select @Batch_Type_Id = ISNULL(Max(Batch_type_Id),0)+1 From Batch_Type
    
    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @CaptionID,
        0,
        GetDate(),
        'Policy Export',
        'POLX'
    )
END
GO

-- *****************************************************************************  
-- * Author:     Aditya Pratap Singh
-- * Date:       12 Dec 2016
-- * Purpose:    Pure4.1RnDWPR03
-- * Comment:    Add New Batch Type Credit Control in Batch Type Table
-- *============================================================================

IF Not Exists (Select 1 From Batch_Type Where Code = 'CREC')
BEGIN
    Declare @CaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'Credit Control', @CaptionID output
    
    Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type
    
    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @CaptionID,
        0,
        GetDate(),
        'Credit Control',
        'CREC'
    )
END
GO

-- *****************************************************************************  
-- * Author:     Aditya Pratap Singh
-- * Date:       12 Dec 2016
-- * Purpose:    Pure4.1RnDWPR03
-- * Comment:    Add New Batch Type Chase Cycle in Batch Type Table
-- *============================================================================

IF Not Exists (Select 1 From Batch_Type Where Code = 'CHACY')
BEGIN
    Declare @CaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'Chase Cycle', @CaptionID output
    
    Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type

    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @CaptionID,
        0,
        GetDate(),
        'Chase Cycle',
        'CHACY'
    )
END
GO

-- *****************************************************************************  
-- * Author:     Aditya Pratap Singh
-- * Date:       12 Dec 2016
-- * Purpose:    Pure4.1RnDWPR03
-- * Comment:    Add New Batch Type Claim BDX in Batch Type Table
-- *============================================================================

IF Not Exists (Select 1 From Batch_Type Where Code = 'BDXCLM')
BEGIN
    Declare @CaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'Claim BDX', @CaptionID output
    
    Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type
    
    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @CaptionID,
        0,
        GetDate(),
        'Claim BDX',
        'BDXCLM'
    )
END
GO

-- *****************************************************************************  
-- * Author:     Aditya Pratap Singh
-- * Date:       12 Dec 2016
-- * Purpose:    Pure4.1RnDWPR03
-- * Comment:    Add New Batch Type Policy BDX in Batch Type Table
-- *============================================================================

IF Not Exists (Select 1 From Batch_Type Where Code = 'BDXPOL')
BEGIN
    Declare @CaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'Policy BDX', @CaptionID output
    
    Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type
    
    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @CaptionID,
        0,
        GetDate(),
        'Policy BDX',
        'BDXPOL'
    )
END
GO

-- *****************************************************************************  
-- * Author:     Aditya Pratap Singh
-- * Date:       12 Dec 2016
-- * Purpose:    Pure4.1RnDWPR03
-- * Comment:    Add New Batch Type Premium BDX in Batch Type Table
-- *============================================================================

IF Not Exists (Select 1 From Batch_Type Where Code = 'BDXPREM')
BEGIN
    Declare @CaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'Premium BDX', @CaptionID output
    
    Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type
    
    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @CaptionID,
        0,
        GetDate(),
        'Premium BDX',
        'BDXPREM'
    )
END
GO

-- *****************************************************************************  
-- * Author:     Aditya Pratap Singh
-- * Date:       14 Dec 2016
-- * Purpose:    Pure4.1RnDWPR03
-- * Comment:    Add New Batch Type Bank Reconciliation Import in Batch Type Table
-- *============================================================================

IF Not Exists (Select 1 From Batch_Type Where Code = 'BCP')
BEGIN
    Declare @CaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'Bank Reconciliation Import', @CaptionID output
    
    Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type
    
    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @CaptionID,
        0,
        GetDate(),
        'Bank Reconciliation Import',
        'BCP'
    )
END
GO

-- *****************************************************************************  
-- * Author:     Aditya Pratap Singh
-- * Date:       14 Dec 2016
-- * Purpose:    Pure4.1RnDWPR03
-- * Comment:    Add New Batch Type Cash Allocation Import in Batch Type Table
-- *============================================================================

IF Not Exists (Select 1 From Batch_Type Where Code = 'CAALLOC')
BEGIN
    Declare @CaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'Cash Allocation Import', @CaptionID output
    
    Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type
    
    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @CaptionID,
        0,
        GetDate(),
        'Cash Allocation Import',
        'CAALLOC'
    )
END
GO

-- *****************************************************************************  
-- * Author:     Aditya Pratap Singh
-- * Date:       14 Dec 2016
-- * Purpose:    Pure4.1RnDWPR03
-- * Comment:    Add New Batch Type Currency Exchange Rates Import in Batch Type Table
-- *============================================================================

IF Not Exists (Select 1 From Batch_Type Where Code = 'CERA')
BEGIN
    Declare @CaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'Currency Exchange Rates', @CaptionID output
    
    Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type
    
    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @CaptionID,
        0,
        GetDate(),
        'Currency Exchange Rates',
        'CERA'
    )
END
GO

-- *****************************************************************************  
-- * Author:     Aditya Pratap Singh
-- * Date:       14 Dec 2016
-- * Purpose:    Pure4.1RnDWPR03
-- * Comment:    Add New Batch Type MID Import in Batch Type Table
-- *============================================================================

IF Not Exists (Select 1 From Batch_Type Where Code = 'MID1')
BEGIN
    Declare @CaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'MID IMPORT', @CaptionID output
    
    Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type
    
    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @CaptionID,
        0,
        GetDate(),
        'MID IMPORT',
        'MID1'
    )
END
GO

-- *****************************************************************************  
-- * Author:     Aditya Pratap Singh
-- * Date:       14 Dec 2016
-- * Purpose:    Pure4.1RnDWPR03
-- * Comment:    Add New Batch Type MID Import in Batch Type Table
-- *============================================================================

IF Not Exists (Select 1 From Batch_Type Where Code = 'SRPI')
BEGIN
    Declare @CaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'Receipt Import', @CaptionID output
    
    Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type

    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @CaptionID,
        0,
        GetDate(),
        'Receipt Import',
        'SRPI'
    )
END
GO


-- *****************************************************************************  
-- * Author:     Anshika Gupta
-- * Date:       30 Nov 2016
-- * Comment:    Fix Caption values for risk_type_rule_set_type
-- *============================================================================
DECLARE @caption_id INT

EXECUTE spu_pm_caption_id_return 1, '.Rul file script', @caption_id OUTPUT
UPDATE risk_type_rule_set_type
SET caption_id=@caption_id
WHERE code = 'SCRIPT'

EXECUTE spu_pm_caption_id_return 1, 'Precision Rating Engine', @caption_id OUTPUT
UPDATE risk_type_rule_set_type
SET caption_id=@caption_id
WHERE code = 'PRE'
  
EXECUTE spu_pm_caption_id_return 1, 'Compiled Rules', @caption_id OUTPUT
UPDATE risk_type_rule_set_type
SET caption_id=@caption_id
WHERE code = 'COMPILED'

--Update the Default Value as Rule file if NULL
UPDATE system_options set value= 1 where option_number in (5159,5160,5161,5162) AND value is NULL

  GO    

-- *****************************************************************************  
-- * Author:     Anshika Gupta
-- * Date:       03 Mar 2017
-- * Comment:    New table for CCM INTEGRATION WPR 04
-- *============================================================================
DECLARE @Cnt AS INT
SELECT @CNT = count(*) FROM CCMStatus

IF @CNT = 0
BEGIN
	DECLARE @lCaptionID int
	EXEC spu_pm_caption_id_return 1, 'Published', @lCaptionID output	
	INSERT INTO CCMStatus(CCMStatus_id,caption_id, code, description,is_deleted, effective_date)
	VALUES (1, @lCaptionID, 'PUB', 'Published',0, getdate())

	EXEC spu_pm_caption_id_return 1, 'Current', @lCaptionID output
	INSERT INTO CCMStatus(CCMStatus_id,caption_id, code, description,is_deleted, effective_date)
	VALUES (2, @lCaptionID, 'CUR', 'Current',0, getdate())

	EXEC spu_pm_caption_id_return 1, 'Accepted', @lCaptionID output
	INSERT INTO CCMStatus(CCMStatus_id,caption_id, code, description,is_deleted, effective_date)
	VALUES (3, @lCaptionID, 'ACC', 'Accepted',0, getdate())
END

UPDATE wp_Fields  SET table_name = REPLACE(REPLACE(REPLACE(REPLACE('Core' + RTRIM(main_group) + RTRIM(ISNULL(sub_group,'')) ,' ',''),'-',''),'/',''),',','')
Where data_model is NULL and Loop1 IS  NULL and Loop2 IS  NULL AND table_name is null

UPDATE wp_Fields  SET table_name = REPLACE(REPLACE(REPLACE(REPLACE('Core' + RTRIM(ISNULL(loop1,'')) + '_' + RTRIM(ISNULL(loop2,'')) ,' ',''),'-',''),'/',''),',','')
Where data_model is NULL and Loop1 IS NOT NULL and Loop2 IS NOT NULL AND table_name is null

UPDATE wp_Fields  SET table_name = REPLACE(REPLACE(REPLACE(REPLACE('Core' + RTRIM(ISNULL(loop1,'')) ,' ',''),'-',''),'/',''),',','')
Where data_model is NULL and Loop1 IS NOT NULL and Loop2 IS  NULL AND table_name is null
GO

-- *****************************************************************************  
-- * Author:      TARIQ RASHID
-- * Date:        01 APRIL 2015
-- * Purpose:     RACTI JIRA SSP 1318
-- *****************************************************************************
IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='RiskTaxValueUnrounded')
BEGIN
    INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
    VALUES ('RiskTaxValueUnrounded','spu_wp_risktax','risk_tax_value',18,'Risk','Risk Tax','Risk Tax Value Unrounded',
    1,'RiskTax',9)
END
GO

-- *****************************************************************************  
-- * Author:      Ashish Sachdeva
-- * Date:        08 Nov 2013
-- * Purpose:     RND003 - DMEToSharePoint
-- *****************************************************************************
DECLARE @lCaptionID INT
IF NOT EXISTS(SELECT NULL FROM DME_Migration_Status WHERE code = 'WIP')
BEGIN
 EXEC spu_pm_caption_id_return 1, 'In Progress', @lCaptionID OUTPUT
 INSERT INTO DME_Migration_Status(
 code,description,caption_id,effective_date,is_deleted)
 VALUES ('WIP','In Progress',@lCaptionID,GETDATE(),0)
END
IF NOT EXISTS(SELECT NULL FROM DME_Migration_Status WHERE code = 'FAIL')
BEGIN
 EXEC spu_pm_caption_id_return 1, 'Failed', @lCaptionID OUTPUT
 INSERT INTO DME_Migration_Status(
 code,description,caption_id,effective_date,is_deleted)
 VALUES ('FAIL','Failed',@lCaptionID,GETDATE(),0)
END
IF NOT EXISTS(SELECT NULL FROM DME_Migration_Status WHERE code = 'COMPLETE')
BEGIN
 EXEC spu_pm_caption_id_return 1, 'Completed', @lCaptionID OUTPUT
 INSERT INTO DME_Migration_Status(
 code,description,caption_id,effective_date,is_deleted)
 VALUES ('COMPLETE','Completed',@lCaptionID,GETDATE(),0)
END
GO  


-- *****************************************************************************  
-- * Author:      Swati Saxena
-- * Date:        10 Jun 2015
-- * Purpose:     To remove 'Authorise Transactions' task from Back office
-- *****************************************************************************
update PMWrk_Task set is_deleted =1 where code='authtrans'

Go

-- *****************************************************************************  
-- * Author:      Swati Saxena
-- * Date:        30 September 2015
-- * Purpose:     To add MTA Cancellation Task and Task group
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMACMQ', 'SAM Cancel MTA Quote'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMACMQ'
GO

update Process_Types_Docs set allow_filtering=1 where process_types_docs_id in (3,8,9,10,11,12,13)

GO

IF NOT EXISTS ( SELECT NULL FROM Insurance_File_Type WHERE code='MTAQCAN')
  BEGIN
    DECLARE @MTAQCANcaption_id INT
    DECLARE @Insurance_File_Type_id INT

    SELECT @Insurance_File_Type_id = MAX(ISNULL(Insurance_File_Type_id,0))+1 
    FROM Insurance_File_Type

    EXECUTE spu_pm_caption_id_return 1, 'MTA Quotation Cancellation', @MTAQCANcaption_id OUTPUT

    INSERT INTO Insurance_File_Type
    (Insurance_File_Type_id,caption_id,code,description,var_data_structure_id,is_deleted,effective_date)
    VALUES
    (@Insurance_File_Type_id,@MTAQCANcaption_id,'MTAQCAN','MTA Quotation Cancellation',NULL,0,GETDATE())
  END
  
  GO

-- *****************************************************************************  
-- * Author:      Swati Saxena
-- * Date:        09 Nov 2015
-- * Purpose:     CAPRICORN CR040, Locking
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGLD', 'GetLockDetails'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGLD'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMMLock', 'MaintainLock'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMMLock'
GO


-- *****************************************************************************
-- * Author:  Preeti Verma
-- * Date:    13/11/2017
-- * Purpose: Insert default values for Correspondence Type look up
--			Paralleling WPR 41 from 3.2SR4.IH to 3.1SR7 Capricon
-- *****************************************************************************
IF NOT EXISTS(select code FROM Correspondence_Type WHERE code = 'DEFAULT')
    BEGIN

    DECLARE @lCaptionID INT
 
    -- Use Default
        EXECUTE spu_pm_caption_id_return 1, 'Use Default', @lCaptionID OUTPUT
        INSERT INTO Correspondence_Type(code, description, caption_id, effective_date,is_deleted)
    	VALUES('DEFAULT','Use Default', @lCaptionID, GETDATE(), 0 )
  
END
GO

IF NOT EXISTS(select code FROM Correspondence_Type WHERE code = 'LETTER')
    BEGIN

    DECLARE @lCaptionID INT
 
    -- Letter - Override
        EXECUTE spu_pm_caption_id_return 1, 'Letter - Override', @lCaptionID OUTPUT
        INSERT INTO Correspondence_Type(code, description, caption_id, effective_date,is_deleted)
    	VALUES('LETTER','Letter - Override', @lCaptionID, GETDATE(), 0 )
  
END
GO
IF NOT EXISTS(select code FROM Correspondence_Type WHERE code = 'EMAIL')
    BEGIN

    DECLARE @lCaptionID INT
 
    -- Email - Override
        EXECUTE spu_pm_caption_id_return 1, 'Email - Override', @lCaptionID OUTPUT
        INSERT INTO Correspondence_Type(code, description, caption_id, effective_date,is_deleted)
    	VALUES('EMAIL','Email - Override', @lCaptionID, GETDATE(), 0 )
  
END
GO
IF NOT EXISTS(select code FROM Correspondence_Type WHERE code = 'BR-EMAIL')
    BEGIN

    DECLARE @lCaptionID INT
 
    -- Branch
        EXECUTE spu_pm_caption_id_return 1, 'Branch', @lCaptionID OUTPUT
        INSERT INTO Correspondence_Type(code, description, caption_id, effective_date,is_deleted)
    	VALUES('BR-EMAIL','Branch', @lCaptionID, GETDATE(), 0 )
  
END
GO


-- *****************************************************************************
-- *****************************************************************************  
-- * Comment: Update Apply OOS option in Fee maintenance for existing records 
-- *============================================================================

UPDATE Fee_Amounts SET Use_When_Deleted = 1 WHERE 
Use_When_Deleted is NULL
GO

-- *****************************************************************************


-- *****************************************************************************
-- * ADO #39336: Instalment for Claim Recovery - Transaction Type Mapping
-- * Purpose:  Add ICC/ICD transaction codes for claim recovery instalments
-- *****************************************************************************
DECLARE @icc_caption_id INT
DECLARE @icd_caption_id INT
DECLARE @icc_id INT
DECLARE @icd_id INT

IF NOT EXISTS (SELECT 1 FROM pfinstalments_transaction WHERE code = 'ICC')
BEGIN
    SELECT @icc_id = ISNULL(MAX(pfinstalments_transaction_id), 0) + 1 FROM pfinstalments_transaction
    EXECUTE spu_pm_caption_id_return 1, 'Instalment Claim Credit', @icc_caption_id OUTPUT
    INSERT INTO pfinstalments_transaction (pfinstalments_transaction_id, code, description, caption_id, effective_date, is_deleted)
    VALUES (@icc_id, 'ICC', 'Instalment Claim Credit', @icc_caption_id, GETDATE(), 0)
END

IF NOT EXISTS (SELECT 1 FROM pfinstalments_transaction WHERE code = 'ICD')
BEGIN
    SELECT @icd_id = ISNULL(MAX(pfinstalments_transaction_id), 0) + 1 FROM pfinstalments_transaction
    EXECUTE spu_pm_caption_id_return 1, 'Instalment Claim Debit', @icd_caption_id OUTPUT
    INSERT INTO pfinstalments_transaction (pfinstalments_transaction_id, code, description, caption_id, effective_date, is_deleted)
    VALUES (@icd_id, 'ICD', 'Instalment Claim Debit', @icd_caption_id, GETDATE(), 0)
END
GO

-- * End of File
-- *****************************************************************************

-- *****************************************************************************
-- * Author:  Amrita Garg
-- * Date:    07/03/2017
-- * Purpose: Insert default values for Correspondence Type look up
--			Paralleling WPR 3.1 from BFM to IH
-- *****************************************************************************
IF NOT EXISTS(select code FROM Correspondence_Type WHERE code = 'DEFAULT')
    BEGIN

    DECLARE @lCaptionID INT
 
    -- Instalment Deposit
        EXECUTE spu_pm_caption_id_return 1, 'Use Default', @lCaptionID OUTPUT
        INSERT INTO Correspondence_Type(code, description, caption_id, effective_date,is_deleted)
    	VALUES('DEFAULT','Use Default', @lCaptionID, GETDATE(), 0 )
  
END
GO

IF NOT EXISTS(select code FROM Correspondence_Type WHERE code = 'LETTER')
    BEGIN

    DECLARE @lCaptionID INT
 
    -- Instalment Deposit
        EXECUTE spu_pm_caption_id_return 1, 'Letter - Override', @lCaptionID OUTPUT
        INSERT INTO Correspondence_Type(code, description, caption_id, effective_date,is_deleted)
    	VALUES('LETTER','Letter - Override', @lCaptionID, GETDATE(), 0 )
  
END
GO
IF NOT EXISTS(select code FROM Correspondence_Type WHERE code = 'EMAIL')
    BEGIN

    DECLARE @lCaptionID INT
 
    -- Instalment Deposit
        EXECUTE spu_pm_caption_id_return 1, 'Email - Override', @lCaptionID OUTPUT
        INSERT INTO Correspondence_Type(code, description, caption_id, effective_date,is_deleted)
    	VALUES('EMAIL','Email - Override', @lCaptionID, GETDATE(), 0 )

END
GO
IF NOT EXISTS(select code FROM Correspondence_Type WHERE code = 'BR-EMAIL')
    BEGIN

    DECLARE @lCaptionID INT
 
    -- Instalment Deposit
        EXECUTE spu_pm_caption_id_return 1, 'Branch', @lCaptionID OUTPUT
        INSERT INTO Correspondence_Type(code, description, caption_id, effective_date,is_deleted)
    	VALUES('BR-EMAIL','Branch', @lCaptionID, GETDATE(), 0 )
  
END
GO

-- *****************************************************************************
-- * Author:    Samar Jeet
-- * Date:       14 Feb 2017
-- * Purpose:    To Ensure the Quote Versioning functionality remain OFF
-- * Comment:    
-- *============================================================================
   UPDATE System_Options set value=0  where option_number=5089
   GO

-- *****************************************************************************
-- * Author:    Anshika Gupta
-- * Date:       06 July 2017
-- * Purpose:    WPR005 Instalment Paralleling
-- *============================================================================

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGETPF', 'Get Header And Summaries PFPlanByKey'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGETPF'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUPDIS', 'SAM_Update_Premium_Finance_Instalment'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMUPDIS'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCANPFPL', 'Cancel Premium Finance Plan'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMCANPFPL'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCANPFPO', 'Cancel PF Policies'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMCANPFPO'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMPROPFP', 'Process PF Plan'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMPROPFP'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUPDPF', 'SAM_Update_Premium_Finance_Instalment'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMUPDPF'
GO
-- *****************************************************************************
-- * Author:	Samar Jeet
-- * Date:		11/Jul/2018
-- * Purpose:	WPR02 Developement
-- *****************************************************************************
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCLMD', 'SAM Delete Abandon Claim' 
GO 
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMCLMD'
GO
-- *****************************************************************************
-- * Author:  Amita Aggarwal
-- * Date:    26/04/2017
-- * Purpose: Setting the default value to Letter for existing personal /corporate/ agent parties
-- *****************************************************************************
UPDATE party 
SET correspondence_type_id = 6 
WHERE (correspondence_type_id  =  0 OR correspondence_type_id is NULL) 
AND party_type_id in (1,3,4)
GO

-- *****************************************************************************
-- * Author:	Suman Anjna
-- * Date:		Jul/2017
-- * Purpose:	MID2
-- *****************************************************************************
DECLARE @pmwork_task_id int,
		@caption_id int

IF NOT EXISTS(select null from PMWrk_Task where code='MIDMAINT')
BEGIN
	EXECUTE spu_pm_caption_id_return  1,'MID Maintenance', @caption_id OUTPUT

	INSERT INTO PMWrk_Task
           ([caption_id],[code],[description],[is_deleted],[effective_date],[is_system_task],[type_of_task],[pmnav_process_id],[component_object_name]
			,[component_class_name],[auto_delete_after_num_days],[display_icon],[is_view_only_task],[linked_object_name],[linked_class_name]
			,[linked_caption_id],[is_available_task],[pmwrk_task_category_id],[pmnavxm_process_id])
     	VALUES
           (@caption_id,'MIDMAINT','MID Maintenance',0,GETDATE(),0,1,null,'iSIRMIDMaintenance','NavigatorV3',0,1,0,null,null,null,1	,2,null)
END
GO
DECLARE @pmwork_task_id int,
		@caption_id int

IF NOT EXISTS(select null from PMWrk_Task where code='SAMMID')
BEGIN
	EXECUTE spu_pm_caption_id_return  1,'GetMIDFiles', @caption_id OUTPUT

	INSERT INTO PMWrk_Task
           ([caption_id],[code],[description],[is_deleted],[effective_date],[is_system_task],[type_of_task],[pmnav_process_id],[component_object_name]
			,[component_class_name],[auto_delete_after_num_days],[display_icon],[is_view_only_task],[linked_object_name],[linked_class_name]
			,[linked_caption_id],[is_available_task],[pmwrk_task_category_id],[pmnavxm_process_id])
     	VALUES
           (@caption_id,'SAMMID','GetMIDFiles',0,GETDATE(),0,1,null,NULL,NULL,NULL,1,0,null,null,null,0,2,null)
END
GO
DECLARE @pmwork_task_id int,
		@caption_id int

IF NOT EXISTS(select null from PMWrk_Task where code='SAMMIDDET')
BEGIN
	EXECUTE spu_pm_caption_id_return  1,'SAM MID Details', @caption_id OUTPUT

	INSERT INTO PMWrk_Task
           ([caption_id],[code],[description],[is_deleted],[effective_date],[is_system_task],[type_of_task],[pmnav_process_id],[component_object_name]
			,[component_class_name],[auto_delete_after_num_days],[display_icon],[is_view_only_task],[linked_object_name],[linked_class_name]
			,[linked_caption_id],[is_available_task],[pmwrk_task_category_id],[pmnavxm_process_id])
     	VALUES
           (@caption_id,'SAMMIDDET','SAM MID Details',0,GETDATE(),0,1,null,NULL,NULL,NULL,1,0,null,null,null,0,2,null)
END
GO

IF Not Exists (Select 1 From Batch_Type Where Code = 'MID2')
BEGIN
    Declare @lCaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'MID2', @lCaptionID output
    Select @Batch_Type_Id = (Max(Batch_type_Id) + 1) From Batch_Type
    
    INSERT INTO Batch_type 
		(Batch_type_id,Caption_id,is_deleted,effective_date,Description,Code)
    VALUES
		(@Batch_Type_Id,@lCaptionID,0,GetDate(),'MID2 Export','MID2')
END
GO

IF NOT EXISTS (SELECT lookup_table_name FROM PMProduct_Lookup WHERE lookup_table_name = 'Supplier_Type')
BEGIN
	INSERT INTO PMProduct_Lookup
			(pmproduct_id, lookup_table_name, edit_privilege_level, is_generic_maintenance)
	VALUES	(2, 'Supplier_Type', 3, 1)
End
GO

-- *****************************************************************************
-- * Author:	Suman Anjna
-- * Purpose:	MID2 File Status
-- *****************************************************************************
IF Not Exists (Select 1 From mid_status Where code = 'PENDING')
BEGIN
    Declare @nCaptionID integer    
    Execute spu_pm_caption_id_return 1, 'PENDING', @nCaptionID output 
	
	INSERT INTO mid_status	([code],[description],[caption_id],[effective_date],[is_deleted])
	VALUES ('PENDING','PENDING',@nCaptionID,GETDATE(),0)
END
GO
IF Not Exists (Select 1 From mid_status Where code = 'GENERATED')
BEGIN
    Declare @nCaptionID integer    
    Execute spu_pm_caption_id_return 1, 'GENERATED', @nCaptionID output
	
	INSERT INTO mid_status	([code],[description],[caption_id],[effective_date],[is_deleted])
	VALUES ('GENERATED','GENERATED',@nCaptionID,GETDATE(),0)
END
GO
IF Not Exists (Select 1 From mid_status Where code = 'LOADED')
BEGIN
    Declare @nCaptionID integer    
    Execute spu_pm_caption_id_return 1, 'LOADED', @nCaptionID output  
	
	INSERT INTO mid_status	([code],[description],[caption_id],[effective_date],[is_deleted])
	VALUES ('LOADED','LOADED',@nCaptionID,GETDATE(),0)
END
GO
IF Not Exists (Select 1 From mid_status Where code = 'ERROR')
BEGIN
    Declare @nCaptionID integer    
    Execute spu_pm_caption_id_return 1, 'ERROR', @nCaptionID output 
	
	INSERT INTO mid_status	([code],[description],[caption_id],[effective_date],[is_deleted])
	VALUES ('ERROR','ERROR',@nCaptionID,GETDATE(),0)
END
GO
IF Not Exists (Select 1 From mid_status Where code = 'RECEIVED')
BEGIN
    Declare @nCaptionID integer    
    Execute spu_pm_caption_id_return 1, 'RECEIVED', @nCaptionID output
	
	INSERT INTO mid_status	([code],[description],[caption_id],[effective_date],[is_deleted])
	VALUES ('RECEIVED','RECEIVED',@nCaptionID,GETDATE(),0)
END
GO
IF Not Exists (Select 1 From mid_status Where code = 'REJECTED')
BEGIN
    Declare @nCaptionID integer    
    Execute spu_pm_caption_id_return 1, 'REJECTED', @nCaptionID output
	
	INSERT INTO mid_status	([code],[description],[caption_id],[effective_date],[is_deleted])
	VALUES ('REJECTED','REJECTED',@nCaptionID,GETDATE(),0)
END
GO

-- *****************************************************************************
--  Purpose of this Script to assign the Rule type as .RuleFile 
-- *****************************************************************************
-- * Author:	Samar Jeet
-- * Purpose:	Initialise defalut Rule type as  rule files
-- * Date    :05/Sep/2017
-- *****************************************************************************

DECLARE  @this_branch_id INT
DECLARE @cboOption5160 Int
DECLARE @cboOption5161 INT
DECLARE @cboOption5162 INT
DECLARE @cboOption5159 INT
 
Set @cboOption5160=5160--Credit Control Rule Type
Set @cboOption5161=5161--Chase Cycle Rule Type
Set @cboOption5162=5162--Address lookup Installation Rule Type
Set @cboOption5159=5159--Payment gateway Rule Type


DECLARE @nValue Int
SET @nValue=0
                        
DECLARE BRANCH_CURSOR SCROLL CURSOR  FOR
SELECT source_id from source
OPEN BRANCH_CURSOR
FETCH NEXT FROM BRANCH_CURSOR INTO @this_branch_id
WHILE @@FETCH_STATUS=0

BEGIN

	SELECT @nValue=ISNULL(value,0)  from system_options where branch_id = @this_branch_id and option_number = @cboOption5160
	If @nValue=0 
		Begin
			INSERT INTO system_options (branch_id, option_number, value) VALUES (@this_branch_id,5160,1)
		End

	Select @nValue=ISNULL(value,0)  from system_options where branch_id = @this_branch_id and option_number = @cboOption5161
	If @nValue=0 
		Begin
			INSERT INTO system_options (branch_id, option_number, value) VALUES (@this_branch_id,5161,1)
		End

	SELECT @nValue=ISNULL(value,0)  from system_options where branch_id = @this_branch_id and option_number = @cboOption5162
	If @nValue=0 
		Begin
			INSERT INTO system_options (branch_id, option_number, value) VALUES (@this_branch_id,5162,1)
		End
                  
	SELECT @nValue=ISNULL(value,0)  from system_options where branch_id = @this_branch_id and option_number = @cboOption5159
	If @nValue=0 
		Begin
			INSERT INTO system_options (branch_id, option_number, value) VALUES (@this_branch_id,5159,1)
		End
					    
	FETCH NEXT FROM BRANCH_CURSOR INTO @this_branch_id
END
CLOSE BRANCH_CURSOR
DEALLOCATE BRANCH_CURSOR

If EXISTS(SELECT NULL FROM Hidden_options WHERE option_number = 88 AND ISNULL(value,0) <> 0)
BEGIN
	UPDATE System_Options SET value = 1 WHERE option_number = 5263
END
-- *****************************************************************************
-- *****************************************************************************
-- * Author:  Swati Burnwal
-- * Date:    18/08/2017       
-- * Purpose: 24008 - Add Otstanding Amount Task Entries.
-- *****************************************************************************
DECLARE @pmwrk_task_id INT
DECLARE @caption_id INT
IF NOT EXISTS(SELECT null FROM pmwrk_task WHERE code='SAMGPOA')
BEGIN
EXECUTE spu_pm_caption_id_return 1,'SAMGetOSAmount',@caption_id OUTPUT
INSERT INTO pmwrk_task
(caption_id,code,description,is_deleted,effective_date,is_system_task,type_of_task,pmnav_process_id,component_object_name,component_class_name,auto_delete_after_num_days,display_icon,is_view_only_task,linked_object_name,linked_class_name,linked_caption_id,is_available_task,pmwrk_task_category_id,pmnavxm_process_id)
VALUES(@caption_id,'SAMGPOA','SAMGetOSAmount',0,getdate(),0,1,null,null,null,null,1,0,null,null,null,0,2,null)
End
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGETPF', 'Get Header And Summaries PFPlanByKey'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGETPF'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUPDIS', 'SAM_Update_Premium_Finance_Instalment'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMUPDIS'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCANPFPL', 'Cancel Premium Finance Plan'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMCANPFPL'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMCANPFPO', 'Cancel PF Policies'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMCANPFPO'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMPROPFP', 'Process PF Plan'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMPROPFP'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUPDPF', 'SAM_Update_Premium_Finance_Instalment'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMUPDPF'
GO

-- *****************************************************************************
-- * Author:	Suman Anjna
-- * Date:		Mar/2018
-- * Purpose:	GDPR
-- *****************************************************************************
IF NOT EXISTS(select NULL from Service_level where Upper(code)='RESTRICTED')
BEGIN
	DECLARE @caption_id int
	EXECUTE spu_pm_caption_id_return 1,'Restricted', @caption_id OUTPUT
	INSERT INTO [Service_level]
			([service_level_id],[caption_id],[code],[description],[is_deleted],[effective_date])
		VALUES		 
			((SELECT max(service_level_id) + 1 from Service_Level),@caption_id,'Restricted','Restricted',0, GETDATE())
END
GO
IF NOT EXISTS(select NULL from Service_level where Upper(code)='OBJECTED')
	BEGIN
	DECLARE @caption_id int
	EXECUTE spu_pm_caption_id_return  1,'Objected', @caption_id OUTPUT
	INSERT INTO [Service_level]
			([service_level_id],[caption_id],[code],[description],[is_deleted],[effective_date])
		VALUES		 
			((SELECT max(service_level_id) + 1 from Service_Level),@caption_id,'Objected','Objected',0, GETDATE())
END
GO

-- *****************************************************************************


-- *****************************************************************************
-- * ADO #39336: Instalment for Claim Recovery - Transaction Type Mapping
-- * Purpose:  Add ICC/ICD transaction codes for claim recovery instalments
-- *****************************************************************************
DECLARE @icc_caption_id INT
DECLARE @icd_caption_id INT
DECLARE @icc_id INT
DECLARE @icd_id INT

IF NOT EXISTS (SELECT 1 FROM pfinstalments_transaction WHERE code = 'ICC')
BEGIN
    SELECT @icc_id = ISNULL(MAX(pfinstalments_transaction_id), 0) + 1 FROM pfinstalments_transaction
    EXECUTE spu_pm_caption_id_return 1, 'Instalment Claim Credit', @icc_caption_id OUTPUT
    INSERT INTO pfinstalments_transaction (pfinstalments_transaction_id, code, description, caption_id, effective_date, is_deleted)
    VALUES (@icc_id, 'ICC', 'Instalment Claim Credit', @icc_caption_id, GETDATE(), 0)
END

IF NOT EXISTS (SELECT 1 FROM pfinstalments_transaction WHERE code = 'ICD')
BEGIN
    SELECT @icd_id = ISNULL(MAX(pfinstalments_transaction_id), 0) + 1 FROM pfinstalments_transaction
    EXECUTE spu_pm_caption_id_return 1, 'Instalment Claim Debit', @icd_caption_id OUTPUT
    INSERT INTO pfinstalments_transaction (pfinstalments_transaction_id, code, description, caption_id, effective_date, is_deleted)
    VALUES (@icd_id, 'ICD', 'Instalment Claim Debit', @icd_caption_id, GETDATE(), 0)
END
GO

-- * End of File
-- *****************************************************************************
UPDATE party 
SET correspondence_type_id = (SELECT contact_type_id FROM Contact_Type WHERE CODE='LETTER')
WHERE (correspondence_type_id  =  0 OR correspondence_type_id is NULL) 
AND party_type_id in (1,3,4)
GO

-- *****************************************************************************
-- * Author:        Deepak Arora
-- * Date:          29/12/2017
-- * Purpose:		Pure5.0 WPR13 Master Client Associations
-- *****************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMUPDPASO', 'SAM_Update Policy Associates'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMUPDPASO'
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGETPASO', 'SAM_Get Policy Associates'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGETPASO'
GO

IF NOT EXISTS (SELECT lookup_table_name FROM PMProduct_Lookup WHERE lookup_table_name = 'Association_Type')
BEGIN
 INSERT INTO PMProduct_Lookup  (pmproduct_id, lookup_table_name, edit_privilege_level,  is_generic_maintenance, linked_data_mandatory)
 VALUES       (2, 'Association_Type', 3, 1, 0)
END
GO

-- *****************************************************************************
-- * Purpose:     Add WP Feild For Policy Associates Document
-- *****************************************************************************

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='PartyName')
	BEGIN
		INSERT INTO wp_fields(
               field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
		VALUES('PartyName','spu_wp_insurancefileassociates','Party_Name',0,'Policy','Associates','Party Name',1,'PartyName',9)
	END

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='AssociationTypeDesc')
	BEGIN
		INSERT INTO wp_fields(
               field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
		VALUES('AssociationTypeDesc','spu_wp_insurancefileassociates','Association_Type_Desc',0,'Policy','Associates','Type Of Association ',1,'AssociationTypeDesc',9)
	END

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='DateAttached')
	BEGIN
		INSERT INTO wp_fields(
               field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
		VALUES('DateAttached','spu_wp_insurancefileassociates','Date_Attached',4,'Policy','Associates','Date Attached',1,'DateAttached',9)
	END


IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='DateRemoved')
	BEGIN
		INSERT INTO wp_fields(
               field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
		VALUES('DateRemoved','spu_wp_insurancefileassociates','Date_Removed',4,'Policy','Associates','Date Removed',1,'DateRemoved',9)
	END

IF NOT EXISTS(SELECT NULL FROM wp_fields WHERE field_name='AssociationDetail')
	BEGIN
		INSERT INTO wp_fields(
               field_name,sql,column_name,column_type,main_group,sub_group,display_name,is_displayed,loop1,product_family)
		VALUES('AssociationDetail','spu_wp_insurancefileassociates','Association_Detail',0,'Policy','Associates','Association Detail',1,'AssociationDetail',9)
	END
	
Go

IF NOT EXISTS (SELECT lookup_table_name FROM PMProduct_Lookup 
WHERE lookup_table_name = 'Association_Type')
BEGIN
 INSERT INTO PMProduct_Lookup
 (pmproduct_id, lookup_table_name, edit_privilege_level, 
 is_generic_maintenance, linked_data_mandatory)
 VALUES       (2, 'Association_Type', 3, 1, 0)
END
GO
-- *****************************************************************************
-- * Author:  Shivraj Rathor
-- * Date:    08/01/2018       
-- * Purpose: 5.0SR0WPR03 - add one more record for display 'Reversed' as reason in Instalment plan maintenance
-- *****************************************************************************
IF NOT EXISTS(SELECT null FROM PFInstalments_Result WHERE code='REV')
BEGIN
DECLARE @caption_id INT
SET @caption_id=0
EXECUTE spu_pm_caption_id_return 1,'Reversed',@caption_id OUTPUT
INSERT INTO PFInstalments_Result(PFInstalments_Result_id,code,description,caption_id,effective_date,is_deleted)
select (select max(PFInstalments_Result_id)+1 from PFInstalments_Result),'REV','Reversed',@caption_id,getdate(),0
End
GO

-- *****************************************************************************
-- * Author:  Samar Jeet
-- * Date:    28/02/2018       
-- * Purpose: WPR-14 Address Control Developement ,Merged fields for Document production.
-- *****************************************************************************

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='BillingAddressLine5')
BEGIN
	INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model,           property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
	VALUES ('BillingAddressLine5', 'spu_wp_addresses', 'Billing_Address_Address5', 0, 'Address - Client', 'Billing Address', 'Address 5', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientBillingAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='BillingAddressLine6')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('BillingAddressLine6', 'spu_wp_addresses', 'Billing_Address_Address6', 0, 'Address - Client', 'Billing Address', 'Address 6', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientBillingAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='BillingAddressLine7')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('BillingAddressLine7', 'spu_wp_addresses', 'Billing_Address_Address7', 0, 'Address - Client', 'Billing Address', 'Address 7', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientBillingAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='BillingAddressLine8')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('BillingAddressLine8', 'spu_wp_addresses', 'Billing_Address_Address8', 0, 'Address - Client', 'Billing Address', 'Address 8', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientBillingAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='BillingAddressLine9')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('BillingAddressLine9', 'spu_wp_addresses', 'Billing_Address_Address9', 0, 'Address - Client', 'Billing Address', 'Address 9', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientBillingAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='BillingAddressLine10')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('BillingAddressLine10', 'spu_wp_addresses', 'Billing_Address_Address10', 0, 'Address - Client', 'Billing Address', 'Address 10', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientBillingAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='BranchAddressLine5')
BEGIN

INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('BranchAddressLine5', 'spu_wp_addresses', 'Branch_Address_Address5', 0, 'Address - Client', 'Branch Address', 'Address 5', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientBranchAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='BranchAddressLine6')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('BranchAddressLine6', 'spu_wp_addresses', 'Branch_Address_Address6', 0, 'Address - Client', 'Branch Address', 'Address 6', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientBranchAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='BranchAddressLine7')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('BranchAddressLine7', 'spu_wp_addresses', 'Branch_Address_Address7', 0, 'Address - Client', 'Branch Address', 'Address 7', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientBranchAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='BranchAddressLine8')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('BranchAddressLine8', 'spu_wp_addresses', 'Branch_Address_Address8', 0, 'Address - Client', 'Branch Address', 'Address 8', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientBranchAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='BranchAddressLine9')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('BranchAddressLine9', 'spu_wp_addresses', 'Branch_Address_Address9', 0, 'Address - Client', 'Branch Address', 'Address 9', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientBranchAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='BranchAddressLine10')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('BranchAddressLine10', 'spu_wp_addresses', 'Branch_Address_Address10', 0, 'Address - Client', 'Branch Address', 'Address 10', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientBranchAddress', NULL)
END
GO
---
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='BusinessAddressLine5')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('BusinessAddressLine5', 'spu_wp_addresses', 'Business_Address_Address5', 0, 'Address - Client', 'Business Address', 'Address 5', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientBusinessAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='BusinessAddressLine6')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('BusinessAddressLine6', 'spu_wp_addresses', 'Business_Address_Address6', 0, 'Address - Client', 'Business Address', 'Address 6', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientBusinessAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='BusinessAddressLine7')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('BusinessAddressLine7', 'spu_wp_addresses', 'Business_Address_Address7', 0, 'Address - Client', 'Business Address', 'Address 7', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientBusinessAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='BusinessAddressLine8')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('BusinessAddressLine8', 'spu_wp_addresses', 'Business_Address_Address8', 0, 'Address - Client', 'Business Address', 'Address 8', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientBusinessAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='BusinessAddressLine9')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('BusinessAddressLine9', 'spu_wp_addresses', 'Business_Address_Address9', 0, 'Address - Client', 'Business Address', 'Address 9', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientBusinessAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='BusinessAddressLine10')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('BusinessAddressLine10', 'spu_wp_addresses', 'Business_Address_Address10', 0, 'Address - Client', 'Business Address', 'Address 10', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientBusinessAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='CorrespondenceAddressLine5')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('CorrespondenceAddressLine5', 'spu_wp_addresses', 'Correspondence_Address_Address5', 0, 'Address - Client', 'Correspondence Address', 'Address 5', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientCorrespondenceAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='CorrespondenceAddressLine6')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('CorrespondenceAddressLine6', 'spu_wp_addresses', 'Correspondence_Address_Address6', 0, 'Address - Client', 'Correspondence Address', 'Address 6', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientCorrespondenceAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='CorrespondenceAddressLine7')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('CorrespondenceAddressLine7', 'spu_wp_addresses', 'Correspondence_Address_Address7', 0, 'Address - Client', 'Correspondence Address', 'Address 7', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientCorrespondenceAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='CorrespondenceAddressLine8')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('CorrespondenceAddressLine8', 'spu_wp_addresses', 'Correspondence_Address_Address8', 0, 'Address - Client', 'Correspondence Address', 'Address 8', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientCorrespondenceAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='CorrespondenceAddressLine9')
BEGIN
	INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
	VALUES ('CorrespondenceAddressLine9', 'spu_wp_addresses', 'Correspondence_Address_Address9', 0, 'Address - Client', 'Correspondence Address', 'Address 9', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientCorrespondenceAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='CorrespondenceAddressLine10')
BEGIN
	INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model,          property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
	VALUES ('CorrespondenceAddressLine10', 'spu_wp_addresses', 'Correspondence_Address_Address10', 0, 'Address - Client', 'Correspondence Address', 'Address 10', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientCorrespondenceAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='EmailAddressLine5')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('EmailAddressLine5', 'spu_wp_addresses', 'Email_Address_Address5', 0, 'Address - Client', 'Email Address', 'Address 5', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientEmailAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='EmailAddressLine6')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('EmailAddressLine6', 'spu_wp_addresses', 'Email_Address_Address6', 0, 'Address - Client', 'Email Address', 'Address 6', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientEmailAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='EmailAddressLine7')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('EmailAddressLine7', 'spu_wp_addresses', 'Email_Address_Address7', 0, 'Address - Client', 'Email Address', 'Address 7', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientEmailAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='EmailAddressLine8')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('EmailAddressLine8', 'spu_wp_addresses', 'Email_Address_Address8', 0, 'Address - Client', 'Email Address', 'Address 8', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientEmailAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='EmailAddressLine9')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('EmailAddressLine9', 'spu_wp_addresses', 'Email_Address_Address9', 0, 'Address - Client', 'Email Address', 'Address 9', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientEmailAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='EmailAddressLine10')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('EmailAddressLine10', 'spu_wp_addresses', 'Email_Address_Address10', 0, 'Address - Client', 'Email Address', 'Address 10', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientEmailAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='HomeAddressLine5')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('HomeAddressLine5', 'spu_wp_addresses', 'Home_Address_Address5', 0, 'Address - Client', 'Home Address', 'Address 5', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientHomeAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='HomeAddressLine6')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('HomeAddressLine6', 'spu_wp_addresses', 'Home_Address_Address6', 0, 'Address - Client', 'Home Address', 'Address 6', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientHomeAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='HomeAddressLine7')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('HomeAddressLine7', 'spu_wp_addresses', 'Home_Address_Address7', 0, 'Address - Client', 'Home Address', 'Address 7', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientHomeAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='HomeAddressLine8')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('HomeAddressLine8', 'spu_wp_addresses', 'Home_Address_Address8', 0, 'Address - Client', 'Home Address', 'Address 8', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientHomeAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='HomeAddressLine9')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('HomeAddressLine9', 'spu_wp_addresses', 'Home_Address_Address9', 0, 'Address - Client', 'Home Address', 'Address 9', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientHomeAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='HomeAddressLine10')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('HomeAddressLine10', 'spu_wp_addresses', 'Home_Address_Address10', 0, 'Address - Client', 'Home Address', 'Address 10', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientHomeAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='OtherLine5')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('OtherLine5', 'spu_wp_addresses', 'Other_Address5', 0, 'Address - Client', 'Other', 'Address 5', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientOther', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='OtherLine6')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('OtherLine6', 'spu_wp_addresses', 'Other_Address6', 0, 'Address - Client', 'Other', 'Address 6', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientOther', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='OtherLine7')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('OtherLine7', 'spu_wp_addresses', 'Other_Address7', 0, 'Address - Client', 'Other', 'Address 7', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientOther', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='OtherLine8')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('OtherLine8', 'spu_wp_addresses', 'Other_Address8', 0, 'Address - Client', 'Other', 'Address 8', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientOther', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='OtherLine9')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('OtherLine9', 'spu_wp_addresses', 'Other_Address9', 0, 'Address - Client', 'Other', 'Address 9', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientOther', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='OtherLine10')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('OtherLine10', 'spu_wp_addresses', 'Other_Address10', 0, 'Address - Client', 'Other', 'Address 10', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientOther', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='PreviousAddressLine5')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('PreviousAddressLine5', 'spu_wp_addresses', 'Previous_Address_Address5', 0, 'Address - Client', 'Previous Address', 'Address 5', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientPreviousAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='PreviousAddressLine6')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('PreviousAddressLine6', 'spu_wp_addresses', 'Previous_Address_Address6', 0, 'Address - Client', 'Previous Address', 'Address 6', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientPreviousAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='PreviousAddressLine7')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('PreviousAddressLine7', 'spu_wp_addresses', 'Previous_Address_Address7', 0, 'Address - Client', 'Previous Address', 'Address 7', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientPreviousAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='PreviousAddressLine8')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('PreviousAddressLine8', 'spu_wp_addresses', 'Previous_Address_Address8', 0, 'Address - Client', 'Previous Address', 'Address 8', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientPreviousAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='PreviousAddressLine9')
BEGIN

INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('PreviousAddressLine9', 'spu_wp_addresses', 'Previous_Address_Address9', 0, 'Address - Client', 'Previous Address', 'Address 9', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientPreviousAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='PreviousAddressLine10')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('PreviousAddressLine10', 'spu_wp_addresses', 'Previous_Address_Address10', 0, 'Address - Client', 'Previous Address', 'Address 10', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientPreviousAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='RegisteredAddressLine5')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('RegisteredAddressLine5', 'spu_wp_addresses', 'Registered_Address_Address5', 0, 'Address - Client', 'Registered Address', 'Address 5', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientRegisteredAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='RegisteredAddressLine6')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('RegisteredAddressLine6', 'spu_wp_addresses', 'Registered_Address_Address6', 0, 'Address - Client', 'Registered Address', 'Address 6', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientRegisteredAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='RegisteredAddressLine7')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('RegisteredAddressLine7', 'spu_wp_addresses', 'Registered_Address_Address7', 0, 'Address - Client', 'Registered Address', 'Address 7', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientRegisteredAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='RegisteredAddressLine8')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('RegisteredAddressLine8', 'spu_wp_addresses', 'Registered_Address_Address8', 0, 'Address - Client', 'Registered Address', 'Address 8', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientRegisteredAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='RegisteredAddressLine9')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('RegisteredAddressLine9', 'spu_wp_addresses', 'Registered_Address_Address9', 0, 'Address - Client', 'Registered Address', 'Address 9', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientRegisteredAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='RegisteredAddressLine10')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('RegisteredAddressLine10', 'spu_wp_addresses', 'Registered_Address_Address10', 0, 'Address - Client', 'Registered Address', 'Address 10', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientRegisteredAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='SiteAddressLine5')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('SiteAddressLine5', 'spu_wp_addresses', 'Site_Address_Address5', 0, 'Address - Client', 'Site Address', 'Address 5', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientSiteAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='SiteAddressLine6')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('SiteAddressLine6', 'spu_wp_addresses', 'Site_Address_Address6', 0, 'Address - Client', 'Site Address', 'Address 6', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientSiteAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='SiteAddressLine7')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('SiteAddressLine7', 'spu_wp_addresses', 'Site_Address_Address7', 0, 'Address - Client', 'Site Address', 'Address 7', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientSiteAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='SiteAddressLine8')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('SiteAddressLine8', 'spu_wp_addresses', 'Site_Address_Address8', 0, 'Address - Client', 'Site Address', 'Address 8', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientSiteAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='SiteAddressLine9')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('SiteAddressLine9', 'spu_wp_addresses', 'Site_Address_Address9', 0, 'Address - Client', 'Site Address', 'Address 9', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientSiteAddress', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='SiteAddressLine10')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('SiteAddressLine10', 'spu_wp_addresses', 'Site_Address_Address10', 0, 'Address - Client', 'Site Address', 'Address 10', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientSiteAddress', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='SubAgentLine5')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('SubAgentLine5', 'spu_wp_addresses', 'Sub_Agent_Address5', 0, 'Address - Client', 'Sub Agent', 'Address 5', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientSubAgent', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='SubAgentLine6')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('SubAgentLine6', 'spu_wp_addresses', 'Sub_Agent_Address6', 0, 'Address - Client', 'Sub Agent', 'Address 6', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientSubAgent', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='SubAgentLine7')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('SubAgentLine7', 'spu_wp_addresses', 'Sub_Agent_Address7', 0, 'Address - Client', 'Sub Agent', 'Address 7', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientSubAgent', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='SubAgentLine8')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('SubAgentLine8', 'spu_wp_addresses', 'Sub_Agent_Address8', 0, 'Address - Client', 'Sub Agent', 'Address 8', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientSubAgent', NULL)
END
GO

IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='SubAgentLine9')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('SubAgentLine9', 'spu_wp_addresses', 'Sub_Agent_Address9', 0, 'Address - Client', 'Sub Agent', 'Address 9', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientSubAgent', NULL)
END
GO
IF NOT EXISTS ( SELECT NULL FROM wp_fields WHERE field_name='SubAgentLine10')
BEGIN
INSERT INTO dbo.wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3, hidden_option_number, required_option_value, sub_group4, loop4, specials_type, Table_Name, DataStructure_Name)
VALUES ('SubAgentLine10', 'spu_wp_addresses', 'Sub_Agent_Address10', 0, 'Address - Client', 'Sub Agent', 'Address 10', 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'CoreAddressClientSubAgent', NULL)
END
GO
DECLARE @caption_id int
IF EXISTS(SELECT NULL FROM Rate_type WHERE Description = 'No Rating')
BEGIN
    EXECUTE spu_pm_caption_id_return 1, 'Flat Rate', @caption_id OUTPUT
    UPDATE Rate_type SET Description = 'Flat Rate', Caption_id = @caption_id WHERE Description = 'No Rating' 
END
-- *****************************************************************************
-- * Author:	Vivek Verma
-- * Purpose:	Corrected spell based on defect #51077
-- *****************************************************************************
UPDATE PMNavXM_Process
SET xml_definition = CAST(REPLACE(CAST(xml_definition as NVarchar(MAX)),'Maintainence','Maintenance') AS TEXT)
WHERE file_name IN ('PFQUOTE.XML','PFQUOTEMTA.XML', 'UNDERMTA.XML', 'UNDERNB.XML', 'UNDERNBPFP.XML', 'UNDERREINS.XML', 'UWCancel.XML')
GO
-- *****************************************************************************
-- * Author:   Amit Tyagi
-- * Date:     27-03-2019
-- * Purpose:  WPR05 paralleling WPR27
-- *****************************************************************************
DECLARE @Caption_ID INT
DECLARE @Risk_Status_ID INT
IF NOT EXISTS(SELECT NULL FROM Risk_Status WHERE Code = 'REVIEWFAC')
BEGIN
	
	EXECUTE spu_PM_Caption_ID_Return 1, 'Unquoted - Review Reinsurance FAC applied', @Caption_Id OUTPUT

	SELECT @Risk_Status_ID = MAX(Risk_Status_ID)+1 FROM Risk_Status

    INSERT INTO Risk_Status (Risk_Status_ID,Caption_ID,Code,Description,Is_Deleted,Effective_Date)
	VALUES (@Risk_Status_ID,@Caption_ID,'REVIEWFAC','Unquoted - Review Reinsurance FAC applied',0,GETDATE())
END

-- *****************************************************************************
-- * Author:   Vivek Verma
-- * Date:     29-04-2019
-- * Purpose:  Defect #46720
-- *****************************************************************************
IF EXISTS(SELECT NULL FROM Risk_Status WHERE Code = 'REVIEWFAC' AND description <> 'Unquoted - Review Reinsurance FAC applied')
BEGIN
	
	EXECUTE spu_PM_Caption_ID_Return 1, 'Unquoted - Review Reinsurance FAC applied', @Caption_Id OUTPUT
    UPDATE Risk_Status SET Description = 'Unquoted - Review Reinsurance FAC applied', Caption_id = @caption_id WHERE Code = 'REVIEWFAC' 
END
GO

IF NOT EXISTS (SELECT NULL from Batch_Type where code = 'COMMX')
BEGIN
	DECLARE @Caption_ID INT,
			@Batch_Type_ID INT
	EXECUTE spu_pm_caption_id_return 1, 'Commission Export', @Caption_ID OUTPUT
	SELECT @Batch_Type_ID = MAX(Batch_Type_ID) + 1 FROM batch_type

	INSERT INTO batch_type (batch_type_id,caption_id,is_deleted,effective_date,description,code)
		VALUES(@Batch_Type_ID, @caption_id, 0, GETDATE(), 'Commission Export', 'COMMX')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_policyagent' AND Field_name='PolAGCode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family, Table_Name )
VALUES ('PolAGCode','spu_wp_policyagent','agent_code','0','Policy','Agent','Agent Code','1','9','CorePolicyAgent')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_PFInstalment' AND Field_name='InstalmentStatusCode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family, loop1)
VALUES ('InstalmentStatusCode','spu_wp_PFInstalment','InstalmentStatusCode','0','Instalment','Instalment Details','Instalment Status Code','1','9','PFInstalment')
END
GO

IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN

    DECLARE @cashListItem_receipt_type_id INT
    DECLARE @lCaptionID INT
 
    -- Instalment Deposit
    IF NOT EXISTS(select code FROM CashListItem_Receipt_Type WHERE code = 'TPPF')
    BEGIN

        -- Add LookupValue
        SELECT @cashListItem_receipt_type_id = MAX(cashListItem_receipt_type_id)+1 FROM CashListItem_Receipt_Type
        EXECUTE spu_pm_caption_id_return 1, 'TP Instalment Debt', @lCaptionID OUTPUT
        INSERT INTO CashListItem_Receipt_Type(cashListItem_receipt_type_id,code, description, caption_id, effective_date,is_deleted, is_Instalment )
    	VALUES(@cashListItem_receipt_type_id,'TPPF','TP Instalment Debt', @lCaptionID, GETDATE(), 0,0 )

    END	    
END
GO
--***********for AZD 96617 for Batch Scheduler*********
DECLARE @pmwork_task_id int,
@caption_id int

IF NOT EXISTS(SELECT NULL from PMWrk_Task where code='BATCHSCHD')
BEGIN
EXECUTE spu_pm_caption_id_return 1,'Scheduler', @caption_id OUTPUT

 INSERT INTO PMWrk_Task
([caption_id],[code],[description],[is_deleted],[effective_date],[is_system_task],[type_of_task],[pmnav_process_id],[component_object_name]
,[component_class_name],[auto_delete_after_num_days],[display_icon],[is_view_only_task],[linked_object_name],[linked_class_name]
,[linked_caption_id],[is_available_task],[pmwrk_task_category_id],[pmnavxm_process_id])
VALUES
(@caption_id
,'BATCHSCHD'
,'Scheduler'
,0
,GETDATE()
,0
,1
,NULL
,'iSIRBatchScheduler','NavigatorV3',0
,1
,0
,NULL,NULL,NULL
,1
,2
,NULL)
 
END
GO
--***********END for AZD 96617 for Batch Scheduler*********

-- *****************************************************************************
-- * Author:	Sumeet Singh
-- * Date:		Jul/2019
-- * Purpose:	Insert the missing export format in Export_Map_Format table. (TFS:50871)
-- *****************************************************************************
If Not Exists (Select * From export_map_detail Where target_field_name = 'EXPRIOUTTQ') Begin
        Declare @id int
    
        -- Insert map detail
        Insert Into export_map_detail (export_map_model_id, export_map_detail_id, target_field_name, sequence)
            Select  1, IsNull(Max(export_map_detail_id), 0) + 1, 'EXPRIOUTTQ', IsNull(Max(sequence), 0) + 1
            From    export_map_detail
    
        Select  @id = export_map_detail_id
        From    export_map_detail 
        Where   target_field_name = 'EXPRIOUTTQ'
    
        -- Insert map formats    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 1, '{account_type_id}', 1, '2 {EXPENSES}', Null, Null, Null, Null, '\', 0)
			    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 2, '{mapping_code}', 2, 'RI Treaty Premium', Null, Null, Null, Null, Null, 0)
End

Go

If Not Exists (Select * From export_map_detail Where target_field_name = 'INCRICOMTQ') Begin
        Declare @id int
    
        -- Insert map detail
        Insert Into export_map_detail (export_map_model_id, export_map_detail_id, target_field_name, sequence)
            Select  1, IsNull(Max(export_map_detail_id), 0) + 1, 'INCRICOMTQ', IsNull(Max(sequence), 0) + 1
            From    export_map_detail
    
        Select  @id = export_map_detail_id
        From    export_map_detail 
        Where   target_field_name = 'INCRICOMTQ'
    
        -- Insert map formats    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 1, '{account_type_id}', 1, '1 {INCOME}', Null, Null, Null, Null, '\', 0)
    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 2, '{mapping_code}', 2, 'RI Treaty Commission', Null, Null, Null, Null, Null, 0)
End
Go

-- *****************************************************************************
-- * Author:	Sumeet Singh
-- * Date:		Aug/2019
-- * Purpose:	Insert the fac address return code for printin FAC address. (TFS:47265)
-- *****************************************************************************
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_ReinsuranceFAC' AND Field_name='FACAddress1')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family, Table_Name )
VALUES ('FACAddress1','spu_wp_ReinsuranceFAC','fac_address1','0','Reinsurance','Fac','FAC Address1','1','9','CoreReinsuranceFAC')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_ReinsuranceFAC' AND Field_name='FACAddress2')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family, Table_Name )
VALUES ('FACAddress2','spu_wp_ReinsuranceFAC','fac_address2','0','Reinsurance','Fac','FAC Address2','1','9','CoreReinsuranceFAC')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_ReinsuranceFAC' AND Field_name='FACAddress3')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family, Table_Name )
VALUES ('FACAddress3','spu_wp_ReinsuranceFAC','fac_address3','0','Reinsurance','Fac','FAC Address3','1','9','CoreReinsuranceFAC')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_ReinsuranceFAC' AND Field_name='FACAddress4')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family, Table_Name )
VALUES ('FACAddress4','spu_wp_ReinsuranceFAC','fac_address4','0','Reinsurance','Fac','FAC Address4','1','9','CoreReinsuranceFAC')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_ReinsuranceFAC' AND Field_name='FACPostalCode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, product_family, Table_Name )
VALUES ('FACPostalCode','spu_wp_ReinsuranceFAC','fac_postal_code','0','Reinsurance','Fac','FAC postal code','1','9','CoreReinsuranceFAC')
END
GO

-- Default User Authorities values to 0 instead of NULL 
UPDATE User_Authorities
SET	can_change_reserves_on_claim_payments = ISNULL(can_change_reserves_on_claim_payments,0),
	has_payments_authority = ISNULL(has_payments_authority,0),
	has_claim_Payments_authority = ISNULL(has_claim_Payments_authority,0),
	has_paynow_write_off_authority = ISNULL(has_paynow_write_off_authority,0),
	is_recommender = ISNULL(is_recommender,0),
	allow_reverse_allocations = ISNULL(allow_reverse_allocations,0)

-- *****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     02-March-2020
-- * Purpose:  New Merge fields for Cash List
-- *****************************************************************************

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_debitcashAllocatedPolicy' AND Field_name='AllocatedPolicyNumber')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, Table_Name )
VALUES ('AllocatedPolicyNumber','spu_wp_debitcashAllocatedPolicy','insurance_ref','0','Receipts/Payments','Allocated Policy Details','Policy Number','1','debitcashAllocatedPolicy','9','CoreDebitCashAllocatedPolicy')
END
GO
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_debitcashAllocatedPolicy' AND Field_name='AllocatedProduct')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, Table_Name )
VALUES ('AllocatedProduct','spu_wp_debitcashAllocatedPolicy','description','0','Receipts/Payments','Allocated Policy Details','Product','1','debitcashAllocatedPolicy','9','CoreDebitCashAllocatedPolicy')
END
GO
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_debitcashAllocatedPolicy' AND Field_name='AllocatedInsuredName')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, Table_Name )
VALUES ('AllocatedInsuredName','spu_wp_debitcashAllocatedPolicy','insured_name','0','Receipts/Payments','Allocated Policy Details','Insured Name','1','debitcashAllocatedPolicy','9','CoreDebitCashAllocatedPolicy')
END
GO
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_debitcashAllocatedPolicy' AND Field_name='AllocatedAmount')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, Table_Name )
VALUES ('AllocatedAmount','spu_wp_debitcashAllocatedPolicy','alloc_ccy_amount','0','Receipts/Payments','Allocated Policy Details','Allocated Amount','1','debitcashAllocatedPolicy','9','CoreDebitCashAllocatedPolicy')
END
GO
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_debitcashAllocatedPolicy' AND Field_name='AllocatedAgentCode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, Table_Name )
VALUES ('AllocatedAgentCode','spu_wp_debitcashAllocatedPolicy','agentcode','0','Receipts/Payments','Allocated Policy Details','Agent Code','1','debitcashAllocatedPolicy','9','CoreDebitCashAllocatedPolicy')
END
GO
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_debitcashAllocatedPolicy' AND Field_name='AllocatedAgentTaxNumber')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, Table_Name )
VALUES ('AllocatedAgentTaxNumber','spu_wp_debitcashAllocatedPolicy','agent_tax_number','0','Receipts/Payments','Allocated Policy Details','Agent Tax Number','1','debitcashAllocatedPolicy','9','CoreDebitCashAllocatedPolicy')
END
GO
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_debitcashAllocatedPolicy' AND Field_name='AllocatedClientCode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, Table_Name )
VALUES ('AllocatedClientCode','spu_wp_debitcashAllocatedPolicy','clientcode','0','Receipts/Payments','Allocated Policy Details','Client Code','1','debitcashAllocatedPolicy','9','CoreDebitCashAllocatedPolicy')
END
GO
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_debitcashAllocatedPolicy' AND Field_name='ClientTaxNumber')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, Table_Name )
VALUES ('ClientTaxNumber','spu_wp_debitcashAllocatedPolicy','client_tax_number','0','Receipts/Payments','Allocated Policy Details','Client Tax Number','1','debitcashAllocatedPolicy','9','CoreDebitCashAllocatedPolicy')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_debitcashAllocatedClaim' AND Field_name='AllocatedClaimNumber')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, Table_Name )
VALUES ('AllocatedClaimNumber','spu_wp_debitcashAllocatedClaim','Claim_Number','0','Receipts/Payments','Allocated Claim Details','Claim Number','1','debitcashAllocatedClaim','9','CoreDebitCashAllocatedClaim')
END
GO
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_debitcashAllocatedClaim' AND Field_name='AllocatedClaimPolicyNumber')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, Table_Name )
VALUES ('AllocatedClaimPolicyNumber','spu_wp_debitcashAllocatedClaim','Policy_Number','0','Receipts/Payments','Allocated Claim Details','Policy Number','1','debitcashAllocatedClaim','9','CoreDebitCashAllocatedClaim')
END
GO
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_debitcashAllocatedClaim' AND Field_name='AllocatedClaimAmount')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, Table_Name )
VALUES ('AllocatedClaimAmount','spu_wp_debitcashAllocatedClaim','alloc_ccy_amount','0','Receipts/Payments','Allocated Claim Details','Allocated Amount','1','debitcashAllocatedClaim','9','CoreDebitCashAllocatedClaim')
END
GO
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_debitcashAllocatedClaim' AND Field_name='AllocatedPrimaryCause')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, Table_Name )
VALUES ('AllocatedPrimaryCause','spu_wp_debitcashAllocatedClaim','primary_cause','0','Receipts/Payments','Allocated Claim Details','Primary Cause','1','debitcashAllocatedClaim','9','CoreDebitCashAllocatedClaim')
END
GO
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_debitcashAllocatedClaim' AND Field_name='AllocatedClaimAgentCode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, Table_Name )
VALUES ('AllocatedClaimAgentCode','spu_wp_debitcashAllocatedClaim','agentcode','0','Receipts/Payments','Allocated Claim Details','Agent Code','1','debitcashAllocatedClaim','9','CoreDebitCashAllocatedClaim')
END
GO
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_debitcashAllocatedClaim' AND Field_name='AllocatedClaimClientCode')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, Table_Name )
VALUES ('AllocatedClaimClientCode','spu_wp_debitcashAllocatedClaim','Client_Code','0','Receipts/Payments','Allocated Claim Details','Client Code','1','debitcashAllocatedClaim','9','CoreDebitCashAllocatedClaim')
END
GO
IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_debitcashAllocatedClaim' AND Field_name='AllocatedClaimClientName')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, Table_Name )
VALUES ('AllocatedClaimClientName','spu_wp_debitcashAllocatedClaim','Client_name','0','Receipts/Payments','Allocated Claim Details','Client Name','1','debitcashAllocatedClaim','9','CoreDebitCashAllocatedClaim')
END
GO

IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE SQL= 'spu_wp_debitcashAllocatedPolicy' AND Field_name='Spare')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, Table_Name )
VALUES ('Spare','spu_wp_debitcashAllocatedPolicy','spare','0','Receipts/Payments','Allocated Policy Details','Spare','1','debitcashAllocatedPolicy','9','CoreDebitCashAllocatedPolicy')
END
GO

-- *****************************************************************************
-- * Author:	Abhishek
-- * Date:		30/mar/2020
-- * Purpose:	Insert the missing export format in Export_Map_Format table. (TFS:61309)
-- *****************************************************************************
If Not Exists (Select * From export_map_detail Where target_field_name = 'CLMRITQTPR') Begin
        Declare @id int
    
        -- Insert map detail
        Insert Into export_map_detail (export_map_model_id, export_map_detail_id, target_field_name, sequence)
            Select  1, IsNull(Max(export_map_detail_id), 0) + 1, 'CLMRITQTPR', IsNull(Max(sequence), 0) + 1
            From    export_map_detail
    
        Select  @id = export_map_detail_id
        From    export_map_detail 
        Where   target_field_name = 'CLMRITQTPR'
    
        -- Insert map formats    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 1, '{account_type_id}', 1, '2 {EXPENSES}', Null, Null, Null, Null, '\', 0)
    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 2, '{mapping_code}', 2, 'RI TTY TP Rec.', Null, Null, Null, Null, Null, 0)
End
GO
If Not Exists (Select * From export_map_detail Where target_field_name = 'CLMRIFXTPR') Begin
        Declare @id int
    
        -- Insert map detail
        Insert Into export_map_detail (export_map_model_id, export_map_detail_id, target_field_name, sequence)
            Select  1, IsNull(Max(export_map_detail_id), 0) + 1, 'CLMRIFXTPR', IsNull(Max(sequence), 0) + 1
            From    export_map_detail
    
        Select  @id = export_map_detail_id
        From    export_map_detail 
        Where   target_field_name = 'CLMRIFXTPR'
    
        -- Insert map formats    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 1, '{account_type_id}', 1, '2 {EXPENSES}', Null, Null, Null, Null, '\', 0)
    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 2, '{mapping_code}', 2, 'RI Other TP Rec.', Null, Null, Null, Null, Null, 0)
End
GO
If Not Exists (Select * From export_map_detail Where target_field_name = 'CLMRITSTPR') Begin
        Declare @id int
    
        -- Insert map detail
        Insert Into export_map_detail (export_map_model_id, export_map_detail_id, target_field_name, sequence)
            Select  1, IsNull(Max(export_map_detail_id), 0) + 1, 'CLMRITSTPR', IsNull(Max(sequence), 0) + 1
            From    export_map_detail
    
        Select  @id = export_map_detail_id
        From    export_map_detail 
        Where   target_field_name = 'CLMRITSTPR'
    
        -- Insert map formats    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 1, '{account_type_id}', 1, '2 {EXPENSES}', Null, Null, Null, Null, '\', 0)
    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 2, '{mapping_code}', 2, 'RI TTY TP Rec.', Null, Null, Null, Null, Null, 0)
End
GO
If Not Exists (Select * From export_map_detail Where target_field_name = 'CLMRITXTPR') Begin
        Declare @id int
    
        -- Insert map detail
        Insert Into export_map_detail (export_map_model_id, export_map_detail_id, target_field_name, sequence)
            Select  1, IsNull(Max(export_map_detail_id), 0) + 1, 'CLMRITXTPR', IsNull(Max(sequence), 0) + 1
            From    export_map_detail
    
        Select  @id = export_map_detail_id
        From    export_map_detail 
        Where   target_field_name = 'CLMRITXTPR'
    
        -- Insert map formats    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 1, '{account_type_id}', 1, '2 {EXPENSES}', Null, Null, Null, Null, '\', 0)
    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 2, '{mapping_code}', 2, 'RI XOL TP Rec', Null, Null, Null, Null, Null, 0)
End
GO
If Not Exists (Select * From export_map_detail Where target_field_name = 'CLMRITXSAL') Begin
        Declare @id int
    
        -- Insert map detail
        Insert Into export_map_detail (export_map_model_id, export_map_detail_id, target_field_name, sequence)
            Select  1, IsNull(Max(export_map_detail_id), 0) + 1, 'CLMRITXSAL', IsNull(Max(sequence), 0) + 1
            From    export_map_detail
    
        Select  @id = export_map_detail_id
        From    export_map_detail 
        Where   target_field_name = 'CLMRITXSAL'
    
        -- Insert map formats    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 1, '{account_type_id}', 1, '2 {EXPENSES}', Null, Null, Null, Null, '\', 0)
    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 2, '{mapping_code}', 2, 'RI XOL Salvage Rec', Null, Null, Null, Null, Null, 0)
End
GO
If Not Exists (Select * From export_map_detail Where target_field_name = 'CLMRITSSAL') Begin
        Declare @id int
    
        -- Insert map detail
        Insert Into export_map_detail (export_map_model_id, export_map_detail_id, target_field_name, sequence)
            Select  1, IsNull(Max(export_map_detail_id), 0) + 1, 'CLMRITSSAL', IsNull(Max(sequence), 0) + 1
            From    export_map_detail
    
        Select  @id = export_map_detail_id
        From    export_map_detail 
        Where   target_field_name = 'CLMRITSSAL'
    
        -- Insert map formats    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 1, '{account_type_id}', 1, '2 {EXPENSES}', Null, Null, Null, Null, '\', 0)
    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 2, '{mapping_code}', 2, 'RI TTY Salvage Rec.', Null, Null, Null, Null, Null, 0)
End
GO
If Not Exists (Select * From export_map_detail Where target_field_name = 'CLMRITQ') Begin
        Declare @id int
    
        -- Insert map detail
        Insert Into export_map_detail (export_map_model_id, export_map_detail_id, target_field_name, sequence)
            Select  1, IsNull(Max(export_map_detail_id), 0) + 1, 'CLMRITQ', IsNull(Max(sequence), 0) + 1
            From    export_map_detail
    
        Select  @id = export_map_detail_id
        From    export_map_detail 
        Where   target_field_name = 'CLMRITQ'
    
        -- Insert map formats    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 1, '{account_type_id}', 1, '1 {INCOME}', Null, Null, Null, Null, '\', 0)
    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 2, '{mapping_code}', 2, 'RI TTY Claims Rec.', Null, Null, Null, Null, Null, 0)
End
GO
If Not Exists (Select * From export_map_detail Where target_field_name = 'CLMOSRITQ') Begin
        Declare @id int
    
        -- Insert map detail
        Insert Into export_map_detail (export_map_model_id, export_map_detail_id, target_field_name, sequence)
            Select  1, IsNull(Max(export_map_detail_id), 0) + 1, 'CLMOSRITQ', IsNull(Max(sequence), 0) + 1
            From    export_map_detail
    
        Select  @id = export_map_detail_id
        From    export_map_detail 
        Where   target_field_name = 'CLMOSRITQ'
    
        -- Insert map formats    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 1, '{account_type_id}', 1, '3 {ASSETS}', Null, Null, Null, Null, '\', 0)
    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 2, '{mapping_code}', 2, 'O/S Claims RI TTY', Null, Null, Null, Null, Null, 0)
End
GO
If Not Exists (Select * From export_map_detail Where target_field_name = 'CLMRITX') Begin
        Declare @id int
    
        -- Insert map detail
        Insert Into export_map_detail (export_map_model_id, export_map_detail_id, target_field_name, sequence)
            Select  1, IsNull(Max(export_map_detail_id), 0) + 1, 'CLMRITX', IsNull(Max(sequence), 0) + 1
            From    export_map_detail
    
        Select  @id = export_map_detail_id
        From    export_map_detail 
        Where   target_field_name = 'CLMRITX'
    
        -- Insert map formats    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 1, '{account_type_id}', 1, '1 {INCOME}', Null, Null, Null, Null, '\', 0)
    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 2, '{mapping_code}', 2, 'RI XOL Claims Rec.', Null, Null, Null, Null, Null, 0)
End

GO
If Not Exists (Select * From export_map_detail Where target_field_name = 'CLMRIFX') Begin
        Declare @id int
    
        -- Insert map detail
        Insert Into export_map_detail (export_map_model_id, export_map_detail_id, target_field_name, sequence)
            Select  1, IsNull(Max(export_map_detail_id), 0) + 1, 'CLMRIFX', IsNull(Max(sequence), 0) + 1
            From    export_map_detail
    
        Select  @id = export_map_detail_id
        From    export_map_detail 
        Where   target_field_name = 'CLMRIFX'
    
        -- Insert map formats    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 1, '{account_type_id}', 1, '1 {INCOME}', Null, Null, Null, Null, '\', 0)
    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 2, '{mapping_code}', 2, 'RI TTY Claims Rec.', Null, Null, Null, Null, Null, 0)
End
GO
-- *****************************************************************************
-- * Author:	Rahul
-- * Date:		21/05/2020
-- * Purpose:	Fixing textcase Inconsistencies of merge fields which causing problem in CCM
-- *****************************************************************************
UPDATE wp_fields SET table_name='CoreClaimPeril' WHERE table_name='Coreclaimperil'
UPDATE wp_fields SET loop1='ClaimPeril' WHERE loop1='claimperil'
UPDATE wp_fields SET Table_Name='CoreClaimPeril_PerilReserve' WHERE Table_Name='CoreClaimPeril_perilreserve'
UPDATE wp_fields SET loop2='PerilReserve' WHERE loop2='perilreserve'
UPDATE wp_fields SET sub_group='' WHERE sub_group IS NULL
UPDATE wp_fields SET Table_Name='CoreClaimPeril_PerilDetails' WHERE main_group='claim' AND sql='spu_wp_perilreserve' AND sub_group='Peril Details'

 IF NOT EXISTS ( SELECT 1 FROM wp_fields WHERE Field_name='DCBankName')
BEGIN 
 INSERT INTO wp_fields (field_name, [sql], column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, product_family, Table_Name )
VALUES ('DCBankName','spu_wp_debitCash','bank_name','0','Receipts/Payments','Cash','Drawee Bank','1',NULL,'9','CoreReceiptsPaymentsCash')
END
GO
-- *****************************************************************************
-- * Author:	Sandeep Kumar
-- * Date:		06/11/2020
-- * Purpose:	Fixing addressess Inconsistencies of merge fields which causing problem in CCM
-- *****************************************************************************
UPDATE wp_fields SET Table_Name = 'CoreAddressClientBillingAddress' WHERE Table_Name = 'CoreAddressPartyBillingAddress'
UPDATE wp_fields SET Table_Name = 'CoreAddressClientBranchAddress' WHERE Table_Name = 'CoreAddressPartyBranchAddress'
UPDATE wp_fields SET Table_Name = 'CoreAddressClientBusinessAddress' WHERE Table_Name = 'CoreAddressPartyBusinessAddress'
UPDATE wp_fields SET Table_Name = 'CoreAddressClientCorrespondenceAddress' WHERE Table_Name = 'CoreAddressPartyCorrespondenceAddress'
UPDATE wp_fields SET Table_Name = 'CoreAddressClientEmailAddress' WHERE Table_Name = 'CoreAddressPartyEmailAddress'
UPDATE wp_fields SET Table_Name = 'CoreAddressClientHomeAddress' WHERE Table_Name = 'CoreAddressPartyHomeAddress'
UPDATE wp_fields SET Table_Name = 'CoreAddressClientOther' WHERE Table_Name = 'CoreAddressPartyOther'
UPDATE wp_fields SET Table_Name = 'CoreAddressClientPreviousAddress' WHERE Table_Name = 'CoreAddressPartyPreviousAddress'
UPDATE wp_fields SET Table_Name = 'CoreAddressClientRegisteredAddress' WHERE Table_Name = 'CoreAddressPartyRegisteredAddress'
UPDATE wp_fields SET Table_Name = 'CoreAddressClientSiteAddress' WHERE Table_Name = 'CoreAddressPartySiteAddress'
UPDATE wp_fields SET Table_Name = 'CoreAddressClientSubAgent' WHERE Table_Name = 'CoreAddressPartySubAgent'
GO
-- *****************************************************************************
-- * Author:	Sumeet Singh
-- * Date:		Jul/2019
-- * Purpose:	Insert the missing export format in Export_Map_Format table. (TFS:50871)
-- *****************************************************************************
If Not Exists (Select * From export_map_detail Where target_field_name = 'EXPRIOUTTQ') Begin
        Declare @id int
    
        -- Insert map detail
        Insert Into export_map_detail (export_map_model_id, export_map_detail_id, target_field_name, sequence)
            Select  1, IsNull(Max(export_map_detail_id), 0) + 1, 'EXPRIOUTTQ', IsNull(Max(sequence), 0) + 1
            From    export_map_detail
    
        Select  @id = export_map_detail_id
        From    export_map_detail 
        Where   target_field_name = 'EXPRIOUTTQ'
    
        -- Insert map formats    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 1, '{account_type_id}', 1, '2 {EXPENSES}', Null, Null, Null, Null, '\', 0)
    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 2, '{mapping_code}', 2, 'RI Treaty Premium', Null, Null, Null, Null, Null, 0)
End

Go

If Not Exists (Select * From export_map_detail Where target_field_name = 'INCRICOMTQ') Begin
        Declare @id int
    
        -- Insert map detail
        Insert Into export_map_detail (export_map_model_id, export_map_detail_id, target_field_name, sequence)
            Select  1, IsNull(Max(export_map_detail_id), 0) + 1, 'INCRICOMTQ', IsNull(Max(sequence), 0) + 1
            From    export_map_detail
    
        Select  @id = export_map_detail_id
        From    export_map_detail 
        Where   target_field_name = 'INCRICOMTQ'
    
        -- Insert map formats    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 1, '{account_type_id}', 1, '2 {EXPENSES}', Null, Null, Null, Null, '\', 0)
    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 2, '{mapping_code}', 2, 'RI Treaty Premium', Null, Null, Null, Null, Null, 0)
End

GO
-- *****************************************************************************
-- * Author:	Sumeet Singh
-- * Date:		Jul/2019
-- * Purpose:	Insert the missing export format in Export_Map_Format table. (TFS:50871)
-- *****************************************************************************
If Not Exists (Select * From export_map_detail Where target_field_name = 'EXPRIOUTTQ') Begin
        Declare @id int
    
        -- Insert map detail
        Insert Into export_map_detail (export_map_model_id, export_map_detail_id, target_field_name, sequence)
            Select  1, IsNull(Max(export_map_detail_id), 0) + 1, 'EXPRIOUTTQ', IsNull(Max(sequence), 0) + 1
            From    export_map_detail
    
        Select  @id = export_map_detail_id
        From    export_map_detail 
        Where   target_field_name = 'EXPRIOUTTQ'
    
        -- Insert map formats    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 1, '{account_type_id}', 1, '2 {EXPENSES}', Null, Null, Null, Null, '\', 0)
    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 2, '{mapping_code}', 2, 'RI Treaty Premium', Null, Null, Null, Null, Null, 0)
End

Go

If Not Exists (Select * From export_map_detail Where target_field_name = 'INCRICOMTQ') Begin
        Declare @id int
    
        -- Insert map detail
        Insert Into export_map_detail (export_map_model_id, export_map_detail_id, target_field_name, sequence)
            Select  1, IsNull(Max(export_map_detail_id), 0) + 1, 'INCRICOMTQ', IsNull(Max(sequence), 0) + 1
            From    export_map_detail
    
        Select  @id = export_map_detail_id
        From    export_map_detail 
        Where   target_field_name = 'INCRICOMTQ'
    
        -- Insert map formats    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 1, '{account_type_id}', 1, '2 {EXPENSES}', Null, Null, Null, Null, '\', 0)
    
        Insert Into export_map_format(export_map_model_id, export_map_detail_id, export_map_format_id, source_field_name, sequence, leading_characters, trailing_characters, start_position, number_of_chars, valid_value, field_separator, is_upper_case)
            Values(1, @id, 2, '{mapping_code}', 2, 'RI Treaty Premium', Null, Null, Null, Null, Null, 0)
End

Go
-- *****************************************************************************
-- * Author:	Garima Garg
-- * Date:		Aug/2020
-- * Purpose:	To Update the caption of Task with Duplicate Description. (TFS:70319)
-- *****************************************************************************
DECLARE @P1 NVARCHAR(50)
DECLARE @Caption VARCHAR(100)
SET @Caption = 'Risk Type Maintenance (Claims)'
	
IF NOT EXISTS(SELECT caption_id FROM Pmcaption WHERE caption = @Caption) 
	BEGIN
		SELECT @P1 = MAX(caption_id) + 1 FROM PMcaption 
		INSERT INTO PMcaption(caption_id,language_id,caption) VALUES(@p1,1,@Caption)
		UPDATE PMWrk_Task SET caption_id = @p1, DESCRIPTION = @caption WHERE Code = 'RISKTYPE'
	END

GO	

Declare @table nvarchar(100),
    @columnIdentityUpd nvarchar(100) 
    DECLARE @CurrentIdentity int 
	Declare @sqlquery nvarchar(250)
Declare @SQL nvarchar(250) Declare cur Cursor  For SELECT 
  OBJECT_NAME(OBJECT_ID) AS TABLENAME,           NAME AS COLUMNNAME FROM  
     SYS.IDENTITY_COLUMNS  ORDER BY 1  
Open cur
Fetch next from cur into @table,@columnIdentityUpd
While @@FETCH_Status= 0 Begin Set @SQL ='SELECT @CurrentIdentity = IDENT_CURRENT('''+ @table +''')' 
      EXEC SP_EXECUTESQL @SQL,N'@CurrentIdentity INT OUTPUT',@CurrentIdentity OUTPUT  
      If @CurrentIdentity = 0
		  DECLARE @CurrentIdentityNew int
		  DECLARE @MaxId INT
		  SET @CurrentIdentityNew = IDENT_CURRENT(@table)
		  IF @CurrentIdentityNew = 0 BEGIN  
		  set @sqlquery=N'select @MaxId = (SELECT  ISNULL(MAX('+ @columnIdentityUpd +'),0)+1 FROM '+ @table + ')' 
		  execute sp_executesql    @sqlquery , N'@MaxId int OUTPUT',     @MaxId = @MaxId output;
		  DBCC CHECKIDENT(@table,reseed,@MaxId)			 
		  END 
         Fetch next from cur into @table,@columnIdentityUpd
End
Close Cur Deallocate cur
GO
EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMSPEXC', 'Allow execute store procedures'
GO
EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMSPEXC'
GO 

IF NOT EXISTS(SELECT NULL FROM BatchProcesses_List WHERE pmwrk_task_id = 335) 
BEGIN
INSERT BatchProcesses_List(description,pmwrk_task_id,is_deleted) VALUES('Batch Renewal',335,0)
END

IF NOT EXISTS(SELECT NULL FROM BatchProcesses_List WHERE pmwrk_task_id = 711) 
BEGIN
	INSERT BatchProcesses_List(description,pmwrk_task_id,is_deleted) VALUES('Chase Cycle',711,0)
END

IF NOT EXISTS(SELECT NULL FROM BatchProcesses_List WHERE pmwrk_task_id = 106) 
BEGIN
	INSERT BatchProcesses_List(description,pmwrk_task_id,is_deleted) VALUES('Credit Control',106,0)
END

IF NOT EXISTS(SELECT NULL FROM BatchProcesses_List WHERE pmwrk_task_id = 223 AND description='Export') 
BEGIN
	INSERT BatchProcesses_List(description,pmwrk_task_id,is_deleted) VALUES ('Export',223,0)
END

IF NOT EXISTS(SELECT NULL FROM BatchProcesses_List WHERE pmwrk_task_id = 223 AND description='Import') 
BEGIN
	INSERT BatchProcesses_List(description,pmwrk_task_id,is_deleted) VALUES ('Import',223,0)
END

IF NOT EXISTS(SELECT NULL FROM BatchProcesses_List WHERE pmwrk_task_id = 19) 
BEGIN
	INSERT BatchProcesses_List(description,pmwrk_task_id,is_deleted) VALUES('Period End',19,0)
End

IF EXISTS(SELECT NULL FROM BatchProcesses_List WHERE pmwrk_task_id IN (219,713)) 
BEGIN
	DELETE FROM BatchProcesses_List WHERE pmwrk_task_id IN (219,713)
END

UPDATE Element SET element_name='Gross Written Premium' WHERE element_name='Gross Written Premiu'



-- *****************************************************************************  
-- * Purpose:      SMTPUserName value for system option
-- *****************************************************************************

DECLARE  @branch_id INT
                      
DECLARE branchCursor SCROLL CURSOR  FOR
SELECT source_id from source
OPEN branchCursor 
FETCH NEXT FROM branchCursor INTO @branch_id
WHILE @@FETCH_STATUS=0

BEGIN
			
	IF EXISTS (SELECT ISNULL(value,0) FROM system_options WHERE option_number = 5244 AND branch_id = @branch_id)
	BEGIN
		IF EXISTS (SELECT ISNULL(value,0) FROM system_options WHERE branch_id = @branch_id AND option_number = 5047 AND value is not null AND value <>'')
		BEGIN
		
			UPDATE 
				system_options 
				SET value = (Select TOP 1 value FROM system_options WHERE option_number = 5047 AND branch_id = @branch_id) 
				WHERE option_number = 5244 AND branch_id = @branch_id
				AND (value IS NULL OR Value = '')
		END
	END 	
	ELSE
	BEGIN 
		IF EXISTS (SELECT ISNULL(value,0) FROM system_options WHERE branch_id = @branch_id AND option_number = 5047 AND value is not null AND value <>'')
			INSERT INTO system_options (branch_id, option_number, value) VALUES (@branch_id,5244,(Select TOP 1 value FROM system_options WHERE option_number = 5047 AND branch_id = @branch_id))
		ELSE
			INSERT INTO system_options (branch_id, option_number, value) VALUES (@branch_id,5244,'')
	END	
		
FETCH NEXT FROM branchCursor INTO @branch_id
END
CLOSE branchCursor
DEALLOCATE branchCursor
GO
-- *****************************************************************************  
-- * Purpose:      For renewal acceptance automatic email
-- *****************************************************************************
IF NOT EXISTS(SELECT * from process_types_docs where process_types_docs_id=14)
BEGIN
	DECLARE @lCaptionID as INT
	IF NOT EXISTS(SELECT * FROM PMCAPTION WHERE Caption='Renewal Acceptance')
	BEGIN
		EXEC spu_pm_caption_id_return 1,'',@lCaptionID output
	END
	ELSE
	BEGIN
		SELECT @lCaptionID = caption_id FROM PMCaption WHERE caption = 'Renewal Acceptance'
	END
	INSERT INTO process_types_docs(process_types_docs_id,caption_id, code,description,is_deleted,effective_date,allow_filtering,allow_split_documents) VALUES(14,@lCaptionID,'RAC','Renewal Acceptance',0,'2007-03-09',1,1)
END
GO

--*****************************************************************************
-- * Author: Madhu Varna
-- * Date:   12/09/2024
-- *****************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGetMedt', 'GetMediaType'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGetMedt'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGetAccB', 'GetAccountBalanceByAccountCode'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGetAccB'
GO
-- *****************************************************************************
-- * Author: Mansi Jain
-- * Date:   13/11/2024
-- *****************************************************************************

UPDATE wp_fields set column_type=2 where field_name ='RPDOCUMENTDATE'

-- *****************************************************************************
-- * Audit Trail
-- * Date:   13/11/2024
-- *****************************************************************************

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=1)
INSERT INTO Audit_Trail_Modules VALUES(1,'Instalment Scheme Maintenance')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=2)
INSERT INTO Audit_Trail_Modules VALUES(2,'Credit Control Maintenance')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=3)
INSERT INTO Audit_Trail_Modules VALUES(3,'Commission Maintenance')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=4)
INSERT INTO Audit_Trail_Modules VALUES(4,'Fee Maintenance')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=5)
INSERT INTO Audit_Trail_Modules VALUES(5,'User Maintenance')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=6)
INSERT INTO Audit_Trail_Modules VALUES(6,'Agent Maintenance')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=7)
INSERT INTO Audit_Trail_Modules VALUES(7,'Reinsurer Maintenance')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=8)
INSERT INTO Audit_Trail_Modules VALUES(8,'Treaty Maintenance')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=9)
INSERT INTO Audit_Trail_Modules VALUES(9,'RI Model Maintenance')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=10)
INSERT INTO Audit_Trail_Modules VALUES(10,'Maintain System Options')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=11)
INSERT INTO Audit_Trail_Modules VALUES(11,'Maintain Currency Rates')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=12)
INSERT INTO Audit_Trail_Modules VALUES(12,'User Maintainable Rate Tables')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=13)
INSERT INTO Audit_Trail_Modules VALUES(13,'Lookup Maintenance')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=14)
INSERT INTO Audit_Trail_Modules VALUES(14,'Account Executive')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=15)
INSERT INTO Audit_Trail_Modules VALUES(15,'Account Handler')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=16)
INSERT INTO Audit_Trail_Modules VALUES(16,'Discount Maintenance')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=17)
INSERT INTO Audit_Trail_Modules VALUES(17,'Extra Maintenance')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=18)
INSERT INTO Audit_Trail_Modules VALUES(18,'Branch Maintenance')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=19)
INSERT INTO Audit_Trail_Modules VALUES(19,'Bank Maintenance')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=20)
INSERT INTO Audit_Trail_Modules VALUES(20,'Currency Maintenance')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=21)
INSERT INTO Audit_Trail_Modules VALUES(21,'Other Party Maintenance')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=22)
INSERT INTO Audit_Trail_Modules VALUES(22,'Agent Group Maintenance')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=23)
INSERT INTO Audit_Trail_Modules VALUES(23,'Maintain User Authority Level')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=24)
INSERT INTO Audit_Trail_Modules VALUES(24,'Maintain Numbering Schemes')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=25)
INSERT INTO Audit_Trail_Modules VALUES(25,'Licence Administrator')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=26)
INSERT INTO Audit_Trail_Modules VALUES(26,'Maintain Document Templates')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=27)
INSERT INTO Audit_Trail_Modules VALUES(27,'Risk Type Maintenance')

IF NOT EXISTS(SELECT NULL FROM Audit_Trail_Modules WHERE Modules_id=28)
INSERT INTO Audit_Trail_Modules VALUES(28,'Product Risk Maintenance')

EXEC DDLAddOrAlterExtendedProperty 'pfscheme','EndDate','End Date'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','SchemeName','Scheme Name'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','StartDate','Start Date'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','QuoteableInd','Enabled'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','deposit_as_instalment','Deposit Will Be Transacted As An Instalment'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','is_plan_reference_editable','Plan Reference Editable'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','deposit_on_other_media_type','Deposit On A Different Media Type'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','spread_commission','Spread Commission'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','SchemeDescription','Scheme Description'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','spread_taxes','Spread Taxes'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','spread_ri','Spread Re-Insurance'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','Spread_subagent_Commission','Spread Sub Agent Commission'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','mediatype_id','Media Type'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','pfscheme_printtype_id','Documents-When To Print'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','CreditDocID','Documents-Credit Detail'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','QuoteDocID','Documents-Quote'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','ConfirmationDocID','Documents-Confirmation'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','ColNotDocID','Documents-Collection Notification'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','BankDocID','Documents-Collection Form'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','bankaccount_id','Bank Account'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','suspense_account_id','Revenue Account'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','interest_account_id','Interest Account'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','protection_account_id','Protection Account'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','commission_subagent_suspense_account_id','Sub Agent Commission Account'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','tax_group_id','Tax Group'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','ColNotNumDays','Collection Notification Days'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','pffrequency_id','Frequency'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','backdated_rollup_to','Backdated Instalments'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','align_to','Align Instalments'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','DaysDelay','First Instalment Adjustment Between'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','start_limit','First Instalment Adjustment And'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','DepositPC','Deposit'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','deposit_charged_to','Deposit Included In Interest Charge'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','bank_name_mandatory','Bank Name'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','bank_address_mandatory','Bank Address'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','branch_name_mandatory','Branch Name'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','branch_code_mandatory','Branch Code'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','ri_suspense_account_id','Re-Insurance Account'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','commission_suspense_account_id','Commission Account'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','direct_debit_supplier_name','Direct Debit Supplier For XML Export'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','direct_debit_supplier_id','Direct Debit Supplier ID For XML Export'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','remitter','Remitter For XML Export'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','financial_institution_code','Financial Institution Code For XML Export'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','processing_days','Processing Days For XML Export'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','EndDate','End Date'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','Mnemonic','Code'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','ArrangementFee','Arrangement Fee'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','ProtectRate','Protection Fee'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','protection_type','Protection Fee Type'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','fee_charged_to','Arrangement Fee Charged To'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','fee_type','Arrangement Fee Type'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','protection_charged_to','Protection Fee Charged To'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','deposit_type','Deposit Type'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','apply_fee_percentages_to_fees','Fee Percentages Applied To Policy/Risk Fees'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','apply_fee_percentages_to_taxes','Fee Percentages Applied To Policy/Risk Taxes'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','is_deposit_override_allowed','Deposit Override Allowed'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','existing_days_delay','Existing Agreement Days Delay'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','first_instalment_align_with_day_in_month','First Instalment Align With Day In Month'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','single_instalment_per_month','Single Instalment Per Month'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','retry_limit','Retry Limit'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','recollect_days','Recollection Of The Failed Instalment Days Later'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','recollect_on_next','Recollection Of The Failed Instalment On Next Instalment Date'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','Min1','Charges1: Min Amount'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','Max1','Charges1: Max Amount'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','Rate1','Charges1: Rate'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','R1Com','Charges1: Commission %'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','Min2','Charges2: Min Amount'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','Max2','Charges2: Max Amount'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','Rate2','Charges2: Rate'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','R2Com','Charges2: Commission %'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','Min3','Charges3: Min Amount'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','Max3','Charges3: Max Amount'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','Rate3','Charges3: Rate'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','R3Com','Charges3: Commission %'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','Min4','Charges3: Min Amount'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','Max4','Charges4: Max Amount'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','Rate4','Charges4: Rate'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','R4Com','Charges4: Commission %'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','Min5','Charges5: Min Amount'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','Max5','Charges5: Max Amount'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','Rate5','Charges5: Rate'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','R5Com','Charges5: Commission %'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','MinInterest','Minimum Interest'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','maximum_instalments','Maximum Instalments'
EXEC DDLAddOrAlterExtendedProperty 'pfscheme','receipt_difference_option','Receipt Difference'
EXEC DDLAddOrAlterExtendedProperty 'PFRF','finance_net_commission','Finance Net Commission'

EXEC DDLAddOrAlterExtendedProperty 'Commission_Arrangement','Product_id','Product'
EXEC DDLAddOrAlterExtendedProperty 'Commission_Arrangement','commission_band_id','Commission Band'
EXEC DDLAddOrAlterExtendedProperty 'Commission_Arrangement','risk_type_id','Risk Type'
EXEC DDLAddOrAlterExtendedProperty 'Commission_Arrangement','transaction_type_id','Transaction Type'
EXEC DDLAddOrAlterExtendedProperty 'Commission_Arrangement','commission_grouping','Commission Group'
EXEC DDLAddOrAlterExtendedProperty 'Commission_Arrangement','tax_group_id','Tax Group'
EXEC DDLAddOrAlterExtendedProperty 'Commission_Arrangement','party_cnt','Party'
EXEC DDLAddOrAlterExtendedProperty 'Commission_Arrangement','commission_level_id','Commission Level' 
EXEC DDLAddOrAlterExtendedProperty 'Commission_Arrangement','Party_type','Party Type' 
EXEC DDLAddOrAlterExtendedProperty 'Commission_Arrangement','effective_date','Effective Date' 
EXEC DDLAddOrAlterExtendedProperty 'Commission_Arrangement','Maximum_rate','Maximum Rate' 
EXEC DDLAddOrAlterExtendedProperty 'Commission_Arrangement','is_value','Rate Is Value'
EXEC DDLAddOrAlterExtendedProperty 'Commission_Arrangement','rate','Rate'
EXEC DDLAddOrAlterExtendedProperty 'Commission_Arrangement','is_deleted','Is Deleted'

EXEC DDLAddOrAlterExtendedProperty 'RI_Model','effective_date','Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model','code','Code'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model','expiry_date','Expiry Date'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model','ri_model_type','RI Model Type'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model','fac_premium_type','FAC Premiums'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model','claim_allocation_type','Claim Allocation'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model','currency_id','Currency'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model','description','Description'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model','xol_clm_ri_model_id','Claim XOL Model'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model','xol_clm_limit','Claim XOL Limit'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model','xol_cat_ri_model_id','Catastrophe XOL Model'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model','xol_cat_limit','Catastrophe XOL Limit'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model','xol_cat_reinstatements','Reinstatements'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model','is_deleted','RI Model Deleted'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model','treaty_premium_type','Treaty Premium Type'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model_Line','ri_model_line_id','RIModel Line'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model_Line','ri_model_id','RIModel'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model_Line','priority','Priority'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model_Line','number_of_lines','Number Of Lines'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model_Line','line_limit','Line/Upper Limit'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model_Line','treaty_id','Treaty'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model_Line','share_percent','Share %'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model_Line','lower_limit','Lower Limit'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model_Line','Treaty_Type_id','Treaty Type'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model_Line','Is_Obligatory','Is Obligatory'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model_Line','cede_premium_only','Cede Premium Only'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model_Line','ceding_rate','Ceding Rate'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model_Line','premium_calculation_basis_Id','Premium Calculation Basis'
EXEC DDLAddOrAlterExtendedProperty 'RI_Model_Line','Is_VariableQuotaShare','Is Variable Quota Share'
EXEC DDLAddOrAlterExtendedProperty 'RIModelCurrencyRates','conversion_rate','Model Currency'

EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Rule','description','Description'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Rule','source_id','Branch'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Rule','business_type','Business Type'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Rule','is_active','Is Active'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Rule','processing_days','Processing Days'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Rule','product_id','Product'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Rule','policy_is_paid','Paid Position'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Rule','pffrequency_id','Frequency'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Rule','use_inception_date','Use Inception Date For Cancellation'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Rule','use_effective_date','Use Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Rule','use_greater_of_transaction_and_effective_date','Use The Greater Of Transaction Or Policy Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Rule','use_due_date','Use Due Date'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Rule','pfinstalments_result_id','Payment Failed Reason'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','broker_letter_id','Broker Letter'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','single_instalment_broker_letter_id','Single Instalment Broker Letter'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','check_auto_cancel','Check Auto-Cancellation Rules'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','auto_cancel_policy','Run Auto-Cancel Rules'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','stop_account','Stop Account'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','instalment_failure_count','Instalment Failure Count'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','recurring_days','Recurring Days'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','recurring_letters','Re-Print Letters'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','client_document_template_id','Client Letter'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','write_off_tolerance','Write-Off Tolerance Amount'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','write_off_reason_id','Write-Off Reason'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','pmwrk_task_id','Task'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','pmwrk_task_group_id','Task Group'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','pmuser_group_id','User Group'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','auto_cancel_document_1_trigger_amount','If Oustanding Balance>='
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','auto_cancel_document_2_trigger_amount','If Oustanding Balance<='
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','auto_cancel_document_1_template_id','Auto Cancellation Document 1'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','auto_cancel_document_2_template_id','Auto Cancellation Document 2'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','jump_to_next_step_broker','Broker Business-Jump To Next Step'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','jump_to_next_step','Direct Business-Jump To Next Step'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','broker_days','Broker Business-Elapsed Days After Jump'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','number_of_days','Direct Business-Elapsed Days After Jump'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','account_tolerance_amount','Agents Account Tolerance Amount'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','policy_tolerance_amount','Policy Tolerance Amount'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','step_description','Step Description'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','step_number','Step'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','next_step','Next Step'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','previous_step','Previous Step'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','off_hold_step','Off-Hold Step'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','single_instalment_jump_to_next_step_broker','Single Instalment-Jump To Next Step'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','single_instalment_account_tollerance_amount','Single Instalment-Account Tolerance Amount'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','single_instalment_account_number_of_days','Single Instalment-Elapsed Days After Jump'
EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Step','auto_lapse_renewal','Auto Lapse Renewal'

EXEC DDLAddOrAlterExtendedProperty 'Treaty','code','Code'
EXEC DDLAddOrAlterExtendedProperty 'Treaty','description','Description'
EXEC DDLAddOrAlterExtendedProperty 'Treaty','effective_date','Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'Treaty','expiry_date','Expiry Date'
EXEC DDLAddOrAlterExtendedProperty 'Treaty','agreement_code','Agreement Code'
EXEC DDLAddOrAlterExtendedProperty 'Treaty','replaced_by_effective_date','Replaced By Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'Treaty','replaces_treaty_id','Replaces Treaty'
EXEC DDLAddOrAlterExtendedProperty 'Treaty','replaced_by_treaty_id','Replaced By'
EXEC DDLAddOrAlterExtendedProperty 'Treaty','is_deleted','Is Deleted'
EXEC DDLAddOrAlterExtendedProperty 'Treaty','reinsurance_type_id','Insurance Type'
EXEC DDLAddOrAlterExtendedProperty 'Treaty_Party','share_percent','Share %'
EXEC DDLAddOrAlterExtendedProperty 'Treaty_Party','commission_percent','Commission %'
EXEC DDLAddOrAlterExtendedProperty 'Treaty_Party','party_cnt','Insurer'
EXEC DDLAddOrAlterExtendedProperty 'Treaty_Party','is_Reinsurer_Approved','Is Reinsurer Approved'

EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','has_unrestricted_enquiry','User Has Unrestricted Enquiry Access'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','has_unrestricted_update','User Has Unrestricted Update Access'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','ManualJournal_currency_id','User Has A Manual Journal Limit Currency'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','ManualJournal_currency_amount','User Has A Manual Journal Limit Currency Amount'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','has_ManualJournal_authority','User Has A Manual Journal Limit'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_user_debug_dynamic_logic_scripts','User Can Debug Dynamic Logic With CTRL-ALT-F12'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','user_server_scripts_run_in_debug','Server Rules Run In Debug Mode For This User'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','has_ViewBatchProcessStatus','User Can View Batch Process status'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','loss_gain_currency_id','Loss/Gain Write-off Currency'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','has_claim_payments_authority','User Has Claim Payments Authority'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','claim_payments_amount','User Has Claim Payments Amount'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','claims_payments_currency_id','User Has Claim Payments Currency'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','has_payments_authority','User Has A Payment limit'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','payments_amount','User Has A Payment limit Amount'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','payments_currency_id','User Has A Payment limit Currency'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','is_recommender','Recommender'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','recommender_currency_id','Recommender Currency'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','recommender_currency_amount','Recommender Amount'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_change_reserves_on_claim_payments','User Can Change Reserves'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_perform_broker_transfer','User Can Perform Broker Transfer'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','allow_ratingsection_adddelete','User Can Add/Remove Rating Sections'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','allow_ratingsection_editing','User Can Edit Existing Rating Sections'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','display_reinsurance','Display Reinsurance Screen'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','display_claim_reinsurance','Display Claim Reinsurance'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_make_live_invoice','Invoice'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_make_live_instalments','Instalments'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_make_live_paynow','Pay Now'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_make_live_cashdeposit','Cash Deposit'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_make_live_BankGuarantee','Bank Guarantee'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','has_paynow_write_off_authority','User Can Perform Pay Now Write-Offs'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','paynow_write_off_currency_id','User Can Perform Pay Now Write-Offs Currency'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','paynow_write_off_amount','User Can Perform Pay Now Write-Offs Amount'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','Edit_Default_Commission','Edit Default Commission'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','Edit_Default_Commission_NB_RN','Transaction Type NB/RN'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','Edit_Default_Commission_MTA','Transaction Type MTA'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','Edit_Default_Commission_MTC','Transaction Type MTC'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','Edit_Default_Commission_MTR','Transaction Type MTR'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','Agent_Editable_During_MTA_MTC','Agent Editable During MTA/MTC'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_change_instalment_default_currency','Can Change Instalment Plan Default Currency'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','out_of_sequence_mta_authority','Backdate MTA/MTC/MTR Authority'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_change_exchange_date','Exchange Date On Policy Screen'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_change_exchange_rate','Exchange Rate On Policy Screen'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_override_duplicate_claims','Duplicate Claim Override'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_override_posting_period','Posting Period'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_override_cheque_Numbers','Cheque Numbers'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_edit_instalment_date','User Can Edit Instalment Due Dates'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','edit_instalment_by_no_of_days','Number Of Days'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_update_instalment_status','Instalment Status'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','allow_reverse_allocations','User Has Authority To Reverse Allocations'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','reverse_allocations_days','Time Period (Number Of Days)'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','allow_receipt_reversal','User Has Authority To Reverse The Receipts (SRP)'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_update_instalment_status','Instalment Status'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','is_perform_allocations','User Can Perform Allocation Write-Offs'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','write_off_currency_id','User Can Perform Allocation Write-Offs Currency'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','write_off_amount','User Can Perform Allocation Write-Offs Amount'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','Currency_Loss_Gain_Limit','Loss/Gain Write-Off Amount'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_change_prepolicy_exchange_date','Exchange Date On Other Multi-Currency Screens'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_change_prepolicy_exchange_rate','Exchange Rate On Other Multi-Currency Screens'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','can_backdate_collection_date','Collection Date on Cash/Cheque Reciepts'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','is_view_only_client_manager','Is View Clients'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','is_view_only_agents_maintenance','Is View Only Agent Information'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','is_view_only_account_handler_maintenance','Is View Only Account Handler Information'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','is_view_only_account_executive_maintenance','Is View Only Executives Information'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','is_view_only_insurer_maintenance','Is View Only Reinsurers/Insurers Inofrmation'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','is_view_only_other_party_maintenance','Is View Only Other Party Information'
EXEC DDLAddOrAlterExtendedProperty 'User_Authorities','has_write_off_authority','User Can Perform Allocations Write-Offs'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','is_printer_changeable','Changeable?'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','email_address','Email Address'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','initials','Initials/Ref'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','full_name','Full Name'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','title','Title'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','telephone_number','Direct Dial Number'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','mobile_number','Mobile Number'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','extension_number','Extn'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','fax_number','Fax Number'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','lastlogin','Last Login'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','job_basis','Job Basis'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','percent_hours_worked','Percentage Of Normal Hours Worked'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','signature_file','Digital Signature'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','effective_date','Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','username','User Name'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','server_printer','Printer Name'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','sirius_user','Sirius User'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','alternative_identifier','Domain Account User'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','password_change_date','Password Last Changed'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','is_deleted','User Deleted'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','claim_handler_id','User Type Claims Handler?'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','other_party_id','User Type Other Party?'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','sso_preferred_username','SSPO Preferred User Name'
EXEC DDLAddOrAlterExtendedProperty 'PMUser','logged_on_at_client','Logged On At Client'
-- User Maintainable Rate Table
EXEC DDLAddOrAlterExtendedProperty 'GIS_user_def_header_inds','code','LookUp Indicator Code'
EXEC DDLAddOrAlterExtendedProperty 'GIS_user_def_header_inds','description','LookUp Indicator Description'
EXEC DDLAddOrAlterExtendedProperty 'GIS_user_def_header_inds','is_deleted','LookUp Indicator Is Deleted'
EXEC DDLAddOrAlterExtendedProperty 'GIS_User_Def_Detail','code','LookUp Detail Code'
EXEC DDLAddOrAlterExtendedProperty 'GIS_User_Def_Detail','description',' LookUp Detail Description'
EXEC DDLAddOrAlterExtendedProperty 'GIS_User_Def_Detail','is_deleted','LookUp Detail Deleted'
EXEC DDLAddOrAlterExtendedProperty 'GIS_User_Def_Detail','Parent','LookUp Detail Parent'
EXEC DDLAddOrAlterExtendedProperty 'GIS_User_Def_Detail','GIS_user_def_header_inds_id','Indicator'
EXEC DDLAddOrAlterExtendedProperty 'GIS_user_def_header_inds','GIS_user_def_header_inds_id','LookUp Indicator Description'
EXEC DDLAddOrAlterExtendedProperty 'GIS_user_def_header','description','LookUp Header Description'
EXEC DDLAddOrAlterExtendedProperty 'GIS_user_def_header','Parent','LookUp Header Parent'
EXEC DDLAddOrAlterExtendedProperty 'GIS_user_def_header','is_deleted','LookUp Header Is Deleted'

--Fee Maintenance
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','product_id','Product/Risk/Peril Group'
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','peril_group_id','Product/Risk/Peril Group'
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','risk_type_group_id','Product/Risk/Peril Group'
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','fee_percentage','Percentage'
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','fee_amount','Value'
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','effective_date','Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','include_fee_in_instalments','Is Include Fee In Instalment'
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','is_fee_applied_to_cr','Apply To Credit Transaction'
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','spread_fee_across_instalments','Is Spread The Fee Across Instalment'
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','tax_group_id','Tax Group'
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','currency_id','Currency'
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','Is_Prorated','Apply Pro-Rated?'
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','Is_Override','Override Rate/Amount'
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','Use_When_Deleted','Do You Want To Apply The Fee To OOS Transactions When Deleted?'
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','Calculation_Basis','Calculation Basis'
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','MakeLiveOptions_id','Payment Method'
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','DoPaymentTerms_id','Debit Order Payment Terms'
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','transaction_sub_type','Effective Transactions'

EXEC DDLAddOrAlterExtendedProperty 'party_insurer','report_indicator','Report Indicator'
EXEC DDLAddOrAlterExtendedProperty 'party_insurer','binder_indicator','Binder Indicator'
EXEC DDLAddOrAlterExtendedProperty 'party_insurer','terms_of_payment_id','Terms Of Payment'
EXEC DDLAddOrAlterExtendedProperty 'party_insurer','is_reinsurance_debit_credit_no','Insurance Debit Credit Note?'
EXEC DDLAddOrAlterExtendedProperty 'party_insurer','default_comm_rate','Default Commission'
EXEC DDLAddOrAlterExtendedProperty 'party_insurer','agency_number','Binding Authority'
EXEC DDLAddOrAlterExtendedProperty 'party_insurer','reinsurance_type','Insurance Type'
EXEC DDLAddOrAlterExtendedProperty 'party_insurer','domiciled_for_tax','Is Domiciled For Tax'
EXEC DDLAddOrAlterExtendedProperty 'party_insurer','tax_group_id','Tax Group'
EXEC DDLAddOrAlterExtendedProperty 'party_insurer','is_ri_broker','RI Broker'
EXEC DDLAddOrAlterExtendedProperty 'party_insurer','is_retained','Is Retained'

EXEC DDLAddOrAlterExtendedProperty 'Party_Certificate_Year','CertYearCode','Code'
EXEC DDLAddOrAlterExtendedProperty 'Party_Certificate_Year','CertYearDescription','Description'
EXEC DDLAddOrAlterExtendedProperty 'Party_Certificate_Year','CertYearStartDate','Start Date'
EXEC DDLAddOrAlterExtendedProperty 'Party_Certificate_Year','CertYearEndDate','End Date'

EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','account_holder_name','Account/Card Holders Name'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','account_number','Account Number'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','account_type','Account Type'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','bank_branch_code','Bank Branch Code'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','bank_branch','Bank Branch'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','bank_name_id','Bank Name'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','business_identifier_code','BIC'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','international_bank_account_number','IBAN'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','bank_add1','No./Name Street'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','bank_add2','Locality'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','bank_town','Post Town'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','bank_pcode','Post Code'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','bank_region','County'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','bank_country','Country'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','cc_num','Card Number'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','cc_start_date','Start Date'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','cc_expiry_date','Expiry Date'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','cc_issue_num','Issue Number'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','cc_pin','CSV/PIN'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','cc_add1','No./Name Street'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','cc_add2','Locality'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','cc_add3','County'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','cc_town','Post Town'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','cc_pcode','Post Code'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','cc_country','Country'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','name_on_card','Name On Card'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','manual_auth_number','Manual Auth'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','is_registered','Registered Card Holder'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','is_deleted','Bank Active/Inactive'
EXEC DDLAddOrAlterExtendedProperty 'Party_Bank','is_bank','Bank Account/Credit Card'

EXEC DDLAddOrAlterExtendedProperty 'Credit_Control_Rule_Insurance_File_Status','insurance_file_status_id','Policy Status Filters'

--Agent Maintenance
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','agency_next_review_date','Next Review Date'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','agency_account_number','Account Number'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','trading_name','Name'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','binder_indicator','Binding Indicator'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','report_indicator','Report Indicator'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','is_single_instalment_plan','Single Instalment Plan Only'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','common_renewal_date','Common Renewal Date'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','use_override_commission_rate','Use Override Commission Rate'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','party_agent_origin_id','Source'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','bankaccount_id','Bank Account'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','payment_method','Payment Method'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','payment_frequency','Payment Frequency'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','allow_consolidated_commission','Allow Consolidated Commissions'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','can_make_live_invoice','Invoice'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','can_make_live_instalments','Instalments'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','can_make_live_paynow','Pay Now'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','can_make_live_BankGuarantee','Bank Guarantee'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','can_make_live_cashdeposit','Cash Deposit'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','produce_agent_renewal_list','Product Agent Renewal List'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','address_on_notice','Address On Notice'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','title','Title'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','contact_person','Contact Person'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','first_name','First Name'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','date_cancelled','Date Cancelled'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','is_in_transfer_mode','Broker In Transfer Mode'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','transfer_to_business_type_id','Broker/Agent Business Type'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','transfer_to_party_cnt','Broker/Agent Code'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','alternate_reference_mandatory','Alternative Reference Mandatory'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','alternate_reference_for_each_transaction','Alternative Reference For Each Transaction'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','Receives_Client_Correspondence','Receives Client Correspondence'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','party_agent_type_id','Agent Type'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','linked_account_group','Agent Group Code'
EXEC DDLAddOrAlterExtendedProperty 'Party_Agent','linked_account_executive_id','Agent Account Executive Code'

EXEC DDLAddOrAlterExtendedProperty 'Agent_Commission_Level','Commission_level_id','Agent Type'
EXEC DDLAddOrAlterExtendedProperty 'Agent_Commission_Level','Effective_date','Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'Agent_Commission_Level','Is_deleted','Agent Commission Level Deleted'

EXEC DDLAddOrAlterExtendedProperty 'Address','address1','Address1'
EXEC DDLAddOrAlterExtendedProperty 'Address','address2','Address2'
EXEC DDLAddOrAlterExtendedProperty 'Address','address3','Address3'
EXEC DDLAddOrAlterExtendedProperty 'Address','address4','Address4'
EXEC DDLAddOrAlterExtendedProperty 'Address','postal_code','Post Code'
EXEC DDLAddOrAlterExtendedProperty 'Address','country_id','Country'

EXEC DDLAddOrAlterExtendedProperty 'Contact','area_code','Area Code'
EXEC DDLAddOrAlterExtendedProperty 'Contact','number','Number'
EXEC DDLAddOrAlterExtendedProperty 'Contact','extension','Extension'
EXEC DDLAddOrAlterExtendedProperty 'Contact','description','Description'
EXEC DDLAddOrAlterExtendedProperty 'Contact','contact_type_id','Contact Type'

EXEC DDLAddOrAlterExtendedProperty 'Party','tax_number','Tax Number'
EXEC DDLAddOrAlterExtendedProperty 'Party','domiciled_for_tax','Is Domiciled For Tax'
EXEC DDLAddOrAlterExtendedProperty 'Party','tax_exempt','Tax Exempt'
EXEC DDLAddOrAlterExtendedProperty 'Party','tax_percentage','Tax Percentage'
EXEC DDLAddOrAlterExtendedProperty 'Party','payment_term_code','Terms Of Payment'
EXEC DDLAddOrAlterExtendedProperty 'Party','currency_id','Currency'

EXEC DDLAddOrAlterExtendedProperty 'Party_Relationship','relation_cnt','AssociateName'
EXEC DDLAddOrAlterExtendedProperty 'Party_Relationship','relationship_type_id','Relationship'

EXEC DDLAddOrAlterExtendedProperty 'Party_Address_Usage','address_usage_type_id','Type'
EXEC DDLAddOrAlterExtendedProperty 'Fee_Amounts','is_deleted','Fee Deleted'

EXEC DDLAddOrAlterExtendedProperty 'Party_Handler','forename','Forename'
EXEC DDLAddOrAlterExtendedProperty 'Party_Handler','initials','Initials'
EXEC DDLAddOrAlterExtendedProperty 'Party_Handler','party_title_code','Title'
EXEC DDLAddOrAlterExtendedProperty 'Party_Handler','department_id','Department'


---Branch Maintenance

EXEC DDLAddOrAlterExtendedProperty 'Source','code','Branch Code'
EXEC DDLAddOrAlterExtendedProperty 'Source','description','Branch Name'
EXEC DDLAddOrAlterExtendedProperty 'Source','is_deleted',' '
EXEC DDLAddOrAlterExtendedProperty 'Source','reg_no_1','Registration No. 1'
EXEC DDLAddOrAlterExtendedProperty 'Source','reg_no_2','Registration No. 2'
EXEC DDLAddOrAlterExtendedProperty 'Source','address1','Line 1'
EXEC DDLAddOrAlterExtendedProperty 'Source','address2','Line 2'
EXEC DDLAddOrAlterExtendedProperty 'Source','address3','Town'
EXEC DDLAddOrAlterExtendedProperty 'Source','address4','Region'
EXEC DDLAddOrAlterExtendedProperty 'Source','postal_code','Postal Code'
EXEC DDLAddOrAlterExtendedProperty 'Source','country_id','Country'
EXEC DDLAddOrAlterExtendedProperty 'Source','phone_area_code','Telephone No : Area Code'
EXEC DDLAddOrAlterExtendedProperty 'Source','phone_number','Telephone No : Number'
EXEC DDLAddOrAlterExtendedProperty 'Source','phone_extension','Telephone No : Extension'
EXEC DDLAddOrAlterExtendedProperty 'Source','fax_area_code','Fax No : Area Code'
EXEC DDLAddOrAlterExtendedProperty 'Source','fax_number','Fax No : Number'
EXEC DDLAddOrAlterExtendedProperty 'Source','fax_extension','Fax No : Extension'
EXEC DDLAddOrAlterExtendedProperty 'Source','email','Email'
EXEC DDLAddOrAlterExtendedProperty 'Source','vat_no','Vat Number'
EXEC DDLAddOrAlterExtendedProperty 'Source','sender_mailbox_id','Sender Mailbox Id'
EXEC DDLAddOrAlterExtendedProperty 'Source','broker_abi_id','Broker Abi Id'
EXEC DDLAddOrAlterExtendedProperty 'Source','user_licence_id','User Licence Id'
EXEC DDLAddOrAlterExtendedProperty 'Source','pm_company_number','Company No'
EXEC DDLAddOrAlterExtendedProperty 'Source','default_indicator','Default Indicator'

-- Maintain User Authority Level
EXEC DDLAddOrAlterExtendedProperty 'PMUser_Authority_Level','authority_level_type_id','Authority Level'

-- Maintain Numbering Scheme
EXEC DDLAddOrAlterExtendedProperty 'Numbering_Scheme','code','Code'
EXEC DDLAddOrAlterExtendedProperty 'Numbering_Scheme','description','Description'
EXEC DDLAddOrAlterExtendedProperty 'Numbering_Scheme','numbering_scheme_type_id','Business Type'
EXEC DDLAddOrAlterExtendedProperty 'Numbering_Scheme','numbering_scheme','Numbering Scheme'
EXEC DDLAddOrAlterExtendedProperty 'Numbering_Scheme','is_generated','Generated'
EXEC DDLAddOrAlterExtendedProperty 'Numbering_Scheme','mask_code','Mask Code'
EXEC DDLAddOrAlterExtendedProperty 'Numbering_Scheme','fixed_code','Fixed Code'
EXEC DDLAddOrAlterExtendedProperty 'Numbering_Scheme','next_number','Next Number'
EXEC DDLAddOrAlterExtendedProperty 'Numbering_Scheme','highest_number','Highest Number'
EXEC DDLAddOrAlterExtendedProperty 'Numbering_Scheme','step','Step'
EXEC DDLAddOrAlterExtendedProperty 'Numbering_Scheme','numbering_scheme','Numbering Scheme'
EXEC DDLAddOrAlterExtendedProperty 'Numbering_Scheme','party_type_id','Party Type'
EXEC DDLAddOrAlterExtendedProperty 'Numbering_Scheme','is_reuse_abandoned','Reuse Abandoned Numbers'
EXEC DDLAddOrAlterExtendedProperty 'Numbering_Scheme','is_reset_daily','Reset Number Daily'
EXEC DDLAddOrAlterExtendedProperty 'Numbering_Scheme','is_reset_number','Reset Number On Financial Year Change'
EXEC DDLAddOrAlterExtendedProperty 'Numbering_Scheme','is_read_only','Read Only'

EXEC DDLAddOrAlterExtendedProperty 'Document_Template','code','Code'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','description','Description'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','effective_date','Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','document_type_id','Type'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','printer','Printer'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','document_filter','Filter'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','document_template_group_id','Template Group'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','document_template_sub_group_id','Template Sub Group'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','CCMDocumentTemplate','KCM Document Template Mapping'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','is_editable_after_merging','Editable after merging?'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','is_visible_from_web','Visible from Web?'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','is_visible_from_client_manager','Visible from Client Manager?'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','archive_with_no_print','Archive with No Print?'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','email_as_body','Send Document As E-mail Body?'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','spool_document','Automatically Spool Document?'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','archive_as_text','Archive as Text?'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','archive_as_xml','Archive as XML?'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','email_sub_template_code','Subject Template'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','email_attachment_template_code','Attachment Template'
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','is_portal_internal_only','Internal Only '
EXEC DDLAddOrAlterExtendedProperty 'Document_Template','is_portal_selected_by_default','Selected by default'

-- Risk Type Miantenance
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','code','Code'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','description','Description'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','effective_date','Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','accumulation_level','Accumulation level'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','stamp_duty_rate1','Stamp Duty Rate 1'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','stamp_duty_rate2','Stamp Duty Rate 2'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','primary_sort','Primary Sort'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','secondary_sort','Secondary Sort'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','header_clause_id','Header clause'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','trailer_clause_id','Trailer clause'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','is_share_with_co_insurers','Share Tax with Coinsurer'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','is_share_with_re_insurers','Share Tax with Insurer'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','is_suppress_public_text','Suppress Public Text'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','is_suppress_private_text','Suppress Private Text'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','is_suppress_taxes','Suppress Taxes'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','claims_is_post_taxes','Post Claim taxes Separately'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','section_mask','Section Mask'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','is_auto_reinsured','Auto Insured'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','display_reinsurance_screen','Display Reinsurance Screen'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','display_claims_reinsurance_screen','Display Claim Reinsurance'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','is_deferred_RI_permitted','Deffered Insurance Permitted'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','Claims_Cover_basis_ID','Cover Verification Basis'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','Claims_type_basis_ID','Claims Type Basis'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','Attach_Claim_Outside_Of_Policy_Period','Attach Claim Outside Of Policy Period'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','allow_add_ratingsection','Adding New Sections'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','allow_edit_ratingsection','Edit Existing Sections'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','allow_delete_ratingsection','Delete Existing Sections'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','allow_edit_ratingsection_ratetype','Rate Type'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','allow_edit_ratingsection_rate','Rate'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','allow_edit_ratingsection_suminsured','Sum Insured'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','allow_edit_ratingsection_thispremium','This Premium'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','gis_screen_id','Associated Screen'
 
EXEC DDLAddOrAlterExtendedProperty 'risk_type_rule_set','code','Code'
EXEC DDLAddOrAlterExtendedProperty 'risk_type_rule_set','description','Description'
EXEC DDLAddOrAlterExtendedProperty 'risk_type_rule_set','effective_date','Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'risk_type_rule_set','type','Rule Type'
EXEC DDLAddOrAlterExtendedProperty 'risk_type_rule_set','file_name','Rule File'
EXEC DDLAddOrAlterExtendedProperty 'risk_type_rule_set','pre_version','PRE Version'
EXEC DDLAddOrAlterExtendedProperty 'risk_type_rule_set','dre_executor_url','PRE Executor URL'
EXEC DDLAddOrAlterExtendedProperty 'risk_type_rule_set','dre_default_token','PRE Profile Token'
EXEC DDLAddOrAlterExtendedProperty 'risk_type_rule_set','pre_ruleset_effective_date','PRE Ruleset Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'risk_type_rule_set','live','Rule is Live'
EXEC DDLAddOrAlterExtendedProperty 'risk_type_rule_set','pre_child_ruleset_effectivedate','Use Effective Date in Child Rule Set'
EXEC DDLAddOrAlterExtendedProperty 'risk_type_rule_set','dre_default','Default'
EXEC DDLAddOrAlterExtendedProperty 'risk_type_rule_set','dre_quote','Quote'
EXEC DDLAddOrAlterExtendedProperty 'risk_type_rule_set','dre_validation','Validation'
EXEC DDLAddOrAlterExtendedProperty 'risk_type_rule_set','pre_pre_rule','Pre-PRE run additional Rule '
EXEC DDLAddOrAlterExtendedProperty 'risk_type_rule_set','post_dre_script','Post-PRE run additional Rule '
EXEC DDLAddOrAlterExtendedProperty 'risk_type_rule_set','file_name','Assembly Name'
  
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type_RI_Model_Usage','description','Description'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type_RI_Model_Usage','ri_model_id','Insurance Model'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type_RI_Model_Usage','ri_band','Insurance Band'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type_RI_Model_Usage','effective_date','Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type_RI_Model_Usage','expiry_date','Expiry Date'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type_RI_Model_Usage','is_deleted','Is Deleted'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type_RI_Model_Usage','portfolio_transfer_from_cnt','Portfolio Transfer From'
EXEC DDLAddOrAlterExtendedProperty 'Primary_Cause_Risk_Type_Group','risk_type_group_id','Risk Type Group'

EXEC DDLAddOrAlterExtendedProperty 'Risk_Type_RI_Limit_Version','ri_limit_end_date','Expiry Date'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type_RI_Limit_Version','ri_limit_start_date','Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'Risk_Type','is_deleted','Is Deleted'

-- Product Risk Maintenance
EXEC DDLAddOrAlterExtendedProperty 'Product','code','Code'
EXEC DDLAddOrAlterExtendedProperty 'Product','description','Description'
EXEC DDLAddOrAlterExtendedProperty 'Product','effective_date','Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'Product','scheme_agency_ref','Scheme agency ref'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_short_period_rated','Short Period Rated'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_midnight_renewal','Midnight Renewal'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_accumulation','Accumulation'
EXEC DDLAddOrAlterExtendedProperty 'Product','ri_pointer','R/I pointer'
EXEC DDLAddOrAlterExtendedProperty 'Product','nb_prorata','NB Prorata'
EXEC DDLAddOrAlterExtendedProperty 'Product','nb_prorata','MTA Prorata'
EXEC DDLAddOrAlterExtendedProperty 'Product','disable_cover_start_date_on_REN','Disable Cover Start Date on Renewal'
EXEC DDLAddOrAlterExtendedProperty 'Product','do_not_delete_renQuote_on_mta','Do not delete Renewal Quote on MTA'
EXEC DDLAddOrAlterExtendedProperty 'Product','bind_renewal_without_invitation','Bind Manual Renewal without invitation'
EXEC DDLAddOrAlterExtendedProperty 'Product','use_prior_term_scheme_at_ren','Always Use Prior Scheme At Renewal'

EXEC DDLAddOrAlterExtendedProperty 'Product','Delete_And_ReRun_RenQuote','Delete And Auto Select Renewal Upon MTA'
EXEC DDLAddOrAlterExtendedProperty 'Product','Change_Ren_Policy_No_Auto','Change Policy Number At Renewal Automatically'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_roundoff_to_zero','Gross Total Round Off upto 0 Decimals'
EXEC DDLAddOrAlterExtendedProperty 'Product','change_policy_number_at_renewal','Change Policy Number At Renewal'

EXEC DDLAddOrAlterExtendedProperty 'Product','can_make_live_invoice','Invoice'
EXEC DDLAddOrAlterExtendedProperty 'Product','can_make_live_invoice','Instalments'
EXEC DDLAddOrAlterExtendedProperty 'Product','can_make_live_invoice','Pay Now'
EXEC DDLAddOrAlterExtendedProperty 'Product','produce_schedule','Produce Schedule'
EXEC DDLAddOrAlterExtendedProperty 'Product','produce_certificate','Produce Certificate'
EXEC DDLAddOrAlterExtendedProperty 'Product','produce_debit_note','Produce Debit Note'
EXEC DDLAddOrAlterExtendedProperty 'Product','TradeNBOnline','Trade NB on-line'
EXEC DDLAddOrAlterExtendedProperty 'Product','TradeMTAOnline','Trade MTA on-line'
EXEC DDLAddOrAlterExtendedProperty 'Product','OnlineTradingCommencedOn','On-line commenced On'
 
EXEC DDLAddOrAlterExtendedProperty 'Product','enable_mtc_rating_rule','Instalments'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_renewable','Produce Schedule'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_renewal_selection_enabled','Produce Certificate'
EXEC DDLAddOrAlterExtendedProperty 'Product','true_monthly_policy_renewal_communication','Produce Debit Note'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_enable_PrePayment','Enable PrePayment'
EXEC DDLAddOrAlterExtendedProperty 'Product','Hide_Summary_At_Renewal_Acceptance','Hide Summary At Renewal Acceptance'
EXEC DDLAddOrAlterExtendedProperty 'Product','default_cover_to_date_to_last_day','Default Cover to Date to last day of 12th month?'
EXEC DDLAddOrAlterExtendedProperty 'Product','use_policy_inception_date','Policy Inception Date'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_retain_policy_number_on_copy','Retain Policy Number on Copy'
EXEC DDLAddOrAlterExtendedProperty 'Product','block_no','Block no.'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_tax_suppressed','Tax suppressed'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_auto_renewable','Auto Renewal'
EXEC DDLAddOrAlterExtendedProperty 'Product','nb_prorata','NB Prorata'
EXEC DDLAddOrAlterExtendedProperty 'Product','mta_prorata','MTA Prorata'
EXEC DDLAddOrAlterExtendedProperty 'Product','policy_style_mandatory','Policy Style Is Mandatory'
EXEC DDLAddOrAlterExtendedProperty 'Product','allow_currency_change','Currency Change'
EXEC DDLAddOrAlterExtendedProperty 'Product','allow_standard_wording_edit','Allow Standard Wording Edit'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_retain_policy_number_on_copy','Retain Policy Number on Copy'
EXEC DDLAddOrAlterExtendedProperty 'Product','written_rem_task_group','Reminder Task group'
EXEC DDLAddOrAlterExtendedProperty 'Product','written_rem_user_group','Reminder User Group'
EXEC DDLAddOrAlterExtendedProperty 'Product','written_task_manager_days','Task Manager Days'
EXEC DDLAddOrAlterExtendedProperty 'Product','allow_written_status','Written Policy'
EXEC DDLAddOrAlterExtendedProperty 'Product','ri_manual_premium_adjustment','Reinsurance Manual Premium Adjustment'
EXEC DDLAddOrAlterExtendedProperty 'Product','use_nb_payment_term_at_renselection','Use NB/Renewal Payment Terms At Selection'
EXEC DDLAddOrAlterExtendedProperty 'Product','renewal_period','Renewal Period'
EXEC DDLAddOrAlterExtendedProperty 'Product','default_renewal_months','Default Months for Renewal'
EXEC DDLAddOrAlterExtendedProperty 'Product','grace_period','Quote Expiry (days)'
EXEC DDLAddOrAlterExtendedProperty 'Product','report_pointer','Document pointer'
EXEC DDLAddOrAlterExtendedProperty 'Product','enable_mtc_rating_rule','MTC Rating Rules Enabled'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_renewable','Renewable'
 
EXEC DDLAddOrAlterExtendedProperty 'Product','allow_Negative_reserve','Allow Negative Reserves'
EXEC DDLAddOrAlterExtendedProperty 'Product','inclusion_of_CoInsurers_On_Claims','Inclusion of Co-Insurers on Claims'
EXEC DDLAddOrAlterExtendedProperty 'Product','valid_Policy_Version_At_Loss_Date','Display only valid policy version at loss date'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_Gross_Claim_Payment_Amount','Claim Payment Amount is Gross'
EXEC DDLAddOrAlterExtendedProperty 'Product','payment_cannot_exceed_reserve','Payment Cannot Exceed Reserve'
EXEC DDLAddOrAlterExtendedProperty 'Product','claim_year_to_check','Claim Year'
EXEC DDLAddOrAlterExtendedProperty 'Product','max_single_claim_value','Single Claim Value'
EXEC DDLAddOrAlterExtendedProperty 'Product','max_number_of_claim','Allowed Claims'
EXEC DDLAddOrAlterExtendedProperty 'Product','max_total_claim_value','Total Claims Value'
EXEC DDLAddOrAlterExtendedProperty 'Product','media_type_mandatory','Media Type Field Mandatory on Claim Payments'
EXEC DDLAddOrAlterExtendedProperty 'Product','prevent_cancelled_agents','Prevent Claim Payments on Cancelled agents'
EXEC DDLAddOrAlterExtendedProperty 'Product','allow_loss_currency_change','Allow Loss Currency Change'
EXEC DDLAddOrAlterExtendedProperty 'Product','Suppress_Reserves','Reserve'
EXEC DDLAddOrAlterExtendedProperty 'Product','Suppress_Payments','Payment'
EXEC DDLAddOrAlterExtendedProperty 'Product','Suppress_Recoveries','Recovery'
EXEC DDLAddOrAlterExtendedProperty 'Product','multiple_claims_payments','Multiple Claim Payments'
EXEC DDLAddOrAlterExtendedProperty 'Product','max_unauthorised_claim_value','Max. Unauthorised Claims Value'
EXEC DDLAddOrAlterExtendedProperty 'Product','max_unauthorised_no_claim_payments','Max. No. of Unauthorised Claim Payments'
EXEC DDLAddOrAlterExtendedProperty 'Product','run_authorisation_scripts_claim_payments','Run Authorisation Scripts for Claims Payments'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_Recommend_Claim_Payments','Recommender Steps for Claim Payments'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_reserves_read_only','Run Claim Scripts for Reserves'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_recoveries_read_only','Run Claim Scripts for Recoveries'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_payments_read_only','Run Claim Scripts for Payments'
 
EXEC DDLAddOrAlterExtendedProperty 'Product','ext_Clm_Handler_Acknowledged_Task_Allowed_Time','Acknowledged Task Allowed Time'
EXEC DDLAddOrAlterExtendedProperty 'Product','ext_Clm_Handler_Supply_Pre_Report_Task_Allowed_Time','Supply Preliminary Report Task Allowed Time'
EXEC DDLAddOrAlterExtendedProperty 'Product','claim_Task_Group','Claim Task Group'
EXEC DDLAddOrAlterExtendedProperty 'Product','claim_User_Group','Claim User Group'
EXEC DDLAddOrAlterExtendedProperty 'Product','claims_UDT_A','Table A'
EXEC DDLAddOrAlterExtendedProperty 'Product','claims_UDT_B','Table B'
EXEC DDLAddOrAlterExtendedProperty 'Product','claims_UDT_C','Table C'
EXEC DDLAddOrAlterExtendedProperty 'Product','claims_UDT_D','Table D'
EXEC DDLAddOrAlterExtendedProperty 'Product','claims_UDT_E','Table E'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_Duplicate_Claim_Check_Enabled','Duplicate Claim Check Enabled'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_Advanced_Tax_Script_Enabled','Advanced Tax Script'
EXEC DDLAddOrAlterExtendedProperty 'Product','out_of_sequence_mta_dates','Dates allowed'
EXEC DDLAddOrAlterExtendedProperty 'Product','out_of_sequence_mta_allocation','Allocation'
EXEC DDLAddOrAlterExtendedProperty 'Product','out_of_sequence_MTA_UserGroup','User Group'
EXEC DDLAddOrAlterExtendedProperty 'Product','out_of_sequence_MTA_TaskGroup','Task Group' 
EXEC DDLAddOrAlterExtendedProperty 'Product','allow_backdated_mtas','Backdated MTA''s Allowed'
EXEC DDLAddOrAlterExtendedProperty 'Product','allow_backdated_can','Backdated Cancellation Allowed'
EXEC DDLAddOrAlterExtendedProperty 'Product','Cover_Note_Default_Period','Cover Note Default Period'
EXEC DDLAddOrAlterExtendedProperty 'Product','Cover_Note_doc_Template_id','Cover Nore Doc. Template'
EXEC DDLAddOrAlterExtendedProperty 'Product','Cover_Note_reused_upto','Maximun No. Of Cover Notes'
EXEC DDLAddOrAlterExtendedProperty 'Product','bankAccount_Id','Bank Account'
EXEC DDLAddOrAlterExtendedProperty 'Product','Cover_Note_numbering_id','Cover Note Numbering'
EXEC DDLAddOrAlterExtendedProperty 'Product','policy_auto_numbering_id','Policy Numbering'
EXEC DDLAddOrAlterExtendedProperty 'Product','prov_claim_auto_numbering_id','Provisonal Claim Auto Numbering'
EXEC DDLAddOrAlterExtendedProperty 'Product','allow_positive_cancellation','Positive Values In Cancel Policy'
EXEC DDLAddOrAlterExtendedProperty 'Product','Mandatory_Risk_Type_Id','Apply Mandatory Risk'
EXEC DDLAddOrAlterExtendedProperty 'Product','full_claim_auto_numbering_id','Full Claim Auto Numbering'
EXEC DDLAddOrAlterExtendedProperty 'Product','policy_style_id','Default Policy Style'
EXEC DDLAddOrAlterExtendedProperty 'Product','round_prem_to_nearest_unit','Round Premium'
EXEC DDLAddOrAlterExtendedProperty 'Product','rounding_section_id','Rating Section'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_policy_number_at_quote','Policy Number At Quote'
EXEC DDLAddOrAlterExtendedProperty 'Product','quote_auto_numbering_id','Quote Numbering'
EXEC DDLAddOrAlterExtendedProperty 'Product','Default_Payment_Method','Payment Method'
EXEC DDLAddOrAlterExtendedProperty 'Product','can_make_live_instalments','Instalments'
EXEC DDLAddOrAlterExtendedProperty 'Product','can_make_live_paynow','Pay Now'
EXEC DDLAddOrAlterExtendedProperty 'Product','can_make_live_BankGuarantee','Bank Guarantee'
EXEC DDLAddOrAlterExtendedProperty 'Product','claim_Value_For_Large_Loss_Advice','Reinsurance for Large Loss Advice Required When Claim Value Exceeds'
EXEC DDLAddOrAlterExtendedProperty 'Product','inclusion_of_CoInsurers_On_Claims','Inclusion of Co-Insurers On Claims'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_Payment_Ref_Check_Enabled','Payment Reference Check'
EXEC DDLAddOrAlterExtendedProperty 'Product','Check_MediaType_Status_At_Claim_Payment','Claim Payment - Check Media Type Status as Cleared'
EXEC DDLAddOrAlterExtendedProperty 'Product','Check_MediaType_Status_At_Policy_Refund','Refund MTA/MTC - Check Media Type Status as Cleare'
EXEC DDLAddOrAlterExtendedProperty 'Product','Quote_all_risk_NB','Display rerate for Quote & NB'
EXEC DDLAddOrAlterExtendedProperty 'Product','Quote_all_risk_MTC','Display rerate for Cancellations & Reinstatements'
EXEC DDLAddOrAlterExtendedProperty 'Product','Quote_all_risk_MTA','Display rerate for MTA'
EXEC DDLAddOrAlterExtendedProperty 'Product','Quote_all_risk_RENEWAL','Display rerate for Renewal'
EXEC DDLAddOrAlterExtendedProperty 'Product','is_quote_versioning','Quote Versioning Enabled'
EXEC DDLAddOrAlterExtendedProperty 'Product','delete_quote_after','Delete Quote Version After(Days)'

EXEC DDLAddOrAlterExtendedProperty 'Product_Claims_Workflow','check_unpaid_status','Check Unpaid Status'
EXEC DDLAddOrAlterExtendedProperty 'Product_Claims_Workflow','reinsurance_recovery','Reinsurance Recoveries'
EXEC DDLAddOrAlterExtendedProperty 'Product_Claims_Workflow','salvage_recovery','Salvage Recoveries'
EXEC DDLAddOrAlterExtendedProperty 'Product_Claims_Workflow','third_party_recovery',' Third Party Recovery'
EXEC DDLAddOrAlterExtendedProperty 'Product_Claims_Workflow','external_claim_handling',' External Claim Handling'
EXEC DDLAddOrAlterExtendedProperty 'Product_Claims_Workflow','claim_notification_doc_message','Display Generate Claim Notification Document message'
EXEC DDLAddOrAlterExtendedProperty 'Product_Claims_Workflow','generate_claim_notification_doc','Produce Claim Documents'
EXEC DDLAddOrAlterExtendedProperty 'Product_Claims_Workflow','claim_payment_process','Claim Payments Process'
EXEC DDLAddOrAlterExtendedProperty 'Product_Claims_Workflow','fast_track_claims','Fast Track Claims Payments'
EXEC DDLAddOrAlterExtendedProperty 'Product_Claims_Workflow','reinsurance_payment','Reinsurance Payment'
EXEC DDLAddOrAlterExtendedProperty 'Product_Claims_Workflow','description_for_change_in_payment','Enter Description for Change'
EXEC DDLAddOrAlterExtendedProperty 'Product_Claims_Workflow','cash_payment_process','Cash Payments Process'
EXEC DDLAddOrAlterExtendedProperty 'Product_Claims_Workflow','claim_payment_doc_message','Display Generate Claim Payment Documents Message'
EXEC DDLAddOrAlterExtendedProperty 'Product_Claims_Workflow','generate_claim_payment_doc','Produce Claim Payment Documents'
EXEC DDLAddOrAlterExtendedProperty 'Product_Claims_Workflow','make_further_payments','Do you wish to make further payments?'
EXEC DDLAddOrAlterExtendedProperty 'Product_Claims_Workflow','check_deferred_reinsurance','Check Deferred Reinsurance'
EXEC DDLAddOrAlterExtendedProperty 'Product_Claims_Workflow','description_for_change_in_reserve','Enter Description For Change'

EXEC DDLAddOrAlterExtendedProperty 'PMB_Doc_Link','Process_Type_Id','Process'
EXEC DDLAddOrAlterExtendedProperty 'PMB_Doc_Link','Document_Type_Id','Document Type'
EXEC DDLAddOrAlterExtendedProperty 'PMB_Doc_Link','Document_Template_Id','Document Template'
EXEC DDLAddOrAlterExtendedProperty 'PMB_Doc_Link','spool_document','Spooler'
EXEC DDLAddOrAlterExtendedProperty 'PMB_Doc_Link','is_client','Client'
EXEC DDLAddOrAlterExtendedProperty 'PMB_Doc_Link','is_agent','Agent'
EXEC DDLAddOrAlterExtendedProperty 'PMB_Doc_Link','is_office','Office'
EXEC DDLAddOrAlterExtendedProperty 'PMB_Doc_Link','generate_through_BO','Generate through Back Office'
EXEC DDLAddOrAlterExtendedProperty 'PMB_Doc_Link','generate_through_SAM','Generate through SAM'

EXEC DDLAddOrAlterExtendedProperty 'index_linking_detail','effective_date','Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'index_linking_detail','percentage','Percentage'

EXEC DDLAddOrAlterExtendedProperty 'Tax_group_tax_band','sequence','Sequence'
EXEC DDLAddOrAlterExtendedProperty 'Tax_group_tax_band','allocation_sequence','Allocation Sequence'
EXEC DDLAddOrAlterExtendedProperty 'Tax_group_tax_band','allocation_rule','Allocation Rule'
EXEC DDLAddOrAlterExtendedProperty 'Tax_group_tax_band','tax_band_id','Tax Band'

EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','effective_date','Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','is_value','Is Value'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','rate','Value/Rate(%)'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','Calc_Basis','Calculation Basis'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','NB','New Business'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','AMTA','Additional MTA'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','RMTA',' Return MTA'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','CANC','Cancellation'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','REN','Renewals'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','currency_id','Currency'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','class_of_business_id','Class Of Business'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','allow_tax_credit','Allow CR Tax?'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','use_for_refund_when_expired','Use for refund when expired'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','use_for_backdated_nb','Use for backdated NB transaction'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','country_id','Country'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','state_id','State'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','TTRI','Reinsurance'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','TTRIC','RI Commission'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','TTAC','Agent Commission'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','TTF','Fees'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','TTI','Instalments'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','TTRIPR','RI Payments/Recoveries'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','TTCP','Payments'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','TTCS','Salvage'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','TTCR','Third Party Recovery'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','description','Description'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','Basis_Value','Per Value Of Sum Insured'
EXEC DDLAddOrAlterExtendedProperty 'tax_band_rate','Sum_Insured_Rounded','Rounded?'

EXEC DDLAddOrAlterExtendedProperty 'RI_Band_Version','effective_date','Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'RI_Band_Version','Date_for_Treaty_XOL_Calculation_id','Date For Treaty Xol Calculation'
EXEC DDLAddOrAlterExtendedProperty 'RI_Band_Version','XOL_Treaty_To_Recover_From_id','Xol Treaty To Recover From'
EXEC DDLAddOrAlterExtendedProperty 'RI_Band_Version','Proportional_RI_Cal_Method','Proportional RI Cal Method'
EXEC DDLAddOrAlterExtendedProperty 'RI_Band_Version','use_anniversary_date_for_TMP','Use Anniversary Date For TMP'
EXEC DDLAddOrAlterExtendedProperty 'RI_Band_Version','ri_band_version_id','Effective Date'

EXEC DDLAddOrAlterExtendedProperty 'Currency','is_base','Is Base'
EXEC DDLAddOrAlterExtendedProperty 'Currency','minor_part','Minor Part'
EXEC DDLAddOrAlterExtendedProperty 'Currency','symbol','Symbol'
EXEC DDLAddOrAlterExtendedProperty 'Currency','alignment','Alignment'
EXEC DDLAddOrAlterExtendedProperty 'Currency','decimal_places','Decimal Places'
EXEC DDLAddOrAlterExtendedProperty 'Currency','effective_date','Effective Date'
EXEC DDLAddOrAlterExtendedProperty 'Currency','format_string','Format String'
EXEC DDLAddOrAlterExtendedProperty 'Currency','round_to_places','Round To X Decimal Places'
EXEC DDLAddOrAlterExtendedProperty 'Currency','is_deleted','Is Deleted'

EXEC DDLAddOrAlterExtendedProperty 'party_other','license_number','Licence Number'
EXEC DDLAddOrAlterExtendedProperty 'party_other','gender','Gender'
EXEC DDLAddOrAlterExtendedProperty 'party_other','active_indicator','Active'
EXEC DDLAddOrAlterExtendedProperty 'party_other','after_hours_indicator','After Hours'
EXEC DDLAddOrAlterExtendedProperty 'party_other','Priority_indicator','Priority'
EXEC DDLAddOrAlterExtendedProperty 'party_other','date_of_birth','Date Of Birth'
EXEC DDLAddOrAlterExtendedProperty 'party_other','license_type_id','Licence Type'
EXEC DDLAddOrAlterExtendedProperty 'party_other','reg_number','Registration Number'
EXEC DDLAddOrAlterExtendedProperty 'party_other','party_status','Status'
EXEC DDLAddOrAlterExtendedProperty 'party_other','is_TPA_Settle_directly','TPA Settle Directly'

EXEC DDLAddOrAlterExtendedProperty 'party_conviction','conviction_date','Conviction Details Date'
EXEC DDLAddOrAlterExtendedProperty 'party_conviction','description','Conviction Description'
EXEC DDLAddOrAlterExtendedProperty 'party_conviction','fine_amt','Fine'
EXEC DDLAddOrAlterExtendedProperty 'party_conviction','status_code','Status'
EXEC DDLAddOrAlterExtendedProperty 'party_conviction','sentence_code','Sentence Type'
EXEC DDLAddOrAlterExtendedProperty 'party_conviction','sentence_description','Sentence Description'
EXEC DDLAddOrAlterExtendedProperty 'party_conviction','sentence_duration','Sentence Duration'
EXEC DDLAddOrAlterExtendedProperty 'party_conviction','sentence_effective_date','Sentence Date'
EXEC DDLAddOrAlterExtendedProperty 'party_conviction','alcohol_level','Alcohol Level'
EXEC DDLAddOrAlterExtendedProperty 'party_conviction','alcohol_measurement_method','Alcohol Measurement Method'
EXEC DDLAddOrAlterExtendedProperty 'party_conviction','driving_licence_penalty_pts','Penalty Points'
EXEC DDLAddOrAlterExtendedProperty 'party_conviction','code','Conviction Details Type'
EXEC DDLAddOrAlterExtendedProperty 'party_conviction','sentence_duration_qualifier','Sentence Time Unit'

EXEC DDLAddOrAlterExtendedProperty 'previous_accidents','is_at_fault',' At Fault'


--Peril Type
EXEC DDLAddOrAlterExtendedProperty 'Peril_Type_Usage','peril_type_id','Peril type'
EXEC DDLAddOrAlterExtendedProperty 'Peril_Type_Usage','allocate_percent','Share %'

EXEC DDLAddOrAlterExtendedProperty 'Earning_Pattern_Usage','Earning_Pattern_id','Earning Pattern'
EXEC DDLAddOrAlterExtendedProperty 'Earning_Pattern_Usage','effective_date','Effective Date'

DELETE FROM Audit_trail_custom_fields

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='pfrf' AND FieldName = 'fee_type' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('pfrf','fee_type',0,'Amount')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='pfrf' AND FieldName = 'fee_type' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('pfrf','fee_type',1,'%')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='pfrf' AND FieldName = 'fee_charged_to' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('pfrf','fee_charged_to',0,'Plan')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='pfrf' AND FieldName = 'fee_charged_to' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('pfrf','fee_charged_to',1,'First Instalment')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='pfrf' AND FieldName = 'protection_charged_to' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('pfrf','protection_charged_to',0,'Plan')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='pfrf' AND FieldName = 'protection_charged_to' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('pfrf','protection_charged_to',1,'First Instalment')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='pfrf' AND FieldName = 'protection_type' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('pfrf','protection_type',0,'Amount')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='pfrf' AND FieldName = 'protection_type' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('pfrf','protection_type',1,'%')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='pfrf' AND FieldName = 'deposit_type' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('pfrf','deposit_type',0,'Amount')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='pfrf' AND FieldName = 'deposit_type' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('pfrf','deposit_type',1,'%')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='pfrf' AND FieldName = 'backdated_rollup_to' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('pfrf','backdated_rollup_to',0,'Spread Over Plan')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='pfrf' AND FieldName = 'backdated_rollup_to' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('pfrf','backdated_rollup_to',1,'Roll- Up into First Instalment')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='pfrf' AND FieldName = 'align_to' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('pfrf','align_to',0,'With the Policy Renewal Date')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='pfrf' AND FieldName = 'align_to' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('pfrf','align_to',1,'With the Customers preference')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='pfscheme' AND FieldName = 'receipt_difference_option' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('pfscheme','receipt_difference_option',0,'Write-off difference')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='pfscheme' AND FieldName = 'receipt_difference_option' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('pfscheme','receipt_difference_option',1,'Take exact amount')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='pfscheme' AND FieldName = 'receipt_difference_option' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('pfscheme','receipt_difference_option',2,'Users choice')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='RI_Model' AND FieldName = 'ri_model_type' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('RI_Model','ri_model_type',0,'Standard')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='RI_Model' AND FieldName = 'ri_model_type' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('RI_Model','ri_model_type',1,'Default')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='RI_Model' AND FieldName = 'ri_model_type' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('RI_Model','ri_model_type',2,'Deffered')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='RI_Model' AND FieldName = 'ri_model_type' AND fieldValue = 3)
INSERT INTO Audit_trail_custom_fields values('RI_Model','ri_model_type',3,'Excess Of Loss')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='RI_Model' AND FieldName = 'ri_model_type' AND fieldValue = 4)
INSERT INTO Audit_trail_custom_fields values('RI_Model','ri_model_type',4,'Cloned')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='RI_Model' AND FieldName = 'fac_premium_type' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('RI_Model','fac_premium_type',0,'Proportional')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='RI_Model' AND FieldName = 'fac_premium_type' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('RI_Model','fac_premium_type',1,'Non-Proportional')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='RI_Model' AND FieldName = 'claim_allocation_type' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('RI_Model','claim_allocation_type',0,'Proportional')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='RI_Model' AND FieldName = 'claim_allocation_type' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('RI_Model','claim_allocation_type',2,'Non-Proportional')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='RI_Model' AND FieldName = 'treaty_premium_type' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('RI_Model','treaty_premium_type',0,'Standard')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='RI_Model' AND FieldName = 'treaty_premium_type' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('RI_Model','treaty_premium_type',1,'Variable Cession Order')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='User_Authorities' AND FieldName = 'out_of_sequence_mta_authority' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('User_Authorities','out_of_sequence_mta_authority',1,'Not authorised to process MTA/MTC')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='User_Authorities' AND FieldName = 'out_of_sequence_mta_authority' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('User_Authorities','out_of_sequence_mta_authority',2,'MTA/MTC with no claims')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='User_Authorities' AND FieldName = 'out_of_sequence_mta_authority' AND fieldValue = 3)
INSERT INTO Audit_trail_custom_fields values('User_Authorities','out_of_sequence_mta_authority',3,'MTA/MTC with claims')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Tax_group_tax_band' AND FieldName = 'allocation_rule' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('Tax_group_tax_band','allocation_rule',0,'Before Premium')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Tax_group_tax_band' AND FieldName = 'allocation_rule' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('Tax_group_tax_band','allocation_rule',1,'With Premium')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Tax_group_tax_band' AND FieldName = 'allocation_rule' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('Tax_group_tax_band','allocation_rule',2,'After Premium')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='tax_band_rate' AND FieldName = 'Calc_Basis' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('tax_band_rate','Calc_Basis',0,'Premium')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='tax_band_rate' AND FieldName = 'Calc_Basis' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('tax_band_rate','Calc_Basis',1,'Sum Insured')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='tax_band_rate' AND FieldName = 'Calc_Basis' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('tax_band_rate','Calc_Basis',2,'Sum Insured Change')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='tax_band_rate' AND FieldName = 'Calc_Basis' AND fieldValue = 3)
INSERT INTO Audit_trail_custom_fields values('tax_band_rate','Calc_Basis',3,'Running Total')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='tax_band_rate' AND FieldName = 'Calc_Basis' AND fieldValue = 4)
INSERT INTO Audit_trail_custom_fields values('tax_band_rate','Calc_Basis',4,'Total COB Premium')

DELETE FROM foreign_key_table

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product_Risk_Type_Group' AND column_name = 'risk_type_group_id')
INSERT INTO foreign_key_table VALUES ('Product_Risk_Type_Group','risk_type_group_id','Risk_Type_Group','risk_type_group_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'written_rem_user_group')
INSERT INTO foreign_key_table VALUES ('Product','written_rem_user_group','PMUser_Group','pmuser_group_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'written_rem_task_group')
INSERT INTO foreign_key_table VALUES ('Product ','written_rem_task_group','PMWrk_Task_Group','pmwrk_task_group_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Primary_Cause_Risk_Type_Group' AND column_name = 'risk_type_group_id')
INSERT INTO foreign_key_table VALUES ('Primary_Cause_Risk_Type_Group','risk_type_group_id','risk_type_group','risk_type_group_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Tax_group_tax_band' AND column_name = 'tax_band_id')
INSERT INTO foreign_key_table VALUES ('Tax_group_tax_band','tax_band_id','tax_band','tax_band_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Peril_Type_Usage' AND column_name = 'peril_type_id')
INSERT INTO foreign_key_table VALUES ('Peril_Type_Usage','peril_type_id','peril_type','peril_type_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Earning_Pattern_Usage' AND column_name = 'Earning_Pattern_id')
INSERT INTO foreign_key_table VALUES ('Earning_Pattern_Usage','Earning_Pattern_id','Earning_Pattern','Earning_Pattern_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PFScheme' AND column_name = 'mediatype_id')
INSERT INTO foreign_key_table VALUES ('PFScheme','mediatype_id','mediatype','mediatype_id','description')
 
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PFScheme' AND column_name = 'pfscheme_printtype_id')
INSERT INTO foreign_key_table VALUES ('PFScheme','pfscheme_printtype_id','PFScheme_PrintType','pfscheme_printtype_id','description')
 
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PFScheme' AND column_name = 'CreditDocID')
INSERT INTO foreign_key_table VALUES ('PFScheme','CreditDocID','Document_Template','document_template_id','description')
 
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PFScheme' AND column_name = 'QuoteDocID')
INSERT INTO foreign_key_table VALUES ('PFScheme','QuoteDocID','Document_Template','document_template_id','description')
 
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PFScheme' AND column_name = 'ConfirmationDocID')
INSERT INTO foreign_key_table VALUES ('PFScheme','ConfirmationDocID','Document_Template','document_template_id','description')
 
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PFScheme' AND column_name = 'ColNotDocID')
INSERT INTO foreign_key_table VALUES ('PFScheme','ColNotDocID','Document_Template','document_template_id','description')
 
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PFScheme' AND column_name = 'BankDocID')
INSERT INTO foreign_key_table VALUES ('PFScheme','BankDocID','Document_Template','document_template_id','description')
 
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PFScheme' AND column_name = 'bankaccount_id')
INSERT INTO foreign_key_table VALUES ('PFScheme','bankaccount_id','BankAccount','bankaccount_id','description')
 
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PFScheme' AND column_name = 'suspense_account_id')
INSERT INTO foreign_key_table VALUES ('PFScheme','suspense_account_id','Account','account_id','short_code')
 
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PFScheme' AND column_name = 'interest_account_id')
INSERT INTO foreign_key_table VALUES ('PFScheme','interest_account_id','Account','account_id','short_code')
 
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PFScheme' AND column_name = 'protection_account_id')
INSERT INTO foreign_key_table VALUES ('PFScheme','protection_account_id','Account','account_id','short_code')
 
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PFScheme' AND column_name = 'commission_suspense_account_id')
INSERT INTO foreign_key_table VALUES ('PFScheme','commission_suspense_account_id','Account','account_id','short_code')
 
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PFScheme' AND column_name = 'ri_suspense_account_id')
INSERT INTO foreign_key_table VALUES ('PFScheme','ri_suspense_account_id','Account','account_id','short_code')
 
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PFScheme' AND column_name = 'commission_subagent_suspense_account_id')
INSERT INTO foreign_key_table VALUES ('PFScheme','commission_subagent_suspense_account_id','Account','account_id','short_code')
 
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PFScheme' AND column_name = 'tax_group_id')
INSERT INTO foreign_key_table VALUES ('PFScheme','tax_group_id','Tax_Group','tax_group_id','description')
 
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PFRF' AND column_name = 'pffrequency_id')
INSERT INTO foreign_key_table VALUES ('PFRF','pffrequency_id','PFFrequency','pffrequency_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PFSchemeSource' AND column_name = 'source_id')
INSERT INTO foreign_key_table VALUES ('PFSchemeSource','source_id','source','source_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PFSchemeProducts' AND column_name = 'product_id')
INSERT INTO foreign_key_table VALUES ('PFSchemeProducts','product_id','product','product_id','description')

-- Commission Maintenance

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'commission_arrangement' AND column_name = 'transaction_type_id')
INSERT INTO foreign_key_table VALUES ('commission_arrangement','transaction_type_id','Transaction_Type','transaction_type_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'commission_arrangement' AND column_name = 'Product_id')
INSERT INTO foreign_key_table VALUES ('commission_arrangement','Product_id','Product','Product_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'commission_arrangement' AND column_name = 'commission_band_id')
INSERT INTO foreign_key_table VALUES ('commission_arrangement','commission_band_id','Commission_Band','commission_band_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'commission_arrangement' AND column_name = 'risk_type_id')
INSERT INTO foreign_key_table VALUES ('commission_arrangement','risk_type_id','Risk_Type','risk_type_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'commission_arrangement' AND column_name = 'tax_group_id')
INSERT INTO foreign_key_table VALUES ('commission_arrangement','tax_group_id','Tax_Group','tax_group_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'commission_arrangement' AND column_name = 'commission_level_id')
INSERT INTO foreign_key_table VALUES ('commission_arrangement','commission_level_id','Commission_level','commission_level_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'commission_arrangement' AND column_name = 'commission_level_id')
INSERT INTO foreign_key_table VALUES ('commission_arrangement','Party_type','Party_Type','party_type_id','description')

-- RI Model Maintenance

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'RI_Model' AND column_name = 'currency_id')
INSERT INTO foreign_key_table VALUES ('RI_Model','currency_id','Currency','currency_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'RI_Model' AND column_name = 'xol_clm_ri_model_id')
INSERT INTO foreign_key_table VALUES ('RI_Model','xol_clm_ri_model_id','RI_Model','ri_model_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'RI_Model' AND column_name = 'xol_cat_ri_model_id')
INSERT INTO foreign_key_table VALUES ('RI_Model','xol_cat_ri_model_id','RI_Model','ri_model_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'RI_Model_Line' AND column_name = 'Treaty_Type_id')
INSERT INTO foreign_key_table VALUES ('RI_Model_Line','Treaty_Type_id','Treaty_Type','Treaty_Type_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'RI_Model_Line' AND column_name = 'treaty_id')
INSERT INTO foreign_key_table VALUES ('RI_Model_Line','treaty_id','Treaty','treaty_id','description')

--Credit Control Maintenance
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Credit_Control_Rule' AND column_name = 'source_id')
INSERT INTO foreign_key_table VALUES ('Credit_Control_Rule','source_id','Source','source_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Credit_Control_Rule' AND column_name = 'product_id')
INSERT INTO foreign_key_table VALUES ('Credit_Control_Rule','product_id','Product','product_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Credit_Control_Rule' AND column_name = 'pffrequency_id')
INSERT INTO foreign_key_table VALUES ('Credit_Control_Rule','pffrequency_id','PFFrequency','pffrequency_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Credit_Control_Rule' AND column_name = 'pfinstalments_result_id')
INSERT INTO foreign_key_table VALUES ('Credit_Control_Rule','pfinstalments_result_id','PFInstalments_Result','pfinstalments_result_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Credit_Control_Step' AND column_name = 'client_document_template_id')
INSERT INTO foreign_key_table VALUES ('Credit_Control_Step','client_document_template_id','Document_Template','document_template_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Credit_Control_Step' AND column_name = 'broker_letter_id')
INSERT INTO foreign_key_table VALUES ('Credit_Control_Step','broker_letter_id','Document_Template','document_template_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Credit_Control_Step' AND column_name = 'single_instalment_broker_letter_id')
INSERT INTO foreign_key_table VALUES ('Credit_Control_Step','single_instalment_broker_letter_id','Document_Template','document_template_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Credit_Control_Step' AND column_name = 'auto_cancel_document_1_template_id')
INSERT INTO foreign_key_table VALUES ('Credit_Control_Step','auto_cancel_document_1_template_id','Document_Template','document_template_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Credit_Control_Step' AND column_name = 'auto_cancel_document_2_template_id')
INSERT INTO foreign_key_table VALUES ('Credit_Control_Step','auto_cancel_document_2_template_id','Document_Template','document_template_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Credit_Control_Step' AND column_name = 'write_off_reason_id')
INSERT INTO foreign_key_table VALUES ('Credit_Control_Step','write_off_reason_id','write_off_reason','write_off_reason_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Credit_Control_Step' AND column_name = 'pmwrk_task_group_id')
INSERT INTO foreign_key_table VALUES ('Credit_Control_Step','pmwrk_task_group_id','PMWrk_Task_Group','pmwrk_task_group_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Credit_Control_Step' AND column_name = 'pmwrk_task_id')
INSERT INTO foreign_key_table VALUES ('Credit_Control_Step','pmwrk_task_id','PMWrk_Task','pmwrk_task_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Credit_Control_Step' AND column_name = 'pmuser_group_id')
INSERT INTO foreign_key_table VALUES ('Credit_Control_Step','pmuser_group_id','PMUser_Group','pmuser_group_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Credit_Control_Rule_Insurance_File_Status' AND column_name = 'insurance_file_status_id')
INSERT INTO foreign_key_table VALUES ('Credit_Control_Rule_Insurance_File_Status','insurance_file_status_id','insurance_file_status','insurance_file_status_id','description')

-- Treaty Maintenance
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Treaty' AND column_name = 'replaces_treaty_id')
INSERT INTO foreign_key_table VALUES ('Treaty','replaces_treaty_id','Treaty','treaty_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Treaty' AND column_name = 'replaced_by_treaty_id')
INSERT INTO foreign_key_table VALUES ('Treaty','replaced_by_treaty_id','Treaty','treaty_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Treaty' AND column_name = 'reinsurance_type_id')
INSERT INTO foreign_key_table VALUES ('Treaty','reinsurance_type_id','Reinsurance_type','reinsurance_type_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Treaty_Party' AND column_name = 'party_cnt')
INSERT INTO foreign_key_table VALUES ('Treaty_Party','party_cnt','Party','party_cnt','resolved_name')

-- User Maintenance
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'User_Authorities' AND column_name = 'write_off_currency_id')
INSERT INTO foreign_key_table VALUES ('User_Authorities','write_off_currency_id','Currency','currency_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'User_Authorities' AND column_name = 'payments_currency_id')
INSERT INTO foreign_key_table VALUES ('User_Authorities','payments_currency_id','Currency','currency_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'User_Authorities' AND column_name = 'claims_payments_currency_id')
INSERT INTO foreign_key_table VALUES ('User_Authorities','claims_payments_currency_id','Currency','currency_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'User_Authorities' AND column_name = 'paynow_write_off_currency_id')
INSERT INTO foreign_key_table VALUES ('User_Authorities','paynow_write_off_currency_id','Currency','currency_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'User_Authorities' AND column_name = 'recommender_currency_id')
INSERT INTO foreign_key_table VALUES ('User_Authorities','recommender_currency_id','Currency','currency_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'User_Authorities' AND column_name = 'ManualJournal_currency_id')
INSERT INTO foreign_key_table VALUES ('User_Authorities','ManualJournal_currency_id','Currency','currency_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'User_Authorities' AND column_name = 'loss_gain_currency_id')
INSERT INTO foreign_key_table VALUES ('User_Authorities','loss_gain_currency_id','Currency','currency_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PMUser_Source' AND column_name = 'source_id')
INSERT INTO foreign_key_table VALUES ('PMUser_Source','source_id','Source','source_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PMUser_Group_User' AND column_name = 'pmuser_group_id')
INSERT INTO foreign_key_table VALUES ('PMUser_Group_User','pmuser_group_id','PMUser_Group','pmuser_group_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PMUser' AND column_name = 'party_handler_id')
INSERT INTO foreign_key_table VALUES ('PMUser','party_handler_id','Party','party_cnt','resolved_name')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PMUser' AND column_name = 'claim_handler_id')
INSERT INTO foreign_key_table VALUES ('PMUser','claim_handler_id','handler','handler_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PMUser' AND column_name = 'other_party_id')
INSERT INTO foreign_key_table VALUES ('PMUser','other_party_id','Party','party_cnt','resolved_name')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PMUser' AND column_name = 'party_cnt')
INSERT INTO foreign_key_table VALUES ('PMUser','party_cnt','Party','party_cnt','resolved_name')

-- User Maintainable Rates

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'GIS_User_Def_Header' AND column_name = 'Parent')
INSERT INTO foreign_key_table VALUES ('GIS_User_Def_Header','Parent','GIS_User_Def_Header','gis_user_def_header_id','code')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'GIS_User_Def_detail' AND column_name = 'Parent')
INSERT INTO foreign_key_table VALUES ('GIS_User_Def_detail','Parent','GIS_User_Def_detail','GIS_User_Def_detail_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party' AND column_name = 'currency_id')
INSERT INTO foreign_key_table VALUES ('Party','currency_id','Currency','currency_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party' AND column_name = 'source_id')
INSERT INTO foreign_key_table VALUES ('Party','source_id','Source','source_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party' AND column_name = 'sub_branch_id')
INSERT INTO foreign_key_table VALUES ('Party','sub_branch_id','Source','source_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party' AND column_name = 'party_category_id')
INSERT INTO foreign_key_table VALUES ('Party','party_category_id','party_category','party_category_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party' AND column_name = 'renewal_stop_code_id')
INSERT INTO foreign_key_table VALUES ('Party','renewal_stop_code_id','Renewal_stop_code','renewal_stop_code_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party' AND column_name = 'payment_term_code')
INSERT INTO foreign_key_table VALUES ('Party','payment_term_code','PFFrequency','pffrequency_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Address' AND column_name = 'country_id')
INSERT INTO foreign_key_table VALUES ('Address','country_id','Country','country_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Contact' AND column_name = 'contact_type_id')
INSERT INTO foreign_key_table VALUES ('Contact','contact_type_id','contact_type','contact_type_id','description')

--ReInusrer Maintenance
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'party_insurer' AND column_name = 'agency_number')
INSERT INTO foreign_key_table VALUES ('party_insurer','agency_number','Treaty','treaty_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'party_insurer' AND column_name = 'reinsurance_type')
INSERT INTO foreign_key_table VALUES ('party_insurer','Reinsurance_type','Reinsurance_type','reinsurance_type_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'party_insurer' AND column_name = 'tax_group_id')
INSERT INTO foreign_key_table VALUES ('party_insurer','tax_group_id','Tax_Group','tax_group_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party_Bank' AND column_name = 'bank_name_id')
INSERT INTO foreign_key_table VALUES ('Party_Bank','bank_name_id','CashListItem_Bank','cashlistitem_bank_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party_Bank' AND column_name = 'bank_country')
INSERT INTO foreign_key_table VALUES ('Party_Bank','bank_country','Country','country_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party_Bank' AND column_name = 'cc_country')
INSERT INTO foreign_key_table VALUES ('Party_Bank','cc_country','Country','country_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party_Agent' AND column_name = 'party_agent_origin_id')
INSERT INTO foreign_key_table VALUES ('Party_Agent','party_agent_origin_id','Party_Agent_Origin','party_agent_origin_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party_Agent' AND column_name = 'payment_method')
INSERT INTO foreign_key_table VALUES ('Party_Agent','payment_method','MediaType','mediatype_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party_Agent' AND column_name = 'payment_frequency')
INSERT INTO foreign_key_table VALUES ('Party_Agent','payment_frequency','payment_frequency','payment_frequency_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party_Agent' AND column_name = 'transfer_to_party_cnt')
INSERT INTO foreign_key_table VALUES ('Party_Agent','transfer_to_party_cnt','party','party_cnt','shortname')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party_Agent' AND column_name = 'bankaccount_id')
INSERT INTO foreign_key_table VALUES ('Party_Agent','bankaccount_id','BankAccount','bankaccount_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party_Agent' AND column_name = 'transfer_to_business_type_id')
INSERT INTO foreign_key_table VALUES ('Party_Agent','transfer_to_business_type_id','Business_Type','business_type_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party_Agent' AND column_name = 'linked_account_executive_id')
INSERT INTO foreign_key_table VALUES ('Party_Agent','linked_account_executive_id','Party','party_cnt','shortname')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party_Agent_Branch' AND column_name = 'source_id')
INSERT INTO foreign_key_table VALUES ('Party_Agent_Branch','source_id','Source','source_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party_Agent_Product' AND column_name = 'product_id')
INSERT INTO foreign_key_table VALUES ('Party_Agent_Product','product_id','Product','product_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party_Agent' AND column_name = 'linked_account_group')
INSERT INTO foreign_key_table VALUES ('Party_Agent','linked_account_group','Party','party_cnt','shortname')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party_Agent' AND column_name = 'address_on_notice')
INSERT INTO foreign_key_table VALUES ('Party_Agent','address_on_notice','Address_Usage_Type','address_usage_type_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Agent_Docs' AND column_name = 'process_type')
INSERT INTO foreign_key_table VALUES ('Agent_Docs','process_type','Process_Types_Docs','process_types_docs_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Fee_Amounts' AND column_name = 'tax_group_id')
INSERT INTO foreign_key_table VALUES ('Fee_Amounts','tax_group_id','Tax_Group','tax_group_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Fee_Amounts' AND column_name = 'currency_id')
INSERT INTO foreign_key_table VALUES ('Fee_Amounts','currency_id','Currency','currency_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Fee_Amounts' AND column_name = 'product_id')
INSERT INTO foreign_key_table VALUES ('Fee_Amounts','product_id','Product','product_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Fee_Amounts' AND column_name = 'peril_group_id')
INSERT INTO foreign_key_table VALUES ('Fee_Amounts','peril_group_id','Peril_Group','peril_group_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Fee_Amounts' AND column_name = 'risk_type_group_id')
INSERT INTO foreign_key_table VALUES ('Fee_Amounts','risk_type_group_id','Risk_Type_Group','risk_type_group_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party_Address_Usage' AND column_name = 'address_usage_type_id')
INSERT INTO foreign_key_table VALUES ('Party_Address_Usage','address_usage_type_id','Address_Usage_Type','address_usage_type_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party_Relationship' AND column_name = 'relationship_type_id')
INSERT INTO foreign_key_table VALUES ('Party_Relationship','relationship_type_id','Relationship_Type','relationship_type_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Party_Relationship' AND column_name = 'relation_cnt')
INSERT INTO foreign_key_table VALUES ('Party_Relationship','relation_cnt','Party','party_cnt','name')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'party_handler_branch' AND column_name = 'source_id')
INSERT INTO foreign_key_table VALUES ('party_handler_branch','source_id','Source','source_id','description')

---Branch Maintenance

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Source' AND column_name = 'country_id')
INSERT INTO foreign_key_table VALUES ('Source','country_id','Country','country_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Source' AND column_name = 'base_currency_id')
INSERT INTO foreign_key_table VALUES ('Source','base_currency_id','Currency','currency_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Source' AND column_name = 'FSA_companyCategory_id')
INSERT INTO foreign_key_table VALUES ('Source','FSA_companyCategory_id','FSA_CompanyCategory','FSA_CompanyCategory_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'PMUser_Authority_Level' AND column_name = 'authority_level_type_id')
INSERT INTO foreign_key_table VALUES ('PMUser_Authority_Level','authority_level_type_id','Authority_Level_Type','authority_level_type_id','description')

-- MAINTAIN Numbering Schemes

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Numbering_Scheme' AND column_name = 'numbering_scheme_type_id')
INSERT INTO foreign_key_table VALUES ('Numbering_Scheme','numbering_scheme_type_id','numbering_scheme_type','numbering_scheme_type_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Numbering_Scheme' AND column_name = 'party_type_id')
INSERT INTO foreign_key_table VALUES ('Numbering_Scheme','party_type_id','Party_Type','party_type_id','description')

-- Maintain Document Templates

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Document_Template' AND column_name = 'document_type_id')
INSERT INTO foreign_key_table VALUES ('Document_Template','document_type_id','Document_Type','document_type_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Document_Template' AND column_name = 'document_template_sub_group_id')
INSERT INTO foreign_key_table VALUES ('Document_Template','document_template_sub_group_id','Document_Template_Sub_Group','document_template_sub_group_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Document_Template' AND column_name = 'document_template_group_id')
INSERT INTO foreign_key_table VALUES ('Document_Template','document_template_group_id','Document_Template_Group','document_template_group_id','description')

-- Risk Type Maintenance

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Wording_Risk_Type_Link' AND column_name = 'branch_id')
INSERT INTO foreign_key_table VALUES ('Wording_Risk_Type_Link','branch_id','Source','source_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Risk_Type_Rating_Section_Type' AND column_name = 'rating_section_type_id')
INSERT INTO foreign_key_table VALUES ('Risk_Type_Rating_Section_Type','rating_section_type_id','Rating_Section_Type','rating_section_type_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Risk_Type_RI_Model_Usage' AND column_name = 'ri_model_id')
INSERT INTO foreign_key_table VALUES ('Risk_Type_RI_Model_Usage','ri_model_id','RI_Model','ri_model_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Risk_Type_RI_Model_Usage' AND column_name = 'ri_band')
INSERT INTO foreign_key_table VALUES ('Risk_Type_RI_Model_Usage','ri_band','RI_Band','ri_band_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Risk_Type_Usage' AND column_name = 'risk_type_group_id')
INSERT INTO foreign_key_table VALUES ('Risk_Type_Usage','risk_type_group_id','Risk_Type_Group','risk_type_group_id','code')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Risk_Type' AND column_name = 'header_clause_id')
INSERT INTO foreign_key_table VALUES ('Risk_Type','header_clause_id','Document_Template','document_template_id','code')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Risk_Type' AND column_name = 'trailer_clause_id')
INSERT INTO foreign_key_table VALUES ('Risk_Type','trailer_clause_id','Document_Template','document_template_id','code')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Risk_Type' AND column_name = 'Claims_Cover_basis_ID')
INSERT INTO foreign_key_table VALUES ('Risk_Type','Claims_Cover_basis_ID','Claims_Cover_basis','Claims_Cover_basis_id','Description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Risk_Type' AND column_name = 'Claims_type_basis_ID')
INSERT INTO foreign_key_table VALUES ('Risk_Type','Claims_type_basis_ID','Claims_Type_basis','Claims_type_basis_id','Description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Risk_Type' AND column_name = 'gis_screen_id')
INSERT INTO foreign_key_table VALUES ('Risk_Type','gis_screen_id','GIS_Screen','gis_screen_id','description')

-- Product Risk Maintenance
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'policy_auto_numbering_id')
INSERT INTO foreign_key_table VALUES ('Product','policy_auto_numbering_id','numbering_scheme','numbering_scheme_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'prov_claim_auto_numbering_id')
INSERT INTO foreign_key_table VALUES ('Product','prov_claim_auto_numbering_id','numbering_scheme','numbering_scheme_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'full_claim_auto_numbering_id')
INSERT INTO foreign_key_table VALUES ('Product','full_claim_auto_numbering_id','numbering_scheme','numbering_scheme_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'quote_auto_numbering_id')
INSERT INTO foreign_key_table VALUES ('Product','quote_auto_numbering_id','numbering_scheme','numbering_scheme_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'claim_Task_Group')
INSERT INTO foreign_key_table VALUES ('Product','claim_Task_Group','PMWrk_Task_Group','pmwrk_task_group_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'bankAccount_Id')
INSERT INTO foreign_key_table VALUES ('Product','bankAccount_Id','BankAccount','bankaccount_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'claim_User_Group')
INSERT INTO foreign_key_table VALUES ('Product','claim_User_Group','PMUser_Group','pmuser_group_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'Cover_Note_doc_Template_id')
INSERT INTO foreign_key_table VALUES ('Product','Cover_Note_doc_Template_id','Document_Template','document_template_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'Mandatory_Risk_Type_Id')
INSERT INTO foreign_key_table VALUES ('Product','Mandatory_Risk_Type_Id','Risk_Type','risk_type_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'out_of_Sequence_MTA_UserGroup')
INSERT INTO foreign_key_table VALUES ('Product','out_of_Sequence_MTA_UserGroup','PMUser_Group','pmuser_group_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'out_of_sequence_MTA_TaskGroup')
INSERT INTO foreign_key_table VALUES ('Product','out_of_sequence_MTA_TaskGroup','PMWrk_Task_Group','PMWrk_Task_Group_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'claims_UDT_A')
INSERT INTO foreign_key_table VALUES ('Product','claims_UDT_A','GIS_User_Def_Header','gis_user_def_header_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'claims_UDT_B')
INSERT INTO foreign_key_table VALUES ('Product','claims_UDT_B','GIS_User_Def_Header','gis_user_def_header_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'claims_UDT_C')
INSERT INTO foreign_key_table VALUES ('Product','claims_UDT_C','GIS_User_Def_Header','gis_user_def_header_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'claims_UDT_D')
INSERT INTO foreign_key_table VALUES ('Product','claims_UDT_D','GIS_User_Def_Header','gis_user_def_header_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'claims_UDT_E')
INSERT INTO foreign_key_table VALUES ('Product','claims_UDT_E','GIS_User_Def_Header','gis_user_def_header_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product' AND column_name = 'rounding_section_id')
INSERT INTO foreign_key_table VALUES ('Product','rounding_section_id','Rating_Section_Type','rating_section_type_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product_MTA_Events' AND column_name = 'mta_event_description_id')
INSERT INTO foreign_key_table VALUES ('Product_MTA_Events','mta_event_description_id','MTA_Event_Description','mta_event_description_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product_Claim_Events' AND column_name = 'claim_event_description_id')
INSERT INTO foreign_key_table VALUES ('Product_Claim_Events','claim_event_description_id','Claim_Event_Description','claim_event_description_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product_Source' AND column_name = 'source_id')
INSERT INTO foreign_key_table VALUES ('Product_Source','source_id','Source','source_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'pmb_doc_link' AND column_name = 'PMB_Doc_Link_Id')
INSERT INTO foreign_key_table VALUES ('pmb_doc_link','PMB_Doc_Link_Id','Document_Template','document_template_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Product_Allowed_Causation' AND column_name = 'primary_cause_id')
INSERT INTO foreign_key_table VALUES ('Product_Allowed_Causation','primary_cause_id','primary_cause','primary_cause_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Wording_Product_Link' AND column_name = 'branch_id')
INSERT INTO foreign_key_table VALUES ('Wording_Product_Link','branch_id','Source','source_id','description')

-- Report_Group
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Report_Group_Contents' AND column_name = 'report_id')
INSERT INTO foreign_key_table VALUES ('Report_Group_Contents','report_id','Report','report_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Report_Group_User_Groups' AND column_name = 'pmuser_group_id')
INSERT INTO foreign_key_table VALUES ('Report_Group_User_Groups','pmuser_group_id','PMUser_Group','pmuser_group_id','description')

-- Tax Band
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'tax_band_rate' AND column_name = 'currency_id')
INSERT INTO foreign_key_table VALUES ('tax_band_rate','currency_id','Currency','currency_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'tax_band_rate' AND column_name = 'state_id')
INSERT INTO foreign_key_table VALUES ('tax_band_rate','state_id','State','state_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Class_Of_Business' AND column_name = 'class_of_business_id')
INSERT INTO foreign_key_table VALUES ('tax_band_rate','class_of_business_id','Class_Of_Business','class_of_business_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'tax_band_rate' AND column_name = 'country_id')
INSERT INTO foreign_key_table VALUES ('tax_band_rate','country_id','Country','country_id','description')

-- RI Band

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'RI_Band_Version' AND column_name = 'Date_for_Treaty_XOL_Calculation_id')
INSERT INTO foreign_key_table VALUES ('RI_Band_Version','Date_for_Treaty_XOL_Calculation_id','Date_for_Treaty_XOL_Calculation','Date_for_Treaty_XOL_Calculation_id','Description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'RI_Band_Version' AND column_name = 'XOL_Treaty_To_Recover_From_id')
INSERT INTO foreign_key_table VALUES ('RI_Band_Version','XOL_Treaty_To_Recover_From_id','XOL_Treaty_To_Recover_From','XOL_Treaty_To_Recover_From_id','Description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'RI_Band_Version' AND column_name = 'Proportional_RI_Cal_Method')
INSERT INTO foreign_key_table VALUES ('RI_Band_Version','Proportional_RI_Cal_Method','Proportional_RI_Calculation_Method','Proportional_RI_Calculation_Method_id','Description')

--Bank Account nd Branch Maintainace

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'BankAccount_Source' AND column_name = 'source_id')
INSERT INTO foreign_key_table VALUES ('BankAccount_Source','source_id','Source','source_id','code')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'CompanyCurrency' AND column_name = 'currency_id')
INSERT INTO foreign_key_table VALUES ('CompanyCurrency','currency_id','Currency','currency_id','iso_code')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'Other_Party_Branch' AND column_name = 'source_id')
INSERT INTO foreign_key_table VALUES ('Other_Party_Branch','source_id','Source','source_id','description')

IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'RI_Model_Line' AND column_name = 'premium_calculation_basis_Id')
INSERT INTO foreign_key_table VALUES ('RI_Model_Line','premium_calculation_basis_Id','premium_calculation_basis','premium_calculation_basis_Id','description')

-- Audit Trail Custom Fields
-- System Options
IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5171' AND fieldValue = 3)
INSERT INTO Audit_trail_custom_fields values('system_options','5171',3,'Accepted')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5171' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('system_options','5171',2,'Current')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5171' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','5171',1,'Published')


IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '1004' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('system_options','1004',0,'Period Basis')
 
IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '1004' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','1004',1,'Effective Month Basis')
 
IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '1005' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('system_options','1005',0,'Period Basis')
 
IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '1005' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','1005',1,'Effective Month Basis')
 
IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '1006' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('system_options','1006',0,'Period Basis')
 
IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '1006' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','1006',1,'Effective Month Basis')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '1' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('system_options','1',0,'UK Formats With IPT')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '1' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','1',1,'Overseas format')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '1' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('system_options','1',2,'Republic Of Ireland')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '1' AND fieldValue = 3)
INSERT INTO Audit_trail_custom_fields values('system_options','1',3,'UK Formats Without IPT')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '2' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('system_options','2',0,'Client Sequence')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '2' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','2',1,'Renewal Date Sequence')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '2' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('system_options','2',2,'Insurer Sequence')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '2' AND fieldValue = 3)
INSERT INTO Audit_trail_custom_fields values('system_options','2',3,'Brokerage Code Sequence')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '2' AND fieldValue = 4)
INSERT INTO Audit_trail_custom_fields values('system_options','2',4,'Consultant Sequence')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '2' AND fieldValue = 5)
INSERT INTO Audit_trail_custom_fields values('system_options','2',5,'Account Handler Sequence')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '2' AND fieldValue = 6)
INSERT INTO Audit_trail_custom_fields values('system_options','2',6,'Agent Sequence')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('system_options','5',0,'At End Of Transaction, No Daily Report')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','5',1,'At End Of Transaction, Daily Report')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('system_options','5',2,'Additional Reports For Client And Reinsurer')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5' AND fieldValue = 3)
INSERT INTO Audit_trail_custom_fields values('system_options','5',3,'Daily Report Only')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '8' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('system_options','8',0,'Statement Only')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '8' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','8',1,'Letter Only')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '8' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('system_options','8',2,'Both Statement And Letter')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '8' AND fieldValue = 3)
INSERT INTO Audit_trail_custom_fields values('system_options','8',3,'Neither Statement Nor Letter')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '9' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('system_options','9',0,'No Tracking Of Discounts')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '9' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','9',1,'Track Discounts With Sub-Agent Accounting')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '9' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('system_options','9',2,'Track Discounts Without Sub-Agent Accounting')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '16' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('system_options','16',0,'As Debited')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '16' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','16',1,'When Client Pays')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '16' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('system_options','16',2,'When Insurer Settled')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '16' AND fieldValue = 3)
INSERT INTO Audit_trail_custom_fields values('system_options','16',3,'When Policy Effective')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '10' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('system_options','10',0,'Spooler Only')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '10' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','10',1,'Documaster')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '10' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('system_options','10',2,'Microsoft Sharepoint 2010(or later)')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5163' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('system_options','5163',0,'Pure In-House Document Production')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5163' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','5163',1,'KCM Document Production')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5146' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('system_options','5146',0,'yyyyMMdd hhmmss tt')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5146' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','5146',1,'MMddyyyy hhmmss tt')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '60' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('system_options','60',0,'Cheque Production Not Installed')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '60' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','60',1,'Cheque Production Installed')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5069' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('system_options','5069',0,'Internal')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5069' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','5069',1,'External')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '1040' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('system_options','1040',0,'By Transaction Date')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '1040' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','1040',1,'By Cover Start Date')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '13' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('system_options','13',0,'No Address Look Up Installed')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '13' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','13',1,'UK QAS Rapid Installed')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '13' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('system_options','13',2,'UK QAS Pro Installed')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '13' AND fieldValue = 3)
INSERT INTO Audit_trail_custom_fields values('system_options','13',3,'UK QAS Names Installed')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '13' AND fieldValue = 4)
INSERT INTO Audit_trail_custom_fields values('system_options','13',4,'UK PAF Wrapper Installed')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5019' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('system_options','5019',0,'By Transaction Date')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5019' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','5019',1,'By Cover Start Date')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5019' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('system_options','5019',2,'By Risk Inception Date')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5019' AND fieldValue = 3)
INSERT INTO Audit_trail_custom_fields values('system_options','5019',3,'By Inception Date TPI')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5030' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('system_options','5030',0,'Reported Date')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5030' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('system_options','5030',1,'System Date')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='system_options' AND FieldName = '5030' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('system_options','5030',2,'Loss Date')

--Fee Maintenance
IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Fee_Amounts' AND FieldName = 'transaction_sub_type' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('Fee_Amounts','transaction_sub_type',0,'New Business')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Fee_Amounts' AND FieldName = 'transaction_sub_type' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('Fee_Amounts','transaction_sub_type',1,'Additional MTA')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Fee_Amounts' AND FieldName = 'transaction_sub_type' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('Fee_Amounts','transaction_sub_type',2,'Return MTA')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Fee_Amounts' AND FieldName = 'transaction_sub_type' AND fieldValue = 3)
INSERT INTO Audit_trail_custom_fields values('Fee_Amounts','transaction_sub_type',3,'Cancellation')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Fee_Amounts' AND FieldName = 'transaction_sub_type' AND fieldValue = 4)
INSERT INTO Audit_trail_custom_fields values('Fee_Amounts','transaction_sub_type',4,'Renewal')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Fee_Amounts' AND FieldName = 'transaction_sub_type' AND fieldValue = 5)
INSERT INTO Audit_trail_custom_fields values('Fee_Amounts','transaction_sub_type',5,'Re-Instatement')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Fee_Amounts' AND FieldName = 'calculation_basis' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('Fee_Amounts','calculation_basis',0,'Net Premium')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Fee_Amounts' AND FieldName = 'calculation_basis' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('Fee_Amounts','calculation_basis',1,'Net Premium + Tax Premium')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Fee_Amounts' AND FieldName = 'MakeLiveOptions_id' AND fieldValue = null)
INSERT INTO Audit_trail_custom_fields values('Fee_Amounts','MakeLiveOptions_id',null,'All')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Fee_Amounts' AND FieldName = 'MakeLiveOptions_id' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('Fee_Amounts','MakeLiveOptions_id',1,'Mark For Collection')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Fee_Amounts' AND FieldName = 'MakeLiveOptions_id' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('Fee_Amounts','MakeLiveOptions_id',2,'Instalments')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Fee_Amounts' AND FieldName = 'MakeLiveOptions_id' AND fieldValue = 3)
INSERT INTO Audit_trail_custom_fields values('Fee_Amounts','MakeLiveOptions_id',3,'Bank Guarantee')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Fee_Amounts' AND FieldName = 'MakeLiveOptions_id' AND fieldValue = 4)
INSERT INTO Audit_trail_custom_fields values('Fee_Amounts','MakeLiveOptions_id',4,'Pay Now')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Fee_Amounts' AND FieldName = 'MakeLiveOptions_id' AND fieldValue = 5)
INSERT INTO Audit_trail_custom_fields values('Fee_Amounts','MakeLiveOptions_id',5,'Cash Deposit')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Fee_Amounts' AND FieldName = 'MakeLiveOptions_id' AND fieldValue = 6)
INSERT INTO Audit_trail_custom_fields values('Fee_Amounts','MakeLiveOptions_id',6,'Invoice')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Fee_Amounts' AND FieldName = 'is_deleted' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('Fee_Amounts','is_deleted',0,NULL)

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Fee_Amounts' AND FieldName = 'is_deleted' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('Fee_Amounts','is_deleted',1,NULL)

--Re-Insurer Maintenace

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='party_insurer' AND FieldName = 'binder_indicator' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('party_insurer','binder_indicator',0,'All Outstanding')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='party_insurer' AND FieldName = 'binder_indicator' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('party_insurer','binder_indicator',1,'Paid By Client')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='party_insurer' AND FieldName = 'report_indicator' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('party_insurer','report_indicator',0,'Payment Date')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='party_insurer' AND FieldName = 'report_indicator' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('party_insurer','report_indicator',1,'Policy Number')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='party_insurer' AND FieldName = 'report_indicator' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('party_insurer','report_indicator',2,'Client Date')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='party_insurer' AND FieldName = 'report_indicator' AND fieldValue = 3)
INSERT INTO Audit_trail_custom_fields values('party_insurer','report_indicator',3,'Effective Date')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'payment_method_code' AND fieldValue = 'Fee')
INSERT INTO Audit_trail_custom_fields values('Party','payment_method_code','Fee','Payment Method')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'shortname' AND fieldValue = 'Fee')
INSERT INTO Audit_trail_custom_fields values('Party','shortname','Fee','Account Code')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'name' AND fieldValue = 'Fee')
INSERT INTO Audit_trail_custom_fields values('Party','name','Fee','Account Name')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'currency_id' AND fieldValue = 'Fee')
INSERT INTO Audit_trail_custom_fields values('Party','currency_id','Fee','Currency')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'shortname' AND fieldValue = 'Reinsurer')
INSERT INTO Audit_trail_custom_fields values('Party','shortname','Reinsurer','Code')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'name' AND fieldValue = 'Reinsurer')
INSERT INTO Audit_trail_custom_fields values('Party','name','Reinsurer','Name')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'sub_branch_id' AND fieldValue = 'Reinsurer')
INSERT INTO Audit_trail_custom_fields values('Party','sub_branch_id','Reinsurer','Sub-Branch')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'source_id' AND fieldValue = 'Reinsurer')
INSERT INTO Audit_trail_custom_fields values('Party','source_id','Reinsurer','Branch')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'shortname' AND fieldValue = 'Agent Group')
INSERT INTO Audit_trail_custom_fields values('Party','shortname','Agent Group','Agent Group Code')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'shortname' AND fieldValue = 'Agent')
INSERT INTO Audit_trail_custom_fields values('Party','shortname','Agent','Agent Code')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'name' AND fieldValue = 'Agent')
INSERT INTO Audit_trail_custom_fields values('Party','name','Agent','Name')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'party_category_id' AND fieldValue = 'Agent')
INSERT INTO Audit_trail_custom_fields values('Party','party_category_id','Agent','Category')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'file_code' AND fieldValue = 'Agent')
INSERT INTO Audit_trail_custom_fields values('Party','file_code','Agent','File Code')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'statements' AND fieldValue = 'Agent')
INSERT INTO Audit_trail_custom_fields values('Party','statements','Agent','Statement?')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'renewal_stop_code_id' AND fieldValue = 'Agent')
INSERT INTO Audit_trail_custom_fields values('Party','renewal_stop_code_id','Agent','Renewal Stop Code')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'renewal_stop_code_id' AND fieldValue = 'Agent')
INSERT INTO Audit_trail_custom_fields values('Party','renewal_stop_code_id','Agent','Renewal Stop Code')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'shortname' AND fieldValue = 'Account Executive')
INSERT INTO Audit_trail_custom_fields values('Party','shortname','Account Executive','Executive Code')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'name' AND fieldValue = 'Account Executive')
INSERT INTO Audit_trail_custom_fields values('Party','name','Account Executive','Lastname')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'shortname' AND fieldValue = 'Account Handler')
INSERT INTO Audit_trail_custom_fields values('Party','shortname','Account Handler','Handler Code')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'name' AND fieldValue = 'Account Handler')
INSERT INTO Audit_trail_custom_fields values('Party','name','Account Handler','Lastname')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'shortname' AND fieldValue = 'Discount Account')
INSERT INTO Audit_trail_custom_fields values('Party','shortname','Discount Account','Account Code')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'name' AND fieldValue = 'Discount Account')
INSERT INTO Audit_trail_custom_fields values('Party','name','Discount Account','Account Name')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'payment_method_code' AND fieldValue = 'Discount Account')
INSERT INTO Audit_trail_custom_fields values('Party','payment_method_code','Discount Account','Payment Method')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'payment_method_code' AND fieldValue = 'Extra Account')
INSERT INTO Audit_trail_custom_fields values('Party','payment_method_code','Extra Account','Payment Method')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'shortname' AND fieldValue = 'Extra Account')
INSERT INTO Audit_trail_custom_fields values('Party','shortname','Extra Account','Account Code')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'name' AND fieldValue = 'Extra Account')
INSERT INTO Audit_trail_custom_fields values('Party','name','Extra Account','Account Name')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'name' AND fieldValue = 'Agent Group')
INSERT INTO Audit_trail_custom_fields values('Party','name','Agent Group','Name')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'shortname' AND fieldValue = 'Agent Group')
INSERT INTO Audit_trail_custom_fields values('Party','shortname','Agent Group','Agent Group Code')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'source_id' AND fieldValue = 'Agent Group')
INSERT INTO Audit_trail_custom_fields values('Party','source_id','Agent Group','Branch')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party_Agent' AND FieldName = 'binder_indicator' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('Party_Agent','binder_indicator',0,'All Outstanding')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party_Agent' AND FieldName = 'binder_indicator' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('Party_Agent','binder_indicator',1,'Paid By Client')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party_Agent' AND FieldName = 'report_indicator' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('Party_Agent','report_indicator',0,'Payment Date')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party_Agent' AND FieldName = 'report_indicator' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('Party_Agent','report_indicator',1,'Policy Number')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party_Agent' AND FieldName = 'report_indicator' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('Party_Agent','report_indicator',2,'Client Code')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party_Agent' AND FieldName = 'report_indicator' AND fieldValue = 3)
INSERT INTO Audit_trail_custom_fields values('Party_Agent','report_indicator',3,'Renewal Date')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party_Agent' AND FieldName = 'report_indicator' AND fieldValue = 4)
INSERT INTO Audit_trail_custom_fields values('Party_Agent','report_indicator',4,'Risk Code')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party_Agent' AND FieldName = 'linked_account_group' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('Party_Agent','linked_account_group',0,NULL)

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Credit_Control_Rule' AND FieldName = 'policy_is_paid' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('Credit_Control_Rule','policy_is_paid',0,'Unpaid')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Credit_Control_Rule' AND FieldName = 'policy_is_paid' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('Credit_Control_Rule','policy_is_paid',1,'Paid')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party_Bank' AND FieldName = 'is_deleted' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('Party_Bank','is_deleted',0,'Active')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party_Bank' AND FieldName = 'is_deleted' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('Party_Bank','is_deleted',1,'Inactive')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party_Bank' AND FieldName = 'is_bank' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('Party_Bank','is_bank',0,'Credit Card')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party_Bank' AND FieldName = 'is_bank' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('Party_Bank','is_bank',1,'Bank Account')

--GIS Export
IF NOT EXISTS(SELECT NULL FROM foreign_key_table WHERE table_name = 'GIS_Property' AND column_name = 'index_linking_id')
INSERT INTO foreign_key_table VALUES ('GIS_Property','index_linking_id','index_linking','index_linking_id','description')

---Branch Maintenance

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Source' AND FieldName = 'is_deleted' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('Source','is_deleted',0,'Re-Open')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Source' AND FieldName = 'is_deleted' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('Source','is_deleted',1,'Closed')

--Product Risk Maintenance

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Product' AND FieldName = 'inclusion_of_CoInsurers_On_Claims' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('Product','inclusion_of_CoInsurers_On_Claims',0,'Not Required')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Product' AND FieldName = 'inclusion_of_CoInsurers_On_Claims' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('Product','inclusion_of_CoInsurers_On_Claims',1,'Required')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Product' AND FieldName = 'inclusion_of_CoInsurers_On_Claims' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('Product','inclusion_of_CoInsurers_On_Claims',2,'Required With Statistics')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Product' AND FieldName = 'allow_positive_cancellation' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('Product','allow_positive_cancellation',0,'Allow')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Product' AND FieldName = 'allow_positive_cancellation' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('Product','allow_positive_cancellation',1,'Deny')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Product' AND FieldName = 'allow_positive_cancellation' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('Product','allow_positive_cancellation',2,'Prompt')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Product' AND FieldName = 'out_of_sequence_mta_allocation' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('Product','out_of_sequence_mta_allocation',0,'Leave Unallocated')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Product' AND FieldName = 'out_of_sequence_mta_allocation' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('Product','out_of_sequence_mta_allocation',1,'Reverse Allocations')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Product' AND FieldName = 'out_of_sequence_mta_dates' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('Product','out_of_sequence_mta_dates',0,'Not Allowed')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Product' AND FieldName = 'out_of_sequence_mta_dates' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('Product','out_of_sequence_mta_dates',1,'Current Period Only')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Product' AND FieldName = 'out_of_sequence_mta_dates' AND fieldValue = 2)
INSERT INTO Audit_trail_custom_fields values('Product','out_of_sequence_mta_dates',2,'Current Period + 1')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Product' AND FieldName = 'out_of_sequence_mta_dates' AND fieldValue = 3)
INSERT INTO Audit_trail_custom_fields values('Product','out_of_sequence_mta_dates',3,'Unrestricted')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Product' AND FieldName = 'Default_Payment_Method' AND fieldValue = 0)
INSERT INTO Audit_trail_custom_fields values('Product','Default_Payment_Method',0,'Invoice')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Product' AND FieldName = 'Default_Payment_Method' AND fieldValue = 1)
INSERT INTO Audit_trail_custom_fields values('Product','Default_Payment_Method',1,'Instalments')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'sub_branch_id' AND fieldValue = 'Other Party')
INSERT INTO Audit_trail_custom_fields values('Party','sub_branch_id','Other Party','Sub-Branch')

IF NOT EXISTS(SELECT NULL FROM Audit_trail_custom_fields WHERE TableName='Party' AND FieldName = 'source_id' AND fieldValue = 'Other Party')
INSERT INTO Audit_trail_custom_fields values('Party','source_id','Other Party','Branch')
--
-- *****************************************************************************
-- * Manual Journal
-- * Date:   13/11/2024
-- *****************************************************************************
DECLARE @lCaptionID int
IF NOT EXISTS (SELECT 1 FROM Debtor_User_Groups_Type WHERE code = 'JN')
BEGIN	
	EXECUTE spu_pm_caption_id_return 1, 'Manual Journals', @lCaptionID OUTPUT
	INSERT INTO Debtor_User_Groups_Type(debtor_user_groups_type_id,caption_id, code, description,is_deleted, effective_date)
	VALUES ((Select Max(debtor_user_groups_type_id) + 1 from Debtor_User_Groups_Type),@lCaptionID, 'JN', 'Manual Journals',0, '2024-01-01')
END

GO
-- *****************************************************************************
-- * Author:  Kapil Sanotra
-- * Date:    03/12/2024      
-- * Purpose: Add additional Task Entries.
-- *****************************************************************************
declare @pmwrk_task_id int
declare @caption_id int
if not exists(select null from pmwrk_task where code = 'SAMAUT')
Begin
    EXECUTE spu_pm_caption_id_return 1, 'SamAuditTrail', @caption_id OUTPUT
    insert into pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values(@caption_id, 'SAMAUT', 'SamAuditTrail',0,getdate(),0,1,null,null,null,null,1,0,null,null,null,0,2,null)
End
GO
DECLARE @pmwrk_task_id INT
DECLARE @caption_id INT
IF NOT EXISTS(SELECT null FROM pmwrk_task WHERE code = 'SAMMJAUT')
BEGIN
    EXECUTE spu_pm_caption_id_return 1, 'SamJournalAuth', @caption_id OUTPUT
    INSERT INTO pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id, pmnavxm_process_id)
    values(@caption_id, 'SAMMJAUT', 'SamJournalAuth',0,getdate(),0,1,null,null,null,null,1,0,null,null,null,0,2,null)
END

go

--*****************************************************************************
-- * Author: Anil Kumar
-- * Date:   06/02/2025
-- *****************************************************************************

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMFINDADD', 'Add User Group'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMFINDADD'
GO

EXECUTE spu_SAM_PMWrk_Task_add 1, 'SAMGNCNO', 'Get Numbering SchemeNo'
GO

EXECUTE spu_SAM_PMWrk_Task_Group_Task_add 'SAMADMIN', 'SAMGNCNO'
GO

--*****************************************************************************
-- * Author: Prince
-- * Date:   20/03/2025  AZD - 24520 IH

-- *****************************************************************************

UPDATE Rule_set set risk_type_rule_set_type_id = 1 where risk_type_rule_set_type_id IS NULL

Go

--*****************************************************************************
-- * Author: Sravanti Pasumarti
-- * Date:   23/06/2025  AZD - 31547 RI Band Versioning
-- * Purpose : Insert the Caption for the button RI Band
-- *****************************************************************************
DECLARE @caption_id INT
EXEC spu_pm_caption_id_return 1, 'Configuration', @caption_id OUTPUT
GO	
--*****************************************************************************
-- * Author: Sravanti Pasumarti
-- * Date:   23/06/2025  AZD - 31547 RI Band Versioning
--*Purpose : Link the interface name upon click of Configuration button for RI Band
-- *****************************************************************************

DECLARE @captionID INT;

SELECT @captionID = caption_id  
FROM PMCaption 
WHERE caption = 'Configuration' AND language_id = 1;

IF @captionID IS NOT NULL 
BEGIN
    IF EXISTS (
        SELECT 1
        FROM PMProduct_Lookup 
        WHERE lookup_table_name = 'ri_band' AND linked_object_name IS NULL
    ) 
    BEGIN
        UPDATE PMProduct_Lookup 
        SET 
            linked_caption_id = @captionID, 
            linked_object_name = 'iPMURIBandVersion', 
            linked_class_name = 'NavigatorV3' 
        WHERE lookup_table_name = 'ri_band';
    END
END

GO
-- *****************************************************************************
-- * Author: Sravanti Pasumarti
-- * Date:   31/07/2025  AZD 31701 Event Log updates for Client Manager Edits
-- * Purpose : Insert the event type
-- *****************************************************************************
IF NOT EXISTS (Select 1 From Event_Type Where Code = 'CLIENTUPD')
BEGIN
    DECLARE @nCaptionID INTEGER
    DECLARE @nEvent_type_Id Integer

    EXECUTE spu_pm_caption_id_return 1, 'CLIENTUPD', @nCaptionID OUTPUT
   
    Select @nEvent_type_Id = Max(Event_type_Id)+1 From Event_type
    
    Insert into Event_Type
    (
        event_type_id,
        caption_id,
        code,
        description,
        is_deleted,
        effective_date,
        Event_type_Group_Id
    )
    Values
    (
        @nEvent_type_Id,
        @nCaptionID,
        'CLIENTUPD',
        'Client Updated',
        0,
        GETDATE(),
        1
    )
    END
GO

-- *****************************************************************************
-- * Author:	Sweta
-- * Date:		27/08/2025
-- * Purpose:	spu_wp_ris is not present in Database therefore deleting from wp_fields table
-- *****************************************************************************
 
IF NOT EXISTS(SELECT 1 FROM sysobjects WHERE name ='spu_wp_ris' and xtype='P' )
BEGIN
Delete wp_fields where sql = 'spu_wp_ris' and field_name = 'RiskCoverNoteLink'
END
GO

-- *****************************************************************************
-- * Author:	Sweta
-- * Date:		1/09/2025
-- * Purpose:	Spelling of Outstanding corrected in Reports
-- *****************************************************************************
 
update report set description = 'Outstanding Claims Report' where report_id = 1 and code = 'Out_Claim' and description = 'Oustanding Claims Report'
update report set description = 'Outstanding Claims Gross To Net Report' where report_id = 48 and code = 'OS_ClmGN' and description = 'Oustanding Claims Gross To Net Report'
GO

/**********************************************************************************
Name	:	SQL Script to Add a new Reinsurance Type
Date	:	12/12/2025
Author	:	Sanjana Gulia

**********************************************************************************/

    DECLARE @Caption_id INT;
    DECLARE @MAX_id INT;
IF NOT EXISTS (SELECT 1 FROM Reinsurance_Type WHERE code = 'PAX')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(Reinsurance_Type_id), 0) + 1
    FROM Reinsurance_Type;

    EXEC spu_pm_caption_id_return 1, 'Proportional XOL', @Caption_id OUTPUT;

    INSERT INTO Reinsurance_Type (
        Reinsurance_Type_id,
        Caption_id,
        Code,
        Description,
         is_deleted,
        effective_date
    )
    VALUES (
        @MAX_id,
        @Caption_id,
        'PAX',
        'Proportional XOL',
        0,
        GETDATE())
END

IF NOT EXISTS (SELECT 1 FROM Reinsurance_Type WHERE code = 'QSR')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(Reinsurance_Type_id), 0) + 1
    FROM Reinsurance_Type;

    EXEC spu_pm_caption_id_return 1, 'Quota Share Retained', @Caption_id OUTPUT;

    INSERT INTO Reinsurance_Type (
        Reinsurance_Type_id,
        Caption_id,
        Code,
        Description,
        is_deleted,
        effective_date
    )
    VALUES (
        @MAX_id,
        @Caption_id,
        'QSR',
        'Quota Share Retained',
        0,
        GETDATE())
END
GO

/**********************************************************************************
Name	:	SQL Script to Add a new Premium_Calculation_Basis
Date	:	16/12/2025
Author	:	Sanjana Gulia

**********************************************************************************/

DECLARE @Caption_id INT;
DECLARE @MAX_id INT;
-- Insert PROPGRSS
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'PROPGRSS')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'Prop RI % X Gross', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'PROPGRSS', 'Prop RI % X Gross', 0, GETDATE());
END;

-- Insert PROPGRFAC
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'PROPGRFAC')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'Prop RI % X (Gross - Fac)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'PROPGRFAC', 'Prop RI % X (Gross - Fac)', 0, GETDATE());
END;

-- Insert PRGRFACCAT
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'PRGRFACCAT')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'Prop RI % X (Gross - Fac - CAT)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'PRGRFACCAT', 'Prop RI % X (Gross - Fac - CAT)', 0, GETDATE());
END;

-- Insert PRGRFACXOL
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'PRGRFACXOL')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'Prop RI % X (Gross - Fac - XOL)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'PRGRFACXOL', 'Prop RI % X (Gross - Fac - XOL)', 0, GETDATE());
END;

-- Insert PRGFACXCAT
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'PRGFACXCAT')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'Prop RI % X (Gross - Fac - XOL - CAT)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'PRGFACXCAT', 'Prop RI % X (Gross - Fac - XOL - CAT)', 0, GETDATE());
END;

-- Insert PROPRETND
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'PROPRETND')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'Prop RI % X Retained', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'PROPRETND', 'Prop RI % X Retained', 0, GETDATE());
END;

-- Insert XOLRATEGRO
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'XOLRATEGRO')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'XOL Rate X Gross', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'XOLRATEGRO', 'XOL Rate X Gross', 0, GETDATE());
END;

-- Insert XOLGRSFAC
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'XOLGRSFAC')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'XOL Rate % X (Gross - Fac)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'XOLGRSFAC', 'XOL Rate % X (Gross - Fac)', 0, GETDATE());
END;

-- Insert XOLFACPRI
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'XOLFACPRI')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'XOL Rate % X (Gross - Fac - Prop RI)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'XOLFACPRI', 'XOL Rate % X (Gross - Fac - Prop RI)', 0, GETDATE());
END;

-- Insert XOLPRICAT
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'XOLPRICAT')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'XOL Rate % X (Gross - Fac - Prop RI - CAT)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'XOLPRICAT', 'XOL Rate % X (Gross - Fac - Prop RI - CAT)', 0, GETDATE());
END;

-- Insert XOLFACCAT
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'XOLFACCAT')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'XOL Rate % X (Gross - Fac - CAT)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'XOLFACCAT', 'XOL Rate % X (Gross - Fac - CAT)', 0, GETDATE());
END;
-- Insert CATRATEGRO
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'CATRATEGRO')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'CAT Rate % X Gross', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'CATRATEGRO', 'CAT Rate % X Gross', 0, GETDATE());
END;

-- Insert CATGRSFAC
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'CATGRSFAC')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'CAT Rate % X (Gross - Fac)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'CATGRSFAC', 'CAT Rate % X (Gross - Fac)', 0, GETDATE());
END;

-- Insert CATFACPRI
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'CATFACPRI')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'CAT Rate % X (Gross - Fac - Prop RI)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'CATFACPRI', 'CAT Rate % X (Gross - Fac - Prop RI)', 0, GETDATE());
END;

-- Insert CATPRIXOL
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'CATPRIXOL')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'CAT Rate % X (Gross - Fac - Prop RI - XOL)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'CATPRIXOL', 'CAT Rate % X (Gross - Fac - Prop RI - XOL)', 0, GETDATE());
END;

-- Insert CATFACXOL
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'CATFACXOL')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'CAT Rate % X (Gross - Fac - XOL)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'CATFACXOL', 'CAT Rate % X (Gross - Fac - XOL)', 0, GETDATE());
END;

-- Insert PROPNTXOPX
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'PROPNTXOPX')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'Prop RI % X (Gross - Fac - XOL- Prop XOL)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'PROPNTXOPX', 'Prop RI % X (Gross - Fac - XOL- Prop XOL)', 0, GETDATE());
END;

-- Insert PROPNTXPC
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'PROPNTXPC')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'Prop RI % X (Gross - Fac - XOL-Prop XOL - CAT)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'PROPNTXPC', 'Prop RI % X (Gross - Fac - XOL-Prop XOL - CAT)', 0, GETDATE());
END;

-- Insert CATNTPRXO
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'CATNTPRXO')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'CAT Rate X (Gross - Fac - Prop RI - XOL- Prop XOL)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'CATNTPRXO', 'CAT Rate X (Gross - Fac - Prop RI - XOL- Prop XOL)', 0, GETDATE());
END;

-- Insert CATNTXOPX
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'CATNTXOPX')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'CAT Rate X (Gross - Fac - XOL- Prop XOL)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'CATNTXOPX', 'CAT Rate X (Gross - Fac - XOL- Prop XOL)', 0, GETDATE());
END;

-- Insert PXGRS
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'PXGRS')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'XOL Rate X Gross', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'PXGRS', 'XOL Rate X Gross', 0, GETDATE());
END;

-- Insert PXGRSFAC
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'PXGRSFAC')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'XOL Rate % X (Gross - Fac)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'PXGRSFAC', 'XOL Rate % X (Gross - Fac)', 0, GETDATE());
END;

-- Insert PXFACPRP
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'PXFACPRP')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'XOL Rate % X (Gross - Fac - Prop RI)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'PXFACPRP', 'XOL Rate % X (Gross - Fac - Prop RI)', 0, GETDATE());
END;

-- Insert PXFPRPXOL
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'PXFPRPXOL')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'XOL Rate X (Gross - Fac - Prop RI- XOL)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'PXFPRPXOL', 'XOL Rate X (Gross - Fac - Prop RI- XOL)', 0, GETDATE());
END;

-- Insert PXFPRPCAT
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'PXFPRPCAT')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'XOL Rate % X (Gross - Fac - Prop RI - CAT)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'PXFPRPCAT', 'XOL Rate % X (Gross - Fac - Prop RI - CAT)', 0, GETDATE());
END;

-- Insert PXFPRCATXL
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'PXFPRCATXL')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'XOL Rate X (Gross - Fac - Prop RI - CAT-XOL)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'PXFPRCATXL', 'XOL Rate X (Gross - Fac - Prop RI - CAT-XOL)', 0, GETDATE());
END;

-- Insert PXFACCAT
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'PXFACCAT')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'XOL Rate % X (Gross - Fac - CAT)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'PXFACCAT', 'XOL Rate % X (Gross - Fac - CAT)', 0, GETDATE());
END;

-- Insert PXFACCATXL
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'PXFACCATXL')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'XOL Rate X (Gross - Fac - CAT-XOL)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'PXFACCATXL', 'XOL Rate X (Gross - Fac - CAT-XOL)', 0, GETDATE());
END;
Go

update Premium_Calculation_Basis set reinsurance_type_id =14 where code ='PROPRETND'
update Premium_Calculation_Basis set reinsurance_type_id =5 where code in(
'XOLRATEGRO',
'XOLGRSFAC', 
'XOLFACPRI', 
'XOLPRICAT', 
'XOLFACCAT')
update Premium_Calculation_Basis set reinsurance_type_id =13 where code in(
'PXGRS',
'PXGRSFAC',
'PXFACPRP',
'PXFPRPXOL',
'PXFPRPCAT',
'PXFPRCATXL',
'PXFACCAT',
'PXFACCATXL')
update Premium_Calculation_Basis set reinsurance_type_id =12 where code in(
'CATRATEGRO',
'CATGRSFAC', 
'CATFACPRI', 
'CATPRIXOL', 
'CATFACXOL',
'CATNTPRXO',
'CATNTXOPX')
go
-- *****************************************************************************
-- * Author:  Ramesh Kumar
-- * Date:    06/11/2025
-- * Purpose: Idea 85 - Auditing Client Manager Access via Back Office
-- *          Add Client Viewed event type for audit trail
-- *****************************************************************************

IF NOT EXISTS (Select 1 From Event_Type Where Code = 'CLVIEW')
BEGIN
    Declare @lCaptionID integer    
    Declare @Event_type_Id Integer
    Execute spu_pm_caption_id_return 1, 'CLVIEW', @lCaptionID output
    
    Select @Event_type_Id = Max(Event_type_Id)+1 From Event_type
    
    Insert into Event_Type
    (
        event_type_id,
        caption_id,
        code,
        description,
        is_deleted,
        effective_date,
        Event_type_Group_Id
    )
    Values
    (
        @Event_type_Id,
        @lCaptionID,
        'CLVIEW',
        'Client Viewed',
        0,
        GetDate(),
        1
    )
    END

GO


-- *****************************************************************************
-- * Author:   Amita Aggarwal
-- * Date:     24-11-2025
-- * Purpose:  EH100709 - Arch void transactions: Added New "Void" Insurance file type and status
-- *****************************************************************************
 IF NOT EXISTS ( SELECT NULL FROM Insurance_File_Type WHERE code='VOID')
  BEGIN
    DECLARE @Voidcaption_id INT
    DECLARE @Insurance_File_Type_id INT

    SELECT @Insurance_File_Type_id = MAX(ISNULL(Insurance_File_Type_id,0))+1 
    FROM Insurance_File_Type

    EXECUTE spu_pm_caption_id_return 1, 'Void', @Voidcaption_id OUTPUT

    INSERT INTO Insurance_File_Type
    (Insurance_File_Type_id,caption_id,code,description,var_data_structure_id,is_deleted,effective_date)
    VALUES
    (@Insurance_File_Type_id,@Voidcaption_id,'VOID','Void',NULL,0,GETDATE())
  END
  
  GO


   IF NOT EXISTS ( SELECT NULL FROM Insurance_File_Type WHERE code='VOIDREP')
  BEGIN
    DECLARE @VoidRepcaption_id INT
    DECLARE @Insurance_File_Type_id INT

    SELECT @Insurance_File_Type_id = MAX(ISNULL(Insurance_File_Type_id,0))+1 
    FROM Insurance_File_Type

    EXECUTE spu_pm_caption_id_return 1, 'Void Replaced', @VoidRepcaption_id OUTPUT

    INSERT INTO Insurance_File_Type
    (Insurance_File_Type_id,caption_id,code,description,var_data_structure_id,is_deleted,effective_date)
    VALUES
    (@Insurance_File_Type_id,@VoidRepcaption_id,'VOIDREP','Void Replaced',NULL,0,GETDATE())
  END
  
  GO

 IF NOT EXISTS ( SELECT NULL FROM Insurance_File_Type WHERE code='VOIDRENREP')
  BEGIN
    DECLARE @VoidRenRepcaption_id INT
    DECLARE @Insurance_File_Type_id INT

    SELECT @Insurance_File_Type_id = MAX(ISNULL(Insurance_File_Type_id,0))+1 
    FROM Insurance_File_Type

    EXECUTE spu_pm_caption_id_return 1, 'Void Renewal Replaced', @VoidRenRepcaption_id OUTPUT

    INSERT INTO Insurance_File_Type
    (Insurance_File_Type_id,caption_id,code,description,var_data_structure_id,is_deleted,effective_date)
    VALUES
    (@Insurance_File_Type_id,@VoidRenRepcaption_id ,'VOIDRENREP','Void Renewal Replaced',NULL,0,GETDATE())
  END
  
  GO


 IF NOT EXISTS ( SELECT NULL FROM Insurance_File_Status WHERE code='VOID')
  BEGIN
    DECLARE @Voidcaption_id INT
    DECLARE @Insurance_File_Status_id INT

    SELECT @Insurance_File_Status_id = MAX(ISNULL(Insurance_File_Type_id,0))+1 
    FROM Insurance_File_Type

    EXECUTE spu_pm_caption_id_return 1, 'Voided', @Voidcaption_id OUTPUT

    INSERT INTO Insurance_File_Status 
    (Insurance_File_Status_id,caption_id,code,description,is_deleted,effective_date)
    VALUES
    (@Insurance_File_Status_id,@Voidcaption_id,'VOID','Voided',0,GETDATE())
  END
  
  GO

/**********************************************************************************
Name	:	SQL Script to update the calculation factors 
Date	:	03/02/2026
Author	:	Sravanti Pasumarti

**********************************************************************************/
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Premium_Calculation_Basis')
BEGIN
UPDATE Premium_Calculation_Basis
SET calculation_factors = 
    CASE description
        -- Retained
        WHEN 'Prop RI % X Retained' THEN 'R'
        
        -- Gross only
        WHEN 'CAT Rate % X Gross' THEN 'G'
        WHEN 'Prop RI % X Gross' THEN 'G'
        WHEN 'XOL Rate X Gross' THEN 'G'
        
        -- Gross - Fac
        WHEN 'CAT Rate % X (Gross - Fac)' THEN 'G,F'
        WHEN 'Prop RI % X (Gross - Fac)' THEN 'G,F'
        WHEN 'XOL Rate % X (Gross - Fac)' THEN 'G,F'
        
        -- Gross - Fac - Prop RI
        WHEN 'CAT Rate % X (Gross - Fac - Prop RI)' THEN 'G,F,P'
        WHEN 'XOL Rate % X (Gross - Fac - Prop RI)' THEN 'G,F,P'
        
        -- Gross - Fac - XOL
        WHEN 'CAT Rate % X (Gross - Fac - XOL)' THEN 'G,F,TX'
        WHEN 'Prop RI % X (Gross - Fac - XOL)' THEN 'G,F,TX'
        
        -- Gross - Fac - CAT
        WHEN 'Prop RI % X (Gross - Fac - CAT)' THEN 'G,F,TC'
        WHEN 'XOL Rate % X (Gross - Fac - CAT)' THEN 'G,F,TC'
        
        -- Gross - Fac - Prop RI - XOL
        WHEN 'CAT Rate % X (Gross - Fac - Prop RI - XOL)' THEN 'G,F,P,TX'
        
        -- Gross - Fac - Prop RI - CAT
        WHEN 'XOL Rate % X (Gross - Fac - Prop RI - CAT)' THEN 'G,F,P,TC'
        
        -- Gross - Fac - XOL - CAT
        WHEN 'Prop RI % X (Gross - Fac - XOL - CAT)' THEN 'G,F,TX,TC'
        
        -- With Prop XOL (PX)
        WHEN 'CAT Rate X (Gross - Fac - XOL- Prop XOL)' THEN 'G,F,TX,PX'
        WHEN 'Prop RI % X (Gross - Fac - XOL- Prop XOL)' THEN 'G,F,TX,PX'
        WHEN 'XOL Rate X (Gross - Fac - Prop RI- XOL)' THEN 'G,F,P,TX'
        WHEN 'CAT Rate X (Gross - Fac - Prop RI - XOL- Prop XOL)' THEN 'G,F,P,TX,PX'
        WHEN 'Prop RI % X (Gross - Fac - XOL-Prop XOL - CAT)' THEN 'G,F,TX,PX,TC'
        WHEN 'XOL Rate % X (Gross - Fac - Prop RI - CAT)' THEN 'G,F,P,TC'
        WHEN 'XOL Rate X (Gross - Fac - CAT-XOL)' THEN 'G,F,TC,TX'
        WHEN 'XOL Rate X (Gross - Fac - Prop RI - CAT-XOL)' THEN 'G,F,P,TC,TX'
        
        ELSE calculation_factors
    END
WHERE is_deleted = 0 and calculation_factors IS NULL;

END

GO


/**********************************************************************************
Name	:	SQL Script to Add Premium_Calculation_Basis entries for Reinsurance Type 9 (Retained - RET RI This %)
Date	:	16/05/2025
Author	:	Sravanti Pasumarti

**********************************************************************************/

DECLARE @Caption_id INT;
DECLARE @MAX_id INT;

-- Insert T9PROPGRSS
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'T9PROPGRSS')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'RET RI This % X Gross', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'T9PROPGRSS', 'RET RI This % X Gross', 0, GETDATE());
END;

-- Insert T9PROPGRFC
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'T9PROPGRFC')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'RET RI This % X (Gross - Fac)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'T9PROPGRFC', 'RET RI This % X (Gross - Fac)', 0, GETDATE());
END;

-- Insert T9PRGRFCCT
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'T9PRGRFCCT')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'RET RI This % X (Gross - Fac - CAT)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'T9PRGRFCCT', 'RET RI This % X (Gross - Fac - CAT)', 0, GETDATE());
END;

-- Insert T9PRGRFCXL
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'T9PRGRFCXL')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'RET RI This % X (Gross - Fac - XOL)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'T9PRGRFCXL', 'RET RI This % X (Gross - Fac - XOL)', 0, GETDATE());
END;

-- Insert T9PRGFXCAT
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'T9PRGFXCAT')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'RET RI This % X (Gross - Fac - XOL - CAT )', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'T9PRGFXCAT', 'RET RI This % X (Gross - Fac - XOL - CAT )', 0, GETDATE());
END;

-- Insert T9PRNTXOPX
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'T9PRNTXOPX')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'RET RI This % X (Gross - Fac - XOL- Prop XOL)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'T9PRNTXOPX', 'RET RI This % X (Gross - Fac - XOL- Prop XOL)', 0, GETDATE());
END;

-- Insert T9PRNTXPCT
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'T9PRNTXPCT')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'RET RI This % X (Gross - Fac - XOL-Prop XOL - CAT )', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'T9PRNTXPCT', 'RET RI This % X (Gross - Fac - XOL-Prop XOL - CAT )', 0, GETDATE());
END;

-- Insert T9PRGRFCPR
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'T9PRGRFCPR')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'RET RI This % X (Gross - Fac- Prop)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'T9PRGRFCPR', 'RET RI This % X (Gross - Fac- Prop)', 0, GETDATE());
END;

-- Insert T9PRFCPRXL
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'T9PRFCPRXL')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'RET RI This % X (Gross - Fac- Prop- XOL)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'T9PRFCPRXL', 'RET RI This % X (Gross - Fac- Prop- XOL)', 0, GETDATE());
END;

-- Insert T9PRFCPRCT
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'T9PRFCPRCT')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'RET RI This % X (Gross - Fac- Prop- XOL- CAT)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'T9PRFCPRCT', 'RET RI This % X (Gross - Fac- Prop- XOL- CAT)', 0, GETDATE());
END;

-- Insert T9PRFCPRPX
IF NOT EXISTS (SELECT 1 FROM Premium_Calculation_Basis WHERE code = 'T9PRFCPRPX')
BEGIN
    SELECT @MAX_id = ISNULL(MAX(premium_calculation_basis_id), 0) + 1
    FROM Premium_Calculation_Basis;

    EXEC spu_pm_caption_id_return 1, 'RET RI This % X (Gross - Fac- Prop- XOL- CAT-Prop XOL)', @Caption_id OUTPUT;

    INSERT INTO Premium_Calculation_Basis (premium_calculation_basis_id, Caption_id, Code, Description, is_deleted, effective_date)
    VALUES (@MAX_id, @Caption_id, 'T9PRFCPRPX', 'RET RI This % X (Gross - Fac- Prop- XOL- CAT-Prop XOL)', 0, GETDATE());
END;
Go

-- Set reinsurance_type_id = 9 for all new RET RI This % entries
UPDATE Premium_Calculation_Basis SET reinsurance_type_id = 9 WHERE code IN (
'T9PROPGRSS',
'T9PROPGRFC',
'T9PRGRFCCT',
'T9PRGRFCXL',
'T9PRGFXCAT',
'T9PRNTXOPX',
'T9PRNTXPCT',
'T9PRGRFCPR',
'T9PRFCPRXL',
'T9PRFCPRCT',
'T9PRFCPRPX')
GO

-- Set calculation_factors for new RET RI This % entries
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Premium_Calculation_Basis')
BEGIN
UPDATE Premium_Calculation_Basis
SET calculation_factors =
    CASE description
        WHEN 'RET RI This % X Gross'                                        THEN 'G'
        WHEN 'RET RI This % X (Gross - Fac)'                                THEN 'G,F'
        WHEN 'RET RI This % X (Gross - Fac - CAT)'                          THEN 'G,F,TC'
        WHEN 'RET RI This % X (Gross - Fac - XOL)'                          THEN 'G,F,TX'
        WHEN 'RET RI This % X (Gross - Fac - XOL - CAT )'                   THEN 'G,F,TX,TC'
        WHEN 'RET RI This % X (Gross - Fac - XOL- Prop XOL)'                THEN 'G,F,TX,PX'
        WHEN 'RET RI This % X (Gross - Fac - XOL-Prop XOL - CAT )'          THEN 'G,F,TX,PX,TC'
        WHEN 'RET RI This % X (Gross - Fac- Prop)'                          THEN 'G,F,P'
        WHEN 'RET RI This % X (Gross - Fac- Prop- XOL)'                     THEN 'G,F,P,TX'
        WHEN 'RET RI This % X (Gross - Fac- Prop- XOL- CAT)'                THEN 'G,F,P,TX,TC'
        WHEN 'RET RI This % X (Gross - Fac- Prop- XOL- CAT-Prop XOL)'       THEN 'G,F,P,TX,TC,PX'
        ELSE calculation_factors
    END
WHERE code IN (
'T9PROPGRSS','T9PROPGRFC','T9PRGRFCCT','T9PRGRFCXL','T9PRGFXCAT',
'T9PRNTXOPX','T9PRNTXPCT','T9PRGRFCPR','T9PRFCPRXL','T9PRFCPRCT','T9PRFCPRPX')
AND is_deleted = 0;
END

GO
-- *****************************************************************************
-- * ADO #39336: Instalment for Claim Recovery - Scheme Configuration
-- * Purpose:  Add Claim Recovery entry to PFScheme_Type lookup table
-- *****************************************************************************
declare @caption_id int
IF NOT EXISTS (SELECT 1 FROM PFScheme_Type WHERE code = 'CR')
BEGIN
 EXECUTE spu_pm_caption_id_return 1, 'Claim Recovery', @caption_id OUTPUT
    INSERT INTO PFScheme_Type (pfscheme_type_id, code, description, caption_id, effective_date, is_deleted)
    VALUES (3, 'CR', 'Claim Recovery', @caption_id, GETDATE(), 0)


END
GO

-- *****************************************************************************


-- *****************************************************************************
-- * ADO #39336: Instalment for Claim Recovery - Transaction Type Mapping
-- * Purpose:  Add ICC/ICD transaction codes for claim recovery instalments
-- *****************************************************************************
DECLARE @icc_caption_id INT
DECLARE @icd_caption_id INT
DECLARE @icc_id INT
DECLARE @icd_id INT

IF NOT EXISTS (SELECT 1 FROM pfinstalments_transaction WHERE code = 'ICC')
BEGIN
    SELECT @icc_id = ISNULL(MAX(pfinstalments_transaction_id), 0) + 1 FROM pfinstalments_transaction
    EXECUTE spu_pm_caption_id_return 1, 'Instalment Claim Credit', @icc_caption_id OUTPUT
    INSERT INTO pfinstalments_transaction (pfinstalments_transaction_id, code, description, caption_id, effective_date, is_deleted)
    VALUES (@icc_id, 'ICC', 'Instalment Claim Credit', @icc_caption_id, GETDATE(), 0)
END

IF NOT EXISTS (SELECT 1 FROM pfinstalments_transaction WHERE code = 'ICD')
BEGIN
    SELECT @icd_id = ISNULL(MAX(pfinstalments_transaction_id), 0) + 1 FROM pfinstalments_transaction
    EXECUTE spu_pm_caption_id_return 1, 'Instalment Claim Debit', @icd_caption_id OUTPUT
    INSERT INTO pfinstalments_transaction (pfinstalments_transaction_id, code, description, caption_id, effective_date, is_deleted)
    VALUES (@icd_id, 'ICD', 'Instalment Claim Debit', @icd_caption_id, GETDATE(), 0)
END
GO



-- ============================================================
-- PBI 39544: System Events on Extracting Client Data
-- Task 1: Seed Data Migration Script
-- Seeds: event_type table + Audit_Trail_Modules table
-- ============================================================

SET NOCOUNT ON
GO

-- ----------------------------------------------------------
-- 1. event_type: Add 'CLIEXTRACT' - Client Data Extracted
--    Used by AddEvent (POST /core/event) via EventTypeKey
-- ----------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM Event_Type WHERE Code = 'CLIEXTRACT')
BEGIN
    DECLARE @lCaptionID   INT
    DECLARE @nEventTypeId INT
    DECLARE @nGroupId     INT

    -- Get next available event_type_id
    SELECT @nEventTypeId = MAX(event_type_id) + 1 FROM Event_type

    -- Get or create the caption
    EXECUTE spu_pm_caption_id_return 1, 'Client Data Extracted', @lCaptionID OUTPUT

    -- Resolve group: reuse the same group as CLVIEW (client-related events)
    SELECT @nGroupId = Event_type_Group_Id
    FROM Event_Type
    WHERE Code = 'CLVIEW'

    -- Fallback: use group 1 if CLVIEW not found
    IF @nGroupId IS NULL
        SET @nGroupId = 1

    INSERT INTO Event_Type
    (
        event_type_id,
        Event_type_Group_Id,
        Caption_id,
        Code,
        Description,
        is_deleted,
        effective_date
    )
    VALUES
    (
        @nEventTypeId,
        @nGroupId,
        @lCaptionID,
        'CLIEXTRACT',
        'Client Data Extracted',
        0,
        GETDATE()
    )

END
GO

-- ----------------------------------------------------------
-- 2. Audit_Trail_Modules: Add 'Extract Client Data'
--    Used by spu_get_audit_trail_moduleList to populate
--    the Event Type filter dropdown in secure/SystemEvents.aspx
--    IDs in this table are NOT identity — must assign explicitly
-- ----------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM Audit_Trail_Modules WHERE ModuleName = 'Extract Client Data')
BEGIN
    DECLARE @nModuleId INT
    SELECT @nModuleId = MAX(Modules_id) + 1 FROM Audit_Trail_Modules

    INSERT INTO Audit_Trail_Modules (Modules_id, ModuleName)
    VALUES (@nModuleId, 'Extract Client Data')
END

GO

SET NOCOUNT OFF
GO

-- * End of File
-- *****************************************************************************


