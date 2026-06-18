SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_party_coinsurer'
GO


CREATE PROCEDURE spu_get_party_coinsurer
    @ClaimID int
AS


BEGIN
     Select party_cnt,name from Party

    END
GO


