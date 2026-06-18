SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_user_defined_risk_data_sel'
GO

CREATE PROCEDURE spe_user_defined_risk_data_sel
    @insurance_file_cnt int,
    @defined_risk_data_id int,
    @instance int
AS
SELECT
    insurance_file_cnt,
    defined_risk_data_id,
    instance,
    value
 FROM user_defined_risk_data
WHERE insurance_file_cnt = @insurance_file_cnt AND defined_risk_data_id = @defined_risk_data_id AND instance = @instance

GO

