SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Arc_Coi_Arrangement_upd'
GO

CREATE PROCEDURE spe_Arc_Coi_Arrangement_upd
    @insurance_file_cnt int,
    @is_recovered tinyint,
    @is_surcharged tinyint,
    @coi_default_id int
AS
BEGIN
UPDATE Arc_Coi_Arrangement
    SET
    is_recovered=@is_recovered,
    is_surcharged=@is_surcharged,
    coi_default_id=@coi_default_id
WHERE insurance_file_cnt = @insurance_file_cnt
END

GO

