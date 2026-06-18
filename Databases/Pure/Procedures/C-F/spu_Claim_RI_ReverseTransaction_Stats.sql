SET QUOTED_IDENTIFIER OFF 

SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Claim_RI_ReverseTransaction_Stats'
GO


CREATE procedure spu_Claim_RI_ReverseTransaction_Stats    
    @Claim_id int,    
    @do_reversal int output,    
    @Stats_folder_cnt_new int Output    
    
AS    
--All the Values will go into new Stats Folder    
Declare @ri_arrangement_version int    
Declare @Stats_folder_cnt int, @stats_detail_id int    
Declare @Base_claim_id int    
Declare    
    @insurance_file_cnt int,    
    @Document_comment varchar(100),    
    @transaction_type_code varchar(10),    
    @created_by_user_id int,    
    @Debit_Credit varchar(1),    
    @transaction_type_id int,    
    @user_id int,    
    @user_name varchar(25)  
  
    
Select @Base_claim_id = base_claim_id from claim where Claim_id = @Claim_id    
    
Select    
        @insurance_file_cnt=insurance_file_cnt,    
        @Document_comment=Document_comment,    
        @transaction_type_code=transaction_type_code,    
        @transaction_type_id=transaction_type_id,    
        @created_by_user_id=created_by_user_id,    
        @Debit_Credit=Debit_Credit,    
        @user_id=created_by_user_id,    
  @user_name=created_by_username    
    
From Stats_folder    
    Where Stats_folder_cnt=(Select TOP 1 Stats_folder_cnt from Stats_folder Where loss_id=@Base_claim_id)    
    
Select @ri_arrangement_version=ri_arrangement_version    
From    
Claim_ri_arrangement where claim_id =@claim_id    
    
IF Exists(    
        Select * From claim_ri_arrangement Where claim_id in    
        (Select Claim_id From Claim Where base_claim_id=@Base_Claim_id    
        and Claim_id<@Claim_id and ri_arrangement_version=@ri_arrangement_version-1)    
        )    
    Set @do_reversal=1    
   else    
    Return --Return if No Rows Found ( Reversal not Required)    
                
--Create a New Stats Folder    
----------------------------    
Exec spu_add_stats_folder_claims    
    @stats_folder_cnt=@stats_folder_cnt_new output,    
    @insurance_file_cnt=@insurance_file_cnt,    
    @Debit_Credit=@Debit_Credit,    
    @document_comment=@Document_comment ,    
    @transaction_type_id=@transaction_type_id ,    
    @transaction_type_code=@transaction_type_code,    
    @user_id=@user_id,    
    @user_name=@user_name,    
    @claim_id=@claim_id ,    
    @documenttype_id=0   
  
--     
EXEC spu_Create_Reverse_Gross_StatsDetail  
   @Claim_id=@Claim_id,  
   @Stats_Folder_cnt=@Stats_Folder_cnt_new  
     
--Select * from stats_folder    
Declare stats_details_cur cursor    
    Static    
    For    
        Select sd.stats_detail_id, sd.stats_folder_cnt    
        From stats_detail sd    
            Join stats_folder sf ON sd.stats_folder_cnt=sf.stats_folder_cnt    
            Join Claim_Ri_arrangement cra ON cra.claim_id=sf.loss_id    
            Join Claim c ON c.Claim_id=cra.claim_id    
        Where base_claim_id=@Base_Claim_id    
            And C.Claim_id < @Claim_id And ri_arrangement_version=@ri_arrangement_version-1    
            and sd.Stats_detail_type in('FAC', 'FAX', 'TTY','TYX','NET')    
    
Open stats_details_cur    
    
Fetch next From stats_details_cur into @stats_detail_id,@Stats_folder_cnt    
WHILE @@Fetch_status=0    
BEGIN    
    
Declare @sd_id int    
Select @sd_id =IsNull(max(stats_detail_id),0)+1 from stats_detail where Stats_folder_cnt=@Stats_folder_cnt_new    
    
Insert into stats_detail(    
    stats_folder_cnt,    
    stats_detail_id,    
    stats_detail_type,    
    risk_id,    
    risk_type_id,    
    risk_type_code,    
    peril_id,    
    peril_description,    
    peril_type_id,    
    peril_type_code,    
    policy_section_type_id,    
    policy_section_type_code,    
    class_of_business_id,    
    class_of_business_code,    
    tax_type_id,    
    tax_type_code,    
    tax_value,    
    ri_party_cnt,    
    ri_shortname,    
    ri_party_type,    
    ri_share_percent,    
    ri_agreement_code,    
    annual_premium,    
    currency_code,    
    currency_rate,    
    this_premium_original,    
    this_premium_home,    
    commission_percent,    
    lead_commission_value_home,    
    sub_commission_value_home,    
    sum_insured_home,    
    sum_insured_currency_code,    
    sum_insured_change,    
    transaction_ledger_id,    
    transaction_account_id,    
    account_type_code,    
    ceded_ref,    
    cover_share_percent,    
    sum_insured_total,    
    charges_total,    
    taxes_total,    
    recoveries_total,    
    commission_excluded,    
    withholding_tax_excluded,    
    purchase_order_no,    
    purchase_invoice_no,    
    stats_version,    
    this_premium_system,    
    lead_commission_value_system,    
    sub_commission_value_system,    
    sum_insured_system,    
    is_commission_modified,    
    original_flag,    
    cover_to_date,    
    Claim_RI_Only_Amendment)    
    
Select @stats_folder_cnt_New,    
    @sd_id,    
    stats_detail_type,    
    risk_id,    
    risk_type_id,    
    risk_type_code,    
    peril_id,    
    peril_description,    
    peril_type_id,    
    peril_type_code,    
    policy_section_type_id,    
    policy_section_type_code,    
    class_of_business_id,    
    class_of_business_code,    
    tax_type_id,    
    tax_type_code,    
    tax_value * -1,    
    ri_party_cnt,    
    ri_shortname,    
    ri_party_type,    
    ri_share_percent,    
    ri_agreement_code,    
    annual_premium * -1,    
    currency_code,    
    currency_rate,    
    this_premium_original * -1,    
    this_premium_home * -1,    
    commission_percent,    
    lead_commission_value_home * -1,    
    sub_commission_value_home * -1,    
    sum_insured_home * -1,    
    sum_insured_currency_code,    
    sum_insured_change,    
    transaction_ledger_id,    
    transaction_account_id,    
    account_type_code,    
    ceded_ref,    
    cover_share_percent,    
    sum_insured_total * -1,    
    charges_total * -1,    
    taxes_total * -1,    
    recoveries_total * -1,    
    commission_excluded * -1,    
    withholding_tax_excluded * -1,    
    purchase_order_no,    
    purchase_invoice_no,    
    stats_version,    
    this_premium_system * -1,    
    lead_commission_value_system * -1,    
    sub_commission_value_system * -1,    
    sum_insured_system * -1,    
    is_commission_modified,    
    original_flag,    
    cover_to_date,    
    1    
    From Stats_detail    
    Where Stats_folder_cnt=@Stats_folder_cnt And stats_detail_id=@stats_detail_id    
    
Fetch next From stats_details_cur into @stats_detail_id,@Stats_folder_cnt    
    
END    
Close stats_details_cur    
Deallocate stats_details_cur    
  

SET QUOTED_IDENTIFIER OFF 

SET ANSI_NULLS ON
GO
