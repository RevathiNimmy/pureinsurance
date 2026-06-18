EXECUTE DDLDropProcedure 'spu_GIS_Get_Rate_Type'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE spu_GIS_Get_Rate_Type
            @gis_scheme_id      int
AS
BEGIN

    SELECT  grt.[gis_rate_type_id],
        grt.[description],
        glt1.[code] as lookup1_code,
        glt1.[description] as lookup1_description,
        glt2.[code] as lookup1_code,
        glt2.[description] as lookup2_description,
        glt3.[code] as lookup1_code,
        glt3.[description] as lookup3_description
    FROM    gis_rate_type grt
    LEFT OUTER JOIN gis_list_type glt1
        ON   glt1.[gis_list_type_id] = grt.[gis_list_type_lookup1]
    LEFT OUTER JOIN gis_list_type glt2
        ON   glt2.[gis_list_type_id] = grt.[gis_list_type_lookup2]
    LEFT OUTER JOIN gis_list_type glt3
        ON   glt3.[gis_list_type_id] = grt.[gis_list_type_lookup3]
    WHERE   grt.[gis_scheme_id] = @gis_scheme_id
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
