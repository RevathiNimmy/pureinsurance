SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Coinsurer_Details_Select'
GO

CREATE PROCEDURE spu_SAM_Coinsurer_Details_Select
@claim_id int
AS

BEGIN

    DECLARE @total_reserve money
    DECLARE @claim_number varchar(30)
    DECLARE @insurance_file_cnt int

    SELECT @insurance_file_cnt = policy_id
    FROM   Claim
    WHERE  claim_id = @claim_id

    SELECT @claim_number = claim_number
    FROM   Claim
    WHERE  claim_id = @claim_id

     -- Get total reserve for this claim
    SELECT  @total_reserve = SUM(ISNULL(r.initial_reserve,0) + ISNULL(r.revised_reserve,0) - ISNULL(r.paid_to_date,0))
    FROM    Reserve r
        INNER JOIN Claim_Peril cp ON
            cp.claim_peril_id = r.claim_peril_id        
    WHERE   cp.claim_id = @claim_id
    
    -- Non-retained parties (individual rows)
	SELECT  p.Party_cnt PartyKey,
        p.name Name,
        cv.share_percent Share,
        (ISNULL(@total_reserve, 0) / 100) * ISNULL(cv.share_percent, 0) ShareValue,
        ISNULL(@total_reserve, 0) TotalCurrentShareValue,
        @claim_number ClaimNumber
	FROM    Coi_Value cv
		INNER JOIN Party p ON cv.party_cnt = p.party_cnt
		LEFT JOIN Party_Insurer pin ON pin.party_cnt = p.party_cnt
	WHERE   cv.insurance_file_cnt = @insurance_file_cnt
    AND ISNULL(pin.is_retained, 0) = 0

	UNION ALL

		-- Retained parties (grouped into one row)
	SELECT  0 AS PartyKey,
        'Retained' AS Name,
        SUM(cv.share_percent) AS Share,
        (ISNULL(@total_reserve, 0) / 100) * SUM(ISNULL(cv.share_percent, 0)) AS ShareValue,
        ISNULL(@total_reserve, 0) AS TotalCurrentShareValue,
        @claim_number AS ClaimNumber
	FROM    Coi_Value cv
		INNER JOIN Party p ON cv.party_cnt = p.party_cnt
		LEFT JOIN Party_Insurer pin ON pin.party_cnt = p.party_cnt
	WHERE   cv.insurance_file_cnt = @insurance_file_cnt
		AND ISNULL(pin.is_retained, 0) = 1
	HAVING SUM(cv.share_percent) > 0


END
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
