SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SIRRen_GetGISSchemeID'
GO


CREATE PROCEDURE spu_SIRRen_GetGISSchemeID
    @insurance_folder_cnt integer
AS

/* Fetch gis_scheme_id */
/* History : SSL 27/07/2001 - Created */
BEGIN
    SELECT gis_scheme_id FROM renewal_control
    WHERE insurance_folder_cnt = @insurance_folder_cnt
END
GO


