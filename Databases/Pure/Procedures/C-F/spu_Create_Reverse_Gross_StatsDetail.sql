SET QUOTED_IDENTIFIER OFF 

SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Create_Reverse_Gross_StatsDetail'
GO
  
CREATE Procedure spu_Create_Reverse_Gross_StatsDetail  
    @Claim_id INT,  
    @Stats_Folder_cnt INT  
  
--Copies the Details from @Copy_Stats_Folder_cnt IN @Stats_Folder_cnt      
As  
Declare   
    @risk_id INT,  
    @risk_type_id INT,  
    @risk_type_code  Varchar(10) ,  
    @peril_id INT,  
    @peril_description VARCHAR(30) ,  
    @peril_type_id INT,  
    @peril_type_code  VARCHAR(10),  
    @policy_section_type_id INT,  
    @policy_section_type_code  VARCHAR(10),  
    @class_of_business_id  INT,  
    @class_of_business_code VARCHAR(10),  
    @tax_type_id INT ,  
    @tax_type_code VARCHAR(10),  
    @ri_party_cnt INT,  
    @ri_shortname VARCHAR(20),  
    @ri_party_type VARCHAR(3) ,  
    @ri_share_percent Numeric(19,4),  
    @ri_agreement_code INT ,  
    @currency_code varchar(15) ,  
    @currency_rate Numeric(19,4),  
    @commission_percent Numeric(19,4),  
    @sum_insured_currency_code VARCHAR(10),  
    @sum_insured_change  Numeric(19,4),  
    @transaction_ledger_id  VARCHAR(10),  
    @transaction_account_id  INT,  
    @account_type_code VARCHAR(10),  
    @ceded_ref VARCHAR(20),  
    @cover_share_percent Numeric(19,4),  
    @purchase_order_no  VARCHAR(40),  
    @purchase_invoice_no  VARCHAR(40),  
    @stats_version  INT,  
    @is_commission_modified INT,  
    @original_flag INT,  
    @cover_to_date  DATETIME,  
    @Claim_RI_Only_Amendment INT,  
    @Copy_Stats_Folder_cnt INT ,  
    @Base_claim_id int    
  
Select @Base_claim_id = base_claim_id from claim where Claim_id = @Claim_id   
      
    Select @Copy_Stats_Folder_cnt= min(sd.stats_folder_cnt)  
    From stats_detail sd    
        Join stats_folder sf ON sd.stats_folder_cnt=sf.stats_folder_cnt    
        Join Claim_Ri_arrangement cra ON cra.claim_id=sf.loss_id    
        Join Claim c ON c.Claim_id=cra.claim_id    
    Where base_claim_id=@Base_Claim_id    
        And C.Claim_id < @Claim_id  
        And sd.Stats_detail_type ='GRS'  
  
  
--Insert GRS Line  
Insert into Stats_detail (  
    stats_folder_cnt,  
    Stats_Detail_Id,  
    stats_detail_type,  
    Annual_premium,    
    this_premium_original,    
    this_premium_home,    
    tax_value,    
    lead_commission_value_home,    
    sub_commission_value_home,    
    sum_insured_home,    
    sum_insured_total,    
    charges_total,    
    taxes_total,    
    recoveries_total,    
    commission_excluded,    
    withholding_tax_excluded,    
    this_premium_system,    
    lead_commission_value_system,    
    sub_commission_value_system,   
    sum_insured_system  
)  
  
Select   
    @stats_folder_cnt,  
    1,  
    sd.Stats_detail_type,    
    sum(Annual_premium)* -1,    
    Sum(this_premium_original)* -1,    
    Sum(this_premium_home)* -1,    
    Sum(tax_value) * -1,    
    Sum(lead_commission_value_home) * -1,    
    Sum(sub_commission_value_home) * -1,    
    Sum(sum_insured_home) * -1,    
    Sum(sum_insured_total) * -1,    
    Sum(charges_total) * -1,    
    Sum(taxes_total) * -1,    
    Sum(recoveries_total) * -1,    
    Sum(commission_excluded) * -1,    
    Sum(withholding_tax_excluded) * -1,    
    Sum(this_premium_system) * -1,    
    Sum(lead_commission_value_system )* -1,    
    Sum(sub_commission_value_system )* -1,    
    Sum(sum_insured_system) * -1  
  
