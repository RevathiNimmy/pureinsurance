SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Total_Premium_Amount'
GO


CREATE PROCEDURE spu_SAM_Get_Total_Premium_Amount

@insurance_file_cnt int    
    
AS    
    
BEGIN    
    
 DECLARE @insurance_file_tax money
 DECLARE @risk_tax money
 DECLARE @fee_amount money
 DECLARE @this_premium money
 DECLARE @policy_tax money
 DECLARE @policy_fee_tax money
 DECLARE @risk_fee_tax money
 DECLARE @insurance_folder_cnt int
 DECLARE @product_is_true_monthly_policy int
 DECLARE @amount_to_be_put_on_next_instalment money
 DECLARE @policy_fee_amount money
 DECLARE @risk_fee_amount money
 DECLARE @commission_value money
 DECLARE @total_stamp_insured money
 DECLARE @levy_tax  money  
 DECLARE @totalTaxNotAppliedToClient money

 -- annual premium
 SELECT @this_premium = this_premium FROM insurance_file WHERE insurance_file_cnt = @insurance_file_cnt

 -- policy level tax
  SELECT @insurance_file_tax = sum(value) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt and transtype ='TTIF' and is_not_applied_to_client<>1
  SELECT @totalTaxNotAppliedToClient = sum(value) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt and transtype ='TTIF' and is_not_applied_to_client = 1

 -- risk_tax
 SELECT @risk_tax = SUM(value) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt AND risk_cnt IN (SELECT risk_cnt FROM risk WHERE risk_cnt IN (SELECT risk_cnt FROM insurance_file_risk_link WHERE insurance_file_cnt = @insurance_file_cnt)
 and is_risk_selected =1) and transtype ='TTR'  and is_not_applied_to_client<>1

 SELECT @total_stamp_insured=SUM(this_premium)   
                FROM    Peril p  
                JOIN   peril_type pt  
                      ON p.peril_type_id = pt.peril_type_id AND ISNULL(pt.is_stamp_duty_insured,0) = 1  
                INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = p.risk_cnt 
				WHERE ifrl.insurance_file_cnt = @insurance_file_cnt

-- levy_tax  
 SELECT    @levy_tax = SUM(this_premium)  FROM  Peril WHERE is_levy_tax=1 AND is_premium=0 AND is_taxed IS NULL AND  risk_cnt IN
	(SELECT risk_cnt FROM risk WHERE risk_cnt IN (SELECT risk_cnt FROM insurance_file_risk_link WHERE insurance_file_cnt = @insurance_file_cnt)  and is_risk_selected =1) 
 
 -- policy fees tax
 SELECT @policy_fee_tax = sum(value) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt and transtype ='TTF'  and risk_cnt IS NULL  and is_not_applied_to_client<>1

 -- risk fees tax
 SELECT @risk_fee_tax = sum(value) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt and transtype ='TTF'
 AND risk_cnt IN (SELECT risk_cnt FROM risk WHERE risk_cnt IN (SELECT risk_cnt FROM insurance_file_risk_link WHERE insurance_file_cnt = @insurance_file_cnt) and is_risk_selected =1) and is_not_applied_to_client<>1


 -- policy_fees_u  - policy fees
SELECT @policy_fee_amount = SUM(currency_amount) FROM policy_fee_u WHERE insurance_file_cnt = @insurance_file_cnt   and risk_cnt IS NULL

 -- policy_fees_u  - risk fees
 SELECT @risk_fee_amount = SUM(currency_amount) FROM policy_fee_u
 INNER JOIN risk ON
  policy_fee_u.risk_cnt = risk.risk_cnt
 WHERE insurance_file_cnt = @insurance_file_cnt
 AND policy_fee_u.risk_cnt IS NOT NULL
 AND risk.is_risk_selected = 1

 SELECT @commission_value = SUM(commission_value) FROM agent_commission WHERE insurance_file_cnt = @insurance_file_cnt 

 -- return the premium and the details of the items included in the total amount
 SELECT Round(ISNULL(@this_premium,0) +
 ISNULL(@insurance_file_tax,0) +
 ISNULL(@risk_tax,0)+
 ISNULL(@policy_fee_amount,0) +
 ISNULL(@risk_fee_amount,0) +
 ISNULL(@policy_fee_tax, 0) +
 ISNULL(@risk_fee_tax, 0) + 
 ISNULL(@total_stamp_insured,0) +
 ISNULL(@levy_tax, 0),4) TOTAL_PREMIUM_AMOUNT,  
 Round(ISNULL(@this_premium,0),4) as this_premium,
 ISNULL(@insurance_file_tax,0) as insurance_file_tax,
 Round(ISNULL(@risk_tax,0) + ISNULL(@total_stamp_insured,0),4) as risk_tax,
 ISNULL(@policy_fee_amount,0)  as policy_fee_amount,
 Round(ISNULL(@risk_fee_amount,0),4)  as risk_fee_amount,
 ISNULL(@policy_fee_tax, 0) as fee_tax,
 ISNULL(@commission_value, 0) as commission_value,
 ROUND(ISNULL(@totalTaxNotAppliedToClient,0),4) as totalTaxNotAppliedToClient  
    
END    




GO
