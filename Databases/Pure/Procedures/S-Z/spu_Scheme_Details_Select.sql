SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Scheme_Details_Select'
GO


CREATE PROCEDURE spu_Scheme_Details_Select
    @Company_No Varchar(10),
    @Scheme_No int
AS


SELECT
 gs.gis_scheme_id,
 gs.scheme_desc
 FROM GIS_scheme gs
WHERE
 gs.scheme_no = @Scheme_No
 AND gs.qm_insurer_ref = @Company_No
GO


