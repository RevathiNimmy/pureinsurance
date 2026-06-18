SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spe_Party_Agent_upd'
GO


-- DC070803 PS253 -added extra parameter agent_status_id
-- TR 08/12/03 - Put AccountType & LedgerType update into Broking Only wrapper.
-- TR 08/12/03 - Put ElementID & NodeID update into Broking Only wrapper.
-- SFU - Does not support changing Agent Types after they are created. (As per Sarah Johnstone)
-- DC070205 PN18484 added checks for Introducer Agent Type and Introducer Ledger
CREATE PROCEDURE spe_Party_Agent_upd
    @party_cnt int,
    @party_agent_type_id int,
    @party_agent_origin_id int,
    @agency_agreement_date datetime,
    @agency_next_review_date datetime,
    @agency_account_number varchar(255),
    @is_branch tinyint,
    @is_head_office tinyint,
    @default_commission_percent numeric(19,4),
    @trading_name varchar(255),
    @binder_indicator int,
    @report_indicator int,
    @linked_account_executive_id int,
    @linked_account_group int,
    @payment_method int,
    @payment_frequency int,
    @address_on_notice int,
    @type_of_statement int,
    @source int,
    @title varchar(70),
    @multipac tinyint,
    @contact_person varchar(255),
    @first_name varchar(255),
    --@bank_account varchar(30),
    @date_cancelled datetime,
    @agent_status_id int,
    @fsa_registration_number varchar(255),
    @broker_abi_id varchar(20),
    @expense_account_id int,
    @is_in_transfer_mode tinyint,
    @transfer_to_business_type_id smallint,
    @transfer_to_party_cnt int,
    @use_override_commission_rate tinyint,
    @use_override_commission_renewal tinyint,
    @domiciled_for_tax tinyint,
    @allow_consolidated_commission tinyint,
    @can_make_live_invoice tinyint,
    @can_make_live_instalments tinyint,
    @can_make_live_paynow tinyint,
    @is_standard_account tinyint,
    @is_float_balance_account tinyint,
    @is_overdraft_account tinyint,
    @is_prepayment_account tinyint,
    @expected_daily_premium money,
    @days_allowed smallint,
    @float_balance_limit money,
    @overdraft_limit money,
    @overdraft_expiry datetime,
    @alternate_reference_mandatory tinyint,
    @alternate_reference_for_each_transaction tinyint,  --(RC) QBENZ014
    @commission_posting_type int, --(RC) PLICO 9-10
    @is_single_instalment_plan tinyint,
    @common_renewal_date datetime,
    @produce_agent_renewal_list tinyint,	 
    @can_make_live_bankguarantee tinyint,
	@can_make_live_cashdeposit tinyint = 0, --Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
	@commission_level_id int = NULL,    
    @is_gross_agent tinyint,
    @bankaccount_id int = null,
	@receives_client_correspondence TINYINT =0,
 @user_id int=NULL,
 @unique_id varchar(50)=NULL,
 @screen_hierarchy varchar(500)=NULL
 AS
BEGIN

IF NOT EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U') BEGIN
 -- Broking upgrade code goes here
    -- MKW030603 PN 4457 Update accounts START
    DECLARE @NewLedgerType int
    DECLARE @NewAccountType int
    DECLARE @ElementID int
    DECLARE @NodeID int

    Select @NewLedgerType = ledgertype_id
    FROM ledgertype
    WHERE
     ((description = 'Introducer'
      and
      @Party_agent_type_id = 3)
  OR
     (description = 'Sub Agent'
      and
      @Party_agent_type_id = 2))
     OR
     (description = 'Agent'
      and
      @Party_agent_type_id = 1)

    select @NewAccountType = accounttype_id
    FROM accounttype
    WHERE
     ((Description = 'Liability'
      and
     @Party_agent_type_id = 3)
  or
     (Description = 'Asset'
      and
      @Party_agent_type_id = 2))
     or
     (Description = 'Liability'
      and
     @Party_agent_type_id = 1)

    update account
    set ledger_id=@NewLedgerType,
    accounttype_id = @NewAccountType
    where account_key = @party_cnt

    SELECT @ElementID = element_id
    FROM element
    WHERE
     ((element_name = 'Introducer Ledger'
      and
      @party_agent_type_id = 3
     )
  or
     (element_name = 'Sub Agent Ledger'
      and
      @party_agent_type_id = 2
     ))
     OR
     (element_name = 'Agent Ledger'
      and
      @party_agent_type_id = 1
     )

    SELECT @NodeId = node_id
    FROM StructureTree
    WHERE element_id = @ElementId

    UPDATE StructureTree
    SET parent_node_id = @NodeId
    where account_id=
     (select account_id
      FROM account
      WHERE account_key=@party_cnt)
    -- MKW030603 PN 4457 Update accounts END
