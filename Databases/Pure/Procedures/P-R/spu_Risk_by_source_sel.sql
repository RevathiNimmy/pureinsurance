SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Risk_by_source_sel'
GO


CREATE PROCEDURE spu_Risk_by_source_sel
    @risk_group_id int
AS


SELECT  source_id,
    commission_cnt
FROM    risk_by_source
WHERE   risk_group_id = @risk_group_id
GO


