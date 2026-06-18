SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ReOpenClaim'
GO


CREATE PROCEDURE spu_ReOpenClaim
    @ClaimID integer
AS


update Claim
    set Claim_Status_id = 4
    where Claim_id = @ClaimID
GO


