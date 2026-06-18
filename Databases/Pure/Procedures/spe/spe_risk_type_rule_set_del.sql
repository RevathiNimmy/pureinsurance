SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_risk_type_rule_set_del'
GO

CREATE PROCEDURE spe_risk_type_rule_set_del
    @risk_type_rule_set_id int
AS

DELETE FROM risk_type_rule_set

WHERE risk_type_rule_set_id = @risk_type_rule_set_id

GO

