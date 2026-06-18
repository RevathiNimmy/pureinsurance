SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_private_motor_del'
GO

CREATE PROCEDURE spe_private_motor_del
    @insurance_file_cnt int
AS
DELETE FROM private_motor
WHERE insurance_file_cnt = @insurance_file_cnt

GO

