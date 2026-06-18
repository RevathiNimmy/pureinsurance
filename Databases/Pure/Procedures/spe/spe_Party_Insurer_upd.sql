SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spe_Party_Insurer_upd'
GO

-- RAW 18/12/2002 : PS187 : Added new data items  
--DC110803 -added new fsa fields  
--ECK10/10/2005 New Claims Rating Fields for Datasure   
CREATE PROCEDURE spe_Party_Insurer_upd  
    @party_cnt int,  
    @agency_number varchar(255),  
    @binder_indicator int,  
    @report_indicator int,  
    @is_reinsurer int,  
    @reinsurance_type int,  
    @is_reinsurance_debit_credit_n int,  
    @default_comm_rate float,  
    @tax_group_id INT,  
    @payment_method integer,  
    @payment_frequency integer,  
    @bank_account varchar(30),  
    @fsa_registration_number varchar(255),  
    @fsa_insurercreditrating_id integer,  
    @fsa_insurerstatus_id integer,  
    @is_retained tinyint,
    @claims_rating_agency_id integer,
    @claims_rating_grading varchar(255),
    @claims_rating_date datetime,
    @claims_rating_description varchar(4000),
    @terms_of_payment_id integer = Null,
    @domiciled_for_Tax tinyint,
    @risk_transfer_agreement bit,  
    @Brokerlink_Subaccount int,
    @Brokerlink_UW_ID varchar(10),
    @Is_ri_Broker tinyint,
    @insurer_locking_type_id int = 1,
    @risk_transfer_editable bit,
    @insurer_type_id int,
    @bureau_account_cnt int = NULL,        
    @UserId int=NULL,        
    @UniqueId VARCHAR(50)=NULL,          
    @ScreenHierarchy VARCHAR(500)=NULL
AS  
BEGIN  
UPDATE Party_Insurer  
    SET  
    agency_number=@agency_number,  
    binder_indicator=@binder_indicator,  
    report_indicator=@report_indicator,  
    is_reinsurer=@is_reinsurer,  
    reinsurance_type=@reinsurance_type,  
    is_reinsurance_debit_credit_no=@is_reinsurance_debit_credit_n,  
    default_comm_rate=@default_comm_rate,  
    tax_group_id=@tax_group_id,  
    payment_method=@payment_method,  
    payment_frequency=@payment_frequency,  
    bank_account=@bank_account,  
    fsa_registration_number=@fsa_registration_number,  
    fsa_insurercreditrating_id=@fsa_insurercreditrating_id,  
    fsa_insurerstatus_id=@fsa_insurerstatus_id,  
    is_retained=@is_retained,
    claims_rating_agency_id=@claims_rating_agency_id,
    claims_rating_grading=@claims_rating_grading,
    claims_rating_date=@claims_rating_date,
    claims_rating_description=@claims_rating_description,
    terms_of_payment_id=@terms_of_payment_id,
    domiciled_for_tax=@domiciled_for_Tax,
    risk_transfer_agreement=@risk_transfer_agreement,
    Brokerlink_Subaccount=@Brokerlink_Subaccount,
    Brokerlink_UW_ID=@Brokerlink_UW_ID,
    is_ri_broker=@Is_ri_Broker,
    insurer_locking_type_id=@insurer_locking_type_id,
    risk_transfer_editable=@risk_transfer_editable,
    insurer_type_id = @insurer_type_id,
    bureauaccountparty=@bureau_account_cnt,    
    UserId = @UserId,    
    UniqueId = @UniqueId,    
    ScreenHierarchy = @ScreenHierarchy
    
WHERE party_cnt = @party_cnt  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
