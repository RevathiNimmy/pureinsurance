SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_user_defined_risk_data_upd'
GO

CREATE PROCEDURE spe_user_defined_risk_data_upd
    @insurance_file_cnt int,
    @defined_risk_data_id int,
    @instance int,
    @value varchar(255)
AS
BEGIN
UPDATE user_defined_risk_data
    SET
    value=@value
WHERE insurance_file_cnt = @insurance_file_cnt AND defined_risk_data_id = @defined_risk_data_id AND instance = @instance
END

GO

