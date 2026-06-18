SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_CashListType'
GO


CREATE PROCEDURE spu_ACT_Check_CashListType
    @cashlisttype_id int OUTPUT
AS


BEGIN
    SELECT @cashlisttype_id = cashlisttype_id
    FROM CashListType
    WHERE cashlisttype_id = @cashlisttype_id
END
BEGIN
IF @cashlisttype_id = NULL
    SELECT @cashlisttype_id = -1
END
GO


