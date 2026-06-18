SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_personal_accident_del'
GO

CREATE PROCEDURE spe_personal_accident_del
    @insurance_file_cnt int
AS
DELETE FROM personal_accident
WHERE insurance_file_cnt = @insurance_file_cnt

GO

