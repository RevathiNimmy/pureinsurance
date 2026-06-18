SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

Execute DDLDropProcedure 'spu_CLM_Find_Claim'
GO

CREATE PROCEDURE spu_CLM_Find_Claim

@shortname varchar(30) = NULL,
@insurance_ref varchar(30) = NULL,
@claim_id int = NULL,
@ViaClaimVersionList tinyint = 0,
@TPAID varchar (30) =NULL 

AS

BEGIN          
  
IF EXISTS (SELECT NULL FROM Hidden_Options WHERE option_number = 1 AND value = 'A')

 SELECT claim.claim_id,
 claim.claim_status_id,
 claim.info_only,
 claim.policy_number,
 claim.description,
 claim.claim_number,
 insurance_file.risk_code_id,
 risk_code.description
 FROM claim
  JOIN insurance_file WITH (NOLOCK) ON claim.policy_id=insurance_file.insurance_file_cnt
  JOIN risk_code WITH (NOLOCK) ON insurance_file.risk_code_id=risk_code.risk_code_id
 INNER JOIN (SELECT MAX(claim_id)as claim_id, MAX(version_id) as version_id, base_claim_id
      FROM claim WITH (NOLOCK)
      WHERE ((@shortname IS NULL)  OR (claim.client_short_name  = @shortname))
		AND ((@insurance_ref IS NULL) OR (claim.policy_number =@insurance_ref))
		AND ((@claim_id IS NULL) OR (claim.claim_id = @claim_Id))
		AND is_dirty = 0
      GROUP by base_claim_id) claim_versions ON
   claim.claim_id = claim_versions.claim_id

 WHERE ((@shortname IS NULL)  OR (claim.client_short_name  = @shortname))
 AND ((@insurance_ref IS NULL) OR (claim.policy_number =@insurance_ref))
 AND ((@claim_id IS NULL) OR (claim.claim_id = @claim_Id))
 AND ((@TPAID IS NULL) OR (claim.other_party_id=@TPAID))
 ORDER BY claim_number

ELSE

IF @ViaClaimVersionList = 1
BEGIN

CREATE TABLE #temp1(claim_id int, version_id int, base_claim_id int)
INSERT INTO #temp1 (claim_id, version_id, base_claim_id)
SELECT MAX(claim_id), MAX(version_id), base_claim_id FROM Claim WITH (nolock)
WHERE is_dirty = 0
GROUP BY base_claim_id

 SELECT claim.claim_id,
 claim.claim_status_id,
 claim.info_only,
 claim.policy_number,
 claim.description,
 claim.claim_number,
 insurance_file.product_id,
 product.description,
 claim.loss_from_date,
 claim_status.description 'status',
    0 'total_indemnity',
    0 'total_expense',
    0 'total_excess',
    (SELECT case_number FROM [case] WHERE case_id= claim.base_case_id) 'case_number',
    insurance_file.insurance_file_cnt
 FROM claim WITH (NOLOCK)
  JOIN insurance_file WITH (NOLOCK) ON claim.policy_id=insurance_file.insurance_file_cnt
  JOIN product WITH (NOLOCK) ON insurance_file.product_id=product.product_id
  JOIN claim_status WITH (NOLOCK) ON claim.claim_status_id = claim_status.claim_status_id

INNER JOIN #temp1 WITH (nolock) ON #temp1.claim_id = claim.claim_id

 WHERE ((@shortname IS NULL)  OR (claim.client_short_name  = @shortname))
 AND ((@insurance_ref IS NULL) OR (claim.policy_number =@insurance_ref))
 AND ((@claim_id IS NULL) OR (claim.claim_id = @claim_Id))
 AND ((@TPAID IS NULL) OR (claim.other_party_id=@TPAID))
 ORDER BY claim_number

DROP TABLE #temp1
END

ELSE
BEGIN

 SELECT claim.claim_id,
 claim.claim_status_id,
 claim.info_only,
 claim.policy_number,
 claim.description,
 claim.claim_number,
 insurance_file.product_id,
 product.description,
 claim.loss_from_date,
 claim_status.description 'status',
    ISNULL((SELECT SUM((R.initial_reserve + R.revised_reserve) - paid_to_date ) FROM claim_peril CP WITH (NOLOCK)
    JOIN reserve R WITH (NOLOCK)
    ON CP.claim_peril_id = R.claim_peril_id
    JOIN reserve_type RT WITH (NOLOCK)
    ON R.reserve_type_id = RT.reserve_type_id
    AND RT.is_indemnity=1
    WHERE CP.claim_id=claim.claim_id),0)
    'total_indemnity',

    ISNULL((SELECT
    SUM((R.initial_reserve + R.revised_reserve) - paid_to_date )
    FROM claim_peril CP WITH (NOLOCK)
    JOIN reserve R WITH (NOLOCK)
    ON CP.claim_peril_id = R.claim_peril_id
    JOIN reserve_type RT WITH (NOLOCK)
    ON R.reserve_type_id = RT.reserve_type_id
    AND RT.is_expense=1
    WHERE
    CP.claim_id=claim.claim_id),0)
    'total_expense',

    ISNULL((SELECT
    SUM((R.initial_reserve + R.revised_reserve) - paid_to_date )
    FROM claim_peril CP WITH (NOLOCK)
    JOIN reserve R WITH (NOLOCK)
        ON CP.claim_peril_id = R.claim_peril_id
    JOIN reserve_type RT WITH (NOLOCK)
        ON R.reserve_type_id = RT.reserve_type_id
        AND RT.is_excess=1
    WHERE
        CP.claim_id=claim.claim_id),0)
    'total_excess',
    (SELECT case_number FROM [case] WITH (NOLOCK) WHERE case_id= claim.base_case_id) 'case_number',
    insurance_file.insurance_file_cnt
 FROM claim WITH (NOLOCK)
  JOIN insurance_file WITH (NOLOCK) ON claim.policy_id=insurance_file.insurance_file_cnt
  JOIN product WITH (NOLOCK) ON insurance_file.product_id=product.product_id
  JOIN claim_status WITH (NOLOCK) ON claim.claim_status_id = claim_status.claim_status_id
 INNER JOIN (SELECT MAX(claim_id)as claim_id, MAX(version_id) as version_id, base_claim_id
      FROM claim WITH (NOLOCK)
      WHERE ((@shortname IS NULL)  OR (claim.client_short_name  = @shortname))
		AND ((@insurance_ref IS NULL) OR (claim.policy_number =@insurance_ref))
		AND ((@claim_id IS NULL) OR (claim.claim_id = @claim_Id))
		AND is_dirty = 0
      GROUP by base_claim_id) claim_versions ON
   claim.claim_id = claim_versions.claim_id

 WHERE ((@shortname IS NULL)  OR (claim.client_short_name  = @shortname))
 AND ((@insurance_ref IS NULL) OR (claim.policy_number =@insurance_ref))
 AND ((@claim_id IS NULL) OR (claim.claim_id = @claim_Id))
 AND ((@TPAID IS NULL) OR (claim.other_party_id=@TPAID))
 ORDER BY claim_number
END

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
