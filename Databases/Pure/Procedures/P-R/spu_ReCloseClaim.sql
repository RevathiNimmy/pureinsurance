SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ReCloseClaim'
GO


CREATE PROCEDURE spu_ReCloseClaim
    @ClaimID integer
AS


update Claim
    set Claim_Status_id = 5
    where Claim_id = @ClaimID
GO


