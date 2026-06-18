SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.spu_GIS_List_Group_Items_Summary    Script Date: 27/06/2002 11:37:13 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spu_GIS_List_Group_Items_Summary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spu_GIS_List_Group_Items_Summary]
GO


CREATE PROCEDURE spu_GIS_List_Group_Items_Summary
			@gis_scheme_id int,
			@gis_list_type_id int
AS

BEGIN

	SELECT 	glg.[gis_list_grouping_id],
		glg.[code],
		glg.[description],
		glg.[is_deleted],
		(SELECT COUNT(*) 
		 FROM gis_list_grouping_items glgi
		 WHERE glgi.[gis_list_grouping_id] = glg.[gis_list_grouping_id]
		 AND glgi.[gis_scheme_id] = @gis_scheme_id) as 'items_cnt'
	FROM	gis_list_grouping glg
	WHERE	glg.[gis_scheme_id] = @gis_scheme_id
	AND	glg.[gis_list_type_id] = @gis_list_type_id

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

