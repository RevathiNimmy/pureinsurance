
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_select_associated_client_count'
GO


CREATE PROCEDURE spu_select_associated_client_count
    @party_cnt int
AS

    SELECT  COUNT(*)
    FROM    party_relationship 
    WHERE   party_cnt = @party_cnt

GO


