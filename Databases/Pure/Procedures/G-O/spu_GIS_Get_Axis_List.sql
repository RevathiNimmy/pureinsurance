EXECUTE DDLDropProcedure 'spu_GIS_Get_Axis_List'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_GIS_Get_Axis_List
    @SchemeID     int,
    @RateTypeDesc varchar(70)

AS

    SELECT gis_list_type_lookup1,
           gis_list_type_lookup2,
           gis_list_type_lookup3

      FROM gis_rate_type
     WHERE gis_scheme_id= @SchemeID
       AND Description = @RateTypeDesc

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

