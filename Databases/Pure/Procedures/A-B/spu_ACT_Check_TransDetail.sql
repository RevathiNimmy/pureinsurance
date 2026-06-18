SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_TransDetail'
GO


CREATE PROCEDURE spu_ACT_Check_TransDetail
    @transdetail_id int OUTPUT
AS


BEGIN
    SELECT @transdetail_id = transdetail_id
    FROM TransDetail
    WHERE transdetail_id = @transdetail_id
END
BEGIN
IF @transdetail_id = NULL
    SELECT @transdetail_id = -1
END
GO


