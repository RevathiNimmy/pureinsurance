SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Claim_Del'
GO


CREATE PROCEDURE spu_Claim_Del
    @Claim_id int
AS


DELETE FROM Claim
WHERE Claim_id = @Claim_id
GO


