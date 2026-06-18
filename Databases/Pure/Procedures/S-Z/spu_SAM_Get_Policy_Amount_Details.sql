SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_SAM_Get_Policy_Amount_Details'
GO

CREATE  PROCEDURE spu_SAM_Get_Policy_Amount_Details  
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
 
 DECLARE @insurance_file_tax_UR money  
 DECLARE @risk_tax_UR money  
 DECLARE @fee_amount_UR money  
 DECLARE @this_premium_UR money  
 DECLARE @policy_tax_UR money  
 DECLARE @policy_fee_tax_UR money  
 DECLARE @risk_fee_tax_UR money  
 DECLARE @insurance_folder_cnt_UR int  
 DECLARE @product_is_true_monthly_policy_UR int  
 DECLARE @amount_to_be_put_on_next_instalment_UR money  
 DECLARE @policy_fee_amount_UR money  
 DECLARE @risk_fee_amount_UR money
 DECLARE @tax_total_UR money 
 DECLARE @premium_total_UR money 
 DECLARE @tax_rate_UR money
 DECLARE @insurance_file_status_id int
  

SELECT @insurance_file_status_id = insurance_file_status_id from insurance_file_status where code ='VOID'
  
SET @insurance_file_status_id = ISNULL(@insurance_file_status_id,0)

-- annual premium  
SELECT @this_premium = ROUND(this_premium,2), @this_premium_UR = this_premium FROM insurance_file WHERE insurance_file_cnt = @insurance_file_cnt  
  
-- policy level tax  
SELECT @insurance_file_tax = sum(value), @insurance_file_tax_UR = sum(value) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt and transtype ='TTIF'  
  
-- risk_tax  
SELECT  @risk_tax=ROUND(SUM(ISNULL(risk_taxes.totaltax,0)+ISNULL(levy_tax.totallevytax,0) + ISNULL(totalstampinsured,0)),2),  
		@risk_tax_UR = SUM((ISNULL(risk_taxes.totaltax,0)+ISNULL(levy_tax.totallevytax,0) + ISNULL(totalstampinsured,0)) )  
FROM    insurance_file 
INNER JOIN insurance_file_risk_link 
INNER JOIN Risk       
        ON Risk.risk_cnt = insurance_file_risk_link.risk_cnt  
	ON insurance_file_risk_link.insurance_file_cnt = insurance_file.insurance_file_cnt
		
LEFT JOIN  
       (SELECT  insurance_file_cnt,risk_cnt,sum(value) totaltax  
        FROM    tax_calculation  
        WHERE   transtype = 'TTR'  
        GROUP BY insurance_file_cnt,risk_cnt  
        ) risk_taxes   
        ON insurance_file_risk_link.risk_cnt = risk_taxes.risk_cnt AND insurance_file_risk_link.insurance_file_cnt = risk_taxes.insurance_file_cnt
LEFT JOIN  
       (SELECT  risk_cnt,sum(this_premium) totallevytax  
        FROM    Peril  
	WHERE is_levy_tax=1 AND is_premium=0 AND is_taxed IS NULL  
        GROUP BY risk_cnt  
        ) levy_tax 
	ON insurance_file_risk_link.risk_cnt = levy_tax.risk_cnt

LEFT JOIN  
 	(SELECT  risk_cnt,sum(this_premium) totalstampinsured  
         FROM    Peril p  
      	 JOIN peril_type pt ON p.peril_type_id = pt.peril_type_id where isnull(pt.is_stamp_duty_insured,0) = 1  
         GROUP BY p.risk_cnt  
         ) stampinsured               
	ON insurance_file_risk_link.risk_cnt = stampinsured.risk_cnt

WHERE   insurance_file.insurance_file_cnt = @insurance_file_cnt  
AND     ISNULL(insurance_file.insurance_file_status_id, 2) IN (1,2,3,4,5,6,309,@insurance_file_status_id)  
AND     insurance_file_risk_link.status_flag <> 'U'  
AND     Risk.is_risk_selected =1  
--ORDER BY rsk.risk_number  
--GROUP BY risk_cnt  
  
 -- policy fees tax  
 SELECT @policy_fee_tax = sum(value),@policy_fee_tax_UR = sum(value) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt and transtype ='TTF'  and risk_cnt IS NULL  
  
 -- risk fees tax  
 SELECT @risk_fee_tax = sum(ROUND(value,2)), @risk_fee_tax_UR = sum(value) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt and transtype ='TTF'  
 AND risk_cnt IN (SELECT risk_cnt 
                  FROM   risk 
                  WHERE  risk_cnt IN (SELECT risk_cnt 
                                      FROM   insurance_file_risk_link 
                                      WHERE  insurance_file_cnt = @insurance_file_cnt AND status_flag <> 'U') AND is_risk_selected =1)  
  
 -- policy_fees_u  - policy fees  
SELECT @policy_fee_amount = SUM(currency_amount),@policy_fee_amount_UR = SUM(currency_amount) FROM policy_fee_u WHERE insurance_file_cnt = @insurance_file_cnt   and risk_cnt IS NULL  
  
 -- policy_fees_u  - risk fees  
 SELECT @risk_fee_amount = SUM(ROUND(currency_amount,2)) , @risk_fee_amount_UR = SUM(currency_amount) FROM policy_fee_u  
 INNER JOIN risk ON  
  policy_fee_u.risk_cnt = risk.risk_cnt  
 WHERE insurance_file_cnt = @insurance_file_cnt  
  AND policy_fee_u.risk_cnt IS NOT NULL  
  AND risk.is_risk_selected = 1  
  


SET @tax_total_UR = ISNULL(@insurance_file_tax_UR,0) +  ISNULL(@risk_tax_UR,0)  + ISNULL(@policy_fee_tax_UR, 0) + ISNULL(@risk_fee_tax_UR, 0) 
SET @premium_total_UR  = ISNULL(@this_premium_UR,0) +  ISNULL(@insurance_file_tax_UR,0) +  ISNULL(@risk_tax_UR,0)+  ISNULL(@policy_fee_amount_UR,0) +  ISNULL(@risk_fee_amount_UR,0) +  ISNULL(@policy_fee_tax_UR, 0) +  ISNULL(@risk_fee_tax_UR, 0)

If ISNULL(@premium_total_UR,0) = 0
	SET @tax_rate_UR = 0
ELSE
	SET @tax_rate_UR = @tax_total_UR / @premium_total_UR



-- return the premium and the details of the items included in the total amount

SELECT ISNULL(@this_premium,0) +  
ISNULL(@insurance_file_tax,0) +  
ISNULL(@risk_tax,0)+  
ISNULL(@policy_fee_amount,0) +  
ISNULL(@risk_fee_amount,0) +  
ISNULL(@policy_fee_tax, 0) +  
ISNULL(@risk_fee_tax, 0) TOTAL_PREMIUM_AMOUNT,  
ISNULL(@this_premium,0) as this_premium,  
ISNULL(@insurance_file_tax,0) as insurance_file_tax,  
ISNULL(@risk_tax,0) as risk_tax,  
ISNULL(@policy_fee_amount,0)  as policy_fee_amount,  
ISNULL(@risk_fee_amount,0)  as risk_fee_amount,  
ISNULL(@policy_fee_tax, 0) as fee_tax,  
ISNULL(@risk_fee_tax, 0) as risk_fee_tax,
ISNULL(@tax_rate_UR, 0) as tax_rate_UR

END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

