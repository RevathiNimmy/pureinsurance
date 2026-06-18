SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_private_public_hire_del'
GO

CREATE PROCEDURE spe_private_public_hire_del
    @insurance_file_cnt int
AS
DELETE FROM private_public_hire
WHERE insurance_file_cnt = @insurance_file_cnt

GO

