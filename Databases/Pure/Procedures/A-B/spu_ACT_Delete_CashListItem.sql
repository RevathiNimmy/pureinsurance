SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_CashListItem'
GO


CREATE PROCEDURE spu_ACT_Delete_CashListItem
    @cashlistitem_id int
AS

/*************************************************************************/
/* ERWIN generated delete record with key supplied                       */
/*************************************************************************/
/*************************************************************************/
/* 1.0 07/07/97 SP Original                                              */
/*************************************************************************/
DELETE FROM CashListItem
WHERE cashlistitem_id = @cashlistitem_id
GO


