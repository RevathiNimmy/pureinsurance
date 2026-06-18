SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_claim_conviction_del'
GO


CREATE PROCEDURE spu_claim_conviction_del
    @ClaimConvictionID int
AS


DELETE FROM claim_conviction
WHERE claim_conviction_id = @ClaimConvictionID
GO


