SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Risk_by_source_del'
GO


CREATE PROCEDURE spu_Risk_by_source_del
    @risk_group_id int
AS


DELETE FROM risk_by_source
WHERE risk_group_id = @risk_group_id
GO


