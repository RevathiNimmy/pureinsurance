SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_SIR_Get_BackdatedPolicyVersions'
GO

CREATE PROCEDURE spu_SIR_Get_BackdatedPolicyVersions 
 @nInsurance_file_cnt INT  
AS  
BEGIN  
  
-- edited risks  
WITH Risk_Folder_CTE AS (  
 SELECT risk_folder_cnt  
  FROM Risk r  
  INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = r.risk_cnt  
  WHERE ISNULL(ifrl.is_risk_edited, 0) = 1 AND status_flag <> 'D' AND ifrl.insurance_file_cnt = @nInsurance_file_cnt  
 )  
  
SELECT reapplied.insurance_file_cnt 'InsuranceFileCnt',  
  ISNULL(ift.description, 'MTA Quotation Cancellation') 'PolicyType',  
  ISNULL(reapplied.cover_start_date, reversed.cover_start_date) 'CoverStartDate',  
  ISNULL(reapplied.expiry_date, reversed.expiry_date) 'CoverEndDate',  
  CASE reapplied.insurance_file_cnt  
   WHEN @nInsurance_file_cnt THEN ISNULL(reapplied.this_premium, 0) + ISNULL(reapplied.tax_amount, 0)  + 
   (select ISNULL(SUM(p.this_premium), 0) FROM    peril p Join Peril_Type pt ON p.Peril_Type_id= pt.Peril_Type_id
 join  insurance_file_risk_link ifrl ON ifrl.risk_cnt = p.risk_cnt WHERE   
 ifrl.insurance_file_cnt = reapplied.insurance_file_cnt AND  isnull(pt.is_stamp_duty_insured,0) = 1)
    ELSE  
     CASE ISNULL(reapplied.risk_processed, 0)  
       WHEN 0 THEN 0  
     ELSE  
      ISNULL(reapplied.this_premium, 0) + ISNULL(reapplied.tax_amount, 0)  + 
   (select ISNULL(SUM(p.this_premium), 0) FROM    peril p Join Peril_Type pt ON p.Peril_Type_id= pt.Peril_Type_id
 join  insurance_file_risk_link ifrl ON ifrl.risk_cnt = p.risk_cnt WHERE   
 ifrl.insurance_file_cnt = reapplied.insurance_file_cnt AND  isnull(pt.is_stamp_duty_insured,0) = 1) END END 'MTAPremium',  
  CASE reapplied.insurance_file_cnt  
   WHEN @nInsurance_file_cnt THEN ISNULL(reversed.this_premium, 0) + ISNULL(reversed.tax_amount, 0)   +  
   (select ISNULL(SUM(p.this_premium), 0) FROM    peril p Join Peril_Type pt ON p.Peril_Type_id= pt.Peril_Type_id  
 join  insurance_file_risk_link ifrl ON ifrl.risk_cnt = p.risk_cnt WHERE  
 ifrl.insurance_file_cnt =  reversed.insurance_file_cnt AND  isnull(pt.is_stamp_duty_insured,0) = 1)
    ELSE  
     CASE ISNULL(reapplied.risk_processed, 0)  
      WHEN 0 THEN 0  
     ELSE  
      ISNULL(reversed.this_premium, 0) + ISNULL(reversed.tax_amount, 0)  +  
   (select ISNULL(SUM(p.this_premium), 0) FROM    peril p Join Peril_Type pt ON p.Peril_Type_id= pt.Peril_Type_id  
 join  insurance_file_risk_link ifrl ON ifrl.risk_cnt = p.risk_cnt WHERE  
 ifrl.insurance_file_cnt =  reversed.insurance_file_cnt AND  isnull(pt.is_stamp_duty_insured,0) = 1) END END 'OriginalMTAPremium',  
  ISNULL(IFS.description,'Live') 'PolicyStatus',  
  reversed.insurance_file_cnt 'ReversedInsuranceFileCnt',  
  CASE WHEN reapplied.insurance_file_cnt IS NULL THEN 'Quoted'  
  ELSE  
   CASE WHEN EXISTS(SELECT NULL FROM risk r  
       Inner Join insurance_file_risk_link ifrl ON r.risk_cnt = ifrl.risk_cnt  
       Inner Join Risk_Folder_CTE re ON re.risk_folder_cnt = r.risk_folder_cnt  
       Inner Join insurance_file ON insurance_file.insurance_file_cnt = ifrl.insurance_file_cnt  
     WHERE ifrl.insurance_file_cnt = reapplied.insurance_file_cnt  
     AND ISNULL(mifl.new_linked_insurance_file_cnt, 0) <> 0 AND ISNULL(mifl.cancelled_linked_insurance_file_cnt, 0) <> 0 -- Leave it for automta to decide when to quote or not as these will either be MTC or MTR  
     And (r.risk_status_id <> 3 OR ifrl.status_flag NOT IN ('C', 'D'))) -- atleast one unquoted  
     OR (ISNULL(reapplied.risk_processed, 0) = 0 AND reapplied.insurance_file_cnt <> @nInsurance_file_cnt)  
   THEN 'Unquoted' ELSE 'Quoted' END  
  END 'QuoteStatus',  
  CASE reapplied.insurance_file_cnt  
   WHEN @nInsurance_file_cnt THEN (SELECT Sum(ISNULL(commission_value, 0) + ISNULL(tax_amount, 0)) FROM Agent_Commission ac WHERE ac.insurance_file_cnt = reversed.insurance_file_cnt)  
    ELSE  
     CASE ISNULL(reapplied.risk_processed, 0)  
      WHEN 0 THEN 0  
     ELSE  
      (SELECT Sum(ISNULL(commission_value, 0) + ISNULL(tax_amount, 0)) From Agent_Commission ac WHERE ac.insurance_file_cnt = reversed.insurance_file_cnt) END END 'OriginalCommission',  
  CASE reapplied.insurance_file_cnt  
   WHEN @nInsurance_file_cnt THEN (SELECT Sum(ISNULL(commission_value, 0) + ISNULL(tax_amount, 0)) FROM Agent_Commission ac WHERE ac.insurance_file_cnt = reapplied.insurance_file_cnt)  
    ELSE  
     CASE ISNULL(reapplied.risk_processed, 0)  
      WHEN 0 THEN 0  
     ELSE  
      (SELECT Sum(ISNULL(commission_value, 0) + ISNULL(tax_amount, 0)) FROM Agent_Commission ac Where ac.insurance_file_cnt = reapplied.insurance_file_cnt) END END 'MTACommission',  
  CASE reapplied.insurance_file_cnt  
   WHEN @nInsurance_file_cnt THEN (SELECT Sum(ISNULL(currency_amount, 0) + ISNULL(currency_tax_amount, 0)) FROM policy_fee_u pfu WHERE pfu.insurance_file_cnt = reversed.insurance_file_cnt)  
    ELSE  
     CASE ISNULL(reapplied.risk_processed, 0)  
      WHEN 0 THEN 0  
     ELSE  
      (SELECT Sum(ISNULL(currency_amount, 0) + ISNULL(currency_tax_amount, 0)) FROM policy_fee_u pfu WHERE pfu.insurance_file_cnt = reversed.insurance_file_cnt) END END 'OriginalFee',  
  CASE reapplied.insurance_file_cnt  
   WHEN @nInsurance_file_cnt THEN (SELECT Sum(ISNULL(currency_amount, 0) + ISNULL(currency_tax_amount, 0)) FROM policy_fee_u pfu WHERE pfu.insurance_file_cnt = reapplied.insurance_file_cnt)  
    ELSE  
     CASE ISNULL(reapplied.risk_processed, 0)  
      WHEN 0 THEN 0  
     ELSE  
      (SELECT Sum(ISNULL(currency_amount, 0) + ISNULL(currency_tax_amount, 0)) FROM policy_fee_u pfu WHERE pfu.insurance_file_cnt = reapplied.insurance_file_cnt) END END 'MTAFee'  
FROM mta_insurance_file_link mifl  
 Left Join insurance_file reversed ON reversed.Insurance_File_Cnt = mifl.cancelled_linked_insurance_file_cnt AND ISNULL(mifl.new_linked_insurance_file_cnt, 0) <> 0  
 Left Join insurance_file reapplied ON reapplied.Insurance_File_Cnt = mifl.new_linked_insurance_file_cnt  
           Or (reapplied.Insurance_File_Cnt = mifl.cancelled_linked_insurance_file_cnt AND ISNULL(mifl.new_linked_insurance_file_cnt, 0) = 0)  
           Or (reapplied.Insurance_File_Cnt = mifl.insurance_file_cnt AND ISNULL(mifl.new_linked_insurance_file_cnt, 0) = 0 AND ISNULL(mifl.cancelled_linked_insurance_file_cnt, 0) = 0)  
 Left Join insurance_file_type IFT ON reapplied.insurance_file_type_id = IFT.insurance_file_type_id  
 Left Join insurance_file_Status IFS ON reapplied.insurance_file_status_id = IFS.insurance_file_status_id  
WHERE mifl.insurance_file_cnt = @nInsurance_file_cnt  
ORDER BY reapplied.insurance_file_cnt  
END