END

UPDATE Party_Agent
    SET
    party_agent_type_id=@party_agent_type_id,
    party_agent_origin_id=@party_agent_origin_id,
    agency_agreement_date=@agency_agreement_date,
    agency_next_review_date=@agency_next_review_date,
    agency_account_number=@agency_account_number,
    is_branch=@is_branch,
    is_head_office=@is_head_office,
    default_commission_percent=@default_commission_percent,
    trading_name=@trading_name,
    binder_indicator=@binder_indicator,
    report_indicator=@report_indicator,
    linked_account_executive_id=@linked_account_executive_id,
    linked_account_group=@linked_account_group,
    payment_method=@payment_method,
    payment_frequency=@payment_frequency,
    address_on_notice=@address_on_notice,
    type_of_statement=@type_of_statement,
    source=@source,
    title=@title,
    multipac=@multipac,
    contact_person=@contact_person,
    first_name=@first_name,
    --bank_account=@bank_account,
    date_cancelled=@date_cancelled,
    agent_status_id=@agent_status_id,
    fsa_registration_number=@fsa_registration_number,
    broker_abi_id=@broker_abi_id,
    expense_account_id=@expense_account_id,
    is_in_transfer_mode=@is_in_transfer_mode,
    transfer_to_business_type_id=@transfer_to_business_type_id,
    transfer_to_party_cnt=@transfer_to_party_cnt,
    use_override_commission_rate =@use_override_commission_rate,
    use_override_commission_renewal =@use_override_commission_renewal,
    domiciled_for_tax = @domiciled_for_tax,
    allow_consolidated_commission = @allow_consolidated_commission,
    can_make_live_invoice=@can_make_live_invoice,
    can_make_live_instalments=@can_make_live_instalments,
    can_make_live_paynow=@can_make_live_paynow,
    is_standard_account=@is_standard_account,
    is_float_balance_account=@is_float_balance_account,
    is_overdraft_account=@is_overdraft_account,
    is_prepayment_account=@is_prepayment_account,
    expected_daily_premium=@expected_daily_premium,
    days_allowed=@days_allowed,
    float_balance_limit=@float_balance_limit,
    overdraft_limit=@overdraft_limit,
    overdraft_expiry=@overdraft_expiry,
    alternate_reference_mandatory=@alternate_reference_mandatory,
    alternate_reference_for_each_transaction=@alternate_reference_for_each_transaction, --(RC) QBENZ014
    commission_posting_type_id=@commission_posting_type, --(RC) PLICO 9-10
    is_single_instalment_plan=@is_single_instalment_plan, 
    common_renewal_date=@common_renewal_date,
    produce_agent_renewal_list=@produce_agent_renewal_list,	
    can_make_live_bankguarantee = @can_make_live_bankguarantee,
    can_make_live_cashdeposit = @can_make_live_cashdeposit, --Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
    commission_level_id = @commission_level_id,     
    is_gross_agent=@is_gross_agent,
    bankaccount_id=@bankaccount_id,
	Receives_Client_Correspondence = @receives_client_correspondence,
	UserId = @user_id,
	UniqueId = @unique_id,
	ScreenHierarchy = @screen_hierarchy
WHERE party_cnt = @party_cnt
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
