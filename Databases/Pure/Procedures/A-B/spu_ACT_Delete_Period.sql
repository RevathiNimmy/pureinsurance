SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Period'
GO


CREATE PROCEDURE spu_ACT_Delete_Period
    @period_id int
AS


DELETE FROM Period
WHERE period_id = @period_id
GO


