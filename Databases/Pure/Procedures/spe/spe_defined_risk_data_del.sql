SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_defined_risk_data_del'
GO

CREATE PROCEDURE spe_defined_risk_data_del
    @defined_risk_data_id int
AS
DELETE FROM defined_risk_data
WHERE defined_risk_data_id = @defined_risk_data_id

GO

