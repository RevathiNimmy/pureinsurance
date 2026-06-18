EXECUTE DDLDropProcedure 'spu_Delete_Groups_For_Scheme'
GO


SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_Delete_Groups_For_Scheme
@scheme_id int,
@gis_list_type_id int
AS
BEGIN
 
 /*
 
 CTAF 070602 - The attempted syntax was below, but was unable
        to get it workign within the time frame.
        Attempt to fix when more time is available.
 
  --DELETE FROM gis_list_grouping_items
-- INNER JOIN GIS_List_Groupping glg
-- ON glg.gis_list_grouping_id = gis_list_grouping_items.gis_list_grouping_id
-- WHERE gis_list_grouping_items.gis_scheme_id = @scheme_id
-- AND glg.gis_list_type_id = @gis_list_type_id
 
 */
 
 DELETE FROM gis_list_grouping_items
 WHERE gis_scheme_id = @scheme_id 
 AND gis_list_grouping_id IN (
  SELECT gis_list_grouping_id 
  FROM gis_list_grouping
  WHERE gis_list_type_id = @gis_list_type_id 
  AND gis_scheme_id = @scheme_id )
 
 
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
