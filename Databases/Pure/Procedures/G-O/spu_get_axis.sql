/* JES200103 Added order by for matrix */

EXECUTE DDLDropProcedure 'spu_GIS_Get_Axis'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE     PROCEDURE spu_GIS_Get_Axis
    @SchemeID     int,
    @ListTypeId   int

AS

    SELECT Description,
           gis_list_grouping_id
      FROM gis_list_grouping
     WHERE gis_scheme_id=@SchemeID
       AND gis_list_type_id=@ListTypeId
       AND is_deleted=0
     ORDER BY gis_list_grouping_id
GO
