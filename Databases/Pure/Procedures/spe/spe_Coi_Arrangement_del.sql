SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Coi_Arrangement_del'
GO

CREATE PROCEDURE spe_Coi_Arrangement_del
    @insurance_file_cnt int
AS
DELETE FROM Coi_Arrangement
WHERE insurance_file_cnt = @insurance_file_cnt

GO

