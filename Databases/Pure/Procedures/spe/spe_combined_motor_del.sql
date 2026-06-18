SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_combined_motor_del'
GO

CREATE PROCEDURE spe_combined_motor_del
    @insurance_file_cnt int
AS
DELETE FROM combined_motor
WHERE insurance_file_cnt = @insurance_file_cnt

GO

