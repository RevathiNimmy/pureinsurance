SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_clm_reset_reserves'
GO

CREATE PROCEDURE
spu_clm_reset_reserves
(
@ClaimId int
)
AS

UPDATE
reserve
SET
paid_to_date = ISNULL(initial_reserve,0)
WHERE
claim_peril_id IN
(
SELECT R.claim_peril_id
FROM
reserve R
INNER JOIN claim_peril CP ON CP.claim_peril_id=R.claim_peril_id
INNER JOIN reserve_type RT ON RT.reserve_type_id=R.reserve_type_id
WHERE
CP.claim_id=@ClaimId AND
RT.include_in_total=1 AND
(NOT (ISNULL(R.revised_reserve,0)<>0 OR ISNULL(R.revised_reserve_entered,0)<>0))
)

UPDATE
reserve
SET
paid_to_date = ISNULL(revised_reserve,0)
WHERE
claim_peril_id IN
(
SELECT R.claim_peril_id
FROM
reserve R
INNER JOIN claim_peril CP ON CP.claim_peril_id=R.claim_peril_id
INNER JOIN reserve_type RT ON RT.reserve_type_id=R.reserve_type_id
WHERE
CP.claim_id=@ClaimId AND
RT.include_in_total=1 AND
(ISNULL(R.revised_reserve,0)<>0 OR ISNULL(R.revised_reserve_entered,0)<>0)
)

UPDATE
recovery
SET
received_to_date = ISNULL(initial_reserve,0) + ISNULL(revised_reserve,0)
WHERE
claim_peril_id IN
(
SELECT
R.claim_peril_id
FROM
recovery R
INNER JOIN claim_peril CP ON CP.claim_peril_id=R.claim_peril_id
WHERE
CP.claim_id=@ClaimId
)

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

