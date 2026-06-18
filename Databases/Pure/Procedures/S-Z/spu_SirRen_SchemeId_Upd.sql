SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SirRen_SchemeId_Upd'
GO


CREATE PROCEDURE spu_SirRen_SchemeId_Upd
    @insurance_folder_cnt INT,
    @renewal_gis_scheme_id INT
AS

/* update renewal_gis_scheme_id on renewal_control table */
UPDATE renewal_control
SET renewal_gis_scheme_id = @renewal_gis_scheme_id
WHERE insurance_folder_cnt = @insurance_folder_cnt
GO


