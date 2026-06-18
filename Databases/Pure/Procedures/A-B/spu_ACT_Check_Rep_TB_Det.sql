SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_Rep_TB_Det'
GO


CREATE PROCEDURE spu_ACT_Check_Rep_TB_Det
    @rep_TB_det_id int OUTPUT
AS


BEGIN
    SELECT @rep_TB_det_id = rep_TB_det_id
    FROM Rep_TB_Det
    WHERE rep_TB_det_id = @rep_TB_det_id
END
BEGIN
IF @rep_TB_det_id = NULL
    SELECT @rep_TB_det_id = -1
END
GO


