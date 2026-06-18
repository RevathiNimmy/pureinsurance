SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_TransDetail_NotReported'
GO


CREATE PROCEDURE spu_ACT_Update_TransDetail_NotReported
    @transdetail_id int,
    @not_reported tinyint
AS

UPDATE TransDetail SET
    not_reported=@not_reported
WHERE transdetail_id = @transdetail_id

GO


