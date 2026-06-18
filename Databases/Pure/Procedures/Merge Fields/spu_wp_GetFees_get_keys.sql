SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_wp_GetFees_get_keys'
GO

CREATE PROCEDURE spu_wp_GetFees_get_keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SET NOCOUNT ON

CREATE TABLE #TempWPGetFees(
    policy_fee_u_id int,
    product_id int,
    peril_group_id int,
    risk_type_group_id int
)

INSERT INTO #TempWPGetFees

SELECT
  pfu.policy_fee_u_id,
  pfu.product_id,
  pfu.peril_group_id,
  pfu.risk_type_group_id
FROM policy_fee_u pfu
INNER JOIN party p ON pfu.party_cnt = p.party_cnt
LEFT JOIN product pr ON pfu.product_id = pr.product_id
LEFT JOIN risk_type_group rtg ON pfu.risk_type_group_id = rtg.risk_type_group_id
LEFT JOIN peril_group pg ON pfu.peril_group_id = pg.peril_group_id
LEFT JOIN tax_group tg ON pfu.tax_group_id = tg.tax_group_id
INNER JOIN transaction_type tt ON pfu.transaction_type_id = tt.transaction_type_id
LEFT JOIN Currency c ON pfu.fee_rate_currency_id = c.currency_id
WHERE insurance_file_cnt = @InsuranceFileCnt AND risk_cnt IS NULL

UNION

-- Retrieve Risk\Peril Level Fees
 SELECT
  pfu.policy_fee_u_id,
  pfu.product_id,
  pfu.peril_group_id,
  pfu.risk_type_group_id
 FROM policy_fee_u pfu
 INNER JOIN party p ON pfu.party_cnt = p.party_cnt
 LEFT JOIN product pr ON pfu.product_id = pr.product_id
 LEFT JOIN risk_type_group rtg ON pfu.risk_type_group_id = rtg.risk_type_group_id
 LEFT JOIN peril_group pg ON pfu.peril_group_id = pg.peril_group_id
 LEFT JOIN tax_group tg ON pfu.tax_group_id = tg.tax_group_id
 INNER JOIN transaction_type tt ON pfu.transaction_type_id = tt.transaction_type_id
 LEFT JOIN Currency c ON pfu.fee_rate_currency_id = c.currency_id
 WHERE insurance_file_cnt = @InsuranceFileCnt AND risk_cnt in 
	(select risk_cnt from policy_fee_u 
	 where insurance_file_cnt = @InsuranceFileCnt AND risk_cnt is not null)
 ORDER BY pfu.product_id desc, pfu.risk_type_group_id desc, pfu.peril_group_id desc

-- Only Retrive the policy fee id
SELECT policy_fee_u_id FROM #TempWPGetFees

DROP TABLE #TempWPGetFees