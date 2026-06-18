SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Coi_Arrangement_add'
GO

CREATE PROCEDURE spe_Coi_Arrangement_add
    @insurance_file_cnt int,
    @is_recovered tinyint,
    @is_surcharged tinyint,
    @coi_default_id int
AS
BEGIN
INSERT INTO Coi_Arrangement (
    insurance_file_cnt ,
    is_recovered ,
    is_surcharged ,
    coi_default_id )
VALUES (
    @insurance_file_cnt,
    @is_recovered,
    @is_surcharged,
    @coi_default_id)
END

GO

