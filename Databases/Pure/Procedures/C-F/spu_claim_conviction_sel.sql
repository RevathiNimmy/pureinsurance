SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_claim_conviction_sel'
GO


CREATE PROCEDURE spu_claim_conviction_sel
    @ClaimConvictionID int
AS


SELECT
 party_claim_id,
 claim_conviction_id,
 code,
 conviction_date,
 description,
 fine_amt,
 sentence_code,
 sentence_description,
 sentence_duration,
 sentence_duration_qualifier,
 sentence_effective_date,
 status_code,
 alcohol_level,
 alcohol_measurement_method,
 driving_licence_penalty_pts
FROM claim_conviction
WHERE claim_conviction_id = @ClaimConvictionID
GO