From stats_detail sd    
    Join stats_folder sf ON sd.stats_folder_cnt=sf.stats_folder_cnt    
    Join Claim_Ri_arrangement cra ON cra.claim_id=sf.loss_id    
    Join Claim c ON c.Claim_id=cra.claim_id    
Where base_claim_id=@Base_Claim_id  
    And C.Claim_id < @Claim_id     
    and sd.Stats_detail_type ='GRS'  
Group by Stats_detail_type  
  
Select   
    @risk_id=risk_id,  
    @risk_type_id = risk_type_id,  
    @risk_type_code = risk_type_code,  
    @peril_id = peril_id,  
    @peril_description = peril_description,  
    @peril_type_id = peril_type_id,  
    @peril_type_code = peril_type_code,  
    @policy_section_type_id = policy_section_type_id,  
    @policy_section_type_code = policy_section_type_code,  
    @class_of_business_id = class_of_business_id,  
    @class_of_business_code = class_of_business_code,  
    @tax_type_id = tax_type_id,  
    @tax_type_code = tax_type_code,  
    @ri_party_cnt = ri_party_cnt,  
    @ri_shortname = ri_shortname,  
    @ri_party_type = ri_party_type,  
    @ri_share_percent = ri_share_percent,  
    @ri_agreement_code = ri_agreement_code,  
    @currency_code = currency_code,  
    @currency_rate = currency_rate,  
    @commission_percent = commission_percent,  
    @sum_insured_currency_code = sum_insured_currency_code,  
    @sum_insured_change = sum_insured_change,  
    @transaction_ledger_id = transaction_ledger_id,  
    @transaction_account_id = transaction_account_id,  
    @account_type_code = account_type_code,  
    @ceded_ref = ceded_ref,  
    @cover_share_percent = cover_share_percent,  
    @purchase_order_no = purchase_order_no,  
    @purchase_invoice_no = purchase_invoice_no,  
    @stats_version = stats_version,  
    @is_commission_modified = is_commission_modified,  
    @original_flag = original_flag,  
    @cover_to_date = cover_to_date,  
    @Claim_RI_Only_Amendment = Claim_RI_Only_Amendment  
From Stats_detail  
Where Stats_Folder_cnt=@Copy_Stats_Folder_cnt  
AND  Stats_detail_type ='GRS'  
   
Update Stats_Detail Set  
    risk_id=@risk_id,  
    risk_type_id = @risk_type_id,  
    risk_type_code = @risk_type_code,  
    peril_id = @peril_id,  
    peril_description = @peril_description,  
    peril_type_id = @peril_type_id,  
    peril_type_code = @peril_type_code,  
    policy_section_type_id = @policy_section_type_id,  
    policy_section_type_code = @policy_section_type_code,  
    class_of_business_id = @class_of_business_id,  
    class_of_business_code = @class_of_business_code,  
    tax_type_id = @tax_type_id,  
    tax_type_code = @tax_type_code,  
    ri_party_cnt = @ri_party_cnt,  
    ri_shortname = @ri_shortname,  
    ri_party_type = @ri_party_type,  
    ri_share_percent = @ri_share_percent,  
    ri_agreement_code = @ri_agreement_code,  
    currency_code = @currency_code,  
    currency_rate = @currency_rate,  
    commission_percent = @commission_percent,  
    sum_insured_currency_code = @sum_insured_currency_code,  
    sum_insured_change = @sum_insured_change,  
    transaction_ledger_id = @transaction_ledger_id,  
    transaction_account_id = @transaction_account_id,  
    account_type_code = @account_type_code,  
    ceded_ref = @ceded_ref,  
    cover_share_percent = @cover_share_percent,  
    purchase_order_no = @purchase_order_no,  
    purchase_invoice_no =@purchase_invoice_no,  
    stats_version = @stats_version,  
    is_commission_modified = @is_commission_modified,  
    original_flag = @original_flag,  
    cover_to_date = @cover_to_date,  
    Claim_RI_Only_Amendment = @Claim_RI_Only_Amendment  
    From Stats_detail  
Where Stats_Folder_cnt=@Stats_folder_cnt  
  
SET QUOTED_IDENTIFIER OFF 

SET ANSI_NULLS ON
GO
  
  
  
