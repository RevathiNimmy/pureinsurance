SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Branch_Code'
GO


CREATE PROCEDURE spu_SAM_Get_Branch_Code  
  
@sourceId int,   
@code varchar(100)  OUTPUT
  
AS  
  
SELECT @code = code FROM source   
WHERE source_id=@sourceId

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
