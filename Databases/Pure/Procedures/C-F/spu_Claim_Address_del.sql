SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Claim_Address_del'
GO


CREATE PROCEDURE spu_Claim_Address_del
    @address_cnt int
AS


DELETE FROM Claim_Address
WHERE address_cnt = @address_cnt
GO


