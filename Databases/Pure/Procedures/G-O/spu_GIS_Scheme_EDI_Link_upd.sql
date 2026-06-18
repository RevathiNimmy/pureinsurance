SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_GIS_Scheme_EDI_Link_upd'
GO

CREATE PROCEDURE spu_GIS_Scheme_EDI_Link_upd
    @gis_scheme_edi_link_id int,
    @external_scheme_no varchar(6)
AS
BEGIN

UPDATE GIS_Scheme_EDI_Link
SET    external_scheme_no=@external_scheme_no
WHERE  gis_scheme_edi_link_id = @gis_scheme_edi_link_id

END
GO