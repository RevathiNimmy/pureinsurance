SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_allowed_risk_values_del'
GO

CREATE PROCEDURE spe_allowed_risk_values_del
    @risk_group_id int
AS
DELETE FROM allowed_risk_values
WHERE   defined_risk_data_id IN (
    SELECT  defined_risk_data_id
    FROM    defined_risk_data
    WHERE   risk_group_id = @risk_group_id)

GO

