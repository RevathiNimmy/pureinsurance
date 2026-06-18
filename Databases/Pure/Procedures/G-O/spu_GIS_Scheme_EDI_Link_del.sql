SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_GIS_Scheme_EDI_Link_del'
GO

CREATE PROCEDURE spu_GIS_Scheme_EDI_Link_del
    @gis_scheme_edi_link_id int
AS

DELETE FROM GIS_Scheme_EDI_Link
WHERE gis_scheme_edi_link_id = @gis_scheme_edi_link_id 

GO