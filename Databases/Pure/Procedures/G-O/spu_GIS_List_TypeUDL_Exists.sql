
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_GIS_List_TypeUDL_Exists'
GO

Create Procedure spu_GIS_List_TypeUDL_Exists
	@table varchar(255)	
AS

	Select * FROM GIS_list_type WHERE 'UDL_'+Code= @table


