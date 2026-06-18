SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Transaction_Comment_upd'
GO


CREATE PROCEDURE spu_ACT_Transaction_Comment_upd
    @cashlist_id int

AS

DECLARE @cashlistitem_id int

DECLARE cashlistitem_cursor CURSOR FOR
    SELECT cashlistitem_id 
    FROM   cashlistitem 
    WHERE cashlist_id = @cashlist_id
    
OPEN cashlistitem_cursor

FETCH NEXT FROM cashlistitem_cursor
INTO @cashlistitem_id

WHILE @@FETCH_STATUS = 0
BEGIN
    
    UPDATE transaction_comment
    SET    transdetail_id = (SELECT transdetail_id 
                             FROM   cashlistitem
                             WHERE  cashlistitem_id = @cashlistitem_id)
    FROM   transaction_comment,
           cashlistitem
    WHERE  transaction_comment.cashlistitem_id = cashlistitem.cashlistitem_id
    AND    cashlistitem.cashlistitem_id = @cashlistitem_id
    AND    cashlistitem.cashlist_id = @cashlist_id
    
    FETCH NEXT FROM cashlistitem_cursor
    INTO  @cashlistitem_id
END
  
CLOSE cashlistitem_cursor
DEALLOCATE cashlistitem_cursor

GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO





