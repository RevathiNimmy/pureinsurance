SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Rep_TB_Det'
GO


CREATE PROCEDURE spu_ACT_Delete_Rep_TB_Det
    @rep_TB_det_id int
AS


DELETE FROM Rep_TB_Det
WHERE rep_TB_det_id = @rep_TB_det_id
GO


