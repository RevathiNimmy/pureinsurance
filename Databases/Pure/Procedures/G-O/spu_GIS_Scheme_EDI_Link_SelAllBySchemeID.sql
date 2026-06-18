SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_GIS_Scheme_EDI_Link_SelAllBySchemeID'
GO

CREATE PROCEDURE spu_GIS_Scheme_EDI_Link_SelAllBySchemeID
    @gis_scheme_id int
AS

--********************************************************************************************************
--* Stored Procedure spu_GIS_Scheme_EDI_Link_SelAllBySchemeID returns all link records from the          *
--* GIS_Scheme_EDI_Link table for the specified scheme id indicating all of the external scheme          *
--* identifiers that link to this one GIS Scheme. Note that the NULL returned acts as a placeholder for  *
--* the db update status used in the system.  								 *						 
--********************************************************************************************************

SELECT
    gis_scheme_edi_link_id,
    gis_scheme_id,
    external_scheme_no,
    NULL
FROM GIS_Scheme_EDI_Link
WHERE gis_scheme_id = @gis_scheme_id
GO