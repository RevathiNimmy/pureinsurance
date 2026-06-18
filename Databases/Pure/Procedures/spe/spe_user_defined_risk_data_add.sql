SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_user_defined_risk_data_add'
GO

CREATE PROCEDURE spe_user_defined_risk_data_add
    @insurance_file_cnt int,
    @defined_risk_data_id int,
    @instance int,
    @value varchar(255)
AS
BEGIN
INSERT INTO user_defined_risk_data (
    insurance_file_cnt ,
    defined_risk_data_id ,
    instance ,
    value )
VALUES (
    @insurance_file_cnt,
    @defined_risk_data_id,
    @instance,
    @value)
END

GO

