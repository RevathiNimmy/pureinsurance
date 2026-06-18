SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_user_defined_risk_data_del'
GO

CREATE PROCEDURE spe_user_defined_risk_data_del
    @insurance_file_cnt int,
    @defined_risk_data_id int,
    @instance int
AS
DELETE FROM user_defined_risk_data
WHERE insurance_file_cnt = @insurance_file_cnt AND defined_risk_data_id = @defined_risk_data_id AND instance = @instance

GO

