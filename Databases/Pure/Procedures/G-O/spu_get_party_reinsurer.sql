SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_party_reinsurer'
GO


CREATE PROCEDURE spu_get_party_reinsurer
    @ClaimID int
AS


BEGIN
     Select party_cnt,name from Party
        where Party_cnt IN (Select party_cnt from party_insurer where is_reinsurer =1)

    END
GO


