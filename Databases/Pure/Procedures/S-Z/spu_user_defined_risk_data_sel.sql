SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_user_defined_risk_data_sel'
GO


CREATE PROCEDURE spu_user_defined_risk_data_sel
    @insurance_file_cnt int
AS


SELECT
    defined_risk_data_id,
    instance,
    value
 FROM user_defined_risk_data
WHERE insurance_file_cnt = @insurance_file_cnt
GO


