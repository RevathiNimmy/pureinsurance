SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Rep_TB'
GO


CREATE PROCEDURE spu_ACT_Delete_Rep_TB
    @rep_TB_id int
AS


DELETE FROM Rep_TB
WHERE rep_TB_id = @rep_TB_id
GO


