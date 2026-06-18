SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_user_defined_risk_data_del'
GO


CREATE PROCEDURE spu_user_defined_risk_data_del
    @insurance_file_cnt int
AS


DELETE FROM user_defined_risk_data
WHERE insurance_file_cnt = @insurance_file_cnt
GO


