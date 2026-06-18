SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_GIS_Scheme_Audit_add'
GO

CREATE PROCEDURE spe_GIS_Scheme_Audit_add
    @gis_scheme_audit_cnt int OUTPUT,
    @gis_scheme_id int,
    @datetime_of_action datetime,
    @action int
AS
BEGIN
    INSERT INTO GIS_Scheme_Audit (
        gis_scheme_id,
        datetime_of_action,
        action)
    VALUES (
        @gis_scheme_id,
        @datetime_of_action,
        @action)

    SELECT @gis_scheme_audit_cnt = @@IDENTITY
END

GO

