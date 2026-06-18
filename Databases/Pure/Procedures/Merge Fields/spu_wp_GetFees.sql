SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_wp_GetFees'
GO

CREATE PROCEDURE spu_wp_GetFees
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

-- Retrieve Policy Level Fees

SELECT
  pfu.policy_fee_u_id,
  p.resolved_name,
  pfu.product_id,
  pr.description ItemDescription,
  pfu.transaction_type_id,
  tt.description TransactionTypeDescription,
  NULL, -- fee amount id,
  pfu.party_cnt,
  pfu.fee_rate_percentage,
  pfu.fee_rate_amount,
  NULL, -- fa.effective_date,
  pfu.tax_group_id,
  pfu.is_fee_applied_to_cr,
  pfu.currency_id,
  c.description CurrencyDescription,
  pfu.peril_group_id,
  pg.description peril_group_description,
  pfu.risk_type_group_id,
  rtg.description risk_type_group_description,
  c.iso_code,
  branch_id,
  currency_amount,
  currency_tax_amount,
  tg.description tax_group_description,
  pfu.policy_fee_u_id,
  pfu.fee_premium,
  isnull(currency_amount,0) + isnull(currency_tax_amount,0) TotalAmount
FROM policy_fee_u pfu
INNER JOIN party p ON pfu.party_cnt = p.party_cnt
LEFT JOIN product pr ON pfu.product_id = pr.product_id
LEFT JOIN risk_type_group rtg ON pfu.risk_type_group_id = rtg.risk_type_group_id
LEFT JOIN peril_group pg ON pfu.peril_group_id = pg.peril_group_id
LEFT JOIN tax_group tg ON pfu.tax_group_id = tg.tax_group_id
INNER JOIN transaction_type tt ON pfu.transaction_type_id = tt.transaction_type_id
LEFT JOIN Currency c ON pfu.fee_rate_currency_id = c.currency_id
WHERE insurance_file_cnt = @InsuranceFileCnt AND risk_cnt IS NULL AND pfu.policy_fee_u_id = @Instance2

UNION

-- Retrieve Risk\Peril Level Fees
SELECT
  pfu.policy_fee_u_id,
  p.resolved_name,
  pfu.product_id,
  isnull(pg.description,'') + isnull(rtg.description,'') ItemDescription,
  pfu.transaction_type_id,
  tt.description TransactionTypeDescription,
  NULL, -- fee amount id,
  pfu.party_cnt,
  pfu.fee_rate_percentage,
  pfu.fee_rate_amount,
  NULL, -- fa.effective_date,
  pfu.tax_group_id,
  pfu.is_fee_applied_to_cr,
  pfu.currency_id,
  c.description CurrencyDescription, 
  pfu.peril_group_id,
  null,
  pfu.risk_type_group_id,
  null,
  c.iso_code,
  branch_id,
  currency_amount,
  currency_tax_amount,
  tg.description as tax_group_description,
  pfu.policy_fee_u_id,
  pfu.fee_premium,
  isnull(currency_amount,0) + isnull(currency_tax_amount,0) TotalAmount
 FROM policy_fee_u pfu
 INNER JOIN party p ON pfu.party_cnt = p.party_cnt
 LEFT JOIN product pr ON pfu.product_id = pr.product_id
 LEFT JOIN risk_type_group rtg ON pfu.risk_type_group_id = rtg.risk_type_group_id
 LEFT JOIN peril_group pg ON pfu.peril_group_id = pg.peril_group_id
 LEFT JOIN tax_group tg ON pfu.tax_group_id = tg.tax_group_id
 INNER JOIN transaction_type tt ON pfu.transaction_type_id = tt.transaction_type_id
 LEFT JOIN Currency c ON pfu.fee_rate_currency_id = c.currency_id
 WHERE insurance_file_cnt = @InsuranceFileCnt 
 AND risk_cnt in (select risk_cnt from policy_fee_u 
      	          where insurance_file_cnt = @InsuranceFileCnt AND risk_cnt is not null)
 AND pfu.policy_fee_u_id = @Instance2

 order by pfu.product_id desc, pfu.risk_type_group_id desc, pfu.peril_group_id desc