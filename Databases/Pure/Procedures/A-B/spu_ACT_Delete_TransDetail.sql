SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_TransDetail'
GO


CREATE PROCEDURE spu_ACT_Delete_TransDetail
    @transdetail_id int
AS


DELETE FROM TransDetail
WHERE transdetail_id = @transdetail_id
GO


