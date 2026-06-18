SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_UnMark_TransMatch'
GO


CREATE PROCEDURE spu_ACT_UnMark_TransMatch
    @transdetail_id int
AS

DELETE FROM TransMatch
WHERE transdetail_id = @transdetail_id
AND   allocationdetail_id IS null
GO


