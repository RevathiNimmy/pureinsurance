SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_CashListType'
GO


CREATE PROCEDURE spu_ACT_Delete_CashListType
    @cashlisttype_id int
AS


DELETE FROM CashListType
WHERE cashlisttype_id = @cashlisttype_id
GO


