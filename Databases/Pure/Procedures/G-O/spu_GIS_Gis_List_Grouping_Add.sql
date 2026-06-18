SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.spu_GIS_Gis_List_Grouping_Add    Script Date: 27/06/2002 11:37:12 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spu_GIS_Gis_List_Grouping_Add]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spu_GIS_Gis_List_Grouping_Add]
GO


CREATE PROCEDURE spu_GIS_Gis_List_Grouping_Add
			@gis_scheme_id int,
			@gis_list_type_id int,
			@code char(20),
			@description varchar(255),
			@gis_list_grouping_id int OUTPUT
AS
BEGIN

	/* Add record */
	INSERT INTO gis_list_grouping
	( gis_scheme_id, gis_list_type_id, code, is_deleted, description )
	VALUES
	( @gis_scheme_id, @gis_list_type_id, @code, 0, @description)

	/* Get the new index */
	SELECT @gis_list_grouping_id = @@IDENTITY

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

