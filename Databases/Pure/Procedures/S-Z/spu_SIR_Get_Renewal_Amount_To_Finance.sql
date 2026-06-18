SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Get_Renewal_Amount_To_Finance'
GO

CREATE PROCEDURE spu_SIR_Get_Renewal_Amount_To_Finance
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
 DECLARE @levy_tax money 

  -- get any mta to be put no next renewal instalment
  EXEC spu_SIR_Get_MTA_Amount_To_Put_On_Next_Renewal @insurance_file_cnt, @amount_to_be_put_on_next_instalment OUTPUT

  -- ensure that this is a valid amount
  SET @amount_to_be_put_on_next_instalment = ISNULL(@amount_to_be_put_on_next_instalment,0)

 -- annual premium  
 Select @this_premium = sum(this_premium) from rating_section where risk_cnt in (select risk_cnt from insurance_file_risk_link with (nolock) where insurance_file_cnt = @insurance_file_cnt)
  
 -- insurance_file_tax  
 SELECT @insurance_file_tax = sum(value) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt and transtype ='TTIF' And include_tax_in_instalments=1 
  
 -- risk_tax  
/* SELECT @risk_tax = SUM(value) FROM tax_calculation WHERE risk_cnt IN (SELECT risk_cnt FROM risk WHERE risk_cnt IN (SELECT risk_cnt FROM insurance_file_risk_link WHERE insurance_file_cnt = @insurance_file_cnt)  
 and is_risk_selected =1) and transtype ='TTR'  
*/

SELECT @risk_tax = SUM(value) FROM tax_calculation ,Tax_BAND TB,Tax_Type TT WHERE risk_cnt IN (SELECT risk_cnt FROM risk WHERE risk_cnt IN (SELECT risk_cnt FROM insurance_file_risk_link WHERE insurance_file_cnt = @insurance_file_cnt)
 and is_risk_selected =1) and transtype ='TTR' and TB.Tax_Band_ID = Tax_Calculation.Tax_Band_ID and TB.Tax_Type_Id = TT.Tax_Type_ID and TT.is_Include_Tax_in_Instalments = 1
  
 -- policy fees tax  
 SELECT @policy_fee_tax = sum(value) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt and transtype ='TTF'  and risk_cnt IS NULL and include_tax_in_instalments = 1
  
 -- risk fees tax  
 SELECT @risk_fee_tax = sum(value) FROM tax_calculation WHERE insurance_file_cnt = @insurance_file_cnt and transtype ='TTF'  
 AND risk_cnt IN (SELECT risk_cnt FROM risk WHERE risk_cnt IN (SELECT risk_cnt FROM insurance_file_risk_link WHERE insurance_file_cnt = @insurance_file_cnt))  
  
 -- policy_fees_u  - policy fees  
 SELECT @fee_amount = SUM(currency_amount) FROM policy_fee_u WHERE insurance_file_cnt = @insurance_file_cnt  
  
 SELECT @levy_tax = SUM(this_premium) 
    FROM Peril P 
    INNER JOIN risk r on r.risk_cnt = p.risk_cnt
    INNER JOIN insurance_file_risk_link ifrl on ifrl.risk_cnt = p.risk_cnt
      WHERE ifrl.insurance_file_cnt = @insurance_file_cnt 
		AND ISNULL(r.is_risk_selected, 0) = 1 AND ifrl.status_flag = 'C'
		AND p.is_levy_tax = 1 AND p.is_premium = 0 AND ISNULL(p.is_taxed, 0) = 0
                GROUP BY ifrl.insurance_file_cnt

 SELECT ISNULL(@this_premium,0) + ISNULL(@insurance_file_tax,0) + ISNULL(@risk_tax,0) + ISNULL(@fee_amount,0) + ISNULL(@policy_fee_tax, 0) +  ISNULL(@risk_fee_tax, 0) + @amount_to_be_put_on_next_instalment + ISNULL(@levy_tax,0) as amount_to_finance,  
 ISNULL(@this_premium,0) as this_premium,  
 ISNULL(@insurance_file_tax,0) as insurance_file_tax,  
 ISNULL(@risk_tax,0) as risk_tax,  
 ISNULL(@fee_amount,0)  as fee_amount,  
 ISNULL(@policy_fee_tax, 0) as fee_tax,
 ISNULL(@amount_to_be_put_on_next_instalment,0) as amount_to_be_put_on_next_instalment,
 ISNULL(@levy_tax, 0) as levy_tax
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
