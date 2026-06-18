SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_Rep_TB'
GO


CREATE PROCEDURE spu_ACT_Check_Rep_TB
    @rep_TB_id int OUTPUT
AS


BEGIN
    SELECT @rep_TB_id = rep_TB_id
    FROM Rep_TB
    WHERE rep_TB_id = @rep_TB_id
END
BEGIN
IF @rep_TB_id = NULL
    SELECT @rep_TB_id = -1
END
GO


