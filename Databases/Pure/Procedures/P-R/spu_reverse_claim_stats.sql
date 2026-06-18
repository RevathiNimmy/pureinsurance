
Execute DDLDropProcedure 'spu_reverse_claim_stats'
GO

CREATE PROCEDURE spu_reverse_claim_stats  
@claim_id INT,  
@New_Stats_Folder_Cnt INT,
@transaction_type_code CHAR(10),   
@ThisRevesionPresent INT OUTPUT   
AS  

Declare @ri_arrangement_version int  
Declare @Stats_folder_cnt int, @stats_detail_id int  
Declare @Base_claim_id int  
Declare  
    @insurance_file_cnt int,  
    @Document_comment varchar(100),  
    @created_by_user_id int,  
    @Debit_Credit varchar(1),  
    @transaction_type_id int,  
    @user_id int,  
    @user_name varchar(25)  

SELECT @ThisRevesionPresent=(CASE  
       WHEN ROUND(res.this_revision,2) <>0 THEN  1  
       ELSE 0  
       END)
 FROM Claim_Payment_Item cpi  
 LEFT OUTER JOIN Claim_payment cp ON cp.claim_payment_id =cpi.claim_payment_id 
 LEFT OUTER JOIN Reserve Res ON Res.Reserve_id =cpi.reserve_id  
 WHERE CP.claim_id =@CLAIM_ID AND CPI.this_payment <>0  
 AND CP.claim_payment_id = CP.base_claim_payment_id 
    
Declare stats_details_cur cursor  
    Static  
    For  
        Select sd.stats_detail_id, sd.stats_folder_cnt  
        From stats_detail sd  
            Join stats_folder sf ON sd.stats_folder_cnt=sf.stats_folder_cnt  
        Where sf.loss_id=@claim_id  and sf.stats_folder_cnt <> @New_Stats_Folder_Cnt  
			and sf.transaction_type_code = @transaction_type_code 
			and sf.document_ref NOT LIKE 'CLC%' and sf.document_ref NOT LIKE 'CLD%'
            and sd.Stats_detail_type in('FAC', 'FAX', 'TTY','TYX','TFS','NET','GRS')  
  
Open stats_details_cur  
  
Fetch next From stats_details_cur into @stats_detail_id,@Stats_folder_cnt  
WHILE @@Fetch_status=0  
BEGIN  
  
Declare @sd_id int  
Select @sd_id =IsNull(max(stats_detail_id),0)+1 from stats_detail where Stats_folder_cnt=@New_Stats_Folder_Cnt  
  
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
  
Select @New_Stats_Folder_Cnt,  
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
  
   Update Stats_Detail  
      SET tax_value = 0,  
      annual_premium=0,  
      this_premium_original =0,  
      this_premium_home=0,  
      lead_commission_value_home=0,  
      sub_commission_value_home=0,  
      sum_insured_home=0,  
      sum_insured_total=0,  
      charges_total=0,  
      taxes_total =0,  
      recoveries_total =0,  
      commission_excluded =0,  
      withholding_tax_excluded =0,  
      this_premium_system =0,  
      lead_commission_value_system =0,  
      sub_commission_value_system =0,  
      sum_insured_system =0  
      Where stats_folder_cnt = @New_Stats_Folder_Cnt and Stats_detail_type ='GRS'  
  
        
END  
Close stats_details_cur  
Deallocate stats_details_cur  
SELECT   @ThisRevesionPresent