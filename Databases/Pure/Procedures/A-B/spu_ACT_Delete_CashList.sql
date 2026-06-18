SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_CashList'
GO


CREATE PROCEDURE spu_ACT_Delete_CashList
    @cashlist_id int
AS


DELETE FROM CashList
WHERE cashlist_id = @cashlist_id
GO


