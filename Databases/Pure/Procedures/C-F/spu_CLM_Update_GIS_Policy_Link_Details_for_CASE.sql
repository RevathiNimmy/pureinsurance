SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Update_GIS_Policy_Link_Details_for_CASE'
GO

CREATE PROCEDURE spu_CLM_Update_GIS_Policy_Link_Details_for_CASE  
 @gis_policy_link_id int,  
 @case_id   int = -1
AS  
BEGIN  
 IF (@case_id <> -1)  
 BEGIN  
  UPDATE  gis_policy_link  
         SET  case_id = @case_id  
      WHERE  gis_policy_link_id = @gis_policy_link_id  

 END  
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
