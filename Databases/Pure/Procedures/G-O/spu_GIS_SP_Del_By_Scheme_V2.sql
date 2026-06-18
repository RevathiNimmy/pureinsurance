SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_SP_Del_By_Scheme_V2'
GO


CREATE PROCEDURE spu_GIS_SP_Del_By_Scheme_V2
    @qm_insurer_ref varchar(10),
    @scheme_no int,
    @class_of_business varchar(10)
AS


BEGIN
    DELETE FROM gis_scheme_property
    FROM gis_scheme s
    WHERE gis_scheme_property.gis_scheme_id = s.gis_scheme_id
    AND s.class_of_business = @class_of_business
    AND s.qm_insurer_ref = @qm_insurer_ref
    AND s.scheme_no = @scheme_no
END
GO


