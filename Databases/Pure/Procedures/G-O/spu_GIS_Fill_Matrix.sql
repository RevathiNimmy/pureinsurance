SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_GIS_Fill_Matrix'
GO

/*Adds zeros to rate type item table where values don't exist*/
CREATE PROCEDURE spu_GIS_Fill_Matrix
    @schemeid INT,
    @ratetype VARCHAR(70)
AS

DECLARE @ratetypeid INT

/*Get rate type id*/
SELECT 
    @ratetypeid = gis_rate_type_id
FROM gis_rate_type
WHERE description = @ratetype
AND gis_scheme_id = @schemeid

/*Insert all of the zero rates for the matrix entries that don't already have values*/
INSERT gis_rate_items 
(
    gis_rate_type_id,
    rate,
    lookup1,
    lookup2,
    lookup3
)
SELECT 
    rt.gis_rate_type_id,
    0,
    g1.gis_list_grouping_id,
    g2.gis_list_grouping_id,
    g3.gis_list_grouping_id
FROM gis_rate_type rt
JOIN gis_list_grouping g1
    ON g1.gis_list_type_id = rt.gis_list_type_lookup1
    AND g1.gis_scheme_id = rt.gis_scheme_id
    AND g1.is_deleted = 0
LEFT JOIN gis_list_grouping g2
    ON g2.gis_list_type_id = rt.gis_list_type_lookup2
    AND g2.gis_scheme_id = rt.gis_scheme_id
    AND g2.is_deleted = 0
LEFT JOIN gis_list_grouping g3
    ON g3.gis_list_type_id = rt.gis_list_type_lookup3
    AND g3.gis_scheme_id = rt.gis_scheme_id
    AND g3.is_deleted = 0
WHERE rt.gis_rate_type_id = @ratetypeid
AND NOT EXISTS
    (
        SELECT 
            NULL
        FROM gis_rate_items
        WHERE gis_rate_type_id = rt.gis_rate_type_id
        AND ISNULL(lookup1,0) = ISNULL(g1.gis_list_grouping_id,0)
        AND ISNULL(lookup2,0) = ISNULL(g2.gis_list_grouping_id,0)
        AND ISNULL(lookup3,0) = ISNULL(g3.gis_list_grouping_id,0)
    )
ORDER BY g1.gis_list_grouping_id, g2.gis_list_grouping_id, g3.gis_list_grouping_id


GO

