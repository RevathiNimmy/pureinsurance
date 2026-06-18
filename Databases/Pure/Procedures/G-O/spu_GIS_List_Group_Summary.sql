SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.spu_GIS_List_Group_Summary    Script Date: 27/06/2002 11:37:13 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spu_GIS_List_Group_Summary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spu_GIS_List_Group_Summary]
GO


CREATE PROCEDURE spu_GIS_List_Group_Summary
			@gis_scheme_id	int
AS
BEGIN
	SELECT	glt.[gis_list_type_id],
		glt.[code],
		glt.[description],
		(SELECT COUNT(*)
			FROM gis_list_grouping glg
			WHERE glg.[gis_list_type_id] = glt.[gis_list_type_id]
			AND glg.[is_deleted] = 0
			AND glg.[gis_scheme_id] = @gis_scheme_id)  AS 'uses_cnt'
	FROM	gis_list_type glt
	WHERE	glt.[is_deleted] = 0

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

