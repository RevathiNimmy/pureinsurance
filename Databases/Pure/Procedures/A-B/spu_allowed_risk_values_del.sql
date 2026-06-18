SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_allowed_risk_values_del'
GO


CREATE PROCEDURE spu_allowed_risk_values_del
    @risk_group_id int,
    @source_id int
AS


DELETE FROM allowed_risk_values
WHERE   defined_risk_data_id IN (
    SELECT  defined_risk_data_id
    FROM    defined_risk_data
    WHERE   risk_group_id = @risk_group_id
    AND source_id = @source_id)
GO


