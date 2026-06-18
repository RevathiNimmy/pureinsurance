EXECUTE DDLDropProcedure 'spu_Clearup_Groups'
GO


SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_Clearup_Groups

@Scheme_ID int

AS

Update gis_list_grouping set is_deleted = 1
WHERE gis_list_grouping_id NOT IN (SELECT gis_list_grouping_id FROM gis_list_grouping_items)
AND Gis_scheme_ID = @Scheme_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
