SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_GIS_Scheme_EDI_Link_add'
GO

CREATE PROCEDURE spu_GIS_Scheme_EDI_Link_add
    @gis_scheme_edi_link_id int OUTPUT,
    @gis_scheme_id int ,
    @external_scheme_no varchar(6)
AS
BEGIN
INSERT INTO GIS_Scheme_EDI_Link (
    gis_scheme_id,
    external_scheme_no)
VALUES (
    @gis_scheme_id,
    @external_scheme_no)

SELECT @gis_scheme_edi_link_id = @@IDENTITY
END
GO

