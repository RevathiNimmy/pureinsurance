SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_CashListItem'
GO


CREATE PROCEDURE spu_ACT_Check_CashListItem
    @cashlistitem_id int OUTPUT
AS

/*************************************************************************/
/* ERWIN generated check for record exists - = Not exists                */
/*************************************************************************/
/*************************************************************************/
/* 1.0 07/07/97 SP Original                                              */
/*************************************************************************/
BEGIN
    SELECT @cashlistitem_id = cashlistitem_id
    FROM CashListItem
    WHERE cashlistitem_id = @cashlistitem_id
END
BEGIN
IF @cashlistitem_id = NULL
    SELECT @cashlistitem_id = -1
END
GO


