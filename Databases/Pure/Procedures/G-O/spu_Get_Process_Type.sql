SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Process_Type'
GO

CREATE PROCEDURE spu_Get_Process_Type  
    @FunctionalArea int = 0  
AS  
    SELECT Process_Type_ID,  Description, Code   
    FROM Process_type  
    WHERE  @FunctionalArea = 0 OR Functional_Area= @FunctionalArea  
  
