SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_QM_Scheme_ID_Lookup'
GO


CREATE PROCEDURE spu_GIS_QM_Scheme_ID_Lookup
    @gis_insurer_id int,
    @scheme_no int,
    @gis_business_type char(10)
AS

/* SP to return Scheme ID for a given Insurer ID and QM scheme no */
SELECT DISTINCT
    S.gis_scheme_id,
    S.scheme_desc

FROM
    GIS_Scheme  as S,
    GIS_Business_Type as B
WHERE
    S.gis_insurer_id = @gis_insurer_id
AND S.scheme_no = @scheme_no
AND B.code = @gis_business_type
AND B.gis_business_type_id = S.gis_business_type_id
GO


