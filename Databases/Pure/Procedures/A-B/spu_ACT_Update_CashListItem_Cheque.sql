SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_CashListItem_Cheque'
GO


CREATE PROCEDURE spu_ACT_Update_CashListItem_Cheque
    @transdetail_id int,
    @media_ref varchar(100)
AS


BEGIN
UPDATE CashListItem
SET media_ref =  @media_ref
WHERE transdetail_id=@transdetail_id
END
GO


