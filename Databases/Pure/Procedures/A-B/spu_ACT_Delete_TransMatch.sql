SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_TransMatch'
GO


CREATE PROCEDURE spu_ACT_Delete_TransMatch
    @transmatch_id int,
    @transdetail_id INT = NULL
AS

IF ISNULL(@transdetail_id,0) <> 0
   DELETE FROM TransMatch  
    WHERE transdetail_id = @transdetail_id
    AND allocationdetail_id IS null
ELSE
   DELETE FROM TransMatch
   WHERE transmatch_id = @transmatch_id


GO
