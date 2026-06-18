SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_allowed_risk_values_sel'
GO


CREATE PROCEDURE spu_allowed_risk_values_sel
    @risk_group_id int,
    @source_id int
AS


SELECT  arv.defined_risk_data_id,
    arv.record_number,
    arv.allowed_value_1,
    arv.allowed_value_2
FROM    defined_risk_data drd,
    allowed_risk_values arv
WHERE   drd.risk_group_id = @risk_group_id
AND drd.source_id = @source_id
AND arv.defined_risk_data_id = drd.defined_risk_data_id
ORDER BY defined_risk_data_id,record_number
GO


