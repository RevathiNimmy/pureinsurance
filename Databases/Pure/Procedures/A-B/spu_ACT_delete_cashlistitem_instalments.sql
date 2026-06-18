EXECUTE DDLDropProcedure 'spu_ACT_delete_cashlistitem_instalments'
GO


SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE spu_ACT_delete_cashlistitem_instalments
    @cashlistitem_id integer
AS
BEGIN
delete from cashlistitem_instalments where cashlistitem_ID = @cashlistitem_id
END
GO
