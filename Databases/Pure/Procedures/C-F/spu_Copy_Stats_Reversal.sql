SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDROPPROCEDURE spu_Copy_Stats_Reversal  
GO
CREATE PROCEDURE spu_Copy_Stats_Reversal  
@nInsuranceFileCnt  INT,  
@nStatsFolderCnt INT  
AS  
DECLARE @nOldStatsFolderCnt INT  

SELECT @nOldStatsFolderCnt=Stats_Folder_cnt from Stats_Folder WHERE insurance_file_cnt=@nInsuranceFileCnt  
and Stats_Folder_cnt NOT IN (@nStatsFolderCnt) and loss_id is null and transaction_type_code NOT IN ('DRI','DRIC') 

Insert Into Stats_Detail (stats_folder_cnt,
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
Claim_RI_Only_Amendment,
Earning_Pattern_id,
ri_arrangement_line_Id) 
Select @nStatsFolderCnt,stats_detail_id,stats_detail_type,risk_id,risk_type_id,risk_type_code,  
  peril_id,peril_description,peril_type_id,peril_type_code,policy_section_type_id,policy_section_type_code,  
  class_of_business_id,class_of_business_code,tax_type_id,tax_type_code,-tax_value,ri_party_cnt,ri_shortname,  
  ri_party_type,ri_share_percent,ri_agreement_code,-annual_premium,currency_code,currency_rate,  
  -this_premium_original,-this_premium_home,commission_percent,-lead_commission_value_home,-sub_commission_value_home,  
  -sum_insured_home,sum_insured_currency_code,-sum_insured_change,transaction_ledger_id,transaction_account_id,account_type_code,ceded_ref,  
  cover_share_percent,-sum_insured_total,-charges_total,-taxes_total,-recoveries_total,commission_excluded,withholding_tax_excluded,purchase_order_no,  
  purchase_invoice_no,stats_version,-this_premium_system,-lead_commission_value_system,-sub_commission_value_system,-sum_insured_system,is_commission_modified,  
  original_flag,cover_to_date,Claim_RI_Only_Amendment,Earning_Pattern_id,ri_arrangement_line_Id  
 from Stats_Detail  WHERE Stats_Folder_cnt=@nOldStatsFolderCnt 
 GO
