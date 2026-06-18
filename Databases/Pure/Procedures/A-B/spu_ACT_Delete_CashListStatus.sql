SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_CashListStatus'
GO


CREATE PROCEDURE spu_ACT_Delete_CashListStatus
    @cashliststatus_id int
AS


DELETE FROM CashListStatus
WHERE cashliststatus_id = @cashliststatus_id
GO


