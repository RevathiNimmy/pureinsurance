SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure spu_SAM_GetDetailsFromId
GO

--Start (Vivek Athalye) - (Tech Spec - UIIC WR22 - Capture Quote Details - Co-Insurance.doc) - (generic SP, not mentioned in tech spec) 
CREATE  procedure spu_SAM_GetDetailsFromId
 @table_name varchar(255),
 @id_field varchar(255),
 @id_value varchar(255)
as
Begin

 declare @stmt varchar(600)
 set @stmt = 'select * from ' + @table_name + ' where ' + @id_field + ' = ' + @id_value

 exec (@stmt)

End
--End (Vivek Athalye) - (Tech Spec - UIIC WR22 - Capture Quote Details - Co-Insurance.doc) - (generic SP, not mentioned in tech spec) 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

