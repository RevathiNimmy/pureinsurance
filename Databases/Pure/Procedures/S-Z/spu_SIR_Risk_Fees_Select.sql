Execute DDLDropProcedure 'spu_SIR_Risk_Fees_Select'
GO

CREATE PROCEDURE spu_SIR_Risk_Fees_Select  
  
 @risk_cnt int,  
 @insurance_file_cnt int  
  
AS  
  
BEGIN  
  
 SELECT  
  p.resolved_name,  
  pfu.product_id,  
  pr.description 'ProductDesc',  
  pfu.transaction_type_id,  
  tt.description,  
  NULL, -- fee amount id,  
  pfu.party_cnt,  
  pfu.fee_rate_percentage,  
  pfu.fee_rate_amount,  
  NULL, -- fa.effective_date,  
  pfu.tax_group_id,  
  pfu.is_fee_applied_to_cr,  
  pfu.currency_id,  
  c.description,  
  pfu.peril_group_id,  
  pg.description as peril_group_description,  
  pfu.risk_type_group_id,  
  rtg.description as risk_type_group_description,  
  c.iso_code,  
  branch_id,  
  currency_amount,  
  currency_tax_amount,  
  tg.description as tax_group_description,  
  pfu.policy_fee_u_id,  
  pfu.fee_premium,
  pfu.include_fee_in_instalments,
  pfu.spread_fee_across_instalments,
  tran_c.iso_code,
  pfu.FeeTypePercent ,
  ISNULL(pfu.Calculation_Basis,0) 'Calculation_Basis',
  ISNULL(pfu.DoPaymentTerms_id,0) 'DoPaymentTerms_id',
  ISNULL(pfu.MakeLiveOptions_id,0) 'MakeLiveOptions_id',
  ISNULL(pfu.Is_Prorated,0) 'Is_Prorated',
  ISNULL(pfu.Pro_rata_rate,0) 'Pro_rata_rate',
  ISNULL(pfu.is_override,0) 'is_override' 
  
 FROM policy_fee_u pfu  
  
  INNER JOIN party p ON  
   pfu.party_cnt = p.party_cnt  
  
    LEFT JOIN product pr ON  
    pfu.product_id = pr.product_id  
  
    LEFT JOIN risk_type_group rtg ON  
    pfu.risk_type_group_id = rtg.risk_type_group_id  
  
    LEFT JOIN peril_group pg ON  
    pfu.peril_group_id = pg.peril_group_id  
  
    LEFT JOIN tax_group tg ON  
    pfu.tax_group_id = tg.tax_group_id  
  
    LEFT JOIN transaction_type tt ON  
    pfu.transaction_type_id = tt.transaction_type_id  
  
    LEFT JOIN Currency c ON  
    pfu.fee_rate_currency_id = c.currency_id  

    INNER JOIN Currency tran_c ON   
    pfu.currency_id = tran_c.currency_id
  
 WHERE insurance_file_cnt = @insurance_file_cnt  
 AND risk_cnt = @risk_cnt  
  
END  
GO
