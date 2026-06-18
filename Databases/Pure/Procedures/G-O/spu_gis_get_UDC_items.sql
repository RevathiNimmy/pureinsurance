SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.spu_GIS_Get_UDC_items    Script Date: 28/02/2002 16:25:14 ******/
EXECUTE DDLDropProcedure 'spu_GIS_Get_UDC_items'
GO

CREATE procedure spu_GIS_Get_UDC_items

@ListType varchar(30)

as

--REMOVE Blanks
set @listtype=rtrim(@listtype)

Exec ('SELECT UDL_'  + @listtype + '_ID, Code from UDL_' + @listtype + '  WHERE Code like "UDC%"')
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

