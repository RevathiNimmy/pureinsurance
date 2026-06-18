EXECUTE DDLDropProcedure 'spu_GIS_Get_Rate_Type_List'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_GIS_Get_Rate_Type_List

AS

    SELECT code,
           description,
           gis_list_type_id
      FROM gis_list_type
      ORDER BY code

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
