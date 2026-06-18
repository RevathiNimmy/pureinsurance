SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Coi_Arrangement_sel'
GO

CREATE PROCEDURE spe_Coi_Arrangement_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    is_recovered,
    is_surcharged,
    coi_default_id
 FROM Coi_Arrangement
WHERE insurance_file_cnt = @insurance_file_cnt

GO

