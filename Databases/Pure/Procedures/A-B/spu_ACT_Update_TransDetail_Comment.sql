SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_TransDetail_Comment'
GO


CREATE PROCEDURE spu_ACT_Update_TransDetail_Comment
    @transdetail_id int,
    @comment varchar(500)
AS

UPDATE TransDetail SET
    comment=@comment
WHERE transdetail_id = @transdetail_id

GO


