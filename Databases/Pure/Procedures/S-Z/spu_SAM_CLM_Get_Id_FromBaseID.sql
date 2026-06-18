SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Get_Id_FromBaseID'
GO



  /*******************************************************************************************************/  
/* spu_CLM_Get_Id_FromBaseID                                                                              */  
/*                                                                                        */  
/* Selects id by using base Id and version id */  
/*******************************************************************************************************/  
CREATE   PROCEDURE spu_SAM_CLM_Get_Id_FromBaseID 
    @tablename varchar(255),  
 @baseid int,  
 @versionId int  
     
AS  
BEGIN  
  
    
Declare @sql varchar(2000)  
 SELECT @sql =        "SELECT MAX(tn." + @tablename + "_id) "  
      SELECT @sql = @sql + "       FROM " + @tablename + " tn "  
       SELECT @sql = @sql + "       WHERE tn."+"base_"+@tablename+"_id="+CONVERT(varchar(10), @baseid )   
  SELECT @sql = @sql + "            AND tn.version_id="+CONVERT(varchar(10), @versionid )     
  
  
      
     
   
   --PRINT (@sql)  
  
    EXECUTE (@SQL)  
  
END 

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
 